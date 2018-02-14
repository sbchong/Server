using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class ServerControl
    {
        private Socket serverSocket;
        private List<Socket> clientList;

        public ServerControl()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientList = new List<Socket>();
        }

        public void Start()
        {
            serverSocket.Bind(new IPEndPoint(IPAddress.Any,22222));
            serverSocket.Listen(10);
            Console.WriteLine("服务器成功启用");

            Thread threadAccept = new Thread(Accept);
            threadAccept.IsBackground = true;
            threadAccept.Start();           
        }

        private void Accept()
        {
            Socket client = serverSocket.Accept();
            IPEndPoint point = client.RemoteEndPoint as IPEndPoint;
            Console.WriteLine(point.Address + "通过端口" + point.Port + "已连接");
            clientList.Add(client);

            Thread threadAccept = new Thread(Receive);
            threadAccept.IsBackground = true;
            threadAccept.Start(client);

            Accept();
        }

        private void Receive(object obj)
        {
            Socket client = obj as Socket;

            IPEndPoint point = client.RemoteEndPoint as IPEndPoint;
            try
            {
                byte[] msg = new byte[1024 * 1024];
                int msgLen = client.Receive(msg);
                Console.WriteLine(point.Address + "：" + Encoding.UTF8.GetString(msg, 0, msgLen));
                Broadcast(point.Address + "：" + Encoding.UTF8.GetString(msg, 0, msgLen));

                Receive(client);
            }
            catch
            {
                Console.WriteLine(point.Address + "离开");
                clientList.Remove(client);
            }
            
        }

        private void Broadcast(string msg)
        {
            foreach (var client in clientList)
            {              
                    client.Send(Encoding.UTF8.GetBytes(msg));
            }
        }
    }
}
