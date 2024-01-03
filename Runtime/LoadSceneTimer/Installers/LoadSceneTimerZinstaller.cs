using UnityEngine;
using Zenject;
using ZenjectExtensions.Zinstallers;

namespace ScenesLoaderSystem
{
    public class LoadSceneTimerZinstaller :  Zinstaller
    {
        [Header("Config")]
        [SerializeField] private int _duration;
        
        [Inject]
        private ISceneDataLoader _sceneDataLoader;

        public override void Install()
        {
            new LoadSceneTimer(_duration, _sceneDataLoader);
        }
    }
}