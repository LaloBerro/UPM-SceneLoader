﻿using ScenesLoaderSystem.Core.Domain;
using UnityEngine;

namespace ScenesLoaderSystem.Core.InterfaceAdapters
{
    [CreateAssetMenu(fileName = "SceneData", menuName = "ScriptableObjects/SceneData")]
    public class SceneDataSO : ScriptableObject
    {
        [Header("Config")]
        [SerializeField] private string _sceneName;
        [SerializeField] private bool _hasToUseLoadingScreen; 
        [SerializeField] private bool _isLockedScene;
        [SerializeField] private bool _hasToRemoveLockedScenes;
        [SerializeField] private bool _isPrincipal;
        [SerializeField] private bool _hasToCloseOthersScenes;
        [SerializeField] private bool _hasToKeepOpen;
        
        [Header("References")]
        [SerializeField] private SceneDataSO[] _scenesDataToOpen;
        [SerializeField] private SceneDataSO[] _scenesDataToRemove;
        
        private SceneData _currentSceneData;

        public SceneData GetSceneData()
        {
            if (!ReferenceEquals(_currentSceneData, null)) 
                return _currentSceneData;
            
            SceneData[] sceneDatasToOpen = GetSceneData(_scenesDataToOpen);
            SceneData[] sceneDatasToRemove = GetSceneData(_scenesDataToRemove);
                
            _currentSceneData = new SceneData(_sceneName, _hasToUseLoadingScreen, _isLockedScene, _hasToRemoveLockedScenes,
                _isPrincipal, _hasToCloseOthersScenes, _hasToKeepOpen, sceneDatasToOpen, sceneDatasToRemove);

            return _currentSceneData;
        }

        private SceneData[] GetSceneData(SceneDataSO[] sceneDataSo)
        {
            int totalScenesData = sceneDataSo.Length;
            SceneData[] scenesData = new SceneData[totalScenesData];

            for (int i = 0; i < totalScenesData; i++)
            {
                scenesData[i] = sceneDataSo[i].GetSceneData();
            }

            return scenesData;
        }
    }
}