FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /tree-sitter

COPY ./ /tree-sitter

RUN apt update
RUN yes | apt install build-essential
RUN yes | apt-get install python3
RUN yes | apt-get install vim
RUN yes | apt-get install procps

ENV LD_LIBRARY_PATH=/tree-sitter/TreeSitter
ENV C_INCLUDE_PATH=/tree-sitter/tree-sitter/lib/include
ENV CPLUS_INCLUDE_PATH=/tree-sitter/tree-sitter/lib/include

RUN python3 buildLinux.py

WORKDIR /tree-sitter/TreeSitter.Test
RUN dotnet test
WORKDIR /tree-sitter