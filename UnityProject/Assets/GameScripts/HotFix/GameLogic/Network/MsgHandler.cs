using System.Collections;
using System.Collections.Generic;
using Pb;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public partial class NetworkManager : Singleton<NetworkManager>
    {
        public void HandleMsg()
        {
            MsgDispatcher.Instance.RegisterMsgReceiver<S2C_Error>(GameEvent.Get<IEventMsg>().S2C_Error);
            MsgDispatcher.Instance.RegisterMsgReceiver<S2C_Heartbeat>(GameEvent.Get<IEventMsg>().S2C_Heartbeat);
            MsgDispatcher.Instance.RegisterMsgReceiver<S2C_Login>(GameEvent.Get<IEventMsg>().S2C_Login);
            MsgDispatcher.Instance.RegisterMsgReceiver<S2C_GetLevelDetail>(GameEvent.Get<IEventMsg>().S2C_GetLevelDetail);
            MsgDispatcher.Instance.RegisterMsgReceiver<S2C_StartLevel>(GameEvent.Get<IEventMsg>().S2C_StartLevel);
            MsgDispatcher.Instance.RegisterMsgReceiver<S2C_FinishLevel>(GameEvent.Get<IEventMsg>().S2C_FinishLevel);
        }
    }
}
