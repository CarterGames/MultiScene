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
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace CarterGames.Experimental.MultiScene.Editor
{
    /// <summary>
    /// Custom Inspector for the SceneGroup scriptable object.
    /// </summary>
    [CustomEditor(typeof(SceneGroup))]
    public sealed class SceneGroupEditor : UnityEditor.Editor
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */      

        private SerializedProperty groupIndex;
        private SerializedProperty group;
        private SerializedProperty index;
        private SerializedProperty color;
        private SerializedProperty label;
        private SerializedProperty scenes;

        private SceneGroup sceneGroupRef;

        private string[] allGroupOptions;
        private string[] buildSettingsOptions;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Unity Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */          
        
        private void OnEnable()
        {
            sceneGroupRef = target as SceneGroup;

            groupIndex = serializedObject.Fp("groupCategoryIndex");
            group = serializedObject.Fp("groupCategory");
            index = serializedObject.Fp("buttonIndex");
            color = serializedObject.Fp("buttonColor");
            label = serializedObject.Fp("buttonLabel");
            scenes = serializedObject.Fp("scenes");
            
            allGroupOptions = UtilEditor.RuntimeSettings.AllGroupCategories.Select(t => t.groupName).ToList().ToDisplayOptions();
            buildSettingsOptions = EditorSceneHelper.ScenesInBuildSettings.ToDisplayOptions();
            
            EditorSceneHelper.UpdateCaches();
            EditorSceneHelper.OnCacheUpdate.Add(UpdateSceneNames);
            MultiSceneEditorEvents.Settings.OnGroupCategoriesChanged.Add(RefreshCategoryOptions);
        }

        
        private void OnDisable()
        {
            scenes = null;
            EditorSceneHelper.OnCacheUpdate.Remove(UpdateSceneNames);
            MultiSceneEditorEvents.Settings.OnGroupCategoriesChanged.Remove(RefreshCategoryOptions);
        }

        
        public override void OnInspectorGUI()
        {
            // Renders the title for the group...
            GUILayout.Space(7.5f);
            DrawScriptSection();
            GUILayout.Space(5f);
            DrawHelpBox();
            GUILayout.Space(5f);
            DrawMetaData();
            GUILayout.Space(5f);
            DrawToolsSection();
            GUILayout.Space(5f);
            DrawScenesSection();
            GUILayout.Space(5f);
            DrawDangerZoneSection();
            
            // Applies changes if changes have been made...
            if (!serializedObject.hasModifiedProperties) return;
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Draw Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        
        /// <summary>
        /// Updates the scene names options.
        /// </summary>
        private void UpdateSceneNames()
        {
            buildSettingsOptions = EditorSceneHelper.ScenesInBuildSettings.ToDisplayOptions();
        }

        
        /// <summary>
        /// Draws the script field for this editor.
        /// </summary>
        private void DrawScriptSection()
        {
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(1.5f);
            
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script:", MonoScript.FromScriptableObject(target as SceneGroup), typeof(SceneGroup), false);
            GUI.enabled = true;
            
            GUILayout.Space(1.5f);
            EditorGUILayout.EndVertical();
        }
        

        /// <summary>
        /// Draws a help box with some important info to show.
        /// </summary>
        private void DrawHelpBox()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.HelpBox("Scene groups control a \"scene\" in the multi-scene setup, each group is its own scene collection which can be loaded and unloaded easily.", MessageType.Info);
            EditorGUILayout.EndVertical();
        }


        /// <summary>
        /// Shows the meta data section of the group.
        /// </summary>
        private void DrawMetaData()
        {
            GUILayout.Space(1.5f);
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(1.5f);
            
            EditorGUILayout.LabelField("Meta Data", EditorStyles.boldLabel);
            UtilEditor.DrawHorizontalGUILine();
            
            EditorGUI.BeginChangeCheck();
            
            groupIndex.intValue = EditorGUILayout.Popup(new GUIContent("Group Name"), groupIndex.intValue, allGroupOptions);
            EditorGUILayout.PropertyField(index, new GUIContent("Order In Group"));
            EditorGUILayout.PropertyField(label, new GUIContent("Button Label"));
            color.colorValue = EditorGUILayout.ColorField(new GUIContent("Button Colour"), color.colorValue, true, false, false);

            if (EditorGUI.EndChangeCheck())
            {
                group.stringValue = UtilEditor.RuntimeSettings.AllGroupCategories[groupIndex.intValue].groupName;
                serializedObject.ApplyModifiedProperties();
                MultiSceneEditorEvents.SceneGroups.OnSceneGroupCategoryChanged.Raise();
            }
            
            GUILayout.Space(1.5f);
            EditorGUILayout.EndVertical();
            GUILayout.Space(1.5f);
        }


        /// <summary>
        /// Draws the tool buttons section.
        /// </summary>
        private void DrawToolsSection()
        {
            GUILayout.Space(1.5f);
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(1.5f);
            
            EditorGUILayout.LabelField("Tools", EditorStyles.boldLabel);
            UtilEditor.DrawHorizontalGUILine();

            EditorGUI.BeginDisabledGroup(!sceneGroupRef.IsValid);
            
            if (GUILayout.Button("Load Scenes"))
            {
                LoadSceneGroupInEditor();
            }
            
            EditorGUI.EndDisabledGroup();
            
            GUILayout.Space(1.5f);
            EditorGUILayout.EndVertical();
            GUILayout.Space(1.5f);
        }


        /// <summary>
        /// Draws the scene field section.
        /// </summary>
        private void DrawScenesSection()
        {
            GUILayout.Space(1.5f);
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(1.5f);
            
            EditorGUILayout.LabelField("Scenes", EditorStyles.boldLabel);
            UtilEditor.DrawHorizontalGUILine();

            scenes ??= serializedObject.Fp("scenes");
            
            // Shows the base field button if there are no entries in the scene group...
            if (scenes.arraySize <= 0)
            {
                GUI.backgroundColor = UtilEditor.Green;
                
                if (GUILayout.Button("Add Main Scene"))
                {
                    CallAddBaseField();
                }
                
                GUI.backgroundColor = Color.white;
            }
            else if (scenes.arraySize > 0)
            {
                EditorGUILayout.BeginVertical("Box");
                RenderBaseSceneField();
                EditorGUILayout.EndVertical();
                
                GUILayout.Space(2.5f);
                
                EditorGUILayout.BeginVertical("Box");
                RenderAdditiveSceneFields();
                EditorGUILayout.EndVertical();
            }

            GUILayout.Space(1.5f);
            EditorGUILayout.EndVertical();
            GUILayout.Space(1.5f);
        }


        /// <summary>
        /// Draws the danger zone section.
        /// </summary>
        private void DrawDangerZoneSection()
        {
            GUI.backgroundColor = UtilEditor.Red;
            
            GUILayout.Space(1.5f);
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(1.5f);
            
            EditorGUILayout.LabelField("Danger Zone", EditorStyles.boldLabel);
            
            GUI.backgroundColor = Color.white;
            UtilEditor.DrawHorizontalGUILine();
            GUI.backgroundColor = UtilEditor.Red;

            if (GUILayout.Button("Reset Group"))
            {
                CallResetGroup();
            }

            GUILayout.Space(1.5f);
            EditorGUILayout.EndVertical();
            
            GUI.backgroundColor = Color.white;
        }
        

        /// <summary>
        /// Adds the base scene to the editor
        /// </summary>
        private void CallAddBaseField()
        {
            scenes.InsertIndex(0);
        }
        

        /// <summary>
        /// Renders the base scene grouping...
        /// </summary>
        private void RenderBaseSceneField()
        {
            GUI.backgroundColor = UtilEditor.Green;
            
            EditorGUILayout.LabelField("Main Scene", EditorStyles.boldLabel);
            
            EditorGUI.BeginChangeCheck();
            
            var options = UnusedSceneOptions(scenes.GetIndex(0).Fpr("sceneName").stringValue);
            
            scenes.GetIndex(0).Fpr("sceneName").stringValue = 
            EditorSceneHelper.ConvertIntToScene(EditorGUILayout.Popup(EditorSceneHelper.ConvertStringToIndex(scenes.GetIndex(0).Fpr("sceneName").stringValue, options), options), options);
            
            if (EditorGUI.EndChangeCheck())
            {
                var sceneName = scenes.GetIndex(0).Fpr("sceneName").stringValue;
                        
                if (EditorSceneHelper.GetAllScenesInProject().ContainsKey(sceneName))
                {
                    scenes.GetIndex(0).Fpr("scenePath").stringValue = EditorSceneHelper.ScenesInBuildSettings[sceneName];
                    scenes.GetIndex(0).Fpr("isInBuildSettings").boolValue = EditorSceneHelper.AllSceneNamesInProject.Contains(sceneName);
                }
                
                scenes.GetIndex(0).serializedObject.ApplyModifiedProperties();
            }
            
            GUI.backgroundColor = Color.white;
        }


        /// <summary>
        /// Renders the additive scenes into a grouping...
        /// </summary>
        private void RenderAdditiveSceneFields()
        {
            GUI.backgroundColor = UtilEditor.Yellow;
            
            EditorGUILayout.LabelField("Additive Scene(s)", EditorStyles.boldLabel);

            if (scenes.arraySize <= 1)
            {
                if (sceneGroupRef.IsValid)
                {
                    GUI.backgroundColor = UtilEditor.Yellow;

                    if (GUILayout.Button("Add Additive Scene"))
                    {
                        CallAddNewAdditiveScene();
                    }

                    GUI.backgroundColor = Color.white;
                }
            }
            else
            {
                for (var i = 1; i < scenes.arraySize; i++)
                {
                    EditorGUILayout.BeginHorizontal();

                    GUI.backgroundColor = UtilEditor.Yellow;

                    EditorGUI.BeginChangeCheck();

                    var options = UnusedSceneOptions(scenes.GetIndex(i).Fpr("sceneName").stringValue);
                    
                    scenes.GetIndex(i).Fpr("sceneName").stringValue =
                        EditorSceneHelper.ConvertIntToScene(EditorGUILayout.Popup(
                            EditorSceneHelper.ConvertStringToIndex(scenes.GetIndex(i)
                                .Fpr("sceneName").stringValue, options), options), options);

                    if (EditorGUI.EndChangeCheck())
                    {
                        var sceneName = scenes.GetIndex(i).Fpr("sceneName").stringValue;

                        if (EditorSceneHelper.GetAllScenesInProject().ContainsKey(sceneName))
                        {
                            scenes.GetIndex(i).Fpr("scenePath").stringValue = EditorSceneHelper.GetAllScenesInProject()[sceneName];
                            scenes.GetIndex(i).Fpr("isInBuildSettings").boolValue = EditorSceneHelper.AllSceneNamesInProject.Contains(sceneName);
                        }

                        scenes.GetIndex(i).serializedObject.ApplyModifiedProperties();
                    }

                    GUI.backgroundColor = UtilEditor.Green;

                    if (GUILayout.Button("+", GUILayout.Width("   +   ".Width())))
                    {
                        CallAddNewAdditiveScene(scenes, i);
                    }

                    GUI.backgroundColor = UtilEditor.Red;

                    if (GUILayout.Button("-", GUILayout.Width("   -   ".Width())))
                    {
                        CallRemoveElementAtIndex(scenes, i);
                    }

                    GUI.backgroundColor = Color.white;

                    EditorGUILayout.EndHorizontal();
                }
            }

            GUI.backgroundColor = Color.white;
        }


        /// <summary>
        /// Removed the element at the index entered...
        /// </summary>
        /// <param name="i">The element to edit</param>
        private void CallRemoveElementAtIndex(SerializedProperty prop, int i)
        {
            prop.DeleteIndex(i);
        }

        
        /// <summary>
        /// Adds a new element to the scenes list that is blank at the element entered.
        /// </summary>
        /// <param name="i">The element to edit</param>
        private void CallAddNewAdditiveScene(SerializedProperty prop, int i)
        {
            prop.InsertIndex(i);
            prop.GetIndex(i + 1).Fpr("sceneName").stringValue = string.Empty;
            scenes.GetIndex(scenes.arraySize - 1).Fpr("scenePath").stringValue = string.Empty;
            scenes.GetIndex(scenes.arraySize - 1).Fpr("isInBuildSettings").boolValue = false;
        }
        
        
        /// <summary>
        /// Adds a new element to the scenes list that is blank.
        /// </summary>
        private void CallAddNewAdditiveScene()
        {
            scenes.InsertIndex(scenes.arraySize - 1);
            scenes.GetIndex(scenes.arraySize - 1).Fpr("sceneName").stringValue = string.Empty;
            scenes.GetIndex(scenes.arraySize - 1).Fpr("scenePath").stringValue = string.Empty;
            scenes.GetIndex(scenes.arraySize - 1).Fpr("isInBuildSettings").boolValue = false;
            scenes.serializedObject.ApplyModifiedProperties();
            scenes.serializedObject.Update();
        }

        
        /// <summary>
        /// Resets the scenes list to a new list.
        /// </summary>
        private void CallResetGroup()
        {
            if (!EditorUtility.DisplayDialog("Clear Scene Group",
                    "Are you sure that you want to clear this scene group? This action cannot be undone once performed.",
                    "Yes", "No")) return;
            
            typeof(SceneGroup).GetMethod("ClearAsset", BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(target, null);
            serializedObject.Update();
        }
        
        
        /// <summary>
        /// Loads the scene group in the editor on call.
        /// </summary>
        private void LoadSceneGroupInEditor()
        {
            var sceneList = new List<string>();

            for (var i = 0; i < scenes.arraySize; i++)
            {
                var path = scenes.GetIndex(i).Fpr("scenePath").stringValue;
                
                if (path.Length <= 0)
                {
                    MultiSceneLogger.Error(
                        "Unable to load group in editor as a scene doesn't have a valid path to load from...");
                    return;
                }
                
                sceneList.Add(scenes.GetIndex(i).Fpr("scenePath").stringValue);
            }
            
            if (sceneList.Count <= 0) return;

            for (var i = 0; i < sceneList.Count; i++)
            {
                var _scene = sceneList[i];

                if (i.Equals(0))
                {
                    EditorSceneManager.OpenScene(_scene, OpenSceneMode.Single);
                }
                else
                {
                    EditorSceneManager.OpenScene(_scene, OpenSceneMode.Additive);
                }
            }
            
            UtilEditor.RuntimeSettings.LastGroup = target as SceneGroup;
            MultiSceneEditorEvents.SceneGroups.OnSceneGroupLoadedInEditor.Raise();
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Utility Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */    

        private void RefreshCategoryOptions()
        {
            allGroupOptions = UtilEditor.RuntimeSettings.AllGroupCategories.Select(t => t.groupName).ToList().ToDisplayOptions();

            if (groupIndex.intValue >= allGroupOptions.Length)
            {
                groupIndex.intValue = 0;
                group.stringValue = UtilEditor.RuntimeSettings.AllGroupCategories[groupIndex.intValue].groupName;

                MultiSceneLogger.Warning($"The category that was selected by: {target.name} has been removed. The group was moved to unassigned automatically due to this change.");
            }
            
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }


        private string[] UnusedSceneOptions(string currentlySelected)
        {
            var list = new List<string>();

            foreach (var sceneName in buildSettingsOptions)
            {
                if (currentlySelected != null)
                {
                    if (sceneName == currentlySelected)
                    {
                        list.Add(sceneName);
                        continue;
                    }
                }
                
                for (var i = 0; i < scenes.arraySize; i++)
                {
                    if (scenes.GetIndex(i).Fpr("sceneName").stringValue == sceneName) goto SkipAdd;
                }
                
                list.Add(sceneName);
                SkipAdd: ;
            }

            return list.ToArray();
        }
    }
}