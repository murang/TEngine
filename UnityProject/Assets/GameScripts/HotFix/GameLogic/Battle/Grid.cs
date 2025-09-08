using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace GameLogic
{
    public class Grid : MonoBehaviour
    {
        public GameObject gridCell;
        public Vector2 bottomCenter;

        private IBattleLogic _logic;
        private readonly List<GameObject> _cells = new();
        private IObjectPool<GameObject> _cellPool;

        private void Awake()
        {
            _cellPool = new ObjectPool<GameObject>(
                createFunc: () => Instantiate(gridCell), // 创建新对象的方法
                actionOnGet: (obj) => obj.SetActive(true), // 获取对象时的操作
                actionOnRelease: (obj) => obj.SetActive(false), // 释放对象时的操作
                actionOnDestroy: (obj) => Destroy(obj), // 销毁对象时的操作
                collectionCheck: true, // 检查重复释放
                defaultCapacity: 7*7, // 默认容量
                maxSize: 10*10 // 最大容量
            );
        }

        public void Build(IBattleLogic logic)
        {
            for (int i = 0; i < _cells.Count; i++)
            {
                _cellPool.Release(_cells[i]);
            }
            _cells.Clear();
            
            _logic = logic;
            Vector2 origin = new Vector2(bottomCenter.x - _logic.GetSize() / 2.0f + 0.5f, bottomCenter.y + 0.5f);
            for (int x = 0; x < _logic.GetSize(); x++)
            {
                for (int y = 0; y < _logic.GetSize(); y++)
                {
                    var cell = _cellPool.Get();
                    cell.transform.SetPositionAndRotation(new Vector3(origin.x + x, origin.y + y), Quaternion.identity);
                    cell.transform.SetParent(transform);
                    _cells.Add(cell);
                }
            }
        }
    }
}
