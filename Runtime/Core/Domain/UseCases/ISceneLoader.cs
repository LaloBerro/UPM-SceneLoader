using System;
using CommandQueues.Core;

namespace ScenesLoaderSystem.Core.Domain
{
    public interface ISceneLoader
    {
        Action OnAllScenesAreLoaded { get; set; }
        void LoadScene(SceneData sceneData, bool dontRemoveOpenScenes = false);
        void SetNodeCommandOfALoadedScene(INodeCommand nodeCommand);
        void RemoveCurrentAndSetPrincipal(SceneData currentSceneData);
        bool IsThisSceneDataOpened(SceneData sceneData);
        void ReloadCurrentScene();
    }
}