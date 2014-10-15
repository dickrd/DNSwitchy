using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace DNSwitchy
{
    public class Core
    {
        public List<NetworkInterface> Adapters { get; set; }
        public List<DnsServer> DnsServers { get; set; }

        public Core()
        {
            Adapters = getAdapters();
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
        public List<string> GetDns(NetworkInterface adapter)
        {
            List<string> dnsServerList = new List<string>();
            IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
            IPAddressCollection dnsServers = adapterProperties.DnsAddresses;
            foreach (var dnsServer in dnsServers)
            {
                dnsServerList.Add(dnsServer.ToString());
            }
            return dnsServerList;
        }

        private List<NetworkInterface> getAdapters()
        {
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            List<NetworkInterface> adapterList = new List<NetworkInterface>();
            foreach (var adapter in adapters)
            {
                if (check(adapter))
                {
                    adapterList.Add(adapter);
                }
            }
            return adapterList;
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
        private bool check(NetworkInterface adapter)
        {
            bool typeChecked = (adapter.NetworkInterfaceType != NetworkInterfaceType.Loopback && adapter.NetworkInterfaceType != NetworkInterfaceType.Tunnel);
            bool up = adapter.OperationalStatus == OperationalStatus.Up;
            if (typeChecked && up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
