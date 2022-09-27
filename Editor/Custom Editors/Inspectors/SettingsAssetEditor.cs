using UnityEditor;
using UnityEngine;

namespace CarterGames.Experimental.MultiScene.Editor
{
    /// <summary>
    /// Custom Inspector for the Multi Scene Settings Asset...
    /// </summary>
    [CustomEditor(typeof(MultiSceneSettingsAsset))]
    public sealed class SettingsAssetEditor : UnityEditor.Editor
    {
        
/* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
 {  Fields  }
───────────────────────────────────────────────────────────────────────────────────────────────────────────────────── */        
        
        private SerializedProperty loadModeProp;
        private SerializedProperty startGroupProp;
        private SerializedProperty lastGroupProp;
        
        private SerializedProperty listenerFreqProp;
        private SerializedProperty unloadResourcesProp;
        private SerializedProperty showLogsProp;
        
        private SerializedProperty userGroupProp;
        private SerializedProperty defaultGroupProp;
        private SerializedProperty showUserGroupProp;
        private SerializedProperty showDefaultGroupProp;

        private Color defaultBackgroundColor;

/* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
 {  Unity Methods  }
───────────────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        private void OnEnable()
        {
            loadModeProp = serializedObject.FindProperty("sceneGroupLoadMode");
            startGroupProp = serializedObject.FindProperty("startGroup");
            lastGroupProp = serializedObject.FindProperty("lastGroupLoaded");

            listenerFreqProp = serializedObject.FindProperty("listenerFrequency");
            unloadResourcesProp = serializedObject.FindProperty("useUnloadResources");
            showLogsProp = serializedObject.FindProperty("showLogs");

            userGroupProp = serializedObject.FindProperty("userGroupCategories");
            defaultGroupProp = serializedObject.FindProperty("defaultCategories");
            showUserGroupProp = serializedObject.FindProperty("showUserGroupsInSetAsset");
            showDefaultGroupProp = serializedObject.FindProperty("showDefaultGroupsInSetAsset");

            defaultBackgroundColor = GUI.backgroundColor;
            
            MultiSceneEditorEvents.Settings.OnSettingChanged.Add(OnSettingUpdate);
        }


        private void OnDisable()
        {
            MultiSceneEditorEvents.Settings.OnSettingChanged.Remove(OnSettingUpdate);
        }


        public override void OnInspectorGUI()
        {
            MultiSceneEditorUtil.DrawSettingsIconOnly();
            DrawScriptSection();
            DrawEditSettingsButton();
            DrawGeneralOptions();
            DrawSceneGroupOptions();
            DrawDefaultSceneGroupCategory();
            DrawUserSceneGroupCategory();
            serializedObject.Update();
        }

        
        private void OnSettingUpdate()
        {
            Repaint();
        }
        
/* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
 {  Drawer Methods  }
───────────────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Draws the script fields in the custom inspector...
        /// </summary>
        private void DrawScriptSection()
        {
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(1.5f);
            
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script:", MonoScript.FromScriptableObject(target as MultiSceneSettingsAsset), typeof(MultiSceneSettingsAsset), false);
            GUI.enabled = true;
        }
        
        
        /// <summary>
        /// Draws the settings button in the custom inspector...
        /// </summary>
        private void DrawEditSettingsButton()
        {
            GUILayout.Space(2f);
            GUI.backgroundColor = MultiSceneEditorUtil.Green;
            
            if (GUILayout.Button("Edit Settings", GUILayout.Height(25f)))
                SettingsService.OpenProjectSettings("Project/Carter Games/Multi Scene");

            GUI.backgroundColor = defaultBackgroundColor;
            
            GUILayout.Space(2.5f);
            EditorGUILayout.EndVertical();
        }


        /// <summary>
        /// Draws the general options in the custom inspector...
        /// </summary>
        private void DrawGeneralOptions()
        {
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(1.5f);

            EditorGUILayout.LabelField("General Options", EditorStyles.boldLabel);

            GUI.enabled = false;
            EditorGUILayout.PropertyField(listenerFreqProp);
            EditorGUILayout.PropertyField(unloadResourcesProp);
            EditorGUILayout.PropertyField(showLogsProp);
            GUI.enabled = true;

            GUILayout.Space(2.5f);
            EditorGUILayout.EndVertical();
        }
        
        
        /// <summary>
        /// Draws the scene group options in the custom inspector...
        /// </summary>
        private void DrawSceneGroupOptions()
        {
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(1.5f);
            
            EditorGUILayout.LabelField("Scene Group Options", EditorStyles.boldLabel);

            GUI.enabled = false;
            EditorGUILayout.PropertyField(loadModeProp);
            EditorGUILayout.PropertyField(startGroupProp);
            EditorGUILayout.PropertyField(lastGroupProp);
            GUI.enabled = true;
            
            GUILayout.Space(2.5f);
            EditorGUILayout.EndVertical();
        }
        
        
        /// <summary>
        /// Draws the pre-defined scene group category options in the custom inspector...
        /// </summary>
        private void DrawDefaultSceneGroupCategory()
        {
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(1.5f);

            EditorGUILayout.LabelField("Scene Group Categories", EditorStyles.boldLabel);

            GUI.enabled = false;
            
            EditorGUI.indentLevel++;
            
            EditorGUI.BeginChangeCheck();
            showDefaultGroupProp.boolValue = EditorGUILayout.Foldout(showDefaultGroupProp.boolValue, "Pre Defined");
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
            }
            
            EditorGUI.indentLevel--;
            
            if (showDefaultGroupProp.boolValue)
            {
                EditorGUILayout.BeginVertical("HelpBox");
                GUILayout.Space(2f);
                
                for (var i = 0; i < defaultGroupProp.arraySize; i++)
                {
                    var name = defaultGroupProp.GetArrayElementAtIndex(i).FindPropertyRelative("groupName");
                    var index = defaultGroupProp.GetArrayElementAtIndex(i).FindPropertyRelative("groupIndex");
                    var show = defaultGroupProp.GetArrayElementAtIndex(i).FindPropertyRelative("showGroup");

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(name, new GUIContent(name.stringValue.Length > 0 ? name.stringValue : "Element " + i));
                    EditorGUILayout.PropertyField(index, GUIContent.none, GUILayout.Width(65));
                    EditorGUILayout.PropertyField(show, GUIContent.none, GUILayout.Width(65));
                    EditorGUILayout.EndHorizontal();
                }

                GUILayout.Space(2f);
                EditorGUILayout.EndVertical();
            }
        }
        
        
        /// <summary>
        /// Draws the user-defined scene group category options in the custom inspector...
        /// </summary>
        private void DrawUserSceneGroupCategory()
        {
            EditorGUI.indentLevel++;
            
            EditorGUI.BeginChangeCheck();
            showUserGroupProp.boolValue = EditorGUILayout.Foldout(showUserGroupProp.boolValue, "User Defined");
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
            }
            
            EditorGUI.indentLevel--;
            
            if (showUserGroupProp.boolValue)
            {
                EditorGUILayout.BeginVertical("HelpBox");
                GUILayout.Space(2f);
                
                for (var i = 0; i < userGroupProp.arraySize; i++)
                {
                    var name = userGroupProp.GetArrayElementAtIndex(i).FindPropertyRelative("groupName");
                    var index = userGroupProp.GetArrayElementAtIndex(i).FindPropertyRelative("groupIndex");
                    var show = userGroupProp.GetArrayElementAtIndex(i).FindPropertyRelative("showGroup");

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(name, new GUIContent(name.stringValue.Length > 0 ? name.stringValue : "Element " + i));
                    EditorGUILayout.PropertyField(index, GUIContent.none, GUILayout.Width(65));
                    EditorGUILayout.PropertyField(show, GUIContent.none, GUILayout.Width(65));
                    EditorGUILayout.EndHorizontal();
                }

                GUILayout.Space(2f);
                EditorGUILayout.EndVertical();
            }
            
            GUI.enabled = true;
            GUILayout.Space(2.5f);
            EditorGUILayout.EndVertical();
        }
    }
}