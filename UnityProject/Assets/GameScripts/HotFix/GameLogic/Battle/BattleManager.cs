using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using TEngine;
using UnityEngine;
using UnityEngine.Serialization;
using Ease = DG.Tweening.Ease;

namespace GameLogic
{
    public class BattleManager : MonoBehaviour
    {
        public int level;
        
        public GameObject prefabDropView;
        public GridView gridView;
        
        private Drop _newDrop;
        private Drop[,] _drops;
        private IFsm<BattleManager> _fsm;
        private IObjectPool<Drop> _dropPool;
        private IBattleLogic _logic;
        
        private void Awake()
        {
            _fsm = GameModule.Fsm.CreateFsm(this, new List<FsmState<BattleManager>>()
            {
                new BattleStatePrepare(),
                new BattleStateStart(),
                new BattleStateRunning(),
                new BattleStateWin(),
                new BattleStateLose(),
            });
            _dropPool = GameModule.ObjectPool.CreateSingleSpawnObjectPool<Drop>();
            _logic = new BattleLogic();
        }

        private void Start()
        {
            _fsm.Start<BattleStatePrepare>();
            GameModule.UI.ShowUIAsync<BattleUI>();
        }

        private void OnDestroy()
        {
            GameModule.ObjectPool.DestroyObjectPool<Drop>();
            GameModule.Fsm.DestroyFsm(_fsm);
            _logic.OnDestroy();
            _logic = null;
        }

        public void InitBattleLogic(int level)
        {
            _drops = new Drop[BattleConst.GridSize,BattleConst.GridSize];
            _logic.Init(new BattleLogicConfig()
            {
                size = BattleConst.GridSize,
                seed = level,
            });
            gridView?.Build(_logic);
        }
        
        public void StartGame()
        {
            _logic.Start();
        }

        public void ShowNewDrop(DropData data)
        {
            if (_dropPool is null)
            {
                throw new Exception("GetDrop Drop pool is null");
            }
            
            if (_dropPool.CanSpawn())
            {
                _newDrop = _dropPool.Spawn();
                _newDrop.SetData(data);
            }
            else
            {
                var view = Instantiate(prefabDropView, gridView.transform);
                _newDrop = Drop.Create(view.GetComponent<DropView>());
                _newDrop.SetData(data);
                _dropPool.Register(_newDrop, true);
            }
            
            _newDrop.View.transform.localScale = Vector3.one;
            _newDrop.View.transform.SetLocalPositionAndRotation(gridView.bottomCenter + new Vector2(0, _logic.GetSize() +0.5f), Quaternion.identity);
        }

        public void DropDown(int x, int y)
        {
            _drops[x,y] = _newDrop;
            _newDrop.View.transform.SetLocalPositionAndRotation(gridView.GetOrigin() + new Vector2(x, _logic.GetSize() - 0.5f), Quaternion.identity);
            _newDrop.View.transform.DOLocalMove(gridView.GetOrigin() + new Vector2(x, y), .5f)
                .SetEase(Ease.InQuad)
                .OnKill(() =>
            {
                GameEvent.Get<IEventBattle>().DropDownEnd();
            });
            _newDrop = null;
        }

        public void MatchStart(List<DropAction> list)
        {
            var seq = DOTween.Sequence();
            for (int i = 0; i < list.Count; i++)
            {
                var action = list[i];
                var drop = _drops[action.x,action.y];
                if (drop == null)
                {
                    throw new Exception($"MatchStart Drop is null pos: {action.x},{action.y}");
                }

                var actioMove = drop.DoAction(action);
                if (action.type == DropActionType.Clear)
                {   // 消除
                    _drops[action.x, action.y] = null;
                    actioMove.OnKill(() =>
                    {
                        _dropPool.Unspawn(drop);
                    });
                }
                seq.Join(actioMove);
            }

            seq.OnKill(() =>
            {
                Tidy();
            });
        }

        public void Tidy()
        {
            var seq = DOTween.Sequence();
            var _size = _logic.GetSize();
            
            bool needTidy = false;
            for (int x = 0; x < _size; x++)
            {
                for (int y = 0; y < _size; y++)
                {
                    if (_drops[x,y] != null)
                    {
                        var drop = _drops[x,y];
                        for (int z = 0; z < y; z++)
                        {
                            if (_drops[x,z] == null)
                            {
                                needTidy = true;
                                _drops[x,z] = drop;
                                _drops[x,y] = null;
                                seq.Join(drop.View.transform.DOLocalMove(gridView.GetOrigin() + new Vector2(x, z), .3f).SetEase(Ease.InQuad));
                                break;
                            }
                        }
                    }
                }
            }

            if (needTidy)
            {
                seq.OnKill(() =>
                {
                    GameEvent.Get<IEventBattle>().MatchEnd();
                });
            }
            else
            {
                GameEvent.Get<IEventBattle>().MatchEnd();
            }
        }
    }
}
