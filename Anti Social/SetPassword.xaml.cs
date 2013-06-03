﻿using System;
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

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for SetPassword.xaml
    /// </summary>
    public partial class SetPassword : Window
    {
        string passwordPresent ;
        string filePath;

        public SetPassword()
        {
            InitializeComponent();

            this.PreviewKeyDown += new KeyEventHandler(CloseOnEscape);

            this.Loaded += SetPassword_Loaded;

            Password.IsEnabled = false;
            ConPassword.IsEnabled = false;
            finish.IsEnabled = false;
        }

        async void  SetPassword_Loaded(object sender, RoutedEventArgs e)
        {
             IsolatedStorageFile password = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null);
             filePath = password.GetType().GetField("m_RootDir", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(password).ToString();
             if (File.Exists(filePath + "Win32AppPas.bin"))
             {
                 File.SetAttributes(filePath + "Win32AppPas.bin", FileAttributes.Normal);               // If the File is going to be there , change the attribute to normal . 
             }
             StreamReader passReader = new StreamReader(new IsolatedStorageFileStream("Win32AppPas.bin", FileMode.OpenOrCreate, password));             
             
             if (passReader != null)
             {
                 string p = await passReader.ReadLineAsync();
                 if (p != null && p!="")
                 {
                     
                     passwordPresent = p;
                     Previous_Pass.Visibility = System.Windows.Visibility.Visible;
                     PrePass.Visibility = System.Windows.Visibility.Visible;
                     UsePassword.IsChecked = true;
                 }
             }
             passReader.Close();
            
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
            IsolatedStorageFile password = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null);
            if (passwordPresent == null )                                // This will execute if there is no password preent in file . 
            {
                if (pass == null || conpass == null || pass == "" || conpass == "")
                    {
                        MessageBox.Show("All fields are mandatory .");
                    }
                else
                {

                if (pass == conpass)
                {
                        try
                        {
                            var passWriter = new StreamWriter(new IsolatedStorageFileStream("Win32AppPas.bin", FileMode.Create, password));
                            await passWriter.WriteLineAsync(pass);
                            passWriter.Close();

                            StreamReader passReader = new StreamReader(new IsolatedStorageFileStream("Win32AppPas.bin", FileMode.Open, password));
                            if (passReader == null)
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
            else                                                                                        // This will execute if there is password present in the file .
            {
                string prePassword = Previous_Pass.Password;
                if (prePassword == passwordPresent)
                {
                    if (pass == conpass)
                    {
                        try
                        {
                            
                            var passWriter = new StreamWriter(new IsolatedStorageFileStream("Win32AppPas.bin", FileMode.Create, password));
                            await passWriter.WriteLineAsync(pass);
                            passWriter.Close();

                            StreamReader passReader = new StreamReader(new IsolatedStorageFileStream("Win32AppPas.bin", FileMode.Open, password));
                            if (passReader == null)
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

                            MessageBox.Show("There has been an exception while creating the password");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Password Provided by You Don't match . Please check");
                    }

                }
                else
                {
                    MessageBox.Show("Old Password provided is not correct . Please provide correct Password .");
                }
            }
            File.SetAttributes(filePath + "Win32AppPas.bin", FileAttributes.Hidden | FileAttributes.System);
        }
    }
}
