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
        void ShowNewDrop(int num);
    }

    [EventInterface(EEventGroup.GroupLogic)]
    public interface IEventBattleLogic
    {
        void NewDrop();
        void TouchGrid(int x);
    }
}
