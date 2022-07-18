namespace ScenesLoaderSystem
{
    public class SceneDataLoader
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly SceneData _sceneData;

        public SceneDataLoader(ISceneLoader sceneLoader, SceneData sceneData)
        {
            _sceneLoader = sceneLoader;
            _sceneData = sceneData;
        }

        public void Load(bool hasToRemoveOpenScenes = false)
        {
            _sceneLoader.LoadScene(_sceneData);
        }
    }
}