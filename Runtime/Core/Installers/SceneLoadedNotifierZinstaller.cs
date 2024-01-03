using CommandQueues.Core;
using ScenesLoaderSystem.Core.Domain;
using ServiceLocatorPattern;
using Zenject;
using ZenjectExtensions.Zinstallers;

namespace ScenesLoaderSystem
{
    public class SceneLoadedNotifierZinstaller : Zinstaller
    {
        [Inject]
        private ICommandQueue _commandQueue;
        private ISceneLoader _sceneLoader;

        public override void Install()
        {
            _sceneLoader = ServiceLocatorInstance.Instance.Get<ISceneLoader>();
            
            new SceneLoadedNotifier(_sceneLoader, _commandQueue);
        }
    }
}