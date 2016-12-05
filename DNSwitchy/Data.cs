using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Net.NetworkInformation;

namespace DNSwitchy
{
    public class Data
    {
        private IEnumerable<NetworkInterface> _interfaces;
        public IEnumerable<NetworkInterface> Interfaces
        {
            get
            {
                return _interfaces;
            }
        }

        private IEnumerable<Profile> _profiles;
        public IEnumerable<Profile> Profiles
        {
            get
            {
                return _profiles;
            }
        }

        public void Load()
        {
            _interfaces = InterfaceManagement.GetValidInterface();

            using (var profile = File.OpenRead(Profile.PATH))
            {
                _profiles = new DataContractJsonSerializer(typeof(List<Profile>)).ReadObject(profile) as List<Profile>;
            }
        }

        public class Profile
        {
            public static string PATH
            {
                get { return "profile.json"; }
            }

            public bool StaticAddress;
            public bool StaticDns;

            public string Address { get; set; }
            public string Mask { get; set; }
            public string Gateway { get; set; }
            public string DnsServer { get; set; }
        }
    }
}
