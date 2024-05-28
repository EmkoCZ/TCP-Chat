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
        // pripojeni uzivatele
        private NetworkStream netStream;
        // jeho username
        private string username;

        public User(NetworkStream networkStream)
        {
            // nastaveni streamu pri vytvoreni uzivatele
            netStream = networkStream;
        }

        // ziskani jeho streamu
        public NetworkStream GetNetworkStream()
        {
            return netStream;
        }

        // nastaveni username | UsernameCommand
        public void SetUsername(string username)
        {
            this.username = username;
        }

        // ziskani jeho username
        public string GetUsername()
        {
            return username;
        }
    }
}
