using System;
using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class BattleManager : MonoBehaviour
    {
        public GameObject prefabDropView;
        public Grid grid;
        
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

        public void ShowNewDrop(int num)
        {
            if (_dropPool is null)
            {
                throw new Exception("GetDrop Drop pool is null");
            }
            
            Drop drop;
            if (_dropPool.CanSpawn())
            {
                drop = _dropPool.Spawn();
                drop.Reset();
                drop.SetNum(num);
            }
            else
            {
                var view = Instantiate(prefabDropView, grid.transform);
                drop = Drop.Create(view.GetComponent<DropView>());
                drop.SetNum(num);
                _dropPool.Register(drop, true);
            }
            
            drop.SetPosition(grid.bottomCenter + new Vector2(0, _logic.GetSize() +0.5f));
        }
        
        
        public void StartGame()
        {
            _logic.Start();
        }
    }
}
