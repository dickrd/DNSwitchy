using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Diagnostics;

namespace DNSwitchy
{
    public class InterfaceManagement
    {
        public static IEnumerable<NetworkInterface> GetValidInterface()
        {
            var interfaceArray = NetworkInterface.GetAllNetworkInterfaces().Where(
                item => item.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                item.NetworkInterfaceType != NetworkInterfaceType.Tunnel &&
                item.OperationalStatus == OperationalStatus.Up);
            return interfaceArray;
        }

        public static string SetStaticAddress(NetworkInterface theInterface, string address, string gateway, string mask)
        {
            string arguments, output;
            arguments = string.Format("interface ip set address {0} static {1} {2} {3}", theInterface.Name, address, mask, gateway);
            output = runCommand("netsh.exe", arguments);
            return output;
        }

        public static string SetDhcpAddress(NetworkInterface theInterface)
        {
            string arguments, output;
            arguments = string.Format("interface ip set address {0} dhcp", theInterface.Name);
            output = runCommand("netsh.exe", arguments);
            return output;
        }

        public static string SetDhcpDns(NetworkInterface theInterface)
        {
            string arguments, output;
            arguments = string.Format("interface ip set dnsservers {0} dhcp", theInterface.Name);
            output = runCommand("netsh.exe", arguments);
            return output;
        }

        public static string SetStaticDns(NetworkInterface theInterface, string dns)
        {
            string arguments, output;
            arguments = string.Format("interface ip set dns name=\"{0}\" static {1} validate=no", theInterface.Name, dns);
            output = runCommand("netsh.exe", arguments);
            return output;
        }

        private static string runCommand(string filename, string arguments)
        {
            string output = "\r\n";
            using (Process process = new Process())
            {
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = filename,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                process.Start();
                output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
            }
            return output;
        }
    }
}
