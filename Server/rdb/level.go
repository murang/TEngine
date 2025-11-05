package rdb

import (
	"context"
	"fmt"
	"server/pb/pb"

	"github.com/murang/potato/log"
	"github.com/redis/go-redis/v9"
)

const KeyLevelRank = "levelRank"
const LevelRankCount = 100

func SetUserScore(userId uint, level int32, score int64) {
	key := fmt.Sprintf("%s:%d", KeyLevelRank, level)
	err := Client.ZAdd(context.Background(), key, redis.Z{
		Score:  float64(score),
		Member: userId,
	}).Err()
	if err != nil {
		log.Sugar.Errorf("set user score error: %v", err)
	}
}

func GetRanks(level int32) []*pb.Rank {
	key := fmt.Sprintf("%s:%d", KeyLevelRank, level)
	res, err := Client.ZRevRangeWithScores(context.Background(), key, 0, LevelRankCount-1).Result()
	if err != nil {
		log.Sugar.Errorf("get ranks error: %v", err)
		return nil
	}
	var ranks []*pb.Rank
	for _, v := range res {
		ranks = append(ranks, &pb.Rank{
			UserId: uint32(v.Member.(uint)),
			Score:  int64(v.Score),
		})
	}
	return ranks
}
