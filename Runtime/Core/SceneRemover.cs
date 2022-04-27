using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScenesLoaderSystem
{
    public class SceneRemover
    {
        public async Task<List<SceneData>> RemoveScenes(List<SceneData> openSceneDatas, bool removeLockedScenes)
        {
            for (int i = 0; i < openSceneDatas.Count; i++)
            {
                if (openSceneDatas[i].isLockedScene && !removeLockedScenes)
                    continue;

                await RemoveScene(openSceneDatas[i]);

                openSceneDatas.Remove(openSceneDatas[i]);

                i--;
            }

            return openSceneDatas;
        }

        private async Task RemoveScene(SceneData openScene)
        {
            AsyncOperation removeSceneOperation = SceneManager.UnloadSceneAsync(openScene.nameScene);

            while (!removeSceneOperation.isDone)
            {
                await Task.Delay(1);
            }
        }
    }
}