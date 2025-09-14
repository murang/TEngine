using System.Collections;
using System.Collections.Generic;
using TEngine;
using TMPro;
using UnityEngine;

namespace GameLogic
{
    public class DropView : MonoBehaviour
    {
        public TMP_Text txtNum;

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
                txtNum.text = "!";
            }else if (_data.block == 1)
            {
                txtNum.text = "?";
            }
            else
            {
                txtNum.text = _data.num.ToString();
            }
        }

        public void SetData(DropData data)
        {
            _data = data;
        }
    }
}
