package wx

import (
	"encoding/json"
	"fmt"
	"io"
	"net/http"
	"server/setting"

	"github.com/murang/potato/log"
)

const sessionUrl = "https://api.weixin.qq.com/sns/jscode2session"

type WXSession struct {
	OpenId     string `json:"openid"`
	SessionKey string `json:"session_key"`
	UnionId    string `json:"unionid"`
	ErrCode    int    `json:"errcode"`
	ErrMsg     string `json:"errmsg"`
}

func Login(code string) *WXSession {
	getUrl := fmt.Sprintf("%s?appid=%s&secret=%s&js_code=%s&grant_type=authorization_code", sessionUrl, setting.Main.WeChat.AppID, setting.Main.WeChat.AppSecret, code)
	resp, err := http.Get(getUrl)
	if err != nil {
		log.Sugar.Errorf("wx login auth err: %s url: %s", err.Error(), getUrl)
		return nil
	}
	defer resp.Body.Close()
	body, err := io.ReadAll(resp.Body)
	if err != nil {
		log.Sugar.Errorf("wx login read body err: %s", err.Error())
		return nil
	}

	var res WXSession
	err = json.Unmarshal(body, &res)
	if err != nil {
		log.Sugar.Errorf("wx login unmarshal err: %s", err.Error())
		return nil
	}
	if res.ErrCode > 0 {
		log.Sugar.Errorf("wx login err: %d msg: %s", res.ErrCode, res.ErrMsg)
		return nil
	}

	return &res
}
