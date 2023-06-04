namespace graver
{
    public class IoUtil
    {
        private IoUtil()
        {

        }

        public static string GetWorkDir()
        {
            return Directory.GetCurrentDirectory();
        }

        public static string GetInstallDir()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }
    }
}