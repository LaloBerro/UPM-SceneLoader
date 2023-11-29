using Installers.Core;
using ScenesLoaderSystem.Core.Domain;
using ScenesLoaderSystem.Core.InterfaceAdapters;
using ServiceLocatorPattern;
using UnityEngine;

namespace ScenesLoaderSystem.Core.Installers
{
    public class SceneLoaderInstaller : MonoInstaller
    {
        [Header("References")]
        [SerializeField] private SceneDataSO _loadingScreenSceneDataSo;
        [SerializeField] private SceneDataSO _firstOpenSceneDataSo;

        public override void Install()
        {
            ISceneLoader sceneLoader = new SceneLoader(_loadingScreenSceneDataSo.GetSceneData(), _firstOpenSceneDataSo.GetSceneData());

            ServiceLocator.Instance.Register<ISceneLoader>(sceneLoader);
        }
    }
}