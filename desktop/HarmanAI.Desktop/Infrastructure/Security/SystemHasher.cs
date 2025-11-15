using System;
using System.Security.Cryptography;
using System.Text;

namespace HarmanAI.Desktop.Infrastructure.Security;

public class SystemHasher
{
    public string ComputeHardwareId()
    {
        using var hasher = SHA256.Create();
        var machine = Environment.MachineName + Environment.UserName;
        var hash = hasher.ComputeHash(Encoding.UTF8.GetBytes(machine));
        return Convert.ToHexString(hash);
    }
}
