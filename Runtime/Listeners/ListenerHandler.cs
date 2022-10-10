using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using CarterGames.Experimental.MultiScene.Editor;
using UnityEngine.SceneManagement;

namespace CarterGames.Experimental.MultiScene
{
    /// <summary>
    /// Handles the logic for running the listeners when called.
    /// </summary>
    public static class ListenerHandler
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private const string AwakeMethodName = "OnMultiSceneAwake";
        private const string EnableMethodName = "OnMultiSceneEnable";
        private const string StartMethodName = "OnMultiSceneStart";


        private static List<OrderedListenerData<IMultiSceneAwake>> _awakeOrderedListeners;
        private static List<OrderedListenerData<IMultiSceneEnable>> _enableOrderedListeners;
        private static List<OrderedListenerData<IMultiSceneStart>> _startOrderedListeners;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Assigns the sorted lists for use. 
        /// </summary>
        private static void GetSortedListeners()
        {
            _awakeOrderedListeners = OrderedHandler.OrderListeners(MultiSceneRef.GetComponentsFromAllScenes<IMultiSceneAwake>(), AwakeMethodName); 
            _enableOrderedListeners = OrderedHandler.OrderListeners(MultiSceneRef.GetComponentsFromAllScenes<IMultiSceneEnable>(), EnableMethodName); 
            _startOrderedListeners = OrderedHandler.OrderListeners(MultiSceneRef.GetComponentsFromAllScenes<IMultiSceneStart>(), StartMethodName); 
        }
        
        
        /// <summary>
        /// Calls all the listeners, but only actually runs on the last scene to load...
        /// </summary>
        public static void CallListeners(Scene s, LoadSceneMode l)
        {
            MultiSceneManager.OnSceneLoaded.Raise(s.name);
            
            if (!s.name.Equals(MultiSceneManager.ActiveSceneGroup.scenes[MultiSceneManager.ActiveSceneGroup.scenes.Count - 1].sceneName))
                return;

            GetSortedListeners();

            typeof(MultiSceneManager).GetMethod("UpdateActiveSceneNames", BindingFlags.Static)?.Invoke(null, null);

            MultiSceneManager.Mono.StartCoroutine(CallMultiSceneAwake());
            SceneManager.sceneLoaded -= CallListeners;
        }

        
        /// <summary>
        /// Calls all IMultiSceneAwake implementations in the project.
        /// </summary>
        private static IEnumerator CallMultiSceneAwake()
        {
            var count = 0;

            for (var i = 0; i < _awakeOrderedListeners.Count; i++)
            {
                _awakeOrderedListeners[i].Listener.OnMultiSceneAwake();
                count++;

                if (count < AssetAccessor.GetAsset<MultiSceneSettingsAsset>().ListenerFrequency) continue;
                count = 0;
                yield return null;
            }

            MultiSceneManager.Mono.StartCoroutine(CallMultiSceneEnable());
        }
        
        
        /// <summary>
        /// Calls all IMultiSceneEnable implementations in the project. 
        /// </summary>
        private static IEnumerator CallMultiSceneEnable()
        {
            var count = 0;

            for (var i = 0; i < _enableOrderedListeners.Count; i++)
            {
                _enableOrderedListeners[i].Listener.OnMultiSceneEnable();
                count++;
                
                if (count < AssetAccessor.GetAsset<MultiSceneSettingsAsset>().ListenerFrequency) continue;
                count = 0;
                yield return null;
            }

            MultiSceneManager.Mono.StartCoroutine(CallMultiSceneStart());
        }
        
        
        /// <summary>
        /// Calls all IMultiSceneStart implementations in the project. 
        /// </summary>
        private static IEnumerator CallMultiSceneStart()
        {
            var count = 0;
            
            for (var i = 0; i < _startOrderedListeners.Count; i++)
            {
                _startOrderedListeners[i].Listener.OnMultiSceneStart();
                count++;
                
                if (count < AssetAccessor.GetAsset<MultiSceneSettingsAsset>().ListenerFrequency) continue;
                count = 0;
                yield return null;
            }

            MultiSceneManager.PostScenesLoaded.Raise();
        }
    }
}