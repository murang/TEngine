using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class BattleStateRunning : FsmState<BattleManager>
    {
        private BattleManager _manager;
        protected override void OnInit(IFsm<BattleManager> fsm)
        {
            _manager = fsm.Owner;
        }

        protected override void OnEnter(IFsm<BattleManager> fsm)
        {
            GameEvent.AddEventListener<int>(IEventBattle_Event.ShowNewDrop, ShowNewDrop);
            _manager.StartGame();
            GameModule.UI.HideUI<BattleUI>();
        }

        protected override void OnLeave(IFsm<BattleManager> fsm, bool isShutdown)
        {
            GameEvent.RemoveEventListener<int>(IEventBattle_Event.ShowNewDrop, ShowNewDrop);
        }

        void ShowNewDrop(int num)
        {
            _manager.ShowNewDrop(num);
        }
    }
}
