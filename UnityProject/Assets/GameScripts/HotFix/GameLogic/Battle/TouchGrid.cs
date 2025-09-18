using System;
using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameLogic
{
    [RequireComponent(typeof(GridView))]
    public class TouchGrid : MonoBehaviour, IPointerClickHandler
    {
        private GridView _gridView;
        
        private void Start()
        {
            _gridView = GetComponent<GridView>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var x = _gridView.GetTouchX(eventData.position);
            GameEvent.Get<IEventBattle>().TouchGrid(x);
        }
    }
}
