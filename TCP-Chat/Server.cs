using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TCP_Chat
{
    public class Server
    {
        private static List<User> onlineUsers;
        private NetworkStream netStream;
        private CommandDispatcher dispatcher;

        public Server()
        {
            Console.Clear();
            onlineUsers = new List<User>();
            dispatcher = new CommandDispatcher();
            Console.Title = "Chat Server";
            startServer();
        }

        public static List<User> GetUsers()
        {
            return onlineUsers;
        }

        private void startServer()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 3559);
            listener.Start();

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Task.Run(() => AddUser(client.GetStream()));

                NetworkStream stream = client.GetStream();
                netStream = stream;

                Task.Run(() =>
                {
                    while(true)
                    {
                        if (stream.DataAvailable)
                        {
                            byte[] receivedBytes = ReadToEnd(stream);
                            string streamData = Encoding.UTF8.GetString(receivedBytes);
                            Console.WriteLine(streamData);
                            //Write(onlineUsers[0].GetNetworkStream(), Encoding.UTF8.GetBytes("GOT DATA"));
                            dispatcher.Dispatch(streamData, stream);
                        }
                        else Thread.Sleep(10);

                        //Write(onlineUsers[0].GetNetworkStream(), Encoding.UTF8.GetBytes("JJ: TEST"));
                    }
                });
            }
        }

        private void AddUser(NetworkStream stream)
        {
            User newUser = new User(stream);

            onlineUsers.Add(newUser);
            Console.WriteLine("Add user");
        }

        private void RemUser(NetworkStream stream)
        {
            try
            {
                foreach (User user in onlineUsers)
                {
                    if (user.GetNetworkStream() == stream)
                    {
                        onlineUsers.Remove(user);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private byte[] ReadToEnd(NetworkStream stream)
        {
            List<byte> receivedBytes = new List<byte>();

            while (stream.DataAvailable)
            {
                byte[] buffer = new byte[1024];
                stream.Read(buffer, 0, buffer.Length);
                receivedBytes.AddRange(buffer);
            }

            receivedBytes.RemoveAll(b => b == 0);
            return receivedBytes.ToArray();
        }

        private void Write(NetworkStream stream, byte[] data)
        {
            stream.Write(data, 0, data.Length);
        }
    }
}
