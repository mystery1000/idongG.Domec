using idongG.Domec.PlcDA;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace idongG.Domec.PlcDA.Save
{
    public class SaveDataClass
    {
        public List<IEntity> Items { get; set; }
        public string CurrentID { get; set; }

        public SaveDataClass()
        {
            Items = new List<IEntity>();
            CurrentID = "";
        }

        public static T CloneJson<T>(T t)
        {
            T Clone;
            var settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.All;
            var strJson = JsonConvert.SerializeObject(t, settings);
            Clone = JsonConvert.DeserializeObject<T>(strJson, settings);
            return Clone;
        }

        public static T LoadJson<T>(FileInfo file)
        {
            if (file == null || !file.Exists) return default(T);

            string strJson;
            try
            {
                strJson = File.ReadAllText(file.FullName, Encoding.UTF8);
                if (string.IsNullOrEmpty(strJson)) return default(T);

                var settings = new JsonSerializerSettings();
                settings.TypeNameHandling = TypeNameHandling.Auto;
                return JsonConvert.DeserializeObject<T>(strJson, settings);
            }
            catch (Exception ex)
            {
                // 这里可以添加日志记录或其他异常处理逻辑
                Console.WriteLine($"An error occurred: {ex.Message}");
                return default(T);
            }
        }

        public static bool SaveJson<T>(FileInfo file, T t)
        {
            bool SaveJson;
            if (file == null)
            {
                SaveJson = false;
            }
            else
            {
                if (file.Directory.Exists == false) file.Directory.Create();
                if (!file.Exists)
                {
                    new FileStream(file.FullName, FileMode.Create, FileAccess.ReadWrite).Close();
                }
                var settings = new JsonSerializerSettings();
                settings.TypeNameHandling = TypeNameHandling.Auto;
                File.WriteAllText(file.FullName, JsonConvert.SerializeObject(t, settings), ASCIIEncoding.UTF8);
                SaveJson = true;
            }
            return SaveJson;
        }
    }
}