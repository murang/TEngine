using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class BattleLogic: IBattleLogic
    {
        private int _size;
        private Drop[][] _dropMatrix;
        private IFsm<IBattleLogic> _fsm;
        
        public void Init(int size)
        {
            if (_fsm is not null)
            {
                GameModule.Fsm.DestroyFsm(_fsm);
            }
            _fsm = GameModule.Fsm.CreateFsm(this, new List<FsmState<IBattleLogic>>()
            {
                new LogicStateWait(),
                new LogicStateMatch(),
            });
            
            _size = size;
            _dropMatrix = new Drop[size][];
            for (int x = 0; x < size; x++)
            {
                _dropMatrix[x] = new Drop[size];
            }
        }
        
        public int GetSize()
        {
            return _size;
        }

        public void Start()
        {
            _fsm.Start<LogicStateWait>();
        }

        public int NewDrop()
        {
            return Random.Range(1, _size+1);
        }

        public void Match()
        {
            throw new System.NotImplementedException();
        }
    }
}
