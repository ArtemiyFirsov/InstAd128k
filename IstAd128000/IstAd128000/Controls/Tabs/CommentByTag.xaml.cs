﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using InstAd128000.Helpers;

namespace InstAd128000.Controls.Tabs
{
    /// <summary>
    /// Interaction logic for tAGS.xaml
    /// </summary>
    public partial class CommentByTag : UserControl
    {
        public CommentByTag()
        {
            InitializeComponent();
        }

        private async void Comment_OnClick(object sender, RoutedEventArgs e)
        {
            ControlGetter.MainWindow.IsNoProcessPerformed = false;
            SpinnerInstance.SetToMainWindow();
            CommentButton.IsEnabled = false;

            if (string.IsNullOrWhiteSpace(CommentTag.Text))
            {
                CommentTag.Text = "Please, enter valid text";
                CommentTag.Foreground = Brushes.Red;
                return;
            }


            int commentsNumber = Convert.ToInt32(CommentsNumber.Text);
            //todo: добавить поиск последнего id в бд для пагинации ептэ
            string lastId = "0";

            var result = await ControlGetter.MainWindow.User.CommentByTag(CommentTag.Text, CommentText.Text, commentsNumber, lastId, Convert.ToBoolean(ShouldLikeCheckBox.IsChecked));
            CommentedPostsCount.Text = result.Count.ToString();

            ControlGetter.MainWindow.IsNoProcessPerformed = true;
            SpinnerInstance.RemoveFromMainWindow();
            CommentButton.IsEnabled = true;
        }
    }
}
