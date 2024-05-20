using System.Collections.Generic;
using TCP_Chat.Commands;
using System.Net.Sockets;

namespace TCP_Chat
{
    public class CommandDispatcher
    {
        private readonly Dictionary<string, ICommandHandler> handlers;

        public CommandDispatcher()
        {
            handlers = new Dictionary<string, ICommandHandler>();
            handlers.Add("username", new UsernameCommand());
            handlers.Add("message", new MessageCommand());
        }

        public void Dispatch(string data, NetworkStream networkStream)
        {
            var parts = data.Split('/');
            var command = parts[0];
            handlers.TryGetValue(command, out ICommandHandler handler);
            handler.Handle(data, networkStream);
        }
    }
}
