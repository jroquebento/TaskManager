using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;
using TaskManager.Application.Interfaces;

namespace TaskManager.Infrastructure.Security;

public class Argon2PasswordHasher : IPasswordHasher
{
    private const int DegreeOfParallelism = 1;
    private const int Iterations = 2;
    private const int MemorySize = 20 * 1024;
    private const int SaltSize = 16;
    private const int HashSize = 32;


    public string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = HashPassword(password, salt);
        var combinedBytes = new byte[hash.Length + salt.Length];

        salt.CopyTo(combinedBytes);
        hash.CopyTo(combinedBytes, index: salt.Length);

        return Convert.ToBase64String(combinedBytes);
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        var combinedBytes = Convert.FromBase64String(passwordHash);

        var salt = new byte[SaltSize];
        var storedHash = new byte[HashSize];

        Array.Copy(combinedBytes, salt, SaltSize);
        Array.Copy(combinedBytes, SaltSize, storedHash, 0, HashSize);

        var newHash = HashPassword(password, salt);

        return CryptographicOperations.FixedTimeEquals(storedHash, newHash);
    }

    private byte[] HashPassword(string password, byte[] salt)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var hashAlgorithm = new Argon2id(passwordBytes)
        {
            DegreeOfParallelism = DegreeOfParallelism,
            Iterations = Iterations,
            MemorySize = MemorySize,
            Salt = salt
        };

        return hashAlgorithm.GetBytes(HashSize);
    }
}
