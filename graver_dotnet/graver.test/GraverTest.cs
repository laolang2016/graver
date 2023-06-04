using System;
using Xunit;

namespace graver.test;

public class GraverTest 
{
    private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

    [Fact]
    public void Test1()
    {
        bool flag = true;
        try
        {
            var compiler = new Compiler();
            compiler.compileFile("/home/laolang/github/graver/graver_dotnet/graver/example/test.c",
            "example/build/bin/test", 0);
        }
        catch (GraverException ex)
        {
            log.Error(String.Format("graver compiler failure:{0}", ex.Message));
            flag = false;
        }
        catch (Exception ex)
        {
            log.Error(String.Format("编译出错:{0}", ex.Message));
            flag = false;
        }

        Assert.True(flag);
    }
}