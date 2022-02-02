/*
 * 
 *  Multi-Scene Workflow
 *							  
 *	Multi-Scene Manager
 *      Handles the loading and unloading of scenes.
 *			
 *  Written by:
 *      Jonathan Carter
 *		
 *	Last Updated: 05/11/2021 (d/m/y)							
 * 
 */

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

        public SceneGroup defaultGroup;
        public UnityEvent BeforeScenesLoaded;
        public UnityEvent PostSceneLoaded;

        public bool LoadOnAwake
        {
            get => loadOnAwake;
            set => loadOnAwake = value;
        }
        
        public static Action<string> OnSceneLoaded;
        public static Action<SceneGroup> OnSceneGroupLoaded;
        public SceneGroup GetActiveGroup => activeSceneGroup;
        


        /// <summary>
        /// Checks to see if the scene by the name entered currently active
        /// </summary>
        /// <param name="sceneName">The scene to find</param>
        /// <returns>Bool</returns>
        public static bool IsSceneLoaded(string sceneName)
        {
            if (!main.hasCachedScenesList)
                main.GetActiveSceneNames();

            return main.cachedActiveSceneNames.Contains(sceneName);
        }
        
        
        /// <summary>
        /// Checks to see if the scene by the name entered is in the group entered
        /// </summary>
        /// <param name="sceneName">The scene to find</param>
        /// <returns>Bool</returns>
        public static bool IsSceneInGroup(string sceneName)
        {
            return main.activeSceneGroup.scenes.Contains(sceneName);
        }

        /// <summary>
        /// Checks to see if the scene by the name entered is in the group entered
        /// </summary>
        /// <param name="group">The group to check in</param>
        /// <param name="sceneName">The scene to find</param>
        /// <returns>Bool</returns>
        public static bool IsSceneInGroup(SceneGroup group, string sceneName)
        {
            return group.scenes.Contains(sceneName);
        }

        /// <summary>
        /// Gets the list of active scenes and returns them as a string. 
        /// </summary>
        public List<string> GetActiveSceneNames()
        {
            var _list = new List<string>();
            
            for (var i = SceneManager.sceneCount - 1; i >= 0; i--)
                _list.Add(SceneManager.GetSceneAt(i).name);

            cachedActiveSceneNames = _list;
            hasCachedScenesList = true;
            return cachedActiveSceneNames;
        }
        

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


        /// <summary>
        /// Sets the active group to the group entered...
        /// </summary>
        /// <param name="group"></param>
        public void SetGroup(SceneGroup group)
        {
            activeSceneGroup = group;
            GetActiveSceneNames();
        }
        
        
        /// <summary>
        /// Calls all the listeners, but only actually runs on the last scene to load...
        /// </summary>
        private void CallListeners(Scene s, LoadSceneMode l)
        {
            OnSceneLoaded?.Invoke(s.name);
            
            if (!s.name.Equals(activeSceneGroup.scenes[activeSceneGroup.scenes.Count - 1]))
                return;

            GetSortedListeners();
            GetActiveSceneNames();
            
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
                _l.listener.OnMultiSceneAwake();

            StartCoroutine(CallMultiSceneEnable());
        }
        
        
        /// <summary>
        /// Calls all IMultiSceneEnable implementations in the project. 
        /// </summary>
        private IEnumerator CallMultiSceneEnable()
        {
            yield return new WaitForEndOfFrame();
            
            foreach (var _l in enableOrderedListeners)
                _l.listener.OnMultiSceneEnable();

            StartCoroutine(CallMultiSceneStart());
        }
        
        
        /// <summary>
        /// Calls all IMultiSceneStart implementations in the project. 
        /// </summary>
        private IEnumerator CallMultiSceneStart()
        {
            yield return new WaitForEndOfFrame();
            
            foreach (var _l in startOrderedListeners)
                _l.listener.OnMultiSceneStart();
            
            PostSceneLoaded?.Invoke();
            OnSceneGroupLoaded?.Invoke(activeSceneGroup);
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


        /// <summary>
        /// Loads the scenes in the inspector selected scene group... overriding any existing active scenes. 
        /// </summary>
        public void LoadScenes()
        {
            activeSceneGroup = defaultGroup;
            var _scenes = new List<string>();

            for (var i = 0; i < SceneManager.sceneCount; i++)
                _scenes.Add(SceneManager.GetSceneAt(i).name);

            for (var i = 0; i < activeSceneGroup.scenes.Count; i++)
            {
                var _s = activeSceneGroup.scenes[i];

                if (i.Equals(activeSceneGroup.scenes.Count - 1))
                    SceneManager.sceneLoaded += CallListeners;
                    
                if (_scenes.Contains(_s)) continue;
                
                if (i.Equals(0))
                    SceneManager.LoadSceneAsync(_s, LoadSceneMode.Single);
                else
                    SceneManager.LoadSceneAsync(_s, LoadSceneMode.Additive);
            }
            
            GetActiveSceneNames();
        }
        
        /// <summary>
        /// Loads the scenes in the selected scene group...overriding any existing active scenes. 
        /// </summary>
        public void LoadScenes(SceneGroup group)
        {
            activeSceneGroup = group;
            var _scenes = new List<string>();

             for (var i = 0; i < SceneManager.sceneCount; i++)
                 _scenes.Add(SceneManager.GetSceneAt(i).name);
            

            for (var i = 0; i < activeSceneGroup.scenes.Count; i++)
            {
                var _s = activeSceneGroup.scenes[i];

                if (i.Equals(activeSceneGroup.scenes.Count - 1))
                    SceneManager.sceneLoaded += CallListeners;
                    
                if (_scenes.Contains(_s)) continue;
                
                if (i.Equals(0))
                    SceneManager.LoadSceneAsync(_s, LoadSceneMode.Single);
                else
                    SceneManager.LoadSceneAsync(_s, LoadSceneMode.Additive);
            }
            
            GetActiveSceneNames();
        }
        
        
        /// <summary>
        /// Loads the scenes in the selected scene group but leaves the base scene as is...
        /// </summary>
        public void LoadScenesKeepBase(SceneGroup group)
        {
            activeSceneGroup = group;
            var _activeScene = SceneManager.GetActiveScene().name;
            
            UnloadAllAdditiveScenes();

            for (var i = 0; i < activeSceneGroup.scenes.Count; i++)
            {
                var _s = activeSceneGroup.scenes[i];

                if (i.Equals(activeSceneGroup.scenes.Count - 1))
                {
                    SceneManager.sceneLoaded += CallListeners;
                }

                if (_s.Equals(_activeScene)) continue;
                
                if (i.Equals(0))
                    SceneManager.LoadSceneAsync(_s, LoadSceneMode.Single);
                else
                    SceneManager.LoadSceneAsync(_s, LoadSceneMode.Additive);
            }
            
            GetActiveSceneNames();
        }
    }
}