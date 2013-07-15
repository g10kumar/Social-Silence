using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Runtime.InteropServices;
using System.IO.IsolatedStorage;
using System.IO;
using System.Windows.Interop;


namespace SocialSilence
{
    class SysTrayMenu: ContextMenu
    {
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out System.Drawing.Point lpPoint);

        public SysTrayMenu()
        {
            try
            {
                System.Drawing.Point mousePosition = new System.Drawing.Point(System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Right - 16, System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Bottom - 24);
                GetCursorPos(out mousePosition);

                List<object> quickLaunchMenuItems = new List<object>();
                ItemsSource = quickLaunchMenuItems;

                MenuItem menuItem = null;

                menuItem = new MenuItem();
                menuItem.Click += (sender, e) =>
                {
                    if (App.Current.MainWindow.WindowState == WindowState.Minimized)
                    {
                        App.Current.MainWindow.WindowState = WindowState.Normal;
                    }
                    else
                    {
                        App.Current.MainWindow.Show();
                        CommonFunctions.isHidden = false;
                    }
                };
                menuItem.Header = FindResource("RestoreApp"); 
                quickLaunchMenuItems.Add(menuItem);
                quickLaunchMenuItems.Add(new Separator());

                menuItem = new MenuItem();
                menuItem.Click += (sender, e) =>
                {
                    
                };
                menuItem.Header = FindResource("AboutUs");
                quickLaunchMenuItems.Add(menuItem);
                quickLaunchMenuItems.Add(new Separator());

                menuItem = new MenuItem();
                menuItem.Click += (sender, e) =>
                {
                    SetPassword passObj = new SetPassword();
                    passObj.Show();
                };
                menuItem.Header = FindResource("SetPassword");
                quickLaunchMenuItems.Add(menuItem);
                quickLaunchMenuItems.Add(new Separator());

                    if (PasswordRequire.passUsed)
                    {
                        menuItem = new MenuItem();
                        menuItem.Click += (sender, e) =>
                        {
                            RemovePassword remObj = new RemovePassword();
                            remObj.Show();
                        };
                        menuItem.Header = FindResource("RemovePassword");
                        quickLaunchMenuItems.Add(menuItem);
                        quickLaunchMenuItems.Add(new Separator());
                    }
               

                Placement = System.Windows.Controls.Primitives.PlacementMode.AbsolutePoint;
                VerticalOffset = mousePosition.Y - 2;
                HorizontalOffset = mousePosition.X - 8;

            }
            catch
            { }
        }


    }
}
