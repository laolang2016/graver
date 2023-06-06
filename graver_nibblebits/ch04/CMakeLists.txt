cmake_minimum_required(VERSION 3.0)

project(graver C)

set(CMAKE_C_STANDARD 17)
set(CMAKE_C_STANDARD_REQUIRED True)
set(CMAKE_C_EXTENSIONS OFF)

include_directories(${PROJECT_SOURCE_DIR})
aux_source_directory(. SRCS)

set(EXECUTABLE_OUTPUT_PATH ${CMAKE_BINARY_DIR}/bin)

add_executable(${PROJECT_NAME} ${SRCS})
