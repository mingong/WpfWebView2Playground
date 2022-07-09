using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;
using System.Text;
/*
"工程需添加引用, 程序集->框架->System.Web"
*/

using System;
using System.Windows;

namespace WpfApp1.Controls
{
    /// Object that gets called from JavaScript
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class DotnetInterop
    {
        private string fileName = CONSTANTS.FAKE_NAME;
        private string workingFolderName = CONSTANTS.FAKE_PATH;

        private bool shouldSave = false;
        /*
        private bool isModified = false;
        */

        private bool shouldClose = false;

        public JavaScriptInterop JsInterop { get; }

        public MainWindow Window { get; set; }

        public string StringProp { get; set; }

        public DotnetInterop(MainWindow mainWindow, JavaScriptInterop jsInterop)
        {
            JsInterop = jsInterop;
            Window = mainWindow;

            StringProp = "Hello, WebView2";

            Window.Title = fileName;
        }

        public string ChooseWorkingDirectory()
        {
            string path;

            if (string.Equals(fileName.ToLower(), CONSTANTS.FAKE_NAME, StringComparison.Ordinal))
            {
                Ookii.Dialogs.Wpf.VistaFolderBrowserDialog workingFolderBrowserDialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();

                if (string.Equals(workingFolderName.ToLower(), CONSTANTS.FAKE_PATH.ToLower(), StringComparison.Ordinal))
                {
                    workingFolderBrowserDialog.RootFolder = Environment.SpecialFolder.Desktop;
                }
                else
                {
                    workingFolderBrowserDialog.SelectedPath = workingFolderName;
                }

                /*
                ** "调用ShowDialog()方法显示该对话框, 该方法的返回值代表用户是否点击了确定按钮"
                */
                bool? workingDialogResult = workingFolderBrowserDialog.ShowDialog();

                if (workingDialogResult == true)
                {
                    workingFolderName = workingFolderBrowserDialog.SelectedPath.Trim();

                    path = workingFolderName;
                }
                else
                {
                    path = CONSTANTS.NULL_TEXT;
                }
            }
            else
            {
                _ = System.Windows.MessageBox.Show("Can Not Choose Working Directory.", "WPF Window");

                path = CONSTANTS.NULL_TEXT;
            }

            return path;
        }

        public string EsoohcWorkingDirectory()
        {
            string path;

            if (string.Equals(fileName.ToLower(), CONSTANTS.FAKE_NAME, StringComparison.Ordinal))
            {
                if (string.Equals(workingFolderName.ToLower(), CONSTANTS.FAKE_PATH.ToLower(), StringComparison.Ordinal))
                {
                    _ = System.Windows.MessageBox.Show("Do Not Need To Esoohc Working Directory.", "WPF Window");

                    path = CONSTANTS.NULL_TEXT;
                }
                else
                {
                    workingFolderName = CONSTANTS.FAKE_PATH;

                    _ = System.Windows.MessageBox.Show("Esoohc Working Directory.", "WPF Window");

                    /*
                    path = AppDomain.CurrentDomain.BaseDirectory;
                    */
                    path = System.Windows.Forms.Application.StartupPath;
                }
            }
            else
            {
                _ = System.Windows.MessageBox.Show("Can Not Esoohc Working Directory.", "WPF Window");

                path = CONSTANTS.NULL_TEXT;
            }

            return path;
        }

        public void Wen()
        {
            fileName = CONSTANTS.FAKE_NAME;

            workingFolderName = CONSTANTS.FAKE_PATH;

            Window.Title = fileName;
            /*
            
            isModified = false;
            */
        }

        public OpenMessage Open()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                // 
                // "openFileDialog"
                // 

                Filter = "Text Files (*.txt)|*.txt",
                /*
                Title = "Open File",
                ValidateNames = false
                */
                Title = "Open File"
            };

            string path;
            string content;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                fileName = openFileDialog.FileName;

                workingFolderName = Path.GetDirectoryName(fileName);

                path = workingFolderName;

                content = File.ReadAllText(fileName, Encoding.UTF8);

                fileName = Path.GetFileName(fileName);

                Window.Title = fileName;

