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
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CarterGames.Experimental.MultiScene
{
    /// <summary>
    /// Gets components from the current, any or all scenes currently in use...
    /// A bit more performant that FindObjectsOfType & GameObject.Find()...
    /// </summary>
    public static class MultiSceneRef
    {
        /// <summary>
        /// Gets the root objects in the scene requested.
        /// </summary>
        /// <param name="sceneName">The scene to search through</param>
        /// <returns>The objects in said scene</returns>
        public static GameObject[] GetRootObjects(string sceneName)
        {
            var scene = SceneManager.GetSceneByName(sceneName);
            return scene.GetRootGameObjects();
        }
        
        
        /// <summary>
        /// Moves the object entered in the scene string entered...
        /// </summary>
        /// <param name="obj">The object to move</param>
        /// <param name="sceneName">The scene to move to</param>
        /// <returns>Was the move successful?</returns>
        public static void MoveObjectToScene(GameObject obj, string sceneName)
        {
            var scene = SceneManager.GetSceneByName(sceneName);
            SceneManager.MoveGameObjectToScene(obj, scene);
        }
        
        
        /// <summary>
        /// Moves the objects entered in the scene string entered...
        /// </summary>
        /// <param name="obj">The objects to move</param>
        /// <param name="sceneName">The scene to move to</param>
        /// <returns>Was the move successful?</returns>
        public static void MoveObjectsToScene(List<GameObject> obj, string sceneName)
        {
            var scene = SceneManager.GetSceneByName(sceneName);
            
            foreach (var i in obj)
            {
                SceneManager.MoveGameObjectToScene(i, scene);
            }
        }
        
        
        /// <summary>
        /// Finds the first object that matches the name entered...
        /// </summary>
        /// <remarks>Alternative to GameObject.Find()</remarks>
        /// <param name="sceneName">The scene name to find.</param>
        /// <param name="name">The name of the object to find.</param>
        /// <returns>The object found</returns>
        public static GameObject FindObject(string sceneName, string name)
        {
            return FindObjects(sceneName, name)[0];
        }
        
        
        /// <summary>
        /// Finds the first object that matches the name entered... But only in the active (base) scene...
        /// </summary>
        /// <remarks>Alternative to GameObject.Find()</remarks>
        /// <param name="name">The name of the object to find.</param>
        /// <returns>The object found</returns>
        public static GameObject FindObject(string name)
        {
            return FindAllObjects(name)[0];
        }
        
        
        /// <summary>
        /// Finds all the objects that matches the name entered... But only in the active (base) scene...
        /// </summary>
        /// <param name="name">The name of the object to find.</param>
        /// <returns>List of all the objects found in the scene</returns>
        public static List<GameObject> FindAllObjects(string name)
        {
            var objs = new List<GameObject>();
            var scenes = new List<Scene>();
            var validObjectsFromScene = new List<GameObject>();

            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                scenes.Add(SceneManager.GetSceneAt(i));
            }

            foreach (var scene in scenes)
            {
                objs.AddRange(scene.GetRootGameObjects());
            }

            foreach (var go in objs)
            {
                validObjectsFromScene.AddRange(from Transform child in go.transform
                    where child.name.Equals(name)
                    select child.gameObject);
            }

            return validObjectsFromScene;
        }
        
        
        /// <summary>
        /// Finds all the objects that matches the name entered... But only in the active (base) scene...
        /// </summary>
        /// <param name="name">The name of the object to find.</param>
        /// <returns>List of all the objects found in the scene</returns>
        public static List<GameObject> FindObjects(string name)
        {
            var objs = new List<GameObject>();
            var scene = SceneManager.GetActiveScene();
            var validObjectsFromScene = new List<GameObject>();
            
            scene.GetRootGameObjects(objs);
            
            foreach (var go in objs)
            {
                validObjectsFromScene.AddRange(from Transform child in go.transform
                    where child.name.Equals(name)
                    select child.gameObject);
            }

            return validObjectsFromScene;
        }
        
        
        /// <summary>
        /// Finds all the objects that matches the name entered...
        /// </summary>
        /// <param name="sceneName">The scene name to search through.</param> 
        /// <param name="name">The name of the object to find.</param>
        /// <returns>List of all the objects found in the scene</returns>
        public static List<GameObject> FindObjects(string sceneName, string name)
        {
            var objs = new List<GameObject>();
            var scene = SceneManager.GetSceneByName(sceneName);
            var validObjectsFromScene = new List<GameObject>();
            
            scene.GetRootGameObjects(objs);
            
            foreach (var go in objs)
            {
                validObjectsFromScene.AddRange(from Transform child in go.transform
                    where child.name.Equals(name)
                    select child.gameObject);
            }

            return validObjectsFromScene;
        }


        /// <summary>
        /// Gets any and all of the type requested from the active scene...
        /// </summary>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns>List of any instances of the type found in the scene</returns>
        public static List<T> GetComponentsFromActiveScene<T>()
        {
            var objs = new List<GameObject>();
            var scene = SceneManager.GetActiveScene();
            var validObjectsFromScene = new List<T>();
            
            scene.GetRootGameObjects(objs);
            
            foreach (var go in objs)
            {
                validObjectsFromScene.AddRange(go.GetComponentsInChildren<T>(true));
            }

            return validObjectsFromScene;
        }
        
        
        /// <summary>
        /// Gets any and all of the type requested from the active scene...
        /// </summary>
        /// <param name="obj">The object in the scene to search</param>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns>List of any instances of the type found in the scene</returns>
        public static List<T> GetComponentsFromThisScene<T>(this MonoBehaviour obj)
        {
            var objs = new List<GameObject>();
            var scene = obj.gameObject.scene;
            var validObjectsFromScene = new List<T>();
            
            scene.GetRootGameObjects(objs);
            
            foreach (var go in objs)
            {
                validObjectsFromScene.AddRange(go.GetComponentsInChildren<T>(true));
            }

            return validObjectsFromScene;
        }
        
        
        /// <summary>
        /// Gets any and all of the type requested from any active scene...
        /// </summary>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns>List of any instances of the type found in the scene</returns>
        public static List<T> GetComponentsFromAllScenes<T>()
        {
            var objs = new List<GameObject>();
            var scenes = new List<Scene>();
            var validObjectsFromScene = new List<T>();

            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                scenes.Add(SceneManager.GetSceneAt(i));
            }

            foreach (var scene in scenes)
            {
                objs.AddRange(scene.GetRootGameObjects());
            }

            foreach (var go in objs)
            {
                validObjectsFromScene.AddRange(go.GetComponentsInChildren<T>(true));
            }

            return validObjectsFromScene;
        }


        /// <summary>
        /// Gets any and all of the type requested from the scene requested...
        /// </summary>
        /// <param name="sceneName">The scene to search</param>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns>List of any instances of the type found in the scene</returns>
        /// <returns></returns>
        public static List<T> GetComponentsFromScene<T>(string sceneName)
        {
            var objs = new List<GameObject>();
            var scene = SceneManager.GetSceneByName(sceneName);
            var validObjectsFromScene = new List<T>();
            
            scene.GetRootGameObjects(objs);
            
            foreach (var go in objs)
            {
                validObjectsFromScene.AddRange(go.GetComponentsInChildren<T>(true));
            }

            return validObjectsFromScene;
        }


        /// <summary>
        /// Gets any and all of the type requested from the scene requested...
        /// </summary>
        /// <param name="sceneNamesList">The scenes to search</param>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns>List of any instances of the type found in the scene</returns>
        /// <returns></returns>
        public static List<T> GetComponentsFromScenes<T>(List<string> sceneNamesList)
        {
            var validObjectsFromScene = new List<T>();
            var sceneObjects = new List<GameObject>();

            foreach (var scene in sceneNamesList)
            {
                sceneObjects.AddRange(SceneManager.GetSceneByName(scene).GetRootGameObjects());
            }
            
            foreach (var go in sceneObjects)
            {
                validObjectsFromScene.AddRange(go.GetComponentsInChildren<T>(true));
            }

            return validObjectsFromScene;
        }


        /// <summary>
        /// Gets the first of any and all of the type requested from the active scene...
        /// </summary>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns>First instance of the type found in the active scene</returns>
        public static T GetComponentFromActiveScene<T>()
        {
            var allOfType = GetComponentsFromActiveScene<T>();

            if (allOfType.Count > 0)
            {
                return allOfType[0];
            }
            else
            {
                MultiSceneLogger.Error("Unable To Find Any of Type In Scene");
                return default;
            }
        }
        
        
        /// <summary>
        /// Gets the first of any and all of the type requested from the active scene...
        /// </summary>
        /// <param name="obj">The object in the scene to search</param>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns>First instance of the type found in the active scene</returns>
        public static T GetComponentFromThisScene<T>(this MonoBehaviour obj)
        {
            var allOfType = GetComponentsFromThisScene<T>(obj);

            if (allOfType.Count > 0)
            {
                return allOfType[0];
            }
            else
            {
                MultiSceneLogger.Error("Unable To Find Any of Type In Scene");
                return default;
            }
        }
        
        
        /// <summary>
        /// Gets the first of any and all of the type requested from any active scene...
        /// </summary>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns>First instance of the type found in the active scene</returns>
        public static T GetComponentFromAllScenes<T>()
        {
            var allOfType = GetComponentsFromAllScenes<T>();

            if (allOfType.Count > 0)
            {
                return allOfType[0];
            }
            else
            {
                MultiSceneLogger.Error("Unable To Find Any of Type In Scene");
                return default;
            }
        }
        
        
        /// <summary>
        /// Gets the first of any and all of the type requested from the scene requested...
        /// </summary>
        /// <param name="sceneName">The scene to search</param>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns>First instance of the type found in the scene provided</returns>
        public static T GetComponentFromScene<T>(string sceneName)
        {
            var allOfType = GetComponentsFromScene<T>(sceneName);

            if (allOfType.Count > 0)
            {
                return allOfType[0];
            }
            else
            {
                MultiSceneLogger.Error("Unable To Find Any of Type In Scene");
                return default;
            }
        }
        
        
        /// <summary>
        /// Gets the first of any and all of the type requested from the scene requested...
        /// </summary>
        /// <param name="sceneNameList">The scenes to search</param>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns>First instance of the type found in the scene provided</returns>
        public static T GetComponentFromScenes<T>(List<string> sceneNameList)
        {
            var allOfType = GetComponentsFromScenes<T>(sceneNameList);

            if (allOfType.Count > 0)
            {
                return allOfType[0];
            }
            else
            {
                MultiSceneLogger.Error("Unable To Find Any of Type In Scene");
                return default;
            }
        }
    }
}