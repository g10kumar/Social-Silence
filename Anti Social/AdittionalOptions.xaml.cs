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
using System.Diagnostics;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using PSHostsFile;
using PSHostsFile.Core;

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for AdittionalOptions.xaml
    /// </summary>
    public partial class AdittionalOptions : Page
    {
        string blacklist_path = @"TextFiles\BlackList.txt";
        string address = "0.0.0.0";
        ObservableCollection<string> domainsToBlock = new ObservableCollection<string>();
        MessageBoxResult PopUpResult;
        List<string> domains_present = new List<string>();
        string hostfile_location = PSHostsFile.HostsFile.GetHostsPath();

        public AdittionalOptions()
        {
            InitializeComponent();
        }
        public AdittionalOptions(ObservableCollection<string> theList, MessageBoxResult res):this()
        {
            domainsToBlock = theList;
            PopUpResult = res;
        }

        private void h1_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            string navigateUri = h1.NavigateUri.ToString();
            Process.Start(new ProcessStartInfo(navigateUri));
            e.Handled = true;
        }

        private void FinishSetting(object sender, RoutedEventArgs e)
        {
            XElement root = XElement.Load(@"TextFiles\XMLFile1.xml");
            List<string> finalblacksites = new List<string>();
            foreach (string site in domainsToBlock)
            {
                IEnumerable<string> s = from el in root.Elements("site")
                                        where el.Element("name").Value.Equals(site)
                                        select el.Element("domain").Value;
                foreach (string st in s)
                {
                    finalblacksites.Add(st);
                }
            }

            if ((bool)BlockPopUp.IsChecked)
            {
                if (PopUpResult.Equals(MessageBoxResult.Yes))
                {
                    
                    IEnumerable<HostsFileEntry> listed_domains = PSHostsFile.HostsFile.Get(hostfile_location);
                    PSHostsFile.HostsFile.Set(listed_domains, blacklist_path);
                }
                foreach (string st in finalblacksites)
                {
                    PSHostsFile.HostsFile.Set(st, address, blacklist_path);
                }

                System.IO.File.Copy(blacklist_path, hostfile_location, true);

            }
            else 
            {
                if (PopUpResult.Equals(MessageBoxResult.Yes))
                {
                    IEnumerable<HostsFileEntry> listed_domains = PSHostsFile.HostsFile.Get(hostfile_location);
                    PSHostsFile.HostsFile.Set(listed_domains, @"TextFiles\host");
                }
                foreach (string st in finalblacksites)
                {
                    PSHostsFile.HostsFile.Set(st, address, @"TextFiles\host");
                }
                System.IO.File.Copy(@"TextFiles\host",hostfile_location, true);
              
            }


           
            
           
        }
    }
}
