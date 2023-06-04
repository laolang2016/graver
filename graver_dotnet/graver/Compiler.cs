namespace graver
{
    public class Compiler
    {
        public CompilerStatus compileFile(string sourceFilename, string outFilename, int flags)
        {
            var process = new CompileProcess(sourceFilename, outFilename, flags);

            // 词法分析

            // 语法分析

            // 代码生成
            return CompilerStatus.COMPILEER_FILE_FAILURE;
        }
    }
}