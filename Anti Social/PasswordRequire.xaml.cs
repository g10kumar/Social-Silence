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

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for PasswordRequire.xaml
    /// </summary>
    public partial class PasswordRequire : Page
    {
        string passwordStored;
        public PasswordRequire()
        {
            InitializeComponent();
            ShowsNavigationUI = false;
            Loaded += PasswordRequire_Loaded;
            Proceed.IsEnabled = false;
            
        }

        async void PasswordRequire_Loaded(object sender, RoutedEventArgs e)
        {

            ((NavigationWindow)LogicalTreeHelper.GetParent(this)).ResizeMode = ResizeMode.CanMinimize;
            try
            {
                IsolatedStorageFile password = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null);
                StreamReader passReader = new StreamReader(new IsolatedStorageFileStream("Win32AppPas.bin", FileMode.OpenOrCreate, password));
                if (passReader != null)
                {
                    string p = await passReader.ReadLineAsync();
                    if (p == null)
                    {
                        StartPage pobj = new StartPage();
                        this.NavigationService.Navigate(pobj);
                    }
                    else
                    {
                        passwordStored = p;
                    }
                }
            }
            catch
            {
                //System.IO.FileNotFoundException ;
            }

        }

        private void CheckPassword(object sender, RoutedEventArgs e)
        {
            string userPassword = Password.Password;
            if (userPassword == passwordStored)
            {
                StartPage pobj = new StartPage();
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
    }
}
