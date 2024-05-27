---
title: 使用 CMake 预设
order: 4
---

## 前言
之前的工程有什么缺陷呢?

1. 没有指定构建类型
2. 没有标准化构建目录
3. 没有配置编译器细节

## 什么是 CMake 预设

建议参考文档: [cmake 预设](https://cmake-doc.readthedocs.io/zh-cn/latest/manual/cmake-presets.7.html)

## 目录结构

```
laolang@laolang-mint:cmake-hello$ tree
.
├── CMakeLists.txt
├── CMakePresets.json
├── main.c
└── Makefile

0 directories, 4 files
laolang@laolang-mint:cmake-hello$ 
```

## Makefile
```makefile
ALL:run

.PHONY: config build run rebuild rerun clean
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
```

## CMakePresets.json
```json
{
    "version": 6,
    "cmakeMinimumRequired": {
        "major": 3,
        "minor": 26,
        "patch": 0
    },
    "configurePresets": [
        {
            "name": "linux-base",
            "displayName": "linux base",
            "description": "通用设置",
            "cacheVariables": {
                "CMAKE_MAKE_PROGRAM": "ninja",
                "CMAKE_GENERATOR": "Ninja",
                "CMAKE_C_COMPILER": "gcc",
                "CMAKE_C_FLAGS": "-Wall -Wextra"
            }
        },
        {
            "name": "linux-release",
            "displayName": "linux release",
            "inherits": "linux-base",
            "binaryDir": "${sourceDir}/build/ninja-release",
            "cacheVariables": {
                "CMAKE_BUILD_TYPE": "Release"
            }
        },
        {
            "name": "linux-debug",
            "displayName": "linux debug",
            "inherits": "linux-base",
            "binaryDir": "${sourceDir}/build/ninja-debug",
            "cacheVariables": {
                "CMAKE_BUILD_TYPE": "Debug",
                "CMAKE_DEBUG_POSTFIX": "d"
            }
        }
    ],
    "buildPresets": [
        {
            "name": "linux-release",
            "configurePreset": "linux-release"
        },
        {
            "name": "linux-debug",
            "configurePreset": "linux-debug"
        }
    ]
}
```

## 效果
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
[2/2] Linking C executable dist/bin/graver
cd build/ninja-release/dist/bin && ./graver
Hello Cmake.
laolang@laolang-mint:cmake-hello$ 
```

## 些许说明

1. 使用 `ninja` 加快编译速度
2. `relase` 构建目录为 `build/ninja-release` 
3. `debug` 构建目录为 `build/ninja-debug` 
4. 可以在 `CMakePresets.json` 中指定编译器参数

## 结语

上述 `CMakePresets.json` 已经足够应付简单的学习场景了, 后续可能会添加 `windows` 环境的配置