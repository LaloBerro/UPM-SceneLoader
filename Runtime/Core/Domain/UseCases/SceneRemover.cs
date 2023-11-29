using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScenesLoaderSystem.Core.Domain
{
    public class SceneRemover
    {
        public async Task<List<SceneData>> RemoveScenes(List<SceneData> openSceneDatas, bool removeLockedScenes)
        {
            for (int i = 0; i < openSceneDatas.Count; i++)
            {
                if (openSceneDatas[i].IsLockedScene && !removeLockedScenes || openSceneDatas[i].HasToKeepOpen)
                    continue;

                await RemoveScene(openSceneDatas[i]);

                openSceneDatas.Remove(openSceneDatas[i]);

                i--;
            }

            return openSceneDatas;
        }

        public async Task RemoveScene(SceneData openScene)
        {
            AsyncOperation removeSceneOperation = SceneManager.UnloadSceneAsync(openScene.SceneName);

            while (!removeSceneOperation.isDone)
            {
                await Task.Yield();
            }
        }
    }
}