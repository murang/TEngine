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
        private DropData[,] _dropMatrix;
        private IFsm<IBattleLogic> _fsm;
        
        public void Init(int size)
        {
            if (_fsm is not null)
            {
                GameModule.Fsm.DestroyFsm(_fsm);
            }
            _fsm = GameModule.Fsm.CreateFsm(this, new List<FsmState<IBattleLogic>>()
            {
                new LogicStateReady(),
                new LogicStateWait(),
                new LogicStateMatch(),
            });
            
            _size = size;
            _dropMatrix = new DropData[size,size];
        }
        
        public int GetSize()
        {
            return _size;
        }

        public void Start()
        {
            _fsm.Start<LogicStateReady>();
        }

        public DropData NewDrop()
        {
            _newDrop = MemoryPool.Acquire<DropData>();
            _newDrop.num = Random.Range(1, _size + 1);
            return _newDrop;
        }

        public bool DropDown(int x)
        {
            if (_dropMatrix[x, _size-1] != null)
            {
                Log.Debug("col full");
                return false;
            }

            for (int y = 0; y < _size; y++)
            {
                if (_dropMatrix[x, y] == null)
                {
                    _newDrop.x = x;
                    _newDrop.y = y;
                    _dropMatrix[x, y] = _newDrop;
                    _newDrop = null;
                    GameEvent.Get<IEventBattle>().DropDownStart(x, y);
                    break;
                }
            }

            return true;
        }

        public List<DropData> Match()
        {
            var list = new List<DropData>();
            
            // 先计算每行有多少
            var xCount = new int[_size];
            var yCount = new int[_size];
            for (int x = 0; x < _size; x++)
            {
                for (int y = 0; y < _size; y++)
                {
                    if (_dropMatrix[x,y] != null)
                    {
                        xCount[x] += 1;
                        yCount[y] += 1;
                    }
                }
            }
            
            // 匹配数字
            for (int x = 0; x < _size; x++)
            {
                for (int y = 0; y < _size; y++)
                {
                    if (_dropMatrix[x,y] != null)
                    {
                        var drop = _dropMatrix[x,y];
                        if (drop.num == xCount[x] || drop.num == yCount[y])
                        {
                            list.Add(drop);
                            _dropMatrix[x, y] = null;
                        }
                    }
                }
            }
            
            // 整理
            for (int x = 0; x < _size; x++)
            {
                for (int y = 0; y < _size; y++)
                {
                    if (_dropMatrix[x,y] != null)
                    {
                        var drop = _dropMatrix[x,y];
                        for (int z = 0; z < y; z++)
                        {
                            if (_dropMatrix[x,z] == null)
                            {
                                drop.x = x;
                                drop.y = z;
                                _dropMatrix[x,z] = drop;
                                _dropMatrix[x,y] = null;
                                break;
                            }
                        }
                    }
                }
            }

            return list;
        }
    }
}
