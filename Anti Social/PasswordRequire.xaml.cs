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

namespace SocialSilence
{
    /// <summary>
    /// Interaction logic for PasswordRequire.xaml
    /// </summary>
    public partial class PasswordRequire : CommonFunctions
    {
        
        string passwordStored;
        string userLanguage;
        public  static System.Windows.Forms.NotifyIcon notifyIcon = null;
        private Dictionary<string, System.Drawing.Icon> IconHandles = null;

        public PasswordRequire()
        {
            InitializeComponent();
            //NavigationWindow win = (NavigationWindow)Window.GetWindow(this);
            //win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ShowsNavigationUI = false;
            Proceed.IsEnabled = false;
            //this.PreviewKeyDown += new KeyEventHandler(KeyPress);
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

            
            var OSname = (from x in new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem").Get().OfType<ManagementObject>()
                         select x.GetPropertyValue("Caption")).FirstOrDefault();

            if (OSname.ToString().Contains("Windows 8"))
            {
                string[] languageSet = (string[])Registry.GetValue(@"HKEY_CURRENT_USER\Control Panel\International\User Profile", "Languages", null);
                userLanguage = languageSet[0].Remove(2) ;
            }
            else if (OSname.ToString().Contains("Windows 7"))
            {
                string[] languageSet = (string[])Registry.GetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop\MuiCached", "MachinePreferredUILanguages", null);
                userLanguage = languageSet[0].Remove(2);
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
                notifyIcon.BalloonTipTitle = "Windows Social Silence";
                notifyIcon.BalloonTipText = "Click on this icon to make changes";
                //notifyIcon.ContextMenuStrip.MouseLeave += ContextMenuStrip_MouseLeave;
                notifyIcon.Visible = true;

                Rect workArea = System.Windows.SystemParameters.WorkArea;
                App.Current.MainWindow.Left = workArea.Left + (workArea.Width - this.WindowWidth) / 2;
                App.Current.MainWindow.Top = workArea.Top + (workArea.Height - this.WindowHeight) / 2;
                ((NavigationWindow)LogicalTreeHelper.GetParent(this)).ResizeMode = ResizeMode.CanMinimize;

            }
            catch (Exception ex)
            {
                MessageBox.Show("An exception created while creating the taskbar icon " + ex.Message + ex.StackTrace);
            }

            try
            {
                IsolatedStorageFile password = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null);
                filePath = password.GetType().GetField("m_RootDir", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(password).ToString();
                StreamReader passReader = new StreamReader(new IsolatedStorageFileStream("Win32AppPas.bin", FileMode.OpenOrCreate, password));                
               // File.SetAttributes(filePath + "Win32AppPas.bin", FileAttributes.Hidden|FileAttributes.System);
                File.SetAttributes(filePath + "Win32AppPas.bin", FileAttributes.Normal);
                if (passReader != null)
                {
                    string p = await passReader.ReadLineAsync();
                    if (p == null || p=="")                                  // If there is no password in the file then the application is going to start from Start Page. 
                    {
                        StartPage pobj = new StartPage(filePath);
                        this.NavigationService.Navigate(pobj);
                    }
                    else
                    {
                        passwordStored = p;
                    }
                }
                passReader.Close();
                
            }
            catch(Exception ex)
            {
                MessageBox.Show("An exception occured while opening the password from the file . " + ex.Message + ex.StackTrace);
            }


        }

        //void ContextMenuStrip_MouseLeave(object sender, EventArgs e)
        //{
        //    SocialSilence.SysTrayMenu systray = new SysTrayMenu();
        //    systray.IsOpen = false;
        //}

        private void notifyIcon_Click(object sender, EventArgs e)
        {
            SocialSilence.SysTrayMenu systray = new SysTrayMenu();
            systray.IsOpen = true;

        }

        void item2_Click(object sender, RoutedEventArgs e)
        {
            SetPassword passObj = new SetPassword();
            passObj.Show();
        }



        public void CheckPassword(object sender, RoutedEventArgs e)
        {
            string passWrong = (string)this.FindResource("PasswordWrongMessage");
            string userPassword = Password.Password;
            if (userPassword == passwordStored && userPassword != null && userPassword != "")
            {
                StartPage pobj = new StartPage(filePath);
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
            if (userPassword == passwordStored && userPassword != null && userPassword != "")
            {
                StartPage pobj = new StartPage(filePath);
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
