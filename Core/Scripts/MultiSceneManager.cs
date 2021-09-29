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

namespace JTools.MultiScene
{
    public class MultiSceneManager : MonoBehaviour
    {
        public SceneGroup scenesToLoad;
        public UnityEvent BeforeSceneLoaded;

        private void Awake()
        {
            BeforeSceneLoaded?.Invoke();
            LoadScenes();
        }
        
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= CallGroupLoadedListeners;
            StopAllCoroutines();
        }

        public void CallGroupLoadedListeners(Scene s, LoadSceneMode l)
        {
            if (!s.name.Equals(scenesToLoad.scenes[scenesToLoad.scenes.Count - 1]))
                return;

            var _list = SceneElly.GetComponentsFromAllScenes<IMultiSceneAwake>();
            StartCoroutine(CallMultiSceneAwake(_list));
            SceneManager.sceneLoaded -= CallGroupLoadedListeners;
        }

        public void CallOnAwake()
        {
            var _list = SceneElly.GetComponentsFromAllScenes<IMultiSceneAwake>();
            StartCoroutine(CallMultiSceneAwake(_list));
        }
        
        public void CallOnStart()
        {
            var _list = SceneElly.GetComponentsFromAllScenes<IMultiSceneStart>();
            StartCoroutine(CallMultiSceneStart(_list));
        }


        private IEnumerator CallMultiSceneAwake(List<IMultiSceneAwake> listeners)
        {
            yield return new WaitForEndOfFrame();
            
            foreach (var _l in listeners)
                _l.OnMultiSceneAwake();

            StartCoroutine(CallMultiSceneStart(SceneElly.GetComponentsFromAllScenes<IMultiSceneStart>()));
        }
        
        private IEnumerator CallMultiSceneStart(List<IMultiSceneStart> listeners)
        {
            yield return new WaitForEndOfFrame();
            
            foreach (var _l in listeners)
                _l.OnMultiSceneStart();
        }


        public void UnloadAllActiveScenes()
        {
            var _scenes = new List<string>();

            for (var i = SceneManager.sceneCount - 1; i >= 0; i--)
                _scenes.Add(SceneManager.GetSceneAt(i).name);

            foreach (var _s in _scenes)
                SceneManager.UnloadSceneAsync(_s);
        }


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
                    SceneManager.sceneLoaded += CallGroupLoadedListeners;
                    
                if (_scenes.Contains(_s)) continue;
                
                if (i.Equals(0))
                    SceneManager.LoadSceneAsync(_s, LoadSceneMode.Single);
                else
                    SceneManager.LoadSceneAsync(_s, LoadSceneMode.Additive);
            }
        }
    }
}