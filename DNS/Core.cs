using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Net.NetworkInformation;
using System.Diagnostics;

namespace DNSwitchy
{
	public class Core
	{
		public NetworkInterface[] Adapters { get; set; }
        public DnsServer[] DnsServers { get; set; }

		public Core()
		{
			Adapters = getAdapters();
            DnsServers = getDnsServers();
		}

		public string SetPrimaryDns(NetworkInterface adapter, string dnsServer)
		{
            string arguments, output;
            arguments = string.Format("interface ip set dns name=\"{0}\" static {1} validate=no", adapter.Name, dnsServer);
            output = runCommand("netsh.exe", arguments);
            return output;
		}
        public string SetSecondaryDns(NetworkInterface adapter, string dnsServer)
        {
            string arguments, output;
            arguments = string.Format("interface ip add dns name=\"{0}\" {1} index=2 validate=no", adapter.Name, dnsServer);
            output = runCommand("netsh.exe", arguments);
            return output;
        }
		public string[] GetDns(NetworkInterface adapter)
		{
			List<string> dnsServerList = new List<string>();
			IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
			IPAddressCollection dnsServers = adapterProperties.DnsAddresses;
			foreach (var dnsServer in dnsServers)
		    {
				dnsServerList.Add(dnsServer.ToString());
			}
			return dnsServerList.ToArray();
		}

		private NetworkInterface[] getAdapters()
		{
			NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
			return adapters;
		}
        private DnsServer[] getDnsServers()
        {
            List<DnsServer> dnsServerList = new List<DnsServer>();
            dnsServerList.Add(new DnsServer() { 
                Name = "Google",
                PrimaryAddress = "8.8.8.8",
                SecondaryAddress = "8.8.4.4"
            });
            dnsServerList.Add(new DnsServer()
            {
                Name = "Ali",
                PrimaryAddress = "223.8.8.8",
                SecondaryAddress = "223.8.4.4"
            });
            return dnsServerList.ToArray();
        }
        private string runCommand(string filename, string arguments)
        {
            Process process = new Process();
            string output;
            process.StartInfo.FileName = filename;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;

            process.Start();
            output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output;
        }
	}
}
