/*
 * Copyright (c) 2024 Carter Games
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

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

        private static List<OrderedListenerData<IMultiSceneAwake>> awakeOrderedListeners;
        private static List<OrderedListenerData<IMultiSceneEnable>> enableOrderedListeners;
        private static List<OrderedListenerData<IMultiSceneStart>> startOrderedListeners;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Assigns the sorted lists for use. 
        /// </summary>
        private static void GetSortedListeners()
        {
            awakeOrderedListeners = OrderedHandler.OrderListeners(MultiSceneRef.GetComponentsFromAllScenes<IMultiSceneAwake>(), AwakeMethodName); 
            enableOrderedListeners = OrderedHandler.OrderListeners(MultiSceneRef.GetComponentsFromAllScenes<IMultiSceneEnable>(), EnableMethodName); 
            startOrderedListeners = OrderedHandler.OrderListeners(MultiSceneRef.GetComponentsFromAllScenes<IMultiSceneStart>(), StartMethodName); 
        }
        
        
        /// <summary>
        /// Calls all the listeners, but only actually runs on the last scene to load...
        /// </summary>
        public static void CallListeners(Scene s, LoadSceneMode l)
        {
            MultiSceneManager.OnSceneLoaded.Raise(s.name);
            
            if (!s.name.Equals(MultiSceneManager.ActiveSceneGroup.scenes[MultiSceneManager.ActiveSceneGroup.scenes.Count - 1].sceneName))
            {
                return;
            }

            GetSortedListeners();

            typeof(MultiSceneManager).GetMethod("UpdateActiveSceneNames", BindingFlags.Static)?.Invoke(null, null);

            MultiSceneManager.MonoInstance.StartCoroutine(CallMultiSceneListeners());
            SceneManager.sceneLoaded -= CallListeners;
        }


        private static IEnumerator CallMultiSceneListeners()
        {
            yield return CallListeners(awakeOrderedListeners, "OnMultiSceneAwake");
            yield return null;
            yield return CallListeners(enableOrderedListeners, "OnMultiSceneEnable");
            yield return null;
            yield return CallListeners(startOrderedListeners, "OnMultiSceneStart");
        }


        private static IEnumerator CallListeners<T>(List<OrderedListenerData<T>> orderedListeners, string methodName)
        {
            var count = 0;

            if (methodName.Equals(string.Empty))
            {
                MultiSceneLogger.Warning("Unable to find the interface type to send listeners for... skipping.");
                yield break;
            }
            
            if (orderedListeners.Count <= 0) yield break;

            foreach (var listener in orderedListeners)
            {
                listener.Listener.GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance)?.Invoke(listener.Listener, null);
                count++;

                if (count < AssetAccessor.GetAsset<AssetGlobalRuntimeSettings>().ListenerFrequency) continue;
                
                count = 0;
                yield return null;
            }
        }
    }
}