#!/bin/bash

export PATH=$PATH:/usr/local/share/dotnet

cd "$(dirname "$0")"
echo "当前目录: $(pwd)"

export WORKSPACE="$(realpath ../../)"
export LUBAN_DLL="${WORKSPACE}/Tools/Luban/Luban.dll"
export CONF_ROOT="$(pwd)"
export DATA_OUTPATH="${WORKSPACE}/UnityProject/Assets/AssetRaw/Configs/bytes/"
export CODE_OUTPATH="${WORKSPACE}/UnityProject/Assets/GameScripts/HotFix/GameProto/GameConfig/"

cp -R "${CONF_ROOT}/CustomTemplate/ConfigSystem.cs" \
   "${WORKSPACE}/UnityProject/Assets/GameScripts/HotFix/GameProto/ConfigSystem.cs"
cp -R "${CONF_ROOT}/CustomTemplate/ExternalTypeUtil.cs" \
    "${WORKSPACE}/UnityProject/Assets/GameScripts/HotFix/GameProto/ExternalTypeUtil.cs"

dotnet "${LUBAN_DLL}" \
    -t client \
    -c cs-bin \
    -d bin \
    --conf "${CONF_ROOT}/luban.conf" \
    --customTemplateDir "${CONF_ROOT}/CustomTemplate/CustomTemplate_Client_LazyLoad" \
    -x code.lineEnding=crlf \
    -x outputCodeDir="${CODE_OUTPATH}" \
    -x outputDataDir="${DATA_OUTPATH}"

export SERVER_DATA_OUTPATH="${WORKSPACE}/Server/configs/"
export SERVER_CODE_OUTPATH="${WORKSPACE}/Server/cfg"

dotnet "${LUBAN_DLL}" \
    -t server \
    -d json \
    --conf "${CONF_ROOT}/luban.conf" \
    -x outputDataDir="${SERVER_DATA_OUTPATH}"
