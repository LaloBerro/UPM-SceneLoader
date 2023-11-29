using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommandQueues.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScenesLoaderSystem.Core.Domain
{
    public class SceneLoader : ISceneLoader
    {
        private readonly SceneData _loadingScreenSceneData;
        private readonly SceneData _emptySceneData;

        private SceneData _currentSceneData;
        private List<SceneData> _openScenes = new List<SceneData>();
        private SceneRemover _sceneRemover = new SceneRemover();
        private Queue<SceneData> _scenesToOpenQueue;
        private List<INodeCommand> _nodeCommands;
        private CommandQueue _commandQueue;

        public Action OnAllScenesAreLoaded { get; set; }

        public SceneLoader(SceneData loadingScreenSceneData, SceneData firstOpenSceneData, SceneData emptySceneData)
        {
            _loadingScreenSceneData = loadingScreenSceneData;
            _emptySceneData = emptySceneData;

            _openScenes.Add(firstOpenSceneData);
        }

        public async void RemoveCurrentAndSetPrincipal(SceneData currentSceneData)
        {
            await LoadLoadingScreenAsync();

            await _sceneRemover.RemoveScene(_currentSceneData);
            _openScenes.Remove(_currentSceneData);

            _currentSceneData = currentSceneData;

            AllSceneLoaded();
        }

        public async void LoadScene(SceneData sceneData, bool dontRemoveOpenScenes = false)
        {
            if (string.IsNullOrEmpty(sceneData.SceneName))
                throw new Exception("SceneLoader Error: Trying to Load a Scene with empty name.");
            
            _currentSceneData = sceneData;

            await LoadLoadingScreenAsync();

            bool hasToCloseOtherScenes = sceneData.HasToCloseOthersScenes;
            bool mustRemoveOpenScenes = !dontRemoveOpenScenes;

            if (hasToCloseOtherScenes && mustRemoveOpenScenes)
                _openScenes = await _sceneRemover.RemoveScenes(_openScenes, _currentSceneData.HasToRemoveLockedScenes);

            await RemoveScenesFromCurrentSceneData();

            OpenScenes();
        }

        private async Task RemoveScenesFromCurrentSceneData()
        {
            SceneData[] sceneDatas = _currentSceneData.GetAllScenesDataToRemove();

            if (ReferenceEquals(sceneDatas, null))
                return;

            foreach (var sceneData in sceneDatas)
            {
                if (!_openScenes.Contains(sceneData))
                    continue;

                await _sceneRemover.RemoveScene(sceneData);
                _openScenes.Remove(sceneData);
            }
        }

        private async Task LoadLoadingScreenAsync()
        {
            if (_currentSceneData.HasToUseLoadingScreen == false)
                return;

            await LoadSceneAsync(_loadingScreenSceneData.SceneName);
        }
        
        private async Task LoadSceneAsync(string sceneName)
        {
            AsyncOperation loadLoadingOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            while (!loadLoadingOperation.isDone)
            {
                await Task.Yield();
            }
        }

        private void OpenScenes()
        {
            SceneData[] scenesToLoad = _currentSceneData.GetAllScenesToOpen();
            _scenesToOpenQueue = new Queue<SceneData>();
            _nodeCommands = new List<INodeCommand>();

            foreach (var sceneData in scenesToLoad)
            {
                _scenesToOpenQueue.Enqueue(sceneData);
            }

            OpenNextScene();
        }

        private void OpenNextScene()
        {
            while (true)
            {
                if (_scenesToOpenQueue.Count <= 0) 
                    return;

                SceneData sceneData = _scenesToOpenQueue.Dequeue();

                if (_openScenes.Contains(sceneData))
                    continue;
                
                OpenScene(sceneData);
                break;
            }
        }

        private void OpenScene(SceneData sceneData)
        {
            SceneManager.LoadSceneAsync(sceneData.SceneName, LoadSceneMode.Additive);

            _openScenes.Add(sceneData);
        }

        public async void SetNodeCommandOfALoadedScene(INodeCommand nodeCommand)
        {
            //Added for security reasons because not always load the scene correctly, so we need to wait the main thread
            await Task.Delay(10);

            if (nodeCommand != null)
                _nodeCommands.Add(nodeCommand);

            if (_scenesToOpenQueue.Count <= 0)
            {
                InitializeScenes();
                return;
            }

            OpenNextScene();
        }

        private void InitializeScenes()
        {
            if (_nodeCommands.Count <= 0)
            {
                AllSceneLoaded();
                return;
            }

            _commandQueue = new CommandQueue(_nodeCommands.ToArray());
            _commandQueue.OnExecutionDone += AllSceneLoaded;
            _commandQueue.Execute();
        }

        private async void AllSceneLoaded()
        {
            if (_commandQueue != null)
                _commandQueue.OnExecutionDone -= AllSceneLoaded;

            SetPrincipalScene();

            //Added for security reasons because not always load the scene correctly, so we need to wait the main thread
            await Task.Delay(10);

            UnloadTransitionScenes();

            OnAllScenesAreLoaded?.Invoke();
        }

        private void UnloadTransitionScenes()
        {
            int totalScene = SceneManager.sceneCount;
            for (int i = 0; i < totalScene; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);

                if (scene.name == _loadingScreenSceneData.SceneName || scene.name == _emptySceneData.SceneName)
                    SceneManager.UnloadSceneAsync(scene);
            }
        }

        private void SetPrincipalScene()
        {
            foreach (var sceneData in _openScenes)
            {
                if (!sceneData.IsPrincipal)
                    continue;

                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneData.SceneName));
            }
        }

        public bool IsThisSceneDataOpened(SceneData sceneData)
        {
            return _openScenes.Contains(sceneData);
        }

        public async void ReloadCurrentScene()
        {
            await LoadSceneAsync(_loadingScreenSceneData.SceneName);
            await LoadSceneAsync(_emptySceneData.SceneName);

            _openScenes = await _sceneRemover.RemoveScenes(_openScenes, _currentSceneData.HasToRemoveLockedScenes);
            
            LoadScene(_currentSceneData);
        }
    }
}