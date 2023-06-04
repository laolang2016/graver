namespace graver
{
    public class Graver
    {
        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            log.Info("Hello, Graver!");
            try
            {
                var compiler = new Compiler();
                compiler.compileFile("example/test.c", "example/build/bin/test", 0);
            }
            catch (GraverException ex)
            {
                log.Error(String.Format("graver compiler failure:{0}", ex.Message));
            }
            catch (Exception ex)
            {
                log.Error(String.Format("编译出错:{0}", ex.Message));
            }
        }
    }
}