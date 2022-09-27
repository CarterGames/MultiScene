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
        /// <param name="scene">The scene to search through</param>
        /// <returns>The objects in said scene</returns>
        public static GameObject[] GetRootObjects(string scene)
        {
            var _scene = SceneManager.GetSceneByName(scene);
            return _scene.GetRootGameObjects();
        }
        
        
        /// <summary>
        /// Moves the object entered in the scene string entered...
        /// </summary>
        /// <param name="obj">The object to move</param>
        /// <param name="scene">The scene to move to</param>
        /// <returns>Was the move successful?</returns>
        public static void MoveObjectToScene(GameObject obj, string scene)
        {
            var _scene = SceneManager.GetSceneByName(scene);
            SceneManager.MoveGameObjectToScene(obj, _scene);
        }
        
        
        /// <summary>
        /// Moves the objects entered in the scene string entered...
        /// </summary>
        /// <param name="obj">The objects to move</param>
        /// <param name="scene">The scene to move to</param>
        /// <returns>Was the move successful?</returns>
        public static void MoveObjectsToScene(List<GameObject> obj, string scene)
        {
            var _scene = SceneManager.GetSceneByName(scene);
            foreach (var i in obj)
                SceneManager.MoveGameObjectToScene(i, _scene);
        }
        
        
        /// <summary>
        /// Finds the first object that matches the name entered...
        /// </summary>
        /// <remarks>Alternative to GameObject.Find()</remarks>
        /// <param name="scene">The scene name to find.</param>
        /// <param name="name">The name of the object to find.</param>
        /// <returns>The object found</returns>
        public static GameObject FindObject(string scene, string name)
        {
            return FindObjects(scene, name)[0];
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
            var _objects = new List<GameObject>();
            var _scenes = new List<Scene>();
            var _validObjectsFromScene = new List<GameObject>();

            for (var i = 0; i < SceneManager.sceneCount; i++)
                _scenes.Add(SceneManager.GetSceneAt(i));

            foreach (var _s in _scenes)
                _objects.AddRange(_s.GetRootGameObjects());

            foreach (var _go in _objects)
                _validObjectsFromScene.AddRange(from Transform _child in _go.transform where _child.name.Equals(name) select _child.gameObject);

            return _validObjectsFromScene;
        }
        
        
        /// <summary>
        /// Finds all the objects that matches the name entered... But only in the active (base) scene...
        /// </summary>
        /// <param name="name">The name of the object to find.</param>
        /// <returns>List of all the objects found in the scene</returns>
        public static List<GameObject> FindObjects(string name)
        {
            var _objects = new List<GameObject>();
            var _scene = SceneManager.GetActiveScene();
            var _validObjectsFromScene = new List<GameObject>();
            
            _scene.GetRootGameObjects(_objects);
            
            foreach (var _go in _objects)
                _validObjectsFromScene.AddRange(from Transform _child in _go.transform where _child.name.Equals(name) select _child.gameObject);

            return _validObjectsFromScene;
        }
        
        
        /// <summary>
        /// Finds all the objects that matches the name entered...
        /// </summary>
        /// <param name="scene">The scene name to search through.</param> 
        /// <param name="name">The name of the object to find.</param>
        /// <returns>List of all the objects found in the scene</returns>
        public static List<GameObject> FindObjects(string scene, string name)
        {
            var _objects = new List<GameObject>();
            var _scene = SceneManager.GetSceneByName(scene);
            var _validObjectsFromScene = new List<GameObject>();
            
            _scene.GetRootGameObjects(_objects);
            
            foreach (var _go in _objects)
                _validObjectsFromScene.AddRange(from Transform _child in _go.transform where _child.name.Equals(name) select _child.gameObject);

            return _validObjectsFromScene;
        }


        /// <summary>
        /// Gets any and all of the type requested from the active scene...
        /// </summary>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns>List of any instances of the type found in the scene</returns>
        public static List<T> GetComponentsFromActiveScene<T>()
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
        /// Gets any and all of the type requested from the active scene...
        /// </summary>
        /// <param name="obj">The object in the scene to search</param>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns>List of any instances of the type found in the scene</returns>
        public static List<T> GetComponentsFromThisScene<T>(this MonoBehaviour obj)
        {
            var _objects = new List<GameObject>();
            var _scene = obj.gameObject.scene;
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
        /// <param name="s">The scenes to search</param>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns>List of any instances of the type found in the scene</returns>
        /// <returns></returns>
        public static List<T> GetComponentsFromScenes<T>(List<string> s)
        {
            var _validObjectsFromScene = new List<T>();
            var _sceneObjects = new List<GameObject>();

            foreach (var scene in s)
                _sceneObjects.AddRange(SceneManager.GetSceneByName(scene).GetRootGameObjects());

            foreach (var _go in _sceneObjects)
                _validObjectsFromScene.AddRange(_go.GetComponentsInChildren<T>(true));

            return _validObjectsFromScene;
        }


        /// <summary>
        /// Gets the first of any and all of the type requested from the active scene...
        /// </summary>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns>First instance of the type found in the active scene</returns>
        public static T GetComponentFromActiveScene<T>()
        {
            var _allOfType = GetComponentsFromActiveScene<T>();

            return _allOfType.Count > 0 
                ? _allOfType[0] 
                : default;
        }
        
        
        /// <summary>
        /// Gets the first of any and all of the type requested from the active scene...
        /// </summary>
        /// <param name="obj">The object in the scene to search</param>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns>First instance of the type found in the active scene</returns>
        public static T GetComponentFromThisScene<T>(this MonoBehaviour obj)
        {
            var _allOfType = GetComponentsFromThisScene<T>(obj);

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
        public static T GetComponentFromScene<T>(string s)
        {
            var _allOfType = GetComponentsFromScene<T>(s);

            if (_allOfType.Count > 0)
            {
                return _allOfType[0];
            }
            else
            {
                MsLog.Error("Unable To Find Any of Type In Scene");
                return default;
            }
            
            // return _allOfType.Count > 0 
            //     ? _allOfType[0] 
            //     : default;
        }
        
        
        /// <summary>
        /// Gets the first of any and all of the type requested from the scene requested...
        /// </summary>
        /// <param name="s">The scenes to search</param>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns>First instance of the type found in the scene provided</returns>
        public static T GetComponentFromScenes<T>(List<string> s)
        {
            var _allOfType = GetComponentsFromScenes<T>(s);

            if (_allOfType.Count > 0)
            {
                return _allOfType[0];
            }
            else
            {
                MsLog.Error("Unable To Find Any of Type In Scene");
                return default;
            }
            
            // return _allOfType.Count > 0 
            //     ? _allOfType[0] 
            //     : default;
        }
    }
}