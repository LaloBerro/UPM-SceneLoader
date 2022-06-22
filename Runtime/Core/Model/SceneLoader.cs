using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommandQueues.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScenesLoaderSystem
{
    public class SceneLoader : ISceneLoader
    {
        private readonly SceneData _loadingScreenSceneData;

        private SceneData _currentSceneData;
        private List<SceneData> _openScenes = new List<SceneData>();
        private SceneRemover _sceneRemover = new SceneRemover();
        private Queue<SceneData> _scenesToOpenQueue;
        private List<INodeCommand> _nodeCommands;
        private CommandQueue _commandQueue;

        public Action OnAllScenesAreLoaded { get; set; }

        public SceneLoader(SceneData loadingScreenSceneData, SceneData firstOpenSceneData)
        {
            _loadingScreenSceneData = loadingScreenSceneData;

            _openScenes.Add(firstOpenSceneData);
        }

        public async void LoadScene(SceneData sceneData)
        {
            _currentSceneData = sceneData;

            await LoadLoadingScreenAsync();

            _openScenes = await _sceneRemover.RemoveScenes(_openScenes, _currentSceneData.removeLockedScenes);

            OpenScenes();
        }

        private async Task LoadLoadingScreenAsync()
        {
            if (_currentSceneData.useLoadingScreen == false)
                return;

            AsyncOperation loadLoadingOperation = SceneManager.LoadSceneAsync(_loadingScreenSceneData.Name, LoadSceneMode.Additive);

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
                Debug.Log("Add scene to opnes: " + sceneData.Name);
                _scenesToOpenQueue.Enqueue(sceneData);
            }

            OpenNextScene();
        }

        private void OpenNextScene()
        {
            SceneData sceneData = _scenesToOpenQueue.Dequeue();

            if (_openScenes.Contains(sceneData))
            {
                OpenNextScene();
                return;
            }

            OpenScene(sceneData);
        }

        private void OpenScene(SceneData sceneData)
        {
            SceneManager.LoadSceneAsync(sceneData.Name, LoadSceneMode.Additive);

            _openScenes.Add(sceneData);
        }

        public void SetNodeCommandOfALoadedScene(INodeCommand nodeCommand)
        {
            if (nodeCommand != null)
                _nodeCommands.Add(nodeCommand);

            Debug.Log("SetNodeCommandOfALoadedScene " + nodeCommand + " | count: " + _nodeCommands.Count);

            if (_scenesToOpenQueue.Count <= 0)
            {
                InitializeScenes();
                return;
            }

            OpenNextScene();
        }

        public void InitializeScenes()
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

        private void AllSceneLoaded()
        {
            if (_commandQueue != null)
                _commandQueue.OnExecutionDone -= AllSceneLoaded;

            SetPrincipalScene();

            if (_currentSceneData.useLoadingScreen)
                SceneManager.UnloadSceneAsync(_loadingScreenSceneData.Name);

            OnAllScenesAreLoaded?.Invoke();
        }

        private void SetPrincipalScene()
        {
            foreach (var sceneData in _openScenes)
            {
                if (!sceneData.isPrincipal)
                    continue;

                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneData.nameScene));
            }
        }
    }
}