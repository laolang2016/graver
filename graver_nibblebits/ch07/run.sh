#!/bin/bash
if [ -d build ]; then
    rm -rf build
fi

cmake -S . -G "Unix Makefiles" -B build
cmake --build build
cp test.txt build/bin/test.c
cd build/bin && ./graver