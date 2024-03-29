using ScenesLoaderSystem.Core.Domain;
using ScenesLoaderSystem.Core.InterfaceAdapters;
using ServiceLocatorPattern;
using UnityEngine;
using ZenjectExtensions.Zinstallers;

namespace ScenesLoaderSystem
{
    public class SceneDataLoaderZinstaller : InstanceZinstaller<ISceneDataLoader>
    {
        [Header("References")]
        [SerializeField] private SceneDataSO _sceneDataSO;

        protected override ISceneDataLoader GetInitializedClass()
        {
            ISceneLoader sceneLoader = ServiceLocatorInstance.Instance.Get<ISceneLoader>();

            return new SceneDataLoader(sceneLoader, _sceneDataSO.GetSceneData());
        }
    }
}