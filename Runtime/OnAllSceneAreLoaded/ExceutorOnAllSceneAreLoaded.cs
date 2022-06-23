using ServiceLocatorPattern;
using UnityEngine;

namespace ScenesLoaderSystem
{
    public abstract class ExceutorOnAllSceneAreLoaded : MonoBehaviour
    {
        private void Awake()
        {
            ServiceLocator.Instance.Get<ISceneLoader>().OnAllScenesAreLoaded += OnAllSceneAreLoaded;
        }

        protected abstract void OnAllSceneAreLoaded();

        private void OnDestroy()
        {
            ServiceLocator.Instance.Get<ISceneLoader>().OnAllScenesAreLoaded -= OnAllSceneAreLoaded;
        }
    }
}