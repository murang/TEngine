package main

import (
	"fmt"
	"server/app/msg"
	"server/db"
	_ "server/pb/pb"
	"server/setting"

	"github.com/murang/potato"
	"github.com/murang/potato/net"
)

func main() {
	potato.SetNetConfig(&net.Config{
		SessionStartId: 0,
		ConnectLimit:   1000,
		Timeout:        30,
		Codec:          &net.PbCodec{},
		MsgHandler:     &msg.Handler{},
	})
	ln, _ := net.NewListener("ws", fmt.Sprintf(":%d", setting.Main.Port))
	potato.GetNetManager().AddListener(ln)

	db.Init()

	potato.Start(nil)
	potato.Run()
	potato.End(nil)
}
