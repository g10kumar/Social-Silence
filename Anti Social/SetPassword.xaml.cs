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
using System.Windows.Shapes;
using System.IO.IsolatedStorage;
using System.IO;

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for SetPassword.xaml
    /// </summary>
    public partial class SetPassword : Window
    {
        
        public SetPassword()
        {
            InitializeComponent();

            this.PreviewKeyDown += new KeyEventHandler(CloseOnEscape);

            this.Loaded += SetPassword_Loaded;

            Password.IsEnabled = false;
            ConPassword.IsEnabled = false;
            finish.IsEnabled = false;
        }

        void SetPassword_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void CloseOnEscape(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

        private void ActivatePassword(object sender, RoutedEventArgs e)
        {
            
            Password.IsEnabled = true;
            ConPassword.IsEnabled = true;
            finish.IsEnabled = true;

        }

        private void DeActivatePassword(object sender, RoutedEventArgs e)
        {
            Password.IsEnabled = false;
            ConPassword.IsEnabled = false;
            finish.IsEnabled = false;

        }

        private async void StorePassword(object sender, RoutedEventArgs e)
        {
            string pass = Password.Password;
            string conpass = ConPassword.Password;
            if (pass == conpass)
            {
                try
                {
                    IsolatedStorageFile password = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null);
                    var passWriter = new StreamWriter(new IsolatedStorageFileStream("password.bin", FileMode.Create, password));
                    await passWriter.WriteLineAsync(pass);
                    passWriter.Close();
                    
                    //this.Close();
                    StreamReader passReader = new StreamReader(new IsolatedStorageFileStream("Win32AppPas.bin",FileMode.Open,password));
                    if(passReader == null)
                    {
                        MessageBox.Show("Password Not Created");
                    }
                    else
                    {
                        string p = await passReader.ReadLineAsync();
                        MessageBox.Show("Password created successfully . Password set by you is " + p);
                    }
                    passReader.Close();

                    this.Close();
                    

                }
                catch 
                {
                  

                }
            }
            else 
            {
                MessageBox.Show("Password Provided by You Don't match . Please check");
            }
            

        }
    }
}
