﻿

#pragma checksum "C:\Users\Teake\Documents\GitHub\Supermarkt-aanbiedingen\Supermarkt aanbiedingen\Supermarkt aanbiedingen.WindowsPhone\SupermarketDiscounts.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F6B495AD40E49A6E4F38A5A89262FEF6"
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
    partial class SupermarketDiscounts : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 36 "..\..\SupermarketDiscounts.xaml"
                ((global::Windows.UI.Xaml.FrameworkElement)(target)).Loaded += this.ProductsLV_Loaded;
                 #line default
                 #line hidden
                #line 36 "..\..\SupermarketDiscounts.xaml"
                ((global::Windows.UI.Xaml.Controls.ListViewBase)(target)).ItemClick += this.ProductsLV_ItemClick;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


