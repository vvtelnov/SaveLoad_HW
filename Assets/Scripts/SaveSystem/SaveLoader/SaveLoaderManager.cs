using System.Collections.Generic;
using SaveSystem.GameData;
using SaveSystem.GameRepository;
using SaveSystem.SaveLoader.SaveLoaders;
using UnityEngine;

namespace SaveSystem.SaveLoader
{
    public class SaveLoaderManager : ISaveLoaderManager
    {
        private readonly List<ISaveLoader> _saveLoaders;

        private IGameRepository _gameRepository;

        public SaveLoaderManager(List<ISaveLoader> saveLoaders, IGameRepository gameRepository)
        {
            _saveLoaders = saveLoaders;
            _gameRepository = gameRepository;
        }

        public void LoadAllData(int saveSlot)
        {
            _gameRepository.ReadSaveData(saveSlot);
            
            foreach (var saveLoader in _saveLoaders)
            {
                saveLoader.LoadData(_gameRepository);
            }
        }

        public void SaveAllData(int saveSlot)
        {
            foreach (var saveLoader in _saveLoaders)
            {
                saveLoader.SaveData(_gameRepository);
            }

            _gameRepository.WriteSavedData(saveSlot);
        }
    }
}