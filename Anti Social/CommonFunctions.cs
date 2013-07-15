using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Management;
using System.Threading;
using System.Security;
using System.Diagnostics;
using System.Reflection;

namespace SocialSilence
{
    public class CommonFunctions: Page
    {

        public static bool isHidden = false;
        public void btnClose_Click(object sender, RoutedEventArgs e)
        {

            if (sender.GetType().Name == "Button")
            {
                App.Current.MainWindow.Hide();
                isHidden = true;
                //Window.GetWindow(this).Close();
 
            }
            else if (((System.Windows.Controls.Page)(((System.Windows.Controls.ContentControl)(sender)).Content)).Title == "FinalPage")
            {
                //((WpfApplication2.FinalPage)(this)).
                //Window.GetWindow(this).Hide();
                App.Current.MainWindow.Hide();
                isHidden = true;
            }
        }

        public void WriteToFileSuperFast(IEnumerable<string> domainList , string address , string filePath)
        {
            if (address == null)
            {
                File.AppendAllLines(filePath, (from r in domainList.AsParallel() select r));
            }
            else
            {

                File.AppendAllLines(filePath, (from r in domainList.AsParallel() select address + "  " + r));
            }
        }

        public void SettingRestore(string destination,string hostfile_location, bool restroreDNS)
        {
            FileAttributes hostattributes = File.GetAttributes(hostfile_location);

            if (hostattributes.ToString().Contains(FileAttributes.Hidden.ToString()) || hostattributes.ToString().Contains(FileAttributes.System.ToString()))
            {
                File.SetAttributes(hostfile_location, FileAttributes.Normal);
            }
            try
            {

                System.IO.File.Copy(destination + "host", hostfile_location, true);                         // Copy the backup host file to that of the present host file . 
            }
            catch
            {
                Thread.Sleep(1000);
                System.IO.File.Copy(destination + "host", hostfile_location, true);
            }
            
            if (restroreDNS)                                                                            // If dns was set to that of OpenDNS then change the dns to that of old one. 
            {
                string DNS_present;
                List<string> nullenumerable = new List<string>();
                DNS_present = File.ReadAllText(destination + "DNSBackUp.bin");


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
                nullenumerable.Add("");
                //File.WriteAllLines(destination + "DNSBackUp.bin", nullenumerable);
                File.Create(destination + "DNSBackUp.bin");


            }
            hostattributes = File.GetAttributes(hostfile_location);

            if (hostattributes.ToString().Contains(FileAttributes.Hidden.ToString()) || hostattributes.ToString().Contains(FileAttributes.System.ToString()))
            {
                File.SetAttributes(hostfile_location, FileAttributes.Normal);
            }

            if (PasswordRequire.firstRun)
            {
                CustomerSurvey surveyObj = new CustomerSurvey();
                surveyObj.Show();
            }

        }


        public void SetLanguageDictionary(string userLanguage)
        {

            ResourceDictionary dictionary = new ResourceDictionary();

            try
            {
                switch(userLanguage)
                {
                    case "en":
                        dictionary.Source = new Uri(Path.Combine(Environment.CurrentDirectory, @"\resources\string.en.xaml"), UriKind.RelativeOrAbsolute);
                        break;
                    case "de":
                        dictionary.Source = new Uri(Path.Combine(Environment.CurrentDirectory, @"\resources\string.de.xaml"), UriKind.RelativeOrAbsolute);
                        break;
                    case "it":
                        dictionary.Source = new Uri(Path.Combine(Environment.CurrentDirectory, @"\resources\string.it.xaml"), UriKind.RelativeOrAbsolute);
                        break;
                    case "pt":
                        dictionary.Source = new Uri(Path.Combine(Environment.CurrentDirectory, @"\resources\string.pt.xaml"), UriKind.RelativeOrAbsolute);
                        break;
                    case "fr":
                        dictionary.Source = new Uri(Path.Combine(Environment.CurrentDirectory, @"\resources\string.fr.xaml"), UriKind.RelativeOrAbsolute);
                        break;
                    case "id":
                        dictionary.Source = new Uri(Path.Combine(Environment.CurrentDirectory, @"\resources\string.id.xaml"), UriKind.RelativeOrAbsolute);
                        break;
                    case "es":
                        dictionary.Source = new Uri(Path.Combine(Environment.CurrentDirectory, @"\resources\string.es.xaml"), UriKind.RelativeOrAbsolute);
                        break;
                    case "tr":
                        dictionary.Source = new Uri(Path.Combine(Environment.CurrentDirectory, @"\resources\string.tr.xaml"), UriKind.RelativeOrAbsolute);
                        break;
                    case "ru":
                        dictionary.Source = new Uri(Path.Combine(Environment.CurrentDirectory, @"\resources\string.ru.xaml"), UriKind.RelativeOrAbsolute);
                        break;
                    case "ja":
                        dictionary.Source = new Uri(Path.Combine(Environment.CurrentDirectory, @"\resources\string.ja.xaml"), UriKind.RelativeOrAbsolute);
                        break;
                    case "zh":
                        dictionary.Source = new Uri(Path.Combine(Environment.CurrentDirectory, @"\resources\string.zh.xaml"), UriKind.RelativeOrAbsolute);
                        break;
                    default:
                        dictionary.Source = new Uri(Environment.CurrentDirectory + @"\resources\string.en.xaml");
                        break;
                }
                App.Current.Resources.MergedDictionaries.Add(dictionary);
            }
            catch(Exception ex)
            {
                Uri test = new Uri(Environment.CurrentDirectory + @"/Resources/string.en.xaml");
                MessageBox.Show("A fault has occured on the common function page. " +ex.Message+ex.StackTrace);
            }

        }

        public void setNotifyIconText(System.Windows.Forms.NotifyIcon notifyIcon, string notificationToolTip)
        {
            if (notificationToolTip.Length >= 128)
            {
                throw new ArgumentOutOfRangeException("Text limited to 127 characters");
            }

            Type t = typeof(System.Windows.Forms.NotifyIcon);
            BindingFlags hidden = BindingFlags.NonPublic | BindingFlags.Instance;
            t.GetField("text", hidden).SetValue(notifyIcon, notificationToolTip);
            if ((bool)t.GetField("added", hidden).GetValue(notifyIcon))
            {
                t.GetMethod("UpdateIcon", hidden).Invoke(notifyIcon, new object[] { true });
            }


        }
     
    }
}
