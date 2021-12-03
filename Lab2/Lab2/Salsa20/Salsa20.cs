using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab2.Salsa20Library
{
    public class Salsa20
    {
        public int R(int x, int n)
        {
            return ((x << n) | x >> (32 - n));
        }
        public void Quarter(ref int a, ref int b, ref int c, ref int d)
        {
            b ^= R(d + a, 7);
	        c ^= R(a + b, 9);
	        d ^= R(b + c, 13);
	        a ^= R(c + d, 18);
        }

        public void Salsa20_words(out int[] res, int[] input)
        {
            res = new int[16];
            int[,] x = new int[4, 4];
            int i = 0;
            for (i = 0; i < 16; ++i) x[i / 4, i % 4] = input[i];
            for (i = 0; i < 10; ++i)
            {
              // column round
                Quarter(ref x[0, 0], ref x[1, 0], ref x[2, 0], ref x[3, 0]);
                Quarter(ref x[1, 1], ref x[2, 1], ref x[3, 1], ref x[0, 1]);
                Quarter(ref x[2, 2], ref x[3, 2], ref x[0, 2], ref x[1, 2]);
                Quarter(ref x[3, 3], ref x[0, 3], ref x[1, 3], ref x[2, 3]);
                // row round
                Quarter(ref x[0, 0], ref x[0, 1], ref x[0, 2], ref x[0, 3]);
                Quarter(ref x[1, 1], ref x[1, 2], ref x[1, 3], ref x[1, 0]);
                Quarter(ref x[2, 2], ref x[2, 3], ref x[2, 0], ref x[2, 1]);
                Quarter(ref x[3, 3], ref x[3, 0], ref x[3, 1], ref x[3, 2]);
            }
            for (i = 0; i < 16; ++i) res[i] = x[i / 4, i % 4] + input[i];
        }
        public void Salsa20_block(out int[] resout, int[] key, int nonce, int index)
        {
            char[] c = "expand 32-byte k".ToCharArray();
            byte[] bkey = key.Select(p => (byte)p).ToArray();

            int[] res = new int[16];
            resout = new int[64];

            res[1] = ToUInt32(bkey, 0);
            res[2] = ToUInt32(bkey, 4);
            res[3] = ToUInt32(bkey, 8);
            res[4] = ToUInt32(bkey, 12);
            
            byte[] constants = Encoding.ASCII.GetBytes("expand 32-byte k");

            int keyIndex = key.Length - 16;

            res[11] = ToUInt32(bkey, keyIndex + 0);
            res[12] = ToUInt32(bkey, keyIndex + 4);
            res[13] = ToUInt32(bkey, keyIndex + 8);
            res[14] = ToUInt32(bkey, keyIndex + 12);

            res[0] = ToUInt32(constants, 0);
            res[5] = ToUInt32(constants, 4);
            res[10] = ToUInt32(constants, 8);
            res[15] = ToUInt32(constants, 12);

            res[6] = (int)(nonce & 0xffffffff);
            res[7] = nonce >> 32;
            res[8] = (int)(index & 0xffffffff);
            res[9] = index >> 32;

            int[] wordout = new int[16];
            Salsa20_words(out wordout, res);
            for (int i = 0; i < 64; ++i) resout[i] = 0xff & (wordout[i / 4] >> (8 * (i % 4)));
        }

        public void Encode(ref int[] message, int[] key, int nonce)
        {
            int i = 0;
            int[] block = new int[64];
            var watch = System.Diagnostics.Stopwatch.StartNew();
            for (i = 0; i < message.Length; i++)
            {
                if (i % 64 == 0) Salsa20_block(out block, key, nonce, i / 64);
                message[i] ^= block[i % 64];
            }

            watch.Stop();
            Console.WriteLine($"Time: {watch.Elapsed}");
        }

        private static int ToUInt32(byte[] input, int inputOffset)
        {
            unchecked
            {
                return (int)(((input[inputOffset] |
                                (input[inputOffset + 1] << 8)) |
                                (input[inputOffset + 2] << 16)) |
                                (input[inputOffset + 3] << 24));
            }
        }

       
    }
}
