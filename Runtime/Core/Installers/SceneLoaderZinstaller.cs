using Installers.Core;
using ScenesLoaderSystem.Core.Domain;
using ScenesLoaderSystem.Core.InterfaceAdapters;
using ServiceLocatorPattern;
using UnityEngine;
using ZenjectExtensions.Zinstallers;

namespace ScenesLoaderSystem.Core.Installers
{
    public class SceneLoaderZinstaller : Zinstaller
    {
        [Header("References")]
        [SerializeField] private SceneDataSO _loadingScreenSceneDataSo;
        [SerializeField] private SceneDataSO _emptySceneDataSo;
        [SerializeField] private SceneDataSO _firstOpenSceneDataSo;
        
        public override void Install()
        {
            ISceneLoader sceneLoader = new SceneLoader(_loadingScreenSceneDataSo.GetSceneData(), _firstOpenSceneDataSo.GetSceneData(), _emptySceneDataSo.GetSceneData());

            ServiceLocatorInstance.Instance.Add<ISceneLoader>(sceneLoader);
        }
    }
}