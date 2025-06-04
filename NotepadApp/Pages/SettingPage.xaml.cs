using iNKORE.UI.WPF.Modern;
using iNKORE.UI.WPF.Modern.Controls;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MessageBox = iNKORE.UI.WPF.Modern.Controls.MessageBox;
using Page = System.Windows.Controls.Page;

namespace NotepadApp.Pages
{
    public partial class SettingPage : Page
    {
        public SettingPage()
        {
            InitializeComponent();
            InitializeFontSettings();
            LoadCurrentSettings();

            FontFamilyComboBox.SelectionChanged += FontFamilyComboBox_SelectionChanged;
            FontWeightComboBox.SelectionChanged += FontWeightComboBox_SelectionChanged;
            ClearConfigButton.Click += ClearConfigButton_Click;

            LightThemeRadio.Checked += (s, e) => ThemeManager.Current.ApplicationTheme = ApplicationTheme.Light;
            DarkThemeRadio.Checked += (s, e) => ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
        }

        private void InitializeFontSettings()
        {
            FontFamilyComboBox.ItemsSource = Fonts.SystemFontFamilies
                .OrderBy(f => f.Source)
                .Select(f => f.Source)
                .ToList();

            FontWeightComboBox.ItemsSource = new[]
            {
                new { Name = "Normal", Weight = FontWeights.Normal },
                new { Name = "Bold", Weight = FontWeights.Bold },
                new { Name = "Light", Weight = FontWeights.Light },
                new { Name = "SemiBold", Weight = FontWeights.SemiBold }
            };
            FontWeightComboBox.DisplayMemberPath = "Name";
            FontWeightComboBox.SelectedValuePath = "Weight";
        }

        private void LoadCurrentSettings()
        {
            // 这里加载实际的设置
            LightThemeRadio.IsChecked = true;
            FontFamilyComboBox.SelectedItem = "Consolas";
            FontWeightComboBox.SelectedItem = FontWeights.Normal;
            RestoreTabsRadio.IsChecked = true;
        }

        private void FontFamilyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FontFamilyComboBox.SelectedItem != null && FontPreviewText != null)
            {
                FontPreviewText.FontFamily = new FontFamily(FontFamilyComboBox.SelectedItem.ToString());
            }
        }
        private void FontWeightComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FontWeightComboBox.SelectedItem != null && FontPreviewText != null)
            {
                dynamic selectedItem = FontWeightComboBox.SelectedItem;
                FontPreviewText.FontWeight = selectedItem.Weight;
            }
        }
        private void ClearConfigButton_Click(object sender, RoutedEventArgs e)
        {
            var path = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "NotepadApp",
            "config.json");
            File.Delete(path);
            MessageBox.Show("配置文件已清空", "操作完成", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}