using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class BattleStateStart : FsmState<BattleManager>
    {
        private IFsm<BattleManager> _fsm;
        protected override void OnInit(IFsm<BattleManager> fsm)
        {
            _fsm = fsm;
        }

        protected override void OnEnter(IFsm<BattleManager> fsm)
        {
            GameEvent.AddEventListener(IEventBattle_Event.StartBattle, StartGame);
        }

        protected override void OnLeave(IFsm<BattleManager> fsm, bool isShutdown)
        {
            GameEvent.RemoveEventListener(IEventBattle_Event.StartBattle, StartGame);
        }

        void StartGame()
        {
            ChangeState<BattleStateRunning>(_fsm);
        }
    }
}
