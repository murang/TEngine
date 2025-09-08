using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class LogicStateWait : FsmState<IBattleLogic>
    {
        protected override void OnEnter(IFsm<IBattleLogic> fsm)
        {
            var logic = fsm.Owner;
            GameEvent.Get<IEventBattleLogic>().NewDrop(logic.NewDrop());
        }
    }
}
