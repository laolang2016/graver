#include "compiler.h"

static struct lex_process_functions compiler_lex_functions = {
    .next_char = compile_process_next_char,
    .peek_char = compile_process_peek_char,
    .push_char = compile_process_push_char};

int compile_file(const char *filename, const char *out_filename, int flags)
{
    struct compile_process *process = compile_process_create(filename, out_filename, flags);
    if (!process)
    {
        return COMPILER_FAILED_WITH_ERRORS;
    }

    // 词法分析
    struct lex_process * lex_process= lex_process_create(process,&compiler_lex_functions, NULL);
    if(!lex_process)
    {
        return COMPILER_FAILED_WITH_ERRORS;
    }
    if( LEXICAL_ANALYSIS_ALL_OK != lex(lex_process))
    {
        return COMPILER_FAILED_WITH_ERRORS;
    }

    // 语法分析

    // 代码生成

    return COMPILER_FILE_COMPILED_OK;
}
