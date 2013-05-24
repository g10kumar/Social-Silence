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
using System.Diagnostics;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using PSHostsFile;
using PSHostsFile.Core;
using System.Threading;
using SHDocVw;
using System.Windows.Forms;
using System.Management;
using System.IO.IsolatedStorage;
using System.IO;
using Xceed.Wpf.Toolkit;

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for AdittionalOptions.xaml
    /// </summary>
    public partial class AdittionalOptions : CommonFunctions
    {
        string blacklist_path = @"TextFiles\BlackList.txt";
        string address = "127.0.0.1";
        ObservableCollection<string> domainsToBlock = new ObservableCollection<string>();
        MessageBoxResult PopUpResult;
        List<string> domains_present = new List<string>();
        string hostfile_location = PSHostsFile.HostsFile.GetHostsPath();
        bool domainCustomized = false;
        bool openDnsUsed = false;
        bool popupBlocked;
        int hours = 0;
        int min = 0;

        public AdittionalOptions()
        {
            InitializeComponent();
        }
        public AdittionalOptions(ObservableCollection<string> theList, MessageBoxResult res,string path,bool customized ):this()
        {
            domainsToBlock = theList;
            PopUpResult = res;
            filePath = path;
            Hours.IsEnabled = false;
            Min.IsEnabled = false;
            domainCustomized = customized;
        }

        private void h1_RequestNavigate(object sender, RequestNavigateEventArgs e)                  // This method is to opendns url in internet explorer. 
        {
            string navigateUri = h1.NavigateUri.ToString();
            Process.Start(new ProcessStartInfo(navigateUri));
            e.Handled = true;
        }

        private void FinishSetting(object sender, RoutedEventArgs e)
        {
            //NavigationWindow win = (NavigationWindow)Window.GetWindow(this);                      // this code is to hide the current window after completing the task 
            //win.Hide();
            //Stopwatch watch = new Stopwatch();
           // watch.Start();
            XElement root = XElement.Load(@"TextFiles\XMLFile1.xml");
            List<string> finalblacksites = new List<string>();

            foreach (string site in domainsToBlock)                                                 // Populate the final blacklist with the domains to be blocked . 
            {
                IEnumerable<string> s = from el in root.Elements("site").AsParallel()
                                        where el.Element("name").Value.Equals(site)
                                        select el.Element("domain").Value.ToLower();

                finalblacksites.AddRange(s);

            }
       
            if ((bool)BlockPopUp.IsChecked)                                                         // This block will execute if user wants to block popup and ads also  
            {
                if (PopUpResult.Equals(MessageBoxResult.Yes))
                {
                    
                    IEnumerable<HostsFileEntry> listed_domains = PSHostsFile.HostsFile.Get(hostfile_location);
                    PSHostsFile.HostsFile.Set(listed_domains, blacklist_path);
                }                

                WriteToFileSuperFast(finalblacksites, address, blacklist_path);

                System.IO.File.Copy(blacklist_path, hostfile_location, true);

                popupBlocked = true;                            


            }
            else                                                                              // This block will execute if the user do not want to block popup and ads
            {
                if (PopUpResult.Equals(MessageBoxResult.Yes))
                {
                    IEnumerable<HostsFileEntry> listed_domains = PSHostsFile.HostsFile.Get(hostfile_location);
                    PSHostsFile.HostsFile.Set(listed_domains, @"TextFiles\host");
                }

                try
                {
                    WriteToFileSuperFast(finalblacksites, address, @"TextFiles\host");


                    System.IO.File.Copy(@"TextFiles\host", hostfile_location, true);
                }
                catch 
                {
                    Task.Delay(2000);
                    System.IO.File.Copy(@"TextFiles\host", hostfile_location, true);
                }

                popupBlocked = false; 
              
            }


            if ((bool)SetOpenDns.IsChecked)
            {
                BackUp_DNS();
                CreateNew_DNS();
                openDnsUsed = true;
                
            }

            
           // watch.Stop(); 

            CheckForOpenBrowser();
            int time;

            if (hours == 0 && min == 0)
            {
                time = 0;
            }
            else
            {
                time = (hours * 60) + (min); 
            }

            FinalPage finalObj = new FinalPage(domainCustomized, openDnsUsed, popupBlocked,time );

            NavigationService.Navigate(finalObj);
            finalObj.ShowsNavigationUI = false;          

            
           
        }

        private static void CheckForOpenBrowser()
        {
            Process[] currentProcess = Process.GetProcesses();
            List<string> browsers = new List<string>();
            MessageBoxResult result = MessageBoxResult.No;
            foreach (Process pro in currentProcess)
            {
                browsers.Add(pro.ProcessName);
            }
            if (browsers.Contains("firefox") || browsers.Contains("iexplore") || browsers.Contains("chrome")) // Checking for Instances of current browser running . 
            {
                result = System.Windows.MessageBox.Show("For the changes to take effect , browser need to be restarted.", "Message", MessageBoxButton.YesNo);
            }

            if (result == MessageBoxResult.Yes)
            {
                if (browsers.Contains("firefox"))
                {
                    RestartFireFox();
                }

                if (browsers.Contains("iexplore"))
                {
                    RestartInterExplorer();
                }
                if(browsers.Contains("chrome"))
                {
                    RestartChrome();
                }


            }
        }

        private static  void RestartFireFox()
        {
            Process[] firefox = Process.GetProcessesByName("firefox");

            foreach (Process item in firefox)
            {
                item.Kill();
                if (item.WaitForExit(2000))
                {
                    Process.Start("firefox.exe");
                }
            }
        }       

        private static void RestartInterExplorer()
        {          
            Process.Start("taskkill.exe", "/F /IM iexplore.exe");                   // First the application will try to close internet explorer through taskkill.
            Task.Delay(2000);
            Process[] iexplorer = Process.GetProcessesByName("iexplore");

            if (iexplorer.Count().Equals(0))
            {
                Process.Start("iexplore.exe");
            }
            else
            {                                                                       // If taskkill fails then IE is going to be close using process.kill . 
                List<DateTime> processTime = new List<DateTime>();
                Dictionary<int, int> process = new Dictionary<int, int>();
                List<int> processId = new List<int>();
                foreach (Process item in iexplorer)
                {
                    processTime.Add(item.StartTime);
                }
                foreach (Process item in iexplorer)
                {
                    if (item.StartTime == processTime.Min())
                    {
                        process.Add(item.Threads.Count, item.Id);
                        processId.Add(item.Threads.Count);
                    }
                }

                Process processToKill = Process.GetProcessById(process[processId.Min()]);
                processToKill.Kill();
                Thread.Sleep(1000);
                Process.Start("iexplore.exe");
            }
                           
            
        }

        private static void RestartChrome()
        {
            Process.Start("taskkill.exe", "/F /IM chrome.exe");
            Task.Delay(2000);
            Process[] chrome = Process.GetProcessesByName("chrome");
            if (chrome.Count().Equals(0))
            {
                Process.Start("chrome.exe");
            }
            else
            {
                List<DateTime> processTime = new List<DateTime>();
                Dictionary<int, int> process = new Dictionary<int, int>();
                List<int> processId = new List<int>();
                foreach (Process item in chrome)
                {
                    processTime.Add(item.StartTime);
                }
                foreach (Process item in chrome)
                {
                    if (item.StartTime == processTime.Min())
                    {
                        process.Add(item.Threads.Count, item.Id);
                        processId.Add(item.Threads.Count);
                    }
                }
                Process processToKill = Process.GetProcessById(process[processId.Min()]);
                processToKill.Kill();
                Thread.Sleep(1000);
                Process.Start("chrome.exe");
            }
        }

        private void MinimizeToSystemTray(object sender, RoutedEventArgs e)
        {
            //Window.
        }

        private void BackUp_DNS()
        {
            
            ManagementScope oMs = new ManagementScope("\\\\localhost\\root\\cimv2");

            string strQuery = "select DNSServerSearchOrder from Win32_NetworkAdapterConfiguration where IPEnabled='true'";

            ObjectQuery oQ = new ObjectQuery(strQuery);

            ManagementObjectSearcher oS = new ManagementObjectSearcher(oMs, oQ);

            ManagementObjectCollection oRc = oS.Get();

            IsolatedStorageFile dnsFile = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null);

            using (StreamWriter file = new StreamWriter(new IsolatedStorageFileStream("DNSBackUp.bin", FileMode.OpenOrCreate, FileAccess.Write, dnsFile)))
            {

                foreach (ManagementObject oR in oRc)
                {

                    foreach (string str in (Array)(oR.Properties["DNSServerSearchOrder"].Value))
                    {
                        if (str != "208.67.222.222" && str != "208.67.220.220")
                        {
                            file.WriteLine(str);
                        }
                    }

                }

            }


        }

        private void CreateNew_DNS()
        {
            using (var NWconManager = new ManagementClass("Win32_NetworkAdapterConfiguration"))
            {
                using (var NWconfig = NWconManager.GetInstances())
                {
                    foreach (var managementObject in NWconfig.Cast<ManagementObject>().Where(managmentObject => (bool)managmentObject["IPEnabled"]))
                    {
                        ManagementBaseObject managementBase = managementObject.GetMethodParameters("SetDNSServerSearchOrder");
                        if (managementBase != null)
                        {
                            string[] OpenDnsIP = { "208.67.222.222", "208.67.220.220" };
                            managementBase["DNSServerSearchOrder"] = OpenDnsIP;
                            managementObject.InvokeMethod("SetDNSServerSearchOrder", managementBase, null);
                        }

                    }
                }

            }

        }

        private void HourButtonSpinner_Spin(object sender, SpinEventArgs e)
        {
            ButtonSpinner spinner = (ButtonSpinner)sender;
            System.Windows.Controls.TextBox txtBox = (System.Windows.Controls.TextBox)spinner.Content;

            int value = String.IsNullOrEmpty(txtBox.Text) ? 0 : Convert.ToInt32(txtBox.Text);
            if (e.Direction == SpinDirection.Increase)
            {
                value++;
                if (value > 23)
                { value = 0; }
            }

            else
            {
                value--;
                if (value < 0)
                { value = 23; }
            }
            
            txtBox.Text = value.ToString();
            hours = value;
        }

        private void MinButtonSpinner_Spin(object sender, SpinEventArgs e)
        {
            ButtonSpinner spinner = (ButtonSpinner)sender;
            System.Windows.Controls.TextBox txtBox = (System.Windows.Controls.TextBox)spinner.Content;

            int value = String.IsNullOrEmpty(txtBox.Text) ? 0 : Convert.ToInt32(txtBox.Text);
            if (e.Direction == SpinDirection.Increase)
            {
                value++;
                if (value > 60)
                { value = 0; }
            }
            else
            {
                value--;
                if (value < 0)
                { value = 60; }
            }
            txtBox.Text = value.ToString();
            min = value;
        }

        private void DisableTimeSetting(object sender, RoutedEventArgs e)
        {
            SettingTime.IsEnabled = false;
        }

        private void EnableTimeSetting(object sender, RoutedEventArgs e)
        {
            SettingTime.IsEnabled = true;
        }

        private void DisableTimeSpinner(object sender, RoutedEventArgs e)
        {
            SettingForever.IsEnabled = true;
            Hours.IsEnabled = false;
            Min.IsEnabled = false;
        }

        private void EnableTimeSpinner(object sender, RoutedEventArgs e)
        {
            SettingForever.IsEnabled = false;
            Hours.IsEnabled = true;
            Min.IsEnabled = true;
        }
    }
}
