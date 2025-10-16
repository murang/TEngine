using UnityEngine;
using UnityEngine.UI;
using TEngine;

namespace GameLogic
{
    [Window(UILayer.UI,location:"BattleUI")]
    class BattleUI : UIWindow
    {
        #region 脚本工具生成的代码
        private Button _btnStart;
        private Button _btnRestart;
        protected override void ScriptGenerator()
        {
            _btnStart = FindChildComponent<Button>("m_btnStart");
            _btnRestart = FindChildComponent<Button>("m_btnRestart");
            _btnStart.onClick.AddListener(OnClickStartBtn);
            _btnRestart.onClick.AddListener(OnClickRestartBtn);
        }
        #endregion

        #region 事件
        private void OnClickStartBtn()
        {
            GameEvent.Get<IEventBattle>().StartBattle();
        }
        private void OnClickRestartBtn()
        {
            GameEvent.Get<IEventBattle>().RestartBattle();
        }
        #endregion

    }
}
