using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; 

namespace SceneLoadSystem.Runtime
{
    public class SceneLoader : MonoBehaviour
    {
        [Header("Scene Load Settings")]
        [Space(10)] 
        [SerializeField] private bool _loadOnAwake = false;  
        [SerializeField] private bool _unLoadSceneIfAlreadyLoaded = false;
        [SerializeField] private LoadSceneMode _loadSceneMode = LoadSceneMode.Additive;
        private Scene _loadedScene;
        private Coroutine _loadingCoroutine;
        [SerializeField, HideInInspector] private int _selectedSceneIndex = 0;
        
        private Action OnSceneStartToLoad;
        private Action OnSceneLoadCompleted;

        public int SelectedSceneIndex
        {
            get => _selectedSceneIndex;
            set => _selectedSceneIndex = value;
        }

        private void Awake()
        {
            if (!_loadOnAwake) return;
            LoadScene();
        }

        public void LoadScene()
        {
            if (_loadingCoroutine != null) return;
            OnSceneStartToLoad?.Invoke();
            _loadingCoroutine = StartCoroutine(LoadSceneAsync());
        }

        private IEnumerator LoadSceneAsync()
        {
            yield return null;
            if (_unLoadSceneIfAlreadyLoaded && _loadedScene != null)
                SceneManager.UnloadSceneAsync(_loadedScene); 
            
            SceneManager.sceneLoaded += OnSceneLoaded;
            AsyncOperation operation = SceneManager.LoadSceneAsync(SelectedSceneIndex, _loadSceneMode);
            
            if (operation != null) yield break;
            Debug.LogError("Failed To Load: " + SelectedSceneIndex);
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
            _loadedScene = scene;

            OnSceneLoadCompleted?.Invoke();
            SceneManager.sceneLoaded -= OnSceneLoaded;
            _loadingCoroutine = null;
        }
    }
}


