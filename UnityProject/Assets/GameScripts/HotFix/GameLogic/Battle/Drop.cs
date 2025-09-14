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
            d.Initialize(d._view);
            return d;
        }
        
        public DropView View => _view;
        
        public void SetData(DropData data)
        {
            _data = data;
            _view.SetData(data);
        }

        protected override void OnUnspawn()
        {
            MemoryPool.Release(_data);
            _data = null;
        }

        protected override void Release(bool isShutdown)
        {
        }

        public Sequence DoAction(DropAction action)
        {
            var seq = DOTween.Sequence();
            
            switch (action.type)
            {
                case DropActionType.Clear:
                    var t1 = _view.transform.DOScale(Vector3.one*1.3f, .1f);
                    var t2 = _view.transform.DOScale(Vector3.zero, .2f);
                    seq.Join(DOTween.Sequence().Append(t1).Append(t2));
                    break;
                case DropActionType.ShowNumber:
                    var t3 = _view.transform.DOScale(Vector3.one*1.3f, .1f);
                    var t4 = _view.transform.DOScale(Vector3.one, .2f);
                    seq.Join(DOTween.Sequence().Append(t3).Append(t4));
                    break;
                case DropActionType.BlockBreak:
                    var t5 = _view.transform.DOScale(Vector3.one*1.3f, .1f);
                    var t6 = _view.transform.DOScale(Vector3.one, .2f);
                    seq.Join(DOTween.Sequence().Append(t5).Append(t6));
                    break;
                default:
                    Log.Error("???");
                    break;
            }
            
            return seq;
        }
    }
}
