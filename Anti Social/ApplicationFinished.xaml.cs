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
        int window;
        private const int GWL_STYLE = -16;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        FileSystemWatcher hostWatcher;
        System.Windows.Forms.Timer runningTimer;
        //Window.f Timer runningTimer;

        public ApplicationFinished()
        {
            InitializeComponent();
            proceed.IsEnabled = false;
        }

        public ApplicationFinished(string pass, string desPath, string hostPath, bool resDNS, int getwindow, FileSystemWatcher watchfile, System.Windows.Forms.Timer timer)
            : this()
        {
            password = pass;
            this.filePath = desPath;
            hostfile_location = hostPath;
            restoreDNS = resDNS;
            Loaded += ApplicationFinished_Loaded;
            window = getwindow;
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
            if (userpassword.Password == password)
            {
                
               // NavigationWindow win = (NavigationWindow)Window.GetWindow(this);
                hostWatcher.EnableRaisingEvents = false;
                runningTimer.Stop();
                Window appwindow = (Window)App.Current.MainWindow;
                var hwnd = new WindowInteropHelper(appwindow).Handle;
                SetWindowLong(hwnd, GWL_STYLE, window);
                int getwin = GetWindowLong(hwnd, GWL_STYLE);
               

                SettingRestore(filePath, hostfile_location, restoreDNS);
                Xceed.Wpf.Toolkit.MessageBox.Show("System Settings Has Been restored ");
                if (getwin != 382337024)
                {
                    SetWindowLong(hwnd, GWL_STYLE, window);
                }
                StartPage startObj = new StartPage(filePath);
                this.NavigationService.Navigate(startObj);
                ShowsNavigationUI = false;
            }
            else
            {
                string passWrong = (string)this.FindResource("PasswordWrongMessage");
                Xceed.Wpf.Toolkit.MessageBox.Show(passWrong);
                userpassword.Clear();

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
