---
title: 基础配置
---

## 说明

1. `vscode` 可以做很多事, 而我目前使用 `vscode` 主要用来写 `c` 、 `C++` 、 `lua` , 其他语言都有更合适的工具, 比如 `java` 使用 `idea`, `python` 使用 `pycharm`.
2. 为节省时间考虑, 只给出配置过程与效果截图, 不做详细的解释
3. 为简单起见, 只考虑 `MinGW` 与 `GNU GCC`

## 参考资料

[VScode配置C++开发工具链(借助clangd+cmake)](https://zhuanlan.zhihu.com/p/603687041)


## C/C++


### 目录结构

```
E:\t\vscode-cpp>tree /f
卷 文档 的文件夹 PATH 列表
卷序列号为 EDE0-BB33
E:.
│  .clang-format
│  .clang-tidy
│  CMakeLists.txt
│  CMakePresets.json
│  main.c
│
└─.vscode
        launch.json
        settings.json
        tasks.json


E:\t\vscode-cpp>
```

### cmake 脚本

#### CMakeLists.txt
```cmake
cmake_minimum_required(VERSION 3.26)

set(CMAKE_C_STANDARD 17)
set(CMAKE_C_STANDARD_REQUIRED True)
set(CMAKE_C_EXTENSIONS OFF)

project(graver LANGUAGES C VERSION 1.0.0)

# 发布目录
set(dist_dir ${CMAKE_BINARY_DIR}/dist)

# 二进制文件目录
set(bin_dir ${dist_dir}/bin)

# 生成 compile_commands.json
set(CMAKE_EXPORT_COMPILE_COMMANDS ON)

aux_source_directory(. SRCS_MAIN)
add_executable(${PROJECT_NAME} ${SRCS_MAIN})
set_target_properties(${PROJECT_NAME} PROPERTIES RUNTIME_OUTPUT_DIRECTORY ${bin_dir})
```


#### CMakePresets.json
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
            "name": "windows-base",
            "displayName": "gnu base",
            "description": "通用设置",
            "cacheVariables": {
                "CMAKE_MAKE_PROGRAM": "D:/program/qt/Tools/Ninja/ninja.exe",
                "CMAKE_GENERATOR": "Ninja",
                "CMAKE_C_COMPILER": "D:/program/qt/Tools/mingw1120_64/bin/gcc.exe",
                "CMAKE_C_FLAGS": "-Wall -Wextra"
            }
        },
        {
            "name": "windows-release",
            "displayName": "windows release",
            "inherits": "windows-base",
            "binaryDir": "${sourceDir}/build/ninja-release",
            "cacheVariables": {
                "CMAKE_BUILD_TYPE": "Release"
            }
        },
        {
            "name": "windows-debug",
            "displayName": "windows debug",
            "inherits": "windows-base",
            "binaryDir": "${sourceDir}/build/ninja-debug",
            "cacheVariables": {
                "CMAKE_BUILD_TYPE": "Debug",
                "CMAKE_DEBUG_POSTFIX": "d"
            }
        },
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
            "name": "windows-release",
            "configurePreset": "windows-release"
        },
        {
            "name": "windows-debug",
            "configurePreset": "windows-debug"
        },
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

### vscode

#### launch.json
```json
{
    // 使用 IntelliSense 了解相关属性。 
    // 悬停以查看现有属性的描述。
    // 欲了解更多信息，请访问: https://go.microsoft.com/fwlink/?linkid=830387
    "version": "0.2.0",
    "configurations": [
        {
            "name": "app",
            "type": "lldb",
            "request": "launch",
            "program": "${workspaceRoot}/build/ninja-debug/dist/bin/graver.exe",
            "args": [],
            "cwd": "${workspaceFolder}/build/ninja-debug/dist/bin",
            "preLaunchTask": "build-debug"
        }
    ]
}
```


