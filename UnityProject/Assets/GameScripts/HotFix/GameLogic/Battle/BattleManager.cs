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
        private Drop[][] _drops;
        
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
            _newDrop.View.transform.SetLocalPositionAndRotation(grid.GetOrigin() + new Vector2(x, _logic.GetSize() + 0.5f), Quaternion.identity);
            _newDrop.View.transform.DOLocalMove(grid.GetOrigin() + new Vector2(x, y), 1)
                .SetEase(Ease.InQuad)
                .OnKill(() =>
            {
                GameEvent.Get<IEventBattle>().DropDownEnd();
            });
        }
    }
}
