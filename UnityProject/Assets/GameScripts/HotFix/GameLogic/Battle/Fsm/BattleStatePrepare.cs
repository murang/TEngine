using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class BattleStatePrepare : FsmState<BattleManager>
    {
        private int _count = 0;
        
        protected override void OnInit(IFsm<BattleManager> fsm)
        {
            Log.Debug("BattleStatePrepare OnInit");
        }

        protected override void OnEnter(IFsm<BattleManager> fsm)
        {
            Log.Debug("BattleStatePrepare OnEnter");
        }

        protected override void OnLeave(IFsm<BattleManager> fsm, bool isShutdown)
        {
            Log.Debug("BattleStatePrepare OnLeave");
        }

        protected override void OnUpdate(IFsm<BattleManager> fsm, float elapseSeconds, float realElapseSeconds)
        {
            // Log.Debug("BattleStatePrepare OnUpdate");
            if (_count++ > 100)
            {
                ChangeState<BattleStateStart>(fsm);
            }
        }

        protected override void OnDestroy(IFsm<BattleManager> fsm)
        {
            Log.Debug("BattleStatePrepare OnDestroy");
        }
    }
}
