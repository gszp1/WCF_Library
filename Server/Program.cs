using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // get Uniform Resource Identifier from configuration
            string uriString = "http://" +
                ConfigurationManager.AppSettings["Address"] +
                ":" +
                ConfigurationManager.AppSettings["Port"] +
                "/" +
                ConfigurationManager.AppSettings["ServiceName"];
            Uri uri = new Uri(uriString);
            Console.WriteLine(uriString); // display URI

            ServiceHost host = new ServiceHost(typeof(LibraryServiceImpl), uri);
        }
    }
}
