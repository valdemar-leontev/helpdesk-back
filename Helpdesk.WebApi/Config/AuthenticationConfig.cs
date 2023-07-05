using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Helpdesk.WebApi.Config;

public sealed class AuthenticationConfig
{
    public required string Issuer { get; set; }

    public int LifeTime { get; set; }

    public required string Audience { get; set; }

    public required string Key { get; set; }

    public SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
    }
}