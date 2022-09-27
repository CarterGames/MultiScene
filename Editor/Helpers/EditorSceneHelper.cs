using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace CarterGames.Experimental.MultiScene.Editor
{
    public class EditorSceneHelper : AssetPostprocessor
    {
        private static List<string> _cacheAllSceneNamesInProject = new List<string>();
        private static Dictionary<string, string> _cachedScenesInBuildSettings = new Dictionary<string, string>();
        private static List<string> _cachedScenesInBuildSettingsKeys = new List<string>();

        private static bool _hasCache;


        public static List<string> AllSceneNamesInProject
        {
            get
            {
                if (_hasCache) return _cacheAllSceneNamesInProject;
                UpdateCaches();
                return _cacheAllSceneNamesInProject;
            }
        }
        
        
        public static Dictionary<string, string> ScenesInBuildSettings
        {
            get
            {
                if (_hasCache) return _cachedScenesInBuildSettings;
                UpdateCaches();
                return _cachedScenesInBuildSettings;
            }
        }
        
        
        public static List<string> ScenesInBuildSettingsKeys
        {
            get
            {
                if (_hasCache) return _cachedScenesInBuildSettingsKeys;
                UpdateCaches();
                return _cachedScenesInBuildSettingsKeys;
            }
        }
        
        

        public static void UpdateCaches()
        {
            _cacheAllSceneNamesInProject = GetNamesOfScenesInProject();
            _cachedScenesInBuildSettings = GetAllScenesInProject();
            _hasCache = true;
        }
        

        public static List<string> GetNamesOfScenesInProject()
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


        public static string ConvertIntToScene(int value)
        {
            if (value.Equals(-1))
                return "";
                
            return ScenesInBuildSettingsKeys[value];
        }
        
        
        public static int ConvertStringToIndex(string value)
        {
            return ScenesInBuildSettingsKeys.IndexOf(value);
        }
    }
}