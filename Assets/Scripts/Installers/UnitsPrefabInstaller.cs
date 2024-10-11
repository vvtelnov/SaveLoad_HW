using System;
using System.Collections.Generic;
using GameEngine;
using UnityEngine;
using Zenject;

namespace Installers
{
    [CreateAssetMenu(fileName = "UnitsPrefabInstaller", 
        menuName = "Installers/UnitsPrefabInstaller")]
    public class UnitsPrefabInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private Unit[] _unitPrefabs;
        
        public override void InstallBindings()
        {
            Dictionary<string, Unit> unitPrefabs = new();

            foreach (var iPrefab in _unitPrefabs)
            {
                if (!iPrefab.TryGetComponent<Unit>(out var component))
                    throw new Exception("UnitPrefab SO contains prefab that is not a unit");
                
                unitPrefabs.Add(component.Type, component);
            }
            
            Container.Bind<Dictionary<string, Unit>>().FromInstance(unitPrefabs).AsSingle();
        }
    }
}