using System;
using CommandQueues.Core;

namespace ScenesLoaderSystem
{
    public interface ISceneLoader
    {
        Action OnAllScenesAreLoaded { get; set; }
        void LoadScene(SceneData sceneData);
        void SetNodeCommandOfALoadedScene(INodeCommand nodeCommand);
    }
}