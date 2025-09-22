using System;
using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameLogic
{
    public class BattleLogic : IBattleLogic
    {
        private DropData _newDrop;
        private int _size;
        private DropData[,] _dropMatrix;
        private IFsm<IBattleLogic> _fsm;
        
        const float blockRate = .3f;

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
            _dropMatrix = new DropData[size, size];
        }

        public void OnDestroy()
        {
            GameModule.Fsm.DestroyFsm(_fsm);
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
            if (Random.value < blockRate)
            {
                _newDrop.block = 2;
            }
            return _newDrop;
        }

        public bool DropDown(int x)
        {
            if (_dropMatrix[x, _size - 1] != null)
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

        public List<DropAction> Match()
        {
            // 规则是每个cell横竖连接cell的数量等于这个cell的数字 这个cell就能被消掉
            
            // 用字典方便去重
            Dictionary<(int, int), DropAction> actionDic = new Dictionary<(int, int), DropAction>();
            
            // 先算列
            var xCount = new int[_size];
            for (int x = 0; x < _size; x++)
            {
                for (int y = 0; y < _size; y++)
                {
                    if (_dropMatrix[x, y] != null)
                    {
                        xCount[x] += 1;
                    }
                }
            }

            for (int x = 0; x < _size; x++)
            {
                for (int y = 0; y < _size; y++)
                {
                    if (_dropMatrix[x, y] != null && _dropMatrix[x, y].block == 0 && _dropMatrix[x, y].num == xCount[x])
                    {
                        actionDic[(x, y)] = new DropAction
                        {
                            x = x,
                            y = y,
                            type = DropActionType.Clear
                        };
                    }
                }
            }

            // 再算行
            for (int y = 0; y < _size; y++)
            {
                int tag = 0;
                for (int x = 0; x < _size; x++)
                {
                    if (_dropMatrix[x, y] == null)
                    {
                        // 如果为空 就可以判断之前的是否匹配
                        int matchNum = x - tag;
                        for (int i = tag; i < x; i++)
                        {
                            if (_dropMatrix[i, y].block == 0 && _dropMatrix[i, y].num == matchNum) // 讲道理_dropMatrix[i, y]不应该为空
                            {
                                actionDic[(i, y)] = new DropAction
                                {
                                    x = i,
                                    y = y,
                                    type = DropActionType.Clear
                                };
                            }
                        }
                        tag = x + 1;
                    }
                }
                // 如果最后一个不为空 也需要判断
                if (tag < _size)
                {
                    int matchNum = _size - tag;
                    for (int i = tag; i < _size; i++)
                    {
                        if (_dropMatrix[i, y].block == 0 && _dropMatrix[i, y].num == matchNum) // 讲道理_dropMatrix[i, y]不应该为空
                        {
                            actionDic[(i, y)] = new DropAction
                            {
                                x = i,
                                y = y,
                                type = DropActionType.Clear
                            };
                        }
                    }
                }
            }

            var blockActioDic = new Dictionary<(int, int), DropAction>();
            foreach (var kv in actionDic)
            {
                int x = kv.Value.x;
                int y = kv.Value.y;
                
                _dropMatrix[x, y] = null;
                // 周围的block修改
                // 上
                if (y < _size - 1)
                {
                    checkAroundBlock(x, y + 1, blockActioDic);
                }
                // 下
                if (y > 0)
                {
                    checkAroundBlock(x, y - 1, blockActioDic);
                }
                // 左
                if (x > 0)
                {
                    checkAroundBlock(x - 1, y, blockActioDic);
                }
                // 上
                if (x < _size - 1)
                {
                    checkAroundBlock(x + 1, y, blockActioDic);
                }
            }

            foreach (var kv in blockActioDic)
            {
                actionDic[kv.Key] = kv.Value;
            }

            // 整理
            for (int x = 0; x < _size; x++)
            {
                for (int y = 0; y < _size; y++)
                {
                    if (_dropMatrix[x, y] != null)
                    {
                        var drop = _dropMatrix[x, y];
                        for (int z = 0; z < y; z++)
                        {
                            if (_dropMatrix[x, z] == null)
                            {
                                drop.x = x;
                                drop.y = z;
                                _dropMatrix[x, z] = drop;
                                _dropMatrix[x, y] = null;
                                break;
                            }
                        }
                    }
                }
            }

            var list = new List<DropAction>();
            foreach(var v in actionDic.Values)
            {
              list.Add(v);  
            }
            
            return list;
        }

        private void checkAroundBlock(int x, int y, Dictionary<(int, int), DropAction> blockDic)
        {
            if (_dropMatrix[x, y] == null || blockDic.ContainsKey((x, y))  || _dropMatrix[x, y].block == 0)
            {
                return;
            }
            _dropMatrix[x, y].block--;
            if (_dropMatrix[x,  y].block == 0)
            {
                blockDic[(x, y)] = new DropAction
                {
                    x = x,
                    y = y,
                    type = DropActionType.ShowNumber
                };
            }else if (_dropMatrix[x, y].block == 1)
            {
                blockDic[(x, y)] = new DropAction
                {
                    x = x,
                    y = y,
                    type = DropActionType.BlockBreak
                };
            }
            else
            {
                Log.Error("???");
            }
        }
    }
}