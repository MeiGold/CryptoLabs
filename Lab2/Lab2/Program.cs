using Lab2.RS4;
using System;
using Lab2.Salsa20Library;
using System.Text;
using System.Linq;
using System.IO;
using Lab2.AESLibrary;
using System.Security.Cryptography;

namespace Lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = @"D:\Downloads\NCLauncher2_Installer.exe";
            #region RS4
            Console.WriteLine("\n\n----RS4");
            byte[] key = ASCIIEncoding.ASCII.GetBytes("1234567890123456");

            RC4 encoder = new RC4(key);
            //string testString = "Plaintext adssad sad sad dsadas sdad sadsasad";
            byte[] testBytes = File.ReadAllBytes(filePath);//ASCIIEncoding.ASCII.GetBytes(testString);
            byte[] result = encoder.Encode(testBytes, testBytes.Length);

            RC4 decoder = new RC4(key);
            byte[] decryptedBytes = decoder.Decode(result, result.Length);
            string decryptedString = ASCIIEncoding.ASCII.GetString(decryptedBytes);
            //Console.WriteLine(decryptedString);
            #endregion

            #region Salsa20
            Console.WriteLine("\n\n----Salsa20");
            Salsa20 s = new Salsa20();

            int nonce = 0;
            int[] msg = testBytes.Select(p => (int)p).ToArray();

            s.Encode(ref msg, key.Select(x => (int)x).ToArray(), nonce);
            Console.WriteLine();
            s.Encode(ref msg, key.Select(x => (int)x).ToArray(), nonce);
            //Console.WriteLine(ASCIIEncoding.ASCII.GetString(msg.Select(p=>(byte)p).ToArray()));
            #endregion

            #region AES_modes
            AES aes_algorithm = new AES();
            Console.WriteLine("\n\nAES:");
            Console.WriteLine("-----------CBC-----------");
            aes_algorithm.CBC(filePath);
            Console.WriteLine("-----------CFB-----------");
            aes_algorithm.CFB(filePath);
            Console.WriteLine("-----------OFB-----------");
            aes_algorithm.OFB(filePath);
            Console.WriteLine("-----------CTR-----------");
            aes_algorithm.CTR(filePath);
            Console.WriteLine("-----------ECB-----------");
            aes_algorithm.ECB(filePath);
            #endregion
        }

    }
}
