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
using System.IO.IsolatedStorage;
using System.IO;



namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        public StartPage()
        {
            InitializeComponent();
            Loaded += StartPage_Loaded;
            ShowsNavigationUI = false;

        }

        private async void StartPage_Loaded(object sender, RoutedEventArgs e)         // On loading the page , create a backup of the host file into application . 
        {         
            string hostfile_location = PSHostsFile.HostsFile.GetHostsPath();
          //  System.IO.File.Copy(hostfile_location, @"TextFiles\host", true);
            IsolatedStorageFile writer = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null);
            StreamWriter hostWriter = new StreamWriter(new IsolatedStorageFileStream("host",FileMode.OpenOrCreate,writer));
            //IEnumerable<string> lineHostFile = 
            foreach (string str in File.ReadAllLines(hostfile_location))
            {
                await hostWriter.WriteLineAsync(str);
            }
            hostWriter.Close();

        }

        private void Highlight(object sender, MouseEventArgs e)
        {
            DefaultSetting.BorderBrush = Brushes.Red; 
        }

        private void GoWithDefault(object sender, RoutedEventArgs e)
        {
            CustomizeList CLobj = new CustomizeList();
            this.NavigationService.Navigate(CLobj);
            ShowsNavigationUI = true;
        }

     

        //private void ChangeOpacity(object sender, MouseEventArgs e)
        //{
        //    var button_tri = DefaultSetting.Triggers;
        //}
    }
}
