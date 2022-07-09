using System;
using System.Windows;

using Microsoft.Web.WebView2.Core;

namespace WpfApp1
{
    /// <summary>
    /// AboutWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();

            InitializeAsync();

            // Using a Mapped virtual url set with CoreWebView2.SetVirtualHostNameToFolderMapping
            /*
           
            */
            // Using a file URL
            webView.Source = new Uri(System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, @"wen\_book\index.html"));
        }

        private async void InitializeAsync()
        {
            if (webView.CoreWebView2 == null)
            {
                // must create a data folder if running out of a secured folder that can't write like Program Files

                CoreWebView2Environment env = await CoreWebView2Environment.CreateAsync(userDataFolder: System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), System.IO.Path.GetFileName(System.Windows.Forms.Application.ExecutablePath) + "." + "WebView2"));
                await webView.EnsureCoreWebView2Async(env);
            }

            // Map a folder from the Executable Folder to a virtual domain
            // https://webwiew2.editor/index.html
            webView.CoreWebView2.SetVirtualHostNameToFolderMapping(
                    "webview2.editor", @"wen\_book",
                    CoreWebView2HostResourceAccessKind.Allow);



        }
    }
}
