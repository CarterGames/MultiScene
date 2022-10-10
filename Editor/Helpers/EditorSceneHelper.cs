using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace CarterGames.Experimental.MultiScene.Editor
{
    /// <summary>
    /// Helps with getting the scenes in the project in different formats for other elements of the asset.
    /// </summary>
    public sealed class EditorSceneHelper : AssetPostprocessor
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private static List<string> _cacheAllSceneNamesInProject = new List<string>();
        private static Dictionary<string, string> _cachedScenesInBuildSettings = new Dictionary<string, string>();
        private static List<string> _cachedScenesInBuildSettingsKeys = new List<string>();

        private static bool _hasCache;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Gets all the scene names in the project.
        /// </summary>
        public static List<string> AllSceneNamesInProject
        {
            get
            {
                if (_hasCache) return _cacheAllSceneNamesInProject;
                UpdateCaches();
                return _cacheAllSceneNamesInProject;
            }
        }
        
        
        /// <summary>
        /// Gets all the scenes in the build settings.
        /// </summary>
        public static Dictionary<string, string> ScenesInBuildSettings
        {
            get
            {
                if (_hasCache) return _cachedScenesInBuildSettings;
                UpdateCaches();
                return _cachedScenesInBuildSettings;
            }
        }
        
        
        /// <summary>
        /// Gets all the scenes in the build settings but only returns the keys of the dictionary stored in this class. 
        /// </summary>
        private static List<string> ScenesInBuildSettingsKeys
        {
            get
            {
                if (_hasCache) return _cachedScenesInBuildSettingsKeys;
                UpdateCaches();
                return _cachedScenesInBuildSettingsKeys;
            }
        }
       
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   General Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        
        /// <summary>
        /// Updates the cache for scenes in the project when called.
        /// </summary>
        public static void UpdateCaches()
        {
            _cacheAllSceneNamesInProject = GetNamesOfScenesInProject();
            _cachedScenesInBuildSettings = GetAllScenesInProject();
            _hasCache = true;
        }


        /// <summary>
        /// Gets the names of all the scenes in the project & processes them into a readable format for use.
        /// </summary>
        /// <returns>A list of scene names in string format.</returns>
        private static List<string> GetNamesOfScenesInProject()
        {
            if (_hasCache) return _cacheAllSceneNamesInProject;
            
            var assets = AssetDatabase.FindAssets("t:scene", null);

            var list = new List<string>();

            for (var i = 0; i < assets.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(assets[i]);
                var split = path.Split('/');
                list.Add(split[split.Length - 1].Replace(".unity", ""));
            }

            return list;
        }


        /// <summary>
        /// Sorts all the scenes in the build settings into a dictionary for use. 
        /// </summary>
        /// <returns>An organised dictionary.</returns>
        public static Dictionary<string, string> GetAllScenesInProject()
        {
            if (_hasCache) return _cachedScenesInBuildSettings;
            
            var _scenes = EditorBuildSettings.scenes;
            var buildSettingsScenes = new Dictionary<string, string> { { "", "" } };

            foreach (var scene in _scenes)
            {
                var filteredPath = scene.path.Replace("Assets/", "").Replace(".unity", "");
                var split = filteredPath.Split('/');

                if (buildSettingsScenes.ContainsKey(split[split.Length - 1]))
                    buildSettingsScenes.Add(split[split.Length - 2] + "/" + split[split.Length - 1], scene.path);
                else
                    buildSettingsScenes.Add(split[split.Length - 1], scene.path);
            }

            _cachedScenesInBuildSettingsKeys = buildSettingsScenes.Keys.ToList();

            return buildSettingsScenes;
        }

        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Utility Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        
        /// <summary>
        /// Converts an int for an index of a scene to a scene name.
        /// </summary>
        /// <param name="value">The int to convert.</param>
        /// <returns>The scene name.</returns>
        public static string ConvertIntToScene(int value)
        {
            if (value.Equals(-1))
                return "";
                
            return ScenesInBuildSettingsKeys[value];
        }
        
        
        /// <summary>
        /// Converts a string into an int index of the string in all the scene names.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        /// <returns>The index of the scene name.</returns>
        public static int ConvertStringToIndex(string value)
        {
            return ScenesInBuildSettingsKeys.IndexOf(value);
        }
    }
}