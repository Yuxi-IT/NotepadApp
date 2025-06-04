// TabManager.cs
using iNKORE.UI.WPF.Modern.Controls;
using NotepadApp.Models;
using NotepadApp.Pages;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Frame = System.Windows.Controls.Frame;
using MessageBox = iNKORE.UI.WPF.Modern.Controls.MessageBox;
using Path = System.IO.Path;

namespace NotepadApp.Models
{
    public class TabManager
    {
        private readonly NavigationView _tabList;
        private readonly Frame _mainFrame;
        private readonly Window _mainWindow;

        public TabManager(NavigationView tabList, Frame mainFrame, Window mainWindow)
        {
            _tabList = tabList;
            _mainFrame = mainFrame;
            _mainWindow = mainWindow;
        }

        public void InitializeTabs()
        {
            if (ConfigManager.LoadConfig())
            {
                Console.WriteLine(ConfigModel.FileList.Count);
                if (ConfigModel.FileList.Count > 0)
                {
                    foreach (var file in ConfigModel.FileList)
                    {
                        Console.WriteLine($"FileName: {Path.GetFileName(file.FilePath)}");
                        AddTabItem(file);
                    }

                    if (_tabList.MenuItems.Count > 0)
                    {
                        _tabList.SelectedItem = _tabList.MenuItems[0];
                    }
                }
                else
                {
                    AddNewTab();
                }
            }
        }

        public void AddNewTab(string file = "NONE")
        {
            var newFile = new TextTabItemModel
            {
                UID = Guid.NewGuid().ToString(),
                FilePath = file,
                TempContent = string.Empty
            };
            ConfigModel.FileList.Add(newFile);
            AddTabItem(newFile, true);
        }

        private void AddTabItem(TextTabItemModel file, bool select = false)
        {
            var newTabItem = new NavigationViewItem
            {
                Content = $"{(file.IsChange ? "*" : "")}{Path.GetFileName(file.FilePath)}",
                Icon = new FontIcon { Glyph = "\ue70b" },
                Tag = file.UID,
            };

            _tabList.MenuItems.Add(newTabItem);

            if (select)
            {
                _tabList.SelectedItem = newTabItem;
                _mainFrame.Navigate(new TextEditorPage(file));
                UpdateWindowTitle(file);
            }
        }

        public void OnTabSelectionChanged(NavigationViewSelectionChangedEventArgs args)
        {
            if(args.SelectedItemContainer == null)
                return;
            
            var selectedUid = args.SelectedItemContainer.Tag?.ToString();
            var selectedItem = ConfigModel.FileList.FirstOrDefault(item => item.UID == selectedUid);

            if (selectedItem != null)
            {
                _mainFrame.Navigate(new TextEditorPage(selectedItem));
                UpdateWindowTitle(selectedItem);
            }
            else
            {
                AddNewTab();
            }
        }

        public void CloseTab(string tabUid)
        {
            var tabToRemove = _tabList.MenuItems
                .OfType<NavigationViewItem>()
                .FirstOrDefault(item => item.Tag?.ToString() == tabUid);

            if (tabToRemove != null)
            {
                var fileToRemove = ConfigModel.FileList.FirstOrDefault(item => item.UID == tabUid);

                if (fileToRemove?.IsChange == true)
                {
                    var result = MessageBox.Show(
                        $"文件 {Path.GetFileName(fileToRemove.FilePath)} 有未保存的更改，是否要保存？",
                        "未保存的更改",
                        MessageBoxButton.YesNoCancel,
                        MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        // Save logic here
                    }
                    else if (result == MessageBoxResult.Cancel)
                    {
                        return;
                    }
                }

                _tabList.MenuItems.Remove(tabToRemove);
                if (fileToRemove != null)
                {
                    ConfigModel.FileList.Remove(fileToRemove);
                }

                if (_tabList.SelectedItem == tabToRemove)
                {
                    if (_tabList.MenuItems.Count > 0)
                    {
                        _tabList.SelectedItem = _tabList.MenuItems[0];
                    }
                    else
                    {
                        AddNewTab();
                    }
                }
            }
        }

        private void UpdateWindowTitle(TextTabItemModel file)
        {
            _mainWindow.Title = $"{Path.GetFileName(file.FilePath)}{(file.IsChange ? " - 未保存" : "")}";
        }

        // 添加新方法
        public TextTabItemModel GetCurrentTab()
        {
            if (_tabList.SelectedItem is NavigationViewItem selectedItem)
            {
                string uid = selectedItem.Tag?.ToString();
                return ConfigModel.FileList.FirstOrDefault(item => item.UID == uid);
            }
            return null;
        }

        public void SaveCurrentTab()
        {
            var currentTab = GetCurrentTab();
            if (currentTab != null)
            {
                // 实现保存逻辑
            }
        }

        public void SaveCurrentTabAs()
        {
            var currentTab = GetCurrentTab();
            if (currentTab != null)
            {
                // 实现另存为逻辑
            }
        }
    }
}