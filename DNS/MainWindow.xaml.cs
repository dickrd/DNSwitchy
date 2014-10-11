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

        public void UpdateAdapters()
        {
            adapter.ItemsSource = app.dns.Adapters;
        }

        private void adapter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dns.Text = " ";
            foreach (var server in app.dns.GetDNS((adapter.SelectedItem as NetworkInterface)))
            {
                dns.Text += server + " ";
            }
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string message = app.dns.SetSecondaryDNS(adapter.SelectedItem as NetworkInterface, dns.Text);
            MessageBox.Show(message);
        }
    }
}
