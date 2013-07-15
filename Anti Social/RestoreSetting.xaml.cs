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
using System.Management;
using System.IO.IsolatedStorage;
using System.IO;
using System.Threading;

namespace SocialSilence
{
    /// <summary>
    /// Interaction logic for RestoreSetting.xaml
    /// </summary>
    public partial class RestoreSetting : Page
    {

        bool currentOpenDNS;
        string hostfile_location;
        public RestoreSetting()
        {
            InitializeComponent();
            this.Loaded += RestoreSetting_Loaded;
        }

        public RestoreSetting(string hostfile): this()
        {
            hostfile_location = hostfile;
        }

        void RestoreSetting_Loaded(object sender, RoutedEventArgs e)                        // Here we are checking if the DNS needs to be restored or not . 
        {
            ManagementScope oMs = new ManagementScope("\\\\localhost\\root\\cimv2");

            string strQuery = "select DNSServerSearchOrder from Win32_NetworkAdapterConfiguration where IPEnabled='true'";

            ObjectQuery oQ = new ObjectQuery(strQuery);

            ManagementObjectSearcher oS = new ManagementObjectSearcher(oMs, oQ);

            ManagementObjectCollection oRc = oS.Get();

            IsolatedStorageFile dnsFile = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null);
                      
            foreach (ManagementObject oR in oRc)
                {
                    if (!(oR.Properties["DNSServerSearchOrder"].Value == null))
                    {

                        foreach (string str in (Array)(oR.Properties["DNSServerSearchOrder"].Value))
                        {
                            if (str == "208.67.222.222" || str == "208.67.220.220")           // in Case if the user is using the OpenDns already
                            {
                                currentOpenDNS = true;
                            }
                            
                        }
                    }

                }

            if (currentOpenDNS)
            {

                string DNS_present;
                DNS_present = File.ReadAllText(PasswordRequire.filePath + "DNSBackUp.bin");
                string[] oldDNS = DNS_present.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string dns in oldDNS)
                {
                    if (dns == "208.67.222.222" || dns == "208.67.220.220")                     // If this is true , then the user was already using openDNS 
                    {
                        RestoreDNS.Visibility = System.Windows.Visibility.Hidden;
                        break;
                    }

                }
            }

            
        }


        private void Restore(object sender, RoutedEventArgs e)                                          // This function is executed on clicking the restore button .
        {
            try
            {

                System.IO.File.Copy(PasswordRequire.filePath + "host", hostfile_location, true);                         // Copy the backup host file to that of the present host file . 
            }
            catch
            {
                Thread.Sleep(1000);
                System.IO.File.Copy(PasswordRequire.filePath + "host", hostfile_location, true);
            }
            if ((bool)RestoreDNS.IsChecked)
            {
                string DNS_present = File.ReadAllText(PasswordRequire.filePath + "DNSBackUp.bin");
                List<string> nullenumerable = new List<string>();
                string[] oldDNS = DNS_present.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                using (var NWconManager = new ManagementClass("Win32_NetworkAdapterConfiguration"))
                {
                    using (var NWconfig = NWconManager.GetInstances())
                    {
                        foreach (var managementObject in NWconfig.Cast<ManagementObject>().Where(managmentObject => (bool)managmentObject["IPEnabled"]))
                        {
                            ManagementBaseObject managementBase = managementObject.GetMethodParameters("SetDNSServerSearchOrder");
                            if (managementBase != null)
                            {
                                managementBase["DNSServerSearchOrder"] = oldDNS;
                                managementObject.InvokeMethod("SetDNSServerSearchOrder", managementBase, null);

                            }

                        }
                    }
                }
                nullenumerable.Add("");
                File.Create(PasswordRequire.filePath + "DNSBackUp.bin");

            }
            Xceed.Wpf.Toolkit.MessageBox.Show("Setting has been restored . ");
            StartPage startObj = new StartPage();
            NavigationService.Navigate(startObj);
        }
    }
}
