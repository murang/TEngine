using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class BattleStateLose : FsmState<BattleManager>
    {
        protected override void OnEnter(IFsm<BattleManager> fsm)
        {
        }

        protected override void OnLeave(IFsm<BattleManager> fsm, bool isShutdown)
        {
        }

    }
}
