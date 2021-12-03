using System;
using System.Numerics;
using System.Text;
using System.Diagnostics;
using crypto_lab_4;

namespace ConsoleForTests
{
    class Program
    {
        static string TestRSA(byte[] input, int keyLength)
        {
            Stopwatch st = new Stopwatch();

            RSA.keyLength = keyLength;

            (Key publicKey, Key privateKey) = RSA.GenerateRSAPair();

            st.Start();

            for (int i = 0; i < 10; ++i)
            {
                byte[] chipher = RSA.EncryptRSA(input, privateKey);

                byte[] plain = RSA.DecryptRSA(chipher, publicKey);
            }

            st.Stop();

            return $"Processed with key length={keyLength} for {st.ElapsedMilliseconds}ms";
        }

        static string TestRSAWithOAEP(byte[] input, int keyLength)
        {
            Stopwatch st = new Stopwatch();

            RSA.keyLength = keyLength;

            (Key publicKey, Key privateKey) = RSA.GenerateRSAPair();

            st.Start();

            for (int i = 0; i < 10; ++i)
            {
                byte[] oaeped_plain = OAEP.TransformOAEP(input, "SHA-256 MGF1", 10);
                byte[] chipher = RSA.EncryptRSA(oaeped_plain, privateKey);

                byte[] plain = OAEP.RestoreOAEP(RSA.DecryptRSA(chipher, publicKey), "SHA-256 MGF1");
            }

            st.Stop();

            return $"Processed with key length={keyLength} for {st.ElapsedMilliseconds}ms";
        }


        static void Main(string[] args)
        {
            byte[] inp = Encoding.ASCII.GetBytes("Some text should be here... But I didn't came up with it yet.");

            TestRSA(inp, 1024);

            Console.WriteLine("===================RSA encryption&decryption===================");
            Console.WriteLine(TestRSA(inp, 128));
            Console.WriteLine(TestRSA(inp, 256));
            Console.WriteLine(TestRSA(inp, 512));
            Console.WriteLine(TestRSA(inp, 1024));

            Console.WriteLine("===================RSA with OAEP encryption&decryption===================");
            Console.WriteLine(TestRSAWithOAEP(inp, 128));
            Console.WriteLine(TestRSAWithOAEP(inp, 256));
            Console.WriteLine(TestRSAWithOAEP(inp, 512));
            Console.WriteLine(TestRSAWithOAEP(inp, 1024));

            Console.ReadKey();
        }
    }
}
