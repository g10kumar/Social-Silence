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
using GaDotNet.Common;
using GaDotNet.Common.Data;
using GaDotNet.Common.Helpers;

namespace SocialSilence
{
    /// <summary>
    /// Interaction logic for CustomerSurvey.xaml
    /// </summary>
    public partial class CustomerSurvey : Window
    {
        public CustomerSurvey()
        {
            InitializeComponent();
        }

        private void SendData(object sender, RoutedEventArgs e)
        {
            List<RadioButton> buttons = myGrid.Children.OfType<RadioButton>().ToList();
            GoogleEvent googleevent;
            foreach (RadioButton butt in buttons)
            {
                if ((bool)butt.IsChecked)
                {

                    if (butt.Tag.ToString().Equals("Yes"))
                    {
                         googleevent = new GoogleEvent("SocialSilence", butt.GroupName, (string)butt.Tag, (string)butt.Tag,1);
                    }
                    else if (butt.Tag.ToString().Equals("No"))
                    {
                         googleevent = new GoogleEvent("SocialSilence", butt.GroupName, (string)butt.Tag, (string)butt.Tag, -1);
                    }
                    else
                    {
                         googleevent = new GoogleEvent("SocialSilence", butt.GroupName, (string)butt.Tag, (string)butt.Tag, 0);
                    }
                    TrackingRequest request = new RequestFactory().BuildRequest(googleevent);
                    GoogleTracking.FireTrackingEvent(request);
                }
            }

            this.Close();
        }
    }
}
