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
using Hardcodet.Wpf.TaskbarNotification;

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for PasswordRequire.xaml
    /// </summary>
    public partial class PasswordRequire : CommonFunctions
    {
        
        string passwordStored;

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
            this.KeyDown += new KeyEventHandler(KeyPress);
            tb = (TaskbarIcon)FindResource("MyNotifyIcon");
            tb.ShowBalloonTip("Windows Anti Social", "Click on this Icon to restore Anti Social", BalloonIcon.None);
            ContextMenu menu = tb.ContextMenu;
            MenuItem item2 = (MenuItem)menu.Items[2];
            item2.Click += item2_Click;
            Rect workArea = System.Windows.SystemParameters.WorkArea;
            App.Current.MainWindow.Left = workArea.Left + (workArea.Width - this.WindowWidth) / 2;
            App.Current.MainWindow.Top = workArea.Top + (workArea.Height - this.WindowHeight) / 2 ;
            ((NavigationWindow)LogicalTreeHelper.GetParent(this)).ResizeMode = ResizeMode.CanMinimize;            

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
            catch
            {
                //System.IO.FileNotFoundException ;
            }


        }

        void item2_Click(object sender, RoutedEventArgs e)
        {
            SetPassword passObj = new SetPassword();
            passObj.Show();
        }



        public void CheckPassword(object sender, RoutedEventArgs e)
        {
            string userPassword = Password.Password;
            if (userPassword == passwordStored && userPassword != null && userPassword != "")
            {
                StartPage pobj = new StartPage(filePath);
                this.NavigationService.Navigate(pobj);
              //  ShowsNavigationUI = true;
            }
            else 
            {
                MessageBox.Show("Password Provided By You is Not Correct . Please Provide Again");
                Password.Clear();
            }

        }
        public void CheckPassword()
        {
            string userPassword = Password.Password;
            if (userPassword == passwordStored && userPassword != null && userPassword != "")
            {
                StartPage pobj = new StartPage(filePath);
                this.NavigationService.Navigate(pobj);
              //  ShowsNavigationUI = true;
            }
            else
            {
                MessageBox.Show("Password Provided By You is Not Correct . Please Provide Again");
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
