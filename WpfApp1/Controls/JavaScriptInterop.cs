/*
using Microsoft.Web.WebView2.Wpf;
*/

namespace WpfApp1.Controls
{
    public class JavaScriptInterop
    {
        public Microsoft.Web.WebView2.Wpf.WebView2 WebView { get; }

        public bool coreWebView2InitializationCompleted = false;

        public JavaScriptInterop(Microsoft.Web.WebView2.Wpf.WebView2 webView)
        {
            WebView = webView;
        }

        public async void New()
        {
            if (WebView != null)
            {
                // "new file"
                _ = await WebView.CoreWebView2.ExecuteScriptAsync($"wen();");
            }
        }

        public async void Open()
        {
            if (WebView != null)
            {
                // "open file"
                _ = await WebView.ExecuteScriptAsync($"open();");
            }
        }


        public void Save(/*string filename*/)
        {
            if (WebView != null && coreWebView2InitializationCompleted)
            {
                // "save file"
                _ = WebView.ExecuteScriptAsync($"save();");
            }
        }

        public void SaveAs()
        {
            if (WebView != null && coreWebView2InitializationCompleted)
            {
                // "save as file"
                _ = WebView.ExecuteScriptAsync($"saveAs();");
            }
        }

        public async void ChooseWorkingDirectory()
        {
            if (WebView != null)
            {
                // "Choose Working Directory"
                _ = await WebView.ExecuteScriptAsync($"set();");
            }
        }

        public async void EsoohcWorkingDirectory()
        {
            if (WebView != null)
            {
                //  Esoohc Working Directory
                _ = await WebView.ExecuteScriptAsync($"tes();");
            }
        }

        public void CsCallJs1()
        {
            if (WebView != null && coreWebView2InitializationCompleted)
            {
                _ = WebView.ExecuteScriptAsync($"csCallJs1();");
            }
        }

        public async void CsCallJs2()
        {
            if (WebView != null && coreWebView2InitializationCompleted)
            {
                object result = await WebView.ExecuteScriptAsync(@"
(function() {
    var body = document.body, html = document.documentElement;
    return Math.max(body.scrollHeight, body.offsetHeight, html.clientHeight,
                    html.scrollHeight, html.offsetHeight);
})();");

                _ = System.Windows.MessageBox.Show("WpfWebView2Playground" + ":" + " " + "result" + "," + " " + result, "WPF Window");

                _ = WebView.Focus();
            }
        }

        public void DevTools()
        {
            if (WebView != null)
            {
                WebView.CoreWebView2.OpenDevToolsWindow();

                _ = WebView.Focus();
            }
        }

        public async void JsCallCs1()
        {
            if (WebView != null)
            {
                _ = await WebView.ExecuteScriptAsync($"callCs1();");
            }
        }

        public void JsCallCs2()
        {
            if (WebView != null)
            {
                _ = WebView.ExecuteScriptAsync($"callCs2();");
            }
        }

        public string Method { get; set; }
        public object[] Parameters = { };
        /*

        public void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (WebView != null && coreWebView2InitializationCompleted)
            {
                e.Cancel = true;

                _ = WebView.ExecuteScriptAsync($"action();");
            }
        }
        */
    }
}
