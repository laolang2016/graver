---
title: cmdline 源码分析
order: 1
---

## 参考资料

[++命令行解析库cmdline使用](https://blog.csdn.net/subfate/article/details/111996410)

[【C++】cmdline —— 轻量级的C++命令行解析库](https://www.cnblogs.com/ljbguanli/p/7235424.html)

`cmdline` github: [https://github.com/tanakh/cmdline/](https://github.com/tanakh/cmdline/)

## cmdline 的基本使用方式

```cpp
#include <iostream>

#include "cmdline/cmdline.h"

int main(int argc, char** argv) {
    cmdline::parser arg;

    arg.set_program_name("graver");
    arg.footer("compiler graver");

    arg.add<std::string>("input", 'i', "Input file", true, "");
    arg.add("lex", 'l', "lexer the input file");

    arg.parse_check(argc, argv);

    std::cout << "input:" << arg.get<std::string>("input") << std::endl;
    std::string lexInput = arg.exist("lex") ? "true" : "false";
    std::cout << "lex:" << lexInput << std::endl;

    return 0;
}
```

效果

```shell
laolang@laolang-mint:bin$ ./graver --input=test.graver -l
input:test.graver
lex:true
laolang@laolang-mint:bin$ 
```


