using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;

namespace MyEasySaveSystem
{
    public static class BasicSaveSystem<T>
    {
        public static void Save(string name, T value)
        {
            string convertedJson = JsonConvert.SerializeObject(value);
            string directory = $"{Application.persistentDataPath}/SavedDatas/{typeof(T)}";

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            string path = $"{directory}/{name}.txt";
            using(StreamWriter writer = new StreamWriter(path, false, Encoding.ASCII))
            {
                writer.WriteLine(convertedJson);
                writer.Close();
            }
        }

        public static async UniTask SaveAsync(string name, T value)
        {
            string convertedJson = JsonConvert.SerializeObject(value);
            string directory = $"{Application.persistentDataPath}/SavedDatas/{typeof(T)}";

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            string path = $"{directory}/{name}.txt";
            using (StreamWriter writer = new StreamWriter(path, false, Encoding.ASCII))
            {
                await writer.WriteLineAsync(convertedJson).AsUniTask();
                writer.Close();
            }
        }

        public static void SaveEncrypt(string name, T value)
        {
            string convertedJson = JsonConvert.SerializeObject(value);
            string directory = $"{Application.persistentDataPath}/SavedDatas/{typeof(T)}";

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            byte[] cipherJson = Encryptor.Encrypt(convertedJson, Encryptor.DefaultKey, Encryptor.DefaultIV);

            string path = $"{directory}/{name}.txt";
            using (StreamWriter writer = new StreamWriter(path, false, Encoding.ASCII))
            {
                writer.WriteLine(cipherJson.ToString());
                writer.Close();
            }
        }

        public static T Load(string name, T defaultValue = default)
        {
            string directory = $"{Application.persistentDataPath}/SavedDatas/{typeof(T)}";
            string path = $"{directory}/{name}.txt";
            
            if (File.Exists(path))
            {
                using(StreamReader reader = new StreamReader(path))
                {
                    string convertedJson = reader.ReadLine();
                    T value = JsonConvert.DeserializeObject<T>(convertedJson);
                    reader.Close();

                    return value;
                }
            }

            return defaultValue;
        }

        public static T LoadDecrypt(string name, T defaultValue = default)
        {
            string directory = $"{Application.persistentDataPath}/SavedDatas/{typeof(T)}";
            string path = $"{directory}/{name}.txt";

            if (File.Exists(path))
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    string convertedJson = reader.ReadLine();
                    string decyptedJson = Encryptor.Decrypt(Encoding.UTF8.GetBytes(convertedJson), Encryptor.DefaultKey, Encryptor.DefaultIV);
                    T value = JsonConvert.DeserializeObject<T>(convertedJson);
                    reader.Close();

                    return value;
                }
            }

            return defaultValue;
        }

        public static async UniTask<T> LoadAsync(string name, T defaultValue = default)
        {
            string directory = $"{Application.persistentDataPath}/SavedDatas/{typeof(T)}";
            string path = $"{directory}/{name}.txt";

            if (File.Exists(path))
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    string convertedJson = await reader.ReadLineAsync().AsUniTask();
                    T value = JsonConvert.DeserializeObject<T>(convertedJson);
                    reader.Close();

                    return value;
                }
            }

            return defaultValue;
        }

        public static void Delete(string name)
        {
            string directory = $"{Application.persistentDataPath}/SavedDatas/{typeof(T)}";
            string path = $"{directory}/{name}.txt";
            
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public static void DeleteAll()
        {
            Directory.Delete($"{Application.persistentDataPath}/SavedDatas");
        }
    }
}