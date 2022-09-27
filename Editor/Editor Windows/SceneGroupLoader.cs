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

        [MenuItem("Tools/Carter Games/Multi Scene/Scene Group Loader")]
        private static void ShowWindow()
        {
            var window = GetWindow<SceneGroupLoader>();
            window.titleContent = new GUIContent("Scene Group Loader")
            {
                image = MultiSceneEditorUtil.LogoTransparent
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

            allCategories = MultiSceneEditorUtil.Settings.AllGroupCategories;
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
                GetAllGroups();

            if (allGroups == null) return;

            if (AssetDatabase.FindAssets("t:scenegroup", null).Length <= 0)
            {
                EditorGUILayout.HelpBox("There are no scene groups in the project currently. You will need to create a scene group for something to show here", MessageType.Info);
            }

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            DrawGroupsAndButtons();
            EditorGUILayout.EndScrollView();
        }


        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

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


        private void DrawGroupsAndButtons()
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
                    var obj = MultiSceneSettings.SettingsAssetObject;

                    var dGroups = obj.FindProperty("defaultCategories");
                    var uGroups = obj.FindProperty("userGroupCategories");

                    for (var i = 0; i < dGroups.arraySize; i++)
                    {
                        if (!dGroups.GetArrayElementAtIndex(i).FindPropertyRelative("groupName").stringValue.Equals(groupCat.groupName)) continue;
                        dGroups.GetArrayElementAtIndex(i).FindPropertyRelative("showGroup").boolValue = groupCat.showGroup;
                        obj.ApplyModifiedProperties();
                        obj.Update();
                        goto EndCheck;
                    }

                    for (var i = 0; i < uGroups.arraySize; i++)
                    {
                        if (!uGroups.GetArrayElementAtIndex(i).FindPropertyRelative("groupName").stringValue.Equals(groupCat.groupName)) continue;
                        uGroups.GetArrayElementAtIndex(i).FindPropertyRelative("showGroup").boolValue = groupCat.showGroup;
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
                                GUI.enabled = !group.ContainsScene(string.Empty) && group.IsValid;

                                if (GUILayout.Button(group.buttonLabel.Length > 0 ? group.buttonLabel : group.name))
                                {
                                    LoadSceneGroupInEditor(group);
                                }

                                GUI.enabled = true;
                            }
                        }

                        GUI.backgroundColor = backgroundColor;
                    }
                }
                
                EditorGUILayout.Space(1.5f);
                EditorGUILayout.EndVertical();
            }
        }


        private void UpdateCachedValues()
        {
            allCategories = MultiSceneEditorUtil.Settings.AllGroupCategories;
        }
        

        private void GetAllGroups()
        {
            var _assetsFound = AssetDatabase.FindAssets("t:scenegroup", null);
            
            if (_assetsFound.Length <= 0) return;
            
            allGroups = new List<SceneGroup>();
            
            foreach (var _asset in _assetsFound)
            {
                var _path = AssetDatabase.GUIDToAssetPath(_asset);
                var _loadedGroup = (SceneGroup)AssetDatabase.LoadAssetAtPath(_path, typeof(SceneGroup));
                
                allGroups.Add(_loadedGroup);
            }
        }
        
        
        private static void LoadSceneGroupInEditor(SceneGroup group)
        {
            var _sceneList = new List<string>();

            for (var i = 0; i < group.scenes.Count; i++)
                _sceneList.Add(group.scenes[i].sceneName);

            var _paths = GetScenePaths();
            if (_sceneList.Count <= 0) return;

            for (var i = 0; i < _sceneList.Count; i++)
            {
                var _scene = _sceneList[i];
                var _path = _paths.FirstOrDefault(t => t.Contains(_scene));

                if (i.Equals(0))
                    EditorSceneManager.OpenScene(_path, OpenSceneMode.Single);
                else
                    EditorSceneManager.OpenScene(_path, OpenSceneMode.Additive);
            }

            MultiSceneEditorUtil.Settings.LastGroup = group;
            MultiSceneEditorEvents.SceneGroups.OnSceneGroupLoadedInEditor.Raise();
        }
        
        
        private static List<string> GetScenePaths()
        {
            var sceneNumber = SceneManager.sceneCountInBuildSettings;
            string[] arrayOfNames;
            arrayOfNames = new string[sceneNumber];
            
            for (int i = 0; i < sceneNumber; i++)
                arrayOfNames[i] = SceneUtility.GetScenePathByBuildIndex(i);

            return arrayOfNames.ToList();
        }
    }
}