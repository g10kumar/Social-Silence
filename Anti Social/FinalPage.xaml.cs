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
        private int port = 8080;
        IPAddress localAddr = IPAddress.Parse("127.0.0.120");
        TcpListener tcplistner;
        Stopwatch watch = new Stopwatch();
        double timeUsed = 0;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        FileSystemWatcher watcher = new FileSystemWatcher(System.IO.Path.GetDirectoryName(hostfile_location));
        int getwindow ;




        public FinalPage()
        {
            
            InitializeComponent();
        }

        public FinalPage(bool domcus, bool openDns , bool popup, int time, string path ):this()
        {
            domainCustomized = domcus;
            openDnsused = openDns;
            popUpBlocked = popup;
            filePath = path;
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
        void  FinalPage_Loaded(object sender, RoutedEventArgs e)
        {
     
            //((NavigationWindow)LogicalTreeHelper.GetParent(this)).PreviewKeyDown += new KeyEventHandler(KeyPress);
            NavigationWindow win = (NavigationWindow)Window.GetWindow(this);
            var hwnd = new WindowInteropHelper(win).Handle;
            getwindow = GetWindowLong(hwnd, GWL_STYLE);       
            win.KeyDown += FinalPage_PreviewKeyDown;
            File.SetAttributes(hostfile_location, FileAttributes.Hidden | FileAttributes.ReadOnly | FileAttributes.System); // Here we are setting the host file attributes.
            watcher.Filter = "hosts";
            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Attributes | NotifyFilters.Size;
            watcher.Changed += watcher_Changed;
            win.Closing += win_Closing;                                                                     // This event will fire when the appliciaton is closed .  
           
            SetWindowLong(hwnd, GWL_STYLE, getwindow & ~WS_SYSMENU);


            if (domainCustomized)
            {
                domainsAre.Text = "Customized By User.";
            }
            else
            {            
                domainsAre.Text = "Default set of Doamins Used.";
            }            
                ManagementScope oMs = new ManagementScope("\\\\localhost\\root\\cimv2");

                string strQuery = "select DNSServerSearchOrder from Win32_NetworkAdapterConfiguration where IPEnabled='true'";

                ObjectQuery oQ = new ObjectQuery(strQuery);

                ManagementObjectSearcher oS = new ManagementObjectSearcher(oMs, oQ);

                ManagementObjectCollection oRc = oS.Get();

                foreach (ManagementObject oR in oRc)
                {

                    foreach (string str in (Array)(oR.Properties["DNSServerSearchOrder"].Value))
                    {
                        if (str == "208.67.222.222" && openDnsused)
                        {
                            openDnsIs.Text = "OpenDNS is configured as the DNS of this system.";
                            break;
                        }
                        else if (str != "208.67.222.222" && !openDnsused)
                        {

                            openDnsIs.Text = "OpenDNS is not configured on this system.";
                            break;
                            
                        }
                        else if (str == "208.67.222.222")
                        {

                            openDnsIs.Text = "OpenDNS was already configured as the DNS of this system.";
                            break;

                        }
                        else
                        {
                            openDnsIs.Text = "Application was unbale to configure OpenDNS on this system."; 
                            break;
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

            watcher.EnableRaisingEvents = true;
            //tcplistner = new TcpListener(localAddr, port);
            //Thread thread = new Thread(new ThreadStart(Listen));
            //thread.Start();

        }

        private async void Listen()
        {
            tcplistner.Start();
            while (true)
            {
                
                TcpClient client = tcplistner.AcceptTcpClient();
                Thread listenThread = new Thread(new ParameterizedThreadStart(ListenThread));
                listenThread.Start(client);

            }
        }

        private async  void ListenThread(object obj)
        {
            NetworkStream ns = ((TcpClient)obj).GetStream();
            string text = "Blocked using Daksatech Social Silence";
            string header = "HTTP/1.1 200 Ok" + Environment.NewLine + Environment.NewLine + text;
            byte[] headerString = Encoding.ASCII.GetBytes(header);
            await ns.WriteAsync(headerString, 0, headerString.Length);
            ns.Close();
        }


        void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            File.SetAttributes(hostfile_location, FileAttributes.Hidden | FileAttributes.ReadOnly | FileAttributes.System);
            //MessageBox.Show("The file has been changed");
        }

        void win_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ////MessageBox.Show("Are you sure you want to close the applicaiton . ");
            if (((System.Windows.Controls.Page)(((System.Windows.Controls.ContentControl)(sender)).Content)).Title == "ApplicationFinished" || ((System.Windows.Controls.Page)(((System.Windows.Controls.ContentControl)(sender)).Content)).Title == "FinalPage")
            {
                e.Cancel = true;
            }
        }

        private void countdownTimer()
        { 
            timer.Tick += timer_Tick;
            timer.Interval = 998;
            timer.Start();
        }

        async void timer_Tick(object sender, EventArgs e)
        {
            watch.Start();
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
                watch.Stop();
                timer.Stop();
                RestoreApplication();
            }
            else
            {
            }


            watch.Stop();
            timeUsed =  timeUsed + watch.Elapsed.TotalSeconds;
        }

        private  async void RestoreApplication(object sender, RoutedEventArgs e)
        {

            try
            {
                IsolatedStorageFile password = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null);
                StreamReader passReader = new StreamReader(new IsolatedStorageFileStream("Win32AppPas.bin", FileMode.OpenOrCreate, password));
                FileAttributes passatt =  File.GetAttributes(filePath + "Win32AppPas.bin");
                if (!passatt.ToString().Contains(FileAttributes.Hidden.ToString()) || !passatt.ToString().Contains(FileAttributes.System.ToString()))
                {
                    File.SetAttributes(filePath + "Win32AppPas.bin", FileAttributes.Hidden | FileAttributes.System);
                }
                if (passReader != null)
                {
                    string p = await passReader.ReadLineAsync();
                    if (p == null || p=="")
                    {
                        timer.Stop();
                        watcher.EnableRaisingEvents = false;
                        Window appwindow = (Window)App.Current.MainWindow;
                        var hwnd = new WindowInteropHelper(appwindow).Handle;
                        SetWindowLong(hwnd, GWL_STYLE, getwindow );
                        int getwin = GetWindowLong(hwnd, GWL_STYLE);
                        SettingRestore(filePath, hostfile_location, openDnsused);
                        Xceed.Wpf.Toolkit.MessageBox.Show("System Settings Has Been restored ");
                        if (getwin != 382337024)
                        {
                            SetWindowLong(hwnd, GWL_STYLE, getwindow);
                        }

                        PasswordRequire passObj = new PasswordRequire();
                        this.NavigationService.Navigate(passObj);
                        ShowsNavigationUI = false;
                    }
                    else
                    {
                       // watcher.EnableRaisingEvents = false;                        // This has been moved to next page , and check for password. 
                        ApplicationFinished passObj = new ApplicationFinished(p, filePath, hostfile_location, openDnsused,getwindow,watcher,timer);
                        NavigationService.Navigate(passObj);
                        ShowsNavigationUI = false;
                        
                    }
                }

                passReader.Close();
            }
            catch
            {
                MessageBox.Show("An exception has happened on final page , while restoring setting .");
            }
            

        }

        private  void RestoreApplication()
        {

            try
            {   
                        watcher.EnableRaisingEvents = false;
                        //NavigationWindow win = (NavigationWindow)Window.GetWindow(this);
                       // var hwnd = new WindowInteropHelper(win).Handle;
                       // SetWindowLong(hwnd, GWL_STYLE, getwindow);
                       // int getwin = GetWindowLong(hwnd, GWL_STYLE);
                        SettingRestore(filePath, hostfile_location, openDnsused);
                        //MessageBox.Show("System Settings Has Been restored ");
                       // if (getwin != 382337024)
                      //  {
                      //      SetWindowLong(hwnd, GWL_STYLE, getwindow);
                      //  }
                        StartPage startObj = new StartPage(filePath);
                        this.NavigationService.Navigate(startObj);
                
                        ShowsNavigationUI = false;
                       
                              
            }
            catch
            {
                MessageBox.Show("An exception occured on Final page while restoring the setting ."); 
            }


        }

        void item0_Click(object sender, RoutedEventArgs e)
        {
            //NavigationWindow win = (NavigationWindow)Window.GetWindow(this);
            //win.Show();
            App.Current.MainWindow.Show();
           
        }
        
    }
}
