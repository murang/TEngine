using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class BattleLogic: IBattleLogic
    {
        private DropData _newDrop;
        private int _size;
        private DropData[][] _dropMatrix;
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
            _dropMatrix = new DropData[size][];
            for (int x = 0; x < size; x++)
            {
                _dropMatrix[x] = new DropData[size];
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

        public DropData NewDrop()
        {
            _newDrop = MemoryPool.Acquire<DropData>();
            _newDrop.num = Random.Range(1, _size + 1);
            return _newDrop;
        }

        public void DropDown(int x)
        {
            // 如果当前列已经满了 无法下落
            var col = _dropMatrix[x];
            if (col[_size-1] != null)
            {
                Log.Debug("col full");
                return;
            }

            for (int y = 0; y < _size; y++)
            {
                if (col[y] == null)
                {
                    _newDrop.x = x;
                    _newDrop.y = y;
                    col[y] = _newDrop;
                    _newDrop = null;
                    GameEvent.Get<IEventBattle>().DropDownStart(x, y);
                    break;
                }
            }
        }

        public void Match()
        {
            throw new System.NotImplementedException();
        }
    }
}
