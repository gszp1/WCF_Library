using System;
using System.Configuration;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string uriString = string.Format(
                "net.tcp//{0}:{1}/{2}",
                ConfigurationManager.AppSettings["ServiceAddress"],
                ConfigurationManager.AppSettings["ServicePort"],
                ConfigurationManager.AppSettings["ServiceName"]
            );
            Uri uri = new Uri(uriString);
            Console.WriteLine($"Used uri: { uriString}");
        }
    }
}
