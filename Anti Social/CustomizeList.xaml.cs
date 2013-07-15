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
using System.Text.RegularExpressions;

namespace SocialSilence
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class CustomizeList : CommonFunctions
    {
        List<string> domains_present = new List<string>();
        ObservableCollection<string> blackListed = new ObservableCollection<string>();
        ObservableCollection<string> whiteListed = new ObservableCollection<string>();
        List<string> Whitebuffer = new List<string>();
        List<string> Blackbuffer = new List<string>();
        MessageBoxResult result;
        bool backUpHost;

        public CustomizeList(bool hostBackUP)
        {
            InitializeComponent();
            backUpHost = hostBackUP;
            //ShowsNavigationUI = false;
            //System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon();
        }

        void NavigationService_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                ShowsNavigationUI = false;
            }
        }


        private async  void AddToWhiteList(object sender, RoutedEventArgs e)
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
                //string white_sites = BlackList.SelectedValue;
                //string[] site_array = white_sites.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);


                //foreach (string wsite in site_array)                                  // Then add sites from blacklist to buffer.
                //{
                //    Whitebuffer.Add(wsite);
                //}

                //var list = (IEnumerable<string>)BlackList.SelectedItems;
                //IEnumerable<string> newlist = list.Cast<string>();

                Whitebuffer.AddRange(((IEnumerable<object>)BlackList.SelectedItems).Cast<string>());

                Whitebuffer.Sort();                                                  // Sort items in buffer.

                foreach (string bufferstring in Whitebuffer)                         // Add sites from the buffer to white list.
                {
                    whiteListed.Add(bufferstring);
                }


                WhiteList.ItemsSource = whiteListed;

                string[] site_array = BlackList.SelectedItems.Cast<string>().ToArray();

                foreach (string bsite in site_array)                                 // Remove that site from blacklist. 
                {
                    await BlackList.Dispatcher.BeginInvoke((Action)delegate { BlackList.ItemsSource = null; blackListed.Remove(bsite); }, null);
                }

                Whitebuffer.Clear();                                                 //Clear the buffer. 
                BlackList.Items.Clear();
                BlackList.ItemsSource = blackListed;
                SelectAllBlackList.IsChecked = false;
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
               // string black_sites = WhiteList.SelectedValue;
               // string[] site_array = black_sites.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);


                //foreach (string bsite in site_array)                             // Then add sites from bu
                //{
                //    Blackbuffer.Add(bsite);
                //}
                Blackbuffer.AddRange(((IEnumerable<object>)WhiteList.SelectedItems).Cast<string>());        // This line is adding sites from the buffer . 

                Blackbuffer.Sort();

                foreach (string bufferstring in Blackbuffer)                    // Copy back sites from buffer .
                {
                    blackListed.Add(bufferstring);
                }

                BlackList.ItemsSource = blackListed;

                string[] site_array = WhiteList.SelectedItems.Cast<string>().ToArray();


                foreach (string bsite in site_array )
                {
                  await WhiteList.Dispatcher.BeginInvoke((Action)delegate { WhiteList.ItemsSource = null; whiteListed.Remove(bsite); }, null);
                }

                Blackbuffer.Clear();
                WhiteList.Items.Clear();                
                WhiteList.ItemsSource = whiteListed;
                SelectAllWhiteList.IsChecked = false;
            }


        }

        private async void Store_Lists(object sender, RoutedEventArgs e)
        {

            if (!whiteListed.Count.Equals(0) || blackListed.Count.Equals(196))                      // Update list on all times , even when all the items are removed
            {                                                                                       // from White list .
                IsolatedStorageFile whiteSite = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null);
                using (var Writer = new StreamWriter(new IsolatedStorageFileStream("WhiteList.bin", FileMode.Create, whiteSite)))
                {
                    foreach (string str in whiteListed)
                    {
                        await Writer.WriteLineAsync(str);
                    }
                }
            }

            if (File.Exists(PasswordRequire.filePath + "host") && backUpHost)
            {
                File.SetAttributes(PasswordRequire.filePath + "host", FileAttributes.Normal);
                string[] lines = File.ReadAllLines(PasswordRequire.filePath + "host");
                string matchLine = "^www.";
                string matchline2 = ".com$";
                foreach (string line in lines)
                {
                    if (Regex.IsMatch(line, matchLine, RegexOptions.IgnoreCase) || Regex.IsMatch(line, matchline2, RegexOptions.IgnoreCase))  // Check the backup host file for domains listed by the user. 
                    {
                        result = Xceed.Wpf.Toolkit.MessageBox.Show("There are domains listed in host file. Do you want to keep them.", "Message", MessageBoxButton.YesNo);
                        break;
                    }
                }

                foreach (string line in lines)
                {
                    if (Regex.IsMatch(line, matchLine, RegexOptions.IgnoreCase) || Regex.IsMatch(line, matchline2, RegexOptions.IgnoreCase))
                    {

                        domains_present.Add(line);
                    }

                }

                File.SetAttributes(PasswordRequire.filePath + "host", FileAttributes.Hidden | FileAttributes.System);
            }
            
            

         
            AdittionalOptions page = new AdittionalOptions(blackListed,result,true,domains_present);
            NavigationService.Navigate(page);
            page.ShowsNavigationUI = false;


            }

        private  async void RetrieveList(object sender, RoutedEventArgs e)                          // This is the function that is going to be executed on loading . 
        {
            this.NavigationService.Navigating += NavigationService_Navigating;
           
            try
            {
            IsolatedStorageFile domain = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null);
            using (StreamReader Reader = new StreamReader(new IsolatedStorageFileStream("WhiteList.bin", FileMode.OpenOrCreate, domain)))
            {
                if (Reader != null)
                {
                    string p = await Reader.ReadToEndAsync();
                    if (p == null || p == "")
                    {
                        LoadAllFromXml();
                    }
                    else
                    {
                        whiteListed.Clear();
                        string[] Wsite = p.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string strin in Wsite)
                        {

                            whiteListed.Add(strin);

                        }

                        LoadAllFromXml();
                    }
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

            XElement root = XElement.Load(@"Resources\XMLFile1.xml");

            IEnumerable<string> site = from el in root.Elements("site")
                                       select el.Element("name").Value;

            foreach (string elm in site)
            {
                blackListed.Add(elm);
            }

            if (!whiteListed.Count.Equals(0))
            {
                foreach (string str in whiteListed)
                {
                    blackListed.Remove(str);
                }

            }          
            
            BlackList.ItemsSource = blackListed;
            WhiteList.ItemsSource = whiteListed;
        }

        private void BlackListSelcectAll(object sender, RoutedEventArgs e)
        {
            List<string> newlist = new List<string>();
            newlist.AddRange(blackListed);
            BlackList.SelectedItemsOverride = newlist;
            BlackList.ItemSelectionChanged += BlackList_ItemSelectionChanged;
            
        }

        void BlackList_ItemSelectionChanged(object sender, Xceed.Wpf.Toolkit.Primitives.ItemSelectionChangedEventArgs e)
        {
            BlackList.ItemSelectionChanged -= BlackList_ItemSelectionChanged;
            SelectAllBlackList.IsChecked = false;
        }

        private void WhiteListSelcectAll(object sender, RoutedEventArgs e)
        {
            List<string> newlist = new List<string>();
            newlist.AddRange(whiteListed);
            WhiteList.SelectedItemsOverride = newlist;
            WhiteList.ItemSelectionChanged += WhiteList_ItemSelectionChanged;
           

        }

        void WhiteList_ItemSelectionChanged(object sender, Xceed.Wpf.Toolkit.Primitives.ItemSelectionChangedEventArgs e)
        {
            WhiteList.ItemSelectionChanged -= WhiteList_ItemSelectionChanged;
            SelectAllWhiteList.IsChecked = false;
            
        }

        private void BlackListUnselectAll(object sender, RoutedEventArgs e)
        {
            BlackList.ItemSelectionChanged -= BlackList_ItemSelectionChanged;
            if (blackListed.Count == BlackList.SelectedItems.Count)
            {
                BlackList.SelectedItemsOverride = null;
            }
        }

        private void WhiteListUnselectAll(object sender, RoutedEventArgs e)
        {
            WhiteList.ItemSelectionChanged -= WhiteList_ItemSelectionChanged;
            if (whiteListed.Count == WhiteList.SelectedItems.Count)
            {
                WhiteList.SelectedItemsOverride = null;
            }

        }





     
    }
}
