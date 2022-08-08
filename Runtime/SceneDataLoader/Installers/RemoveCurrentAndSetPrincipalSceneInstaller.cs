using Installers.Core;
using ServiceLocatorPattern;
using UnityEngine;

namespace ScenesLoaderSystem
{
    public class RemoveCurrentAndSetPrincipalSceneInstaller : MonoInstaller<ISceneDataLoader>
    {
        [Header("References")]
        [SerializeField] private SceneDataSO _sceneDataSO;

        protected override ISceneDataLoader GetData()
        {
            ISceneLoader sceneLoader = ServiceLocator.Instance.Get<ISceneLoader>();

            return new RemoveCurrentAndSetPrincipalScene(sceneLoader, _sceneDataSO.SceneData);
        }
    }
}