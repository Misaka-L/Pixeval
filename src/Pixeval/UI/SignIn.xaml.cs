// Pixeval - A Strong, Fast and Flexible Pixiv Client
//  Copyright (C) 2019-2020 Dylech30th
// This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as
//  published by the Free Software Foundation, either version 3 of the
//  License, or (at your option) any later version.

using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using EmbedIO;
using Microsoft.Web.WebView2.Core;
using Pixeval.Data.Web.Delegation;
using Pixeval.Objects.Exceptions;
using Pixeval.Objects.Exceptions.Logger;
using Pixeval.Objects.I18n;
using Pixeval.Objects.Primitive;
using Pixeval.Objects.Web;
using Pixeval.Persisting;
using Pixeval.Persisting.WebApi;
using Refit;

namespace Pixeval.UI
{
    public partial class SignIn
    {
        public SignIn()
        {
            InitializeComponent();
        }

        private async void Login_OnClick(object sender, RoutedEventArgs e)
        {
            if (Email.Text.IsNullOrEmpty() || Password.Password.IsNullOrEmpty())
            {
                ErrorMessage.Text = AkaI18N.EmptyEmailOrPasswordIsNotAllowed;
                return;
            }

            Login.Disable();

            try
            {
                await Task.WhenAll(Authentication.AppApiAuthenticate(Email.Text, Password.Password), WebApiAuthenticate(Email.Text, Password.Password));
            }
            catch (Exception exception)
            {
                SetErrorHint(exception);
                ExceptionDumper.WriteException(exception);
                Login.Enable();
                return;
            }

            var mainWindow = new MainWindow();
            mainWindow.Show();

            Close();
        }

        private async void SignIn_OnInitialized(object sender, EventArgs e)
        {
            if (Session.ConfExists())
            {
                try
                {
                    DialogHost.OpenControl();
                    await RefreshIfRequired();
                }
                catch (Exception exception)
                {
                    SetErrorHint(exception);
                    ExceptionDumper.WriteException(exception);
                    DialogHost.CurrentSession.Close();
                    return;
                }

                DialogHost.CurrentSession.Close();

                var mainWindow = new MainWindow();
                mainWindow.Show();
                Close();
            }
        }

        private async void SetErrorHint(Exception exception)
        {
            ErrorMessage.Text = exception is ApiException aException && await IsPasswordOrAccountError(aException) ? AkaI18N.EmailOrPasswordIsWrong : exception.Message;
        }

        private static async ValueTask<bool> IsPasswordOrAccountError(ApiException exception)
        {
            var eMess = await exception.GetContentAsAsync<dynamic>();
            return eMess.errors.system.code == 1508;
        }

        /// <summary>
        ///     Refresh the session if required, it is composed of two sections, app-API login
        ///     and web-API login, each section has its own expiration check, and both two sections
        ///     are invoked separately, we will only refresh the required section
        /// </summary>
        /// <returns></returns>
        public async Task RefreshIfRequired()
        {
            if (Session.Current == null) await Session.Restore();

            // refresh through app-API
            async Task RefreshAppApi()
            {
                if (Session.AppApiRefreshRequired(Session.Current))
                {
                    // we'd prefer use refresh token
                    if (Session.Current?.RefreshToken.IsNullOrEmpty() is true)
                        await Authentication.AppApiAuthenticate(Session.Current?.MailAddress, Session.Current?.Password);
                    else
                        await Authentication.AppApiAuthenticate(Session.Current?.RefreshToken);
                }
            }

            // refresh through web-API
            async Task RefreshWebApi()
            {
                if (Session.WebApiRefreshRequired(Session.Current)) await WebApiAuthenticate(Session.Current?.MailAddress, Session.Current?.Password);
            }

            // wait for both two sections to be complete
            await Task.WhenAll(RefreshAppApi(), RefreshWebApi());
        }

