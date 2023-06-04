using Xunit;

namespace graver.test;

public class GraverTest
{
    [Fact]
    public void Test1()
    {
        var process = new CompileProcess();
        Assert.True("hello, graver".Equals(process.SourceFIle));
    }
}