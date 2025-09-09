using System;
using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameLogic
{
    [RequireComponent(typeof(Grid))]
    public class TouchGrid : MonoBehaviour, IPointerClickHandler
    {
        private Grid _grid;
        
        private void Start()
        {
            _grid = GetComponent<Grid>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Log.Debug(eventData.position);
        }
    }
}
