namespace CodeWars.Tests;

public class RailFenceTests {
    [Test, Order(1)]
    public void EncodeSampleTests()
    {
        string[][] encodes = 
        {
            new[] { "WEAREDISCOVEREDFLEEATONCE", "WECRLTEERDSOEEFEAOCAIVDEN" },  // 3 rails
            new[] { "Hello, World!", "Hoo!el,Wrdl l" },    // 3 rails
            new[] { "Hello, World!", "H !e,Wdloollr" },    // 4 rails
            new[] { "", "" }                               // 3 rails (even if...)
        };
        int[] rails = { 3, 3, 4, 3 };
        for (int i = 0; i < encodes.Length; i++)
        {
            Assert.That(RailFenceCipher.Encode(encodes[i][0], rails[i]), Is.EqualTo(encodes[i][1]));
        }
    }

    [Test, Order(2)]
    public void DecodeSampleTests()
    {
        string[][] decodes =
        {
            new[] { "WECRLTEERDSOEEFEAOCAIVDEN", "WEAREDISCOVEREDFLEEATONCE" },    // 3 rails
            new[] { "H !e,Wdloollr", "Hello, World!" },    // 4 rails
            new[] { "", "" }                               // 3 rails (even if...)
        };
        int[] rails = { 3, 4, 3 };
        for (int i = 0; i < decodes.Length; i++)
        {
            Assert.That(RailFenceCipher.Decode(decodes[i][0], rails[i]), Is.EqualTo(decodes[i][1]));
        }
    }
}