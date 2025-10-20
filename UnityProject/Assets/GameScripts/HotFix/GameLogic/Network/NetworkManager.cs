using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Google.Protobuf;
using Pb;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class NetworkSetting
    {
        public static string Url = "ws://127.0.0.1:10086";
        public static int HeartbeatInterval = 5;
    }
    
    public partial class NetworkManager : Singleton<NetworkManager>
    {
        private IConn _conn;
        private bool _isConnect;
        
        public void Init()
        {
            _conn = new WsConn(new ConnOption
            {
                Domain = NetworkSetting.Url,
                Codec = new CodecPb()
            });

            _conn.OnConnected += iConn =>
            {
                Log.Info($"{NetworkSetting.Url} Connected");
                _isConnect = true;
                StartHeartbeat().Forget();
                HandleMsg();
                Login();
            };
            _conn.OnDisconnected += conn =>
            {
                Log.Info("OnDisconnected");
                _isConnect = false;
            };
            _conn.OnException += (conn, e) =>
            {
                Log.Warning("OnException: {0}", e.Message);
            };
            _conn.OnMessage += (conn, o) =>
            {
                Log.Info("OnMessage: {0}", ((IMessage)o).ToString());
                MsgDispatcher.Instance.DispatchMsg(o);
            };
        }
        
        public void Connect()
        {
            _conn.Connect();
        }
        
        public void Disconnect()
        {
            
        }
        
        public void Send(object message)
        {
            _conn.Send(message);
        }

        public async UniTaskVoid StartHeartbeat()
        {
            while (_isConnect)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(NetworkSetting.HeartbeatInterval));
                Send(new C2S_Heartbeat());
            }
        }
    }
}
