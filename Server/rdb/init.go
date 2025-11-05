package rdb

import (
	"context"
	"fmt"
	"server/setting"
	"time"

	"github.com/go-redsync/redsync/v4"
	"github.com/go-redsync/redsync/v4/redis/goredis/v9"
	"github.com/murang/potato/log"
	"github.com/redis/go-redis/v9"
)

var (
	Client redis.UniversalClient
	rs     *redsync.Redsync
)

func Init() {
	opt := &redis.UniversalOptions{
		Addrs: []string{fmt.Sprintf("%s:%d", setting.Main.Redis.Host, setting.Main.Redis.Port)},
		DB:    setting.Main.Redis.Db,
	}
	if setting.Main.Redis.Password != "" {
		opt.Password = setting.Main.Redis.Password
	}
	Client = redis.NewUniversalClient(opt)
	rs = redsync.New(goredis.NewPool(Client))

	if err := Client.Ping(context.Background()).Err(); err != nil {
		log.Sugar.Errorf("redis init error: %v", err)
		panic(err)
	}
	log.Sugar.Info("Redis init success!")
}

func GetLocker(key string) *redsync.Mutex {
	return rs.NewMutex(key,
		redsync.WithRetryDelay(100*time.Millisecond),
		redsync.WithTries(0),
		redsync.WithExpiry(time.Second*1))
}
