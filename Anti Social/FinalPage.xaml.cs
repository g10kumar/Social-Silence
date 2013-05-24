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
using PSHostsFile;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for FinalPage.xaml
    /// </summary>
    public partial class FinalPage : CommonFunctions
    {
        bool domainCustomized;
        bool openDnsused;
        bool popUpBlocked;
        int hour;
        int min;
        int seconds= 0;
        bool timerSet = false;
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);


        public FinalPage()
        {
            InitializeComponent();
        }
        public FinalPage(bool domcus, bool openDns , bool popup, int time ):this()
        {
            domainCustomized = domcus;
            openDnsused = openDns;
            popUpBlocked = popup;
            Loaded += FinalPage_Loaded;
            if (time != 0)
            {
                hour = time / 60;
                min = time % 60;
                timerSet = true;
            }
        }

        void FinalPage_Loaded(object sender, RoutedEventArgs e)
        {
            NavigationWindow win = (NavigationWindow)Window.GetWindow(this);
            win.Closing += win_Closing;
            var hwnd = new WindowInteropHelper(win).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);

            ((NavigationWindow)LogicalTreeHelper.GetParent(this)).ResizeMode = ResizeMode.NoResize;
            if (domainCustomized)
            {
                domainsAre.Text = "Customized By User.";
            }
            else
            {            
                domainsAre.Text = "Default set of Doamins Used.";
            }

            if (openDnsused)
            {
                ManagementScope oMs = new ManagementScope("\\\\localhost\\root\\cimv2");

                string strQuery = "select DNSServerSearchOrder from Win32_NetworkAdapterConfiguration where IPEnabled='true'";

                ObjectQuery oQ = new ObjectQuery(strQuery);

                ManagementObjectSearcher oS = new ManagementObjectSearcher(oMs, oQ);

                ManagementObjectCollection oRc = oS.Get();

                foreach (ManagementObject oR in oRc)
                {

                    foreach (string str in (Array)(oR.Properties["DNSServerSearchOrder"].Value))
                    {
                        if (str == "208.67.222.222" || str == "208.67.220.220")
                        {
                            openDnsIs.Text = "OpenDNS is configured as the DNS of this system.";
                        }
                        else
                        {
                            openDnsIs.Text = "Application was unbale to configure OpenDNS on this system.";
                        }
                    }

                }
                              
            }
            else
            {
                openDnsIs.Text = "OpenDNS is not configured on this system.";               
             
            }

            if (popUpBlocked)
            {
                string hostfile_location = PSHostsFile.HostsFile.GetHostsPath();
                string[] lines = File.ReadAllLines(hostfile_location);
                string matchLine = "#@popup and Ads blocked";
                foreach (string line in lines)
                {
                    if (Regex.IsMatch(line, matchLine))
                    {
                        PopUpAre.Text = "PopUp And Ads are blocked on this system. ";
                        break;
                    }
                }
                
            }
            else
            {
                PopUpAre.Text = "PopUp And Ads are not blocked on this system.";
            }

            if (timerSet)
            {
                countdownTimer();
            }
            else 
            {
                hours.Text = "\u221e";
                minutes.Text = "\u221e";
                sec.Text = "\u221e";                  // Or someplaces the infinity sign can be shown as &#x221E; in xaml.

            }



        }

        void win_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBox.Show("Are you sure you want to close the applicaiton . ");
            //e.Cancel = true;
        }

        private void countdownTimer()
        { 
            timer.Tick += timer_Tick;
            timer.Interval = 998;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {            
            seconds--;
            if (seconds < 0)
            {
                seconds = 60;
                min--;
            }
            if (min < 0)
            {
                min = 59;
                hour--;
            }
            hours.Text = hour.ToString();
            minutes.Text = min.ToString();
            sec.Text = seconds.ToString();
            if (hour == 0 && min == 0 && seconds == 0)
            {
                timer.Stop();
            }




        }

        private void RestoreApplication(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
