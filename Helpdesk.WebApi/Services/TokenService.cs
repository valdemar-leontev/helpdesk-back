using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Admin;
using Helpdesk.Domain.Models.Dictionaries;
using Helpdesk.WebApi.Config;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ClaimTypes = Helpdesk.WebApi.Models.ClaimTypes;

namespace Helpdesk.WebApi.Services;

public sealed class TokenService
{
    private readonly AppDatabaseContext _appDatabaseContext;

    private readonly ApplicationConfig _applicationConfig;

    public TokenService(IApplicationConfig applicationConfig, AppDatabaseContext appDatabaseContext)
    {
        _applicationConfig = (ApplicationConfig)applicationConfig;
        _appDatabaseContext = appDatabaseContext;
    }

    private string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var now = DateTime.UtcNow;

        var jsonWebToken = new JwtSecurityToken
        (
            _applicationConfig.AuthenticationConfig!.Issuer,
            _applicationConfig.AuthenticationConfig.Audience,
            claims,
            now,
            now.AddMilliseconds(_applicationConfig.AuthenticationConfig.LifeTime),
            new SigningCredentials
            (
                _applicationConfig.AuthenticationConfig.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256
            )
        );

        var encodedJsonWebToken = new JwtSecurityTokenHandler().WriteToken(jsonWebToken);

        return encodedJsonWebToken;
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var randomGenerator = RandomNumberGenerator.Create();
        randomGenerator.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private ClaimsPrincipal? GetPrincipalFromExpiredToken(AuthUserModel authUser)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = _applicationConfig.AuthenticationConfig!.GetSymmetricSecurityKey(),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(authUser.Token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }

    public async Task<AuthUserModel?> CreateAuthUserAsync(UserDataModel userEntity)
    {
        if (userEntity.Role is null)
        {
            await _appDatabaseContext
                .Set<RoleDataModel>()
                .LoadAsync();
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.UserId, userEntity.Id.ToString()),
            new(ClaimTypes.Email, userEntity.Email),
            new(ClaimTypes.RoleId, userEntity.RoleId.ToString()),
            new(System.Security.Claims.ClaimTypes.Role, userEntity.Role!.Code)
        };

        if (userEntity.Profile is not null)
        {
            claims.Add(new Claim(ClaimTypes.ProfileId, userEntity.Profile.Id.ToString()));
        }

        var refreshToken = GenerateRefreshToken();

        var userSessionEntity = await _appDatabaseContext
            .Set<UserSessionDataModel>()
            .AddAsync(new UserSessionDataModel
            {
                LoginDate = DateTimeOffset.UtcNow,
                RefreshToken = refreshToken,
                UserId = userEntity.Id
            });

        var affectedEntitiesCount = await _appDatabaseContext.SaveChangesAsync();

        return affectedEntitiesCount > default(int) && userSessionEntity.Entity.Id != default
            ? new AuthUserModel
            {
                RoleId = userEntity.RoleId,
                UserId = userEntity.Id,
                Email = userEntity.Email,
                ProfileId = userEntity.Profile?.Id,
                Token = GenerateAccessToken(claims),
                RefreshToken = refreshToken
            }
            : null;
    }

    public async Task<AuthUserModel?> RefreshAuthUserAsync(AuthUserModel authUser)
    {
        var entity = await _appDatabaseContext
            .Set<UserSessionDataModel>()
            .FirstOrDefaultAsync(userSession =>
                userSession.UserId == authUser.UserId &&
                userSession.RefreshToken == authUser.RefreshToken
            );

        if (entity is null || authUser.RefreshToken != entity.RefreshToken)
        {
            return null;
        }

        var newToken = null as string;
        var principal = GetPrincipalFromExpiredToken(authUser);

        if (principal is not null)
        {
            newToken = GenerateAccessToken(principal.Claims);
            var newRefreshToken = GenerateRefreshToken();

            entity.RefreshToken = newRefreshToken;
            _appDatabaseContext.Set<UserSessionDataModel>().Update(entity);
            var affectedEntitiesCount = await _appDatabaseContext.SaveChangesAsync();

            return affectedEntitiesCount > default(int)
                ? new AuthUserModel
                {
                    RoleId = authUser.RoleId,
                    UserId = authUser.UserId,
                    ProfileId = authUser.ProfileId,
                    Email = authUser.Email,
                    Token = newToken,
                    RefreshToken = newRefreshToken
                }
                : null;
        }

        return null;
    }
}