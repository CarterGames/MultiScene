using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Experimental.MultiScene.Editor
{
    /// <summary>
    /// Handles the settings window for the asset.
    /// </summary>
    public static class MultiSceneSettings
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private static SettingsProvider Provider;
        private static SerializedObject settingsAssetObject;
        private static bool ListeningToEvents;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Gets the settings asset in the project as a SerializedObject.
        /// </summary>
        public static SerializedObject SettingsAssetObject
        {
            get
            {
                if (settingsAssetObject != null) return settingsAssetObject;
                settingsAssetObject = new SerializedObject(MultiSceneEditorUtil.Settings);
                return settingsAssetObject;
            }
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Menu Items
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The menu item for opening the settings window.
        /// </summary>
        [MenuItem("Tools/Carter Games/Multi Scene/Edit Settings", priority = 0)]
        public static void OpenSettings()
        {
            SettingsService.OpenProjectSettings("Project/Carter Games/Multi Scene");
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        
        /// <summary>
        /// Subs to any events needed if they are not already subs to them.
        /// </summary>
        private static void ListenForEvents()
        {
            if (ListeningToEvents) return;
            ListeningToEvents = true;
            MultiSceneEditorEvents.Settings.OnSettingsAssetRegenerated.Add(ClearSettingsAsset);
        }


        /// <summary>
        /// Resets the settings asset object.
        /// </summary>
        private static void ClearSettingsAsset()
        {
            settingsAssetObject = null;
        }
        

        /// <summary>
        /// Handles the settings window in the engine.
        /// </summary>
        [SettingsProvider]
        public static SettingsProvider MultiSceneSettingsDrawer()
        {
            ListenForEvents();
            
            var provider = new SettingsProvider(MultiSceneEditorUtil.SettingsWindowPath, SettingsScope.Project)
            {
                guiHandler = (searchContext) =>
                {
                    if (!MultiSceneEditorUtil.HasSettingsFile) return;

                    MultiSceneEditorUtil.DrawHeader();
                    DrawInfo();
                    
                    EditorGUILayout.BeginVertical("HelpBox");
                    GUILayout.Space(1.5f);
            
                    EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
                    GUILayout.Space(1.5f);
                    
                    EditorGUI.BeginChangeCheck();
                    
                    DrawGeneralOptions();
                    DrawSceneGroupOptions();
                    DrawSceneGroupCategoryOptions();

                    if (EditorGUI.EndChangeCheck())
                    {
                        // Debug.LogError("Setting Edit...");
                        SettingsAssetObject.ApplyModifiedProperties();
                        SettingsAssetObject.Update();
                        MultiSceneEditorEvents.Settings.OnSettingChanged.Raise();
                    }

                    GUILayout.Space(2.5f);
                    EditorGUILayout.EndVertical();
                    
                    DrawButtons();
                },
                
                keywords = new HashSet<string>(new[] { "Carter Games", "External Assets", "Tools", "Scene Manager", "Scenes", "Scene Management", "MultiScene" })
            };
            
            return provider;
        }


        /// <summary>
        /// Draws the info section of the window.
        /// </summary>
        private static void DrawInfo()
        {
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(1.5f);
            
            EditorGUILayout.LabelField("Info", EditorStyles.boldLabel);

            EditorGUILayout.LabelField(new GUIContent("Version", "The version of the asset in use."),  new GUIContent(AssetVersionData.VersionNumber));
            EditorGUILayout.LabelField(new GUIContent("Release Date", "The date this version of the asset was published on."), new GUIContent(AssetVersionData.ReleaseDate));

            GUILayout.Space(2.5f);
            EditorGUILayout.EndVertical();
        }


        /// <summary>
        /// Draws the general options section of the window.
        /// </summary>
        private static void DrawGeneralOptions()
        {
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(1.5f);

            SettingsAssetObject.FindProperty("showGeneralOptions").boolValue = EditorGUILayout.Foldout(SettingsAssetObject.FindProperty("showGeneralOptions").boolValue, " General Options");
            
            if (SettingsAssetObject.FindProperty("showGeneralOptions").boolValue)
            {
                // Listener Frequency...
                EditorGUILayout.PropertyField(SettingsAssetObject.FindProperty("listenerFrequency"),
                    new GUIContent("Listener Frequency",
                        "Controls how many listeners execute per frame. The higher the number the more intensive the scene group loading can be."));
                
                // Use Unload Resources...
                EditorGUILayout.PropertyField(SettingsAssetObject.FindProperty("useUnloadResources"),
                    new GUIContent("Use Unload Resources?", "Runs Resources.UnloadUnusedAssets() if enabled."));
                
                // Show Logs...
                EditorGUILayout.PropertyField(SettingsAssetObject.FindProperty("showLogs"),
                    new GUIContent("Show Log Messages?", "Shows log messages for any errors as well as some handy debugging information."));
            }
            
            GUILayout.Space(2.5f);
            EditorGUILayout.EndVertical();
        }


        /// <summary>
        /// Draws the scene group options section of the window.
        /// </summary>
        private static void DrawSceneGroupOptions()
        {
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(1.5f);
            
            SettingsAssetObject.FindProperty("showSceneGroupOptions").boolValue = EditorGUILayout.Foldout(SettingsAssetObject.FindProperty("showSceneGroupOptions").boolValue, " Scene Group Management");
            
            if (SettingsAssetObject.FindProperty("showSceneGroupOptions").boolValue)
            {
                EditorGUILayout.PropertyField(SettingsAssetObject.FindProperty("sceneGroupLoadMode"),
                    new GUIContent("Group Load Mode", "Defines which group will load on play mode."));
                
                EditorGUILayout.PropertyField(SettingsAssetObject.FindProperty("startGroup"),
                    new GUIContent("Start Scene Group", "The scene group that is loaded first by the system."));
                
                GUI.enabled = false;
                
                EditorGUILayout.PropertyField(SettingsAssetObject.FindProperty("lastGroupLoaded"),
                    new GUIContent("Last Scene Group Loaded", "The last scene group to be loaded in the editor."));
                
                GUI.enabled = true;
            }

            GUILayout.Space(2.5f);
            EditorGUILayout.EndVertical();
        }
        
        
        /// <summary>
        /// Draws the scene group category section of the window.
        /// </summary>
        private static void DrawSceneGroupCategoryOptions()
        {
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(1.5f);
            
            SettingsAssetObject.FindProperty("showGroupCategoryOptions").boolValue = EditorGUILayout.Foldout(SettingsAssetObject.FindProperty("showGroupCategoryOptions").boolValue, " Scene Group Categories");
            
            if (SettingsAssetObject.FindProperty("showGroupCategoryOptions").boolValue)
            {
                EditorGUI.BeginChangeCheck();

                GUI.enabled = false;
                EditorGUILayout.PropertyField(SettingsAssetObject.FindProperty("defaultCategories"), new GUIContent("Pre Defined"));
                GUI.enabled = true;
                
                if (EditorGUI.EndChangeCheck())
                {
                    settingsAssetObject.ApplyModifiedProperties();
                    settingsAssetObject.Update();
                    MultiSceneEditorEvents.Settings.OnGroupCategoriesChanged.Raise();
                }
                
                EditorGUI.BeginChangeCheck();
                
                EditorGUILayout.PropertyField(SettingsAssetObject.FindProperty("userGroupCategories"), new GUIContent("User Defined"));
                
                if (EditorGUI.EndChangeCheck())
                {
                    settingsAssetObject.ApplyModifiedProperties();
                    settingsAssetObject.Update();
                    MultiSceneEditorEvents.Settings.OnGroupCategoriesChanged.Raise();
                }
            }
            
            GUILayout.Space(2.5f);
            EditorGUILayout.EndVertical();
        }
        

        /// <summary>
        /// Draws the buttons section of the window.
        /// </summary>
        private static void DrawButtons()
        {
            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button("GitHub", GUILayout.Height(30), GUILayout.MinWidth(100)))
                Application.OpenURL("https://github.com/CarterGames/MultiScene");

            if (GUILayout.Button("Documentation", GUILayout.Height(30), GUILayout.MinWidth(100)))
                Application.OpenURL("https://carter.games/multiscene");

            if (GUILayout.Button("Change Log", GUILayout.Height(30), GUILayout.MinWidth(100)))
                Application.OpenURL("https://carter.games/multiscene/changelog");

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Email", GUILayout.Height(30), GUILayout.MinWidth(100)))
                Application.OpenURL("mailto:support@carter.games?subject=Multi-Scene asset enquiry");

            if (GUILayout.Button("Discord", GUILayout.Height(30), GUILayout.MinWidth(100)))
                Application.OpenURL("https://carter.games/discord");

            if (GUILayout.Button("Report Issues", GUILayout.Height(30), GUILayout.MinWidth(100)))
                Application.OpenURL("https://carter.games/report");

            EditorGUILayout.EndHorizontal();

            if (MultiSceneEditorUtil.CarterGamesBanner != null)
            {
                var defaultTextColour = GUI.contentColor;
                GUI.contentColor = new Color(1, 1, 1, .75f);

                if (GUILayout.Button(MultiSceneEditorUtil.CarterGamesBanner, GUILayout.MaxHeight(40)))
                    Application.OpenURL("https://carter.games");

                GUI.contentColor = defaultTextColour;
            }
            else
            {
                if (GUILayout.Button("Carter Games", GUILayout.MaxHeight(40)))
                    Application.OpenURL("https://carter.games");
            }
        }
    }
}