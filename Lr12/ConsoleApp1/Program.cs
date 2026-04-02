using System;
using System.Numerics;

namespace LabECP
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Лабораторная работа: Формирование и проверка ЭЦП ===\n");

            int p = 43;
            int q = 59;  
            Console.WriteLine($"1. Простые числа: p = {p}, q = {q}");

            int n = p * q;
            Console.WriteLine($"2. Модуль n = p * q = {n}");

            int phi = (p - 1) * (q - 1);
            Console.WriteLine($"3. Функция Эйлера φ(n) = (p-1)*(q-1) = {phi}");

            int KA = 29; 
       
            while (GCD(KA, phi) != 1)
            {
                KA++;
            }
            Console.WriteLine($"4. Открытый ключ KA = {KA}");

            int kA = ModInverse(KA, phi);
            Console.WriteLine($"5. Закрытый ключ kA = {kA}");
            Console.WriteLine($"   Проверка: (KA * kA) mod φ(n) = {(KA * (long)kA) % phi}\n");

            var messages = new (int index, BigInteger h)[]
            {
    (110, BigInteger.Parse("156000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000")),
    (111, BigInteger.Parse("252000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000")),
    (112, BigInteger.Parse("406000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000")),
    (113, BigInteger.Parse("650000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000")),
    (114, BigInteger.Parse("104000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000"))
            };


            Console.WriteLine("=== Результаты формирования и проверки ЭЦП ===\n");
            Console.WriteLine("№ п/п |    h (хэш)    |      Подпись sign      |     h' (проверка)    | Результат");
            Console.WriteLine(new string('-', 85));

            for (int i = 0; i < messages.Length; i++)
            {
                var msg = messages[i];
                BigInteger h = msg.h;
                int index = msg.index;

           BigInteger sign = BigInteger.ModPow(h, KA, n);
                BigInteger hPrime = BigInteger.ModPow(sign, kA, n);

                bool isValid = (h % n) == hPrime;
                string result = isValid ? "ЭЦП верна" : "ЭЦП не верна";

                Console.WriteLine($"{index,-6} | {h.ToString().Substring(0, Math.Min(12, h.ToString().Length))}... | {sign,-19} | {hPrime,-19} | {result}");
            }

            Console.WriteLine("\n=== Дополнительная проверка с реальными хэшами (учебный пример) ===");
            Console.WriteLine("Для демонстрации работы алгоритма используем небольшие значения хэшей:\n");


            var demoMessages = new (int index, BigInteger h)[]
            {
                (1, 110),
                (2, 111),
                (3, 112),
                (4, 113),
                (5, 114)
            };

            Console.WriteLine($"Используем: p={p}, q={q}, n={n}, φ(n)={phi}, KA={KA}, kA={kA}\n");
            Console.WriteLine("№ п/п | h (хэш) | Подпись sign | h' (проверка) | Результат");
            Console.WriteLine(new string('-', 60));

            foreach (var msg in demoMessages)
            {
                BigInteger h = msg.h;

                BigInteger sign = BigInteger.ModPow(h, KA, n);

                BigInteger hPrime = BigInteger.ModPow(sign, kA, n);

                bool isValid = (h % n) == hPrime;
                string result = isValid ? "ЭЦП верна" : "ЭЦП не верна";

                Console.WriteLine($"{msg.index,-6} | {h,-8} | {sign,-12} | {hPrime,-12} | {result}");
            }

            Console.WriteLine("\nНажмите любую клавишу для завершения...");
            Console.ReadKey();
        }

        static int GCD(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        static int ModInverse(int a, int m)
        {
            a = a % m;
            for (int x = 1; x < m; x++)
            {
                if ((a * (long)x) % m == 1)
                    return x;
            }
            return 1;
        }
    }
}