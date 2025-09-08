using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public struct DropData
    {
        public int x;
        public int y;
        public int type;
        public int num;
    }
    
    public interface IBattleLogic
    {
        void Init(int size);
        int GetSize();
        void Start();
        int NewDrop();
        void Match();
    }
}
