using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class Drop : ObjectBase
    {
        private bool _isDown;
        private DropData _data;
        private DropView _view;
        
        public static Drop Create(DropView view)
        {
            var d = MemoryPool.Acquire<Drop>();
            d._view = view;
            d.Initialize(d._view.name, d._view);
            return d;
        }

        public void SetNum(int num)
        {
            _data.num = num;
            _view.SetData(_data);
        }
        
        public void SetData(DropData data)
        {
            _data = data;
            _view.SetData(data);
        }

        public void Reset()
        {
            _isDown = false;
            _data = default;
            _view.Reset();
        }

        protected override void Release(bool isShutdown)
        {
            if (_view is not null && !isShutdown)
            {
                Object.DestroyImmediate(_view.gameObject);
                _view = null;
            }
        }

        public void SetPosition(Vector2 pos)
        {
            _view.transform.SetLocalPositionAndRotation(pos, Quaternion.identity);
        }
    }
}
