using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Servidor
{
    static class Common
    {
        // Baseado em
        // https://stackoverflow.com/questions/32932679/using-rngcryptoserviceprovider-to-generate-random-string#32932758
        static public string GenerateSalt()
        {
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            int saltSize = 15;
            StringBuilder salt = new StringBuilder();

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] buffer = new byte[15];

                for (int i = 0; i < saltSize; i++)
                {
                    rng.GetBytes(buffer);
                    salt.Append(chars[(int)(BitConverter.ToUInt32(buffer, 0) % (uint)chars.Length)]);
                }
            }

            return salt.ToString();
        }

        static public string HashPassword(string password, string Salt)
        {
            string hashedPassword;
            using (SHA512 sha = SHA512.Create())
            {
                // Converte os dados para byte
                byte[] passwordByte = Encoding.UTF8.GetBytes(password + Salt);

                // Cria o hash
                byte[] hashBytes = sha.ComputeHash(passwordByte);

                // Guarda na base de dados
                hashedPassword = Convert.ToBase64String(hashBytes);
            }

            return hashedPassword;
        }

        /// <summary>
        /// Gera a hash para os dados fornecidos
        /// </summary>
        /// <param name="Dados"></param>
        /// <returns></returns>
        public static string GeraHash(string Dados)
        {
            string hash;
            using (SHA512 sha = SHA512.Create())
            {
                // Converte os dados para byte
                byte[] data = Encoding.UTF8.GetBytes(Dados);

                // Cria o hash
                byte[] hashBytes = sha.ComputeHash(data);

                // Converte o hash em hexadecimal
                hash = Convert.ToBase64String(hashBytes);
            }
            return hash;
        }

        /// <summary>
        /// Verifica se os dados fornecidos produzem a mesma hash
        /// </summary>
        /// <param name="Dados"></param>
        /// <param name="HashDados"></param>
        /// <returns></returns>
        public static bool ValidacaoDados(string Dados, string HashDados)
        {
            
            if (GeraHash(Dados) != HashDados)
            {
                return false;
            }
            return true;
        }
    }
}
