using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace FourierTransform
{
    class Fourier
    {
        //length A is 2^i
        public static Complex[] fft(Complex[] A)
        {
            int N = A.Length;
            if(N == 2)
            {
                Complex[] res = new Complex[2];
                res[0] = A[0] + A[1];
                res[1] = A[0] - A[1];
            }
            Complex[] even = new Complex[N / 2];
            Complex[] odd = new Complex[N / 2];

            for(int i = 0; i < N/2; i++)
            {
                even[i] = A[i * 2];
                odd[i] = A[i * 2 + 1];
            }

            even = fft(even);
            odd = fft(odd);
            return null;
        }
    }
}