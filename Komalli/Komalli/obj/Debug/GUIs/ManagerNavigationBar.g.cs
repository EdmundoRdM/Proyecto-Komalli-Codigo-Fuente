﻿#pragma checksum "..\..\..\GUIs\ManagerNavigationBar.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "25778ADB14ECE0074F20DD39076C7CA38130A5C8A54D1A3903DB44519AE3DBA4"
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

using Komalli.GUIs;
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


namespace Komalli.GUIs {
    
    
    /// <summary>
    /// ManagerNavigationBar
    /// </summary>
    public partial class ManagerNavigationBar : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\..\GUIs\ManagerNavigationBar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid Background;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\GUIs\ManagerNavigationBar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnReports;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\..\GUIs\ManagerNavigationBar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnProducts;
        
        #line default
        #line hidden
        
        
        #line 97 "..\..\..\GUIs\ManagerNavigationBar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnStaff;
        
        #line default
        #line hidden
        
        
        #line 126 "..\..\..\GUIs\ManagerNavigationBar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnExtraordinaryMovements;
        
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
            System.Uri resourceLocater = new System.Uri("/Komalli;component/guis/managernavigationbar.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\GUIs\ManagerNavigationBar.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
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
            this.Background = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.btnReports = ((System.Windows.Controls.Button)(target));
            
            #line 51 "..\..\..\GUIs\ManagerNavigationBar.xaml"
            this.btnReports.Click += new System.Windows.RoutedEventHandler(this.BtnReportsModule_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btnProducts = ((System.Windows.Controls.Button)(target));
            
            #line 80 "..\..\..\GUIs\ManagerNavigationBar.xaml"
            this.btnProducts.Click += new System.Windows.RoutedEventHandler(this.BtnProductsModule_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnStaff = ((System.Windows.Controls.Button)(target));
            
            #line 109 "..\..\..\GUIs\ManagerNavigationBar.xaml"
            this.btnStaff.Click += new System.Windows.RoutedEventHandler(this.BtnStaffModule_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnExtraordinaryMovements = ((System.Windows.Controls.Button)(target));
            
            #line 138 "..\..\..\GUIs\ManagerNavigationBar.xaml"
            this.btnExtraordinaryMovements.Click += new System.Windows.RoutedEventHandler(this.BtnExtraordinaryMovementsModule_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 161 "..\..\..\GUIs\ManagerNavigationBar.xaml"
            ((System.Windows.Controls.Image)(target)).MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.LogOut_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

