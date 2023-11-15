using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace MyEasySaveSystem
{
    public static class Encryptor
    {
        private static string defaultKey = "cr7ca594864f4147bbrw1ea2315a1516";

        private static byte[] defaultIV = new byte[]
        {
            90, 21, 42, 37, 64, 65, 68, 73, 82, 19, 102, 141, 162, 113, 147, 115
        };

        public static byte[] DefaultIV => defaultIV;
        public static string DefaultKey => defaultKey;

        public static byte[] Encrypt(string plainText, string key, byte[] IV)
        {
            byte[] encrypted;

            using(AesManaged aes = new AesManaged())
            {
                byte[] byteKey = Encoding.UTF8.GetBytes(key);
                ICryptoTransform crypto = aes.CreateEncryptor(byteKey, IV);
                using(MemoryStream memoryStream = new MemoryStream())
                {
                    using(CryptoStream cryptoStream = new CryptoStream(memoryStream, crypto, CryptoStreamMode.Write))
                    {
                        using(StreamWriter writer = new StreamWriter(cryptoStream))
                        {
                            writer.Write(plainText);
                            encrypted = memoryStream.ToArray();
                            writer.Close();
                        }

                        cryptoStream.Close();
                    }

                    memoryStream.Close();
                }
            }

            return encrypted;
        }

        public static string Decrypt(byte[] cipherText, string key, byte[] IV)
        {
            string plainText;

            using(AesManaged aes = new AesManaged())
            {
                byte[] byteKey = Encoding.UTF8.GetBytes(key);
                ICryptoTransform crypto = aes.CreateDecryptor(byteKey, IV);
                using(MemoryStream memoryStream = new MemoryStream(cipherText))
                {
                    using(CryptoStream cryptoStream = new CryptoStream(memoryStream, crypto, CryptoStreamMode.Read))
                    {
                        using(StreamReader reader = new StreamReader(cryptoStream))
                        {
                            plainText = reader.ReadToEnd();
                            reader.Close();
                        }

                        cryptoStream.Close();
                    }

                    memoryStream.Close();
                }
            }

            return plainText;
        }
    }
}
