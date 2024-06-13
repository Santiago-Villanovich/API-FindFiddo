using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FindFiddo.Services
{
    public static class EncryptService
    {
        public static string GenerarMD5(string Cadena)
        {
            try
            {
                UnicodeEncoding UeCod = new UnicodeEncoding();

                byte[] ByteSourceText = UeCod.GetBytes(Cadena);
                MD5CryptoServiceProvider Md5 = new MD5CryptoServiceProvider();
                byte[] ByteHash = Md5.ComputeHash(ByteSourceText);
                return Convert.ToBase64String(ByteHash);
            }
            catch (CryptographicException ex)
            { throw (ex); }
            catch (Exception ex)
            { throw (ex); }
        }

        public static string GenerarSHA(string sCadena)
        {
            UnicodeEncoding ueCodigo = new UnicodeEncoding();
            byte[] ByteSourceText = ueCodigo.GetBytes(sCadena);
            SHA1CryptoServiceProvider SHA = new SHA1CryptoServiceProvider();
            byte[] ByteHash = SHA.ComputeHash(ueCodigo.GetBytes(sCadena));
            return Convert.ToBase64String(ByteHash);
        }
        public static string Encriptar(string texto)
        {
            try
            {

                string key = "solutionkeysoft";
                byte[] keyArray;
                byte[] Arreglo_a_Cifrar = UTF8Encoding.UTF8.GetBytes(texto);


                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = tdes.CreateEncryptor();
                byte[] ArrayResultado = cTransform.TransformFinalBlock(Arreglo_a_Cifrar, 0, Arreglo_a_Cifrar.Length);
                tdes.Clear();

                texto = Convert.ToBase64String(ArrayResultado, 0, ArrayResultado.Length);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return texto;
        }

        public static string Desencriptar(string textoEncriptado)
        {
            try
            {
                string key = "solutionkeysoft";
                byte[] keyArray;
                byte[] Array_a_Descifrar = Convert.FromBase64String(textoEncriptado);

                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateDecryptor();
                byte[] ArrayResultado = cTransform.TransformFinalBlock(Array_a_Descifrar, 0, Array_a_Descifrar.Length);
                tdes.Clear();

                string textoDesencriptado = UTF8Encoding.UTF8.GetString(ArrayResultado);
                return textoDesencriptado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string Compute(string password, byte[] salt)
        {
            using (var sha256 = new SHA256Managed())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] saltedPassword = new byte[passwordBytes.Length + salt.Length];

                // Concatenate password and salt
                Buffer.BlockCopy(passwordBytes, 0, saltedPassword, 0, passwordBytes.Length);
                Buffer.BlockCopy(salt, 0, saltedPassword, passwordBytes.Length, salt.Length);

                // Hash the concatenated password and salt
                byte[] hashedBytes = sha256.ComputeHash(saltedPassword);

                // Concatenate the salt and hashed password for storage
                byte[] hashedPasswordWithSalt = new byte[hashedBytes.Length + salt.Length];
                Buffer.BlockCopy(salt, 0, hashedPasswordWithSalt, 0, salt.Length);
                Buffer.BlockCopy(hashedBytes, 0, hashedPasswordWithSalt, salt.Length, hashedBytes.Length);

                return Convert.ToBase64String(hashedPasswordWithSalt);
            }
        }

        public static byte[] GenerateSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[16];
                rng.GetBytes(salt);
                Console.WriteLine("Generated Salt: " + BitConverter.ToString(salt));
                return salt;
            }
        }

        public static bool VerifyPassword(string UserPassword, byte[] salt, string ActualPassword)
        {
            // In a real scenario, you would retrieve these values from your database


            // Convert the stored salt and entered password to byte arrays
            // byte[] storedSaltBytes = Convert.FromBase64String(user.Salt);
            byte[] enteredPasswordBytes = Encoding.UTF8.GetBytes(UserPassword);

            // Concatenate entered password and stored salt
            byte[] saltedPassword = new byte[enteredPasswordBytes.Length + salt.Length];
            Buffer.BlockCopy(enteredPasswordBytes, 0, saltedPassword, 0, enteredPasswordBytes.Length);
            Buffer.BlockCopy(salt, 0, saltedPassword, enteredPasswordBytes.Length, salt.Length);

            // Hash the concatenated value
            string enteredPasswordHash = Compute(UserPassword, salt);

            // Compare the entered password hash with the stored hash
            if (enteredPasswordHash == ActualPassword)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
