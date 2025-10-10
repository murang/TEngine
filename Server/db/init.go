package db

import (
	"fmt"
	"server/constant"
	"server/setting"
	"time"

	"github.com/murang/potato/log"
	"gorm.io/driver/mysql"
	"gorm.io/gorm"
	"gorm.io/gorm/schema"
)

var migrateModels = []interface{}{
	&User{},
}

var (
	Client *gorm.DB
)

func Init() {
	dsn := fmt.Sprintf("%s:%s@tcp(%s:%d)/%s?charset=utf8mb4&parseTime=True&loc=Local",
		setting.Main.DB.User,
		setting.Main.DB.Password,
		setting.Main.DB.Host,
		setting.Main.DB.Port,
		setting.Main.DB.DbName)
	dia := mysql.New(mysql.Config{
		DSN:                       dsn,
		DefaultStringSize:         256,
		DisableDatetimePrecision:  true,
		DontSupportRenameIndex:    true,
		DontSupportRenameColumn:   true,
		SkipInitializeWithVersion: true,
	})
	db, err := gorm.Open(dia, &gorm.Config{
		NamingStrategy: schema.NamingStrategy{SingularTable: true}, // 表名保持单数
		PrepareStmt:    true,                                       // tps高 开启预编译
	})
	if err != nil {
		panic(err.Error())
	}
	mysqlDB, _ := db.DB()
	mysqlDB.SetMaxIdleConns(150)
	mysqlDB.SetMaxOpenConns(600)
	mysqlDB.SetConnMaxIdleTime(time.Minute * 20)
	mysqlDB.SetConnMaxLifetime(time.Minute * 60)
	Client = db
	err = Client.AutoMigrate(migrateModels...)
	if err != nil {
		log.Sugar.Errorf("mysql init error: %v", err)
		panic(err)
	}
	checkAlterTable()
	log.Sugar.Info("DB init success!")
}

func checkAlterTable() {
	var count int64
	if err := Client.Table("user").Count(&count).Error; err != nil || count == 0 {
		// 如果 user 表不存在或没有数据，设置 AUTO_INCREMENT 起始值
		Client.Exec(fmt.Sprintf("ALTER TABLE `user` AUTO_INCREMENT = %d", constant.UserIdStart))
	}
}
