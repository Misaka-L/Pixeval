// Pixeval - A Strong, Fast and Flexible Pixiv Client
//  Copyright (C) 2019-2020 Dylech30th
// This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as
//  published by the Free Software Foundation, either version 3 of the
//  License, or (at your option) any later version.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Transitions;
using Microsoft.WindowsAPICodePack.Dialogs;
using Pixeval.Core;
using Pixeval.Data.ViewModel;
using Pixeval.Data.Web.Delegation;
using Pixeval.Data.Web.Request;
using Pixeval.Objects.Generic;
using Pixeval.Objects.I18n;
using Pixeval.Objects.Native;
using Pixeval.Objects.Primitive;
using Pixeval.Persisting;
using Pixeval.UI.UserControls;
using Refit;
using Xceed.Wpf.AvalonDock.Controls;
using static Pixeval.Objects.Primitive.UiHelper;

#if RELEASE
using System.Net.Http;
using Pixeval.Objects.Exceptions;
using Pixeval.Objects.Exceptions.Logger;

#endif

namespace Pixeval.UI
{
    public partial class MainWindow
    {
        public static MainWindow Instance;

        public static readonly SnackbarMessageQueue MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2)) {IgnoreDuplicate = true};

        public MainWindow()
        {
            Instance = this;
            InitializeComponent();
            NavigatorList.SelectedItem = MenuTab;
            MainWindowSnackBar.MessageQueue = MessageQueue;

            if (Dispatcher != null) Dispatcher.UnhandledException += Dispatcher_UnhandledException;

#pragma warning disable 4014
            AcquireRecommendUser();
#pragma warning restore 4014
        }

        private static void Dispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
#if RELEASE
            switch (e.Exception)
            {
                case QueryNotRespondingException _:
                    MessageQueue.Enqueue(AkaI18N.QueryNotResponding);
                    break;
                case ApiException apiException:
                    if (apiException.StatusCode == HttpStatusCode.BadRequest)
                        MessageQueue.Enqueue(AkaI18N.QueryNotResponding);
                    break;
                case HttpRequestException _: break;
                default:
                    ExceptionDumper.WriteException(e.Exception);
                    break;
            }

            e.Handled = true;
#endif
        }

        private void DoQueryButton_OnClick(object sender, RoutedEventArgs e)
        {
            CloseControls(TrendingTagPopup, AutoCompletionPopup);

            if (KeywordTextBox.Text.IsNullOrEmpty())
            {
                MessageQueue.Enqueue(AkaI18N.InputIsEmpty);
                return;
            }

            var keyword = KeywordTextBox.Text;
            if (QuerySingleArtistToggleButton.IsChecked == true)
                ShowArtist(keyword);
            else if (QueryArtistToggleButton.IsChecked == true)
                TryQueryUser(keyword);
            else if (QuerySingleWorkToggleButton.IsChecked == true)
                TryQuerySingle(keyword);
            else
                QueryWorks(keyword);
        }

        private async void ShowArtist(string userId)
        {
            if (!userId.IsNumber())
            {
                MessageQueue.Enqueue(AkaI18N.UserIdIllegal);
                return;
            }

            try
            {
                await HttpClientFactory.AppApiService().GetUserInformation(new UserInformationRequest {Id = userId});
            }
            catch (ApiException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    MessageQueue.Enqueue(AkaI18N.CannotFindUser);
                    return;
                }
            }

            OpenUserBrowser();
            SetUserBrowserContext(new User {Id = userId});
        }

        private void TryQueryUser(string keyword)
        {
            QueryStartUp();
            SearchingHistoryManager.EnqueueSearchHistory(keyword);
            PixivHelper.Enumerate(new UserPreviewAsyncEnumerable(keyword), NewItemsSource<User>(UserPreviewListView));
        }

        private async void TryQuerySingle(string illustId)
        {
            if (!int.TryParse(illustId, out _))
            {
                MessageQueue.Enqueue(AkaI18N.IdIllegal);
                return;
            }

            try
            {
                OpenIllustBrowser(await PixivHelper.IllustrationInfo(illustId));
            }
            catch (ApiException exception)
            {
                if (exception.StatusCode == HttpStatusCode.NotFound || exception.StatusCode == HttpStatusCode.BadRequest)
                    MessageQueue.Enqueue(AkaI18N.IdDoNotExists);
                else
                    throw;
            }
        }

        private void QueryWorks(string keyword)
        {
            QueryStartUp();
            SearchingHistoryManager.EnqueueSearchHistory(keyword);
            PixivHelper.Enumerate(Settings.Global.SortOnInserting ? (AbstractQueryAsyncEnumerable) new PopularityQueryAsyncEnumerable(keyword, Settings.Global.TagMatchOption, Session.Current.IsPremium, Settings.Global.QueryStart) : new PublishDateQueryAsyncEnumerable(keyword, Settings.Global.TagMatchOption, Session.Current.IsPremium, Settings.Global.QueryStart), NewItemsSource<Illustration>(ImageListView), Settings.Global.QueryPages);
        }

        private void IllustrationContainer_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OpenIllustBrowser(sender.GetDataContext<Illustration>());
            e.Handled = true;
        }

        private void IllustrationContainer_OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            await AddUserNameAndAvatar();
        }

        private async Task AddUserNameAndAvatar()
        {
            if (!Session.Current.AvatarUrl.IsNullOrEmpty() && !Session.Current.Name.IsNullOrEmpty())
            {
                UserName.Text = Session.Current.Name;
                UserAvatar.Source = await PixivIO.FromUrl(Session.Current.AvatarUrl);
            }
        }

        private void PixevalSettingDialog_OnDialogClosing(object sender, DialogClosingEventArgs e)
        {
            SettingsTab.IsSelected = false;
        }

        private void DownloadQueueDialogHost_OnDialogClosing(object sender, DialogClosingEventArgs e)
        {
            DownloadListTab.IsSelected = false;
        }

        

        

        

        

        

        

        

        

        

        
    }
}