using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Game
{
    // Author: David Pagotto
    public class SaveFile : IDisposable
    {
        private const string saveFileDirectory = "saves/";
        private string saveFileName;

        private FileStream saveFileStream;
        private Hashtable savedObjects = new Hashtable();

        private string SaveDirectoryPath => Application.persistentDataPath + "/" + saveFileDirectory;
        private string SaveFilePath => SaveDirectoryPath + saveFileName;

        /// <summary>
        /// Sauvegarde un objet serializable.
        /// </summary>
        /// <typeparam name="T">Le type de l'objet</typeparam>
        /// <param name="saveTag">Le tag de la sauvegarde</param>
        /// <param name="serializableObject"></param>
        /// <param name="uid">S'il y a plusieurs objets sauvegardés avec le tag spécifié, on doit spécifier un identifieur unique.</param>
        public void SaveData<T>(SaveTag saveTag, T serializableObject, string uid = null)
        {
            string key = saveTag.ToString();
            if (uid != null)
                key += $"|{uid}";
            savedObjects[key] = serializableObject;
            WriteSaveFile();
        }

        /// <summary>
        /// Obtient un objet serializable sauvegardé.
        /// </summary>
        /// <typeparam name="T">Le type de l'objet</typeparam>
        /// <param name="saveTag">Le tag de la sauvegarde</param>
        /// <param name="uid">S'il y a plusieurs objets sauvegardés avec le tag spécifié, on doit spécifier un identifieur unique.</param>
        /// <returns>L'objet sauvegardé</returns>
        public T ReadData<T>(SaveTag saveTag, string uid = null)
        {
            string key = saveTag.ToString();
            if (uid != null)
                key += $"|{uid}";
            if (savedObjects.ContainsKey(key))
                return (T)savedObjects[key];
            return default;
        }

        private void WriteSaveFile()
        {
            // https://docs.microsoft.com/en-us/dotnet/api/system.runtime.serialization.formatters.binary.binaryformatter.deserialize?view=netcore-3.1
            saveFileStream.Seek(0, SeekOrigin.Begin);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(saveFileStream, savedObjects);
        }
        private void ReadSaveFile()
        {
            saveFileStream.Seek(0, SeekOrigin.Begin);
            BinaryFormatter formatter = new BinaryFormatter();
            savedObjects = (Hashtable)formatter.Deserialize(saveFileStream);
        }

        public void Dispose()
        { 
            saveFileStream.Flush();
            saveFileStream.Dispose();
        }

        public void Close()
        {
            saveFileStream.Flush();
            saveFileStream.Close();
        }

        public SaveFile(string fileName)
        {
            saveFileName = fileName;
            Directory.CreateDirectory(SaveDirectoryPath);
            bool exists = File.Exists(SaveFilePath);
            saveFileStream = new FileStream(SaveFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            if (exists)
                ReadSaveFile();
            else
                WriteSaveFile();
        }
    }
}
