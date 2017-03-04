using Common;
using System;
using System.IO.Pipes;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Server");
            SendByteAndReceiveResponse();
        }

        private static void SendByteAndReceiveResponse()
        {

            using (NamedPipeServerStream namedPipeServer = new NamedPipeServerStream(Settings.ServerName, PipeDirection.InOut, 10, PipeTransmissionMode.Message))
            {
                namedPipeServer.WaitForConnection();
                Console.WriteLine("Got connection");

                namedPipeServer.WriteString("[1:from server] sync req");

                var msg = namedPipeServer.ReadString();
                Console.WriteLine(msg);

                var message = new Message {Id=1, Body="first time" };
                namedPipeServer.WriteObject<Message>(message);

                var replay = namedPipeServer.ReadObject<Message>();
                Console.WriteLine(replay.ToString());
                Console.ReadKey();
            }
        }
    }
}
