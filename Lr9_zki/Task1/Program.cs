using System;
using System.Diagnostics;
using System.Numerics;
using System.Collections.Generic;

class ElGamalTiming
{
    static void Main()
    {
        BigInteger n1024 = BigInteger.Pow(2, 1024) - 1; 
        BigInteger n2048 = BigInteger.Pow(2, 2048) - 1;
        BigInteger[] nValues = { n1024, n2048 };

        int[] aValues = { 7, 31 };

        List<BigInteger> xValues = new List<BigInteger>();
        for (int i = 3; i <= 100; i += 20) 
        {
            xValues.Add(BigInteger.Parse("1" + new string('0', i)));
        }

        Console.WriteLine($"{"a",-5} | {"x (log10)",-10} | {"n (bits)",-10} | {"Time (ms)",-10}");
        Console.WriteLine(new string('-', 45));

        Stopwatch sw = new Stopwatch();

        foreach (var n in nValues)
        {
            int nBits = n == n1024 ? 1024 : 2048;
            foreach (var a in aValues)
            {
                foreach (var x in xValues)
                {
                    sw.Restart();
                    BigInteger y = BigInteger.ModPow(a, x, n);
                    sw.Stop();

                    Console.WriteLine($"{a,-5} | 10^{x.ToString().Length - 1,-7} | {nBits,-10} | {sw.Elapsed.TotalMilliseconds:F4}");
                }
                Console.WriteLine(new string('-', 45));
            }
        }
    }
}
