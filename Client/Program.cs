﻿using System;
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
            // Read Uniform Resource Identifier from configuration.
            Uri uri;
            try
            {
                uri = ReadConfigurationURI();
            }
            catch
            {
                Console.WriteLine("Provided configuration parameters are invalid.");
                return;
            }
            Console.WriteLine($"Used URI: {uri}");

            // Establish connection with service.
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
            var channel = new ChannelFactory<ILibraryService>(binding);
            var endpoint = new EndpointAddress(uri);
            
            // Create proxy.
            var proxy = channel.CreateChannel(endpoint);
            try
            {
                proxy.GetBookInfo(0);
            }
            catch (Exception ex) when (ex is CommunicationException | ex is TimeoutException)
            {
                Console.WriteLine("Failed to connect with service.");
                return;
            }
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
            
            // variables for storing service outputs
            int[] bookIdentifiers = null;
            BookInfo bookInformation = null;
            
            // Main program loop.
            try {
                while (running)
                {
                    Console.WriteLine(operationString);
                    command = Console.ReadLine();
                    switch (command.ToLower())
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
                            Console.WriteLine(string.Join(" ", bookIdentifiers));
                            break;
                        case "2": // find book with given identifier.
                            try
                            {
                                Console.WriteLine("Enter identifier: ");
                                int identifier = Convert.ToInt32(Console.ReadLine());
                                bookInformation = proxy.GetBookInfo(identifier);
                                Console.WriteLine($"Book details:\nTitle: {bookInformation.title}");
                                int counter = 1;
                                foreach (AuthorInfo author in bookInformation.authors)
                                {
                                    Console.WriteLine($"Author {counter++}: {author.firstName} {author.lastName}");
                                }
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("Invalid identifier format.");
                            }
                            catch (OverflowException)
                            {
                                Console.WriteLine("Identifier is outside defined boundary.");
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
            catch (Exception ex) when (ex is CommunicationException | ex is TimeoutException) {
                Console.WriteLine("Connection with service close. Terminating program.");
            }
        }
        private static Uri ReadConfigurationURI()
        {
            string uriString = string.Format(
                "net.tcp://{0}:{1}/{2}",
                ConfigurationManager.AppSettings["ServiceAddress"],
                ConfigurationManager.AppSettings["ServicePort"],
                ConfigurationManager.AppSettings["ServiceName"]
            );
            return new Uri(uriString);
        }
    }
}
