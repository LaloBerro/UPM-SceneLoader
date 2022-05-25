using System.Collections.Generic;

namespace ScenesLoaderSystem
{
    [System.Serializable]
    public class SceneData
    {
        public string nameScene;
        public bool useLoadingScreen = true;
        public bool isLockedScene;
        public bool removeLockedScenes;
        public bool isPrincipal;

        public SceneDataSO[] scenesData;

        public SceneData[] GetAllScenesToOpen()
        {
            List<SceneData> scenesToOpen = new List<SceneData>();

            foreach (var sceneDataSO in scenesData)
            {
                scenesToOpen.Add(sceneDataSO.SceneData);
            }

            int totalScenesToLoad = scenesToOpen.Count;
            for (var i = 0; i < totalScenesToLoad; i++)
            {
                SceneData[] scenesIntoSceneData = scenesToOpen[i].GetAllScenesToOpen();
                foreach (var sceneDataInto in scenesIntoSceneData)
                {
                    scenesToOpen.Add(sceneDataInto);
                }
            }

            scenesToOpen.Add(this);

            /* SceneData[] scenes = new SceneData[scenesData.Length + 1];

             scenes[scenesData.Length] = this;

             for (int i = 0; i < scenesData.Length; i++)
             {
                 scenes[i] = scenesData[i].SceneData;
             }*/

            return scenesToOpen.ToArray();
        }
    }
}