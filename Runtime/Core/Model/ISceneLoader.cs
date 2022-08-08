using System;
using CommandQueues.Core;

namespace ScenesLoaderSystem
{
    public interface ISceneLoader
    {
        Action OnAllScenesAreLoaded { get; set; }
        void LoadScene(SceneData sceneData, bool dontRemoveOpenScenes = false);
        void SetNodeCommandOfALoadedScene(INodeCommand nodeCommand);
        void RemoveCurrentAndSetPrincipal(SceneData currentSceneData);
        bool IsThiSceneDataOpen(SceneData sceneData);
    }
}