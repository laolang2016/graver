---
title: 使用 Makefile 管理 CMake 构建过程
order: 3
---

## 前言

1. 虽然可以使用 `vscode` 的插件在一定程度上简化 `cmake` 的构建命令, 但是我更加喜欢使用脚本的方式控制, 而不是依赖于某个编辑器
2. Makefile 入门不属于本教程范围, 可参考: [跟我一起写Makefile (PDF重制版)](https://github.com/seisman/how-to-write-makefile)

## 目录结构
```
laolang@laolang-mint:cmake-hello$ tree
.
├── CMakeLists.txt
├── main.c
└── Makefile

0 directories, 3 files
laolang@laolang-mint:cmake-hello$ 
```

## Makefile
```makefile
ALL:run

.PHONY: config build run rebuild rerun clean
config:
	cmake -S . -B build
build: config
	cmake --build build
run: build
	cd build/dist/bin && ./graver
rebuild: clean build
rerun: clean run
clean:
	rm -rf build
```

## 运行效果
> 直接执行 `make` 即可

```
laolang@laolang-mint:cmake-hello$ make
cmake -S . -B build
-- The C compiler identification is GNU 11.4.0
-- Detecting C compiler ABI info
-- Detecting C compiler ABI info - done
-- Check for working C compiler: /usr/bin/cc - skipped
-- Detecting C compile features
-- Detecting C compile features - done
-- Configuring done (0.1s)
-- Generating done (0.0s)
-- Build files have been written to: /home/laolang/code/cmake/cmake-hello/build
cmake --build build
gmake[1]: 进入目录“/home/laolang/code/cmake/cmake-hello/build”
gmake[2]: 进入目录“/home/laolang/code/cmake/cmake-hello/build”
gmake[3]: 进入目录“/home/laolang/code/cmake/cmake-hello/build”
gmake[3]: 离开目录“/home/laolang/code/cmake/cmake-hello/build”
gmake[3]: 进入目录“/home/laolang/code/cmake/cmake-hello/build”
[ 50%] Building C object CMakeFiles/graver.dir/main.c.o
[100%] Linking C executable dist/bin/graver
gmake[3]: 离开目录“/home/laolang/code/cmake/cmake-hello/build”
[100%] Built target graver
gmake[2]: 离开目录“/home/laolang/code/cmake/cmake-hello/build”
gmake[1]: 离开目录“/home/laolang/code/cmake/cmake-hello/build”
cd build/dist/bin && ./graver
Hello Cmake.
laolang@laolang-mint:cmake-hello$ 
```

## 结语

本部分的内容虽然简单, 但是已经足够使用, 后续在此基础上可能会做一些简单的优化, 例如帮助列表等体验上的问题