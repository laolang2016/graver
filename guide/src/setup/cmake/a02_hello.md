---
title: Hello World
order: 2
---

## 前言

选择 `cmake` 不是因为 `cmake` 有多么强大, 而是跟随大众的选择

一些软件的安装过程省略

## 第一个例子

### 目录结构
```
laolang@laolang-mint:cmake-hello$ tree
.
├── CMakeLists.txt
└── main.c

0 directories, 2 files
laolang@laolang-mint:cmake-hello$ 
```

### 源码

#### CMakeLists.txt
```cmake
# 指定 cmake 最小版本, 意指 cmake 版本小于指定版本则构建过程终止
cmake_minimum_required(VERSION 3.26)

# 语言环境配置 ###########################################################################################################

# 语言版本
set(CMAKE_C_STANDARD 17)

# 如果指定的语言版本不受支持, 则构建过程终止
set(CMAKE_C_STANDARD_REQUIRED True)

# 只弃用 ISO C++ 标准的编译器标志, 而不使用特定编译器的扩展
set(CMAKE_C_EXTENSIONS OFF)

# 项目名称和语言
project(graver LANGUAGES C VERSION 1.0.0)

# 发布目录
set(dist_dir ${CMAKE_BINARY_DIR}/dist)

# 二进制文件目录
set(bin_dir ${dist_dir}/bin)

# 生成 compile_commands.json
set(CMAKE_EXPORT_COMPILE_COMMANDS ON)

# 生成可执行程序
aux_source_directory(. SRCS_MAIN)
add_executable(${PROJECT_NAME} ${SRCS_MAIN})

# 设置可执行程序的输出路径
set_target_properties(${PROJECT_NAME} PROPERTIES RUNTIME_OUTPUT_DIRECTORY ${bin_dir})
```


#### main.c
```c
#include <stdio.h>
int main(){
    printf("Hello Cmake.\n");
    return 0;
}
```

### 如何编译

使用一条命令即可: `cmake -S . -B build && cmake --build build`, 编译过程如下
```
laolang@laolang-mint:cmake-hello$ cmake -S . -B build && cmake --build build
-- The C compiler identification is GNU 11.4.0
-- Detecting C compiler ABI info
-- Detecting C compiler ABI info - done
-- Check for working C compiler: /usr/bin/cc - skipped
-- Detecting C compile features
-- Detecting C compile features - done
-- Configuring done (0.1s)
-- Generating done (0.0s)
-- Build files have been written to: /home/laolang/code/cmake/cmake-hello/build
[ 50%] Building C object CMakeFiles/graver.dir/main.c.o
[100%] Linking C executable dist/bin/graver
[100%] Built target graver
laolang@laolang-mint:cmake-hello$ 
```

程序运行结果
```
laolang@laolang-mint:cmake-hello$ ./build/dist/bin/graver 
Hello Cmake.
laolang@laolang-mint:cmake-hello$ 
```

## 结语

以上就是 `cmake` 的 Hello World
