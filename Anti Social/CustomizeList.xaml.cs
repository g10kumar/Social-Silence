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
using PSHostsFile;
using PSHostsFile.Core;
using System.IO;
using System.Management;
using System.Xml.Linq;
using Xceed.Wpf.Toolkit;
using System.Collections.ObjectModel;
using System.Xml;
using System.Windows.Forms;
using System.IO.IsolatedStorage;
namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class CustomizeList : Page
    {
        string address = "0.0.0.0";
        string hostfile_location;
        IEnumerable<HostsFileEntry> listed_domains;
        List<string> domains_present = new List<string>();
        ObservableCollection<string> blackListed = new ObservableCollection<string>();
        ObservableCollection<string> whiteListed = new ObservableCollection<string>();
        List<string> Whitebuffer = new List<string>();
        List<string> Blackbuffer = new List<string>();
        MessageBoxResult result;
        public CustomizeList()
        {
            InitializeComponent();
            //ShowsNavigationUI = false;
            //Loaded += Page1_Loaded;

        }

        void Page1_Loaded(object sender, RoutedEventArgs e)
        {
            NavigationWindow win = (NavigationWindow)Window.GetWindow(this);
            win.ShowsNavigationUI = false;
        }
        private async void AddToWhiteList(object sender, RoutedEventArgs e)
        {
            if (!BlackList.SelectedItems.Count.Equals(0))                          // When one or more sites are selected then only work.
            {
                WhiteList.ItemsSource = null;
                WhiteList.Items.Clear();
                foreach (string whitesite in whiteListed)                             // Firstly add sites to buffer
                {
                    Whitebuffer.Add(whitesite);
                }

                whiteListed.Clear();                                                 //Clear the white list . 
                string white_sites = BlackList.SelectedValue;
                string[] site_array = white_sites.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);


                foreach (string wsite in site_array)                                  // Then add sites from blacklist to buffer.
                {
                    Whitebuffer.Add(wsite);
                }

                Whitebuffer.Sort();                                                  // Sort items in buffer.

                foreach (string bufferstring in Whitebuffer)                         // Add sites from the buffer to white list.
                {
                    whiteListed.Add(bufferstring);
                }


                WhiteList.ItemsSource = whiteListed;

                foreach (string bsite in site_array)                                 // Remove that site from blacklist. 
                {
                    await BlackList.Dispatcher.BeginInvoke((Action)delegate { BlackList.ItemsSource = null; blackListed.Remove(bsite); }, null);
                }

                Whitebuffer.Clear();                                                 //Clear the buffer. 
                BlackList.Items.Clear();
                BlackList.ItemsSource = blackListed;
            }
        }

        private async void AddToBlackList(object sender, RoutedEventArgs e)
        {
            if (!WhiteList.SelectedItems.Count.Equals(0))
            {
                BlackList.ItemsSource = null;
                BlackList.Items.Clear();
                foreach (string blacksite in blackListed)                         // firstly add sites to buffer
                {
                    Blackbuffer.Add(blacksite);
                }
                blackListed.Clear();                                            //This line stops repetation of the sites in the list
                string black_sites = WhiteList.SelectedValue;
                string[] site_array = black_sites.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);


                foreach (string bsite in site_array)                             // Then add sites from bu
                {
                    Blackbuffer.Add(bsite);
                }

                Blackbuffer.Sort();

                foreach (string bufferstring in Blackbuffer)                    // Copy back sites from buffer .
                {
                    blackListed.Add(bufferstring);
                }

                BlackList.ItemsSource = blackListed;

                foreach (string bsite in site_array)
                {
                    await WhiteList.Dispatcher.BeginInvoke((Action)delegate { WhiteList.ItemsSource = null; whiteListed.Remove(bsite); }, null);
                }

                Blackbuffer.Clear();
                WhiteList.Items.Clear();
                WhiteList.ItemsSource = whiteListed;
            }


        }

        private async void Store_Lists(object sender, RoutedEventArgs e)
        {
            //Page2 page2 = new Page2();
            //NavigationService.Navigate(page2);
            //page2.ShowsNavigationUI = true;
            hostfile_location = PSHostsFile.HostsFile.GetHostsPath();
            listed_domains = PSHostsFile.HostsFile.Get(hostfile_location);
            foreach (HostsFileEntry entry in listed_domains)
            {
                domains_present.Add(entry.Hostname);
            }
            if (!domains_present.Count.Equals(0))
            {
                result = System.Windows.MessageBox.Show("There are domains listed in host file. Do you want to keep them.", "Message", MessageBoxButton.YesNo);
                //MessageBox.Show("There are domains that are present in host file");
                //if (result.Equals(MessageBoxResult.Yes))
                //{
                //    foreach (string entry in domains_present)
                //    {
                //        blackListed.Add(entry);
                //    }
                //}
            }

          
            if (!whiteListed.Count.Equals(0))
            {
                IsolatedStorageFile whiteSite = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null);
                var Writer = new StreamWriter(new IsolatedStorageFileStream("WhiteList.bin", FileMode.Create , whiteSite));
                foreach (string str in whiteListed)
                {
                    await Writer.WriteLineAsync(str);
                }
                Writer.Close();
            }
            AdittionalOptions page = new AdittionalOptions(blackListed,result);
            NavigationService.Navigate(page);
            page.ShowsNavigationUI = false;
            
            
            
            //string blacklist_path = @"TextFiles\BlackList.txt";
            //string path = System.Windows.Forms.Application.StartupPath;
            //XElement root = XElement.Load(@"TextFiles\XMLFile1.xml");
            //List<string> finalblacksites = new List<string>();
            //IEnumerable<HostsFileEntry> present_sites = PSHostsFile.HostsFile.Get(blacklist_path);

            ////XmlNode node = d

            //foreach (string site in blackListed)
            //{
            //    IEnumerable<string> s = from el in root.Elements("site")
            //                            where el.Element("name").Value.Equals(site)
            //                            select el.Element("domain").Value;
            //    foreach (string st in s)
            //    {
            //        finalblacksites.Add(st);
            //    }
            //}


            //Remove obj = new Remove();
            //foreach (HostsFileEntry prest in present_sites)
            //{
            //    obj.RemoveFromFile(prest.Hostname, blacklist_path);
            //}





            //foreach (string st in finalblacksites)
            //{
            //    PSHostsFile.HostsFile.Set(st, address, blacklist_path);
            //}



            ////}


            }

        private  async void RetrieveList(object sender, RoutedEventArgs e)
        {           
           
            try
            {
            IsolatedStorageFile domain = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null);
            StreamReader Reader = new StreamReader(new IsolatedStorageFileStream("WhiteList.bin", FileMode.OpenOrCreate, domain));
                if (Reader != null)
                {
                    string p = await Reader.ReadToEndAsync();
                    if (p == null)
                    {
                        LoadAllFromXml();
                    }
                    else
                    {
                        whiteListed.Clear();
                        string[] Wsite = p.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach(string strin in Wsite)
                        {
                            
                            whiteListed.Add(strin);
                            
                        }
                       
                        LoadAllFromXml();
                    }
                }
            }
            catch
            {
                //System.IO.FileNotFoundException ;
            }
            
        }

        private void LoadAllFromXml()
        {          
           
            BlackList.Items.Clear();            

            XElement root = XElement.Load(@"TextFiles\XMLFile1.xml");

            IEnumerable<string> site = from el in root.Elements("site")
                                       select el.Element("name").Value;

            foreach (string elm in site)
            {
                blackListed.Add(elm);
            }
            
            foreach (string str in whiteListed)
            {
                blackListed.Remove(str);
            }

            BlackList.ItemsSource = blackListed;
            WhiteList.ItemsSource = whiteListed;
        }
    }
}
