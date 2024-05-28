using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TCP_Chat
{
    public class Server
    {
        // Deklarace promennych
        private static List<User> onlineUsers; // Pripojeni uzivatele
        private NetworkStream netStream; // pripojeni
        private CommandDispatcher dispatcher; // Ruzne metody pro praci s daty

        public Server()
        {
            // priprava servu v konstruktoru, nastaveni promennych atd...
            Console.Clear();
            onlineUsers = new List<User>();
            dispatcher = new CommandDispatcher();
            Console.Title = "Chat Server";
            startServer();
        }

        // static metoda pro jine tridy abychom mohli dostat uzivatele
        public static List<User> GetUsers()
        {
            return onlineUsers;
        }

        // spusteni serveru
        private void startServer()
        {
            // spusteni na portu 3559
            TcpListener listener = new TcpListener(IPAddress.Any, 3559);
            listener.Start();

            Console.WriteLine("TCP Chat Server is now running on port 3559");
            // chceme neustale prijimat data, proto while true
            while (true)
            {
                // Prijmuti pripojeni od uzivatele
                TcpClient client = listener.AcceptTcpClient();
                // Pridani uzivatele do listu asynchrone
                Task.Run(() => AddUser(client.GetStream()));

                NetworkStream stream = client.GetStream();
                netStream = stream;

                Task.Run(() =>
                {
                    while(true)
                    {
                        // Pokud jsou dostupna data
                        if (stream.DataAvailable)
                        {
                            // nacti prichozi data
                            byte[] receivedBytes = ReadToEnd(stream);
                            // byte[] na citelny string
                            string streamData = Encoding.UTF8.GetString(receivedBytes);
                            // vypis prichozi data
                            Console.WriteLine(streamData);
                            // zpracuj prichozi data
                            dispatcher.Dispatch(streamData, stream);
                        }
                        // Thread sleep pro odlechceni procesoru | aby program byl mene narocny aspon o trochu
                        else Thread.Sleep(10);
                    }
                });
            }
        }

        // pridani uzivatele
        private void AddUser(NetworkStream stream)
        {
            // vytvoreni noveho objektu uzivatele
            User newUser = new User(stream);

            // pridani do listu
            onlineUsers.Add(newUser);
            Console.WriteLine("Add user");
        }

        private byte[] ReadToEnd(NetworkStream stream)
        {
            // list pro prijate bytes
            List<byte> receivedBytes = new List<byte>();

            // Pokud jsou dostupna data v networkStreamu...
            while (stream.DataAvailable)
            {
                // byte[] pro nase data
                byte[] buffer = new byte[1024];
                // nacteme data ze streamu do bufferu
                stream.Read(buffer, 0, buffer.Length);
                // pak pridame data z bufferu do receivedBytes
                receivedBytes.AddRange(buffer);
            }

            // pokud jsou tam data ktera jsou prazdna, smaz je
            receivedBytes.RemoveAll(b => b == 0);
            // vrat vysledna nactena data
            return receivedBytes.ToArray();
        }
    }
}
