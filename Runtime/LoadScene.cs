using UnityEngine;

namespace ScenesLoaderSystem
{
    public class LoadScene : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SceneDataSO _sceneDataSO;

        public void Load()
        {
            ScenesLoader.Instance.LoadScene(_sceneDataSO);
        }
    }
}