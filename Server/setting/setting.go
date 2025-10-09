package setting

import (
	"github.com/spf13/viper"
)

var Main *Setting

func init() {
	s, err := loadConfig("setting.yaml")
	if err != nil {
		panic(err)
	}
	Main = s
}

func loadConfig(path string) (*Setting, error) {
	v := viper.New()
	v.SetConfigFile(path)
	v.SetConfigType("yaml")
	if err := v.ReadInConfig(); err != nil {
		return nil, err
	}

	var cfg Setting
	if err := v.Unmarshal(&cfg); err != nil {
		return nil, err
	}
	return &cfg, nil
}
