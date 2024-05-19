using LibraryService.ServiceContracts;
using System;
using System.Configuration;
using System.ServiceModel;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // get Uniform Resource Identifier from configuration.
            string uriString = string.Format(
                "net.tcp://{0}:{1}/{2}",
                ConfigurationManager.AppSettings["Address"],
                ConfigurationManager.AppSettings["Port"],
                ConfigurationManager.AppSettings["ServiceName"]
            );
            Uri uri = new Uri(uriString);
            Console.WriteLine("Used URI: " + uriString);

            // Start service.
            ServiceHost host = new ServiceHost(typeof(LibraryServiceImpl), uri);
            try
            {
                var binding = new NetTcpBinding(SecurityMode.None);
                host.AddServiceEndpoint(typeof(ILibraryService), binding, "");
                host.Opened += Host_Opened;
                host.Closed += Host_Closed;
                host.Open();
                Console.WriteLine("Library Service is running.\nEnter q to exit");
                while(Console.ReadLine().ToLower().Equals("q") == false);
                host.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred during setup!\nAdditional info: ");
                Console.WriteLine(ex.ToString());
            }
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
