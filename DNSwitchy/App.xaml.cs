using System;
using System.IO;
using System.Net.Http;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DNSwitchy
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public Core CurrentMachine { get; set; }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            getThingsReady();
        }
        private void getThingsReady()
        {
            CurrentMachine = new Core();
            downloadServerList();
            List<DnsServer> dnsServerList = new List<DnsServer>();
            foreach (var dnsServer in File.ReadLines(DnsServer.Path))
            {
                dnsServerList.Add(new DnsServer() {
                    Name = dnsServer.Split(',')[0],
                    PrimaryAddress = dnsServer.Split(',')[1],
                    SecondaryAddress = dnsServer.Split(',')[2]
                });
            }
            CurrentMachine.DnsServers = dnsServerList;
        }
        private void downloadServerList()
        {
            using (var httpClient = new HttpClient())
            {
                var response = httpClient.GetStringAsync(DnsServer.Url);
                if (File.Exists(DnsServer.Path) && double.Parse(response.Result.Split('\n')[0].Split(',')[1]) <= double.Parse(File.ReadAllLines(DnsServer.Path)[0].Split(',')[1]))
                {
                    return;
                }
                File.WriteAllText(DnsServer.Path, response.Result);
            }
        }
    }
}
