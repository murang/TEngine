using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class BattleStateRunning : FsmState<BattleManager>
    {
        protected override void OnInit(IFsm<BattleManager> fsm)
        {
            Log.Debug("BattleStateRunning OnInit");
        }

        protected override void OnEnter(IFsm<BattleManager> fsm)
        {
            Log.Debug("BattleStateRunning OnEnter");
        }
    }
}
