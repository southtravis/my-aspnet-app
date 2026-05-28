namespace MyAspNetApp.Tests;

public class UnitTest1
{
    public void Test1_ThisWillFail()
    {
        int expected = 42;
        int actual = 99;
        Assert.Equal(expected, actual);
    }
}
