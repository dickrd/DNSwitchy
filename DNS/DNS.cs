using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Net.NetworkInformation;

namespace DNSwitchy
{
	public class DNS
	{
		public NetworkInterface[] Adapters { get; set; }

		public DNS()
		{
			Adapters = GetAdapters();
		}
		public void SetDNS(string[] dnsServers)
		{
			using (var networkConfigMng = new ManagementClass("Win32_NetworkAdapterConfiguration"))
			{
				using (var networkConfigs = networkConfigMng.GetInstances())
				{
					foreach (var managementObject in networkConfigs.Cast<ManagementObject>().Where(objMO => (bool)objMO["IPEnabled"]))
					{
						using (var newDNS = managementObject.GetMethodParameters("SetDNSServerSearchOrder"))
						{
							newDNS["DNSServerSearchOrder"] = dnsServers;
							//managementObject.InvokeMethod("SetDNSServerSearchOrder", newDNS, null);
						}
					}
				}
			}
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
	}
}
