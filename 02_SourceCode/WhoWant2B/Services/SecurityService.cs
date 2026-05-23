using System.Security.Cryptography;
using System.Text;
using System.Collections;
using WhoWant2B.Core.Interfaces;

namespace WhoWant2B.Services
{
    /// <summary>
    /// Implementación del servicio de seguridad encargado del procesamiento de criptografía, 
    /// específicamente generación y verificación de hashes para contraseñas.
    /// </summary>
    public class SecurityService : ISecurityService
    {
        /// <summary>
        /// Genera un hash criptográfico SHA512 a partir de una cadena de texto.
        /// </summary>
        /// <param name="input">La cadena de texto (contraseña) que se desea procesar.</param>
        /// <returns>Un arreglo de bytes que contiene el hash generado. Retorna un arreglo vacío si la entrada es nula.</returns>
        public byte[] GetHash(string input)
        {
            if (input == null) return System.Array.Empty<byte>();

            using (var sha512 = SHA512.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                return sha512.ComputeHash(inputBytes);
            }
        }

        /// <summary>
        /// Compara un texto en plano contra un hash almacenado utilizando comparación estructural.
        /// </summary>
        /// <param name="input">Texto plano a verificar.</param>
        /// <param name="storedHash">El hash previamente guardado contra el cual se comparará.</param>
        /// <returns>True si el hash del input coincide con el hash almacenado; de lo contrario, False.</returns>
        public bool VerifyHash(string input, byte[] storedHash)
        {
            if (input == null || storedHash == null) return false;

            byte[] computedHash = GetHash(input);
            return StructuralComparisons.StructuralEqualityComparer.Equals(computedHash, storedHash);
        }
    }
}
