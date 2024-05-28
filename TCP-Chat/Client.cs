using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TCP_Chat
{
    public class Client
    {
        // Deklarace promennych
        private NetworkStream networkStream; // stream dat/propojeni se serverem
        private TcpClient client; // nas TCP klient
        private ContentHandler contentHandler; // Handler na content

        public Client()
        {
            // Nastaveni hodnot a priprava title v konstruktoru
            Console.Clear();
            contentHandler = new ContentHandler();
            chooseServer();
        }

        private void chooseServer()
        {
            // Zvoleni zda se chce uzivatel prihlasit na lokalni server, nebo na server pomoci IP adresy
            Console.WriteLine("Enter number form 1 to 2");
            Console.WriteLine("1. Local server");
            Console.WriteLine("2. Enter IP Address");
            try
            {
                int input = Int16.Parse(Console.ReadLine());
                switch (input)
                {
                    case 1:
                        // lokalni server
                        connectToServer("127.0.0.1");
                        break;
                    case 2:
                        // vzdaleny server
                        Console.Write("Enter IP Address:");
                        string ip = Console.ReadLine();
                        connectToServer(ip);
                        break;
                    default:
                        // chybna volba, opakuj volbu
                        Console.Clear();
                        Console.WriteLine("Numbers from 1 to 2 PLEASE");
                        chooseServer();
                        break;
                }

            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.WriteLine("Please enter only numbers.");
                chooseServer();
                // chybna volba, opakuj volbu
            }
        }

        private void connectToServer(string ipAddress)
        {
            try
            {
                // pokusime se navazat spojeni se serverem
                client = new TcpClient(ipAddress, 3559);
                networkStream = client.GetStream();
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine("Failed to connect to server with IP: " + ipAddress + "\n");
                chooseServer();
                // neco nevyslo, nebo je IP spatna, opakuj zvoleni serveru
            }
            chooseUsername();
            getMessages();
        }

        private void chooseUsername()
        {
            // zvoleni jmena uzivatele, uzivatele mohou mit stejna jmena
            Console.Write("Enter your username: ");
            string username = Console.ReadLine();
            Write(networkStream, Encoding.UTF8.GetBytes("username/"+username));
        }

        private void getMessages()
        {
            // hlavni metoda na prijmani a psani zprav

            // char input drzi jednu klavesu kterou uzivatel naposledy zmackl
            char input;
            // inputString pak drzi slouceni char inputu
            string inputString = "";
            
            // chceme neustale prijimat data, proto while true
            while (true)
            {
                // pokud jsou dostupna data
                if (networkStream.DataAvailable)
                {
                    // nacti prichozi data
                    byte[] data = ReadToEnd(networkStream);
                    // byte[] na citelny string
                    string dataString = Encoding.UTF8.GetString(data);
                    // contentHandler managuje co se ma s daty stat
                    contentHandler.Handle(dataString);
                }

                // async task aby jsme behem cteni dat ze serveru mohli psat do konzole
                // a konzole se nezasekla na cteni inputu
                Task.Run(() =>
                {
                    // nacti input
                    input = Console.ReadKey().KeyChar;
                    // pokud stiskneme enter, posli zpravu
                    if (input == (char)13)
                    {
                        Write(networkStream, Encoding.UTF8.GetBytes("message/" + inputString));
                        inputString = "";
                    } // pokud zmackneme backspace, smaz posledni char
                    else if (input == (char)8)
                        inputString = inputString.Remove(inputString.Length - 1);
                    // pridej char do inputStringu
                    else inputString += input;
                });
                

                // toto jede stale dokola, musime mazat konzoli abychom mohli vse vypsat znovu
                Console.Clear();
                // PrintContent vypise vsechny prichozi zpravy
                contentHandler.PrintContent();
                // nas input
                Console.WriteLine("----------------------------------------------------------------");
                Console.WriteLine($"Type: {inputString}");

                // Thread sleep pro odlechceni procesoru | aby program byl mene narocny aspon o trochu
                Thread.Sleep(10);
            }
        }

        // Metoda na cteni dat
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

        // Metoda na poslani zpravy do serveru
        private void Write(NetworkStream stream, byte[] data)
        {
            try
            {
                // posli data
                stream.Write(data, 0, data.Length);
            }
            catch (Exception)
            {
                // ztraceno pripojeni k server
                contentHandler.Handle("LOST CONNECTION TO SERVER");
            }            
        }
    }
}
