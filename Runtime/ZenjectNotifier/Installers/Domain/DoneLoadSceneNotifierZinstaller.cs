using CommandQueues.Core;
using ServiceLocatorPattern;
using Zenject;
using ZenjectExtensions.Zinstallers;

namespace ScenesLoaderSystem
{
    public class DoneLoadSceneNotifierZinstaller : Zinstaller
    {
        private ISceneLoader _sceneLoader;
        private ICommandQueue _commandQueue;

        [Inject]
        public void InjectDependecies(ICommandQueue commandQueue)
        {
            _sceneLoader = ServiceLocator.Instance.Get<ISceneLoader>();
            _commandQueue = commandQueue;
        }

        public override void Install()
        {
            new DoneLoadSceneNotifier(_sceneLoader, _commandQueue);
        }
    }
}