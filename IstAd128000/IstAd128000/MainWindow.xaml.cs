﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using Instad128000.Core;
using Instad128000.Core.Common.Interfaces.Services;
using Instad128000.Core.Helpers.Selenium;
using Instad128000.Core.Helpers.SocialNetworksUsers;
using InstAd128000.Properties;
using InstAd128000.Services;
using Microsoft.Practices.Unity;
using Application = System.Windows.Application;
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.MessageBox;
using UserControl = System.Windows.Controls.UserControl;

namespace InstAd128000
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            Width = Convert.ToDouble(Settings.Default.DefaultWidth);
            Height = Convert.ToDouble(Settings.Default.DefaultHeight);
            _noProcessPerformed = true;
            _controlsList = new Dictionary<string, UserControl>();
        }

        private bool _loggedIn;
        private bool _noProcessPerformed;
        public bool IsLogged
        {
            get { return _loggedIn; }
            set
            {
                _loggedIn = value;
                NotifyPropertyChanged("IsLogged");
            }
        }
        public bool IsNoProcessPerformed
        {
            get { return _noProcessPerformed; }
            set
            {
                _noProcessPerformed = value;
                NotifyPropertyChanged("IsNoProcessPerformed");
            }
        }
        public bool IsUiFreeForUser
        {
            get { return IsLogged && !IsNoProcessPerformed; }
        }

        [Dependency]
        public IRequestService RequestService { get; set; }
        [Dependency]
        public IStringToSymbolService StringToSymbolService { get; set; }
        [Dependency]
        public IRepeatableStringsService RepeatableStringsService { get; set; }
        [Dependency]
        public IAddableStringsService AddableStringsService { get; set; }

        private Dictionary<string,UserControl> _controlsList;

        public Dictionary<string, UserControl> ControlsList
        {
            get { return _controlsList; }
        }

        public InstagramUser User { get; set; }

        private void AnyButton_OnClick(object sender, RoutedEventArgs e)
        {
            UserControl tab;

            var button = sender as Button;
            if (button == null) return;
            var tag = Convert.ToString(button.Tag);

            if (_controlsList.ContainsKey(tag))
            {
                tab = _controlsList[tag];
                tab.Width = double.NaN;
                tab.Height = double.NaN;
                Panel.Children.Clear();
                Panel.Children.Add(tab);
                return;
            }
            try
            {
                if (tag == "Login")
                {

                    tab =
                        (UserControl)
                            Activator.CreateInstance(Type.GetType("InstAd128000.Controls.InstagramTabs." + tag), RequestService,
                                StringToSymbolService, RepeatableStringsService, AddableStringsService);
                }
                else
                {
                    tab = (UserControl)Activator.CreateInstance(Type.GetType("InstAd128000.Controls.InstagramTabs." + tag));
                }
                _controlsList.Add(tag,tab);
            }
            catch (Exception ex)
            {
                MessageBox.Show("This functionality is under development");
                return;
            }

            tab.Width = double.NaN;
            tab.Height = double.NaN;
            Panel.Children.Clear();
            Panel.Children.Add(tab);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ButtonState != MouseButtonState.Released)
                this.DragMove();
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            Driver.Close();
            Application.Current.Shutdown();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void Minimize_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Maximize_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }

        private void DefaultSize_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Normal;
            this.Width = Convert.ToDouble(Settings.Default.DefaultWidth);
            this.Height = Convert.ToDouble(Settings.Default.DefaultHeight);
        }

        private void Settings_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This functionality is under development");
        }
    }
}
