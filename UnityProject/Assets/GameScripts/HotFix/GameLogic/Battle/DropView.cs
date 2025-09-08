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
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void SetData(DropData data)
        {
            txtNum.text = data.num.ToString();
        }

        public void Reset()
        {
            
        }
    }
}
