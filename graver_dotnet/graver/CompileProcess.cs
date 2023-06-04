namespace graver
{
    public class CompileProcess
    {
        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        public CompileProcess(string sourceFilename, string outFilename, int flags)
        {
            this.sourceFilename = Path.IsPathRooted(sourceFilename) ? sourceFilename
            : IoUtil.GetWorkDir() + Path.DirectorySeparatorChar + sourceFilename;
            this.outfilename = IoUtil.GetWorkDir() + Path.DirectorySeparatorChar + outFilename;
            this.flags = flags;

            ValidSourceFile();
            RefreshOutFile();
        }

        /// <summary>
        /// 验证输入文件
        /// </summary>
        private void ValidSourceFile()
        {
            try
            {
                using (var stream = new FileStream(sourceFilename, FileMode.Open, FileAccess.Read))
                {
                    log.Debug("加载源文件:{}", sourceFilename);
                }
            }
            catch (IOException ex)
            {
                var msg = String.Format("访问源文件 {0} 时发生异常:{1}", sourceFilename, ex.Message);
                log.Error(String.Format(msg));
                throw new GraverException(msg);
            }
            catch (UnauthorizedAccessException ex)
            {
                var msg = String.Format("无法访问源文件:{0}, 原因:{1}", sourceFilename, ex.Message);
                log.Error(String.Format(msg));
                throw new GraverException(msg);
            }
        }
        /// <summary>
        /// 刷新输出文件
        /// </summary>
        public void RefreshOutFile()
        {
            if (File.Exists(outfilename))
            {
                if (FileAttributes.ReadOnly == (File.GetAttributes(outfilename) & FileAttributes.ReadOnly))
                {
                    var msg = String.Format("输出文件 {0} 不能写入", outfilename);
                    log.Error(String.Format(msg));
                    throw new GraverException(msg);
                }
                else
                {
                    File.WriteAllBytes(outfilename, new byte[0]);
                    log.Debug("清空输出文件:{0}", outfilename);
                }
            }
            else
            {
                try
                {
                    // 创建目录
                    DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetDirectoryName(outfilename));
                    if (!directoryInfo.Exists)
                    {
                        Directory.CreateDirectory(directoryInfo.FullName);
                    }

                    // 创建文件
                    FileInfo file = new FileInfo(outfilename);
                    using (File.Create(file.FullName))
                    {
                        log.Debug("创建输出文件:{0}", outfilename);
                    }
                }
                catch (Exception e)
                {
                    var msg = String.Format("无法创建输出文件 {0} , 原因:{1}", outfilename, e.Message);
                    log.Error(String.Format(msg));
                    throw new GraverException(msg);
                }
            }
        }

        private string sourceFilename;
        public string SourceFilename
        {
            get { return sourceFilename; }
            set { sourceFilename = value; }
        }

        private string outfilename;
        public string OutFilename
        {
            get { return outfilename; }
            set { outfilename = value; }
        }

        private int flags;
        public int Flags
        {
            get { return flags; }
            set { flags = value; }
        }

        private string GetDebuggerDisplay()
        {
            return ToString();
        }
    }
}