using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SingletonPattern;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScenesLoaderSystem
{
    [Serializable]
    public class ScenesLoader : MonoSingleton<ScenesLoader>
    {
        #region Vars

        [Header("Header")]
        [SerializeField] private SceneDataSO loadingScreenData;
        [SerializeField] private SceneDataSO firstOpenSceneData;

        [Header("Debug")]
        [SerializeField] private int _progress;
        [SerializeField] private SceneData _currentSceneData;
        [SerializeField] private List<SceneData> _openScenes = new List<SceneData>();

        private List<AsyncOperation> _operations;

        public int Progress { get => _progress; }

        #endregion

        #region Methods

        #region Init Methods

        private void Awake()
        {
            InitializeSingleton();

            PassTroughScenes();

            InitVariables();

            _openScenes.Add(firstOpenSceneData.SceneData);
        }

        private void InitVariables()
        {
            _operations = new List<AsyncOperation>();
        }

        #endregion

        #region Load Scenes

        /// <summary>
        /// Set and run the SceneLoaderData
        /// </summary>
        /// <param name="sceneData"></param>
        public async void LoadScene(SceneDataSO sceneData)
        {
            _operations.Clear();

            _currentSceneData = sceneData.SceneData;

            await LoadProcessAsync();
        }

        private async Task LoadProcessAsync()
        {
            await LoadLoadingScreenAsync();

            RemoveOpenScenes();

            OpenScenes();

            await WaitToAllOperationsDoneAsync();

            UnloadLoadingScreen();

            OnAllOperationsDone();
        }

        private async Task LoadLoadingScreenAsync()
        {
            if (_currentSceneData.useLoadingScreen == false) return;

            AsyncOperation loadLoadingOperation = SceneManager.LoadSceneAsync(loadingScreenData.SceneData.nameScene, LoadSceneMode.Additive);

            while (!loadLoadingOperation.isDone)
            {
                await Task.Delay(1);
            }
        }

        #region Remove Scene

        private void RemoveOpenScenes()
        {
            for (int i = 0; i < _openScenes.Count; i++)
            {
                if (CantRemoveThisScene(_openScenes[i])) continue;

                RemoveScene(_openScenes[i]);

                i--;
            }
        }

        private void RemoveScene(SceneData openScene)
        {
            AsyncOperation removeSceneOperation = SceneManager.UnloadSceneAsync(openScene.nameScene);

            _operations.Add(removeSceneOperation);

            _openScenes.Remove(openScene);
        }

        private bool CantRemoveThisScene(SceneData sceneData)
        {
            return sceneData.isLockedScene && !_currentSceneData.removeLockedScenes;
        }

        #endregion

        #region Open Scene

        private void OpenScenes()
        {
            SceneData[] scenesToLoad = _currentSceneData.GetAllScenesToOpen();

            foreach (var sceneData in scenesToLoad)
            {
                if (IsThisSceneOpen(sceneData)) continue;

                OpenScene(sceneData);
            }
        }

        private bool IsThisSceneOpen(SceneData sceneData)
        {
            return _openScenes.Contains(sceneData);
        }

        private void OpenScene(SceneData sceneData)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneData.nameScene, LoadSceneMode.Additive);

            _operations.Add(operation);

            _openScenes.Add(sceneData);
        }

        #endregion

        private async Task WaitToAllOperationsDoneAsync()
        {
            int operationsCount = _operations.Count;
            float totalProgress = 0;

            foreach (var operation in _operations)
                while (!operation.isDone)
                {
                    totalProgress += operation.progress;

                    _progress = Mathf.RoundToInt(totalProgress / operationsCount);

                    await Task.Delay(1);
                }
        }

        private void UnloadLoadingScreen()
        {
            if (_currentSceneData.useLoadingScreen)
                SceneManager.UnloadSceneAsync(loadingScreenData.SceneData.nameScene);
        }

        private void OnAllOperationsDone()
        {
            Debug.Log("All scene are loaded");
        }

        #endregion

        #endregion
    }
}