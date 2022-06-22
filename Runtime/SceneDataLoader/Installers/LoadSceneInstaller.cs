using Installers.Core;
using ServiceLocatorPattern;
using UnityEngine;

namespace ScenesLoaderSystem
{
    public class LoadSceneInstaller : MonoInstaller<SceneDataLoader>
    {
        [Header("References")]
        [SerializeField] private SceneDataSO _sceneDataSO;

        protected override SceneDataLoader GetData()
        {
            ISceneLoader sceneLoader = ServiceLocator.Instance.Get<ISceneLoader>();

            return new SceneDataLoader(sceneLoader, _sceneDataSO.SceneData);
        }
    }
}