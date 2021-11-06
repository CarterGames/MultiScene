﻿/*
 * 
 *  Multi-Scene Workflow
 *							  
 *	Scene Elly
 *      A J-Tools class that allows you to get objects in the scene as well as move objects between scenes.
 *			
 *  Written by:
 *      Jonathan Carter
 *		
 *	Last Updated: 05/11/2021 (d/m/y)							
 * 
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MultiScene.Core
{
    /// <summary>
    /// Gets components from the current, any or all scenes currently in use...
    /// A bit more performant that FindObjectsOfType & GameObject.Find()...
    /// </summary>
    /// <remarks>Why SceneElly? its my short slang for SceneElement xD</remarks>
    public static class SceneElly
    {
        /// <summary>
        /// Moves the object entered in the scene string entered...
        /// </summary>
        /// <param name="obj">The object to move</param>
        /// <param name="scene">The scene to move to</param>
        /// <returns>Was the move successful?</returns>
        public static bool MoveObjectToScene(GameObject obj, string scene)
        {
            var _scene = SceneManager.GetSceneByName(scene);
            if (_scene == null) return false;
            SceneManager.MoveGameObjectToScene(obj, _scene);
            return true;
        }
        
        
        /// <summary>
        /// Moves the objects entered in the scene string entered...
        /// </summary>
        /// <param name="obj">The objects to move</param>
        /// <param name="scene">The scene to move to</param>
        /// <returns>Was the move successful?</returns>
        public static bool MoveObjectsToScene(List<GameObject> obj, string scene)
        {
            var _scene = SceneManager.GetSceneByName(scene);
            if (_scene == null) return false;
            foreach (var i in obj)
                SceneManager.MoveGameObjectToScene(i, _scene);
            return true;
        }
        
        
        /// <summary>
        /// Moves the object entered in the scene string entered...
        /// </summary>
        /// <param name="obj">The object to move</param>
        /// <param name="scene">The scene to move to</param>
        /// <returns>Was the move successful?</returns>
        public static bool MoveObjectToScene(GameObject obj, int scene)
        {
            var _scene = SceneManager.GetSceneAt(scene);
            if (_scene == null) return false;
            SceneManager.MoveGameObjectToScene(obj, _scene);
            return true;
        }
        
        
        /// <summary>
        /// Moves the objects entered in the scene string entered...
        /// </summary>
        /// <param name="obj">The objects to move</param>
        /// <param name="scene">The scene to move to</param>
        /// <returns>Was the move successful?</returns>
        public static bool MoveObjectsToScene(List<GameObject> obj, int scene)
        {
            var _scene = SceneManager.GetSceneAt(scene);
            if (_scene == null) return false;
            foreach (var i in obj)
                SceneManager.MoveGameObjectToScene(i, _scene);
            return true;
        }
        
        
        /// <summary>
        /// Moves the object entered in the scene string entered...
        /// </summary>
        /// <param name="obj">The object to move</param>
        /// <param name="scene">The scene to move to</param>
        /// <returns>Was the move successful?</returns>
        public static bool MoveObjectToSceneByBuildIndex(GameObject obj, int scene)
        {
            var _scene = SceneManager.GetSceneByBuildIndex(scene);
            if (_scene == null) return false;
            SceneManager.MoveGameObjectToScene(obj, _scene);
            return true;
        }
        
        
        /// <summary>
        /// Moves the objects entered in the scene string entered...
        /// </summary>
        /// <param name="obj">The objects to move</param>
        /// <param name="scene">The scene to move to</param>
        /// <returns>Was the move successful?</returns>
        public static bool MoveObjectsToSceneByBuildIndex(List<GameObject> obj, int scene)
        {
            var _scene = SceneManager.GetSceneByBuildIndex(scene);
            if (_scene == null) return false;
            foreach (var i in obj)
                SceneManager.MoveGameObjectToScene(i, _scene);
            return true;
        }


        /// <summary>
        /// Gets any and all of the type requested from the active scene...
        /// </summary>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns>List of any instances of the type found in the scene</returns>
        public static List<T> GetComponentsFromScene<T>()
        {
            var _objects = new List<GameObject>();
            var _scene = SceneManager.GetActiveScene();
            var _validObjectsFromScene = new List<T>();
            
            _scene.GetRootGameObjects(_objects);
            
            foreach (var _go in _objects)
                _validObjectsFromScene.AddRange(_go.GetComponentsInChildren<T>(true));

            return _validObjectsFromScene;
        }
        
        
        /// <summary>
        /// Gets any and all of the type requested from any active scene...
        /// </summary>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns>List of any instances of the type found in the scene</returns>
        public static List<T> GetComponentsFromAllScenes<T>()
        {
            var _objects = new List<GameObject>();
            var _scenes = new List<Scene>();
            var _validObjectsFromScene = new List<T>();

            for (var i = 0; i < SceneManager.sceneCount; i++)
                _scenes.Add(SceneManager.GetSceneAt(i));

            foreach (var _s in _scenes)
                _objects.AddRange(_s.GetRootGameObjects());

            foreach (var _go in _objects)
                _validObjectsFromScene.AddRange(_go.GetComponentsInChildren<T>(true));

            return _validObjectsFromScene;
        }


        /// <summary>
        /// Gets any and all of the type requested from the scene requested...
        /// </summary>
        /// <param name="s">The scene to search</param>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns>List of any instances of the type found in the scene</returns>
        /// <returns></returns>
        public static List<T> GetComponentsFromScene<T>(Scene s)
        {
            var _objects = new List<GameObject>();
            var _scene = s;
            var _validObjectsFromScene = new List<T>();
            
            _scene.GetRootGameObjects(_objects);
            
            foreach (var _go in _objects)
                _validObjectsFromScene.AddRange(_go.GetComponentsInChildren<T>(true));

            return _validObjectsFromScene;
        }
        
        
        /// <summary>
        /// Gets any and all of the type requested from the scene requested...
        /// </summary>
        /// <param name="s">The scene to search</param>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns>List of any instances of the type found in the scene</returns>
        /// <returns></returns>
        public static List<T> GetComponentsFromScene<T>(string s)
        {
            var _objects = new List<GameObject>();
            var _scene = SceneManager.GetSceneByName(s);
            var _validObjectsFromScene = new List<T>();
            
            _scene.GetRootGameObjects(_objects);
            
            foreach (var _go in _objects)
                _validObjectsFromScene.AddRange(_go.GetComponentsInChildren<T>(true));

            return _validObjectsFromScene;
        }


        /// <summary>
        /// Gets any and all of the type requested from the scene requested...
        /// </summary>
        /// <param name="s">The scene to search</param>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns>List of any instances of the type found in the scene</returns>
        /// <returns></returns>
        public static List<T> GetComponentsFromScene<T>(int s)
        {
            var _objects = new List<GameObject>();
            var _scene = SceneManager.GetSceneAt(s);
            var _validObjectsFromScene = new List<T>();
            
            _scene.GetRootGameObjects(_objects);
            
            foreach (var _go in _objects)
                _validObjectsFromScene.AddRange(_go.GetComponentsInChildren<T>(true));

            return _validObjectsFromScene;
        }
        
        
        /// <summary>
        /// Gets any and all of the type requested from the scene requested...
        /// </summary>
        /// <param name="s">The scene to search</param>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns>List of any instances of the type found in the scene</returns>
        /// <returns></returns>
        public static List<T> GetComponentsFromSceneByBuildIndex<T>(int s)
        {
            var _objects = new List<GameObject>();
            var _scene = SceneManager.GetSceneByBuildIndex(s);
            var _validObjectsFromScene = new List<T>();
            
            _scene.GetRootGameObjects(_objects);
            
            foreach (var _go in _objects)
                _validObjectsFromScene.AddRange(_go.GetComponentsInChildren<T>(true));

            return _validObjectsFromScene;
        }
        
        
        /// <summary>
        /// Gets any and all of the type requested from the scene requested...
        /// </summary>
        /// <param name="s">The scenes to search</param>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns>List of any instances of the type found in the scene</returns>
        /// <returns></returns>
        public static List<T> GetComponentsFromScenes<T>(List<Scene> s)
        {
            var _objects = new List<GameObject>();
            var _validObjectsFromScene = new List<T>();

            foreach (var _scene in s)
            {
                _scene.GetRootGameObjects(_objects);
                foreach (var _go in _objects)
                    _validObjectsFromScene.AddRange(_go.GetComponentsInChildren<T>(true));
            }

            return _validObjectsFromScene;
        }


        /// <summary>
        /// Gets the first of any and all of the type requested from the active scene...
        /// </summary>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns>First instance of the type found in the active scene</returns>
        public static T GetComponentFromScene<T>()
        {
            var _allOfType = GetComponentsFromScene<T>();

            return _allOfType.Count > 0 
                ? _allOfType[0] 
                : default;
        }
        
        
        /// <summary>
        /// Gets the first of any and all of the type requested from any active scene...
        /// </summary>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns>First instance of the type found in the active scene</returns>
        public static T GetComponentFromAllScenes<T>()
        {
            var _allOfType = GetComponentsFromAllScenes<T>();

            return _allOfType.Count > 0 
                ? _allOfType[0] 
                : default;
        }
        
        
        /// <summary>
        /// Gets the first of any and all of the type requested from the scene requested...
        /// </summary>
        /// <param name="s">The scene to search</param>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns>First instance of the type found in the scene provided</returns>
        public static T GetComponentFromScene<T>(Scene s)
        {
            var _allOfType = GetComponentsFromScene<T>(s);

            return _allOfType.Count > 0 
                ? _allOfType[0] 
                : default;
        }
        
        
        /// <summary>
        /// Gets the first of any and all of the type requested from the scene requested...
        /// </summary>
        /// <param name="s">The scene to search</param>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns>First instance of the type found in the scene provided</returns>
        public static T GetComponentFromScene<T>(string s)
        {
            var _allOfType = GetComponentsFromScene<T>(s);

            return _allOfType.Count > 0 
                ? _allOfType[0] 
                : default;
        }
        
        
        /// <summary>
        /// Gets the first of any and all of the type requested from the scene requested...
        /// </summary>
        /// <param name="s">The scene to search</param>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns>First instance of the type found in the scene provided</returns>
        public static T GetComponentFromScene<T>(int s)
        {
            var _allOfType = GetComponentsFromScene<T>(s);

            return _allOfType.Count > 0 
                ? _allOfType[0] 
                : default;
        }
        
        
        /// <summary>
        /// Gets the first of any and all of the type requested from the scene requested...
        /// </summary>
        /// <param name="s">The scene to search</param>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns>First instance of the type found in the scene provided</returns>
        public static T GetComponentFromSceneByBuildIndex<T>(int s)
        {
            var _allOfType = GetComponentsFromSceneByBuildIndex<T>(s);

            return _allOfType.Count > 0 
                ? _allOfType[0] 
                : default;
        }
        
        
        /// <summary>
        /// Gets the first of any and all of the type requested from the active scenes requested...
        /// </summary>
        /// <param name="s">The scene to search</param>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns>First instance of the type found in any of the scenes provided</returns>
        public static T GetComponentFromScenes<T>(List<Scene> s)
        {
            var _allOfType = GetComponentsFromScenes<T>(s);

            return _allOfType.Count > 0 
                ? _allOfType[0] 
                : default;
        }
    }
}