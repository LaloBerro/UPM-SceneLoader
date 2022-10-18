using CommandQueues.Core;
using Zenject;
using ZenjectExtensions.Zinstallers;

namespace ScenesLoaderSystem
{
    public class DoneLoadSceneNotifierZinstaller : Zinstaller
    {
        private ISceneLoader _sceneLoader;
        private ICommandQueue _commandQueue;

        [Inject]
        public void InjectDependecies(ISceneLoader sceneLoader, ICommandQueue commandQueue)
        {
            _sceneLoader = sceneLoader;
            _commandQueue = commandQueue;
        }

        public override void Install()
        {
            new DoneLoadSceneNotifier(_sceneLoader, _commandQueue);
        }
    }
}