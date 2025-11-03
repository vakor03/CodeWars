using System.Reflection;

namespace CodeWars.Tests;

[TestFixture]
public class CalculatorTest
{
    public bool Close(double a, double b)
    {
        if (Math.Abs(a-b)<0.000000001) return true;
        return false;
    }
    
    [Test]
    public void PublicTests()
    {
        Assert.That(Close(Calculate("1 + 2"), 3), Is.True, "1 + 2");
        Assert.That(Close(Calculate("2*2"), 4), Is.True, "2*2");
    }
    
    private static double Calculate(string s)
    {
        Type kataType = typeof(Kata);
        MethodInfo methodInfo = GetMethod("Calculate");
        if (methodInfo == null) methodInfo = GetMethod("calculate");
        object result = methodInfo.Invoke(null, new object[] { s });
        return (double)result;
        MethodInfo GetMethod(string name) => kataType.GetMethod(name, BindingFlags.Public | BindingFlags.Static);
    }
}