using System;
using System.Configuration;
using System.ServiceModel;
using LibraryService.DataContracts;
using LibraryService.ServiceContracts;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string uriString = string.Format(
                "net.tcp://{0}:{1}/{2}",
                ConfigurationManager.AppSettings["ServiceAddress"],
                ConfigurationManager.AppSettings["ServicePort"],
                ConfigurationManager.AppSettings["ServiceName"]
            );
            Console.WriteLine($"Used uri: {uriString}");
            Uri uri = new Uri(uriString);

            //NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
            //var channel = new ChannelFactory<ILibraryService>(binding);
            //var endpoint = new EndpointAddress(uri);
            //var proxy = channel.CreateChannel(endpoint);
            //Console.WriteLine("Created Proxy.");

            // String with available user operations
            string operationString = 
                "Choose operation:\n" +
                "1.| Find books with keyword in title.\n" +
                "2.| Find book with given identifier.\n" +
                "q.| Quit.";
            // buffer for program flow control
            string command = "";
            bool running = true;
            //variables for storing service outputs
            int[] bookIdentifiers = null;
            BookInfo bookInf = null ;
            while (running)
            {   
                Console.WriteLine(operationString);
                command = Console.ReadLine();
                switch(command.ToLower())
                {
                    case "1":
                        break;
                    case "2":
                        break;
                    case "q":
                        running = false;
                        Console.WriteLine("Exiting.");
                        break;
                    default:
                        Console.WriteLine("Chosen option does not exist.");
                        break;
                }
            }
        }
    }
}
