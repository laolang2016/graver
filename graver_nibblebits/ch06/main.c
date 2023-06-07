#include <stdio.h>
#include "compiler.h"

int main()
{
    int res = compile_file("./test.c", "./test", 0);
    if (COMPILER_FILE_COMPILED_OK == res)
    {
        printf("everything compiled fine\n");
    }
    else if (COMPILER_FAILED_WITH_ERRORS == res)
    {
        printf("compile failed\n");
    }
    else
    {
        printf("unknown response compile file\n");
    }
    printf("\nHello World!\n");
}