#### tasks.json
```json
{
    // See https://go.microsoft.com/fwlink/?LinkId=733558
    // for the documentation about the tasks.json format
    "version": "2.0.0",
    "tasks": [
        // debug 构建
        {
            "label": "build-debug",
            "type": "shell",
            "windows": {
                "command": "cmake --preset=windows-debug && cmake --build --preset=windows-debug"
            },
            "linux": {
                "command": "cmake --preset=linux-debug && cmake --build --preset=linux-debug"
            },
        },
        // release 构建
        {
            "label": "build-release",
            "type": "shell",
            "windows": {
                "command": "cmake --preset=windows-release && cmake --build --preset=windows-release"
            },
            "linux": {
                "command": "cmake --preset=linux-release && cmake --build --preset=linux-release"
            }
        },
        // 清理 debug
        {
            "label": "clean-debug",
            "type": "shell",
            "command": "rm -rf build/ninja-debug"
        },
        // 清理 release
        {
            "label": "clean-release",
            "type": "shell",
            "command": "rm -rf build/ninja-release"
        },
        // 清理 所有
        {
            "label": "clean-all",
            "type": "shell",
            "command": "rm -rf build"
        },
        // 重新 debug 构建
        {
            "label": "rebuild-debug",
            "type": "shell",
            "dependsOn": [
                "clean-debug",
                "build-debug"
            ]
        },
        // 重新 release 构建
        {
            "label": "rebuild-release",
            "type": "shell",
            "dependsOn": [
                "clean-release",
                "build-release"
            ]
        },
        // 运行 debug
        {
            "label": "run-debug",
            "type": "shell",
            "command": "cd build/ninja-debug/dist/bin && ./graver",
            "dependsOn": [
                "build-debug"
            ]
        },
        // 运行 release
        {
            "label": "run-release",
            "type": "shell",
            "command": "cd build/ninja-release/dist/bin && ./graver",
            "dependsOn": [
                "build-release"
            ]
        },
        // format
        {
            "label": "format",
            "type": "shell",
            "command": "./format.sh"
        },
        {
            "label": "tidy",
            "type": "shell",
            "command": "./tidy.sh"
        },
        {
            "label": "doc",
            "type": "shell",
            "command": "./doc.sh"
        }
    ]
}
```


