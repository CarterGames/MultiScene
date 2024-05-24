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
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CarterGames.Experimental.MultiScene.Editor
{
    /// <summary>
    /// Handles the editor window for all the scene groups in the project...
    /// </summary>
    public sealed class SceneGroupLoader : EditorWindow
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        private List<SceneGroup> allGroups;
        private Color backgroundColor;

        private List<GroupCategory> allCategories = new List<GroupCategory>();
        private Vector2 scrollPos;

        private List<GroupCategory> validCategories = new List<GroupCategory>();
        private SceneGroup[] validGroups;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Menu Items
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */         

        /// <summary>
        /// Shows the scene group loader or focuses on it if it is already open.
        /// </summary>
        [MenuItem("Tools/Carter Games/Multi Scene/Scene Group Loader")]
        private static void ShowWindow()
        {
            var window = GetWindow<SceneGroupLoader>();
            window.titleContent = new GUIContent("Scene Group Loader")
            {
                image = UtilEditor.MultiSceneTransparentLogo
            };
            
            window.Show();
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Unity Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */ 
        
        private void OnEnable()
        {
            MultiSceneEditorEvents.Settings.OnSettingChanged.Add(UpdateData);
            MultiSceneEditorEvents.Settings.OnGroupCategoriesChanged.Add(UpdateData);

            MultiSceneEditorEvents.SceneGroups.OnSceneGroupCreated.Add(UpdateData);
            MultiSceneEditorEvents.SceneGroups.OnSceneGroupCategoryChanged.Add(UpdateData);

            allCategories = UtilEditor.RuntimeSettings.AllGroupCategories;
            backgroundColor = GUI.backgroundColor;
            GetAllGroups();
            UpdateData();
        }


        private void OnDisable()
        {
            MultiSceneEditorEvents.Settings.OnSettingChanged.Remove(UpdateData);
            MultiSceneEditorEvents.Settings.OnGroupCategoriesChanged.Remove(UpdateData);

            MultiSceneEditorEvents.SceneGroups.OnSceneGroupCreated.Remove(UpdateData);
            MultiSceneEditorEvents.SceneGroups.OnSceneGroupCategoryChanged.Remove(UpdateData);
        }


        private void OnGUI()
        {
            if (Application.isPlaying)
            {
                GUILayout.Space(5f);
                EditorGUILayout.HelpBox("You cannot use this window at runtime, please exit play mode to use this window.", MessageType.Warning);
                GUI.enabled = false;
            }
            else
            {
                GUI.enabled = true;
            }
            
            
            if (allGroups == null)
            {
                GetAllGroups();
            }

            if (allGroups == null) return;

            if (AssetDatabase.FindAssets("t:scenegroup", null).Length <= 0)
            {
                EditorGUILayout.HelpBox("There are no scene groups in the project currently. You will need to create a scene group for something to show here", MessageType.Info);
            }

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            DrawGroupsAndButtons(GUI.enabled);
            EditorGUILayout.EndScrollView();
        }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Updates the options for the scene group loader.
        /// </summary>
        private void UpdateData()
        {
            allCategories.Clear();
            UpdateCachedValues();
            GetAllGroups();

            validCategories.Clear();
            validCategories = GetValidCategories(out var groups);
            validGroups = groups;
            Repaint();
        }
        
        
        /// <summary>
        /// Geta all the categories that currently have scene groups in them.
        /// </summary>
        /// <param name="groups">The groups to search through.</param>
        /// <returns>A list of categories.</returns>
        private List<GroupCategory> GetValidCategories(out SceneGroup[] groups)
        {
            groups = allGroups.OrderBy(t => t.buttonIndex).ToArray();
            var validCats = new List<GroupCategory>();
                
            for (var i = 0; i < allCategories.Count; i++)
            {
                for (var j = 0; j < groups.Length; j++)
                {
                    if (!allCategories[i].groupName.Equals(groups[j].groupCategory)) continue;
                    if (validCats.Contains(allCategories[i])) continue;
                    validCats.Add(allCategories[i]);
                }
            }

            return validCats.OrderBy(t => t.groupIndex).ToList();
        }


        /// <summary>
        /// Draws all the buttons for the scene groups in the categories required.
        /// </summary>
        private void DrawGroupsAndButtons(bool isEnabled)
        {
            EditorGUILayout.Space(4f);

            foreach (var groupCat in validCategories)
            {
                if (validGroups.Count(t => t != null && t.groupCategory.Equals(groupCat.groupName)) <= 0) continue;
                
                EditorGUILayout.BeginVertical("HelpBox");

                EditorGUI.BeginChangeCheck();
                
                groupCat.showGroup = EditorGUILayout.Foldout(groupCat.showGroup, groupCat.groupName.Length > 0 ? groupCat.groupName : "No Category");
                
                if (EditorGUI.EndChangeCheck())
                {
                    var obj = MultiSceneSettingsProvider.SettingsAssetObject;

                    var dGroups = obj.Fp("defaultCategories");
                    var uGroups = obj.Fp("userGroupCategories");

                    for (var i = 0; i < dGroups.arraySize; i++)
                    {
                        if (!dGroups.GetIndex(i).Fpr("groupName").stringValue.Equals(groupCat.groupName)) continue;
                        dGroups.GetIndex(i).Fpr("showGroup").boolValue = groupCat.showGroup;
                        obj.ApplyModifiedProperties();
                        obj.Update();
                        goto EndCheck;
                    }

                    for (var i = 0; i < uGroups.arraySize; i++)
                    {
                        if (!uGroups.GetIndex(i).Fpr("groupName").stringValue.Equals(groupCat.groupName)) continue;
                        uGroups.GetIndex(i).Fpr("showGroup").boolValue = groupCat.showGroup;
                        obj.ApplyModifiedProperties();
                        obj.Update();
                    }
                    
                    EndCheck: ;
                }
                
                if (groupCat.showGroup)
                {
                    foreach (var group in validGroups)
                    {
                        GUI.backgroundColor = new Color(group.buttonColor.r, group.buttonColor.g, group.buttonColor.b,
                            1f);

                        if (group != null)
                        {
                            if (group.groupCategory.Equals(groupCat.groupName))
                            {
                                GUI.enabled = !group.ContainsScene(string.Empty) && group.IsValid && isEnabled;

                                if (GUILayout.Button(group.buttonLabel.Length > 0 ? group.buttonLabel : group.name))
                                {
                                    LoadSceneGroupInEditor(group);
                                }

                                GUI.enabled = isEnabled;
                            }
                        }

                        GUI.backgroundColor = backgroundColor;
                    }
                }
                
                EditorGUILayout.Space(1.5f);
                EditorGUILayout.EndVertical();
            }
        }


        /// <summary>
        /// Updates the cache of all categories in the asset when called.
        /// </summary>
        private void UpdateCachedValues()
        {
            allCategories = UtilEditor.RuntimeSettings.AllGroupCategories;
        }
        

        /// <summary>
        /// Gets all the scene groups in the project.
        /// </summary>
        private void GetAllGroups()
        {
            allGroups = AssetAccessor.GetAssets<SceneGroup>().Where(t => t != null).ToList();
        }
        
        
        /// <summary>
        /// Loads a scene group in the editor when called.
        /// </summary>
        /// <param name="group">The group to load.</param>
        private static void LoadSceneGroupInEditor(SceneGroup group)
        {
            var sceneList = new List<string>();

            for (var i = 0; i < group.scenes.Count; i++)
                sceneList.Add(group.scenes[i].sceneName);

            var paths = GetScenePaths();
            
            if (sceneList.Count <= 0) return;

            for (var i = 0; i < sceneList.Count; i++)
            {
                var scene = sceneList[i];
                var path = paths.FirstOrDefault(t => t.Contains(scene));

                if (i.Equals(0))
                {
                    EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
                }
                else
                {
                    EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
                }
            }

            UtilEditor.RuntimeSettings.LastGroup = group;
            MultiSceneEditorEvents.SceneGroups.OnSceneGroupLoadedInEditor.Raise();
        }
        
        
        /// <summary>
        /// Gets the paths for all scenes in the project build settings for use.
        /// </summary>
        /// <returns>A list of the paths of the scenes in the build settings.</returns>
        private static List<string> GetScenePaths()
        {
            var sceneNumber = SceneManager.sceneCountInBuildSettings;
            var arrayOfNames = new string[sceneNumber];
            
            for (var i = 0; i < sceneNumber; i++)
            {
                arrayOfNames[i] = SceneUtility.GetScenePathByBuildIndex(i);
            }

            return arrayOfNames.ToList();
        }
    }
}