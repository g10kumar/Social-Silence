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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string address = "0.0.0.0";
        string hostfile_location;
        IEnumerable<HostsFileEntry> listed_domains;
        List<string> domains_present = new List<string>();
        ObservableCollection<string> blackListed = new ObservableCollection<string>();
        ObservableCollection<string> whiteListed = new ObservableCollection<string>();
        List<string> Whitebuffer = new List<string>();
        List<string> Blackbuffer = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Contact_router(object sender, RoutedEventArgs e)
        {


            //int _listed = domains_present.Count;            // Get the number of the sites listed

            //for (int i = 0; i < _listed; i++)               // Get the Hostname listed currently present in the 
            //{
            //    blackListed.Add(domains_present.ElementAt(i).Hostname);

            //}

            string hostfile = hostfile_location;

            FileInfo host_info = new FileInfo(hostfile);


            // Writing to Host File
            //List<PSHostsFile.HostsFileEntry> blacklist_domains = new List<PSHostsFile.HostsFileEntry>();


            //HostsFileEntry enrty1 = new HostsFileEntry("twitter.com", address);




            if (blackListed.Contains("twitter.com"))
            {
                System.Windows.MessageBox.Show("The entry that you are trying to make is already present");
            }
            else
            {
                PSHostsFile.HostsFile.Set("twitter.com", address);
                DateTime modified_time = new DateTime();
                modified_time = host_info.LastAccessTimeUtc;

            }





            //CookieContainer cookieContainer = new CookieContainer();
            ////WebClient initiate = new WebClient();
            //WebProxy proxy = null;


            //HttpClientHandler handler = new HttpClientHandler();
            //handler.Proxy = proxy;
            //handler.UseCookies = true;
            //handler.CookieContainer = cookieContainer;
            //handler.Credentials = new NetworkCredential("admin", "admin");
            //HttpClient authenticate = new HttpClient(handler);
            //authenticate.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
            //authenticate.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla","5.0"));
            //authenticate.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-US",0.5));
            //authenticate.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            ////authenticate.DefaultRequestHeaders.Authorization = 
            //var authentication_response = await authenticate.GetAsync(new Uri("http://192.168.1.1/"));
            //var request = authentication_response.RequestMessage;
            ////authentication_response.Headers.
            //if (authentication_response.StatusCode.Equals(System.Net.HttpStatusCode.Unauthorized)) 
            //{
            //    MessageBox.Show("The provided username & password are not correct");
            //}

        }

        //void initiate_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        //{
        //    HtmlDocument filter_html = new HtmlDocument();
        //    filter_html.Load("D:\\Filters.htm");

        //    if (filter_html.DocumentNode != null)
        //    {
        //        List<string> linkTags = new List<string>();
        //        foreach (HtmlNode link in filter_html.DocumentNode.SelectNodes("Link"))
        //        {
        //            HtmlAttribute att = link.Attributes["href"];
        //            linkTags.Add(att.Value);
        //        }


        //    }

        //}

        public void remove_all_sites()
        {

            foreach (HostsFileEntry entry in listed_domains)
            {
                PSHostsFile.HostsFile.Remove(entry.Hostname);
            }

            

        }



        private void loaded(object sender, RoutedEventArgs e)
        {
            hostfile_location = PSHostsFile.HostsFile.GetHostsPath();
            listed_domains = PSHostsFile.HostsFile.Get(hostfile_location);
            BlackList.Items.Clear();
            
            foreach (HostsFileEntry entry in listed_domains)
            {
                domains_present.Add(entry.Hostname);
            }
            if (!domains_present.Count.Equals(0))
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("There are domains listed in host file. Do you want to keep them.", "Message", MessageBoxButton.YesNo);
                    //MessageBox.Show("There are domains that are present in host file");
                if (result.Equals(MessageBoxResult.Yes)) 
                {
                    foreach (string entry in domains_present)
                    {
                        blackListed.Add(entry);
                    }
                }   
            }

            XElement root = XElement.Load(@"TextFiles\XMLFile1.xml");

            IEnumerable<string> site = from el in root.Elements("site").AsParallel()
                                         select el.Element("name").Value;

            foreach (string elm in site) 
            {
                blackListed.Add(elm);
            }

            BlackList.ItemsSource = blackListed;



    
         }

        private void BackUp_DNS(object sender, RoutedEventArgs e)
        {
            ManagementScope oMs = new ManagementScope("\\\\localhost\\root\\cimv2");

            string strQuery = "select DNSServerSearchOrder from Win32_NetworkAdapterConfiguration where IPEnabled='true'";

            ObjectQuery oQ = new ObjectQuery(strQuery);

            ManagementObjectSearcher oS = new ManagementObjectSearcher(oMs, oQ);

            ManagementObjectCollection oRc = oS.Get();

            IsolatedStorageFile dnsFile = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null);


            using (StreamWriter file = new StreamWriter(new IsolatedStorageFileStream("DNSBackUp.bin",FileMode.Create,FileAccess.Write,dnsFile)))
           {

                    foreach (ManagementObject oR in oRc)
                    {

                        foreach (string str in (Array)(oR.Properties["DNSServerSearchOrder"].Value))
                        {

                            file.WriteLine(str);

                        }

                    }
       
                }

            
        }

        private void CreateNew_DNS(object sender, RoutedEventArgs e)
        {
            using(var NWconManager = new ManagementClass("Win32_NetworkAdapterConfiguration"))
            {
                using(var NWconfig = NWconManager.GetInstances())
                {
                    foreach(var managementObject in NWconfig.Cast<ManagementObject>().Where(managmentObject => (bool)managmentObject["IPEnabled"]))
                    {
                        ManagementBaseObject managementBase = managementObject.GetMethodParameters("SetDNSServerSearchOrder");
                        if (managementBase != null)
                        {
                            string[] OpenDnsIP = { "208.67.222.222", "208.67.220.220" };
                            managementBase["DNSServerSearchOrder"] = OpenDnsIP;
                            managementObject.InvokeMethod("SetDNSServerSearchOrder", managementBase, null);
                        }

                    }
                }

            }

        }

        private void FallBack_DNS()
        {
            string DNS_present;

            DNS_present = File.ReadAllText(@"TextFiles\DNSBackUp.txt");


            string[] oldDNS = DNS_present.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            using (var NWconManager = new ManagementClass("Win32_NetworkAdapterConfiguration"))
            {
                using (var NWconfig = NWconManager.GetInstances())
                {
                    foreach (var managementObject in NWconfig.Cast<ManagementObject>().Where(managmentObject => (bool)managmentObject["IPEnabled"]))
                    {
                        ManagementBaseObject managementBase = managementObject.GetMethodParameters("SetDNSServerSearchOrder");
                        if (managementBase != null)
                        {
                            managementBase["DNSServerSearchOrder"] = oldDNS;
                            managementObject.InvokeMethod("SetDNSServerSearchOrder", managementBase, null);
                        }

                    }
                }

            }

        }

        private async void AddToWhiteList(object sender, RoutedEventArgs e)
        {      
            if(!BlackList.SelectedItems.Count.Equals(0))                          // When one or more sites are selected then only work.
            {
            WhiteList.ItemsSource = null;
            WhiteList.Items.Clear();
            foreach (string whitesite in whiteListed)                             // Firstly add sites to buffer
            {
                Whitebuffer.Add(whitesite);
            }
          
            whiteListed.Clear();                                                 //Clear the white list . 
            string white_sites = BlackList.SelectedValue;
            string[] site_array = white_sites.Split(new string[]{","}, StringSplitOptions.RemoveEmptyEntries);
           

            foreach(string wsite in site_array)                                  // Then add sites from blacklist to buffer.
            {
                Whitebuffer.Add(wsite);
            }

            Whitebuffer.Sort();                                                  // Sort items in buffer.

            foreach (string bufferstring in Whitebuffer)                         // Add sites from the buffer to white list.
            {
                whiteListed.Add(bufferstring);
            }         
                        

            WhiteList.ItemsSource = whiteListed;

            foreach(string bsite in site_array)                                 // Remove that site from blacklist. 
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
            if(!WhiteList.SelectedItems.Count.Equals(0))
            {
            BlackList.ItemsSource = null;
            BlackList.Items.Clear();
            foreach (string blacksite in blackListed)                         // firstly add sites to buffer
            {
                Blackbuffer.Add(blacksite);
            }
            blackListed.Clear();                                            //This line stops repetation of the sites in the list
            string black_sites = WhiteList.SelectedValue;
            string[] site_array = black_sites.Split(new string[]{","}, StringSplitOptions.RemoveEmptyEntries);


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

            foreach(string bsite in site_array)
            {
                await WhiteList.Dispatcher.BeginInvoke((Action)delegate { WhiteList.ItemsSource = null; whiteListed.Remove(bsite); }, null);                
            }

            Blackbuffer.Clear();
            WhiteList.Items.Clear();
            WhiteList.ItemsSource = whiteListed;
            }


        }

        private void Store_Lists(object sender, RoutedEventArgs e)
        {
            string blacklist_path = @"TextFiles\BlackList.txt";
            string path = System.Windows.Forms.Application.StartupPath;
            XElement root = XElement.Load(@"TextFiles\XMLFile1.xml");
            List<string> finalblacksites = new List<string>();
            IEnumerable<HostsFileEntry> present_sites = PSHostsFile.HostsFile.Get(blacklist_path);

            //XmlNode node = d

            foreach (string site in blackListed)
            {
                IEnumerable<string> s = from el in root.Elements("site")
                                        where el.Element("name").Value.Equals(site)
                                        select el.Element("domain").Value;
                foreach (string st in s)
                {
                    finalblacksites.Add(st);
                }
            }


            Remove obj = new Remove();
            foreach (HostsFileEntry prest in present_sites)
            {
                obj.RemoveFromFile(prest.Hostname, blacklist_path);
            }


            
            

            foreach (string st in finalblacksites)
            {
                PSHostsFile.HostsFile.Set(st, address, blacklist_path);
            }

            

            //}

            
        }



    }
}
