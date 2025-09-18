using System;
using System.IO;
using System.Net.Sockets;
using Cysharp.Threading.Tasks;
using UnityWebSocket;

namespace GameLogic
{
    public class TcpConn : Conn
    {
        public TcpConn(ConnOption option)
        {
            Option = option;
            
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
    }
}
