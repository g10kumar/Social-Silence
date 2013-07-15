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
using System.IO.IsolatedStorage;
using System.Reflection;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Diagnostics;


namespace SocialSilence
{
    /// <summary>
    /// Interaction logic for FinalPage.xaml
    /// </summary>
    public partial class FinalPage : CommonFunctions
    {
        
        static string hostfile_location = PSHostsFile.HostsFile.GetHostsPath();
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
        public static SocialSilence.HttpServer httpserver = new HttpServer(50);
        FileSystemWatcher watcher = new FileSystemWatcher(System.IO.Path.GetDirectoryName(hostfile_location));

        public static int GetWindowLong(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 4)
            {
                return GetWindowLong32(hWnd, nIndex);
            }
            return GetWindowLongPtr64(hWnd, nIndex);
        }


        [DllImport("user32.dll", EntryPoint = "GetWindowLong", CharSet = CharSet.Auto)]
        private static extern int GetWindowLong32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr", CharSet = CharSet.Auto)]
        private static extern int GetWindowLongPtr64(IntPtr hWnd, int nIndex);


        public static int SetWindowLongPtr(IntPtr hWnd, int nIndex, int dwNewLong)
        {
            if (IntPtr.Size == 8)
                return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
            else
                return SetWindowLong32(hWnd, nIndex, dwNewLong);
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        private static extern int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        private static extern int SetWindowLongPtr64(IntPtr hWnd, int nIndex, int dwNewLong);


        public FinalPage()
        {
            
            InitializeComponent();
        }

        public FinalPage(bool domcus, bool openDns , bool popup, int time ):this()
        {
            domainCustomized = domcus;
            openDnsused = openDns;
            popUpBlocked = popup;
            if (time != 0)
            {
                hour = time / 60;
                min = time % 60;
                timerSet = true;
            }
            //this.PreviewKeyDown += FinalPage_PreviewKeyDown;
            Loaded += FinalPage_Loaded;
            
            
        }

        void FinalPage_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (((System.Windows.Controls.Page)(((System.Windows.Controls.ContentControl)(sender)).Content)).Title == "FinalPage")
            {
                if (e.Key == Key.Escape)
                {
                    e.Handled = true;
                    App.Current.MainWindow.Hide();
                    CommonFunctions.isHidden = true;
                }
            }
        }

        //private void KeyPress(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Escape)
        //    {
        //        if (((System.Windows.Controls.Page)(((System.Windows.Controls.ContentControl)(sender)).Content)).Title == "FinalPage")
        //        {
        //            //((NavigationWindow)LogicalTreeHelper.GetParent((Page)sender)).Hide();
        //            ((NavigationWindow)sender).Hide();
        //        }
        //        if (((System.Windows.Controls.Page)(((System.Windows.Controls.ContentControl)(sender)).Content)).Title == "ApplicationFinished")// if this is not used then 
        //        {                                                                                                   // the cancel function defined is executed. 
        //            FinalPage finalObj = new FinalPage();
        //            ((NavigationWindow)sender).NavigationService.Navigate(finalObj);


        //        }

        //    }

