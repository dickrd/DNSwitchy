using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Linq;

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
        }

        public void UpdateData()
        {
            try
            {
                app.data.Load();

                interfaceList.ItemsSource = app.data.Interfaces;
                profileList.ItemsSource = app.data.Profiles;

                interfaceList.SelectedIndex = 0;
                profileList.SelectedIndex = 0;
            }
            catch (System.Exception exception)
            {
                MessageBox.Show(exception.Message);
                app.Shutdown(-1);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string message;

            (sender as Button).IsEnabled = false;
            try
            {
                message = apply();
            }
            catch (System.Exception exception)
            {
                message = exception.Message;
            }
            (sender as Button).IsEnabled = true;

            MessageBox.Show(message);
        }

        private string apply()
        {
            string message = "Succeed!", output = "\r\n";

            var currentProfile = profileList.SelectedItem as Data.Profile;
            var currentInterface = interfaceList.SelectedItem as NetworkInterface;

            if (currentProfile.StaticDns)
            {
                output = InterfaceManagement.SetStaticDns(currentInterface, currentProfile.DnsServer);
                if (output != "\r\n")
                {
                    message = output;
                    return message;
                }
            }
            else
            {
                output = InterfaceManagement.SetDhcpDns(currentInterface);
                if (output != "\r\n")
                {
                    message = output;
                    return message;
                }
            }

            if (currentProfile.StaticAddress)
            {
                output = InterfaceManagement.SetStaticAddress(currentInterface, currentProfile.Address, currentProfile.Gateway, currentProfile.Mask);
                if (output != "\r\n")
                {
                    message = output;
                    return message;
                }
            }
            else
            {
                output = InterfaceManagement.SetDhcpAddress(currentInterface);
                if (output != "\r\n")
                {
                    message = output;
                    return message;
                }
            }

            return message;
        }
    }
}
