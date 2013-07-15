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
using System.IO.IsolatedStorage;
using System.IO;
using System.Reflection;
using Microsoft.Win32;
using System.Management;
using System.Windows.Threading;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace SocialSilence
{
    /// <summary>
    /// Interaction logic for PasswordRequire.xaml
    /// </summary>
    public partial class PasswordRequire : CommonFunctions
    {

        public static string filePath;
        string passwordStored;
        string userLanguage;
        public  static System.Windows.Forms.NotifyIcon notifyIcon = null;
        private Dictionary<string, System.Drawing.Icon> IconHandles = null;
        public static bool passUsed = false;
        public static bool firstRun = false;
        System.Security.Cryptography.MD5CryptoServiceProvider hashConverter = new System.Security.Cryptography.MD5CryptoServiceProvider(); public static string notificationToolTip;

        public PasswordRequire()
        {
            InitializeComponent();
            ShowsNavigationUI = false;
            Proceed.IsEnabled = false;
            Loaded += PasswordRequire_Loaded;         
            
        }

        private void KeyPress(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CheckPassword();
                e.Handled = true;
            }
            if (e.Key == Key.Escape)
            {
                e.Handled = true;
                NavigationWindow win = (NavigationWindow)Window.GetWindow(this);
                win.Close();
                
            }

        }

        async void PasswordRequire_Loaded(object sender, RoutedEventArgs e)
        {
            Process[] currentProcess = Process.GetProcesses();
            List<string> process = new List<string>();
            MessageBoxResult result = MessageBoxResult.None;
            foreach (Process pro in currentProcess)
            {
                if(pro.ProcessName=="SocialSilence")
                process.Add(pro.ProcessName);
            }
            if (process.Count() != 1 &&  process.Count() != 0  )  // Checking for Instances of current application running . 
            {                
                result = Xceed.Wpf.Toolkit.MessageBox.Show("There are current Instances of same Application running on this computer.", "Message", MessageBoxButton.OK);

                if (result == MessageBoxResult.OK)
                {
                    App.Current.MainWindow.Close();
                }

            }

            

            Rect workArea = System.Windows.SystemParameters.WorkArea;
            App.Current.MainWindow.Left = workArea.Left + (workArea.Width - this.WindowWidth) / 2;
            App.Current.MainWindow.Top = workArea.Top + (workArea.Height - this.WindowHeight) / 2;
            ((NavigationWindow)LogicalTreeHelper.GetParent(this)).ResizeMode = ResizeMode.CanMinimize;
            NavigationWindow win = (NavigationWindow)Window.GetWindow(this);
            win.Closing += win_Closing;
            
            var OSname = (from x in new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem").Get().OfType<ManagementObject>()
                         select x.GetPropertyValue("Caption")).FirstOrDefault();

            if (OSname.ToString().Contains("Windows 8"))
            {
                string[] languageSet = (string[])Registry.GetValue(@"HKEY_CURRENT_USER\Control Panel\International\User Profile", "Languages", null);
                if (languageSet[0].Length > 2)
                {
                    userLanguage = languageSet[0].Remove(2);
                }
                else
                {
                    userLanguage = languageSet[0];
                }
            }
            else if (OSname.ToString().Contains("Windows 7"))
            {
                string[] languageSet = (string[])Registry.GetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop\MuiCached", "MachinePreferredUILanguages", null);
                if (languageSet[0].Length > 2)
                {
                    userLanguage = languageSet[0].Remove(2);
                }
                else
                {
                    userLanguage = languageSet[0];
                }
            }


            SetLanguageDictionary(userLanguage);
            this.KeyDown += new KeyEventHandler(KeyPress);

            try
            {
              
                IconHandles = new Dictionary<string, System.Drawing.Icon>();
                IconHandles.Add("QuickLaunch", new System.Drawing.Icon(Environment.CurrentDirectory + @"\resources\error.ico"));
                notifyIcon = new System.Windows.Forms.NotifyIcon();
                notifyIcon.Click += notifyIcon_Click;
                notifyIcon.Icon = IconHandles["QuickLaunch"];
                notifyIcon.BalloonTipTitle = "Social Silence";
                notifyIcon.BalloonTipText = (string)FindResource("ClickIcon"); ;
                notifyIcon.Visible = true;
                notifyIcon.ShowBalloonTip(500);                
                notificationToolTip = (string)FindResource("ApplicationInactive");
                setNotifyIconText(notifyIcon, notificationToolTip);
               // System.Windows.Forms.ContextMenu menu = notifyIcon.ContextMenu;
               // System.Windows.Forms.MenuItem item3 = menu.MenuItems[3];
                //item3.Visible = false;

                

            }
            catch (Exception ex)
            {
                MessageBox.Show("An exception created while creating the taskbar icon " + ex.Message + ex.StackTrace);
            }

            try
            {
                IsolatedStorageFile password = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null);
                filePath = password.GetType().GetField("m_RootDir", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(password).ToString();
                if(!File.Exists(filePath +"host"))
                {
                    firstRun = true;
                }
                using (StreamReader passReader = new StreamReader(new IsolatedStorageFileStream("Win32AppPas.bin", FileMode.OpenOrCreate, password)))
                {
                    // File.SetAttributes(filePath + "Win32AppPas.bin", FileAttributes.Hidden|FileAttributes.System);
                    File.SetAttributes(filePath + "Win32AppPas.bin", FileAttributes.Normal);
                    if (passReader != null)
                    {
                        string p = await passReader.ReadToEndAsync();
                        if (p == null || p == "")                                  // If there is no password in the file then the application is going to start from Start Page. 
                        {
                            notifyIcon.Click -= notifyIcon_Click;
                            StartPage pobj = new StartPage();
                            this.NavigationService.Navigate(pobj);
                        }
                        else
                        {
                            passwordStored = p;
                        }
                    }
                }
                File.SetAttributes(filePath + "Win32AppPas.bin", FileAttributes.Hidden | FileAttributes.System);
                
            }
            catch(Exception ex)
            {
                MessageBox.Show("An exception occured while opening the password from the file . " + ex.Message + ex.StackTrace);
            }


        }      


        void win_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (((System.Windows.Controls.Page)(((System.Windows.Controls.ContentControl)(sender)).Content)).Title != "FinalPage" && ((System.Windows.Controls.Page)(((System.Windows.Controls.ContentControl)(sender)).Content)).Title != "ApplicationFinished")
            {
                notifyIcon.Dispose();
            }
            else
            {
                e.Cancel = true;
            }
            
        }


        private void notifyIcon_Click(object sender, EventArgs e)
        {
            SocialSilence.SysTrayMenu systray = new SysTrayMenu();
            systray.IsOpen = true;
            if (passUsed)
            {
                if (!((System.Windows.Controls.HeaderedItemsControl)(((System.Windows.Controls.ItemsControl)(systray)).Items[6])).Equals(null))
                {
                    ((System.Windows.Controls.HeaderedItemsControl)(((System.Windows.Controls.ItemsControl)(systray)).Items[6])).Visibility = System.Windows.Visibility.Collapsed;
                }
            }
           var  _popupTimer = new DispatcherTimer(DispatcherPriority.Normal);
           _popupTimer.Interval = TimeSpan.FromMilliseconds(5000);
           _popupTimer.Tick += (obj, x) =>
           {
               systray.IsOpen = false;
           };
           _popupTimer.Start();


            
        }



        public void CheckPassword(object sender, RoutedEventArgs e)
        {
            string passWrong = (string)this.FindResource("PasswordWrongMessage");
            string userPassword = Password.Password;

            byte[] data = System.Text.Encoding.ASCII.GetBytes(userPassword);

            data = hashConverter.ComputeHash(data);

            userPassword = System.Text.Encoding.ASCII.GetString(data);

            userPassword = Regex.Replace(userPassword, @"\t|\n|\r", "");

            passwordStored = Regex.Replace(passwordStored, @"\t|\n|\r", "");


            if (userPassword == passwordStored && userPassword != null && userPassword != "")
            {
                passUsed = true;
                notifyIcon.Click -= notifyIcon_Click;
                StartPage pobj = new StartPage();
                this.NavigationService.Navigate(pobj);
              //  ShowsNavigationUI = true;
            }
            else 
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(passWrong);
                Password.Clear();
            }

        }
        public void CheckPassword()
        {
            string passWrong = (string)this.FindResource("PasswordWrongMessage");
            string userPassword = Password.Password;

            byte[] data = System.Text.Encoding.ASCII.GetBytes(userPassword);

            data = hashConverter.ComputeHash(data);

            userPassword = System.Text.Encoding.ASCII.GetString(data);

            userPassword = Regex.Replace(userPassword, @"\t|\n|\r", "");

            passwordStored = Regex.Replace(passwordStored, @"\t|\n|\r", "");

            if (userPassword == passwordStored && userPassword != null && userPassword != "")
            {
                passUsed = true;
                notifyIcon.Click -= notifyIcon_Click;
                StartPage pobj = new StartPage();
                this.NavigationService.Navigate(pobj);
              //  ShowsNavigationUI = true;
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(passWrong);
                Password.Clear();
            }
        }

        //private void EnableProceed(object sender, ManipulationStartedEventArgs e)
        //{
        //}

        private void EnableProceed(object sender, TextCompositionEventArgs e)
        {
            Proceed.IsEnabled = true;
        }

        //private void btnClose_Click(object sender, RoutedEventArgs e):base.btnClose_Click()
        //{

        //}



        
    }
}
