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
using System.Windows.Forms;
using System.Management;
using System.IO.IsolatedStorage;
using System.IO;
using Xceed.Wpf.Toolkit;

namespace SocialSilence
{
    /// <summary>
    /// Interaction logic for AdittionalOptions.xaml
    /// </summary>
    public partial class AdittionalOptions : CommonFunctions
    {
        string blacklist_path = @"Resources\BlackList.txt";
        string address = "127.0.0.120";
        ObservableCollection<string> domainsToBlock = new ObservableCollection<string>();
        MessageBoxResult PopUpResult;
        List<string> domains_present = new List<string>();
        string hostfile_location = PSHostsFile.HostsFile.GetHostsPath();
        bool domainCustomized = false;
        bool openDnsUsed = false;
        bool popupBlocked;
        int hours = 0;
        int min = 0;
        List<string> domainsFromHostFile = new List<string>();
        List<string> browserToclose = new List<string>();

        public AdittionalOptions()
        {
            InitializeComponent();
        }
        public AdittionalOptions(ObservableCollection<string> theList, MessageBoxResult res,bool customized , List<string> fromHost ):this()
        {
            domainsToBlock = theList;                                                               // This is the list of names of domains that are going to be blocked . 
            PopUpResult = res;                                                                      // This is the message box value if the user wants to add domains from host file.
            Hours.IsEnabled = false;
            Min.IsEnabled = false;
            domainCustomized = customized;                                                          // This is the bool value to show that the domain list has been customized or not.
            domainsFromHostFile = fromHost;
            this.Loaded += AdittionalOptions_Loaded;
        }

        void AdittionalOptions_Loaded(object sender, RoutedEventArgs e)
        {
           this.NavigationService.Navigating +=NavigationService_Navigating;
        }

        void NavigationService_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                ShowsNavigationUI = false;
            }
        }

        private void h1_RequestNavigate(object sender, RequestNavigateEventArgs e)                  // This method is to opendns url in internet explorer. 
        {
            string navigateUri = h1.NavigateUri.ToString();
            Process.Start(new ProcessStartInfo(navigateUri));
            e.Handled = true;
        }

        private void FinishSetting(object sender, RoutedEventArgs e)
        {
            if (hourTextBox.Text != null && !hourTextBox.Text.Equals(""))
            hours = Convert.ToInt32(hourTextBox.Text);

            if (minTextBox.Text != null && !minTextBox.Text.Equals(""))
            min = Convert.ToInt32(minTextBox.Text);
           //Stopwatch watch = new Stopwatch();
           // watch.Start();
            XElement root = XElement.Load(@"Resources\XMLFile1.xml");
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

                    try
                    {

                        WriteToFileSuperFast(domainsFromHostFile, null, blacklist_path);

                    }
                    catch
                    {
                        Thread.Sleep(2000);
                        WriteToFileSuperFast(domainsFromHostFile, null, blacklist_path);

                    }
                }                

                WriteToFileSuperFast(finalblacksites, address, blacklist_path);

                System.IO.File.Copy(blacklist_path, hostfile_location, true);

                popupBlocked = true;                            


            }
            else                                                                              // This block will execute if the user do not want to block popup and ads
            {
                if (PopUpResult.Equals(MessageBoxResult.Yes))                      
                {
                    try
                    {

                        WriteToFileSuperFast(domainsFromHostFile, null, @"Resources\host");

                    }
                    catch
                    {
                        Thread.Sleep(2000);
                        WriteToFileSuperFast(domainsFromHostFile, null, @"Resources\host");

                    }

                }

                try
                {
                    WriteToFileSuperFast(finalblacksites, address, @"Resources\host");


                    System.IO.File.Copy(@"Resources\host", hostfile_location, true);
                }
                catch 
                {
                    Thread.Sleep(2000);
                    System.IO.File.Copy(@"Resources\host", hostfile_location, true);
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
                if (hours > 23)
                {
                    hours = 23;
                    if (min > 60)
                    {
                        min = 60;
                    }
                }
                time = (hours * 60) + (min);
                if (time > 1440)
                {
                    time = 1440;
                }
            }

            FinalPage finalObj = new FinalPage(domainCustomized, openDnsUsed, popupBlocked,time);
            finalObj.ShowsNavigationUI = false;
            NavigationService.Navigate(finalObj);
            //finalObj.ShowsNavigationUI = false;          

            
           
        }

        private void CheckForOpenBrowser()
        {

            string browserMessage = (string)this.FindResource("BrowserMessage");
            Process[] currentProcess = Process.GetProcesses();
            List<string> browsers = new List<string>();
            MessageBoxResult result = MessageBoxResult.No;
            foreach (Process pro in currentProcess)
            {
                browsers.Add(pro.ProcessName);
            }
            if (browsers.Contains("firefox") || browsers.Contains("iexplore") || browsers.Contains("chrome") || browsers.Contains("opera") || browsers.Contains("Safari"))// Checking for Instances of current browser running . 
            {

                result = Xceed.Wpf.Toolkit.MessageBox.Show(browserMessage, "Message", MessageBoxButton.YesNo);
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
                if (browsers.Contains("chrome"))
                {
                    RestartChrome();
                }
                if(browsers.Contains("opera"))
                {
                    RestartOpera();
                }
                if(browsers.Contains("Safari"))
                {
                    RestartSafari();
                }

                if (!browserToclose.Count().Equals(0))
                {

                    Xceed.Wpf.Toolkit.MessageBox.Show("Please close these browsers manually\n " + string.Join(Environment.NewLine, browserToclose));
                }

            }
        }

        private void RestartInterExplorer()
        {
            browserToclose.Add("Internet Explorer");
        }

        private void RestartSafari()
        {
            browserToclose.Add("Safari");
            
        }

        private void RestartOpera()
        {
            Process.Start("taskkill.exe", "/F /IM opera.exe*");
            Thread.Sleep(500);
            Process[] opera = Process.GetProcessesByName("opera");
            if (opera.Count().Equals(0))
            {
                Process.Start("opera.exe");
            }
            else
            {
                List<DateTime> processTime = new List<DateTime>();
                Dictionary<int, int> process = new Dictionary<int, int>();
                List<int> processId = new List<int>();
                foreach (Process item in opera)
                {
                    processTime.Add(item.StartTime);
                }
                foreach (Process item in opera)
                {
                    if (item.StartTime == processTime.Min())
                    {
                        process.Add(item.Threads.Count, item.Id);
                        processId.Add(item.Threads.Count);
                    }
                }
                Process processToKill = Process.GetProcessById(process[processId.Min()]);
                processToKill.Kill();
                Thread.Sleep(500);
                Process.Start("opera.exe");
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


        private static void RestartChrome()
        {
            Process.Start("taskkill.exe", "/F /IM chrome.exe");
            Thread.Sleep(500);
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
                Thread.Sleep(500);
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
                    if (!(oR.Properties["DNSServerSearchOrder"].Value == null))
                    {
                      
                        foreach (string str in (Array)(oR.Properties["DNSServerSearchOrder"].Value))
                        {
                            //if (str != "208.67.222.222" && str != "208.67.220.220")           // in Case if the user is using the OpenDns already
                            //{
                            //    file.WriteLine(str);
                            //}
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
            hourTextBox.Clear();
            minTextBox.Clear();
        }

        private void EnableTimeSpinner(object sender, RoutedEventArgs e)
        {
            SettingForever.IsEnabled = false;
            Hours.IsEnabled = true;
            Min.IsEnabled = true;
        }
    }
}
