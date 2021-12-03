using crypto_lab_3;
using crypto_lab_3.Kupyna;
using System;
using System.Diagnostics;
using System.Text;

namespace ConsoleForTests
{
    class Program
    {
        static Random random = new Random();

        static void ProofOfWork(IHashFunc hashFunc ,string initStr, string matched)
        {
            string hash = "";
            ulong iterationsCounter = 0;

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            while (!hash.StartsWith(matched))
            {
                iterationsCounter++;

                hash = Encoding.ASCII.GetString(hashFunc.CalcHash(Encoding.ASCII.GetBytes(initStr + iterationsCounter.ToString())));
            }

            stopwatch.Stop();

            Console.WriteLine($"Found in {iterationsCounter} iterations \n Time spent {stopwatch.ElapsedMilliseconds} ms");
        }

        static void Main(string[] args)
        {

            IHashFunc kupyna = new Kupyna();
            IHashFunc sha256Func = new SHA256();

            Console.WriteLine("SHA256");
            ProofOfWork(sha256Func, "I like crypto", "777");
            Console.WriteLine();

            Console.WriteLine("Kupyna");
            ProofOfWork(kupyna, "I like crypto", "777");
            Console.WriteLine();
        }
    }
}