#### settings.json
```json
{
    // 隐藏某些文件
    "files.exclude": {
        "**/.git": true,
        "**/.svn": true,
        "**/.hg": true,
        "**/CVS": true,
        "**/.DS_Store": true,
        "**/Thumbs.db": true,
        // "**/build": true,
    },
    // 保存时自动格式化
    "editor.formatOnSave": true,
    "[cmake]": {
        "editor.defaultFormatter": "josetr.cmake-language-support-vscode"
    },
    "[c]": {
        "editor.defaultFormatter": "xaver.clang-format"
    },
    "[h]": {
        "editor.defaultFormatter": "xaver.clang-format"
    },
    "[cpp]": {
        "editor.defaultFormatter": "xaver.clang-format"
    },
    // 修改了注释的颜色
    "editor.tokenColorCustomizations": {
        "comments": {
            "foreground": "#4a524e",
            "fontStyle": ""
        }
    },
    // 右侧标尺
    "editor.rulers": [
        120
    ],
    // 关闭形参显示
    "editor.inlayHints.enabled": "off",
    // clang tidy 配置
    "clang-tidy.buildPath": "build/ninja-release",
    "clang-tidy.checks": [
        // //custom checks
        // "performance-*",
        // "bugprone-*",
        // "portability-*",
        // "modernize-*",
        // "cppcoreguidelines-*",
        // "google-*",
        // "readability-*",
        // //all checks
        // "*",
        //Clion checks
        "bugprone-argument-comment",
        "bugprone-assert-side-effect",
        "bugprone-bad-signal-to-kill-thread",
        "bugprone-branch-clone",
        "bugprone-copy-constructor-init",
        "bugprone-dangling-handle",
        "bugprone-dynamic-static-initializers",
        "bugprone-fold-init-type",
        "bugprone-forward-declaration-namespace",
        "bugprone-forwarding-reference-overload",
        "bugprone-inaccurate-erase",
        "bugprone-incorrect-roundings",
        "bugprone-integer-division",
        "bugprone-lambda-function-name",
        "bugprone-macro-parentheses",
        "bugprone-macro-repeated-side-effects",
        "bugprone-misplaced-operator-in-strlen-in-alloc",
        "bugprone-misplaced-pointer-arithmetic-in-alloc",
        "bugprone-misplaced-widening-cast",
        "bugprone-move-forwarding-reference",
        "bugprone-multiple-statement-macro",
        "bugprone-narrowing-conversions",
        "bugprone-no-escape",
        "bugprone-not-null-terminated-result",
        "bugprone-parent-virtual-call",
        "bugprone-posix-return",
        "bugprone-reserved-identifier",
        "bugprone-sizeof-container",
        "bugprone-sizeof-expression",
        "bugprone-string-constructor",
        "bugprone-string-integer-assignment",
        "bugprone-string-literal-with-embedded-nul",
        "bugprone-suspicious-enum-usage",
        "bugprone-suspicious-include",
        "bugprone-suspicious-memory-comparison",
        "bugprone-suspicious-memset-usage",
        "bugprone-suspicious-missing-comma",
        "bugprone-suspicious-semicolon",
        "bugprone-suspicious-string-compare",
        "bugprone-swapped-arguments",
        "bugprone-terminating-continue",
        "bugprone-throw-keyword-missing",
        "bugprone-too-small-loop-variable",
        "bugprone-undefined-memory-manipulation",
        "bugprone-undelegated-constructor",
        "bugprone-unhandled-self-assignment",
        "bugprone-unused-raii",
        "bugprone-unused-return-value",
        "bugprone-use-after-move",
        "bugprone-virtual-near-miss",
        "boost-use-to-string",
        "cert-dcl03-c",
        "cert-dcl21-cpp",
        "cert-dcl58-cpp",
        "cert-err34-c",
        "cert-err52-cpp",
        "cert-err58-cpp",
        "cert-err60-cpp",
        "cert-flp30-c",
        "cert-msc50-cpp",
        "cert-msc51-cpp",
        "cert-oop54-cpp",
        "cert-str34-c",
        "cppcoreguidelines-interfaces-global-init",
        "cppcoreguidelines-narrowing-conversions",
        "cppcoreguidelines-pro-type-member-init",
        "cppcoreguidelines-pro-type-static-cast-downcast",
        "cppcoreguidelines-slicing",
        "google-default-arguments",
        "google-explicit-constructor",
        "google-runtime-operator",
        "hicpp-exception-baseclass",
        "hicpp-multiway-paths-covered",
        "hicpp-signed-bitwise",
        "misc-misplaced-const",
        "misc-new-delete-overloads",
        "misc-no-recursion",
        "misc-non-copyable-objects",
        "misc-redundant-expression",
        "misc-static-assert",
        "misc-throw-by-value-catch-by-reference",
        "misc-unconventional-assign-operator",
        "misc-uniqueptr-reset-release",
        "modernize-avoid-bind",
        "modernize-concat-nested-namespaces",
        "modernize-deprecated-headers",
        "modernize-deprecated-ios-base-aliases",
        "modernize-loop-convert",
        "modernize-make-shared",
        "modernize-make-unique",
        "modernize-pass-by-value",
        "modernize-raw-string-literal",
        "modernize-redundant-void-arg",
        "modernize-replace-auto-ptr",
        "modernize-replace-disallow-copy-and-assign-macro",
        "modernize-replace-random-shuffle",
        "modernize-return-braced-init-list",
        "modernize-shrink-to-fit",
        "modernize-unary-static-assert",
        "modernize-use-auto",
        "modernize-use-bool-literals",
        "modernize-use-emplace",
        "modernize-use-equals-default",
        "modernize-use-equals-delete",
        "modernize-use-nodiscard",
        "modernize-use-noexcept",
        "modernize-use-nullptr",
        "modernize-use-override",
        "modernize-use-transparent-functors",
        "modernize-use-uncaught-exceptions",
        "mpi-buffer-deref",
        "mpi-type-mismatch",
        "openmp-use-default-none",
        "performance-faster-string-find",
        "performance-for-range-copy",
        "performance-implicit-conversion-in-loop",
        "performance-inefficient-algorithm",
        "performance-inefficient-string-concatenation",
        "performance-inefficient-vector-operation",
        "performance-move-const-arg",
        "performance-move-constructor-init",
        "performance-no-automatic-move",
        "performance-noexcept-move-constructor",
        "performance-trivially-destructible",
        "performance-type-promotion-in-math-fn",
        "performance-unnecessary-copy-initialization",
        "performance-unnecessary-value-param",
        "portability-simd-intrinsics",
        "readability-avoid-const-params-in-decls",
        "readability-const-return-type",
        "readability-container-size-empty",
        "readability-convert-member-functions-to-static",
        "readability-delete-null-pointer",
        "readability-deleted-default",
        "readability-inconsistent-declaration-parameter-name",
        "readability-make-member-function-const",
        "readability-misleading-indentation",
        "readability-misplaced-array-index",
        "readability-non-const-parameter",
        "readability-redundant-control-flow",
        "readability-redundant-declaration",
        "readability-redundant-function-ptr-dereference",
        "readability-redundant-smartptr-get",
        "readability-redundant-string-cstr",
        "readability-redundant-string-init",
        "readability-simplify-subscript-expr",
        "readability-static-accessed-through-instance",
        "readability-static-definition-in-anonymous-namespace",
        "readability-string-compare",
        "readability-uniqueptr-delete-release",
        "readability-use-anyofallof"
    ],
    // clangd 配置
    "clangd.fallbackFlags": [
        // 设置clangd代码检查的c++版本，目前默认是c++14
        "-std=c17",
        // 增加项目自身头文件依赖路劲，因为使用vs2019编译不会生成compile_command.json文件，项目自己的头文件就不会找到
        "-I${workspaceFolder}", // 项目根目录
    ],
    "clangd.arguments": [
        //后台分析并保存索引文件
        "--background-index",
        //compile_commands.json目录位置
        "--compile-commands-dir=build/ninja-release",
        //同时开启的任务数
        "-j=12",
        // "--folding-ranges",
        //"--query-driver=/usr/bin/clang++", //mac上需要配置
        //启用clang-tidy以提供静态检查
        "--clang-tidy",
        //当 clangd 准备就绪时，用它来分析建议
        "--completion-parse=auto",
        //建议风格
        "--completion-style=detailed",
        //补全函数时，给参数提供占位符，可以使用 Tab 切换下一个占位符，知道函数末尾
        "--function-arg-placeholders",
        //默认格式化风格: 可用的有 LLVM, Google, Chromium, Mozilla, Webkit, Microsoft, GNU
        //也可以编写 .clang-format 自定义风格
        "--fallback-style=Webkit",
        //pch 优化的位置(Memory 或 Disk,前者会增加内存开销，但会提升性能)
        "--pch-storage=disk",
        //让clangd生成更详细的日志
        "--log=verbose",
        // 输出的 JSON 文件更加美观
        "--pretty",
        //建议中，已包含头文件的项与还未包含头文件的项会以圆点加以区分
        "--header-insertion-decorators",
        //插入建议时自动引入头文件
        "--header-insertion=iwyu",
        //全局补全，例如没有#include <iostream>时，也会给出std::cout建议
        //建议配合"--header-insertion=iwyu"使用
        "--all-scopes-completion",
        // 建议的排序方案：hueristics (启发式), decision_forest (决策树)
        "--ranking-model=decision_forest",
    ],
    //借助网上的信息排序建议
    "clangd.serverCompletionRanking": true,
    //当其它扩展与 clangd 冲突时警告并建议禁用
    "clangd.detectExtensionConflicts": true,
    // todo 配置
    // "todo-tree.filtering.excludeGlobs": [
    //     "**/third/**"
    // ],
    // "todo-tree.filtering.includeGlobs": [
    //     "**/include/**",
    //     "**/src/**"
    // ],
    "todo-tree.filtering.ignoreGitSubmodules": true,
    "todohighlight.keywords": [],
    "todo-tree.tree.showCountsInTree": true,
    "todohighlight.keywordsPattern": "TODO:|FIXME:|NOTE:|\\(([^)]+)\\)",
    "todohighlight.defaultStyle": {},
    "todohighlight.isEnable": false,
    "todo-tree.highlights.customHighlight": {
        "BUG": {
            "icon": "bug",
            "foreground": "#F56C6C",
            "type": "line"
        },
        "FIXME": {
            "icon": "flame",
            "foreground": "#FF9800",
            "type": "line"
        },
        "TODO": {
            "foreground": "#409EFF",
            "type": "line"
        },
        "NOTE": {
            "icon": "note",
            "foreground": "#67C23A",
            "type": "line"
        },
        "INFO": {
            "icon": "info",
            "foreground": "#909399",
            "type": "line"
        },
        "TAG": {
            "icon": "tag",
            "foreground": "#409EFF",
            "type": "line"
        },
        "HACK": {
            "icon": "versions",
            "foreground": "#E040FB",
            "type": "line"
        },
        "XXX": {
            "icon": "unverified",
            "foreground": "#E91E63",
            "type": "line"
        }
    },
    "todo-tree.general.tags": [
        "BUG",
        "HACK",
        "FIXME",
        "TODO",
        "INFO",
        "NOTE",
        "TAG",
        "XXX"
    ],
    "todo-tree.general.statusBar": "total",
    // 任务栏 task 快捷按钮
    "VsCodeTaskButtons.showCounter": true,
    "VsCodeTaskButtons.tasks": [
        {
            "label": "format",
            "task": "format"
        },
        {
            "label": "tidy",
            "task": "tidy"
        },
        {
            "label": "$(notebook-delete-cell) clean-all",
            "task": "clean-all"
        },
        {
            "label": "$(notebook-delete-cell) clean",
            "task": "clean-release"
        },
        {
            "label": "$(debug-configure) rebuild",
            "task": "rebuild-release"
        },
        {
            "label": "$(debug-configure) build-release",
            "task": "build-release"
        },
        {
            "label": "$(notebook-execute) run",
            "task": "run-release"
        },
        {
            "label": "📖 doc",
            "task": "doc"
        }
    ]
}
```

