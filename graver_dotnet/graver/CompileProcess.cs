namespace graver
{
    public class CompileProcess
    {

        public CompileProcess()
        {
            int a = 1;
            a = 2;
            a = 3;

            sourceFile = "hello, graver";

            a = 4;
            a = 5;

        }

        private string sourceFile;
        public string SourceFIle
        {
            get { return sourceFile; }
            set { sourceFile = value; }
        }

    }
}