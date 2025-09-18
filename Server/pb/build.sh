#!/bin/bash

# 获取当前脚本所在的目录
SCRIPT_DIR=$(dirname "$0")

# 将当前工作目录切换到脚本所在的目录
cd "$SCRIPT_DIR" || exit

# 生成go代码
protoc -I. --go_out=. \
  --go-vtproto_out=. \
  --go-vtproto_opt=features=marshal+unmarshal+size \
  --autoregister_out=. \
  --descriptor_set_out=msg.desc \
  game.proto
protoc -I. --go_out=. \
    --go-vtproto_out=. \
    --go-vtproto_opt=features=marshal+unmarshal+size \
    def.proto

# 生成c#代码
protoc -I. --csharp_out=./cs *.proto

# 生成客户端id注册
go run gen_reg_cs.go

# 复制到客户端
cp -r ./cs/* ../../UnityProject/Assets/GameScripts/HotFix/GameLogic/Network/Pb