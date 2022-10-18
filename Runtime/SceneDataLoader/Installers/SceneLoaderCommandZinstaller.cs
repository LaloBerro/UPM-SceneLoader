using Commands.Core;
using Zenject;
using ZenjectExtensions.Zinstallers;

namespace ScenesLoaderSystem
{
    public class SceneLoaderCommandZinstaller : CachedInstanceZinstaller<ICommand>
    {
        private ISceneDataLoader _sceneDataLoader;

        [Inject]
        public void InjectSceneDataLoader(ISceneDataLoader sceneDataLoader)
        {
            _sceneDataLoader = sceneDataLoader;
        }

        protected override ICommand GetInitializedClass()
        {
            return new SceneLoaderCommand(_sceneDataLoader);
        }
    }
}