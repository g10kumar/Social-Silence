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
using System.Windows.Navigation;
using System.Windows.Shapes;
using PSHostsFile;
using System.IO.IsolatedStorage;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.Xml.Linq;



namespace SocialSilence
{
    /// <summary>
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class StartPage : CommonFunctions
    {

        bool backUpHost = true;
        ObservableCollection<string> defaultlist = new ObservableCollection<string>();
        MessageBoxResult result;
        List<string> domainsPresent = new List<string>();
        string hostMessage;
        public StartPage()
        {
            InitializeComponent();
                      
        }

        public StartPage(string path):this()
        {
            filePath = path;                                                    // This is the path of the local file storage for the application.
            Loaded += StartPage_Loaded;
        }

        private void StartPage_Loaded(object sender, RoutedEventArgs e)         // On loading the page , create a backup of the host file into application . 
        {
            string hostfile_location = PSHostsFile.HostsFile.GetHostsPath();
            FileAttributes hostattributes = File.GetAttributes(hostfile_location);

            if (hostattributes.ToString().Contains(FileAttributes.Hidden.ToString()) || hostattributes.ToString().Contains(FileAttributes.System.ToString()) || hostattributes.ToString().Contains(FileAttributes.ReadOnly.ToString()))
            {
                File.SetAttributes(hostfile_location, FileAttributes.Normal);
            }

            string[] lines = File.ReadAllLines(hostfile_location);
            string matchLine = "#This file is Generated by SocialSilence";
            foreach (string line in lines)                                            //Read the lines in the host file to check if the previous file was created by application. 
            {
                if(Regex.IsMatch(line,matchLine))
                {
                    backUpHost = false;                                               // If the host file was created by application do not make copy .
                    break;
                }
            }

           
            if (backUpHost)
            {
                if (System.IO.File.Exists(filePath + "host"))                                // Check if the backkup host file exits or not . 
                {
                    FileAttributes hostAttrib = File.GetAttributes(filePath + "host");
                    if (hostAttrib.ToString().Contains(FileAttributes.System.ToString()) || hostAttrib.ToString().Contains(FileAttributes.Hidden.ToString()))
                    {
                        File.SetAttributes(filePath + "host", FileAttributes.Normal);
                    }
                }
                File.Copy(hostfile_location, filePath + "host", true);
                File.SetAttributes(filePath + "host", FileAttributes.System | FileAttributes.Hidden);
                
            }
            

        }


        private void GoWithDefault(object sender, RoutedEventArgs e)
        {
            XElement root = XElement.Load(@"Resources\XMLFile1.xml");

            IEnumerable<string> site = from el in root.Elements("site")
                                       select el.Element("name").Value;

            foreach (string elm in site)
            {
                defaultlist.Add(elm);
            }

            if(File.Exists(filePath+"host") && backUpHost)
            {
                hostMessage = (string)this.FindResource("hostMessage");
                File.SetAttributes(filePath + "host", FileAttributes.Normal);
                string [] lines = File.ReadAllLines(filePath + "host");
                string matchLine = "^www.";
                string matchline2 = ".com$";                  
                foreach (string line in lines)
                {
                    if (Regex.IsMatch(line, matchLine, RegexOptions.IgnoreCase) || Regex.IsMatch(line, matchline2, RegexOptions.IgnoreCase))  // Check the backup host file for domains listed by the user. 
                {
                    result = Xceed.Wpf.Toolkit.MessageBox.Show(hostMessage, "Message", MessageBoxButton.YesNo);
                    break;
                }
                }

                foreach(string line in lines)
                {
                    if(Regex.IsMatch(line,matchLine,RegexOptions.IgnoreCase)|| Regex.IsMatch(line,matchline2,RegexOptions.IgnoreCase))
                    {
                        
                        domainsPresent.Add(line);
                    }

                }
                File.SetAttributes(filePath + "host", FileAttributes.Hidden | FileAttributes.System);
            }

            AdittionalOptions page = new AdittionalOptions(defaultlist,result,filePath,false,domainsPresent);
            NavigationService.Navigate(page);
            page.ShowsNavigationUI = false;

           

        }

        private void GoToCustomizeList(object sender, RoutedEventArgs e)
        {
            NavigationWindow win = (NavigationWindow)Window.GetWindow(this);
            win.RemoveBackEntry();
            CustomizeList CLobj = new CustomizeList(filePath,backUpHost);
            this.NavigationService.Navigate(CLobj);
            ShowsNavigationUI = true;
        }

     

        //private void ChangeOpacity(object sender, MouseEventArgs e)
        //{
        //    var button_tri = DefaultSetting.Triggers;
        //}
    }
}
