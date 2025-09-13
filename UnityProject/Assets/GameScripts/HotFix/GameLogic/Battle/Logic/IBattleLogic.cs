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
        public int block; // 0-2
        public int num; // 1-7
        public void Clear()
        {
            x = 0;
            y = 0;
            block = 0;
            num = 0;
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
