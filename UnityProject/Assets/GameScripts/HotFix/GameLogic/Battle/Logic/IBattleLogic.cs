using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class DropData: IMemory
    {
        public int x;
        public int y;
        public int type;
        public int num;
        public void Clear()
        {
            
        }
    }
    
    public interface IBattleLogic
    {
        void Init(int size);
        int GetSize();
        void Start();
        DropData NewDrop();
        bool DropDown(int x);
        List<DropData> Match();
    }
}
