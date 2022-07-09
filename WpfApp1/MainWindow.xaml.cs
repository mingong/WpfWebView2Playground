using System;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Microsoft.Web.WebView2.Core;

using WpfApp1.Controls;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public DotnetInterop interop;

        public JavaScriptInterop jsInterop;

        public MainWindowModel Model { get; set; } = new MainWindowModel();

        public MainWindow()
        {
            InitializeComponent();

            jsInterop = new JavaScriptInterop(webView);
            interop = new DotnetInterop(this, jsInterop);

            // renders in a generic background color - so hide it
            /*
            webView.Visibility = Visibility.Hidden;
            
            */
            DataContext = Model;
            Loaded += MainWindow_Loaded;

            InitializeAsync();

            // WebView initialisation handler, called when control instantiated and ready
            webView.CoreWebView2InitializationCompleted += WebView_CoreWebView2InitializationCompleted;
            webView.Unloaded += (s, a) => webView.Visibility = Visibility.Hidden;

            // Using a file URL
            webView.Source = new Uri(System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, @"wen\editor.html"));

            
            _ = CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, Open));
            _ = CommandBindings.Add(new CommandBinding(ApplicationCommands.Save, Save));

            _ = CommandBindings.Add(new CommandBinding(WebView2Commands.Exit, Exit));
            _ = CommandBindings.Add(new CommandBinding(WebView2Commands.CsCallJs1, CsCallJs1));
            _ = CommandBindings.Add(new CommandBinding(WebView2Commands.CsCallJs2, CsCallJs2));

            _ = CommandBindings.Add(new CommandBinding(WebView2Commands.DevTools, DevTools));

            _ = CommandBindings.Add(new CommandBinding(WebView2Commands.JsCallCs1, JsCallCs1));
            _ = CommandBindings.Add(new CommandBinding(WebView2Commands.JsCallCs2, JsCallCs2));
            _ = CommandBindings.Add(new CommandBinding(WebView2Commands.About, About));
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            /*
            webView.Source = new Uri(Model.Url);
            */
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
            // https://webwiew2.editor/Editor.html
            webView.CoreWebView2.SetVirtualHostNameToFolderMapping(
                    "webview2.editor", "wen",
                    CoreWebView2HostResourceAccessKind.Allow);

            /*
            webView.CoreWebView2.OpenDevToolsWindow();

            */
            webView.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;

            webView.CoreWebView2.AddHostObjectToScript("webview2", interop);
        }

        private void WebView_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                Model.Url = webView.Source.ToString();

                jsInterop.coreWebView2InitializationCompleted = true;
                webView.Visibility = Visibility.Visible;
            }
        }

        private void CoreWebView2_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            string text = e.TryGetWebMessageAsString();

            _ = MessageBox.Show("WpfWebView2Playground" + ":" + " " + text, "WPF Window");
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            interop.Window_Closing(sender, e);
        }

        private void New(object sender, EventArgs e)
        {
            jsInterop.New();
        }

        private void Open(object sender, ExecutedRoutedEventArgs e)
        {
            jsInterop.Open();
        }

        private void Save(object sender, ExecutedRoutedEventArgs e)
        {
            jsInterop.Save();
        }

        private void SaveAs(object sender, ExecutedRoutedEventArgs e)
        {
            jsInterop.SaveAs();
        }

        private void ChooseWorkingDirectory(object sender, ExecutedRoutedEventArgs e)
        {
            jsInterop.ChooseWorkingDirectory();
        }

        private void EsoohcWorkingDirectory(object sender, ExecutedRoutedEventArgs e)
        {
            jsInterop.EsoohcWorkingDirectory();
        }

        private void Shutdown()
        {
            Close();
        }

        private void Exit(object sender, ExecutedRoutedEventArgs e)
        {
            Shutdown();
        }

        private void CsCallJs1(object sender, ExecutedRoutedEventArgs e)
        {
            jsInterop.CsCallJs1();
        }

        private void CsCallJs2(object sender, ExecutedRoutedEventArgs e)
        {
            jsInterop.CsCallJs2();
        }

        private void DevTools(object sender, ExecutedRoutedEventArgs e)
        {
            jsInterop.DevTools();
        }

        private void JsCallCs1(object sender, ExecutedRoutedEventArgs e)
        {
            jsInterop.JsCallCs1();
        }

        private void JsCallCs2(object sender, ExecutedRoutedEventArgs e)
        {
            jsInterop.JsCallCs2();
        }

        private void About(object sender, ExecutedRoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();

            _ = aboutWindow.ShowDialog();

        }
    }

    public class MainWindowModel : INotifyPropertyChanged
    {
        public string Url
        {
            get => url;
            set
            {
                if (value == url)
                {
                    return;
                }

                url = value;
                OnPropertyChanged(nameof(Url));

                return;
            }
        }
        private string url = "about:blank"; //@"C:\Users\Omen\WpfWebView2Playground\WpfApp1\bin\Debug\net472\wen\editor.html";

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
