using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class BattleStatePrepare : FsmState<BattleManager>
    {

        protected override void OnEnter(IFsm<BattleManager> fsm)
        {
            Log.Debug("BattleStatePrepare OnEnter");
            ChangeState<BattleStateStart>(fsm);
        }
    }
}
