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
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Threading;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;


namespace SocialSilence
{
    /// <summary>
    /// Interaction logic for ApplicationFinished.xaml
    /// </summary>
    public partial class ApplicationFinished : CommonFunctions
    {
        string password;
        string hostfile_location;
        bool restoreDNS;
        private const int GWL_STYLE = -16;
        System.Security.Cryptography.MD5CryptoServiceProvider hashConverter = new System.Security.Cryptography.MD5CryptoServiceProvider();
        FileSystemWatcher hostWatcher;
        System.Windows.Forms.Timer runningTimer;
        //Window.f Timer runningTimer;

        public static int GetWindowLong(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 4)
            {
                return GetWindowLong32(hWnd, nIndex);
            }
            return GetWindowLongPtr64(hWnd, nIndex);
        }


        [DllImport("user32.dll", EntryPoint = "GetWindowLong", CharSet = CharSet.Auto)]
        public static extern int GetWindowLong32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr", CharSet = CharSet.Auto)]
        public static extern int GetWindowLongPtr64(IntPtr hWnd, int nIndex);


        public static int SetWindowLongPtr(IntPtr hWnd, int nIndex, int dwNewLong)
        {
            if (IntPtr.Size == 8)
                return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
            else
                return SetWindowLong32(hWnd, nIndex, dwNewLong);
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        public static extern int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        public static extern int SetWindowLongPtr64(IntPtr hWnd, int nIndex, int dwNewLong);

        public ApplicationFinished()
        {
            InitializeComponent();
            proceed.IsEnabled = false;
        }

        public ApplicationFinished(string pass, string hostPath, bool resDNS, FileSystemWatcher watchfile, System.Windows.Forms.Timer timer)
            : this()
        {
            password = pass;
            hostfile_location = hostPath;
            restoreDNS = resDNS;
            Loaded += ApplicationFinished_Loaded;
            hostWatcher = watchfile;
            runningTimer = timer;
        }

        void ApplicationFinished_Loaded(object sender, RoutedEventArgs e)
        {
            //(NavigationWindow)this.PreviewKeyDown += new KeyEventHandler(KeyPress);
        }

        //private void KeyPress(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Escape)
        //    {

        //        returnToFinalPage(sender, e);

        //    }

        //}

  

        private void CheckPassword(object sender, RoutedEventArgs e)
        {
            try
            {

                string userPassword = userpassword.Password;

                byte[] data = System.Text.Encoding.ASCII.GetBytes(userPassword);

                data = hashConverter.ComputeHash(data);

                userPassword = System.Text.Encoding.ASCII.GetString(data);

                userPassword = Regex.Replace(userPassword, @"\t|\n|\r", "");

                password = Regex.Replace(password, @"\t|\n|\r", "");


                if (userPassword == password)
                {
                    // NavigationWindow win = (NavigationWindow)Window.GetWindow(this);
                    hostWatcher.EnableRaisingEvents = false;
                    runningTimer.Stop();
                    Window appwindow = (Window)App.Current.MainWindow;
                    var hwnd = new WindowInteropHelper(appwindow).Handle;
                    SetWindowLongPtr(hwnd, GWL_STYLE, StartPage.getwindow);
                    appwindow.UpdateLayout();
                    int getwin = GetWindowLong(hwnd, GWL_STYLE);

                    string systemRest = (string)this.FindResource("SettingRestoredMessage");
                    SettingRestore(PasswordRequire.filePath, hostfile_location, restoreDNS);
                    Xceed.Wpf.Toolkit.MessageBox.Show(systemRest);
                    if (getwin != 382337024)
                    {
                        SetWindowLongPtr(hwnd, GWL_STYLE, StartPage.getwindow);
                    }
                    AdittionalOptions.httpserver.Dispose();                                                                             // Stopping HttpServer
                    StartPage startObj = new StartPage();
                    this.NavigationService.Navigate(startObj);
                    ShowsNavigationUI = false;
                    // string appStatus = (string)this.FindResource("ApplicationInactive");
                    setNotifyIconText(PasswordRequire.notifyIcon, (string)FindResource("ApplicationInactive"));
                }
                else
                {
                    //string passWrong = (string)this.FindResource("PasswordWrongMessage");
                    Xceed.Wpf.Toolkit.MessageBox.Show((string)this.FindResource("PasswordWrongMessage"));
                    userpassword.Clear();

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void enableProceed(object sender, TextCompositionEventArgs e)
        {
            proceed.IsEnabled = true;
        }

        private void returnToFinalPage(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();                                                                 // When going back like this way constructor is not loaded,
                                                                                                        // but onloaded function is executed. 


        }


       
    }
}
