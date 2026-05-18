using System;

namespace WhoWant2B.Services
{
    /// <summary>
    /// Define las operaciones necesarias para gestionar la seguridad y el hashing de datos.
    /// </summary>
    public interface ISecurityService
    {
        /// <summary>
        /// Genera un hash SHA-512 a partir de una cadena de texto.
        /// </summary>
        byte[] GetHash(string input);

        /// <summary>
        /// Verifica si una cadena de texto coincide con un hash almacenado.
        /// </summary>
        bool VerifyHash(string input, byte[] storedHash);
    }
}

