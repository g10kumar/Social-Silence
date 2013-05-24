using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.IO;

namespace WpfApplication2
{
    public class CommonFunctions: Page
    {
        public string filePath;
        public void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        public void WriteToFileSuperFast(IEnumerable<string> domainList , string address , string filePath)
        {

            File.AppendAllLines(filePath, (from r in domainList.AsParallel() select address + "  " + r));        
        }


    }
}
