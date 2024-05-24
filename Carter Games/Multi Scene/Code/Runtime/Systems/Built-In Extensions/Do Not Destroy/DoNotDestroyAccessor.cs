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

using System.Collections.Generic;
using System.Linq;
using CarterGames.Experimental.MultiScene.Editor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CarterGames.Experimental.MultiScene.DoNotDestroy
{
    /// <summary>
    /// Call to access a spy object and find the object of the type entered...
    /// </summary>
    public sealed class DoNotDestroyAccessor : MonoBehaviour
    { 
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private static DoNotDestroyAccessor instance;
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void Initialise()
        {
            if (instance != null) return;

            var obj = new GameObject("[Multi Scene]: Do Not Destroy Accessor");
            DontDestroyOnLoad(obj);
            obj.AddComponent<DoNotDestroyAccessor>();
        }
        
        
        private void Awake()
        {
            if (instance != null) Destroy(this.gameObject);
            instance = this;
        }
        

        /// <summary>
        /// Gets all the root gameObjects in the scene for use...
        /// </summary>
        /// <returns>A list of all the valid root gameObjects the spy can find.</returns>
        public static List<GameObject> GetRootGameObjectsInDoNotDestroy()
        {
            return instance.gameObject.scene.GetRootGameObjects().ToList();
        }
        
        
        /// <summary>
        /// Moves the object entered in the scene string entered...
        /// </summary>
        /// <param name="obj">The object to move</param>
        /// <returns>Was the move successful?</returns>
        public static void MoveObjectToSceneInDoNotDestroy(GameObject obj)
        {
            SceneManager.MoveGameObjectToScene(obj, instance.gameObject.scene);
        }
        
        
        /// <summary>
        /// Moves the objects entered in the scene string entered...
        /// </summary>
        /// <param name="obj">The objects to move</param>
        /// <returns>Was the move successful?</returns>
        public static void MoveObjectsToSceneInDoNotDestroy(List<GameObject> obj)
        {
            foreach (var i in obj)
            {
                SceneManager.MoveGameObjectToScene(i, instance.gameObject.scene);
            }
        }


        /// <summary>
        /// Finds the first object that matches the name entered... But only in the do not destroy scene...
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static GameObject FindObjectInDoNotDestroy(string name)
        {
            var obj = FindObjectsInDoNotDestroy(name);
            
            if (obj.Count > 0) return FindObjectsInDoNotDestroy(name)[0];
            
            if (AssetAccessor.GetAsset<AssetGlobalRuntimeSettings>().UseLogs)
            {
                MultiSceneLogger.Normal($"Unable to find object of name: {name} in the Do Not Destroy scene.");
            }
            
            return null;
        }


        /// <summary>
        /// Finds all the objects that matches the name entered... But only in the do not destroy scene...
        /// </summary>
        /// <param name="name">The name of the object to find.</param>
        /// <returns>List of all the objects found in the scene</returns>
        public static List<GameObject> FindObjectsInDoNotDestroy(string name)
        {
            var objs = new List<GameObject>();
            var validObjectsFromScene = new List<GameObject>();
            
            instance.gameObject.scene.GetRootGameObjects(objs);
            
            foreach (var go in objs)
            {
                validObjectsFromScene.AddRange(from Transform child in go.transform
                    where child.name.Equals(name)
                    select child.gameObject);
            }

            return validObjectsFromScene;
        }
        
        
        /// <summary>
        /// Gets the first object of the type entered within the do not destroy scene only...
        /// </summary>
        /// <typeparam name="T">The type to find</typeparam>
        /// <returns>The first found object of the type in the do not destroy scene</returns>
        public static T GetComponentInDoNotDestroy<T>()
        {
            var get = GetComponentsInDoNotDestroy<T>();

            if (get.Count > 0) return get[0];
            
            if (AssetAccessor.GetAsset<AssetGlobalRuntimeSettings>().UseLogs)
            {
                MultiSceneLogger.Normal(
                    $"Unable to get any component of the type {typeof(T)} in the Do Not Destroy scene.");
            }
            
            return default;
        }
        
        
        /// <summary>
        /// Gets all the objects of the type entered within the do not destroy scene only...
        /// </summary>
        /// <typeparam name="T">The type to find</typeparam>
        /// <returns>A list of all the found objects of the type in the do not destroy scene</returns>
        public static List<T> GetComponentsInDoNotDestroy<T>()
        {
            var scene = instance.gameObject.scene.GetRootGameObjects();
            var find = new List<T>();
            
            foreach (var obj in scene)
            {
                find.AddRange(obj.GetComponentsInChildren<T>());
            }

            return find;
        }
    }
}