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
    }
}
