using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Management;
using Hardcodet.Wpf.TaskbarNotification;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Threading;

namespace SocialSilence
{
    public class CommonFunctions: Page
    {
        public string filePath;
        public TaskbarIcon tb;


        public void btnClose_Click(object sender, RoutedEventArgs e)
        {

            if (sender.GetType().Name == "Button")
            {
                tb = (TaskbarIcon)FindResource("MyNotifyIcon");
                tb.Dispose();
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


    }
}
