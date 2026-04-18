package db

import (
	"errors"
	"fmt"
	"time"

	"gorm.io/gorm"
)

const (
	UserColDeviceId   = "device_id"
	UserColWeChatId   = "we_chat_id"
	UserColTikTokId   = "tik_tok_id"
	UserColNickname   = "nickname"
	UserColIcon       = "icon"
	UserColCoin       = "coin"
	UserColLevel      = "level"
	UserColHpFullTime = "hp_full_time"
)

type User struct {
	Marks      map[string]struct{} `json:"-" gorm:"-"` // 需要更新的字段
	gorm.Model                     // userId就用基础model中的id
	DeviceId   string              `json:"deviceId" gorm:"index"`                                     // 设备id
	WeChatId   string              `json:"weChatId" gorm:"index"`                                     // 微信openId
	TikTokId   string              `json:"tikTokId" gorm:"index"`                                     // 抖音openId
	Nickname   string              `json:"nickname"`                                                  // 昵称
	Icon       string              `json:"icon"`                                                      // 头像
	Coin       int32               `json:"coin"`                                                      // 金币
	Level      int32               `json:"level"`                                                     // 关卡
	HpFullTime time.Time           `json:"hpFullTime" gorm:"type:datetime;default:CURRENT_TIMESTAMP"` // 血量满时间
}

func (u *User) Patch() {
	u.Marks = map[string]struct{}{}
}

// GetNickname 需要默认昵称
func (u *User) GetNickname() string {
	if u.Nickname == "" {
		return fmt.Sprintf("用户%d", u.ID)
	}
	return u.Nickname
}

func GetUserById(id uint64) (*User, error) {
	var user User
	if err := Client.Where("id = ?", id).First(&user).Error; err != nil {
		return nil, err
	}
	return &user, nil
}

func GetAndPatchUserById(id uint) (*User, error) {
	var user User
	if err := Client.Where("id = ?", id).First(&user).Error; err != nil {
		return nil, err
	}
	user.Patch()
	return &user, nil
}

func GetOrNewUserByDeviceId(deviceId string) (*User, error) {
	var user User
	if err := Client.Where("device_id = ?", deviceId).First(&user).Error; err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			user.DeviceId = deviceId
			if err = Client.Create(&user).Error; err != nil {
				return nil, err
			}
		} else {
			return nil, err
		}
	}
	return &user, nil
}

func GetOrNewUserByWeChatId(openId string) (*User, error) {
	var user User
	if err := Client.Where("we_chat_id = ?", openId).First(&user).Error; err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			user.WeChatId = openId
			if err = Client.Create(&user).Error; err != nil {
				return nil, err
			}
		} else {
			return nil, err
		}
	}
	return &user, nil
}

func GetOrNewUserByTikTokId(openId string) (*User, error) {
	var user User
	if err := Client.Select("id").Where("tik_tok_id = ?", openId).First(&user).Error; err != nil {
		if errors.Is(err, gorm.ErrRecordNotFound) {
			user.TikTokId = openId
			if err = Client.Create(&user).Error; err != nil {
				return nil, err
			}
		} else {
			return nil, err
		}
	}
	return &user, nil
}
