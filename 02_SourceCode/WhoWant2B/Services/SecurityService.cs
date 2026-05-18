using System.Security.Cryptography;
using System.Text;
using System.Collections;

namespace WhoWant2B.Services
{
    public class SecurityService : ISecurityService
    {
        public byte[] GetHash(string input)
        {
            if (input == null) return System.Array.Empty<byte>();

            using (var sha512 = SHA512.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                return sha512.ComputeHash(inputBytes);
            }
        }

        public bool VerifyHash(string input, byte[] storedHash)
        {
            if (input == null || storedHash == null) return false;

            byte[] computedHash = GetHash(input);
            return StructuralComparisons.StructuralEqualityComparer.Equals(computedHash, storedHash);
        }
    }
}

