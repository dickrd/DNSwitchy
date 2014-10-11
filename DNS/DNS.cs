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
	public class DNS
	{
		public NetworkInterface[] Adapters { get; set; }

		public DNS()
		{
			Adapters = GetAdapters();
		}
		public string SetPrimaryDNS(NetworkInterface adapter, string dnsServer)
		{
            string arguments, output;
            arguments = string.Format("interface ip set dns name=\"{0}\" static {1}", adapter.Name, dnsServer);
            output = RunCommand("netsh.exe", arguments);
            return output;
		}
        public string SetSecondaryDNS(NetworkInterface adapter, string dnsServer)
        {
            string arguments, output;
            arguments = string.Format("interface ip add dns name=\"{0}\" {1} index=2", adapter.Name, dnsServer);
            output = RunCommand("netsh.exe", arguments);
            return output;
        }
		public string[] GetDNS(NetworkInterface adapter)
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
		public NetworkInterface[] GetAdapters()
		{
			NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
			return adapters;
		}
        private string RunCommand(string filename, string arguments)
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