### clang

#### .clang-format

```yaml
---
Language:        Cpp
BasedOnStyle:  Google

# 访问说明符(public、private等)的偏移, -4表示顶格
AccessModifierOffset: -4

# 延续的行的缩进宽度
ContinuationIndentWidth: 4
# 缩进宽度
IndentWidth: 4
# tab宽度
TabWidth: 4
UseTab: Never

# 连续赋值时，对齐所有等号
AlignConsecutiveAssignments: true

# 对连续的位域进行对齐
AlignConsecutiveBitFields: false

# 连续声明时，对齐所有声明的变量名
AlignConsecutiveDeclarations: true

# 对齐连续的尾随的注释
AlignTrailingComments: true

# 是否将较短的枚举定义放在一行中
AllowShortEnumsOnASingleLine: false

# 是否将较短的代码块放在一行中
AllowShortBlocksOnASingleLine: Never

# 允许短的函数放在同一行: None, InlineOnly(定义在类中), Empty(空函数), Inline(定义在类中，空函数), All
AllowShortFunctionsOnASingleLine: Empty

# All: lambda 没有超过宽度, 则在同一行 , Empty: 仅允许定义没有参数并且是空的 Lambda 表达式压缩到一行
AllowShortLambdasOnASingleLine: Empty

# 是否将较短的循环放在一行中
AllowShortLoopsOnASingleLine: true

# 是否在多行字符串前强制换行
AlwaysBreakBeforeMultilineStrings: false

# 是否在模板声明后强制换行
AlwaysBreakTemplateDeclarations: true

# 每行字符的限制，0表示没有限制
ColumnLimit: 120

# 对 include 排序
SortIncludes: true

# 宏定义缩进方式: None:不缩进, AfterHash:#后插入空格, #BeforeHash:前插入空格
IndentPPDirectives: BeforeHash

# 是否允许clang-format尝试重新粘合注释(true/false), 不建议使用
ReflowComments:  false
```

