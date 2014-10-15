using System;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNSwitchy
{
    public class DnsServer
    {
        private static string path = "DnsServer";
        public static string Path 
        { 
            get
            {
                return path;
            }
            set
            {
                path = value;
            }
        }
        private static string url = "http://mattli.ml/dnsserver";
        public static string Url
        {
            get
            {
                return url;
            }
            set
            {
                url = value;
            }
        }
        public string Name { get; set; }
        public string PrimaryAddress { get; set; }
        public string SecondaryAddress { get; set; }
        public string PingValue 
        { 
            get
            {
                return pingTest(this);
            }
        }

        private string pingTest(DnsServer server)
        {
            Ping ping = new Ping();
            var reply = ping.Send(server.PrimaryAddress);
            if (reply.Status == IPStatus.Success)
            {
                return reply.RoundtripTime.ToString() + " ms";
            }
            else
            {
                return reply.Status.ToString();
            }
        }
    }
}
