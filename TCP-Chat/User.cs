using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCP_Chat
{
    public class User
    {
        private NetworkStream netStream;
        private string username;

        public User(NetworkStream networkStream)
        {
            netStream = networkStream;
        }

        public NetworkStream GetNetworkStream()
        {
            return netStream;
        }

        public void SetUsername(string username)
        {
            this.username = username;
        }

        public string GetUsername()
        {
            return username;
        }
    }
}
