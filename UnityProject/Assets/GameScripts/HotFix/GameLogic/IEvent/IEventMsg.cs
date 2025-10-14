using System.Collections;
using System.Collections.Generic;
using Pb;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    [EventInterface(EEventGroup.GroupLogic)]
    public interface IEventMsg
    {
        void S2C_Login(S2C_Login msg);
    }
}
