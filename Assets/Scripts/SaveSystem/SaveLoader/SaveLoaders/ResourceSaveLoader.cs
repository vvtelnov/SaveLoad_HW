using System;
using System.Collections.Generic;
using GameEngine;

namespace SaveSystem.SaveLoader.SaveLoaders
{
    public struct ResourceSavable : ISavableFormat
    {
        public string ID;
        public int Amount;
    }
    
    public class ResourceSaveLoader : AbstractSaveLoader<Resource, ResourceSavable>
    {
        private readonly ResourceService _resourceService;

        public ResourceSaveLoader(ResourceService resourceService)
        {
            _resourceService = resourceService;
        }

        // Methods for load data
        protected override Resource SetupSavableServiceWithData(ResourceSavable data)
        {
            foreach (var sceneResource in _resourceService.GetResources())
            {
                if (sceneResource.ID != data.ID) continue;
                sceneResource.Amount = data.Amount;

                return sceneResource;
            }

            throw new Exception($"There is no Resource with id {data.ID} on the scene");
        }

        protected override void SetSavedData(IEnumerable<Resource> savables)
        {
            _resourceService.SetResources(savables);
        }

        // Methods for data save
        protected override IEnumerable<Resource> GetDataForSave()
        {
            return _resourceService.GetResources();
        }

        protected override ResourceSavable ConstructSavableService(Resource resource)
        {
            ResourceSavable resourceSavable = new()
            {
                ID = resource.ID,
                Amount = resource.Amount
            };

            return resourceSavable;
        }
    }
}