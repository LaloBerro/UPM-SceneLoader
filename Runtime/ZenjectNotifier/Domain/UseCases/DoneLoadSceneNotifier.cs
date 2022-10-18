using CommandQueues.Core;

namespace ScenesLoaderSystem
{
    public class DoneLoadSceneNotifier
    {
        public DoneLoadSceneNotifier(ISceneLoader sceneLoader, ICommandQueue commandQueue)
        {
            sceneLoader.SetNodeCommandOfALoadedScene(commandQueue);

        }
    }
}