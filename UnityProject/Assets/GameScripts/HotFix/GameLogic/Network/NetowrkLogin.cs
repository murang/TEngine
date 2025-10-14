using System.Collections;
using System.Collections.Generic;
using Pb;
using UnityEngine;

namespace GameLogic
{
    public partial class NetworkManager : Singleton<NetworkManager>
    {
        public void Login()
        {
            conn.Send(new C2S_Login
            {
                Code = "nice"
            });
        }
    }
}
