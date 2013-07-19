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
using System.Reflection;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace SocialSilence
{
    /// <summary>
    /// Interaction logic for RemovePassword.xaml
    /// </summary>
    public partial class RemovePassword : Window
    {
        string filePath;
        char[] charsToTrim = { ' ', '\t' };
        System.Security.Cryptography.MD5CryptoServiceProvider hashConverter = new System.Security.Cryptography.MD5CryptoServiceProvider();
        public RemovePassword()
        {
            InitializeComponent();
            this.KeyDown += RemovePassword_KeyDown;
        }

        void RemovePassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (Previous_Pass.Password != null && e.Key == Key.Enter)
            {
                PasswordRemove();
            }
        }

        private async void PasswordRemove()
        {
            string passwordPresent = null;
            IsolatedStorageFile password = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null);
            using (StreamReader passReader = new StreamReader(new IsolatedStorageFileStream("Win32AppPas.bin", FileMode.OpenOrCreate, password)))
            {
                filePath = password.GetType().GetField("m_RootDir", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(password).ToString();
                FileAttributes fileAttrib = File.GetAttributes(filePath + "Win32AppPas.bin");
                if (fileAttrib.ToString().Contains(FileAttributes.Hidden.ToString()) || fileAttrib.ToString().Contains(FileAttributes.System.ToString()))
                {
                    File.SetAttributes(filePath + "Win32AppPas.bin", FileAttributes.Normal);
                }
                if (passReader != null)
                {
                    string p = await passReader.ReadToEndAsync();
                    passwordPresent = p;
                }
            }
                string userPassword = Previous_Pass.Password;

                byte[] data = System.Text.Encoding.ASCII.GetBytes(userPassword);

                data = hashConverter.ComputeHash(data);

                userPassword = System.Text.Encoding.ASCII.GetString(data);

                userPassword = Regex.Replace(userPassword, @"\t|\n|\r", "");

                passwordPresent = Regex.Replace(passwordPresent, @"\t|\n|\r", "");

                if (passwordPresent == userPassword)
                {
                    passwordPresent = passwordPresent.Remove(0);
                    File.WriteAllText(filePath + "Win32AppPas.bin", passwordPresent);
                    using (StreamReader passChecker = new StreamReader(new IsolatedStorageFileStream("Win32AppPas.bin", FileMode.OpenOrCreate, password)))
                    {
                        string isEmpty = await passChecker.ReadToEndAsync();
                        if (isEmpty == null || isEmpty == "")
                        {
                            Xceed.Wpf.Toolkit.MessageBox.Show((string)FindResource("RemovePasswordText1"), (string)FindResource("RemovePasswordText2"), MessageBoxButton.OK);
                            File.SetAttributes(filePath + "Win32AppPas.bin", FileAttributes.Hidden | FileAttributes.System);
                            PasswordRequire.passUsed = false;
                            this.Close();
                        }
                    }
                }            
                else
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Password Provided by You is not correct . Please provide again");
                    Previous_Pass.Clear();
                }
            }

    

        private async void PasswordRemove(object sender, RoutedEventArgs e)
        {
            string passwordPresent = null;
            IsolatedStorageFile password = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null);
            using (StreamReader passReader = new StreamReader(new IsolatedStorageFileStream("Win32AppPas.bin", FileMode.OpenOrCreate, password)))
            {
                filePath = password.GetType().GetField("m_RootDir", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(password).ToString();
                FileAttributes fileAttrib = File.GetAttributes(filePath + "Win32AppPas.bin");
                if (fileAttrib.ToString().Contains(FileAttributes.Hidden.ToString()) || fileAttrib.ToString().Contains(FileAttributes.System.ToString()))
                {
                    File.SetAttributes(filePath + "Win32AppPas.bin", FileAttributes.Normal);
                }
                if (passReader != null)
                {
                    string p = await passReader.ReadToEndAsync();
                    passwordPresent = p;
                }
            }
                string userPassword = Previous_Pass.Password;

                byte[] data = System.Text.Encoding.ASCII.GetBytes(userPassword);

                data = hashConverter.ComputeHash(data);

                userPassword = System.Text.Encoding.ASCII.GetString(data);

                userPassword = Regex.Replace(userPassword, @"\t|\n|\r", "");

                passwordPresent = Regex.Replace(passwordPresent, @"\t|\n|\r", "");
                if (passwordPresent == userPassword)
                {
                    passwordPresent = passwordPresent.Remove(0);
                    File.WriteAllText(filePath + "Win32AppPas.bin", passwordPresent);
                    using (StreamReader passChecker = new StreamReader(new IsolatedStorageFileStream("Win32AppPas.bin", FileMode.OpenOrCreate, password)))
                    {
                        string isEmpty = await passChecker.ReadToEndAsync();
                        if (isEmpty == null || isEmpty == "")
                        {
                            Xceed.Wpf.Toolkit.MessageBox.Show((string)FindResource("RemovePasswordText1"), (string)FindResource("RemovePasswordText2"), MessageBoxButton.OK);
                            File.SetAttributes(filePath + "Win32AppPas.bin", FileAttributes.Hidden | FileAttributes.System);
                            PasswordRequire.passUsed = false;
                            this.Close();
                        }
                    }
                }
                else
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show((string)FindResource("PasswordWrongMessage"));
                    Previous_Pass.Clear();
                }
            }

        private void closeWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
