using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCP_Chat
{
    public static class UsersHelper
    {
        public static User GetUserByNetStream(NetworkStream networkStream)
        {
            foreach (var user in Server.GetUsers())
            {
                if(user.GetNetworkStream() == networkStream) 
                    return user;
            }

            return null;
        }
    }
}
