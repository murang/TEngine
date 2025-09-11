using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TEngine;
using UnityEngine;
using Ease = DG.Tweening.Ease;

namespace GameLogic
{
    public class BattleManager : MonoBehaviour
    {
        public GameObject prefabDropView;
        public Grid grid;
        
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
            GameModule.Fsm.DestroyFsm(_fsm);
            GameModule.ObjectPool.DestroyObjectPool<Drop>();
            _logic = null;
        }

        public void InitBattleLogic()
        {
            _drops = new Drop[BattleConst.GridSize,BattleConst.GridSize];
            _logic.Init(BattleConst.GridSize);
            grid?.Build(_logic);
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
                _newDrop.Reset();
                _newDrop.SetData(data);
            }
            else
            {
                var view = Instantiate(prefabDropView, grid.transform);
                _newDrop = Drop.Create(view.GetComponent<DropView>());
                _newDrop.SetData(data);
                _dropPool.Register(_newDrop, true);
            }
            
            _newDrop.View.transform.SetLocalPositionAndRotation(grid.bottomCenter + new Vector2(0, _logic.GetSize() +0.5f), Quaternion.identity);
        }

        public void DropDown(int x, int y)
        {
            _drops[x,y] = _newDrop;
            _newDrop.View.transform.SetLocalPositionAndRotation(grid.GetOrigin() + new Vector2(x, _logic.GetSize() - 0.5f), Quaternion.identity);
            _newDrop.View.transform.DOLocalMove(grid.GetOrigin() + new Vector2(x, y), .5f)
                .SetEase(Ease.InQuad)
                .OnKill(() =>
            {
                GameEvent.Get<IEventBattle>().DropDownEnd();
            });
            _newDrop = null;
        }

        public void MatchStart(List<DropData> list)
        {
            var seq = DOTween.Sequence();
            for (int i = 0; i < list.Count; i++)
            {
                var data = list[i];
                var drop = _drops[data.x,data.y];
                if (drop == null)
                {
                    throw new Exception($"MatchStart Drop is null pos: {data.x},{data.y}");
                }
                _drops[data.x, data.y] = null;
                var t1 = drop.View.transform.DOScale(Vector3.one*1.3f, .1f);
                var t2 = drop.View.transform.DOScale(Vector3.zero, .2f).OnKill(() =>
                {
                    _dropPool.Unspawn(drop);
                });

                seq.Join(DOTween.Sequence().Append(t1).Append(t2));
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
                                seq.Join(drop.View.transform.DOLocalMove(grid.GetOrigin() + new Vector2(x, z), .3f).SetEase(Ease.InQuad));
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
