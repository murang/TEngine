using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class BattleStateStart : FsmState<BattleManager>
    {
        protected override void OnInit(IFsm<BattleManager> fsm)
        {
            Log.Debug("BattleStateStart OnInit");
        }

        protected override void OnEnter(IFsm<BattleManager> fsm)
        {
            Log.Debug("BattleStateStart OnEnter");
        }
    }
}
