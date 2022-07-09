---
layout: post

title: Webview2 in Wpf

date: 2015-9-30 16:16

category: blog

tags:

  - C#
  
  - Wpf
  
  - JavaScript
  
slug: webview2-in-wpf
---

# Webview2 in Wpf

### 这是一个看脸的时代

随着移动设备的崛起, Windows 应用已经被逐渐边缘化, 日常生活中大部分事情不用打开电脑了, 
掏出手机就可以完成. 但是Windows 应用终究还是要有的, 对没错, 我今天就是来讲Windows 应用开发的.

最近有做过一个Windows 上的应用, 大致是个编辑器. 既然是做Windows 应用, 那么一定要祭出VS神器,
但是有一个问题: 太丑了.

托互联网蓬勃发展的福, Web 页面越来越漂亮, 传统的Windows 控件由于定制性不强, 在这个看脸的时代,
显然是入不了人们的法眼的. 于是有大把大把的Windows 应用设计都开始往Web 上靠.

由于做过一点前端, 我一开始就琢磨怎么用Web 页面实现这个功能, 这样样式也炒鸡好调, 功能也好加.

我看了[Electron](https://www.electronjs.org/) , Atom的核, 一个基于Web 技术的跨平台桌面应用开发框架.
很好很好, 尝试了一下, 就是打包后的文件有点太大了.

终于发现了今天的主角: WebView2.
WebView2是一个基于Chromium
的集成浏览器框架, 就是为在桌面应用里添加现代浏览器控件而生, 效率很高, 接近Chrome.
[WebView2](https://docs.microsoft.com/en-us/microsoft-edge/webview2/).

WebView2非常易用, 有了他分分钟可以做个Chrome出来. 你可以去它的GitHub页面上翻翻例子.
这里是项目的[说明](https://docs.microsoft.com/en-us/microsoft-edge/webview2/).

### 安装WebView2

WebView2可通过[NuGet](https://www.nuget.org/) 安装, 如果安装了
NuGet , 在`Project > Manage NuGet Packages` 里打开包管理器, 搜索Microsoft.Web.WebView2, 安装`Microsoft.Web.WebView2`,
这样会自动安装依赖, 并且安装完之后自动添加为项目的引用.

### 使用 WebView2

效果图:

![MainWindow](../images/web/wpf.png)

### 加载在线资源

关于页面是个网页, 集成起来十分简单, 安装

```csharp
using System;
using Microsoft.Web.WebView2.Core;

/* ... */

        public AboutWindow()
        {
            InitializeComponent();

            InitializeAsync();

            webView.Source = new Uri("http://localhost:4000");
        }

        async void InitializeAsync()
        {
            if (webView.CoreWebView2 == null)
            {
                // must create a data folder if running out of a secured folder that can't write like Program Files

                CoreWebView2Environment env = await CoreWebView2Environment.CreateAsync(userDataFolder: System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), System.IO.Path.GetFileName(System.Windows.Forms.Application.ExecutablePath) + "." + "WebView2"));
                await webView.EnsureCoreWebView2Async(env);
            }
        }
```

然后效果就是这样, 这可是一个完整的Chromium哦, 妈妈再也不担心我不会做浏览器了:

![About](../images/web/about.png)

### 加载本地资源

如果我们要做一个本地应用, 不能联网, 那么我们会把所有的资源躲放到本地, 然后从本地进行加载.

比如我们使用[Pure.css](https://purecss.io/) 来做一个极简文本编辑器, 我们先写好页面:

```html
<!DOCTYPE html>
<html>
  <head>
    <meta charset="utf-8">
    <link rel="stylesheet" href="./pure-min.css" media="screen" title="no title" charset="utf-8">
    <title>editor</title>
    <style media="screen">
      body, html {
        margin: 0;
        padding: 0;
        height: 100%;
      }

      form {
        height: 100%;
      }

      button {
        height: 40px;
      }

      textarea {
        height: calc(100% - 40px);
        font-size: 18px;
        font-family: "Noto Sans Mono CJK TC", "Noto Sans Mono CJK TC", monospace;
        white-space: pre;
      }
    </style>
  </head>
  <body>
    <form class="pure-form" name="EditorForm">
		<textarea id="editor" class="pure-input-1" aria-label="textarea"></textarea>
		<button type="submit" class="pure-button pure-input-1 pure-button-primary">Save</button>
    </form>
    <script type="text/javascript">
        window.onload = function () {
            document.EditorForm.editor.focus();
        };
        /*
        */
    </script>
  </body>
</html>
```

效果很简单, 就一个文本框, 一个保存按钮:

![Editor](../images/web/editor.png)

添加`wen` 文件夹, 把静态页面都放进去, 记住要在项目属性里选择将这些静态页面拷贝到输出目录, 
不然程序跑起来就找不到这些页面了.

`WebView2`可以加载本地文件, 使用绝对路径:

```csharp
using System;
using Microsoft.Web.WebView2.Core;

/* ... */

        public MainWindow()
        {
            InitializeComponent();

            InitializeAsync();

            // Using a file URL
            webView.Source = new Uri(System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, @"wen\editor.html"));
        }

        async void InitializeAsync()
        {
            if (webView.CoreWebView2 == null)
            {
                // must create a data folder if running out of a secured folder that can't write like Program Files

                CoreWebView2Environment env = await CoreWebView2Environment.CreateAsync(userDataFolder: System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), System.IO.Path.GetFileName(System.Windows.Forms.Application.ExecutablePath) + "." + "WebView2"));
                
                await webView.EnsureCoreWebView2Async(env);
            }
        }
```

### 与JavaScript进行交互

项目里都用了前端写, 如果不能用JavaScript那简直和没用一样啊, 我们来看看与JavaScript的交互, 
包括

**在C#里调用JavaScript代码**

简单的代码可以这样:

```csharp
using Microsoft.Web.WebView2.Core;

/* ... */

        public bool CoreWebView2InitializationCompleted = false;

        private void CsCallJs1(object sender, ExecutedRoutedEventArgs e)
        {
            if (webView != null && CoreWebView2InitializationCompleted)
            {
                _ = webView.ExecuteScriptAsync($"csCallJs1();");
            }
        }
```

如果需要返回值, 不如我们写一个获取高度的函数:

```csharp
using Microsoft.Web.WebView2.Core;

/* ... */

        public bool CoreWebView2InitializationCompleted = false;

        private async void CsCallJs2(object sender, ExecutedRoutedEventArgs e)
        {
            if (webView != null && CoreWebView2InitializationCompleted)
            {
                object result = await webView.ExecuteScriptAsync(@"
(function() {
    var body = document.body, html = document.documentElement;

    return Math.max(body.scrollHeight, body.offsetHeight, html.clientHeight,
                    html.scrollHeight, html.offsetHeight);
})();");

                _ = System.Windows.MessageBox.Show("WpfWebView2Playground" + ":" + " " + "result" + "," + " " + result, "WPF Window");
                
                _ = webView.Focus();
            }
        }
```

不过这里只能返回简单类型的数据, 不能返回自定义的复杂对象, 如果需要复杂类型, 可以返回json串, 然后序列化成C#对象.

**JavaScript里调用C#代码**

这个时候我们需要把C#对象暴露给JavaScript使用:

```csharp
    public class JavaScriptInterop
    {
        public Microsoft.Web.WebView2.Wpf.WebView2 WebView { get; }

        public JavaScriptInterop(Microsoft.Web.WebView2.Wpf.WebView2 webView)
        {
            WebView = webView;
        }

    }

/* ... */

using System.Runtime.InteropServices;

/* ... */

    /// <summary>
    /// Object that gets called from JavaScript
    /// </summary>
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class DotnetInterop
    {
        public JavaScriptInterop JsInterop { get; }

        public string StringProp { get; set; }

        public DotnetInterop(JavaScriptInterop jsInterop)
        {
            JsInterop = jsInterop;

            StringProp = "Hello, WebView2";
        }

        public void ShowHelloWebView2()
        {
            _ = System.Windows.MessageBox.Show("WpfWebView2Playground" + ":" + " " + "Hello, WebView2!!!!", "WPF Window");

            SetFocus();
        }

        public void SetFocus()
        {
            if (JsInterop.WebView != null)
            {
                _ = JsInterop.WebView.Focus();
            }
        }

        public void WpfAlert(string message)
        {
            _ = System.Windows.MessageBox.Show("WpfWebView2Playground" + ":" + " " + message, "WPF Window");
        }
    }

/* ... */

using System;
using Microsoft.Web.WebView2.Core;

/* ... */

        public DotnetInterop interop;

        public JavaScriptInterop jsInterop;

        public MainWindow()
        {
            InitializeComponent();

            jsInterop = new JavaScriptInterop(webView);
            interop = new DotnetInterop(jsInterop);

            InitializeAsync();

            // Using a file URL
            webView.Source = new Uri(System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, @"wen\editor.html"));
        }

        async void InitializeAsync()
        {
            if (webView.CoreWebView2 == null)
            {
                // must create a data folder if running out of a secured folder that can't write like Program Files

                CoreWebView2Environment env = await CoreWebView2Environment.CreateAsync(userDataFolder: System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), System.IO.Path.GetFileName(System.Windows.Forms.Application.ExecutablePath) + "." + "WebView2"));
                
                await webView.EnsureCoreWebView2Async(env);
            }

            webView.CoreWebView2.AddHostObjectToScript("webview2", interop);
        }
```

然后在JavaScript里就可以放肆使用属性和方法了:

```javascript
    <script type="text/javascript">
        async function callCs() {
            // .NET object reference (async)
            var msg = await window.chrome.webview.hostObjects.webview2.stringProp;
            
            /*
            alert(msg);
            
            */
            window.chrome.webview.hostObjects.webview2.wpfAlert(msg);

            window.chrome.webview.hostObjects.webview2.showHelloWebView2();
        }
    </script>
```

注意

为了保证js代码看起来与其他部分风格一致, 这里在js里面调用的时候第一个字母变成了小写.

### DevTools

Chrome/Chromium 相当好用的开发者工具也是可以使用的.

```csharp
using System;
using Microsoft.Web.WebView2.Core;

/* ... */

        public MainWindow()
        {
            InitializeComponent();

            InitializeAsync();

            // Using a file URL
            webView.Source = new Uri(System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, @"wen\editor.html"));
        }

        async void InitializeAsync()
        {
            if (webView.CoreWebView2 == null)
            {
                // must create a data folder if running out of a secured folder that can't write like Program Files

                CoreWebView2Environment env = await CoreWebView2Environment.CreateAsync(userDataFolder: System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), System.IO.Path.GetFileName(System.Windows.Forms.Application.ExecutablePath) + "." + "WebView2"));
                
                await webView.EnsureCoreWebView2Async(env);
            }

            webView.CoreWebView2.OpenDevToolsWindow();
        }
```

实例项目在[-> HERE <-](https://github.com/mingong/WpfWebView2Playground)

保存功能就没有写了.
