namespace graver
{
    public class Pos
    {
        /// <summary>
        /// 行号
        /// </summary>
        private uint line;
        public uint Line
        {
            get { return line; }
            set { line = value; }
        }

        /// <summary>
        /// 列号
        /// </summary>
        private uint column;
        public uint Column
        {
            get { return column; }
            set { column = value; }
        }

        /// <summary>
        /// 源文件
        /// </summary>
        private string sourceFilename;
        public string SourceFilename
        {
            get { return sourceFilename; }
            set { sourceFilename = value; }
        }
        
        
        
    }
}