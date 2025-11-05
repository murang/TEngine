package work

import (
	"server/pb/pb"
	"server/rdb"
)

func (a *Agent) GetLevelDetail(level int32) *pb.LevelDetail {
	return &pb.LevelDetail{
		Level: level,
		Ranks: rdb.GetRanks(level),
	}
}
