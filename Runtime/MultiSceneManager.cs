using System.Collections.Generic;
using CarterGames.Experimental.MultiScene.Editor;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace CarterGames.Experimental.MultiScene
{
    public static class MultiSceneManager
    {
/* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
 {  Fields  }
───────────────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        private const string PlaceholderSceneName = "Multi Scene Placeholder Scene";
        private static List<string> _activeSceneNames;
        private static bool _hasCachedScenesList;
        private static SceneGroup _activeSceneGroup;
        private static MultiSceneSettingsAsset _settings;
        private static MultiSceneMono _mono;

/* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
 {  Properties  }
───────────────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// The default scene group to load
        /// </summary>
        public static SceneGroup ActiveSceneGroup => _activeSceneGroup;


        /// <summary>
        /// Gets the settings asset for this asset for use...
        /// </summary>
        private static MultiSceneSettingsAsset Settings
        {
            get
            {
                if (_settings != null) return _settings;
                _settings = AssetAccessor.GetAsset<MultiSceneSettingsAsset>();
                return _settings;
            }
        }
        
        
        /// <summary>
        /// Gets the mono script for use...
        /// </summary>
        public static MultiSceneMono Mono
        {
            get
            {
                if (_mono != null) return _mono;
                var go = new GameObject("Multi Scene (Mono Reference)");
                go.AddComponent<MultiSceneMono>();
                _mono = go.GetComponent<MultiSceneMono>();
                return _mono;
            }
        }
        
/* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
 {  Events  }
───────────────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Runs before a scene group loads
        /// </summary>
        public static MultiSceneEvt BeforeScenesLoaded = new MultiSceneEvt();
        
        /// <summary>
        /// Runs after a scene group has loaded & all listeners have been called
        /// </summary>
        public static MultiSceneEvt PostScenesLoaded = new MultiSceneEvt();

        /// <summary>
        /// Runs when each scene is loaded
        /// </summary>
        public static MultiSceneEvt<string> OnSceneLoaded = new MultiSceneEvt<string>();

        /// <summary>
        /// Runs when each scene is loaded
        /// </summary>
        public static MultiSceneEvt<string> OnSceneUnloaded = new MultiSceneEvt<string>();
        
        /// <summary>
        /// Runs when a scene group has loaded
        /// </summary>
        public static MultiSceneEvt OnSceneGroupLoaded = new MultiSceneEvt();

        /// <summary>
        /// Runs when a scene group has loaded & returns the group as a parameter
        /// </summary>
        public static MultiSceneEvt<SceneGroup> OnSceneGroupLoadedWithCtx = new MultiSceneEvt<SceneGroup>();
        
        
        

        public static bool IsSceneInGroup(string sceneName) => _activeSceneGroup.ContainsScene(sceneName);
        public static bool IsSceneInGroup(SceneGroup group, string sceneName) => group.ContainsScene(sceneName);
        
        
        public static bool IsSceneLoaded(string sceneName)
        {
            if (!_hasCachedScenesList)
                UpdateActiveSceneNames();

            return _activeSceneNames.Contains(sceneName);
        }
        
        
        /// <summary>
        /// Sets the active group to the group entered...
        /// </summary>
        /// <param name="group">the group to set to</param>
        public static void SetGroup(SceneGroup group)
        {
            _activeSceneGroup = group;
            UpdateActiveSceneNames();
        }
        
        
        /// <summary>
        /// Updates the list of active scenes for checking...
        /// </summary>
        private static void UpdateActiveSceneNames()
        {
            var _list = new List<string>();
            
            for (var i = SceneManager.sceneCount - 1; i >= 0; i--)
                _list.Add(SceneManager.GetSceneAt(i).name);

            _activeSceneNames = _list;
            _hasCachedScenesList = true;
        }

/* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
 {  Scene Management  }
───────────────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Initialises with the scene group last loaded or the default one based on the asset settings...
        /// </summary>
        /// <remarks>This runs at runtime to load the scene group instead of whatever is currently loaded...</remarks>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialise()
        {
            if (Settings.LoadMode == SceneGroupEditorLoadMode.None) return;
            
            LoadScenes(Settings.LoadMode == SceneGroupEditorLoadMode.Default 
                ? Settings.StartGroup 
                : Settings.LastGroup);
        }
        

        public static void LoadScenes(bool reloadDupScenes = true) => LoadScenes(Settings.StartGroup, reloadDupScenes);
        public static void LoadScenes(SceneGroup group, bool reloadDupScenes = true) => RunSceneLoading(group, reloadDupScenes);


        /// <summary>
        /// Loads the scenes in the selected scene group...overriding any existing active scenes. 
        /// </summary>
        private static void RunSceneLoading(SceneGroup group, bool? reloadDupScenes = true)
        {
            if (!group.IsValid)
            {
                MultiScenePopups.ShowInvalidSceneGroup();
                return;
            }
                
            _activeSceneGroup = group;

            var loadedScenes = new List<string>();

            // Unloads current scenes...
            UnloadAllActiveScenes();

            for (var i = 0; i < SceneManager.sceneCount; i++)
                loadedScenes.Add(SceneManager.GetSceneAt(i).name);


            // Loads the new scenes...
            for (var i = 0; i < _activeSceneGroup.scenes.Count; i++)
            {
                var sceneData = _activeSceneGroup.scenes[i];

                // Calls for the listeners to fire when the last scene in the group is loaded...
                if (i.Equals(_activeSceneGroup.scenes.Count - 1))
                    SceneManager.sceneLoaded += ListenerHandler.CallListeners;
                
                // Skips if scenes are not the be reloaded...
                if (reloadDupScenes == false)
                    if (loadedScenes.Contains(sceneData.sceneName)) continue;

                // Loads the scene via the path...
                SceneManager.LoadSceneAsync(sceneData.scenePath, new LoadSceneParameters(i.Equals(0) 
                        ? LoadSceneMode.Single 
                        : LoadSceneMode.Additive));

                // Runs any events for scenes loading...
                OnSceneLoaded.Raise(sceneData.sceneName);
            }

            if (loadedScenes.Contains(PlaceholderSceneName))
                SceneManager.UnloadSceneAsync(PlaceholderSceneName);
            
            OnSceneGroupLoaded.Raise();
            OnSceneGroupLoadedWithCtx.Raise(group);
            UpdateActiveSceneNames();
        }
        
        
        
        /// <summary>
        /// Unloads all the active scenes
        /// </summary>
        public static void UnloadAllActiveScenes()
        {
            SceneManager.CreateScene(PlaceholderSceneName);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(PlaceholderSceneName));
            
            var _scenes = new List<string>();

            for (var i = SceneManager.sceneCount - 1; i >= 0; i--)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.buildIndex < 0) continue;
                _scenes.Add(scene.name);
            }

            foreach (var _s in _scenes)
            {
                if (_s.Equals(PlaceholderSceneName)) continue;
                SceneManager.UnloadSceneAsync(_s);
                OnSceneUnloaded.Raise(_s);
            }
            
            if (!AssetAccessor.GetAsset<MultiSceneSettingsAsset>().UseUnloadResources) return;
            Resources.UnloadUnusedAssets();
        }
        
        
        /// <summary>
        /// Unloads all the additive scenes loaded but keeps the base scene as is unless overridden with a load method...
        /// </summary>
        public static void UnloadAllAdditiveScenes()
        {
            var _scenes = new List<string>();
            var _activeScene = SceneManager.GetActiveScene().name;

            for (var i = SceneManager.sceneCount - 1; i >= 0; i--)
            {
                if (SceneManager.GetSceneAt(i).name.Equals(_activeScene)) continue;
                _scenes.Add(SceneManager.GetSceneAt(i).name);
            }

            foreach (var scene in _scenes)
            {
                SceneManager.UnloadSceneAsync(scene);
                OnSceneUnloaded.Raise(scene);
            }

            if (!AssetAccessor.GetAsset<MultiSceneSettingsAsset>().UseUnloadResources) return;
            Resources.UnloadUnusedAssets();
        }


        /// <summary>
        /// Reloads the current scene group as if you just loaded the scene again...
        /// </summary>
        public static void ReloadScenes()
        {
            UnloadAllActiveScenes();
            LoadScenes();
        }
    }
}