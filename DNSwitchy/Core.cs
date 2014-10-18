using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.IO;
using System.Net.Http;

namespace DNSwitchy
{
    public class Core
    {
        public IEnumerable<NetworkInterface> Adapters
        {
            get
            {
                NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
                foreach (var adapter in adapters)
                {
                    if (validate(adapter))
                    {
                        yield return adapter;
                    }
                }
            }
        }
        public IEnumerable<DnsServer> DnsServers
        {
            get
            {
                checkUpdate();
                foreach (var dnsServer in File.ReadLines(DnsServer.Path))
                {
                    yield return new DnsServer()
                    {
                        Name = dnsServer.Split(',')[0],
                        PrimaryAddress = dnsServer.Split(',')[1],
                        SecondaryAddress = dnsServer.Split(',')[2]
                    };
                }
            }
        }
        private double currentVersion = -1;
        public double CurrentVersion
        {
            get
            {
                if (currentVersion < 0 && !File.Exists(DnsServer.Path))
                {
                    currentVersion = 0;
                }
                else
                {
                    using (var line = File.ReadLines(DnsServer.Path).GetEnumerator())
                    {
                        line.MoveNext();
                        currentVersion = double.Parse(line.Current.Split(',')[1]);
                    }
                }
                return currentVersion;
            }
            private set
            {
                currentVersion = value;
            }
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
        private bool validate(NetworkInterface adapter)
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
        private void checkUpdate()
        {
            double version;
            using (var httpClient = new HttpClient())
            using (var response = httpClient.GetStringAsync(DnsServer.Url))
            {
                version = double.Parse(response.Result.Split('\n')[0].Split(',')[1]);
                if (version <= CurrentVersion)
                {
                    return;
                }
                File.WriteAllText(DnsServer.Path, response.Result);
                CurrentVersion = version;
            }
        }
    }
}
