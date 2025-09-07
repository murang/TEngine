using System;
using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class BattleManager : MonoBehaviour
    {
        private IFsm<BattleManager> _fsm;
        private IObjectPool<Cell> _cellPool;
        
        private void Awake()
        {
            _fsm = GameModule.Fsm.CreateFsm(this, new List<FsmState<BattleManager>>()
            {
                new BattleStatePrepare(),
                new BattleStateStart(),
                new BattleStateRunning(),
            });
            _cellPool = GameModule.ObjectPool.CreateSingleSpawnObjectPool<Cell>();
        }

        private void Start()
        {
            _fsm.Start<BattleStatePrepare>();
            
            GameEvent.AddEventListener(IEventBattle_Event.StartBattle, StartGame);
        }

        private void OnDestroy()
        {
            GameEvent.RemoveEventListener(IEventBattle_Event.StartBattle, StartGame);
        }

        void StartGame()
        {
            Log.Warning("START ~");
            
            // if (_cellPool.CanSpawn())
            // {
            //     Cell cell = _cellPool.Spawn();
            //     cell.SayHello();
            // }
            // else
            // {
            //     var obj = new GameObject("nice");
            //     CellView v = obj.AddComponent<CellView>();
            //     Cell c = new Cell(v);
            //     
            //     _cellPool.Register(c, true);
            // }
        }
    }
}
