﻿

#pragma checksum "C:\Users\Teake\Documents\GitHub\Supermarkt-aanbiedingen-wp8\Supermarkt aanbiedingen\Supermarkt aanbiedingen.WindowsPhone\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "5A8014F4BD5F7FDC08CFC309F5748CE2"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Supermarkt_aanbiedingen
{
    partial class MainPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 65 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.FrameworkElement)(target)).Loaded += this.BLListview_Loaded;
                 #line default
                 #line hidden
                #line 65 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.ListViewBase)(target)).ItemClick += this.BLListview_ItemClick;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 30 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.FrameworkElement)(target)).Loaded += this.PopularSupermarketsLV_Loaded;
                 #line default
                 #line hidden
                #line 30 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.ListViewBase)(target)).ItemClick += this.PopularSupermarketsLV_ItemClick;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 99 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.SettingsButton_Click;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 100 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.SearchButton_Click;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 102 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.PrivacyPolicyButton_Click;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


