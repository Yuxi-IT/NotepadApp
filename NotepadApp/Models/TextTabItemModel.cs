using NotepadApp.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotepadApp.Models
{
    public class TextTabItemModel
    {
        private string _filePath = "";
        // 文件完整路径 若为NONE则无实际文件
        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                FileType = GetFileTypeByPath(_filePath);
                Encoding = GetEncodingByPath(_filePath);
            }
        }
        // Tab唯一标识符
        public string UID { get; set; } = Guid.NewGuid().ToString();
        // 已更改但未保存的内容
        public string TempContent { get; set; } = "";
        //是否有未保存的更改
        public bool IsChange { get; set; } = false;
        // 上次更改的时间戳
        public ulong LastChange { get; set; } = 0;
        public EncodingE Encoding { get; set; } = EncodingE.UTF8;
        public FileTypeE FileType { get; set; }

        public TextTabItemModel()
        {
            FileType = GetFileTypeByPath(FilePath);
            Encoding = GetEncodingByPath(FilePath);
        }

        /// <summary>
        /// 根据路径获取文件类型
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>文件类型</returns>
        public FileTypeE GetFileTypeByPath(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return FileTypeE.Text;

            string ext = System.IO.Path.GetExtension(filePath).ToLowerInvariant();
            return ext switch
            {
                ".txt" => FileTypeE.Text,
                ".html" or ".htm" => FileTypeE.HTML,
                ".json" => FileTypeE.Json,
                ".md" => FileTypeE.MarkdownV2,
                ".markdown" => FileTypeE.MarkdownV3,
                ".py" => FileTypeE.Python,
                ".cs" => FileTypeE.CSharp,
                ".java" => FileTypeE.Java,
                ".c" => FileTypeE.C,
                ".cpp" or ".cxx" or ".hpp" or ".h" => FileTypeE.CPP,
                ".ts" or ".tsx" => FileTypeE.TypeScript,
                ".js" => FileTypeE.JavaScript,
                ".vue" => FileTypeE.Vue,
                ".log" => FileTypeE.Log,
                ".ini" => FileTypeE.INI,
                ".http" => FileTypeE.HTTP,
                ".sln" => FileTypeE.Sln,
                ".xml" => FileTypeE.Xml,
                ".xaml" => FileTypeE.Xaml,
                ".rdp" => FileTypeE.RDP,
                ".config" => FileTypeE.Config,
                ".class" => FileTypeE.Class,
                ".php" => FileTypeE.PHP,
                ".css" => FileTypeE.CSS,
                _ => FileTypeE.Text
            };
        }

        /// <summary>
        /// 根据文件路径自动检测编码（简单实现：根据扩展名推断，实际可用更复杂检测）
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>编码类型</returns>
        public EncodingE GetEncodingByPath(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return EncodingE.UTF8;

            string ext = System.IO.Path.GetExtension(filePath).ToLowerInvariant();
            return ext switch
            {
                ".txt" => EncodingE.UTF8,
                ".md" or ".markdown" => EncodingE.UTF8,
                ".json" => EncodingE.UTF8,
                ".xml" => EncodingE.UTF8,
                ".ini" => EncodingE.UTF8,
                ".log" => EncodingE.UTF8,
                ".csv" => EncodingE.UTF8,
                ".yml" or ".yaml" => EncodingE.UTF8,
                ".c" or ".cpp" or ".cxx" or ".hpp" or ".h" or ".cs" or ".java" or ".js" or ".ts" or ".tsx" or ".vue" or ".php" or ".css" => EncodingE.UTF8,
                ".htm" or ".html" => EncodingE.UTF8,
                ".rdp" => EncodingE.UTF8,
                ".config" => EncodingE.UTF8,
                ".sln" => EncodingE.UTF8,
                ".xaml" => EncodingE.UTF8,
                ".class" => EncodingE.UTF8,
                _ => EncodingE.UTF8
            };
        }

    }
}