#### .clang-tidy
```yaml
---
Checks: "bugprone-argument-comment,bugprone-assert-side-effect,bugprone-bad-signal-to-kill-thread,bugprone-branch-clone,bugprone-copy-constructor-init,bugprone-dangling-handle,bugprone-dynamic-static-initializers,bugprone-fold-init-type,bugprone-forward-declaration-namespace,bugprone-forwarding-reference-overload,bugprone-inaccurate-erase,bugprone-incorrect-roundings,bugprone-integer-division,bugprone-lambda-function-name,bugprone-macro-parentheses,bugprone-macro-repeated-side-effects,bugprone-misplaced-operator-in-strlen-in-alloc,bugprone-misplaced-pointer-arithmetic-in-alloc,bugprone-misplaced-widening-cast,bugprone-move-forwarding-reference,bugprone-multiple-statement-macro,bugprone-narrowing-conversions,bugprone-no-escape,bugprone-not-null-terminated-result,bugprone-parent-virtual-call,bugprone-posix-return,bugprone-reserved-identifier,bugprone-sizeof-container,bugprone-sizeof-expression,bugprone-string-constructor,bugprone-string-integer-assignment,bugprone-string-literal-with-embedded-nul,bugprone-suspicious-enum-usage,bugprone-suspicious-include,bugprone-suspicious-memory-comparison,bugprone-suspicious-memset-usage,bugprone-suspicious-missing-comma,bugprone-suspicious-semicolon,bugprone-suspicious-string-compare,bugprone-swapped-arguments,bugprone-terminating-continue,bugprone-throw-keyword-missing,bugprone-too-small-loop-variable,bugprone-undefined-memory-manipulation,bugprone-undelegated-constructor,bugprone-unhandled-self-assignment,bugprone-unused-raii,bugprone-unused-return-value,bugprone-use-after-move,bugprone-virtual-near-miss,boost-use-to-string,cert-dcl03-c,cert-dcl21-cpp,cert-dcl58-cpp,cert-err34-c,cert-err52-cpp,cert-err58-cpp,cert-err60-cpp,cert-flp30-c,cert-msc50-cpp,cert-msc51-cpp,cert-oop54-cpp,cert-str34-c,cppcoreguidelines-interfaces-global-init,cppcoreguidelines-narrowing-conversions,cppcoreguidelines-pro-type-member-init,cppcoreguidelines-pro-type-static-cast-downcast,cppcoreguidelines-slicing,google-default-arguments,google-explicit-constructor,google-runtime-operator,hicpp-exception-baseclass,hicpp-multiway-paths-covered,hicpp-signed-bitwise,misc-misplaced-const,misc-new-delete-overloads,misc-no-recursion,misc-non-copyable-objects,misc-redundant-expression,misc-static-assert,misc-throw-by-value-catch-by-reference,misc-unconventional-assign-operator,misc-uniqueptr-reset-release,modernize-avoid-bind,modernize-concat-nested-namespaces,modernize-deprecated-headers,modernize-deprecated-ios-base-aliases,modernize-loop-convert,modernize-make-shared,modernize-make-unique,modernize-pass-by-value,modernize-raw-string-literal,modernize-redundant-void-arg,modernize-replace-auto-ptr,modernize-replace-disallow-copy-and-assign-macro,modernize-replace-random-shuffle,modernize-return-braced-init-list,modernize-shrink-to-fit,modernize-unary-static-assert,modernize-use-auto,modernize-use-bool-literals,modernize-use-emplace,modernize-use-equals-default,modernize-use-equals-delete,modernize-use-nodiscard,modernize-use-noexcept,modernize-use-nullptr,modernize-use-override,modernize-use-transparent-functors,modernize-use-uncaught-exceptions,mpi-buffer-deref,mpi-type-mismatch,openmp-use-default-none,performance-faster-string-find,performance-for-range-copy,performance-implicit-conversion-in-loop,performance-inefficient-algorithm,performance-inefficient-string-concatenation,performance-inefficient-vector-operation,performance-move-const-arg,performance-move-constructor-init,performance-no-automatic-move,performance-noexcept-move-constructor,performance-trivially-destructible,performance-type-promotion-in-math-fn,performance-unnecessary-copy-initialization,performance-unnecessary-value-param,portability-simd-intrinsics,readability-avoid-const-params-in-decls,readability-const-return-type,readability-container-size-empty,readability-convert-member-functions-to-static,readability-delete-null-pointer,readability-deleted-default,readability-inconsistent-declaration-parameter-name,readability-make-member-function-const,readability-misleading-indentation,readability-misplaced-array-index,readability-non-const-parameter,readability-redundant-control-flow,readability-redundant-declaration,readability-redundant-function-ptr-dereference,readability-redundant-smartptr-get,readability-redundant-string-cstr,readability-redundant-string-init,readability-simplify-subscript-expr,readability-static-accessed-through-instance,readability-static-definition-in-anonymous-namespace,readability-string-compare,readability-uniqueptr-delete-release,readability-use-anyofallof"
WarningsAsErrors: "*"
HeaderFilterRegex: ""
AnalyzeTemporaryDtors: false
FormatStyle: none
User: laolang
CheckOptions:
  - key: llvm-else-after-return.WarnOnConditionVariables
    value: "false"
  - key: modernize-loop-convert.MinConfidence
    value: reasonable
  - key: modernize-replace-auto-ptr.IncludeStyle
    value: llvm
  - key: cert-str34-c.DiagnoseSignedUnsignedCharComparisons
    value: "false"
  - key: google-readability-namespace-comments.ShortNamespaceLines
    value: "10"
  - key: cert-err33-c.CheckedFunctions
    value: "::aligned_alloc;::asctime_s;::at_quick_exit;::atexit;::bsearch;::bsearch_s;::btowc;::c16rtomb;::c32rtomb;::calloc;::clock;::cnd_broadcast;::cnd_init;::cnd_signal;::cnd_timedwait;::cnd_wait;::ctime_s;::fclose;::fflush;::fgetc;::fgetpos;::fgets;::fgetwc;::fopen;::fopen_s;::fprintf;::fprintf_s;::fputc;::fputs;::fputwc;::fputws;::fread;::freopen;::freopen_s;::fscanf;::fscanf_s;::fseek;::fsetpos;::ftell;::fwprintf;::fwprintf_s;::fwrite;::fwscanf;::fwscanf_s;::getc;::getchar;::getenv;::getenv_s;::gets_s;::getwc;::getwchar;::gmtime;::gmtime_s;::localtime;::localtime_s;::malloc;::mbrtoc16;::mbrtoc32;::mbsrtowcs;::mbsrtowcs_s;::mbstowcs;::mbstowcs_s;::memchr;::mktime;::mtx_init;::mtx_lock;::mtx_timedlock;::mtx_trylock;::mtx_unlock;::printf_s;::putc;::putwc;::raise;::realloc;::remove;::rename;::scanf;::scanf_s;::setlocale;::setvbuf;::signal;::snprintf;::snprintf_s;::sprintf;::sprintf_s;::sscanf;::sscanf_s;::strchr;::strerror_s;::strftime;::strpbrk;::strrchr;::strstr;::strtod;::strtof;::strtoimax;::strtok;::strtok_s;::strtol;::strtold;::strtoll;::strtoul;::strtoull;::strtoumax;::strxfrm;::swprintf;::swprintf_s;::swscanf;::swscanf_s;::thrd_create;::thrd_detach;::thrd_join;::thrd_sleep;::time;::timespec_get;::tmpfile;::tmpfile_s;::tmpnam;::tmpnam_s;::tss_create;::tss_get;::tss_set;::ungetc;::ungetwc;::vfprintf;::vfprintf_s;::vfscanf;::vfscanf_s;::vfwprintf;::vfwprintf_s;::vfwscanf;::vfwscanf_s;::vprintf_s;::vscanf;::vscanf_s;::vsnprintf;::vsnprintf_s;::vsprintf;::vsprintf_s;::vsscanf;::vsscanf_s;::vswprintf;::vswprintf_s;::vswscanf;::vswscanf_s;::vwprintf_s;::vwscanf;::vwscanf_s;::wcrtomb;::wcschr;::wcsftime;::wcspbrk;::wcsrchr;::wcsrtombs;::wcsrtombs_s;::wcsstr;::wcstod;::wcstof;::wcstoimax;::wcstok;::wcstok_s;::wcstol;::wcstold;::wcstoll;::wcstombs;::wcstombs_s;::wcstoul;::wcstoull;::wcstoumax;::wcsxfrm;::wctob;::wctrans;::wctype;::wmemchr;::wprintf_s;::wscanf;::wscanf_s;"
  - key: cert-oop54-cpp.WarnOnlyIfThisHasSuspiciousField
    value: "false"
  - key: cert-dcl16-c.NewSuffixes
    value: "L;LL;LU;LLU"
  - key: google-readability-braces-around-statements.ShortStatementLines
    value: "1"
  - key: cppcoreguidelines-non-private-member-variables-in-classes.IgnoreClassesWithAllMemberVariablesBeingPublic
    value: "true"
  - key: google-readability-namespace-comments.SpacesBeforeComments
    value: "2"
  - key: modernize-loop-convert.MaxCopySize
    value: "16"
  - key: modernize-pass-by-value.IncludeStyle
    value: llvm
  - key: modernize-use-nullptr.NullMacros
    value: "NULL"
  - key: llvm-qualified-auto.AddConstToQualified
    value: "false"
  - key: modernize-loop-convert.NamingStyle
    value: CamelCase
  - key: llvm-else-after-return.WarnOnUnfixable
    value: "false"
  - key: google-readability-function-size.StatementThreshold
    value: "800"
```


