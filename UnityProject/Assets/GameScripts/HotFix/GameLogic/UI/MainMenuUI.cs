using System;
using UnityEngine;
using UnityEngine.UI;
using TEngine;
using TMPro;

namespace GameLogic
{
    [Window(UILayer.UI,location:"MainMenuUI")]
    class MainMenuUI : UIWindow
    {
        #region 脚本工具生成的代码
        private Button _btnStartLevel;
        private TextMeshProUGUI _tmpHp;
        private TextMeshProUGUI _tmpHpRestoreTime;
        protected override void ScriptGenerator()
        {
            _btnStartLevel = FindChildComponent<Button>("m_btnStartLevel");
            _tmpHp = FindChildComponent<TextMeshProUGUI>("m_tmpHp");
            _tmpHpRestoreTime = FindChildComponent<TextMeshProUGUI>("m_tmpHpRestoreTime");
            _btnStartLevel.onClick.AddListener(OnClickStartLevelBtn);
        }
        #endregion

        #region 事件
        private void OnClickStartLevelBtn()
        {
        }
        #endregion

        protected override void OnUpdate()
        {
            ShowHp();
        }

        void ShowHp()
        {
            var hp = ConfigSystem.Instance.Tables.TbMisc.DataList[0].HpMax;
            var tsHpFull = Player.Instance.data.AssetsInfo.HpFullTime;
            var tsNow = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
            if (tsHpFull > tsNow)
            {
                var waitHp = (tsHpFull - tsNow) / (ConfigSystem.Instance.Tables.TbMisc.DataList[0].HpRestoreSecond * 1000f);
                _tmpHpRestoreTime.text = waitHp.ToString("F2");
            }
        }
    }
}