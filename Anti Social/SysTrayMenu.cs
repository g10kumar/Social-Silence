using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Runtime.InteropServices;


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
                    App.Current.MainWindow.Show();
                };
                menuItem.Header = "Restore Applicaiton";
                quickLaunchMenuItems.Add(menuItem);
                quickLaunchMenuItems.Add(new Separator());

                menuItem = new MenuItem();
                menuItem.Click += (sender, e) =>
                {
                    
                };
                menuItem.Header = "About Us";
                quickLaunchMenuItems.Add(menuItem);
                quickLaunchMenuItems.Add(new Separator());

                menuItem = new MenuItem();
                menuItem.Click += (sender, e) =>
                {
                    SetPassword passObj = new SetPassword();
                    passObj.Show();
                };
                menuItem.Header = "Set/Change application Password";
                quickLaunchMenuItems.Add(menuItem);
                quickLaunchMenuItems.Add(new Separator());

                Placement = System.Windows.Controls.Primitives.PlacementMode.AbsolutePoint;
                VerticalOffset = mousePosition.Y - 2;
                HorizontalOffset = mousePosition.X - 8;

                PasswordRequire.notifyIcon.ContextMenuStrip.MouseLeave += ContextMenuStrip_MouseLeave;

            }
            catch
            { }
        }

        void ContextMenuStrip_MouseLeave(object sender, EventArgs e)
        {
            SocialSilence.SysTrayMenu systray = new SysTrayMenu();
            systray.IsOpen = false;
        }
    }
}
