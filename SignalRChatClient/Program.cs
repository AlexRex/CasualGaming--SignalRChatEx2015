using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNet.SignalR.Client;

namespace SignalRChatClient
{
    public class Program
    {

        static string name;
        static IHubProxy proxy;


        static void Main(string[] args)
        {
            Console.Write("Enter your Name: ");
            name = Console.ReadLine();

            HubConnection connection = new HubConnection("http://localhost:51734");
            proxy = connection.CreateHubProxy("ChatHub");

            connection.Received += Connection_Received;

            Action<string, string> SendMessageRecieved = recieved_a_message;

            proxy.On("broadcastMessage", SendMessageRecieved);

            Console.WriteLine("Waiting for connection");
            connection.Start().Wait();
            Console.WriteLine("You can send messages now.");


            connection.StateChanged += Connection_StateChanged;
            

            string input;
            while ((input = Console.ReadLine()) != null)
            {
                proxy.Invoke("Send", new object[] { name, input });
            }

           
        }

        private static void Connection_StateChanged(StateChange obj)
        {
            string line;
            Console.WriteLine("The server has been closed.");
            line = Console.ReadLine();
            Environment.Exit(0);
        }

        private static void Connection_Received(string obj)
        {
            Console.WriteLine("Message Recieved {0}", obj);
        }

        private static void recieved_a_message(string sender, string message)
        {
            Console.WriteLine(sender);
            Console.WriteLine(message);
            Console.WriteLine("{0} : {1}", sender, message);
        }


    }
}