        //}
        async void FinalPage_Loaded(object sender, RoutedEventArgs e)
        {
            
            //((NavigationWindow)LogicalTreeHelper.GetParent(this)).PreviewKeyDown += new KeyEventHandler(KeyPress);
            NavigationWindow win = (NavigationWindow)Window.GetWindow(this);
            var hwnd = new WindowInteropHelper(win).Handle;
            win.KeyDown += FinalPage_PreviewKeyDown;
            File.SetAttributes(hostfile_location, FileAttributes.Hidden | FileAttributes.ReadOnly | FileAttributes.System); // Here we are setting the host file attributes.
            watcher.Filter = "hosts";
            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Attributes | NotifyFilters.Size;
            watcher.Changed += watcher_Changed;
           
            SetWindowLongPtr(hwnd, GWL_STYLE, StartPage.getwindow & ~WS_SYSMENU);

            string domainStatus;
            if (domainCustomized)
            {
                domainStatus = (string)this.FindResource("DomainsCustomized");
                domainsAre.Text = domainStatus;
            }
            else
            {
                domainStatus = (string)this.FindResource("DomainsDefault");
                domainsAre.Text = domainStatus;
            }            
                ManagementScope oMs = new ManagementScope("\\\\localhost\\root\\cimv2");

                string strQuery = "select DNSServerSearchOrder from Win32_NetworkAdapterConfiguration where IPEnabled='true'";

                ObjectQuery oQ = new ObjectQuery(strQuery);

                ManagementObjectSearcher oS = new ManagementObjectSearcher(oMs, oQ);

                ManagementObjectCollection oRc = oS.Get();

                string OpenDnsstatus;
                string PopUPstatus;
                foreach (ManagementObject oR in oRc)
                {
                    if (oR.Properties["DNSServerSearchOrder"].Value != null)
                    {
                        foreach (string str in (Array)(oR.Properties["DNSServerSearchOrder"].Value))
                        {
                            if (str == "208.67.222.222" && openDnsused)
                            {
                                OpenDnsstatus = (string)this.FindResource("OpenDnsc");
                                openDnsIs.Text = OpenDnsstatus;
                                break;
                            }
                            else if (str != "208.67.222.222" && !openDnsused)
                            {

                                OpenDnsstatus = (string)this.FindResource("OpenDnsnc");
                                openDnsIs.Text = OpenDnsstatus;
                                break;

                            }
                            else if (str == "208.67.222.222")
                            {

                                OpenDnsstatus = (string)this.FindResource("OpenDnsac");
                                openDnsIs.Text = OpenDnsstatus;
                                break;

                            }
                        }
                    }
                    else
                    {
                        if(openDnsused)
                        {
                            OpenDnsstatus = (string)this.FindResource("UnableToOpenDns");
                            openDnsIs.Text = OpenDnsstatus;
                        }
                        else
                        {
                            OpenDnsstatus = (string)this.FindResource("OpenDnsnc");
                            openDnsIs.Text = OpenDnsstatus;
                        } 
                    }

                }

            if (popUpBlocked)
            {
                
                string[] lines = File.ReadAllLines(hostfile_location);
                string matchLine = "#@popup and Ads blocked";
                foreach (string line in lines)
                {
                    if (Regex.IsMatch(line, matchLine))
                    {
                        PopUPstatus = (string)this.FindResource("PopUpBlocked");
                        PopUpAre.Text = PopUPstatus;
                        break;
                    }
                }
                
            }
            else
            {
                PopUPstatus = (string)this.FindResource("PopUpNotBlocked");
                PopUpAre.Text = PopUPstatus;
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
              //  string appStatus = (string)this.FindResource("ApplicaitonActive");
                await Dispatcher.BeginInvoke((Action)delegate { setNotifyIconText(PasswordRequire.notifyIcon, (string)this.FindResource("ApplicaitonActive") + "\nTime left\n" + hours.Text + "Hours" + " " + minutes.Text + "Min" + " " + sec.Text + "Sec"); }, null);
            }

            httpserver.Start(80);                                                                   //Starting Http server to send customized messages to the browser. 
            watcher.EnableRaisingEvents = true;
            //tcplistner = new TcpListener(localAddr, port);
            //Thread thread = new Thread(new ThreadStart(Listen));
            //thread.Start();

        }




