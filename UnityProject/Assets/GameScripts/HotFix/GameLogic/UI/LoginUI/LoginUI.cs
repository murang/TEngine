using System;
using Pb;
using UnityEngine;
using UnityEngine.UI;
using TEngine;

namespace GameLogic
{
    [Window(UILayer.UI,location:"LoginUI")]
    class LoginUI : UIWindow
    {
        #region 脚本工具生成的代码
        private Button _btnLogin;
        protected override void ScriptGenerator()
        {
            _btnLogin = FindChildComponent<Button>("m_btnLogin");
            _btnLogin.onClick.AddListener(OnClickLoginBtn);
        }
        #endregion

        #region 事件
        private void OnClickLoginBtn()
        {
            // Log.Debug(ConfigSystem.Instance.Tables.TbItem.Get(10000).Desc);
            // UIModule.Instance.CloseAll();
            // GameModule.Scene.LoadScene("battle");
            // UIModule.Instance.ShowUIAsync<BattleUI>();
            
            // BeatManager.Instance.LoadClip("rythem", 117.45383d,2.53097d);

            var ws = new WsConn(new ConnOption
            {
                Domain = "ws://127.0.0.1:10086",
                Codec = new CodecPb(),
            });

            ws.OnConnected += conn =>
            {
                Log.Debug("OnConnected");
                conn.Send(new C2S_Login
                {
                    LoginType = 666,
                    Code = "nice"
                });
            };
            ws.OnDisconnected += conn =>
            {
                Log.Debug("OnDisconnected");
            };
            ws.OnException += (conn, e) =>
            {
                Log.Debug("OnException" , e.Message);
            };
            ws.OnMessage += (conn, o) =>
            {
                Log.Debug("OnMessage" , o);
            };
            ws.Connect();
        }
        
        #endregion
    }
}
