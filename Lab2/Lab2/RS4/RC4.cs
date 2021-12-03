using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab2.RS4
{
    static class SwapExt
    {
        public static void Swap<T>(this T[] array, int index1, int index2)
        {
            T temp = array[index1];
            array[index1] = array[index2];
            array[index2] = temp;
        }
    }
    public class RC4
    {
        byte[] S = new byte[256];
        int x = 0;
        int y = 0;

        private void init(byte[] key)
        {
            int keyLength = key.Length;

            for (int i = 0; i < 256; i++)
            {
                S[i] = (byte)i;
            }

            int j = 0;
            for (int i = 0; i < 256; i++)
            {
                j = (j + S[i] + key[i % keyLength]) % 256;
                S.Swap(i, j);
            }
        }
        public RC4(byte[] key)
        {
            init(key);
        }
        private byte keyItem()
        {
            x = (x + 1) % 256;
            y = (y + S[x]) % 256;

            S.Swap(x, y);

            return S[(S[x] + S[y]) % 256];
        }
        public byte[] Encode(byte[] dataB, int size)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            byte[] data = dataB.Take(size).ToArray();

            byte[] cipher = new byte[data.Length];

            for (int m = 0; m < data.Length; m++)
            {
                cipher[m] = (byte)(data[m] ^ keyItem());
            }
            watch.Stop();
            Console.WriteLine($"Time: {watch.Elapsed}");

            return cipher;
        }
        public byte[] Decode(byte[] dataB, int size)
        {
            return Encode(dataB, size);
        }
    }
}
