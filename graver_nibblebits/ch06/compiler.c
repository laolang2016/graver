#include "compiler.h"

int compile_file(const char *filename, const char *out_filename, int flags)
{
    struct compile_process *process = compile_process_create(filename, out_filename, flags);
    if (!process)
    {
        return COMPILER_FAILED_WITH_ERRORS;
    }

    // 词法分析

    // 语法分析

    // 代码生成

    return COMPILER_FILE_COMPILED_OK;
}
