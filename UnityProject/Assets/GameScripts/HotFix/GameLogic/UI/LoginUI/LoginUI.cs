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

            NetworkManager.Instance.Init();
            NetworkManager.Instance.Connect();
        }
        
        #endregion
    }
}
