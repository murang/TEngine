using System;
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
                
            var msg = new Pb.C2S_Hello
            {
                Name = "niceman"
            };
            MsgDispatcher.Instance.RegisterMsgReceiver<Pb.C2S_Hello>(OnC2SHello);
            var codec = new CodecPb();
            var option = new ConnOption
            {
                Host = "127.0.0.1",
                Port = 10086,
                Codec = codec,
                ConnectTimeout = 5,
                Timeout = 30
            };
            var conn = new KcpConn(option);
            conn.OnConnected += conn =>
            {
                Log.Debug("Connected");
                conn.Send(msg);
            };
            conn.OnMessage += (conn, o) =>
            {
                MsgDispatcher.Instance.DispatchMsg(o);
            };
            conn.OnException += (conn1, exception) =>
            {
                Log.Error(exception.Message);
            };
            conn.Connect();
        }
        
        void OnC2SHello(Pb.C2S_Hello msg)
        {
            Log.Debug("OnC2SHello =====> : " + msg);
        }
        #endregion
    }
}
