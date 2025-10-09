package wx

import (
	"bytes"
	"encoding/json"
	"io"
	"net/http"
	"server/setting"
	"time"

	"github.com/murang/potato/log"
)

var AccessToken string

const tokenUrl = "https://api.weixin.qq.com/cgi-bin/stable_token"

func tickToken() {
	if newToken := getAccessToken(); newToken != "" {
		AccessToken = newToken
	}
	ticker := time.NewTicker(5 * time.Minute)
	for {
		select {
		case <-ticker.C:
			if newToken := getAccessToken(); newToken != "" {
				AccessToken = newToken
			}
		}
	}
}

type AccessTokenReq struct {
	GrantType    string `json:"grant_type"`
	AppId        string `json:"appid"`
	Secret       string `json:"secret"`
	ForceRefresh bool   `json:"force_refresh"`
}

type AccessTokenRes struct {
	AccessToken string `json:"access_token"`
	ExpiresIn   int    `json:"expires_in"`
}

func getAccessToken() string {
	req := AccessTokenReq{
		GrantType: "client_credential",
		AppId:     setting.Main.WeChat.AppID,
		Secret:    setting.Main.WeChat.AppSecret,
	}
	reqData, _ := json.Marshal(req)

	client := &http.Client{}
	httpReq, _ := http.NewRequest("POST", tokenUrl, bytes.NewBuffer(reqData))
	httpReq.Header.Set("Content-Type", "application/json")
	resp, err := client.Do(httpReq)
	defer resp.Body.Close()
	body, err := io.ReadAll(resp.Body)
	if err != nil {
		log.Sugar.Errorf("wx getAccessToken err: %s", err.Error())
	} else {
		var res AccessTokenRes
		err = json.Unmarshal(body, &res)
		if err != nil {
			log.Sugar.Errorf("wx getAccessToken res unmarshal err: %s", err.Error())
		}
		return res.AccessToken
	}

	return ""
}
