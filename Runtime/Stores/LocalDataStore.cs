using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

namespace DataStore.Stores
{
    public sealed class LocalDataStore<T> : DataStore<T>
        where T : class, new()
    {
        public LocalDataStore(bool loadOnInitialize = true) : base(loadOnInitialize)
        {
        }

        protected override void OnSave(Action<bool> callback)
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
                Debug.LogError($"(LocalPersistentData) error saving data for {typeof(T)}, {e.Message}");
                callback?.Invoke(false);
                return;
            }
            finally
            {
                fileStream?.Close();
            }

            callback?.Invoke(true);
        }

        protected override void OnLoad(Action<bool> callback)
        {
            string path = GetFullPath();
            if (!File.Exists(path))
            {
                callback?.Invoke(false);
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
                Debug.LogError($"(LocalPersistentData) error loading data for {typeof(T)}, {e.Message}");
                callback?.Invoke(false);
                return;
            }

            callback?.Invoke(true);
        }

        protected override void OnDelete(Action<bool> callback)
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
                Debug.LogError($"(LocalPersistentData) error deleting data for {typeof(T)}, {e.Message}");
                callback?.Invoke(false);
                return;
            }

            callback?.Invoke(true);
        }

        private string GetFullPath()
        {
            return Application.persistentDataPath + Path.DirectorySeparatorChar + typeof(T).FullName;
        }
    }
}