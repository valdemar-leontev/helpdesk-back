using System.Security.Cryptography;
using System.Text;
using Helpdesk.WebApi.Config;
using Microsoft.AspNetCore.DataProtection;

namespace Helpdesk.WebApi.Services;

public sealed class DataProtectionService
{
    private readonly IDataProtectionProvider _dataProtectionProvider;

    private readonly ApplicationConfig _applicationConfig;

    public static async Task<string> GetHashStringAsync(string password)
    {
        return Convert.ToBase64String(await SHA256.HashDataAsync(
                new MemoryStream(Encoding.UTF8. GetBytes(password))
            )
        );
    }

    public DataProtectionService(IDataProtectionProvider dataProtectionProvider, IApplicationConfig applicationConfig)
    {
        _dataProtectionProvider = dataProtectionProvider;
        _applicationConfig = (ApplicationConfig)applicationConfig;
    }

    public string Encrypt(string plainText)
    {
        var protector = _dataProtectionProvider.CreateProtector(_applicationConfig.AppSecret!);

        return protector.Protect(plainText);
    }

    public string Decrypt(string cipherText)
    {
        var protector = _dataProtectionProvider.CreateProtector(_applicationConfig.AppSecret!);

        return protector.Unprotect(cipherText);
    }
}