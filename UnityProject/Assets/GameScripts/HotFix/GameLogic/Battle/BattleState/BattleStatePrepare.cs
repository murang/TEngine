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
            BattleManager manager = fsm.Owner;
            manager.InitBattleLogic();
            ChangeState<BattleStateStart>(fsm);
        }
    }
}
