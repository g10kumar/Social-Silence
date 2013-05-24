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
            ShowsNavigationUI = false;
            Loaded += PasswordRequire_Loaded;
            Proceed.IsEnabled = false;
            this.PreviewKeyDown += new KeyEventHandler(KeyPress);

        }

        private void KeyPress(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CheckPassword();
            }
            if (e.Key == Key.Escape)
            {
                NavigationWindow win = (NavigationWindow)Window.GetWindow(this);
                win.Close();
            }

        }

        async void PasswordRequire_Loaded(object sender, RoutedEventArgs e)
        {

            ((NavigationWindow)LogicalTreeHelper.GetParent(this)).ResizeMode = ResizeMode.CanMinimize;

            try
            {
                IsolatedStorageFile password = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null);
                StreamReader passReader = new StreamReader(new IsolatedStorageFileStream("Win32AppPas.bin", FileMode.OpenOrCreate, password));
                filePath = password.GetType().GetField("m_RootDir", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(password).ToString();
                File.SetAttributes(filePath + "Win32AppPas.bin", FileAttributes.Hidden|FileAttributes.System);
                if (passReader != null)
                {
                    string p = await passReader.ReadLineAsync();
                    if (p == null)                                  // If there is no password in the file then the application is going to start from Start Page. 
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

        public void CheckPassword(object sender, RoutedEventArgs e)
        {
            string userPassword = Password.Password;
            if (userPassword == passwordStored)
            {
                StartPage pobj = new StartPage(filePath);
                this.NavigationService.Navigate(pobj);
                ShowsNavigationUI = true;
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
            if (userPassword == passwordStored)
            {
                StartPage pobj = new StartPage(filePath);
                this.NavigationService.Navigate(pobj);
                ShowsNavigationUI = true;
            }
            else
            {
                MessageBox.Show("Password Provided By You is Not Correct . Please Provide Again");
                Password.Clear();
            }
        }

        private void EnableProceed(object sender, ManipulationStartedEventArgs e)
        {
        }

        private void EnableProceed(object sender, TextCompositionEventArgs e)
        {
            Proceed.IsEnabled = true;
        }

        //private void btnClose_Click(object sender, RoutedEventArgs e):base.btnClose_Click()
        //{

        //}

        
    }
}
