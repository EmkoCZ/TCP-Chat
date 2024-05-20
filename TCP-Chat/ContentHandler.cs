using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCP_Chat
{
    public class ContentHandler
    {
        private List<string> messages;

        public ContentHandler()
        {
            messages = new List<string>();
            messages.Add("");
        }

        public void AddMessage(string message)
        {
            Console.WriteLine("Added " + message);
            messages.Add(message);
        }

        public void PrintContent()
        {
            //for (int i = messages.Count-1; i > (messages.Count - 20); i--)
            //{
            //    Console.WriteLine(messages[i]);
            //}
            foreach (var item in messages)
            {
                Console.WriteLine(item);
            }
        }
    }
}
