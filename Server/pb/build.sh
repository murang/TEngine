#!/bin/bash

# 获取当前脚本所在的目录
SCRIPT_DIR=$(dirname "$0")

# 将当前工作目录切换到脚本所在的目录
cd "$SCRIPT_DIR" || exit

# 生成go代码
protoc -I. --go_out=. \
  --go-vtproto_out=. \
  --go-vtproto_opt=features=marshal+unmarshal+size \
  --autoregister_out=. game.proto

# 生成c#代码
protoc -I. --csharp_out=./cs game.proto

# 复制到客户端
cp -r ./cs ../../UnityProject/Assets/GameScripts/HotFix/GameProto/Pb