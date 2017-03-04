using Common;
using System;
using System.IO.Pipes;
using System.Threading;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Client");
            Thread.Sleep(TimeSpan.FromSeconds(1));
            ReceiveByteAndRespond();
        }

        private static void ReceiveByteAndRespond()
        {

            using (NamedPipeClientStream namedPipeClient = new NamedPipeClientStream(Settings.ServerName))
            {
                namedPipeClient.Connect();
                namedPipeClient.ReadMode = PipeTransmissionMode.Message;

                var msg = namedPipeClient.ReadString();
                Console.WriteLine(msg);
                namedPipeClient.WriteString("[1:from client] sync ack");


                var replay = namedPipeClient.ReadObject<Message>();
                Console.WriteLine(replay.ToString());

                var message = new Message { Id = 2, Body = "last time" };
                namedPipeClient.WriteObject<Message>(message);

                Console.ReadKey();
            }
        }
    }
}
