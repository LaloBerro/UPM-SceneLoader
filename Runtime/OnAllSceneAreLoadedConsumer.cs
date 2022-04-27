using UnityEngine;

namespace ScenesLoaderSystem
{
    public abstract class ExceutorOnAllSceneAreLoaded : MonoBehaviour
    {
        private void Awake()
        {
            ScenesLoader.Instance.OnAllScenesAreLoaded += OnAllSceneAreLoaded;
        }

        protected abstract void OnAllSceneAreLoaded();

        private void OnDestroy()
        {
            ScenesLoader.Instance.OnAllScenesAreLoaded -= OnAllSceneAreLoaded;
        }
    }
}