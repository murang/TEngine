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
            GameEvent.AddEventListener<DropData>(IEventBattle_Event.ShowNewDrop, _manager.ShowNewDrop);
            GameEvent.AddEventListener<int, int>(IEventBattle_Event.DropDownStart, _manager.DropDown);
            GameEvent.AddEventListener<List<DropData>>(IEventBattle_Event.MatchStart, _manager.MatchStart);
            
            _manager.StartGame();
            GameModule.UI.HideUI<BattleUI>();
        }

        protected override void OnLeave(IFsm<BattleManager> fsm, bool isShutdown)
        {
            GameEvent.RemoveEventListener<DropData>(IEventBattle_Event.ShowNewDrop, _manager.ShowNewDrop);
            GameEvent.RemoveEventListener<int, int>(IEventBattle_Event.DropDownStart, _manager.DropDown);
            GameEvent.RemoveEventListener<List<DropData>>(IEventBattle_Event.MatchStart, _manager.MatchStart);
        }
    }
}
