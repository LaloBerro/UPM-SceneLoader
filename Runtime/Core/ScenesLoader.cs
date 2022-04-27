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
        [SerializeField] private SceneDataSO _loadingScreenData;
        [SerializeField] private SceneDataSO _firstOpenSceneData;

        [Header("Debug")]
        [SerializeField] private int _progress;
        [SerializeField] private SceneData _currentSceneData;
        [SerializeField] private List<SceneData> _openScenes = new List<SceneData>();

        private List<AsyncOperation> _operations;

        private SceneRemover _sceneRemover = new SceneRemover();

        public int Progress { get => _progress; }

        public Action OnAllScenesAreLoaded { get; set; }

        #endregion

        #region Methods

        #region Init Methods

        private void Awake()
        {
            InitializeSingleton();

            PassTroughScenes();

            InitVariables();

            _openScenes.Add(_firstOpenSceneData.SceneData);
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

            await Task.Delay(1);

            _openScenes = await _sceneRemover.RemoveScenes(_openScenes, _currentSceneData.removeLockedScenes);

            OpenScenes();

            await WaitToAllOperationsDoneAsync();

            SetPrincipalScene();

            UnloadLoadingScreen();

            OnAllOperationsDone();
        }

        private async Task LoadLoadingScreenAsync()
        {
            if (_currentSceneData.useLoadingScreen == false)
                return;

            AsyncOperation loadLoadingOperation = SceneManager.LoadSceneAsync(_loadingScreenData.SceneData.nameScene, LoadSceneMode.Additive);

            while (!loadLoadingOperation.isDone)
            {
                await Task.Delay(1);
            }
        }

        #region Open Scene

        private void OpenScenes()
        {
            SceneData[] scenesToLoad = _currentSceneData.GetAllScenesToOpen();

            OpenScenesByDatas(scenesToLoad);
        }

        private void OpenScenesByDatas(SceneData[] scenesToLoad)
        {
            foreach (var sceneData in scenesToLoad)
            {
                if (IsThisSceneOpen(sceneData))
                    continue;

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
            {
                while (!operation.isDone)
                {
                    totalProgress += operation.progress;

                    _progress = Mathf.RoundToInt(totalProgress / operationsCount);

                    await Task.Delay(1);
                }
            }
        }

        private void SetPrincipalScene()
        {
            foreach (var sceneData in _openScenes)
            {
                if (!sceneData.isPrincipal)
                    return;

                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneData.nameScene));
            }
        }

        private void UnloadLoadingScreen()
        {
            if (_currentSceneData.useLoadingScreen)
                SceneManager.UnloadSceneAsync(_loadingScreenData.SceneData.nameScene);
        }

        private void OnAllOperationsDone()
        {
            OnAllScenesAreLoaded?.Invoke();
            Debug.Log("All scene are loaded");
        }

        #endregion

        #endregion
    }
}