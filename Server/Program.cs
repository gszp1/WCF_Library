using LibraryService.ServiceContracts;
using System;
using System.Configuration;
using System.ServiceModel;
using LibraryService;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // get Uniform Resource Identifier from configuration.
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

            // Start service.
            ServiceHost host = new ServiceHost(typeof(LibraryServiceImpl), uri);
            try
            {
                var binding = new NetTcpBinding(SecurityMode.None);
                host.AddServiceEndpoint(typeof(ILibraryService), binding, "");
                host.Opened += Host_Opened;
                host.Closed += Host_Closed;
                host.Open();
                
                Console.WriteLine("Enter q to exit");
                while(Console.ReadLine().ToLower().Equals("q") == false);
                host.Close();
            }
            catch (AddressAlreadyInUseException)
            {
                Console.WriteLine("URI is already used!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred during setup!\nAdditional info: ");
                Console.WriteLine(ex.ToString());
            }
        }

        private static Uri ReadConfigurationURI()
        {
            // Read configuration from App.Config
            string uriString = string.Format(
                "{0}://{1}:{2}/{3}",
                ConfigurationManager.AppSettings["Protocol"],
                ConfigurationManager.AppSettings["Address"],
                ConfigurationManager.AppSettings["Port"],
                ConfigurationManager.AppSettings["ServiceName"]
            );
            return new Uri(uriString);
        }
        private static void Host_Opened(object sender, EventArgs e)
        {
            Console.WriteLine("Library service opened.");
        }

        private static void Host_Closed(object sender, EventArgs e)
        {
            Console.WriteLine("Library service closed.");
        }
    }
}
