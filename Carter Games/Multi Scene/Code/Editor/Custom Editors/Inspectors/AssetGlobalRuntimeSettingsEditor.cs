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

using UnityEditor;
using UnityEngine;

namespace CarterGames.Experimental.MultiScene.Editor
{
    /// <summary>
    /// Custom Inspector for the Multi Scene Settings Asset...
    /// </summary>
    [CustomEditor(typeof(AssetGlobalRuntimeSettings))]
    public sealed class AssetGlobalRuntimeSettingsEditor : UnityEditor.Editor
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */        
        
        private SerializedProperty loadModeProp;
        private SerializedProperty startGroupProp;
        private SerializedProperty lastGroupProp;
        
        private SerializedProperty listenerFreqProp;
        private SerializedProperty unloadResourcesProp;
        
        private SerializedProperty userGroupProp;
        private SerializedProperty defaultGroupProp;
        private SerializedProperty showUserGroupProp;
        private SerializedProperty showDefaultGroupProp;

        private Color defaultBackgroundColor;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Unity Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */ 

        private void OnEnable()
        {
            loadModeProp = serializedObject.Fp("sceneGroupLoadMode");
            startGroupProp = serializedObject.Fp("startGroup");
            lastGroupProp = serializedObject.Fp("lastGroupLoaded");

            listenerFreqProp = serializedObject.Fp("listenerFrequency");
            unloadResourcesProp = serializedObject.Fp("useUnloadResources");

            userGroupProp = serializedObject.Fp("userGroupCategories");
            defaultGroupProp = serializedObject.Fp("defaultCategories");
            showUserGroupProp = serializedObject.Fp("showUserGroupsInSetAsset");
            showDefaultGroupProp = serializedObject.Fp("showDefaultGroupsInSetAsset");
            
            MultiSceneEditorEvents.Settings.OnSettingChanged.Add(OnSettingUpdate);
        }


        private void OnDisable()
        {
            MultiSceneEditorEvents.Settings.OnSettingChanged.Remove(OnSettingUpdate);
        }


        public override void OnInspectorGUI()
        {
            GUILayout.Space(7.5f);
            DrawScriptSection();
            GUILayout.Space(5f);
            
            DrawGeneralOptions();
            GUILayout.Space(5f);
            DrawSceneGroupOptions();
            GUILayout.Space(5f);
            DrawDefaultSceneGroupCategory();
            DrawUserSceneGroupCategory();
            GUILayout.Space(5f);
            
            serializedObject.Update();
        }

        
        private void OnSettingUpdate()
        {
            Repaint();
        }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Drawer Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */ 

        /// <summary>
        /// Draws the script fields in the custom inspector...
        /// </summary>
        private void DrawScriptSection()
        {
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(1.5f);
            
            UtilEditor.DrawSoScriptSection((AssetGlobalRuntimeSettings) target);
            
            GUILayout.Space(2.5f);
            UtilEditor.DrawHorizontalGUILine();
            GUILayout.Space(2.5f);
            
            if (GUILayout.Button("Edit Settings"))
            {
                SettingsService.OpenProjectSettings("Project/Carter Games/Multi Scene");
            }
            
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
            UtilEditor.DrawHorizontalGUILine();

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(listenerFreqProp);
            EditorGUILayout.PropertyField(unloadResourcesProp);
            EditorGUI.EndDisabledGroup();

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
            UtilEditor.DrawHorizontalGUILine();

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(loadModeProp);
            EditorGUILayout.PropertyField(startGroupProp);
            EditorGUILayout.PropertyField(lastGroupProp);
            EditorGUI.EndDisabledGroup();
            
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
            UtilEditor.DrawHorizontalGUILine();

            EditorGUI.BeginDisabledGroup(true);
            
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
                    var name = defaultGroupProp.GetIndex(i).Fpr("groupName");
                    var index = defaultGroupProp.GetIndex(i).Fpr("groupIndex");
                    var show = defaultGroupProp.GetIndex(i).Fpr("showGroup");

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(name, new GUIContent(name.stringValue.Length > 0 ? name.stringValue : "Element " + i));
                    EditorGUILayout.PropertyField(index, GUIContent.none, GUILayout.Width(65));
                    EditorGUILayout.PropertyField(show, GUIContent.none, GUILayout.Width(65));
                    EditorGUILayout.EndHorizontal();
                }

                GUILayout.Space(2f);
                EditorGUILayout.EndVertical();
            }
            
            EditorGUI.EndDisabledGroup();
        }
        
        
        /// <summary>
        /// Draws the user-defined scene group category options in the custom inspector...
        /// </summary>
        private void DrawUserSceneGroupCategory()
        {
            EditorGUI.BeginDisabledGroup(true);
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
                    var name = userGroupProp.GetIndex(i).Fpr("groupName");
                    var index = userGroupProp.GetIndex(i).Fpr("groupIndex");
                    var show = userGroupProp.GetIndex(i).Fpr("showGroup");

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(name, new GUIContent(name.stringValue.Length > 0 ? name.stringValue : "Element " + i));
                    EditorGUILayout.PropertyField(index, GUIContent.none, GUILayout.Width(65));
                    EditorGUILayout.PropertyField(show, GUIContent.none, GUILayout.Width(65));
                    EditorGUILayout.EndHorizontal();
                }

                GUILayout.Space(2f);
                EditorGUILayout.EndVertical();
            }
            
            EditorGUI.EndDisabledGroup();
            GUILayout.Space(2.5f);
            EditorGUILayout.EndVertical();
        }
    }
}