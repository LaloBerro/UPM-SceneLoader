using CommandQueues.Core;
using Installers.Core;
using ServiceLocatorPattern;
using UnityEngine;

namespace ScenesLoaderSystem
{
    public class SceneLoadedNotifier : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private MonoInstaller<ICommandQueue> _commandQueueInstaller;

        private void Start()
        {
            Debug.Log("2 SceneLoadedNotifier");
            ServiceLocator.Instance.Get<ISceneLoader>().SetNodeCommandOfALoadedScene(_commandQueueInstaller.Data);
        }
    }
}