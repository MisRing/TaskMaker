﻿#pragma checksum "..\..\MainPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "14B17803204B63C1AC7E80A0F46DC33405130481"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
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
using Task_Maker_v2._0;


namespace Task_Maker_v2._0 {
    
    
    /// <summary>
    /// MainPage
    /// </summary>
    public partial class MainPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button make_task;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button make_test;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button creators;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button FAQ;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button about;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock copyright;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Exit;
        
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
            System.Uri resourceLocater = new System.Uri("/Task Maker v2.0;component/mainpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\MainPage.xaml"
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
            this.make_task = ((System.Windows.Controls.Button)(target));
            
            #line 12 "..\..\MainPage.xaml"
            this.make_task.Click += new System.Windows.RoutedEventHandler(this.OpenTask);
            
            #line default
            #line hidden
            return;
            case 2:
            this.make_test = ((System.Windows.Controls.Button)(target));
            return;
            case 3:
            this.creators = ((System.Windows.Controls.Button)(target));
            
            #line 14 "..\..\MainPage.xaml"
            this.creators.Click += new System.Windows.RoutedEventHandler(this.ShowCreators);
            
            #line default
            #line hidden
            return;
            case 4:
            this.FAQ = ((System.Windows.Controls.Button)(target));
            return;
            case 5:
            this.about = ((System.Windows.Controls.Button)(target));
            
            #line 16 "..\..\MainPage.xaml"
            this.about.Click += new System.Windows.RoutedEventHandler(this.ShowAbout);
            
            #line default
            #line hidden
            return;
            case 6:
            this.copyright = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 7:
            this.Exit = ((System.Windows.Controls.Button)(target));
            
            #line 18 "..\..\MainPage.xaml"
            this.Exit.Click += new System.Windows.RoutedEventHandler(this.WindowClose);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

