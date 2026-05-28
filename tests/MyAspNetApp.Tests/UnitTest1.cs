namespace MyAspNetApp.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1_ThisWillFail()
    {
        int expected = 42;
        int actual = 42;
        Assert.Equal(expected, actual);
    }
}
