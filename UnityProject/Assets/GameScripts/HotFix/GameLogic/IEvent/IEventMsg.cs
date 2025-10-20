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
        void S2C_Error(S2C_Error msg);
        void S2C_Heartbeat(S2C_Heartbeat msg);
        void S2C_Login(S2C_Login msg);
        void S2C_GetLevelDetail(S2C_GetLevelDetail msg);
        void S2C_StartLevel(S2C_StartLevel msg);
        void S2C_FinishLevel(S2C_FinishLevel msg);
    }
}
