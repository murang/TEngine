using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class LogicStateMatch : FsmState<IBattleLogic>
    {
        private IFsm<IBattleLogic> _fsm;
        private IBattleLogic _logic;
        
        protected override void OnEnter(IFsm<IBattleLogic> fsm)
        {
            GameEvent.AddEventListener(IEventBattle_Event.MatchEnd, MatchLoop);
            
            _fsm = fsm;
            _logic = fsm.Owner;
            MatchLoop();
        }

        protected override void OnLeave(IFsm<IBattleLogic> fsm, bool isShutdown)
        {
            GameEvent.RemoveEventListener(IEventBattle_Event.MatchEnd, MatchLoop);
        }

        void MatchLoop()
        {
            var matchList = _logic.Match();
            if (matchList.Count > 0)
            {
                GameEvent.Get<IEventBattle>().MatchStart(matchList);
            }
            else
            {
                // 没消除的就产生新的
                ChangeState<LogicStateReady>(_fsm);
            }
        }
    }
}
