using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCP_Chat
{
    public class ContentHandler
    {
        // list zprav z chatu
        private List<string> messages;

        public ContentHandler()
        {
            // nastav messages
            messages = new List<string>();
            // pridani prazdne zpravy pro vyvarovani se cteni NULL
            messages.Add("");
        }

        public void Handle(string message)
        {
            // Pokud by se funkce rozsirili, zde by se pridala logika na rozliseni dat
            messages.Add(message);
        }

        public void PrintContent()
        {
            // Pokud je mene nez 16 zprav, vypis je vsechny
            if (messages.Count < 16)
            {
                foreach (var item in messages)
                {
                    Console.WriteLine(item);
                }
            } else
            {
                // Pokud jich je vic, vypis jen poslednich x, aby se nevypisovalo treba 1000 zprav...
                for (int i = messages.Count - 15; i < messages.Count; i++)
                {
                    Console.WriteLine(messages[i]);
                }
            }
        }
    }
}
