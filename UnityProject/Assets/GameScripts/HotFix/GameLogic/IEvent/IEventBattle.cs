using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    [EventInterface(EEventGroup.GroupLogic)]
    public interface IEventBattle
    {
        void StartBattle();
        void RestartBattle();
        void ShowNewDrop(DropData data);
        void TouchGrid(int x);
        void DropDownStart(int x, int y);
        void DropDownEnd();
        void MatchStart(List<DropAction> list);
        void MatchEnd();
        void BlockStart();
        void BlockEnd();
    }
}
