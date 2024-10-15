using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Converters;
using UnityEngine;
using UnityEngine.Assertions;

namespace SaveSystem.GameRepository
{
    public class GameRepository : IGameRepository
    {
        private Dictionary<string, string> _dataRepository;
        
        private const string SAVE_FILE_EXTENSION = ".txt";
        private const string SAVE_FILE_NAME = "SaveSlot#"; 
        private const string DEFAULT_SAVE_FILE_NAME = "DefaultSaveSlot"; 
        private readonly string _saveSlotsDirPath; 
        private readonly string _defaultSaveSlotsPath; 
        
        public GameRepository()
        {
            _dataRepository = new Dictionary<string, string>();
            _saveSlotsDirPath = Path.Combine(
                Environment.CurrentDirectory, "Assets", "Scripts", "SaveSystem", "Saves");
            _defaultSaveSlotsPath = ConstructDefaultFilePath();
            
            Directory.CreateDirectory(_saveSlotsDirPath);
        }

        bool IGameRepository.TryGetData<T>(out T data)
        {
            var dataTypeStr = typeof(T).ToString();
            
            if (_dataRepository.TryGetValue(dataTypeStr, out var value))
            {
                data = JsonConvert.DeserializeObject<T>(value);

                return true;
            }
            
            data = default;
            return false;
        }

        void IGameRepository.ReadSaveData(int saveSlot)
        {
            string filePath = ConstructFilePath(saveSlot);

            if (File.Exists(filePath))
            {
                ReadSaveSlot(filePath);
                return;
            }

            if (File.Exists(_defaultSaveSlotsPath))
            {
                ReadSaveSlot(_defaultSaveSlotsPath);
                return;
            }

            throw new DataException("Did not found any saves or default save");
        }

        void IGameRepository.SetData<T>(T data)
        {
            var dataType = typeof(T).ToString();
            var serializedData = JsonConvert.SerializeObject(data);
            
            _dataRepository.Add(dataType, serializedData);
        }

        void IGameRepository.WriteSavedData(int saveSlot)
        {
            if (_dataRepository is null)
                throw new InvalidOperationException("Game Repository does not contain any data for saving to file");
            
            string filePath = ConstructFilePath(saveSlot);
            
            using (StreamWriter saveFileSW = new StreamWriter(filePath, false))
            {
                saveFileSW.Write(
                    JsonConvert.SerializeObject(_dataRepository)
                    );
            }
        }

        public Dictionary<string, string> GetAllSavesInfo()
        {
            Dictionary<string, string> savesInfo = new();
            string[] saves = Directory.GetFiles(_saveSlotsDirPath, $"{SAVE_FILE_NAME}*");

            if (File.Exists(_defaultSaveSlotsPath))
            {
                savesInfo.Add("Default save", "Start from the beginning");
            }

            if (saves.Length == 0)
                return savesInfo;

            foreach (var save in saves)
            {
                if (Path.GetExtension(save) == ".meta")
                    continue;
                
                string key = Path.GetFileNameWithoutExtension(save);
                string timeOfCreation = (File.GetLastAccessTime(save)).ToString();     
                
                savesInfo.Add(key, timeOfCreation);
            }

            return savesInfo;
        }

        private void ReadSaveSlot(string saveSlotPath)
        {
            string savedData;

            using (StreamReader savedFileSR = new StreamReader(saveSlotPath))
            {
                savedData = savedFileSR.ReadToEnd();
            }
            
            _dataRepository = JsonConvert.DeserializeObject<Dictionary<string, string>>(savedData);
        }
        
        
        private string ConstructFilePath(int saveSlotNumber)
        {
            string saveFileName = string.Concat(SAVE_FILE_NAME, saveSlotNumber);
            return string.Concat(_saveSlotsDirPath, Path.DirectorySeparatorChar, saveFileName, SAVE_FILE_EXTENSION);
        }
        
        private string ConstructDefaultFilePath()
        {
            return string.Concat(_saveSlotsDirPath, Path.DirectorySeparatorChar, DEFAULT_SAVE_FILE_NAME, SAVE_FILE_EXTENSION);
        }
    }
}