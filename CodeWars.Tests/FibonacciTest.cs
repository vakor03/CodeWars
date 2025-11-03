using System.Numerics;
using CodeWars;

public class FibonacciTest
{
    [Test]
    [Order(1)]
    public void testFib0()
    {
        testFib(0, 0);
    }

    [Test]
    [Order(2)]
    public void testFib1()
    {
        testFib(1, 1);
    }

    [Test]
    [Order(3)]
    public void testFib2()
    {
        testFib(1, 2);
    }

    [Test]
    [Order(4)]
    public void testFib3()
    {
        testFib(2, 3);
    }

    [Test]
    [Order(5)]
    public void testFib4()
    {
        testFib(3, 4);
    }

    [Test]
    [Order(6)]
    public void testFib5()
    {
        testFib(5, 5);
    }

    private static void testFib(long expected, int input)
    {
        BigInteger found = Fibonacci.fib(input);
        Assert.That(found, Is.EqualTo(new BigInteger(expected)));
    }
}