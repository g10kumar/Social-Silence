﻿#pragma checksum "..\..\AdittionalOptions.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "4F367F9958F1CC5D1AA3708177D1D973"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18033
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using WpfApplication2;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.Chromes;
using Xceed.Wpf.Toolkit.Core.Converters;
using Xceed.Wpf.Toolkit.Core.Input;
using Xceed.Wpf.Toolkit.Core.Utilities;
using Xceed.Wpf.Toolkit.Panels;
using Xceed.Wpf.Toolkit.Primitives;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using Xceed.Wpf.Toolkit.PropertyGrid.Commands;
using Xceed.Wpf.Toolkit.PropertyGrid.Converters;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;
using Xceed.Wpf.Toolkit.Zoombox;


namespace WpfApplication2 {
    
    
    /// <summary>
    /// AdittionalOptions
    /// </summary>
    public partial class AdittionalOptions : WpfApplication2.CommonFunctions, System.Windows.Markup.IComponentConnector {
        
        
        #line 24 "..\..\AdittionalOptions.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox SetOpenDns;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\AdittionalOptions.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Documents.Hyperlink h1;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\AdittionalOptions.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox BlockPopUp;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\AdittionalOptions.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox SettingForever;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\AdittionalOptions.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox SettingTime;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\AdittionalOptions.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Xceed.Wpf.Toolkit.ButtonSpinner Hours;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\AdittionalOptions.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Xceed.Wpf.Toolkit.ButtonSpinner Min;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/WpfApplication2;component/adittionaloptions.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\AdittionalOptions.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.SetOpenDns = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 2:
            this.h1 = ((System.Windows.Documents.Hyperlink)(target));
            
            #line 25 "..\..\AdittionalOptions.xaml"
            this.h1.RequestNavigate += new System.Windows.Navigation.RequestNavigateEventHandler(this.h1_RequestNavigate);
            
            #line default
            #line hidden
            return;
            case 3:
            this.BlockPopUp = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 4:
            this.SettingForever = ((System.Windows.Controls.CheckBox)(target));
            
            #line 31 "..\..\AdittionalOptions.xaml"
            this.SettingForever.Checked += new System.Windows.RoutedEventHandler(this.DisableTimeSetting);
            
            #line default
            #line hidden
            
            #line 31 "..\..\AdittionalOptions.xaml"
            this.SettingForever.Unchecked += new System.Windows.RoutedEventHandler(this.EnableTimeSetting);
            
            #line default
            #line hidden
            return;
            case 5:
            this.SettingTime = ((System.Windows.Controls.CheckBox)(target));
            
            #line 32 "..\..\AdittionalOptions.xaml"
            this.SettingTime.Unchecked += new System.Windows.RoutedEventHandler(this.DisableTimeSpinner);
            
            #line default
            #line hidden
            
            #line 32 "..\..\AdittionalOptions.xaml"
            this.SettingTime.Checked += new System.Windows.RoutedEventHandler(this.EnableTimeSpinner);
            
            #line default
            #line hidden
            return;
            case 6:
            this.Hours = ((Xceed.Wpf.Toolkit.ButtonSpinner)(target));
            
            #line 33 "..\..\AdittionalOptions.xaml"
            this.Hours.Spin += new System.EventHandler<Xceed.Wpf.Toolkit.SpinEventArgs>(this.HourButtonSpinner_Spin);
            
            #line default
            #line hidden
            return;
            case 7:
            this.Min = ((Xceed.Wpf.Toolkit.ButtonSpinner)(target));
            
            #line 37 "..\..\AdittionalOptions.xaml"
            this.Min.Spin += new System.EventHandler<Xceed.Wpf.Toolkit.SpinEventArgs>(this.MinButtonSpinner_Spin);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 43 "..\..\AdittionalOptions.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.FinishSetting);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 44 "..\..\AdittionalOptions.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btnClose_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