### 效果

![](/assets/image/setup/vscode/a02_base_setting/001.png)

### 关键部分说明

#### cmake 相关

`cmake` 部分主要关注如下几点
1. `CMakePresets.json` 中配置了 `binaryDir`
2. `CMakePresets.json` 中单独配置了 `windows` 环境下的一些路径
3. `CMakeLists.txt` 中配置了可执行程序的生成路径

#### vscode 相关

##### settings.json

有两个关键配置

```json
{
    "clang-tidy.buildPath": "build/ninja-release",
    "clangd.arguments": [
        //compile_commands.json目录位置
        "--compile-commands-dir=build/ninja-release",
    ]
}
```

上述配置指定了 `clangd` 与 `clang-tidy` 如何寻找 `compile_commands.json` 文件位置

##### 需要安装哪些插件

在 `.vscode` 目录中添加一个文件: `extensions.json`, 内容如下:
```json
{
    "recommendations": [
        "xaver.clang-format",
        "llvm-vs-code-extensions.vscode-clangd",
        "josetr.cmake-language-support-vscode",
        "vadimcn.vscode-lldb",
        "CS128.cs128-clang-tidy",
        "cschlosser.doxdocgen",
        "usernamehw.errorlens",
        "mhutchie.git-graph",
        "redjue.git-commit-plugin",
        "spencerwmiles.vscode-task-buttons",
        "spmeesseman.vscode-taskexplorer",
        "wayou.vscode-todo-highlight",
        "wayou.vscode-todo-highlight"
    ]
}
```

