﻿using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using InstAd128000.Helpers;
using System.Collections.Generic;
using Instad128000.Core.Helpers.SocialNetworksUsers;
using Instad128000.Core.Common.Interfaces.Services;
using InstAd128000.ViewModels;
using System.Collections.Specialized;
using System.ComponentModel;
using Instad128000.Core.Common.Interfaces;

namespace InstAd128000.Controls.InstagramTabs
{
    /// <summary>
    /// Interaction logic for tAGS.xaml
    /// </summary>
    public partial class CommentByTag : UserControl
    {
        public CommentByTag(IRequestService reqSRV, IDataStringService dataStrSRV)
        {
            InitializeComponent();
            ViewModel.RequestService = reqSRV;
            ViewModel.DataStringService = dataStrSRV;
            UserFactory.Insta.TagsChanged += () => {
                ViewModel.Tags = UserFactory.Insta.TagsToProcess;
            };
            ViewModel.Tags = UserFactory.Insta.TagsToProcess;
            UserFactory.Insta.LocationsChanged += () => {
                ViewModel.Locations = UserFactory.Insta.LocationsToProcess;
            };
            ViewModel.Locations = UserFactory.Insta.LocationsToProcess;
        }
        
        private async void Comment_OnClick(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Tags.Count() == 0)
            {
                MessageBox.Show("Пожалуйста, выберите тэги во вкладке \"Рейтинг тэгов\"");
                return;
            }
            if (ViewModel.EndTime.HasValue)
            {
                var delta = ViewModel.EndTime.Value - DateTime.Now;
                if (delta.Hours < 0 || delta.Minutes <= 0)
                {
                    MessageBox.Show("Пожалуйста, введите валидный промежуток времени");
                    ResetMainWindow();
                    return;
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите валидный промежуток времени");
                ResetMainWindow();
                return;
            }

            IsInProgress(true);
            var result = await UserFactory.Insta.CommentByTagAsync(CommentText.Text, ViewModel.EndTime.Value - DateTime.Now);
            IsInProgress(false);

            ResetMainWindow();
        }

        private void CommentsNumber_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static bool IsTextAllowed(string text)
        {
            var regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }

        private void ResetMainWindow()
        {
            ControlGetter.MainWindow.InstagramTab.ViewModel.IsNoProcessPerformed = true;
            CommentButton.IsEnabled = true;
        }

        protected void IsInProgress(bool isInProgress)
        {
            if (isInProgress)
            {
                UiHelper.InstaBusy(true);
                CommentButton.IsEnabled = false;
            }
            else
            {
                UiHelper.InstaBusy(false);
                CommentButton.IsEnabled = true;
            }
        }
    }
}
