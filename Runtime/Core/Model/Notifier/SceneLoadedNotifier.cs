using System.Threading.Tasks;
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

        private async void Start()
        {
            await Task.Yield();
            ServiceLocator.Instance.Get<ISceneLoader>().SetNodeCommandOfALoadedScene(_commandQueueInstaller.Data);
        }
    }
}