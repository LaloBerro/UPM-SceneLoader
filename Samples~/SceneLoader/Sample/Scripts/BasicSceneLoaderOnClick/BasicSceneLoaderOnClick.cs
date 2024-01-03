using ScenesLoaderSystem.Core.Domain;
using ScenesLoaderSystem.Core.InterfaceAdapters;
using UnityEngine;

namespace ScenesLoaderSystem
{
    public class BasicSceneLoaderOnClick : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private SceneDataSO _sceneDataSo;

        private ISceneLoader _sceneLoader;

        private void Awake()
        {
            _sceneLoader = ServiceLocatorPattern.ServiceLocatorInstance.Instance.Get<ISceneLoader>();
        }

        public void ChangeScene()
        {
            _sceneLoader.LoadScene(_sceneDataSo.GetSceneData());
        }
    }
}