using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace DNSwitchy
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public Data data { get; set; }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            data = new Data();
        }
    }
}
