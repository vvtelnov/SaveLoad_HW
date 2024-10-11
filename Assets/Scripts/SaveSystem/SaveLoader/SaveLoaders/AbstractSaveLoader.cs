using System;
using System.Collections.Generic;
using SaveSystem.GameRepository;

namespace SaveSystem.SaveLoader.SaveLoaders
{
    public abstract class AbstractSaveLoader<TSavable, TSaveFormat> : ISaveLoader
    {
        // Имеет ли смысл передавать сюда GameRepository?
        // Я руководствовался логикой, что нам может понадобиться подменять в runtime репозиторий. 
        public void LoadData(IGameRepository gameRepository)
        {
            if (!gameRepository.TryGetData<List<TSaveFormat>>(out var savedData))
            {
                throw new Exception($"No save data or default data found for {typeof(TSavable)}");
            }

            List<TSavable> savableServices = new();

            CleanUpSceneServices();
            
            foreach ( var iData in savedData)
            {
                TSavable savableService = SetupSavableServiceWithData(iData);
                savableServices.Add(savableService);
            }

            SetSavedData(savableServices);
        }


        public void SaveData(IGameRepository gameRepository)
        {
            IEnumerable<TSavable> servicesToSave = GetDataForSave();
            List<TSaveFormat> savableServices = new();

            foreach (var serviceToSave in servicesToSave)
            {
                savableServices.Add(ConstructSavableService(serviceToSave));
            }

            gameRepository.SetData(savableServices);
        }

        // Methods for load data
        protected virtual void SetSavedData(IEnumerable<TSavable> savables) {}
        protected virtual void CleanUpSceneServices() {}
        protected abstract TSavable SetupSavableServiceWithData(TSaveFormat data);
        
        // Methods for save data
        protected abstract IEnumerable<TSavable> GetDataForSave();
        protected abstract TSaveFormat ConstructSavableService(TSavable serviceToSave);
    }
}