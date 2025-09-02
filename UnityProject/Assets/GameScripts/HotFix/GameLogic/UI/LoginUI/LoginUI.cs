using UnityEngine;
using UnityEngine.UI;
using TEngine;

namespace GameLogic
{
    [Window(UILayer.UI,location:"LoginUI")]
    class LoginUI : UIWindow
    {
        #region 脚本工具生成的代码
        private InputField _inputAccount;
        private InputField _inputPassword;
        private Button _btnLogin;
        protected override void ScriptGenerator()
        {
            _inputAccount = FindChildComponent<InputField>("m_inputAccount");
            _inputPassword = FindChildComponent<InputField>("m_inputPassword");
            _btnLogin = FindChildComponent<Button>("m_btnLogin");
            _btnLogin.onClick.AddListener(OnClickLoginBtn);
        }
        #endregion

        #region 事件
        private void OnClickLoginBtn()
        {
            var item = ConfigSystem.Instance.Tables.TbItem.Get(10000);
            Log.Error(item.Desc);
        }
        #endregion

    }
}