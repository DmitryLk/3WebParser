using System;
using System.Threading.Tasks;

using HtmlAgilityPack;
using CefSharp;
using CefSharp.OffScreen;
using System.Threading;
using CefSharp.Internals;

namespace WebParser.Data
{
    class HtmlByUriChromiumGetter : IHtmlByUriGetter
    {
        public async Task<HtmlDocument> GetHtml(string uri)
        {
            string html;
            //CefSettings settings = new CefSettings();
            //settings.CefCommandLineArgs.Add("proxy-server", "37.75.9.131:8080");
            //settings.UserAgent = "Mozila 5.0";
            //Cef.Initialize(settings);

            using (var chromium = new ChromiumWebBrowser(string.Empty))
            {
                Thread.Sleep(1000);
                await LoadPageAsync(chromium, uri);
                html = await chromium.GetSourceAsync();
            }
            //Cef.Shutdown();

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);
            return document;
        }


        public static Task LoadPageAsync(IWebBrowser browser, string address = null)
        {
            var tcs = new TaskCompletionSource<bool>();
            EventHandler<LoadingStateChangedEventArgs> handler = null;
            handler = (sender, args) =>
            {
                if (!args.IsLoading)
                {
                    browser.LoadingStateChanged -= handler;
                    tcs.TrySetResultAsync(true);
                }
            };
            browser.LoadingStateChanged += handler;
            if (!string.IsNullOrEmpty(address))
            {
                browser.Load(address);
            }
            return tcs.Task;
        }


    }


}
