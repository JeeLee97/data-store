using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

namespace JeeLee.DataStore.Stores
{
    public sealed class LocalDataStore<T> : DataStore<T>
        where T : class, new()
    {
        public LocalDataStore(bool loadOnInitialize = true) : base(loadOnInitialize)
        {
        }

        protected override void OnSave(out bool success)
        {
            FileStream fileStream = null;

            try
            {
                fileStream = File.Create(GetFullPath());
                string json = JsonConvert.SerializeObject(Data);
                byte[] bytes = Encoding.UTF8.GetBytes(json);
                fileStream.Write(bytes, 0, bytes.Length);
            }
            catch (Exception e)
            {
                success = false;
                Debug.LogError($"DataStore error saving data for {typeof(T)}, {e.Message}");
                return;
            }
            finally
            {
                fileStream?.Close();
            }

            success = true;
        }

        protected override void OnLoad(out bool success)
        {
            string path = GetFullPath();
            if (!File.Exists(path))
            {
                success = false;
                return;
            }

            try
            {
                byte[] bytes = File.ReadAllBytes(path);
                string json = Encoding.UTF8.GetString(bytes);
                Data = JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception e)
            {
                success = false;
                Debug.LogError($"DataStore error loading data for {typeof(T)}, {e.Message}");
                return;
            }

            success = true;
        }

        protected override void OnDelete(out bool success)
        {
            try
            {
                string path = GetFullPath();
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (Exception e)
            {
                success = false;
                Debug.LogError($"DataStore error deleting data for {typeof(T)}, {e.Message}");
                return;
            }

            success = true;
        }

        private string GetFullPath()
        {
            return Application.persistentDataPath + Path.DirectorySeparatorChar + typeof(T).FullName;
        }
    }
}