// ----------------------------------------------------------------------------
// MultiSceneLoad.cs
// 
// Author: Jonathan Carter (A.K.A. J)
// Date: 31/08/2021
// ----------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace MultiScene
{
    public class MultiSceneManager : MonoBehaviour
    {
        private List<string> cachedActiveSceneNames;
        private bool hasCachedScenesList;
        
        public SceneGroup scenesToLoad;
        public UnityEvent BeforeScenesLoaded;
        public UnityEvent PostSceneLoaded;


        /// <summary>
        /// Checks to see if the scene by the name entered currently active
        /// </summary>
        /// <param name="sceneName">The scene to find</param>
        /// <returns>Bool</returns>
        public bool IsSceneLoaded(string sceneName)
        {
            if (!hasCachedScenesList)
                GetActiveSceneNames();

            foreach (var s in cachedActiveSceneNames)
            {
                if (SceneManager.GetSceneByName(s).name.Equals(sceneName))
                    return true;
            }

            return false;
        }


        /// <summary>
        /// Checks to see if the scene by the name entered is in the group entered
        /// </summary>
        /// <param name="group">The group to check in</param>
        /// <param name="sceneName">The scene to find</param>
        /// <returns>Bool</returns>
        public bool IsSceneInGroup(SceneGroup group, string sceneName)
        {
            return group.scenes.Contains(sceneName);
        }

        private void GetActiveSceneNames()
        {
            var _list = new List<string>();
            
            for (var i = SceneManager.sceneCount - 1; i >= 0; i--)
                _list.Add(SceneManager.GetSceneAt(i).name);

            cachedActiveSceneNames = _list;
            hasCachedScenesList = true;
        }
        

        private void Awake()
        {
            BeforeScenesLoaded?.Invoke();
            LoadScenes();
        }
        
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= CallListeners;
            StopAllCoroutines();
        }

		
        /// <summary>
        /// Calls all the listeners 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="l"></param>
        /// <returns></returns>
        public void CallListeners(Scene s, LoadSceneMode l)
        {
            if (!s.name.Equals(scenesToLoad.scenes[scenesToLoad.scenes.Count - 1]))
                return;

            var _list = SceneElly.GetComponentsFromAllScenes<IMultiSceneAwake>();
            StartCoroutine(CallMultiSceneAwake(_list));
            SceneManager.sceneLoaded -= CallListeners;
        }
        

        /// <summary>
        /// Calls all IMultiSceneAwake implementations in the project.
        /// </summary>
        /// <param name="listeners">A list of all the awake listeners in the project</param>
        private IEnumerator CallMultiSceneAwake(List<IMultiSceneAwake> listeners)
        {
            yield return new WaitForEndOfFrame();
            
            foreach (var _l in listeners)
                _l.OnMultiSceneAwake();

            StartCoroutine(CallMultiSceneEnable(SceneElly.GetComponentsFromAllScenes<IMultiSceneEnable>()));
        }
        
        
        /// <summary>
        /// Calls all IMultiSceneEnable implementations in the project. 
        /// </summary>
        /// <param name="listeners">A list of all the enable listeners in the project</param>
        private IEnumerator CallMultiSceneEnable(List<IMultiSceneEnable> listeners)
        {
            yield return new WaitForEndOfFrame();
            
            foreach (var _l in listeners)
                _l.OnMultiSceneEnable();
            
            StartCoroutine(CallMultiSceneStart(SceneElly.GetComponentsFromAllScenes<IMultiSceneStart>()));
        }
        
        
        /// <summary>
        /// Calls all IMultiSceneStart implementations in the project. 
        /// </summary>
        /// <param name="listeners">A list of all the start listeners in the project</param>
        private IEnumerator CallMultiSceneStart(List<IMultiSceneStart> listeners)
        {
            yield return new WaitForEndOfFrame();
            
            foreach (var _l in listeners)
                _l.OnMultiSceneStart();
            
            PostSceneLoaded?.Invoke();
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
        /// Loads the scenes in the selected scene group...
        /// </summary>
        public void LoadScenes()
        {
            var _scenes = new List<string>();

            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                _scenes.Add(SceneManager.GetSceneAt(i).name);
            }

            for (var i = 0; i < scenesToLoad.scenes.Count; i++)
            {
                var _s = scenesToLoad.scenes[i];

                if (i.Equals(scenesToLoad.scenes.Count - 1))
                    SceneManager.sceneLoaded += CallListeners;
                    
                if (_scenes.Contains(_s)) continue;
                
                if (i.Equals(0))
                    SceneManager.LoadSceneAsync(_s, LoadSceneMode.Single);
                else
                    SceneManager.LoadSceneAsync(_s, LoadSceneMode.Additive);
            }
        }
        
        /// <summary>
        /// Loads the scenes in the selected scene group...
        /// </summary>
        public void LoadScenes(SceneGroup group)
        {
            var _scenes = new List<string>();

            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                _scenes.Add(SceneManager.GetSceneAt(i).name);
            }

            for (var i = 0; i < group.scenes.Count; i++)
            {
                var _s = group.scenes[i];

                if (i.Equals(group.scenes.Count - 1))
                    SceneManager.sceneLoaded += CallListeners;
                    
                if (_scenes.Contains(_s)) continue;
                
                if (i.Equals(0))
                    SceneManager.LoadSceneAsync(_s, LoadSceneMode.Single);
                else
                    SceneManager.LoadSceneAsync(_s, LoadSceneMode.Additive);
            }
        }
    }
}