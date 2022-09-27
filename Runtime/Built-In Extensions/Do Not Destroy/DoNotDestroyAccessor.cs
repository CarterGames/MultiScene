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
        private static DoNotDestroyAccessor _instance;
        
        

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void Initialise()
        {
            if (_instance != null) return;

            var obj = new GameObject("Multi Scene (Do Not Destroy Accessor)");
            DontDestroyOnLoad(obj);
            obj.AddComponent<DoNotDestroyAccessor>();
        }
        
        
        private void Awake()
        {
            if (_instance != null) Destroy(this.gameObject);
            _instance = this;
        }
        

        /// <summary>
        /// Gets all the root gameObjects in the scene for use...
        /// </summary>
        /// <returns>A list of all the valid root gameObjects the spy can find.</returns>
        public static List<GameObject> GetRootGameObjectsInDoNotDestroy()
        {
            return _instance.gameObject.scene.GetRootGameObjects().ToList();
        }
        
        
        /// <summary>
        /// Moves the object entered in the scene string entered...
        /// </summary>
        /// <param name="obj">The object to move</param>
        /// <returns>Was the move successful?</returns>
        public static void MoveObjectToSceneInDoNotDestroy(GameObject obj)
        {
            SceneManager.MoveGameObjectToScene(obj, _instance.gameObject.scene);
        }
        
        
        /// <summary>
        /// Moves the objects entered in the scene string entered...
        /// </summary>
        /// <param name="obj">The objects to move</param>
        /// <returns>Was the move successful?</returns>
        public static void MoveObjectsToSceneInDoNotDestroy(List<GameObject> obj)
        {
            foreach (var i in obj)
                SceneManager.MoveGameObjectToScene(i, _instance.gameObject.scene);
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
            
            if (AssetAccessor.GetAsset<MultiSceneSettingsAsset>().UseLogs)
                MsLog.Normal($"Unable to find object of name: {name} in the Do Not Destroy scene.");
            
            return null;
        }


        /// <summary>
        /// Finds all the objects that matches the name entered... But only in the do not destroy scene...
        /// </summary>
        /// <param name="name">The name of the object to find.</param>
        /// <returns>List of all the objects found in the scene</returns>
        public static List<GameObject> FindObjectsInDoNotDestroy(string name)
        {
            var _objects = new List<GameObject>();
            var _validObjectsFromScene = new List<GameObject>();
            
            _instance.gameObject.scene.GetRootGameObjects(_objects);
            
            foreach (var _go in _objects)
                _validObjectsFromScene.AddRange(from Transform _child in _go.transform where _child.name.Equals(name) select _child.gameObject);

            return _validObjectsFromScene;
        }
        
        
        /// <summary>
        /// Gets the first object of the type entered within the do not destroy scene only...
        /// </summary>
        /// <typeparam name="T">The type to find</typeparam>
        /// <returns>The first found object of the type in the do not destroy scene</returns>
        public static T GetComponentInDoNotDestroy<T>()
        {
            var _get = GetComponentsInDoNotDestroy<T>();

            if (_get.Count > 0) return _get[0];
            
            if (AssetAccessor.GetAsset<MultiSceneSettingsAsset>().UseLogs)
                MsLog.Normal($"Unable to get any component of the type {typeof(T)} in the Do Not Destroy scene.");
            
            return default;
        }
        
        
        /// <summary>
        /// Gets all the objects of the type entered within the do not destroy scene only...
        /// </summary>
        /// <typeparam name="T">The type to find</typeparam>
        /// <returns>A list of all the found objects of the type in the do not destroy scene</returns>
        public static List<T> GetComponentsInDoNotDestroy<T>()
        {
            var _scene = _instance.gameObject.scene.GetRootGameObjects();
            var _find = new List<T>();
            
            foreach (var _obj in _scene)
                _find.AddRange(_obj.GetComponentsInChildren<T>());

            return _find;
        }
    }
}