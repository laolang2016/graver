---
title: 多目录与动态库
order: 5
---

## 前言

1. 多目录构建
2. 动态库的创建与使用


## 目录结构
```
laolang@laolang-mint:cmake-hello$ tree -a
.
├── .clang-format
├── .clang-tidy
├── CMakeLists.txt
├── CMakePresets.json
├── doc
│   ├── delete_me.css
│   ├── delete_me.html
│   ├── Doxyfile
│   ├── doxygen
│   │   ├── desc.md
│   │   ├── env_ubuntu.md
│   │   ├── env_window.h
│   │   └── mainpage.h
│   ├── doxygen-awesome-css
│   │   ├── doxygen-awesome.css
│   │   ├── doxygen-awesome-fragment-copy-button.js
│   │   ├── doxygen-awesome-interactive-toc.js
│   │   └── doxygen-awesome-sidebar-only.css
│   ├── header.html
│   └── images
│       └── avatar.jpg
├── .gitignore
├── include
│   └── graver
│       └── lib
│           └── graver_double_list.h
├── Makefile
├── src
│   ├── app
│   │   ├── CMakeLists.txt
│   │   └── main.c
│   ├── CMakeLists.txt
│   └── lib
│       ├── CMakeLists.txt
│       └── graver_double_list.c
└── .vscode
    ├── launch.json
    ├── settings.json
    └── tasks.json

11 directories, 28 files
laolang@laolang-mint:cmake-hello$ 
```


## cmake

### 根目录 CMakeLists.txt

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

# 将 install_rpath 的设置应用在 build_rpath 上
# 避免在开发期间出现动态库找不到的问题
set(CMAKE_BUILD_WITH_INSTALL_RPATH True)

# 项目配置 ##############################################################################################################

# 项目名称和语言
project(graver LANGUAGES C VERSION 1.0.0)

# 发布目录
set(dist_dir ${CMAKE_BINARY_DIR}/dist)

# 二进制文件目录
set(bin_dir ${dist_dir}/bin)

# 裤文件目录
set(lib_dir ${dist_dir}/lib)

# 内部库
# 配置类库
set(lib_lib_name graverlib)

# 编译相关配置 ###########################################################################################################

# 生成 compile_commands.json
set(CMAKE_EXPORT_COMPILE_COMMANDS ON)

# 包含全局头文件
include_directories(${PROJECT_SOURCE_DIR}/include)

# 添加子目录
add_subdirectory(src)
```

### 主程序 CMakeLists.txt
```cmake
aux_source_directory(. SRCS_MAIN)
add_executable(${PROJECT_NAME} ${SRCS_MAIN})

# 链接动态库
target_link_libraries(${PROJECT_NAME}
    ${lib_lib_name}
)
set_target_properties(${PROJECT_NAME} PROPERTIES RUNTIME_OUTPUT_DIRECTORY ${bin_dir})

# 设置 rpath
set_target_properties(${PROJECT_NAME} PROPERTIES INSTALL_RPATH "\${ORIGIN}/../lib")
```

### lib CMakeLists.txt
```cmake
aux_source_directory(. SRCS_LIB)
add_library(${lib_lib_name} SHARED ${SRCS_LIB})

# 设置动态库生成目录
set_target_properties(${lib_lib_name} PROPERTIES LIBRARY_OUTPUT_DIRECTORY ${lib_dir})
```

## Makefile
```makefile
ALL:run

.PHONY: config build run rebuild rerun clean format tidy doc
config:
	cmake --preset=linux-release
build: config
	cmake --build --preset=linux-release
run: build
	cd build/ninja-release/dist/bin && ./graver
rebuild: clean build
rerun: clean run
clean:
	rm -rf build
format:
	find src -type f \( -name "*.c" -o -name "*.cc" -o -name "*.cpp" -o -name "*.h" -o -name "*.hpp" \) -exec clang-format -style=file:.clang-format -i {} \;
	find include -type f \( -name "*.c" -o -name "*.cc" -o -name "*.cpp" -o -name "*.h" -o -name "*.hpp" \) -exec clang-format -style=file:.clang-format -i {} \;
tidy:build format
	find src -type f -name "*.c" -print0 | xargs -0 clang-tidy --config-file=.clang-tidy -p=build/ninja-release --quiet
	find include -type f -name "*.h" -print0 | xargs -0 clang-tidy --config-file=.clang-tidy -p=build/ninja-release --quiet
doc:
	rm -rf build/doc
	mkdir -p build/doc
	cd doc && doxygen Doxyfile
	cp doc/doxygen-awesome-css/doxygen-awesome-fragment-copy-button.js build/doc/html/
	cp doc/doxygen-awesome-css/doxygen-awesome-interactive-toc.js build/doc/html/
	cp -r doc/images build/doc/html/images
```

## 运行效果

### 一键运行
```
laolang@laolang-mint:cmake-hello$ make 
cmake --preset=linux-release
Preset CMake variables:

  CMAKE_BUILD_TYPE="Release"
  CMAKE_C_COMPILER="gcc"
  CMAKE_C_FLAGS="-Wall -Wextra"
  CMAKE_GENERATOR="Ninja"
  CMAKE_MAKE_PROGRAM="ninja"

-- The C compiler identification is GNU 11.4.0
-- Detecting C compiler ABI info
-- Detecting C compiler ABI info - done
-- Check for working C compiler: /usr/bin/gcc - skipped
-- Detecting C compile features
-- Detecting C compile features - done
-- Configuring done (0.1s)
-- Generating done (0.0s)
-- Build files have been written to: /home/laolang/code/cmake/cmake-hello/build/ninja-release
cmake --build --preset=linux-release
[4/4] Linking C executable dist/bin/graver
cd build/ninja-release/dist/bin && ./graver
1 2 3 4 5 
laolang@laolang-mint:cmake-hello$ 
```

### rpath 测试

清理项目并将 `build/ninja-release/dist` 目录复制到其他地方, 查看其依赖库信息

```
laolang@laolang-mint:bin$ ldd graver 
	linux-vdso.so.1 (0x00007ffe9bbb5000)
	libgraverlib.so => /home/laolang/tmp/dist/bin/./../lib/libgraverlib.so (0x00007f7be3c3a000)
	libc.so.6 => /lib/x86_64-linux-gnu/libc.so.6 (0x00007f7be39fc000)
	/lib64/ld-linux-x86-64.so.2 (0x00007f7be3c46000)
laolang@laolang-mint:bin$ 
```

## 结语

到这一步已经基本完成了一个 `cmake` 与 `c/c++` 开发环境的搭建, 其实 `cmake` 还有很多功能没有涉及, 比如 [生成器表达式](https://cmake-doc.readthedocs.io/zh-cn/latest/manual/cmake-generator-expressions.7.html), 比如 [cpack](https://cmake-doc.readthedocs.io/zh-cn/latest/index.html), 这些功能暂时还用不到, 就先不考虑了


