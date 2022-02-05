// Multi Scene - Core
// Main manager class which handles scene management for the package
// Author: Jonathan Carter - https://carter.games

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace MultiScene.Core
{
    public class MultiSceneManager : MonoBehaviour
    {
        [SerializeField] private bool loadOnAwake;
        
        private List<string> cachedActiveSceneNames;
        private bool hasCachedScenesList;
        private SceneGroup activeSceneGroup;
        
        private List<OrderedListenerData<IMultiSceneAwake>> awakeOrderedListeners;
        private List<OrderedListenerData<IMultiSceneEnable>> enableOrderedListeners;
        private List<OrderedListenerData<IMultiSceneStart>> startOrderedListeners;

        private static MultiSceneManager main;

        
        /// <summary>
        /// The default scene group to load
        /// </summary>
        public SceneGroup defaultGroup;
        
        /// <summary>
        /// Runs before a scene group loads
        /// </summary>
        public UnityEvent BeforeScenesLoaded;
        
        /// <summary>
        /// Runs after a scene group has loaded & all listeners have been called
        /// </summary>
        public UnityEvent PostSceneLoaded;

        /// <summary>
        /// Runs when each scene is loaded
        /// </summary>
        public static Action<string> OnSceneLoaded;
        
        /// <summary>
        /// Runs when a scene group has loaded
        /// </summary>
        public static Action<SceneGroup> OnSceneGroupLoaded;
        
        
        #region Getters / Setters
        
        public bool LoadOnAwake
        {
            get => loadOnAwake;
            set => loadOnAwake = value;
        }
        
        public static SceneGroup GetActiveGroup => main.activeSceneGroup;
        public static bool IsSceneInGroup(string sceneName) => main.activeSceneGroup.scenes.Contains(sceneName);
        public static bool IsSceneInGroup(SceneGroup group, string sceneName) => group.scenes.Contains(sceneName);
        
        
        public static bool IsSceneLoaded(string sceneName)
        {
            if (!main.hasCachedScenesList)
                main.UpdateActiveSceneNames();

            return main.cachedActiveSceneNames.Contains(sceneName);
        }
        
        
        /// <summary>
        /// Sets the active group to the group entered...
        /// </summary>
        /// <param name="group">the group to set to</param>
        public void SetGroup(SceneGroup group)
        {
            activeSceneGroup = group;
            UpdateActiveSceneNames();
        }
        
        
        #endregion
        

        /// <summary>
        /// Gets the list of active scenes and returns them as a string. 
        /// </summary>
        public List<string> UpdateActiveSceneNames()
        {
            var _list = new List<string>();
            
            for (var i = SceneManager.sceneCount - 1; i >= 0; i--)
                _list.Add(SceneManager.GetSceneAt(i).name);

            cachedActiveSceneNames = _list;
            hasCachedScenesList = true;
            return cachedActiveSceneNames;
        }
        
        
        #region Unity Methods
        
        private void Awake()
        {
            if (main == null)
                main = this;
            else
                Destroy(this);
            
            if (!LoadOnAwake) return;
            activeSceneGroup = defaultGroup;
            BeforeScenesLoaded?.Invoke();
            LoadScenes();
        }
        
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= CallListeners;
            StopAllCoroutines();
        }
        
        #endregion
        
        
        #region Listener Handling
        
        
        /// <summary>
        /// Calls all the listeners, but only actually runs on the last scene to load...
        /// </summary>
        private void CallListeners(Scene s, LoadSceneMode l)
        {
            OnSceneLoaded?.Invoke(s.name);
            
            if (!s.name.Equals(activeSceneGroup.scenes[activeSceneGroup.scenes.Count - 1]))
                return;

            GetSortedListeners();
            UpdateActiveSceneNames();
            
            StartCoroutine(CallMultiSceneAwake());
            SceneManager.sceneLoaded -= CallListeners;
        }
        

        private void GetSortedListeners()
        {
            awakeOrderedListeners = OrderedHandler.OrderListeners(MultiSceneElly.GetComponentsFromAllScenes<IMultiSceneAwake>(), "OnMultiSceneAwake"); 
            enableOrderedListeners = OrderedHandler.OrderListeners(MultiSceneElly.GetComponentsFromAllScenes<IMultiSceneEnable>(), "OnMultiSceneEnable"); 
            startOrderedListeners = OrderedHandler.OrderListeners(MultiSceneElly.GetComponentsFromAllScenes<IMultiSceneStart>(), "OnMultiSceneStart"); 
        }


        /// <summary>
        /// Calls all IMultiSceneAwake implementations in the project.
        /// </summary>
        private IEnumerator CallMultiSceneAwake()
        {
            yield return new WaitForEndOfFrame();
            
            foreach (var _l in awakeOrderedListeners)
                _l.Listener.OnMultiSceneAwake();

            StartCoroutine(CallMultiSceneEnable());
        }
        
        
        /// <summary>
        /// Calls all IMultiSceneEnable implementations in the project. 
        /// </summary>
        private IEnumerator CallMultiSceneEnable()
        {
            yield return new WaitForEndOfFrame();
            
            foreach (var _l in enableOrderedListeners)
                _l.Listener.OnMultiSceneEnable();

            StartCoroutine(CallMultiSceneStart());
        }
        
        
        /// <summary>
        /// Calls all IMultiSceneStart implementations in the project. 
        /// </summary>
        private IEnumerator CallMultiSceneStart()
        {
            yield return new WaitForEndOfFrame();
            
            foreach (var _l in startOrderedListeners)
                _l.Listener.OnMultiSceneStart();
            
            PostSceneLoaded?.Invoke();
            OnSceneGroupLoaded?.Invoke(activeSceneGroup);
        }
        
        
        #endregion

        
        #region Scene Management
        
        /// <summary>
        /// Loads the scenes in the inspector selected scene group... overriding any existing active scenes. 
        /// </summary>
        public void LoadScenes() => LoadScenes(defaultGroup);
        
        /// <summary>
        /// Loads the scenes in the inspector selected scene group... overriding any existing active scenes. 
        /// </summary>
        public void LoadScenes(SceneGroup group) => RunSceneLoading(group);
        
        /// <summary>
        /// Loads the scenes in the selected scene group but leaves the base scene as is...
        /// </summary>
        public void LoadScenesKeepBase(SceneGroup group) => RunSceneLoading(group, true);
        
        /// <summary>
        /// Loads the scenes in the selected scene group...overriding any existing active scenes. 
        /// </summary>
        private void RunSceneLoading(SceneGroup group, bool? keepBase = false)
        {
            activeSceneGroup = group;
            
            var _scenes = new List<string>();
            var _baseScene = SceneManager.GetActiveScene().name;

            if (keepBase == true)
                UnloadAllAdditiveScenes();
            else
            {
                UnloadAllActiveScenes();

                for (var i = 0; i < SceneManager.sceneCount; i++)
                    _scenes.Add(SceneManager.GetSceneAt(i).name);
            }
            
            for (var i = 0; i < activeSceneGroup.scenes.Count; i++)
            {
                var _s = activeSceneGroup.scenes[i];

                if (i.Equals(activeSceneGroup.scenes.Count - 1))
                    SceneManager.sceneLoaded += CallListeners;
                   
                if (keepBase == true)
                    if (_s.Equals(_baseScene)) continue;
               
                if (_scenes.Contains(_s)) continue;

                SceneManager.LoadSceneAsync(_s, i.Equals(0) 
                    ? LoadSceneMode.Single 
                    : LoadSceneMode.Additive);
                
                OnSceneLoaded?.Invoke(_s);
            }
            
            OnSceneGroupLoaded?.Invoke(activeSceneGroup);
            UpdateActiveSceneNames();
        }
        
        /// <summary>
        /// Unloads all the active scenes
        /// </summary>
        public void UnloadAllActiveScenes()
        {
            var _scenes = new List<string>();

            for (var i = SceneManager.sceneCount - 1; i >= 0; i--)
                _scenes.Add(SceneManager.GetSceneAt(i).name);

            foreach (var _s in _scenes)
                SceneManager.UnloadSceneAsync(_s);
        }
        
        /// <summary>
        /// Unloads all the additive scenes loaded but keeps the base scene as is unless overridden with a load method...
        /// </summary>
        public void UnloadAllAdditiveScenes()
        {
            var _scenes = new List<string>();
            var _activeScene = SceneManager.GetActiveScene().name;

            for (var i = SceneManager.sceneCount - 1; i >= 0; i--)
            {
                if (SceneManager.GetSceneAt(i).name.Equals(_activeScene)) continue;
                _scenes.Add(SceneManager.GetSceneAt(i).name);
            }

            foreach (var _s in _scenes)
                SceneManager.UnloadSceneAsync(_s);
        }
        
        #endregion
    }
}