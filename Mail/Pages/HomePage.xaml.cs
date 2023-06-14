﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using CommunityToolkit.Authentication;
using Mail.Extensions;
using Mail.Services;
using Mail.Services.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using NavigationView = Microsoft.UI.Xaml.Controls.NavigationView;

namespace Mail.Pages
{
    public sealed partial class HomePage : Page
    {
        private readonly ObservableCollection<AccountModel> AccountSource = new();

        private readonly ObservableCollection<object> MailFolderSource = new();
        private readonly IMailService Service;

        public HomePage()
        {
            InitializeComponent();

            SetupTitleBar();

            SetupPaneToggleButton();

            //TODO: Test only and should remove this later
            try
            {
                var service = App.Services.GetService<OutlookService>()!;
                Service = service;
                var Provider = service.Provider as MsalProvider;
                var model = new AccountModel(
                    Provider.Account.GetTenantProfiles().First().ClaimsPrincipal.FindFirst("name").Value,
                    Provider.Account.Username,
                    service.MailType);
                AccountSource.Add(model);

                service.CurrentAccount = model;
            }
            catch
            {
                throw;
            }

            Loaded += HomePage_Loaded;
            MailFolderSource.CollectionChanged +=
                ObservableCollectionExtension<MailFolderData>.CollectionChanged_DbHandle;
        }

        private void SetupTitleBar()
        {
            CoreApplicationViewTitleBar SystemBar = CoreApplication.GetCurrentView().TitleBar;
            SystemBar.LayoutMetricsChanged += SystemBar_LayoutMetricsChanged;
            SystemBar.IsVisibleChanged += SystemBar_IsVisibleChanged;
            Window.Current.SetTitleBar(AppTitleBar);
        }

        private void SetupPaneToggleButton()
        {
            PaneToggleButton.ApplyTemplate();
            var content = PaneToggleButton.FindChildOfName<ContentPresenter>("ContentPresenter");
            content.Visibility = Visibility.Collapsed;
        }

        private void HomePage_Loaded(object sender, RoutedEventArgs e)
        {
            if (NavView.SettingsItem is NavigationViewItem SettingItem)
            {
                SettingItem.SelectsOnInvoked = false;
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode != NavigationMode.New) return;
            // 这是分割线
            MailFolderSource.Add(0);
            var isFirstOther = true;
            try
            {
                // the specified folder could not be found in the store
                await foreach (var item in Service.GetMailSuperFoldersAsync(NavigationMode.New)
                                   .OrderBy(item => item.Type))
                {
                    if (item.Type == MailFolderType.Other && isFirstOther)
                    {
                        // 这是分割线
                        MailFolderSource.Add(1);
                        isFirstOther = false;
                    }

                    MailFolderSource.Add(item);
                }
            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception);
            }

            NavView.SelectedItem = MailFolderSource.FirstOrDefault(item => item is MailFolderData);
        }

        private void SystemBar_IsVisibleChanged(CoreApplicationViewTitleBar sender, object args)
        {
            if (sender.IsVisible)
            {
                Window.Current.SetTitleBar(AppTitleBar);
                AppTitleBar.Visibility = Visibility.Visible;
            }
            else
            {
                Window.Current.SetTitleBar(null);
                AppTitleBar.Visibility = Visibility.Collapsed;
            }
        }

        private void SystemBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            AppTitleBar.Height = sender.Height;
        }

        private void NavView_SelectionChanged(NavigationView sender,
            Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                NavigationContent.Navigate(typeof(SettingsPage), null, new DrillInNavigationTransitionInfo());
            }
            else if (args.SelectedItem is MailFolderData data)
            {
                NavigationContent.Navigate(typeof(MailFolderDetailsPage), data, new DrillInNavigationTransitionInfo());
            }
        }

        private void PaneToggleButton_Click(object sender, RoutedEventArgs e)
        {
            NavView.IsPaneOpen = !NavView.IsPaneOpen;
        }
    }

    class FolderIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is MailFolderType folderType)
            {
                return folderType switch
                {
                    MailFolderType.Inbox => "\uE10F",
                    MailFolderType.Deleted => "\uE107",
                    MailFolderType.Drafts => "\uEC87",
                    MailFolderType.SentItems => "\uE122",
                    MailFolderType.Junk => "\uE107",
                    MailFolderType.Archive => "\uE7B8",
                    MailFolderType.Other => "\uE8B7",
                    _ => "\uE8B7"
                };
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    class MailFolderNavigationDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Divider { get; set; }

        public DataTemplate Content { get; set; }

        public DataTemplate ContentWithChild { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (item is MailFolderData folder)
            {
                if (folder.ChildFolders != null) return ContentWithChild;

                return Content;
            }
            else
            {
                return Divider;
            }
        }
    }
}