                /*
                SetFocus();

                isModified = false;
                */
            }
            else
            {
                path = CONSTANTS.NULL_TEXT;
                content = CONSTANTS.NULL_TEXT;
            }

            return new OpenMessage() { Path = path, Content = content };
        }

        public string SaveText(/*string filename*/)
        {
            if (string.Equals(fileName.ToLower(), CONSTANTS.FAKE_NAME, StringComparison.Ordinal))
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    /*
                    Title = "Save",
                    */
                    DefaultExt = "txt",
                    Filter = "Text files|*.txt",
                    /*
                    ** "是否覆盖当前文件"
                    */
                    OverwritePrompt = true,
                    /*
                    ** "还原目录"
                    */
                    /*
                    RestoreDirectory = true,
                    */
                    // "设置文件名"
                    /*
                    FileName = filename
                    */
                    /*
                    ** "还原目录"
                    */
                    RestoreDirectory = true
                };

                if (string.Equals(workingFolderName.ToLower(), CONSTANTS.FAKE_PATH.ToLower(), StringComparison.Ordinal))
                {

                }
                else
                {
                    saveDialog.InitialDirectory = workingFolderName;
                }

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    fileName = saveDialog.FileName;

                    workingFolderName = Path.GetDirectoryName(fileName);
                    fileName = Path.GetFileName(fileName);

                    shouldSave = true;
                }
                else
                {
                    shouldSave = false;
                }
            }
            else
            {
                shouldSave = true;
            }

            return SavePath();
        }

        public void Save_(string content)
        {
            string fullPath;

            fullPath = Path.Combine(workingFolderName, fileName);

            TextWriter wr = new StreamWriter(fullPath);

            wr.Write(content);

            wr.Close();
            /*
            _ = System.Windows.MessageBox.Show("WpfWebView2Playground" + ":" + " " + "content" + "," + " " + "content", "WPF Window");
            */

            // @todo
            /*
            SetFocus();
            */
            Window.Title = fileName;

            /*
            isModified = false;
            */
            shouldSave = false;
        }

        public string SaveTextAs(/*string filename*/)
        {
            SaveFileDialog saveDialog = new SaveFileDialog
            {
                /*
                Title = "Save As",
                */
                DefaultExt = "txt",
                Filter = "Text files|*.txt",
                /*
                ** "是否覆盖当前文件"
                */
                OverwritePrompt = true,
                /*
                ** "还原目录"
                */
                /*
                RestoreDirectory = true,
                */
                // "设置文件名"
                /*
                FileName = filename
                */
                /*
                ** 还原目录
                */
                RestoreDirectory = true
            };

            if (string.Equals(workingFolderName.ToLower(), CONSTANTS.FAKE_PATH.ToLower(), StringComparison.Ordinal))
            {

            }
            else
            {
                saveDialog.InitialDirectory = workingFolderName;
            }

            /*
            string fullPath;

            */
            if (string.Equals(fileName.ToLower(), CONSTANTS.FAKE_NAME, StringComparison.Ordinal))
            {
                /*
                fullPath = workingFolderName;
                */
            }
            else
            {
                /*
                fullPath = Path.Combine(workingFolderName, fileName);

                saveDialog.FileName = fullPath;
                */
                saveDialog.FileName = fileName;
            }

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                fileName = saveDialog.FileName;

                workingFolderName = Path.GetDirectoryName(fileName);
                fileName = Path.GetFileName(fileName);

                shouldSave = true;
            }
            else
            {
                shouldSave = false;
            }

            return SavePath();
        }

        private string SavePath()
        {
            return shouldSave ? workingFolderName : CONSTANTS.NULL_TEXT;
        }

        public void ShowHelloWebView2()
        {
            _ = System.Windows.MessageBox.Show("WpfWebView2Playground" + ":" + " " + "Hello, WebView2!!!!", "WPF Window");

            SetFocus();
        }

        private void Shutdown()
        {
            Window.Close();
        }

        public void Exit()
        {
            shouldClose = true;

            Shutdown();
        }

        public void UpdateTitle()
        {
            Window.Title = fileName + " " + "*";
            /*

            isModified = true;
            */
        }

        public void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (JsInterop.WebView != null && JsInterop.coreWebView2InitializationCompleted)
            {
                if (!shouldClose/* && isModified*/)
                {
                    e.Cancel = true;

                    _ = JsInterop.WebView.ExecuteScriptAsync($"action();");
                }
            }
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

        public string WpfConfirm(string message)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show(message, "WPF Window", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.Cancel)
            {
                return CONSTANTS.NULL_TEXT;
            }
            else
            {
                return "Confirm";
            }
        }
    }

    [DebuggerDisplay("JsonCallback: { Method }")]
    public class JsonCallbackObject
    {
        public string Method { get; set; }

        public List<object> Parameters { get; } = new List<object>();
    }

    internal static class CONSTANTS
    {
        public const string FAKE_PATH = @"C:\fakepath";
        public const string FAKE_NAME = "untitled.txt";
        public const string NULL_TEXT = "";
    }

    public class OpenMessage
    {
        public string Path { get; set; } = CONSTANTS.NULL_TEXT;
        public string Content { get; set; } = CONSTANTS.NULL_TEXT;
    }
}
