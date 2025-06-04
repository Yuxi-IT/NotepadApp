using Microsoft.Win32;
using NotepadApp.Commands;
using NotepadApp.Models;
using System.IO;
using System.Windows;
using System.Windows.Input;
using MessageBox = iNKORE.UI.WPF.Modern.Controls.MessageBox;

namespace NotepadApp.ViewModels
{
    public class MainViewModel
    {
        public ICommand NewFileCommand { get; }
        public ICommand OpenFileCommand { get; }
        public ICommand SaveFileCommand { get; }
        public ICommand ExitCommand { get; }

        public ICommand SaveTabCommand { get; }
        public ICommand SaveTabAsCommand { get; }
        public ICommand CloseTabCommand { get; }

        public MainViewModel()
        {
            NewFileCommand = new RelayCommand(_ => Runtimes.TabManager.AddNewTab());
            OpenFileCommand = new RelayCommand(_ => OpenFile());
            SaveFileCommand = new RelayCommand(_ => SaveCurrentFile());
            ExitCommand = new RelayCommand(_ => Application.Current.Shutdown());

            SaveTabCommand = new RelayCommand(_ => SaveCurrentTab());
            SaveTabAsCommand = new RelayCommand(_ => SaveCurrentTabAs());
            CloseTabCommand = new RelayCommand(_ => CloseCurrentTab());
        }

        private void OpenFile()
        {
            FileDialog openFileDialog = new OpenFileDialog
            {
                Title = "打开文件",
                Filter = "所有文件 (*.*)|*.*"
            };
            openFileDialog.ShowDialog();
            if (openFileDialog.FileNames.Length > 0)
            {
                foreach (var fileName in openFileDialog.FileNames)
                {
                    Runtimes.TabManager.AddNewTab(fileName);
                }
            }
        }

        private void SaveCurrentFile()
        {
            // 实现保存当前文件逻辑
        }

        #region 右键菜单命令
        private void SaveCurrentTab()
        {
            // 实现保存当前标签页逻辑
            MessageBox.Show("1");
            Runtimes.TabManager?.SaveCurrentTab();
        }

        private void SaveCurrentTabAs()
        {
            // 实现另存为当前标签页逻辑
            MessageBox.Show("1");
            Runtimes.TabManager?.SaveCurrentTabAs();
        }

        private void CloseCurrentTab()
        {
            // 实现关闭当前标签页逻辑
            var currentTab = Runtimes.TabManager?.GetCurrentTab();
            if (currentTab != null)
            {
                Runtimes.TabManager.CloseTab(currentTab.UID);
            }
        }
        #endregion
    }
}