        void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            File.SetAttributes(hostfile_location, FileAttributes.Hidden | FileAttributes.ReadOnly | FileAttributes.System);
            //MessageBox.Show("The file has been changed");
        }


        private void countdownTimer()
        { 
            timer.Tick += timer_Tick;
            timer.Interval = 996;
            timer.Start();
        }

        async void timer_Tick(object sender, EventArgs e)
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
                if (CommonFunctions.isHidden)
                {
                    App.Current.MainWindow.Show();
                    CommonFunctions.isHidden = false;
                    Window appwindow = (Window)App.Current.MainWindow;
                    var hwnd = new WindowInteropHelper(appwindow).Handle;
                    SetWindowLongPtr(hwnd, GWL_STYLE, StartPage.getwindow);
                }
                else if (!CommonFunctions.isHidden)
                {
                    Window appwindow = (Window)App.Current.MainWindow;
                    var hwnd = new WindowInteropHelper(appwindow).Handle;
                    SetWindowLongPtr(hwnd, GWL_STYLE, StartPage.getwindow);
                }
                timer.Stop();
              //  string appStatus = (string)this.FindResource("ApplicationInactive");
                await Dispatcher.BeginInvoke((Action)delegate { setNotifyIconText(PasswordRequire.notifyIcon, (string)FindResource("ApplicationInactive")); }, null);
                RestoreApplication();
            }
            else
            {
                string appStatus = (string)this.FindResource("ApplicaitonActive");
                await Dispatcher.BeginInvoke((Action)delegate { setNotifyIconText(PasswordRequire.notifyIcon,appStatus + "\nTime left\n " + hour + "Hours" + " " + min + "Min" + " " + seconds + "Sec"); }, null);
            }


        }

        private  async void RestoreApplication(object sender, RoutedEventArgs e)
        {

            try
            {
                IsolatedStorageFile password = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null);
                StreamReader passReader = new StreamReader(new IsolatedStorageFileStream("Win32AppPas.bin", FileMode.OpenOrCreate, password));
                FileAttributes passatt = File.GetAttributes(PasswordRequire.filePath + "Win32AppPas.bin");
                if (!passatt.ToString().Contains(FileAttributes.Hidden.ToString()) || !passatt.ToString().Contains(FileAttributes.System.ToString()))
                {
                    File.SetAttributes(PasswordRequire.filePath + "Win32AppPas.bin", FileAttributes.Hidden | FileAttributes.System);
                }
                if (passReader != null)
                {
                    string p = await passReader.ReadToEndAsync();
                    if (p == null || p=="")
                    {
                        timer.Stop();
                        watcher.EnableRaisingEvents = false;
                        Window appwindow = (Window)App.Current.MainWindow;
                        var hwnd = new WindowInteropHelper(appwindow).Handle;
                        SetWindowLongPtr(hwnd, GWL_STYLE, StartPage.getwindow);
                        int getwin = GetWindowLong(hwnd, GWL_STYLE);
                        SettingRestore(PasswordRequire.filePath, hostfile_location, openDnsused);
                        string systemRest = (string)this.FindResource("SettingRestoredMessage");
                        Xceed.Wpf.Toolkit.MessageBox.Show(systemRest);
                        if (getwin != 382337024)
                        {
                            SetWindowLongPtr(hwnd, GWL_STYLE, StartPage.getwindow);
                        }
                        httpserver.Dispose();                                                                           //Stopping Httpserver
                        setNotifyIconText(PasswordRequire.notifyIcon, (string)FindResource("ApplicationInactive"));
                        StartPage startObj = new StartPage();
                        this.NavigationService.Navigate(startObj);
                        ShowsNavigationUI = false;
                    }
                    else
                    {
                       // watcher.EnableRaisingEvents = false;                        // This has been moved to next page , and check for password. 
                        ApplicationFinished passObj = new ApplicationFinished(p, hostfile_location, openDnsused, watcher,timer);
                        NavigationService.Navigate(passObj);
                        ShowsNavigationUI = false;
                        
                    }
                }

                passReader.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("An exception has happened on final page , while restoring setting ."+ ex.StackTrace + ex.Message);
            }
            

        }

        private  void RestoreApplication()
        {

            try
            {
                        
                       // ((TextBlock)PasswordRequire.tooltip.Child).Text = (string)this.FindResource("ApplicationInactive");
                        httpserver.Dispose();                                                                               //Stopping HttpServer
                        watcher.EnableRaisingEvents = false;
                        SettingRestore(PasswordRequire.filePath, hostfile_location, openDnsused);
                        //MessageBox.Show("System Settings Has Been restored ");
                       // if (getwin != 382337024)
                      //  {
                      //      SetWindowLong(hwnd, GWL_STYLE, getwindow);
                      //  }
                        setNotifyIconText(PasswordRequire.notifyIcon, (string)FindResource("ApplicationInactive"));
                        string settingRestored = (string)this.FindResource("SettingRestored");
                        PasswordRequire.notifyIcon.BalloonTipTitle = "Social Silence";
                        PasswordRequire.notifyIcon.BalloonTipText = settingRestored;
                        PasswordRequire.notifyIcon.ShowBalloonTip(2000);
                        StartPage startObj = new StartPage();
                        this.NavigationService.Navigate(startObj);                
                        ShowsNavigationUI = false;
                       
                              
            }
            catch(Exception ex)
            {
                MessageBox.Show("An exception occured on Final page while restoring the setting ."+ ex.Message+ex.StackTrace); 
            }


        }

 
        
    }
}
