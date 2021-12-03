using System;
using System.Collections.Generic;
using System.Text;

namespace crypto_lab_3
{
    public interface IHashFunc
    {
        byte[] CalcHash(byte[] input);
    }
}
