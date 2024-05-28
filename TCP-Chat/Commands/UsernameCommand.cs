using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCP_Chat.Commands
{
    public class UsernameCommand : ICommandHandler
    {
        public void Handle(string data, NetworkStream networkStream)
        {
            // nastaveni username uzivatele
            UsersHelper.GetUserByNetStream(networkStream).SetUsername(data.Split('/')[1]);
        }
    }
}
