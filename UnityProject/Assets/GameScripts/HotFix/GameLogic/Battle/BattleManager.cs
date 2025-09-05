using System;
using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class BattleManager : MonoBehaviour
    {
        private IObjectPool<Cell> _cellPool;
        
        private void Awake()
        {
            _cellPool = GameModule.ObjectPool.CreateSingleSpawnObjectPool<Cell>();
            
            GameEvent.AddEventListener(IEventBattle_Event.StartBattle, StartGame);
        }

        private void OnDestroy()
        {
            GameEvent.RemoveEventListener(IEventBattle_Event.StartBattle, StartGame);
        }

        void StartGame()
        {
            Log.Warning("START ~");
            
            
            if (_cellPool.CanSpawn())
            {
                Cell cell = _cellPool.Spawn();
                cell.SayHello();
            }
            else
            {
                var obj = new GameObject("nice");
                CellView v = obj.AddComponent<CellView>();
                Cell c = new Cell(v);
                
                _cellPool.Register(c, true);
            }
            
        }
    }
}
