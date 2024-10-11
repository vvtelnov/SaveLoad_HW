using GameEngine;
using SaveSystem.GameRepository;
using SaveSystem.SaveLoader;
using SaveSystem.SaveLoader.SaveLoaders;
using UnityEngine;
using Zenject;
using Resource = GameEngine.Resource;

namespace Installers
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private Transform _unitsSpawnContainer;

        public override void InstallBindings()
        {
            Container.Bind<EntryPoint>().FromInstance(FindObjectOfType<EntryPoint>()).AsSingle();
            Container.Bind<SaveSystemManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameRepository>().AsSingle();
            Container.BindInterfacesAndSelfTo<SaveLoaderManager>().AsSingle();
            Container.Bind<ResourceService>().AsSingle();
            Container.Bind<UnitManager>().AsSingle();
            Container.Bind<Transform>().FromInstance(_unitsSpawnContainer).AsSingle();
            
            BindSceneResources();
            BingSceneUnits();
            BindSaveLoaders();
        }

        private void BindSceneResources()
        {
            Container.Bind<Resource[]>().FromInstance(FindObjectsOfType<Resource>()).AsSingle();
        }

        private void BingSceneUnits()
        {
            Container.Bind<Unit[]>().FromInstance(FindObjectsOfType<Unit>()).AsSingle();
        }

        private void BindSaveLoaders()
        {
            Container.Bind<ISaveLoader>().To<ResourceSaveLoader>().AsSingle();
            Container.Bind<ISaveLoader>().To<UnitsSaveLoader>().AsSingle();
        }
    }
}