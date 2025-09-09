using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class LogicStateWait : FsmState<IBattleLogic>
    {
        private IBattleLogic _logic;
        protected override void OnEnter(IFsm<IBattleLogic> fsm)
        {
            _logic = fsm.Owner;
            GameEvent.Get<IEventBattle>().ShowNewDrop(_logic.NewDrop());

            GameEvent.AddEventListener<int>(IEventBattleLogic_Event.TouchGrid, TouchGrid);
        }

        protected override void OnLeave(IFsm<IBattleLogic> fsm, bool isShutdown)
        {
            GameEvent.RemoveEventListener<int>(IEventBattleLogic_Event.TouchGrid, TouchGrid);
        }

        void TouchGrid(int x)
        {
            _logic?.DropDown(x);
        }
    }
}
