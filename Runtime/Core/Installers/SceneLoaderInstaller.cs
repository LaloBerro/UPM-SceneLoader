using Installers.Core;
using ServiceLocatorPattern;
using UnityEngine;

namespace ScenesLoaderSystem
{
    public class SceneLoaderInstaller : MonoInstaller
    {
        [Header("References")]
        [SerializeField] private SceneDataSO _loadingScreenSceneDataSo;
        [SerializeField] private SceneDataSO _firstOpenSceneDataSo;
        public override void Install()
        {
            ISceneLoader sceneLoader = new SceneLoader(_loadingScreenSceneDataSo.SceneData, _firstOpenSceneDataSo.SceneData);

            ServiceLocator.Instance.Register<ISceneLoader>(sceneLoader);
        }
    }
}