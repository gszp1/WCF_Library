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

            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
            var channel = new ChannelFactory<ILibraryService>(binding);
            var endpoint = new EndpointAddress(uri);
            var proxy = channel.CreateChannel(endpoint);
            Console.WriteLine("Created Proxy.");

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
                    case "1": // find books with keyword in title.
                        Console.WriteLine("Enter keyword: ");
                        string keyword = Console.ReadLine();
                        bookIdentifiers = proxy.FindBooks(keyword);
                        if (bookIdentifiers.Length == 0)
                        {
                            Console.WriteLine("No books with given keyword in title.");
                            break;
                        }
                        Console.WriteLine("Found identifiers: ");
                        int count = 0;
                        foreach(var bookId in bookIdentifiers)
                        {
                            if (count == 10)
                            {
                                Console.WriteLine();
                                count = 0;
                            }
                            Console.Write($"{bookId} ");
                        }
                        Console.WriteLine();
                        break;
                    case "2": // find book with given identifier.
                        try
                        {
                            Console.WriteLine("Enter identifier: ");
                            int identifier = Convert.ToInt32(Console.ReadLine());
                            bookInf = proxy.GetBookInfo(identifier);
                            Console.WriteLine($"Book details:\nTitle: {bookInf.title}");
                            int counter = 1;
                            foreach (AuthorInfo author in bookInf.authors)
                            {
                                Console.WriteLine($"Author {counter++}: {author.firstName} {author.lastName}");
                            }
                        }
                        catch (Exception ex) when (ex is FormatException || ex is OverflowException)
                        {
                            Console.WriteLine("Invalid identifier.");
                        }
                        catch (FaultException<BookNotFound> bookEx)
                        {
                            Console.WriteLine(bookEx.Message);
                        }
                        break;
                    case "q": // Exit.
                        running = false;
                        Console.WriteLine("Exiting.");
                        break;
                    default: // Wrong option.
                        Console.WriteLine("Chosen option does not exist.");
                        break;
                }
            }
        }
    }
}
