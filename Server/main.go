package main

import (
	_ "server/pb/pb"

	"github.com/murang/potato"
	"github.com/murang/potato/log"
	"github.com/murang/potato/net"
)

func main() {
	potato.SetNetConfig(&net.Config{
		SessionStartId: 0,
		ConnectLimit:   1000,
		Timeout:        30,
		Codec:          &net.PbCodec{}, // 框架内置JsonCodec和PbCodec 可以实现ICodec接口来实现自定义消息编解码
		MsgHandler:     &Handler{},     // 需要用户自己实现IMsgHandler 用于处理消息
	})
	// 网络监听器 支持tcp/kcp/ws
	ln, _ := net.NewListener("kcp", ":10086")
	// 添加网络监听器 可支持同时接收多个监听器消息 统一由MsgHandler处理
	potato.GetNetManager().AddListener(ln)

	potato.Start(func() bool { // 初始化app 入参为启动函数 在初始化所有组件后执行
		log.Logger.Info("all module started, server start")
		return true
	})
	potato.Run() // 开始update 所有组件开始tick 主线程阻塞
	potato.End(func() { // 主线程开始退出 所有组件销毁后执行入参函数
		log.Logger.Info("all module stopped, server stop")
	})
}
