using System;
using ServiceLocatorPattern;
using UnityEngine;

namespace ScenesLoaderSystem
{
    [Obsolete]
    public class LoadScene : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SceneDataSO _sceneDataSO;

        public void Load()
        {
            ServiceLocator.Instance.Get<ISceneLoader>().LoadScene(_sceneDataSO.SceneData);
            //ScenesLoader.Instance.LoadScene(_sceneDataSO);
        }
    }
}