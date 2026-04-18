package work

import (
	"server/db"
	"server/pb/pb"
	"server/rdb"
)

func GetLevelDetail(agent *Agent, msg *pb.C2S_GetLevelDetail) {
	detail := &pb.S2C_GetLevelDetail{
		Detail: agent.GetLevelDetail(agent.User.Model.Level),
	}
	agent.SendMsg(detail)
}

func FinishLevel(agent *Agent, msg *pb.C2S_FinishLevel) {
	if agent.User.Model.Level == msg.Level {
		agent.User.Model.Level++
		agent.MarkUser(db.UserColLevel)
	}

	rdb.SetUserScore(agent.User.Model.ID, msg.Level, msg.Score)

	detail := &pb.S2C_GetLevelDetail{
		Detail: agent.GetLevelDetail(agent.User.Model.Level),
	}
	agent.SendMsg(detail)
}
