using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCP_Chat.Commands
{
    public class MessageCommand : ICommandHandler
    {
        public void Handle(string data, NetworkStream networkStream)
        {
            string message = UsersHelper.GetUserByNetStream(networkStream).GetUsername() + ": " + data.Split('/')[1];
            foreach (var user in Server.GetUsers())
            {
                user.GetNetworkStream().Write(Encoding.UTF8.GetBytes(message), 0, message.Length);
            }
        }
    }
}
