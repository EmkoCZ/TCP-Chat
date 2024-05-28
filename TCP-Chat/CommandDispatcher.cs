using System.Collections.Generic;
using TCP_Chat.Commands;
using System.Net.Sockets;

namespace TCP_Chat
{
    public class CommandDispatcher
    {
        // Dictionary pro vsechny nase prikazy
        private readonly Dictionary<string, ICommandHandler> handlers;

        public CommandDispatcher()
        {
            // nastaveni handlers
            handlers = new Dictionary<string, ICommandHandler>();
            // pridani nasich prikazu
            handlers.Add("username", new UsernameCommand()); // pro nastaveni username uzivatele
            handlers.Add("message", new MessageCommand()); // pro poslani zpravy vsem uzivatelu
        }

        // zpracuj data prikazu
        public void Dispatch(string data, NetworkStream networkStream)
        {
            // data serveru jsou rozdelena takto: typ/data
            // typ je prikaz
            var parts = data.Split('/'); // rozdel na typ a data
            var command = parts[0]; // prikaz je [0]
            handlers.TryGetValue(command, out ICommandHandler handler); // najdi prikaz co pouziva typ command
            handler.Handle(data, networkStream); // prikaz zpracuje data
        }

        //NOTE Tento zpusob prikazu jsem chtel proto aby vsechna logika serveru nebyla
        //    v jedne tride, ale bylo to trochu organizovane
    }
}
