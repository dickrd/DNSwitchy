﻿using System;
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
            UpdateData();
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

        private string save()
        {
            string message;
            message = app.CurrentMachine.SetPrimaryDns(adapterList.SelectedItem as NetworkInterface, currentPrimaryDns.Text);
            message += app.CurrentMachine.SetSecondaryDns(adapterList.SelectedItem as NetworkInterface, currentSecondaryDns.Text);
            return message;
        }
    }
}
