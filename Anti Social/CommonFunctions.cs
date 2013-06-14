using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Threading;
using System.Security;
using System.Diagnostics;

namespace SocialSilence
{
    public class CommonFunctions: Page
    {
        public string filePath;
        //public System.Windows.Forms.NotifyIcon notifyIcon = null;


        public void btnClose_Click(object sender, RoutedEventArgs e)
        {

            if (sender.GetType().Name == "Button")
            {
                PasswordRequire.notifyIcon.Dispose();
               // System.Windows.Forms.NotifyIcon notifyIcon = objPass.notifyIcon;
                
                App.Current.MainWindow.Close();
                //Window.GetWindow(this).Close();
 
            }
            else if (((System.Windows.Controls.Page)(((System.Windows.Controls.ContentControl)(sender)).Content)).Title == "FinalPage")
            {
                //((WpfApplication2.FinalPage)(this)).
                //Window.GetWindow(this).Hide();
                App.Current.MainWindow.Hide();
            }
        }

        public void WriteToFileSuperFast(IEnumerable<string> domainList , string address , string filePath)
        {
            if (address == null)
            {
                File.AppendAllLines(filePath, (from r in domainList.AsParallel() select r));
            }
            else
            {

                File.AppendAllLines(filePath, (from r in domainList.AsParallel() select address + "  " + r));
            }
        }

        public void SettingRestore(string destination,string hostfile_location, bool restroreDNS)
        {
            FileAttributes hostattributes = File.GetAttributes(hostfile_location);

            if (hostattributes.ToString().Contains(FileAttributes.Hidden.ToString()) || hostattributes.ToString().Contains(FileAttributes.System.ToString()))
            {
                File.SetAttributes(hostfile_location, FileAttributes.Normal);
            }
            try
            {

                System.IO.File.Copy(destination + "host", hostfile_location, true);                         // Copy the backup host file to that of the present host file . 
            }
            catch
            {
                Thread.Sleep(1000);
                System.IO.File.Copy(destination + "host", hostfile_location, true);
            }
            
            if (restroreDNS)                                                                            // If dns was set to that of OpenDNS then change the dns to that of old one. 
            {
                string DNS_present;

                DNS_present = File.ReadAllText(destination + "DNSBackUp.bin");


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
            }
            hostattributes = File.GetAttributes(hostfile_location);

            if (hostattributes.ToString().Contains(FileAttributes.Hidden.ToString()) || hostattributes.ToString().Contains(FileAttributes.System.ToString()))
            {
                File.SetAttributes(hostfile_location, FileAttributes.Normal);
            }
        }


        public void SetLanguageDictionary(string userLanguage)
        {

            ResourceDictionary dictionary = new ResourceDictionary();

            try
            {
                switch(userLanguage)
                {
                    case "en":
                        dictionary.Source = new Uri(Environment.CurrentDirectory + @"\resources\string.en.xaml");
                        break;
                    case "de":
                        dictionary.Source = new Uri(Environment.CurrentDirectory + @"\resources\string.de.xaml");
                        break;
                    case "it":
                        dictionary.Source = new Uri(Environment.CurrentDirectory + @"\resources\string.it.xaml");
                        break;                            
                    default:
                        dictionary.Source = new Uri(Environment.CurrentDirectory+ @"\resources\string.en.xaml");
                       break;
                }
                App.Current.Resources.MergedDictionaries.Add(dictionary);
            }
            catch(Exception ex)
            {
                Uri test = new Uri(Environment.CurrentDirectory + @"/Resources/string.en.xaml");
                MessageBox.Show("A fault has occured on the common function page. " +ex.Message+ex.StackTrace);
            }

        }

     
    }
}
