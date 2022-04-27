using UnityEngine;
using UnityEngine.Events;

namespace ScenesLoaderSystem
{
    public class UnityEventOnAllSceneAreLoaded : ExceutorOnAllSceneAreLoaded
    {
        [Header("References")]
        [SerializeField] private UnityEvent _onAllSceneAreLoaded;

        protected override void OnAllSceneAreLoaded()
        {
            _onAllSceneAreLoaded?.Invoke();
        }
    }
}