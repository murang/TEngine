package wx

import (
	"server/setting"

	"github.com/murang/potato/log"
	"github.com/murang/potato/util"
)

func Init() {
	if setting.Main.WeChat.Active {
		util.GoSafe(tickToken)
		log.Sugar.Info("WX init success!")
	}
}
