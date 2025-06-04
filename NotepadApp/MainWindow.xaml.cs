// MainWindow.xaml.cs
using iNKORE.UI.WPF.Modern;
using iNKORE.UI.WPF.Modern.Controls;
using NotepadApp.Models;
using NotepadApp.Pages;
using NotepadApp.ViewModels;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NotepadApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();

            Runtimes.MainWindow = this;
            Runtimes.TabManager = new TabManager(TabList, MainFrame, this);
            Runtimes.TabManager.InitializeTabs();

            var args = Environment.GetCommandLineArgs();
            if (args != null && args.Length > 1)
            {
                if (File.Exists(args[1]))
                    Runtimes.TabManager.AddNewTab(args[1]);
            }

            Closing += (s, e) => ConfigManager.SaveConfig();
            //ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
        }

        private void TabList_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            Runtimes.TabManager.OnTabSelectionChanged(args);
        }

        private void NewTab_Click(object sender, RoutedEventArgs e)
        {
            Runtimes.TabManager.AddNewTab();
        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            TabList.SelectedItem = null;
            Title = "NotepadApp 设置";
            MainFrame.Navigate(new SettingPage());
        }

        private void RemoveTabMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem &&
                menuItem.Parent is ContextMenu contextMenu &&
                contextMenu.PlacementTarget is NavigationViewItem navigationViewItem)
            {
                Runtimes.TabManager.CloseTab(navigationViewItem.Tag?.ToString());
            }
        }

    }
}