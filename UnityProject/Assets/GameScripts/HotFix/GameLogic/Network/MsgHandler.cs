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
            MsgDispatcher.Instance.RegisterMsgReceiver<S2C_Login>(GameEvent.Get<IEventMsg>().S2C_Login);
        }
    }
}
