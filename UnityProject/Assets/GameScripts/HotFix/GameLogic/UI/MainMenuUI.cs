using UnityEngine;
using UnityEngine.UI;
using TEngine;

namespace GameLogic
{
    [Window(UILayer.UI,location:"MainMenuUI")]
    class MainMenuUI : UIWindow
    {
        #region 脚本工具生成的代码
        private Button _btnStartLevel;
        protected override void ScriptGenerator()
        {
            _btnStartLevel = FindChildComponent<Button>("m_btnStartLevel");
            _btnStartLevel.onClick.AddListener(OnClickStartLevelBtn);
        }
        #endregion

        #region 事件
        private void OnClickStartLevelBtn()
        {
            Log.Debug(ConfigSystem.Instance.Tables.TbMisc.DataList[0].HpRestoreSecond);
            
            // UIModule.Instance.CloseAll();
            // GameModule.Scene.LoadScene("battle");
            // UIModule.Instance.ShowUIAsync<BattleUI>();
        }
        #endregion

    }
}