        /// <summary>
        ///     Authentication process to pixiv web api, which is driven by
        ///     <a href="https://github.com/cefsharp/CefSharp">CefSharp</a>
        ///     This method is for login usage only, USE AT YOUR OWN RISK
        /// </summary>
        /// <param name="name">user name</param>
        /// <param name="pwd">user password</param>
        /// <returns></returns>
        public async Task WebApiAuthenticate(string name, string pwd)
        {
            //// create x.509 certificate object for intercepting https traffic, USE AT YOUR OWN RISK
            //var certificate = await CertificateManager.GetFakeServerCertificate();
            //// create https proxy server for intercepting and forwarding https traffic
            //using var proxyServer = HttpsProxyServer.Create("127.0.0.1", AppContext.ProxyPort, (await new PixivApiDnsResolver().Lookup("pixiv.net"))[0].ToString(), certificate);
            //// create pac file server for providing the proxy-auto-configuration file,
            //// which is driven by EmbedIO, this is because CefSharp do not accept file uri
            //using var pacServer = PacFileServer.Create("127.0.0.1", AppContext.PacPort);
            //pacServer.Start();
            const string loginUrl = "https://accounts.pixiv.net/login";
            WebView2.CoreWebView2.Navigate(loginUrl);
            WebView2.CoreWebView2.NavigationCompleted += (sender, e) =>
            {
                // when the login page is loaded, we will execute the following js snippet
                // which is going to fill and submit the form
                if (e.IsSuccess)
                    // ReSharper disable once AccessToDisposedClosure
                    WebView2.CoreWebView2.ExecuteScriptAsync($@"
                                var container_login = document.getElementById('container-login');
                                var fields = container_login.getElementsByClassName('input-field');
                                var account = fields[0].getElementsByTagName('input')[0];
                                var password = fields[1].getElementsByTagName('input')[0];
                                account.value = '{name}';
                                password.value = '{pwd}';
                                document.getElementById('container-login').getElementsByClassName('signup-form__submit')[0].click();
                        ");
            };
            var cancellationTokenSource = new CancellationTokenSource();
            await Task.Run(async () =>
            {
                while (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    await Task.Delay(1000, cancellationTokenSource.Token);
                    var src = await WebView2.ExecuteScriptAsync("document.body.outerHTML");
                    if (src.Contains("error-msg-list__item"))
                    {
                        cancellationTokenSource.Cancel();
                        Dispatcher.Invoke(() =>
                        {
                            MessageBox.Show(AkaI18N.ThisLoginSessionRequiresRecaptcha);
                            BrowserDialog.IsOpen = true;
                        });
                    }
                }
            }, cancellationTokenSource.Token);

            // polling to check we have got the correct Cookie, which names PHPSESSID and
            // has a form like "numbers_hash"
            static bool PixivCookieRecorded(Cookie cookie)
            {
                return Regex.IsMatch(cookie.Domain, @".*\.pixiv\.net") && cookie.Name == "PHPSESSID" && Regex.IsMatch(cookie.Value, @"\d+_.*");
            }

            // create an asynchronous polling task while the authenticate process is running,
            // it will check the Cookie
            await chrome.AwaitAsync(async c =>
            {
                var visitor = new TaskCookieVisitor();
                Cef.GetGlobalCookieManager().VisitAllCookies(visitor);
                return (await visitor.Task).Any(PixivCookieRecorded);
            }, 500, TimeSpan.FromMinutes(3));

            // check if we have got the Cookie when the time limit is exceeded, return successfully
            // if it does, otherwise throw an exception
            var completionVisitor = new TaskCookieVisitor();
            Cef.GetGlobalCookieManager().VisitAllCookies(completionVisitor);

            cancellationTokenSource.Cancel();
            BrowserDialog.CurrentSession?.Close();
            // finalizing objects and save the cookie to user identity
            if ((await completionVisitor.Task).FirstOrDefault(PixivCookieRecorded) is { } cookie)
            {
                Session.Current ??= new Session();
                Session.Current.PhpSessionId = cookie.Value;
                Session.Current.CookieCreation = cookie.Creation.ToLocalTime();
                return;
            }

            throw new AuthenticateFailedException(AkaI18N.WebApiAuthenticateTimeout);
        }
    }
}