using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class Cell : ObjectBase
    {
        private CellView _view;
        
        public Cell(CellView view)
        {
            _view = view;
            Initialize("nice", _view);
        }
        
        protected override void Release(bool isShutdown)
        { }

        public void SayHello()
        {
            Log.Warning("Hello");
        }
    }
}
