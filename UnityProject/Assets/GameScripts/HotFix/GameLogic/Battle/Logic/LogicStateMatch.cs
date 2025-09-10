using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class LogicStateMatch : FsmState<IBattleLogic>
    {
        private IFsm<IBattleLogic> _fsm;
        private IBattleLogic _logic;
        
        protected override void OnEnter(IFsm<IBattleLogic> fsm)
        {
            _fsm = fsm;
            _logic = fsm.Owner;
            
        }

        protected override void OnLeave(IFsm<IBattleLogic> fsm, bool isShutdown)
        {
            
        }
        
        
    }
}
