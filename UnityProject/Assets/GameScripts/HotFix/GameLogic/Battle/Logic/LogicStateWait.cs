using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class LogicStateWait : FsmState<IBattleLogic>
    {
        private IFsm<IBattleLogic> _fsm;
        private IBattleLogic _logic;
        protected override void OnEnter(IFsm<IBattleLogic> fsm)
        {
            _fsm = fsm;
            _logic = fsm.Owner;
            GameEvent.Get<IEventBattle>().ShowNewDrop(_logic.NewDrop());

            GameEvent.AddEventListener<int>(IEventBattle_Event.TouchGrid, TouchGrid);
            GameEvent.AddEventListener(IEventBattle_Event.DropDownEnd, DropDownEnd);
        }

        protected override void OnLeave(IFsm<IBattleLogic> fsm, bool isShutdown)
        {
            GameEvent.RemoveEventListener<int>(IEventBattle_Event.TouchGrid, TouchGrid);
            GameEvent.RemoveEventListener(IEventBattle_Event.DropDownEnd, DropDownEnd);
        }

        void TouchGrid(int x)
        {
            _logic?.DropDown(x);
        }

        void DropDownEnd()
        {
            ChangeState<LogicStateMatch>(_fsm);
        }
    }
}
