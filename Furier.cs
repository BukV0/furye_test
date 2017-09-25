using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace FourierTransform
{
    class Furier
    {
        public Complex[] translate(Complex[] tra)
        {
            int N;
            N = tra.Length;
            for (int i = 0; i < N / 2; i++)
            {
                tra[i] = tra[(int)(0.7 * i)];
            }
            return tra;
        }
        public Complex[] DPF(Complex[] enter)
        {

            int N;
            N = enter.Length;
            if (N == 2)
            {
                Complex[] X = new Complex[2];
                X[0] = enter[0] + enter[1];
                X[1] = enter[0] - enter[1];
                return X;
            }
            Complex[] even = new Complex[N / 2];
            Complex[] odd = new Complex[N / 2];
            for (int i = 0; i < N - 1; i += 2)
            {
                odd[i / 2] = enter[i];
                even[i / 2] = enter[i + 1];
            }
            even = DPF(even);
            odd = DPF(odd);
            for (int i = 0; i < N / 2; i++)
            {
                enter[i] = even[i] + Complex.Exp(-2 * Math.PI * Complex.ImaginaryOne * i / N) * odd[i];
                enter[i + N / 2] = even[i] - Complex.Exp(-2 * Math.PI * Complex.ImaginaryOne * i / N) * odd[i];
            }
            return enter;
        }
    }
}
