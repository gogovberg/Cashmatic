using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CashmaticApp
{
    public static class Helper
    {
        private static string _key = "UkFPi7g5ApTFvcwGd6yp3Th7VjViV3WB";
        private static string _iv = "8fWnMRmK4ic7qHaM";
        private static string _base_path = @"C:\Cashmatic\";

        /// <summary>
        /// The creation of the file “subtotale.txt” starts a new transaction.
        /// </summary>
        /// <param name="amount"></param>
        public static void WriteSubtotale(int amount, bool IsNegative=false)
        {
            try
            {
               if(amount<=999999999)
                {
                    string correct_amount = amount.ToString("000000000");
                    string file_name = "subtotale.txt";
                    string path_to_write = _base_path + file_name;
                    string encrypted_content = encrypt(correct_amount);
                }
                
            }
            catch (Exception ex)
            {

            }
        }


        private static string encrypt(string text)
        {
            byte[] plaintextbytes = System.Text.ASCIIEncoding.ASCII.GetBytes(text);
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.Key = System.Text.ASCIIEncoding.ASCII.GetBytes(_key);
            aes.IV = System.Text.ASCIIEncoding.ASCII.GetBytes(_iv);
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;
            ICryptoTransform crypto = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] encrypted = crypto.TransformFinalBlock(plaintextbytes, 0, plaintextbytes.Length);
            crypto.Dispose();
            return Convert.ToBase64String(encrypted);
        }

        private static string decrypt(string text)
        {
            byte[] encryptedbytes = Convert.FromBase64String(text);
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.Key = System.Text.ASCIIEncoding.ASCII.GetBytes(_key);
            aes.IV = System.Text.ASCIIEncoding.ASCII.GetBytes(_iv);
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;
            ICryptoTransform decrypto = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] decrypted = decrypto.TransformFinalBlock(encryptedbytes, 0, encryptedbytes.Length);
            decrypto.Dispose();
            return System.Text.ASCIIEncoding.ASCII.GetString(decrypted);
        }
    }
}
