package setting

type Setting struct {
	ProjectName string `mapstructure:"project_name"` // 项目名称

	Port int `mapstructure:"port"` // 端口

	DB Mysql `mapstructure:"db"` // 数据库

	Log Log `mapstructure:"log"` // 日志

	WeChat `mapstructure:"wechat"`
	TikTok `mapstructure:"tiktok"`
}

type Mysql struct {
	Host     string `mapstructure:"host"`
	Port     int    `mapstructure:"port"`
	User     string `mapstructure:"user"`
	Password string `mapstructure:"password"`
	DbName   string `mapstructure:"db_name"`
}

type Redis struct {
	Host     string `mapstructure:"host"`
	Port     int    `mapstructure:"port"`
	Password string `mapstructure:"password"`
	Db       int    `mapstructure:"db"`
}

type Log struct {
	IsFile     bool   `mapstructure:"is_file"`
	Path       string `mapstructure:"path"`
	FileName   string `mapstructure:"file_name"`
	Level      string `mapstructure:"level"`
	MaxSize    int    `mapstructure:"max_size"`
	MaxBackups int    `mapstructure:"max_backups"`
	MaxAge     int    `mapstructure:"max_age"`
	Compress   bool   `mapstructure:"compress"`
	FocusError bool   `mapstructure:"focus_error"`
}

type LanFei struct {
	AppID     string `mapstructure:"app_id"`
	AppSecret string `mapstructure:"app_secret"`
	Check     string `mapstructure:"check"`
	Loc       string `mapstructure:"loc"`
}

type WeChat struct {
	Active    bool   `mapstructure:"active"`
	AppID     string `mapstructure:"app_id"`
	AppSecret string `mapstructure:"app_secret"`
	Prefix    string `mapstructure:"prefix"`
}

type TikTok struct {
	Active    bool   `mapstructure:"active"`
	AppID     string `mapstructure:"app_id"`
	AppSecret string `mapstructure:"app_secret"`
	Prefix    string `mapstructure:"prefix"`
}

type ThinkingData struct {
	Active    bool   `mapstructure:"active"`
	AppID     string `mapstructure:"app_id"`
	Prefix    string `mapstructure:"prefix"`
	ServerUrl string `mapstructure:"server_url"`
}
