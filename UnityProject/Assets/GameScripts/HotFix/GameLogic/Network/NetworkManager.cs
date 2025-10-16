using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class NetworkSetting
    {
        public static string Url = "ws://127.0.0.1:10086";
    }
    
    public partial class NetworkManager : Singleton<NetworkManager>
    {
        public IConn conn;
        
        public void Init()
        {
            conn = new WsConn(new ConnOption
            {
                Domain = NetworkSetting.Url,
                Codec = new CodecPb()
            });

            conn.OnConnected += iConn =>
            {
                Log.Info($"{NetworkSetting.Url} Connected");
                HandleMsg();
                Login();
            };
            conn.OnDisconnected += conn =>
            {
                Log.Debug("OnDisconnected");
            };
            conn.OnException += (conn, e) =>
            {
                Log.Warning("OnException: {0}", e.Message);
            };
            conn.OnMessage += (conn, o) =>
            {
                Log.Debug("OnMessage: {0}", ((IMessage)o).ToString());
                MsgDispatcher.Instance.DispatchMsg(o);
            };
        }
        
        public void Connect()
        {
            conn.Connect();
        }
        
        public void Disconnect()
        {
            
        }
        
        public void Send(object message)
        {
            conn.Send(message);
        }
    }
}
