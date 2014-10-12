using System;
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
            CurrentMachine = new Core();
        }
    }
}
