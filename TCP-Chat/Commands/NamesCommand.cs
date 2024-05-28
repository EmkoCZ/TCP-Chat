using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCP_Chat.Commands
{
    public class NamesCommand : ICommandHandler
    {
        public void Handle(string data, NetworkStream networkStream)
        {
            string names = "users:";
            foreach (var user in Server.GetUsers())
            {
                names += user.GetUsername() + ",";
            }
            names.Remove(names.Length-1);
            networkStream.Write(Encoding.UTF8.GetBytes(names), 0, names.Length);
        }
    }
}