然后在扩展的搜索框输入: `@recommended`

即可看到我使用的扩展

![](/assets/image/setup/vscode/a02_base_setting/002.png)


##### 各个扩展的作用


| 插件名称 | 作用 |
|    :----:   | :---: |
| xaver.clang-format | clang-format 格式化代码 |
| llvm-vs-code-extensions.vscode-clangd | clangd lsp, 代码提示 |
| josetr.cmake-language-support-vscode | cmake 代码提示与格式化 |
| vadimcn.vscode-lldb | 代码调试 |
| CS128.cs128-clang-tidy | 代码检查 |
| cschlosser.doxdocgen | 生成 doxygen 代码片段 |
| usernamehw.errorlens | 将错误显示在当前行 |
| mhutchie.git-graph | git 神器 |
| redjue.git-commit-plugin | git commit 规范插件 |
| spencerwmiles.vscode-task-buttons | 将 `tasks.json` 定义的任务在任务栏上以按钮的形式展示, 方便一键运行任务 |
| spmeesseman.vscode-taskexplorer | 任务浏览器 |
| wayou.vscode-todo-highlight | todo 相关 |
| wayou.vscode-todo-highlight | todo 相关 |



## lua 

### 为什么要配置 lua 环境

在我写这篇博客之前, 我找了很多编译原理的教程, 最终找到一个比较合适的

