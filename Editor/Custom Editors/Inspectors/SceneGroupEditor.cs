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
        
        private static Color _defaultBgCol;
        private static Color _defaultGUICol;

        private string[] allGroupOptions;
        private string[] buildSettingsOptions;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Unity Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */          
        
        private void OnEnable()
        {
            sceneGroupRef = target as SceneGroup;

            groupIndex = serializedObject.FindProperty("groupCategoryIndex");
            group = serializedObject.FindProperty("groupCategory");
            index = serializedObject.FindProperty("buttonIndex");
            color = serializedObject.FindProperty("buttonColor");
            label = serializedObject.FindProperty("buttonLabel");
            scenes = serializedObject.FindProperty("scenes");
            
            _defaultBgCol = GUI.backgroundColor;
            _defaultGUICol = GUI.color;
            
            allGroupOptions = MultiSceneEditorUtil.Settings.AllGroupCategories.Select(t => t.groupName).ToList().ToDisplayOptions();
            buildSettingsOptions = EditorSceneHelper.ScenesInBuildSettings.ToDisplayOptions();
            
            EditorSceneHelper.UpdateCaches();
            MultiSceneEditorEvents.Settings.OnGroupCategoriesChanged.Add(RefreshCategoryOptions);
        }

        
        private void OnDisable()
        {
            scenes = null;
            MultiSceneEditorEvents.Settings.OnGroupCategoriesChanged.Remove(RefreshCategoryOptions);
        }

        
        public override void OnInspectorGUI()
        {
            MultiSceneEditorUtil.DrawLogoOnly();
            
            // Renders the title for the group...
            DrawSceneGroupTitle();
            DrawScriptSection();
            DrawHelpBox();
            DrawMetaData();
            DrawToolsSection();
            DrawScenesSection();
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
        /// Renders the title section of the editor...
        /// </summary>
        private static void DrawSceneGroupTitle()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField(" Scene Group ", EditorStyles.boldLabel, GUILayout.Width(MultiSceneEditorUtil.TextWidth(" Scene Group ")));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        
        /// <summary>
        /// Draws the script field for this editor.
        /// </summary>
        private void DrawScriptSection()
        {
            GUILayout.Space(4.5f);
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
            
            EditorGUI.BeginChangeCheck();
            groupIndex.intValue = EditorGUILayout.Popup(new GUIContent("Group Name"), groupIndex.intValue, allGroupOptions);
            
            if (EditorGUI.EndChangeCheck())
            {
                group.stringValue = MultiSceneEditorUtil.Settings.AllGroupCategories[groupIndex.intValue].groupName;
                serializedObject.ApplyModifiedProperties();
                MultiSceneEditorEvents.SceneGroups.OnSceneGroupCategoryChanged.Raise();
            }
            
            EditorGUILayout.PropertyField(index, new GUIContent("Order In Group"));
            EditorGUILayout.PropertyField(label, new GUIContent("Button Label"));
            color.colorValue = EditorGUILayout.ColorField(new GUIContent("Button Colour"), color.colorValue, true, false, false);

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

            GUI.enabled = sceneGroupRef.IsValid;
            if (GUILayout.Button("Load Scenes"))
            {
                LoadSceneGroupInEditor();
            }
            GUI.enabled = true;
            
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

            scenes ??= serializedObject.FindProperty("scenes");
            
            // Shows the base field button if there are no entries in the scene group...
            if (scenes.arraySize <= 0)
            {
                GUI.backgroundColor = MultiSceneEditorUtil.Green;
                
                if (GUILayout.Button("Add Main Scene"))
                    CallAddBaseField();
                
                GUI.backgroundColor = _defaultBgCol;
            }
            else if (scenes.arraySize > 0)
            {
                RenderBaseSceneField();

                if (scenes.arraySize > 1)
                    RenderAdditiveSceneFields();
                else
                {
                    if (sceneGroupRef.IsValid)
                    {
                        GUI.backgroundColor = MultiSceneEditorUtil.Yellow;

                        if (GUILayout.Button("Add Additive Scene"))
                            CallAddNewAdditiveScene();

                        GUI.backgroundColor = _defaultBgCol;
                    }
                }
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
            GUILayout.Space(1.5f);
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(1.5f);
            
            EditorGUILayout.LabelField("Danger Zone", EditorStyles.boldLabel);
            
            GUI.backgroundColor = MultiSceneEditorUtil.Red;
                
            if (GUILayout.Button("Reset Group"))
                CallResetGroup();

            GUI.backgroundColor = _defaultBgCol;
            
            GUILayout.Space(1.5f);
            EditorGUILayout.EndVertical();
        }
        

        /// <summary>
        /// Adds the base scene to the editor
        /// </summary>
        private void CallAddBaseField()
        {
            scenes.InsertArrayElementAtIndex(0);
        }
        

        /// <summary>
        /// Renders the base scene grouping...
        /// </summary>
        private void RenderBaseSceneField()
        {
            GUI.backgroundColor = MultiSceneEditorUtil.Green;
            GUI.color = MultiSceneEditorUtil.Green;
            EditorGUILayout.LabelField("Main Scene", EditorStyles.boldLabel);
            GUI.color = _defaultGUICol;
            
            EditorGUI.BeginChangeCheck();
            
            scenes.GetArrayElementAtIndex(0).FindPropertyRelative("sceneName").stringValue = 
            EditorSceneHelper.ConvertIntToScene(EditorGUILayout.Popup(EditorSceneHelper.ConvertStringToIndex(scenes.GetArrayElementAtIndex(0).FindPropertyRelative("sceneName").stringValue), buildSettingsOptions));
            
            if (EditorGUI.EndChangeCheck())
            {
                var sceneName = scenes.GetArrayElementAtIndex(0).FindPropertyRelative("sceneName").stringValue;
                        
                if (EditorSceneHelper.GetAllScenesInProject().ContainsKey(sceneName))
                {
                    scenes.GetArrayElementAtIndex(0).FindPropertyRelative("scenePath").stringValue = EditorSceneHelper.ScenesInBuildSettings[sceneName];
                    scenes.GetArrayElementAtIndex(0).FindPropertyRelative("isInBuildSettings").boolValue = EditorSceneHelper.AllSceneNamesInProject.Contains(sceneName);
                }
                
                scenes.GetArrayElementAtIndex(0).serializedObject.ApplyModifiedProperties();
            }
            
            GUI.backgroundColor = _defaultBgCol;
        }


        /// <summary>
        /// Renders the additive scenes into a grouping...
        /// </summary>
        private void RenderAdditiveSceneFields()
        {
            GUI.backgroundColor = MultiSceneEditorUtil.Yellow;
            GUI.color = MultiSceneEditorUtil.Yellow;
            EditorGUILayout.LabelField("Additive Scene(s)", EditorStyles.boldLabel);
            GUI.color = _defaultGUICol;

            for (var i = 1; i < scenes.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();

                GUI.backgroundColor = MultiSceneEditorUtil.Yellow;

                EditorGUI.BeginChangeCheck();
                scenes.GetArrayElementAtIndex(i).FindPropertyRelative("sceneName").stringValue =
                    EditorSceneHelper.ConvertIntToScene(EditorGUILayout.Popup(
                        EditorSceneHelper.ConvertStringToIndex(scenes.GetArrayElementAtIndex(i)
                            .FindPropertyRelative("sceneName").stringValue), buildSettingsOptions));

                if (EditorGUI.EndChangeCheck())
                {
                    var sceneName = scenes.GetArrayElementAtIndex(i).FindPropertyRelative("sceneName").stringValue;

                    if (EditorSceneHelper.GetAllScenesInProject().ContainsKey(sceneName))
                    {
                        scenes.GetArrayElementAtIndex(i).FindPropertyRelative("scenePath").stringValue =
                            EditorSceneHelper.GetAllScenesInProject()[sceneName];
                        scenes.GetArrayElementAtIndex(i).FindPropertyRelative("isInBuildSettings").boolValue =
                            EditorSceneHelper.AllSceneNamesInProject.Contains(sceneName);
                    }

                    scenes.GetArrayElementAtIndex(i).serializedObject.ApplyModifiedProperties();
                }

                GUI.backgroundColor = MultiSceneEditorUtil.Green;
                
                if (GUILayout.Button("+", GUILayout.Width(MultiSceneEditorUtil.TextWidth("   +   "))))
                    CallAddNewAdditiveScene(scenes, i);
                
                GUI.backgroundColor = MultiSceneEditorUtil.Red;

                if (GUILayout.Button("-", GUILayout.Width(MultiSceneEditorUtil.TextWidth("   -   "))))
                    CallRemoveElementAtIndex(scenes, i);

                GUI.backgroundColor = _defaultBgCol;
      
                EditorGUILayout.EndHorizontal();
            }

            GUI.backgroundColor = _defaultBgCol;
        }


        /// <summary>
        /// Removed the element at the index entered...
        /// </summary>
        /// <param name="i">The element to edit</param>
        private void CallRemoveElementAtIndex(SerializedProperty prop, int i)
        {
            prop.DeleteArrayElementAtIndex(i);
        }

        /// <summary>
        /// Adds a new element to the scenes list that is blank at the element entered.
        /// </summary>
        /// <param name="i">The element to edit</param>
        private void CallAddNewAdditiveScene(SerializedProperty prop, int i)
        {
            prop.InsertArrayElementAtIndex(i);
            prop.GetArrayElementAtIndex(i + 1).FindPropertyRelative("sceneName").stringValue = string.Empty;
            scenes.GetArrayElementAtIndex(scenes.arraySize - 1).FindPropertyRelative("scenePath").stringValue = string.Empty;
            scenes.GetArrayElementAtIndex(scenes.arraySize - 1).FindPropertyRelative("isInBuildSettings").boolValue = false;
        }
        
        
        /// <summary>
        /// Adds a new element to the scenes list that is blank.
        /// </summary>
        private void CallAddNewAdditiveScene()
        {
            scenes.InsertArrayElementAtIndex(scenes.arraySize - 1);
            scenes.GetArrayElementAtIndex(scenes.arraySize - 1).FindPropertyRelative("sceneName").stringValue = string.Empty;
            scenes.GetArrayElementAtIndex(scenes.arraySize - 1).FindPropertyRelative("scenePath").stringValue = string.Empty;
            scenes.GetArrayElementAtIndex(scenes.arraySize - 1).FindPropertyRelative("isInBuildSettings").boolValue = false;
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
            var _sceneList = new List<string>();

            for (var i = 0; i < scenes.arraySize; i++)
            {
                var path = scenes.GetArrayElementAtIndex(i).FindPropertyRelative("scenePath").stringValue;
                if (path.Length <= 0)
                {
                    MsLog.Error(
                        "Unable to load group in editor as a scene doesn't have a valid path to load from...");
                    return;
                }
                _sceneList.Add(scenes.GetArrayElementAtIndex(i).FindPropertyRelative("scenePath").stringValue);
            }
            
            if (_sceneList.Count <= 0) return;

            for (var i = 0; i < _sceneList.Count; i++)
            {
                var _scene = _sceneList[i];

                if (i.Equals(0))
                    EditorSceneManager.OpenScene(_scene, OpenSceneMode.Single);
                else
                    EditorSceneManager.OpenScene(_scene, OpenSceneMode.Additive);
            }
            
            MultiSceneEditorUtil.Settings.LastGroup = target as SceneGroup;
            MultiSceneEditorEvents.SceneGroups.OnSceneGroupLoadedInEditor.Raise();
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Utility Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */    

        private void RefreshCategoryOptions()
        {
            allGroupOptions = MultiSceneEditorUtil.Settings.AllGroupCategories.Select(t => t.groupName).ToList().ToDisplayOptions();

            if (groupIndex.intValue >= allGroupOptions.Length)
            {
                groupIndex.intValue = 0;
                group.stringValue = MultiSceneEditorUtil.Settings.AllGroupCategories[groupIndex.intValue].groupName;

                MsLog.Warning($"The category that was selected by: {target.name} has been removed. The group was moved to unassigned automatically due to this change.");
            }
            
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }
    }
}