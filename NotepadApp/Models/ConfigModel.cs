using iNKORE.UI.WPF.Modern.Controls;
using Newtonsoft.Json;
using NotepadApp.Models.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NotepadApp.Models
{
    public static class ConfigModel
    {
        public static List<TextTabItemModel> FileList { get; set; } = new List<TextTabItemModel>();

        public static bool RemoveItemByUID(string uid)
        {
            if (string.IsNullOrEmpty(uid)) throw new ArgumentNullException(nameof(uid));

            lock (ConfigModel.FileList)
            {
                var item = ConfigModel.FileList.FirstOrDefault(x =>
                    x.UID.Equals(uid, StringComparison.OrdinalIgnoreCase));

                if (item != null)
                {
                    ConfigModel.FileList.Remove(item);
                    return true;
                }
                return false;
            }
        }

        public static bool UpdateItemByUID(TextTabItemModel item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (string.IsNullOrEmpty(item.UID)) throw new ArgumentNullException(nameof(item.UID));

            lock (ConfigModel.FileList)
            {
                var existingItem = ConfigModel.FileList.FirstOrDefault(x =>
                    x.UID.Equals(item.UID, StringComparison.OrdinalIgnoreCase));

                if (existingItem != null)
                {
                    existingItem.FilePath = item.FilePath;
                    existingItem.TempContent = item.TempContent;
                    existingItem.IsChange = item.IsChange;
                    existingItem.LastChange = item.LastChange;
                    existingItem.Encoding = item.Encoding;
                    existingItem.FileType = item.FileType;
                    return true;
                }
                return false;
            }
        }
    }

    public static class ConfigManager
    {
        private static readonly string ConfigFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "NotepadApp",
            "config.json");

        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Auto
        };

        /// <summary>
        /// 添加或更新一个文本标签项到配置
        /// </summary>
        /// <param name="item">要添加的文本标签项</param>
        public static void AddOrUpdateItem(TextTabItemModel item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            lock (ConfigModel.FileList)
            {
                var existingItem = ConfigModel.FileList.FirstOrDefault(x =>
                    x.FilePath.Equals(item.FilePath, StringComparison.OrdinalIgnoreCase));

                if (existingItem != null)
                {
                    // 更新现有项
                    existingItem.TempContent = item.TempContent;
                    existingItem.IsChange = item.IsChange;
                    existingItem.LastChange = item.LastChange;
                    existingItem.Encoding = item.Encoding;
                    existingItem.FileType = item.FileType;
                }
                else
                {
                    // 添加新项
                    ConfigModel.FileList.Add(item);
                }
            }
        }

        /// <summary>
        /// 从配置中删除指定路径的项
        /// </summary>
        /// <param name="filePath">要删除的项的文件路径</param>
        /// <returns>是否成功删除</returns>
        public static bool RemoveItem(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException(nameof(filePath));

            lock (ConfigModel.FileList)
            {
                var item = ConfigModel.FileList.FirstOrDefault(x =>
                    x.FilePath.Equals(filePath, StringComparison.OrdinalIgnoreCase));

                if (item != null)
                {
                    ConfigModel.FileList.Remove(item);
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 保存配置到文件
        /// </summary>
        /// <returns>是否成功保存</returns>
        public static bool SaveConfig()
        {
            try
            {
                // 确保目录存在
                var configDir = Path.GetDirectoryName(ConfigFilePath);
                if (!Directory.Exists(configDir))
                {
                    Directory.CreateDirectory(configDir);
                }

                lock (ConfigModel.FileList)
                {
                    string json = JsonConvert.SerializeObject(ConfigModel.FileList, SerializerSettings);
                    File.WriteAllText(ConfigFilePath, json);
                }
                return true;
            }
            catch (Exception ex)
            {
                // 在实际应用中，可以记录日志
                Console.WriteLine($"保存配置失败: {ex.Message}");
                return false;
            }
            
        }

        /// <summary>
        /// 从文件加载配置
        /// </summary>
        /// <returns>是否成功加载</returns>
        public static bool LoadConfig()
        {
            try
            {
                if (File.Exists(ConfigFilePath))
                {
                    string json = File.ReadAllText(ConfigFilePath);
                    var list = JsonConvert.DeserializeObject<List<TextTabItemModel>>(json, SerializerSettings);

                    lock (ConfigModel.FileList)
                    {
                        ConfigModel.FileList.Clear();
                        if (list != null)
                        {
                            ConfigModel.FileList.AddRange(list);
                        }
                    }
                    return true;
                }
                else
                {
                    return SaveConfig();
                }
            }
            catch (Exception ex)
            {
                // 在实际应用中，可以记录日志
                Console.WriteLine($"加载配置失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 清除所有配置
        /// </summary>
        public static void ClearConfig()
        {
            lock (ConfigModel.FileList)
            {
                ConfigModel.FileList.Clear();
            }
        }

        /// <summary>
        /// 获取指定路径的配置项
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>找到的配置项，未找到返回null</returns>
        public static TextTabItemModel GetItem(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException(nameof(filePath));

            lock (ConfigModel.FileList)
            {
                return ConfigModel.FileList.FirstOrDefault(x =>
                    x.FilePath.Equals(filePath, StringComparison.OrdinalIgnoreCase));
            }
        }
    }
}