> youtube: [https://www.youtube.com/playlist?list=PLwHDUsnIdlMy52QnKX-2Unl6Hmfm9A6jt](https://www.youtube.com/playlist?list=PLwHDUsnIdlMy52QnKX-2Unl6Hmfm9A6jt)
> 
> github: [https://github.com/alexjercan/cool-compiler](https://github.com/alexjercan/cool-compiler)

此教程完成了一个基本完整的面向对象的语言, 例如这个语言的 Hello World 如下:

```
class Main {
    main(): Object {
        new IO.out_string("Hello, World!\n")
    };
};
```

而我之后要做的不仅仅是一门编程语言, 还包括代码编辑器与构建工具, 见识过 `nvim` 与 `xmake` 之后, 我认为使用 `lua` 作为配置文件的脚本语言是一个很好的选择

### 安装哪些插件

1. `sumneko.lua`
2. `tangzx.emmylua`

搜索 `tangzx.emmylua`

![](/assets/image/setup/vscode/a02_base_setting/003.png)


### windows 调试

#### main.lua
```lua
local a = 1
a = 2
a = 3
a = 4
print(a)
```

#### launch.json
```json
{
    // 使用 IntelliSense 了解相关属性。 
    // 悬停以查看现有属性的描述。
    // 欲了解更多信息，请访问: https://go.microsoft.com/fwlink/?linkid=830387
    "version": "0.2.0",
    "configurations": [
        {
            "type": "emmylua_launch",
            "request": "launch",
            "name": "启动并附加程序",
            "program": "D:/program/lua/lua542/bin/lua54.exe",
            "workingDir": "${workspaceRoot}",
            "arguments": [
                "${workspaceRoot}/main.lua"
            ],
            "newWindow": false
        }
    ]
}
```

#### 效果
![](/assets/image/setup/vscode/a02_base_setting/004.png)


### linux 调试

#### main.lua
```lua
package.cpath = package.cpath .. ";/home/laolang/program/code/data/extensions/tangzx.emmylua-0.7.1-linux-x64/debugger/emmy/linux/emmy_core.so"
local dbg = require("emmy_core")
dbg.tcpListen("localhost", 9966)
dbg.waitIDE()

local a = 1
a = 2
a = 3
a = 4
print(a)
```

#### launch.json
```json
{
    // 使用 IntelliSense 了解相关属性。 
    // 悬停以查看现有属性的描述。
    // 欲了解更多信息，请访问: https://go.microsoft.com/fwlink/?linkid=830387
    "version": "0.2.0",
    "configurations": [
        {
            "type": "emmylua_new",
            "request": "launch",
            "name": "EmmyLua New Debug",
            "host": "localhost",
            "port": 9966,
            "ext": [
                ".lua",
                ".lua.txt",
                ".lua.bytes"
            ],
            "ideConnectDebugger": true
        }
    ]
}
```

#### 注意

> 1. 调试前需要先在终端运行起来
> 2. `/home/laolang/program/code/data/extensions/tangzx.emmylua-0.7.1-linux-x64/debugger/emmy/linux/emmy_core.so` 需要修改为自己的位置

#### 效果
![](/assets/image/setup/vscode/a02_base_setting/005.png)

#### 参考

[Debugging on Mac?](https://github.com/EmmyLua/VSCode-EmmyLua/issues/58)

[Debugger: breakpoint are not working when ide connect to lua](https://github.com/EmmyLua/VSCode-EmmyLua/issues/81)

