using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNSwitchy
{
    public class DnsServer
    {
        private static string path = "Server.dns";
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
    }
}
