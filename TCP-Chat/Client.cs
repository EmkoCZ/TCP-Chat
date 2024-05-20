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
        private NetworkStream networkStream;
        private TcpClient client;
        private ContentHandler contentHandler;

        public Client()
        {
            Console.Clear();
            contentHandler = new ContentHandler();
            chooseServer();
        }

        private void chooseServer()
        {
            Console.WriteLine("Enter number form 1 to 2");
            Console.WriteLine("1. Local server");
            Console.WriteLine("2. Enter IP Address");
            try
            {
                int input = Int16.Parse(Console.ReadLine());
                switch (input)
                {
                    case 1:
                        connectToServer("127.0.0.1");
                        break;
                    case 2:
                        Console.Write("Enter IP Address:");
                        string ip = Console.ReadLine();
                        connectToServer(ip);
                        break;
                    default:
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
            }
        }

        private void connectToServer(string ipAddress)
        {
            try
            {
                client = new TcpClient(ipAddress, 3559);
                networkStream = client.GetStream();
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine("Failed to connect to server with IP: " + ipAddress + "\n");
                chooseServer();
            }
            chooseUsername();
            getMessages();
        }

        private void chooseUsername()
        {
            Console.Write("Enter your username: ");
            string username = Console.ReadLine();
            Write(networkStream, Encoding.UTF8.GetBytes("username/"+username));
        }

        private void getMessages()
        {
            char input;
            string inputString = "";
            
            while (true)
            {
                
                if (networkStream.DataAvailable)
                {
                    byte[] data = ReadToEnd(networkStream);
                    string dataString = Encoding.UTF8.GetString(data);
                    contentHandler.AddMessage(dataString);
                    Console.WriteLine(dataString);
                }

                Task.Run(() =>
                {
                    input = Console.ReadKey().KeyChar;
                    if (input == (char)13)
                    {
                        Write(networkStream, Encoding.UTF8.GetBytes("message/" + inputString));
                        inputString = "";
                    }
                    else if (input == (char)8)
                        inputString = inputString.Remove(inputString.Length - 1);
                    else inputString += input;
                });
                

                Console.Clear();
                contentHandler.PrintContent();
                Console.WriteLine("----------------------------------------------------------------");
                Console.WriteLine($"Type: {inputString}");

                Thread.Sleep(10);
            }
        }

        private byte[] ReadToEnd(NetworkStream stream)
        {
            List<byte> receivedBytes = new List<byte>();

            while (stream.DataAvailable)
            {
                byte[] buffer = new byte[1024];
                stream.Read(buffer, 0, buffer.Length);
                receivedBytes.AddRange(buffer);
            }

            receivedBytes.RemoveAll(b => b == 0);
            return receivedBytes.ToArray();
        }

        private void Write(NetworkStream stream, byte[] data)
        {
            stream.Write(data, 0, data.Length);
        }

        private async void AsyncWrite(NetworkStream stream, byte[] data)
        {
            stream.Write(data, 0, data.Length);
        }
    }
}
