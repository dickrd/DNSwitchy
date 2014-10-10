using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.NetworkInformation;

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
            UpdateAdapters();
        }

        public void UpdateAdapters(){
            foreach (var nic in app.dns.Adapters)
            {
                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Content = nic.Name;
                cbi.DataContext = nic;
                adapter.Items.Add(cbi);
            }
        }

        private void adapter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dns.Text = "";
            foreach (var server in app.dns.GetDNS((adapter.SelectedItem as ComboBoxItem).DataContext as NetworkInterface))
            {
                dns.Text += server;
            }
        }
    }
}
