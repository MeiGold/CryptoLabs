using System;
using System.Collections.Generic;
using System.Text;

namespace crypto_lab1.Interfaces
{
    interface IChipherer
    {
        byte[] Encrypt(byte[] plaintext);

        byte[] Decrypt(byte[] chiphertext);
    }
}
