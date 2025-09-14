using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class Drop : ObjectBase
    {
        private DropData _data;
        private DropView _view;
        
        public static Drop Create(DropView view)
        {
            var d = MemoryPool.Acquire<Drop>();
            d._view = view;
            d.Initialize(d._view.name, d._view);
            return d;
        }
        
        public DropView View => _view;
        
        public void SetData(DropData data)
        {
            _data = data;
            _view.SetData(data);
        }
        
        protected override void Release(bool isShutdown)
        {
            MemoryPool.Release(_data);
            if (_view is not null && !isShutdown)
            {
                Object.DestroyImmediate(_view.gameObject);
                _view = null;
            }
        }
    }
}
