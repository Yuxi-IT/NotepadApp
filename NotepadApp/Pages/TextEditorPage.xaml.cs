using NotepadApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using NotepadApp.Models.Enums;
using MessageBox = iNKORE.UI.WPF.Modern.Controls.MessageBox;

namespace NotepadApp.Pages
{
    /// <summary>
    /// TextEditorPage.xaml 的交互逻辑
    /// </summary>
    public partial class TextEditorPage : Page
    {
        private TextTabItemModel _file;
        private const double ZoomStep = 1.0;
        private const double MinFontSize = 8.0;
        private const double MaxFontSize = 72.0;

        public TextEditorPage(TextTabItemModel file)
        {
            InitializeComponent();
            _file = file;

            if (file.FilePath == "NONE")
            {
                if (file.TempContent != "")
                {
                    MainEditor.Text = file.TempContent;
                }
            }
            else
            {
                MainEditor.Text = File.ReadAllText(file.FilePath);
            }

            MainEditor.TextChanged += (s, e) =>
            {
                file.TempContent = MainEditor.Text;
                file.IsChange = true;
                ConfigModel.UpdateItemByUID(file);
                UpdateStatusBar();
            };

            MainEditor.PreviewKeyDown += MainEditor_PreviewKeyDown;
            MainEditor.PreviewMouseWheel += MainEditor_PreviewMouseWheel;

            UpdateStatusBar();
        }

        private void MainEditor_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                double newSize = MainEditor.FontSize + (e.Delta > 0 ? ZoomStep : -ZoomStep);
                newSize = Math.Max(MinFontSize, Math.Min(MaxFontSize, newSize));
                MainEditor.FontSize = newSize;
                e.Handled = true;
                UpdateStatusBar();
            }
        }

        private void MainEditor_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
            {
                SaveFile();
                e.Handled = true;
            }
        }

        private void SaveFile()
        {
            if (_file.FilePath == "NONE")
            {
                SaveFileAs();
            }
            else
            {
                try
                {
                    File.WriteAllText(_file.FilePath, MainEditor.Text);
                    _file.IsChange = false;
                    ConfigModel.UpdateItemByUID(_file);
                    SetStatusSaved("已保存");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"保存失败: {ex.Message}");
                }
            }
        }

        private void SaveFileAs()
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, MainEditor.Text);
                    _file.FilePath = saveFileDialog.FileName;
                    _file.IsChange = false;
                    ConfigModel.UpdateItemByUID(_file);
                    SetStatusSaved("已保存");
                }
                catch (Exception ex)
                {
                    SetStatusSaved($"保存失败: {ex.Message}");
                }
            }
        }

        private void UpdateStatusBar()
        {
            if (Zoom != null)
                Zoom.Text = $"{(int)(MainEditor.FontSize / 12.0 * 100)}%";
            if (Encoding != null)
                Encoding.Text = GetEncodingText(_file.Encoding);
            if (Type != null)
                Type.Text = GetTypeText(_file.FileType);
            if (IsSaved != null)
                IsSaved.Text = _file.IsChange ? "未保存" : "已保存";
            if (Lines != null)
                Lines.Text = $"{MainEditor.LineCount}行";
            if (Chars != null)
                Chars.Text = $"{MainEditor.Text.Length}个字符";
        }

        private void SetStatusSaved(string status)
        {
            if (IsSaved != null)
                IsSaved.Text = status;
            UpdateStatusBar();
        }


        private string GetEncodingText(EncodingE encoding)
        {
            return encoding switch
            {
                EncodingE.UTF8 => "UTF-8",
                EncodingE.UTF8WithBOM => "UTF-8 BOM",
                EncodingE.UTF16LE => "UTF-16 LE",
                EncodingE.UTF16BE => "UTF-16 BE",
                EncodingE.ASCII => "ASCII",
                EncodingE.GBK => "GBK",
                EncodingE.GB2312 => "GB2312",
                EncodingE.Big5 => "Big5",
                EncodingE.ISO88591 => "ISO-8859-1",
                EncodingE.Windows1252 => "Windows-1252",
                _ => "未知编码"
            };
        }

        private string GetTypeText(FileTypeE type)
        {
            return type switch
            {
                FileTypeE.Text => "Text文本",
                FileTypeE.HTML => "HTML",
                FileTypeE.Json => "Json",
                FileTypeE.MarkdownV2 => "MarkdownV2",
                FileTypeE.MarkdownV3 => "MarkdownV3",
                FileTypeE.Python => "Python",
                FileTypeE.CSharp => "C#",
                FileTypeE.Java => "Java",
                FileTypeE.C => "C",
                FileTypeE.CPP => "C++",
                FileTypeE.TypeScript => "TypeScript",
                FileTypeE.JavaScript => "JavaScript",
                FileTypeE.Vue => "Vue",
                FileTypeE.Log => "Log",
                FileTypeE.INI => "INI",
                FileTypeE.HTTP => "HTTP",
                FileTypeE.Sln => "Sln",
                FileTypeE.Xml => "XML",
                FileTypeE.Xaml => "XAML",
                FileTypeE.RDP => "RDP",
                FileTypeE.Config => "Config",
                FileTypeE.Class => "Class",
                _ => "未知类型"
            };
        }

    }
}