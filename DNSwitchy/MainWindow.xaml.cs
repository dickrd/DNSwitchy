using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace DNSwitchy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        App app;
        public MainWindow()
        {
            app = Application.Current as App;
            InitializeComponent();
            UpdateData();
            adapterList.SelectedIndex = 0;
        }

        public void UpdateData()
        {
            adapterList.ItemsSource = app.CurrentMachine.Adapters;
            dnsServerList.ItemsSource = app.CurrentMachine.DnsServers;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(save());
        }
        private void dnsServerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((dnsServerList.SelectedItem as DnsServer).Name == "Current")
            {
                currentPrimaryDns.IsReadOnly = false;
                currentSecondaryDns.IsReadOnly = false;
            }
            else
            {
                currentPrimaryDns.IsReadOnly = true;
                currentSecondaryDns.IsReadOnly = true;
            }
        }
        private void adapterList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DnsServer current = app.CurrentMachine.DnsServers.Find(x => x.Name == "Current");
            List<string> currentDns = app.CurrentMachine.GetDns(adapterList.SelectedItem as NetworkInterface);
            if (currentDns.Count >= 2)
            {
                current.PrimaryAddress = currentDns[0];
                current.SecondaryAddress = currentDns[1];
            }
            else if (currentDns.Count == 1)
            {
                current.PrimaryAddress = currentDns[0];
                current.SecondaryAddress = "";
            }
            else
            {
                current.PrimaryAddress = "";
                current.SecondaryAddress = "";
            }
        }

        private string save()
        {
            string message, primaryOutput, secondaryOutput;
            primaryOutput = app.CurrentMachine.SetPrimaryDns(adapterList.SelectedItem as NetworkInterface, currentPrimaryDns.Text);
            secondaryOutput = app.CurrentMachine.SetSecondaryDns(adapterList.SelectedItem as NetworkInterface, currentSecondaryDns.Text);
            if (primaryOutput == "\r\n" && secondaryOutput == "\r\n")
            {
                message = "Succeed!";
            }
            else
            {
                message = primaryOutput == "\r\n" ? secondaryOutput : primaryOutput;
            }
            return message;
        }
    }
}
