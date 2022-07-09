// Copyright © 2018-2019 The WebView2 Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.


using System.Windows.Input;

namespace WpfApp1.Controls
{
    public static class WebView2Commands
    {
        public static RoutedUICommand Close = new RoutedUICommand("Close", "Close", typeof(WebView2Commands));
        public static RoutedUICommand Exit = new RoutedUICommand("Exit", "Exit", typeof(WebView2Commands));
        /*
        ** CsCallJs1
        */
        public static RoutedUICommand CsCallJs1 = new RoutedUICommand("CsCallJs1", "CsCallJs1", typeof(WebView2Commands));
        /*
        **
        */
        public static RoutedUICommand CsCallJs2 = new RoutedUICommand("CsCallJs2", "CsCallJs2", typeof(WebView2Commands));

        public static RoutedUICommand DevTools = new RoutedUICommand("DevTools", "DevTools", typeof(WebView2Commands));

        public static RoutedUICommand JsCallCs1 = new RoutedUICommand("JsCallCs1", "JsCallCs1", typeof(WebView2Commands));
        public static RoutedUICommand JsCallCs2 = new RoutedUICommand("JsCallCs2", "JsCallCs2", typeof(WebView2Commands));
        public static RoutedUICommand About = new RoutedUICommand("About", "About", typeof(WebView2Commands));
    }
}
