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
            // Connect with service.
            ILibraryService proxy;
            try
            {
                proxy = ConnectToService();
            }
            catch (UriFormatException)
            {
                Console.WriteLine("Provided configuration parameters are invalid.");
                return;
            }
            catch (Exception ex) when (ex is CommunicationException | ex is TimeoutException)
            {
                Console.WriteLine("Failed to connect with service.");
                return;
            }
            Console.WriteLine("Connecting to service successful. Proxy created.");

            // program control variables
            bool running = true;
            string operationString =
                "Choose operation:\n" +
                "1.| Find books with keyword in title.\n" +
                "2.| Find book with given identifier.\n" +
                "q.| Quit.";

            // variables for storing service outputs
            int[] bookIdentifiers = null;
            BookInfo bookInformation = null;
            
            // Main program loop.
            try {
                while (running)
                {
                    Console.WriteLine(operationString);
                    switch (Console.ReadLine())
                    {
                        case "1": // find books with keyword in title.
                            bookIdentifiers = GetBooksIdentifiers(proxy);
                            break;
                        case "2": // find book with given identifier.
                            bookInformation = GetBookInformation(proxy);
                            break;
                        case "q": // Exit.
                            running = false;
                            Console.WriteLine("Exiting.");
                            break;
                        default: // Wrong option.
                            Console.WriteLine("Chosen operation does not exist.");
                            break;
                    }
                }
            }
            catch (Exception ex) when (ex is CommunicationException | ex is TimeoutException) {
                Console.WriteLine("Connection with service closed. Terminating program.");
            } finally
            {
                CloseProxy(proxy);
            }
        }

        public static ILibraryService ConnectToService()
        {
            // Read Uniform Resource Identifier from configuration.
            Uri uri = ReadConfigurationURI();
            Console.WriteLine($"Used URI: {uri}");

            // Establish connection with service.
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
            var channel = new ChannelFactory<ILibraryService>(binding);
            var endpoint = new EndpointAddress(uri);

            // Create proxy.
            var proxy = channel.CreateChannel(endpoint);

            // Call proxy operation to check if connection was successfully established
            try
            {
                proxy.GetBookInfo(0);
            } catch (FaultException<BookNotFound>) { }
            return proxy;
        }

        private static Uri ReadConfigurationURI()
        {
            // Read configuration from App.Config
            string uriString = string.Format(
                "{0}://{1}:{2}/{3}",
                ConfigurationManager.AppSettings["Protocol"],
                ConfigurationManager.AppSettings["ServiceAddress"],
                ConfigurationManager.AppSettings["ServicePort"],
                ConfigurationManager.AppSettings["ServiceName"]
            );
            return new Uri(uriString);
        }

        private static int[] GetBooksIdentifiers(ILibraryService proxy)
        {
            // Get keyword from user.
            Console.WriteLine("Enter keyword: ");
            string keyword = Console.ReadLine();

            // Get identifiers from service.
            int[] bookIdentifiers = proxy.FindBooks(keyword);
            
            // Display identifiers.
            if (bookIdentifiers == null || bookIdentifiers.Length == 0)
            {
                Console.WriteLine("No books with given keyword in title.");
                return new int[0];
            }
            Console.WriteLine($"Found identifiers:\n{string.Join(" ", bookIdentifiers)}");
            
            return bookIdentifiers;
        }

        private static BookInfo GetBookInformation(ILibraryService proxy)
        {
            try
            {
                // Read identifier from user.
                Console.WriteLine("Enter identifier: ");
                int identifier = Convert.ToInt32(Console.ReadLine());

                // Get book details from service.
                BookInfo bookInformation = proxy.GetBookInfo(identifier);

                // Display book details.
                Console.WriteLine($"Book details:\nTitle: {bookInformation.title}\nYear: {bookInformation.year}");
                int counter = 1;
                foreach (AuthorInfo author in bookInformation.authors)
                {
                    Console.WriteLine($"Author {counter++}: {author.firstName} {author.lastName}");
                }
                return bookInformation;
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid identifier format.");
                return null;
            }
            catch (OverflowException)
            {
                Console.WriteLine("Identifier is outside defined boundary.");
                return null;
            }
            catch (FaultException<BookNotFound> bookEx)
            {
                Console.WriteLine("Book with given identifier was not found.");
                return null;
            }
        }

        private static void CloseProxy(ILibraryService proxy)
        {
            if (proxy is ICommunicationObject communicationObject)
            {
                try
                {
                    // Close proxy gracefully (Allow operations to finish first).
                    communicationObject.Close();
                } catch
                {
                    // Force closing proxy if exception is thrown.
                    communicationObject.Abort();
                }
            }
        }
    }
}
