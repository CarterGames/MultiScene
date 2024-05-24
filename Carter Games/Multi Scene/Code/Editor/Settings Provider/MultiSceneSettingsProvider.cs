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
using UnityEditor;
using UnityEngine;

namespace CarterGames.Experimental.MultiScene.Editor
{
    /// <summary>
    /// Handles the settings window for the asset.
    /// </summary>
    public static class MultiSceneSettingsProvider
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
        public static SerializedObject SettingsAssetObject => UtilEditor.SettingsObject;

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
            
            var provider = new SettingsProvider(UtilEditor.SettingsWindowPath, SettingsScope.Project)
            {
                guiHandler = (searchContext) =>
                {
                    if (UtilEditor.RuntimeSettings == null) return;

                    UtilEditor.DrawHeader();
                    DrawInfo();
                    
                    EditorGUILayout.BeginVertical("HelpBox");
                    GUILayout.Space(1.5f);
            
                    EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
                    GUILayout.Space(1.5f);
                    
                    EditorGUI.BeginChangeCheck();
                    
                    DrawEditorOptions();
                    DrawSceneManagementOptions();
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

            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.LabelField(new GUIContent("Version"),  new GUIContent(AssetVersionData.VersionNumber));
            VersionEditorGUI.DrawCheckForUpdatesButton();
            
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.LabelField(new GUIContent("Release Date", "The date this version of the asset was published on."), new GUIContent(AssetVersionData.ReleaseDate));

            GUILayout.Space(2.5f);
            EditorGUILayout.EndVertical();
        }


        /// <summary>
        /// Draws the general options section of the window.
        /// </summary>
        private static void DrawEditorOptions()
        {
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(1.5f);

            PerUserSettings.SettingsEditorDropdown = EditorGUILayout.Foldout(PerUserSettings.SettingsEditorDropdown, "Editor");
            
            if (PerUserSettings.SettingsEditorDropdown)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginVertical("Box");
                
                // Version validation
                PerUserSettings.VersionValidationAutoCheckOnLoad = EditorGUILayout.Toggle(new GUIContent("Update Check On Load",
                        "Checks for any updates to the asset from the GitHub page when you load the project."),
                    PerUserSettings.VersionValidationAutoCheckOnLoad);
                
                
                // Show Logs...
                PerUserSettingsRuntime.ShowDebugLogs = EditorGUILayout.Toggle(
                    new GUIContent("Show Log Messages?",
                        "Shows log messages for any errors as well as some handy debugging information."),
                    PerUserSettingsRuntime.ShowDebugLogs);
                

                if (GUILayout.Button("Reset Settings"))
                {
                    if (EditorUtility.DisplayDialog("Reset Multi Scene Settings",
                            "Are you sure that you want to reset all settings for the asset to their defaults. This only applies to you. Other users of the project will have to do this for their settings themselves.",
                            "Reset Settings", "Cancel"))
                    {
                        PerUserSettings.ResetPrefs();
                        PerUserSettingsRuntime.ResetPrefs();
                    }
                }
                
                EditorGUILayout.EndVertical();
                EditorGUI.indentLevel--;
            }
            
            GUILayout.Space(2.5f);
            EditorGUILayout.EndVertical();
        }


        /// <summary>
        /// Draws the scene group options section of the window.
        /// </summary>
        private static void DrawSceneManagementOptions()
        {
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(1.5f);
            
            PerUserSettings.SettingsSceneManagementDropdown = EditorGUILayout.Foldout(PerUserSettings.SettingsSceneManagementDropdown, "Scene Management");
            
            if (PerUserSettings.SettingsSceneManagementDropdown)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginVertical("Box");
                
                GUILayout.Space(1.5f);
                
                // Settings label...
                EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
                UtilEditor.DrawHorizontalGUILine();
                
                // Listener Frequency...
                EditorGUILayout.PropertyField(SettingsAssetObject.FindProperty("listenerFrequency"),
                    new GUIContent("Listener Frequency",
                        "Controls how many listeners execute per frame. The higher the number the more intensive the scene group loading can be."));
                
                // Use Unload Resources...
                EditorGUILayout.PropertyField(SettingsAssetObject.FindProperty("useUnloadResources"),
                    new GUIContent("Use Unload Resources?", "Runs Resources.UnloadUnusedAssets() if enabled."));
                
                
                GUILayout.Space(12.5f);
                
                // Settings label...
                EditorGUILayout.LabelField("Scene Group Auto Loading", EditorStyles.boldLabel);
                UtilEditor.DrawHorizontalGUILine();
                
                EditorGUILayout.PropertyField(SettingsAssetObject.FindProperty("sceneGroupLoadMode"),
                    new GUIContent("Group Load Mode", "Defines which group will load on play mode."));
                
                EditorGUILayout.PropertyField(SettingsAssetObject.FindProperty("startGroup"),
                    new GUIContent("Start Scene Group", "The scene group that is loaded first by the system."));
                
                GUI.enabled = false;
                
                EditorGUILayout.PropertyField(SettingsAssetObject.FindProperty("lastGroupLoaded"),
                    new GUIContent("Last Scene Group Loaded", "The last scene group to be loaded in the editor."));
                
                GUI.enabled = true;
                
                EditorGUILayout.EndVertical();
                EditorGUI.indentLevel--;
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
                    SettingsAssetObject.ApplyModifiedProperties();
                    SettingsAssetObject.Update();
                    MultiSceneEditorEvents.Settings.OnGroupCategoriesChanged.Raise();
                }
                
                EditorGUI.BeginChangeCheck();
                
                EditorGUILayout.PropertyField(SettingsAssetObject.FindProperty("userGroupCategories"), new GUIContent("User Defined"));
                
                if (EditorGUI.EndChangeCheck())
                {
                    SettingsAssetObject.ApplyModifiedProperties();
                    SettingsAssetObject.Update();
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
            
            if (GUILayout.Button("Buy Me A Coffee", GUILayout.Height(30), GUILayout.MinWidth(100)))
            {
                Application.OpenURL("https://carter.games/donate");
            }
            
            if (GUILayout.Button("GitHub", GUILayout.Height(30), GUILayout.MinWidth(100)))
            {
                Application.OpenURL("https://github.com/CarterGames/MultiScene");
            }

            if (GUILayout.Button("Documentation", GUILayout.Height(30), GUILayout.MinWidth(100)))
            {
                Application.OpenURL("https://carter.games/multiscene/");
            }

            if (GUILayout.Button("Support", GUILayout.Height(30), GUILayout.MinWidth(100)))
            {
                Application.OpenURL("https://carter.games/contact");
            }

            EditorGUILayout.EndHorizontal();

            if (UtilEditor.CarterGamesBanner != null)
            {
                var defaultTextColour = GUI.contentColor;
                GUI.contentColor = new Color(1, 1, 1, .75f);

                if (GUILayout.Button(UtilEditor.CarterGamesBanner, GUILayout.MaxHeight(40)))
                {
                    Application.OpenURL("https://carter.games");
                }

                GUI.contentColor = defaultTextColour;
            }
            else
            {
                if (GUILayout.Button("Carter Games", GUILayout.MaxHeight(40)))
                {
                    Application.OpenURL("https://carter.games");
                }
            }
        }
    }
}