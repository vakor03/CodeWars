using System.Numerics;

namespace CodeWars;

public class Fibonacci {
    public static BigInteger fib(int n) {
        return CalculateFib(n);
    }

    private static BigInteger CalculateFib(int n) {
        switch (n) {
            case 0: return 0;
            case 1: return 1;
            case 2: return 1;
        }

        if (n<0) {
            return (int)Math.Pow(-1, n + 1) * CalculateFib(-n);
        }

        if (n % 2 == 0) {
            BigInteger fibK = CalculateFib(n / 2);
            BigInteger fibKPlusOne = CalculateFib(n / 2 + 1);
            return fibK * (2 * fibKPlusOne - fibK);
        }
        else {
            BigInteger fibK = CalculateFib((n - 1) / 2);
            BigInteger fibKPlusOne = CalculateFib((n - 1) / 2 + 1);
            return fibKPlusOne * fibKPlusOne + fibK * fibK;
        }
    }
}