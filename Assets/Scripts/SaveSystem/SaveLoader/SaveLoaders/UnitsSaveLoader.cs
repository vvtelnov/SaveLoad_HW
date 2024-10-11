using System;
using System.Collections.Generic;
using System.Linq;
using GameEngine;
using UnityEngine;

namespace SaveSystem.SaveLoader.SaveLoaders
{
    public struct UnitSavable : ISavableFormat
    {
        public Dictionary<string, float> Position;
        public Dictionary<string, float> Rotation;

        public string Type;
        public int HitPoints;
    }
    
    public class UnitsSaveLoader : AbstractSaveLoader<Unit, UnitSavable>
    {
        private readonly UnitManager _unitManager;
        private Dictionary<string, Unit> _unitPrefabs;
        
        public UnitsSaveLoader(UnitManager unitManager, Dictionary<string, Unit> unitPrefabs)
        {
            _unitManager = unitManager;
            _unitPrefabs = unitPrefabs;
        }

        // Methods for load data
        // protected override void SetSavedData(IEnumerable<Unit> savables)
        // {
        //     // _unitManager.SetupUnits(savables);
        //     
        //     // Я не знаю можно ли его удалить?, не будет ли он в таком случае нарушать принцип LSP?
        //     // P.s Данные устанавливаются в методе GameEngine.UnitManager.SpawnUnit
        // }

        protected override void CleanUpSceneServices()
        {
            Unit[] units = _unitManager.GetAllUnits().ToArray();

            for (int i = 0; i < units.Count(); i++)
            {
                var unit = units[i];
                
                if (unit is null)
                    throw new Exception($"You are trying to clean unit that is null");
                    
                _unitManager.DestroyUnit(unit);
            }
        }

        protected override Unit SetupSavableServiceWithData(UnitSavable data)
        {
            if (!_unitPrefabs.TryGetValue(data.Type, out Unit prefab))
                throw new Exception($"UnitPrefab SO does not contain prefab with component '<Unit>' with type {data.Type}");
            
            var unit = _unitManager.SpawnUnit(prefab: prefab,
                position: new Vector3(data.Position["x"], data.Position["y"], data.Position["z"]),
                rotation: Quaternion.Euler(data.Rotation["x"], data.Rotation["y"], data.Rotation["z"])
                );

            unit.HitPoints = data.HitPoints;

            return unit;
        }
        

        // Methods for save data
        protected override IEnumerable<Unit> GetDataForSave()
        {
            return _unitManager.GetAllUnits();
        }

        protected override UnitSavable ConstructSavableService(Unit unit)
        {
            UnitSavable unitSavable = new()
            {
                Position = new Dictionary<string, float>()
                {
                    { "x", unit.Position.x },
                    { "y", unit.Position.y },
                    { "z", unit.Position.z }
                },
                Rotation = new Dictionary<string, float>()
                {
                    { "x", unit.Rotation.x },
                    { "y", unit.Rotation.y },
                    { "z", unit.Rotation.z }
                },
                Type = unit.Type,
                HitPoints = unit.HitPoints
            };

            return unitSavable;
        }
    }
}