package work

import (
	"server/db"
	"server/pb/pb"
	"server/wx"

	"github.com/murang/potato/log"
)

func Heartbeat(agent *Agent, msg *pb.C2S_Heartbeat) {
	agent.SendMsg(&pb.S2C_Heartbeat{})
}

func Login(agent *Agent, msg *pb.C2S_Login) {
	if msg.Code == "" {
		agent.SendMsg(&pb.S2C_Error{Code: pb.ErrCode_ArgumentInvalid})
		return
	}
	var userModel *db.User
	switch msg.LoginType {
	case 0: // 游客
		model, err := db.GetOrNewUserByDeviceId(msg.Code)
		if err != nil {
			log.Sugar.Errorf("get or new user by device id err: %s", err.Error())
			agent.SendMsg(&pb.S2C_Error{Code: pb.ErrCode_InternalServerError})
			return
		}
		userModel = model
	case 1: // 微信
		wxSession := wx.Login(msg.Code)
		if wxSession != nil {
			model, err := db.GetOrNewUserByWeChatId(wxSession.OpenId)
			if err != nil {
				log.Sugar.Errorf("get or new user by we chat id err: %s", err.Error())
				agent.SendMsg(&pb.S2C_Error{Code: pb.ErrCode_InternalServerError})
				return
			}
			userModel = model
		}
	case 2: // 抖音

	}

	if userModel != nil {
		agent.user = GetUserByModel(userModel)
		agent.SendMsg(&pb.S2C_Login{Data: agent.user.GetUserData()})
	} else {
		agent.SendMsg(&pb.S2C_Error{Code: pb.ErrCode_LoginFailed})
	}
}
