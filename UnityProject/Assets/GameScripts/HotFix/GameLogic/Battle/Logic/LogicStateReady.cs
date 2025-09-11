using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class LogicStateReady : FsmState<IBattleLogic>
    {
        private IFsm<IBattleLogic> _fsm;
        private IBattleLogic _logic;
        protected override void OnEnter(IFsm<IBattleLogic> fsm)
        {
            GameEvent.AddEventListener<int>(IEventBattle_Event.TouchGrid, TouchGrid);
            
            _fsm = fsm;
            _logic = fsm.Owner;
            GameEvent.Get<IEventBattle>().ShowNewDrop(_logic.NewDrop());
        }

        protected override void OnLeave(IFsm<IBattleLogic> fsm, bool isShutdown)
        {
            GameEvent.RemoveEventListener<int>(IEventBattle_Event.TouchGrid, TouchGrid);
        }

        void TouchGrid(int x)
        {
            if (_logic.DropDown(x))
            {
                ChangeState<LogicStateWait>(_fsm);
            }
        }
    }
}
