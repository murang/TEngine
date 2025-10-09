using System.Collections.Generic;
using TEngine;
using UnityEngine;
using UnityEngine.U2D;
using YooAsset;

namespace GameLogic
{
    public class DropView : MonoBehaviour
    {
        public SpriteRenderer show;
        public Sprite spBlockP;
        public Sprite spBlock;
        public Sprite[] spNumbers;
        
        private DropData _data;

        // Update is called once per frame
        void Update()
        {
            if (_data == null)
            {
                return;
            }
            if (_data.block == 2)
            {
                show.sprite = spBlockP;
            }else if (_data.block == 1)
            {
                show.sprite = spBlock;
            }
            else if(_data.num >=1 && _data.num<=7)
            {
                show.sprite = spNumbers[_data.num-1];
            }
        }

        public void SetData(DropData data)
        {
            _data = data;
        }
    }
}
