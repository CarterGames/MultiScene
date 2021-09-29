// ----------------------------------------------------------------------------
// SceneGroupEditor.cs
// 
// Author: Jonathan Carter (A.K.A. J)
// Date: 14/09/2021
// ----------------------------------------------------------------------------

using System;
using JTools.Editor;
using UnityEditor;
using UnityEngine;

namespace JTools.MultiScene.Editor
{
    [CustomEditor(typeof(SceneGroup))]
    public class SceneGroupEditor : UnityEditor.Editor
    {
        /// <summary>
        /// The Green colour used in the button.
        /// </summary>
        private static Color32 grnCol = new Color32(72, 222, 55, 255);
        
        
        /// <summary>
        /// The Yellow colour used in the button.
        /// </summary>
        private static Color32 ylwCol = new Color32(245, 234, 56, 255);
        
        
        /// <summary>
        /// The Red colour used in the button.
        /// </summary>
        private static Color32 redCol = new Color32(255, 150, 157, 255);
        
        
        
        private SerializedProperty scenes;
        private Color defaultBGCol;
        private Color defaultGUICol;

        private void OnEnable()
        {
            scenes = serializedObject.FindProperty("scenes");
            defaultBGCol = GUI.backgroundColor;
            defaultGUICol = GUI.color;
        }


        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            Layout.HorizontalCentered(() =>
            {
                Label.BoldCompact(" Scene Group ");
            });
            
            if (scenes == null || scenes.arraySize <= 0)
            {
                Button.GreenButton("Add Base Scene", CallAddBaseField);
            }

            if (scenes.arraySize > 0)
            {
                RenderBaseSceneField();

                if (scenes.arraySize > 1)
                    RenderAdditiveSceneFields();
                else
                    Button.YellowButton("Add Scene", CallAddNewAdditiveScene);
            }

            serializedObject.ApplyModifiedProperties();

            // Un-comment for debugging...
            //base.OnInspectorGUI();
        }


        private void CallAddBaseField()
        {
            scenes.InsertArrayElementAtIndex(0);
        }
        

        private void RenderBaseSceneField()
        {
            GUI.backgroundColor = grnCol;
            GUI.color = grnCol;
            Label.Bold("Base Scene");
            GUI.color = defaultGUICol;
            EditorGUILayout.PropertyField(scenes.GetArrayElementAtIndex(0), GUIContent.none);
            GUI.backgroundColor = defaultBGCol;
        }


        private void RenderAdditiveSceneFields()
        {
            GUI.backgroundColor = ylwCol;
            GUI.color = ylwCol;
            Label.Bold("Additive Scene(s)");
            GUI.color = defaultGUICol;
            
            for (var i = 1; i < scenes.arraySize; i++)
            {
                Layout.Horizontal(() =>
                {
                    EditorGUILayout.PropertyField(scenes.GetArrayElementAtIndex(i), GUIContent.none);
                
                    Button.GreenButton("+", () => CallAddNewAdditiveScene(i));
                    Button.RedButton("-", () => CallRemoveElementAtIndex(i));
                });
            }

            GUI.backgroundColor = defaultBGCol;
        }


        private void CallRemoveElementAtIndex(int i)
        {
            scenes.DeleteArrayElementAtIndex(i);
        }

        private void CallAddNewAdditiveScene(int i)
        {
            scenes.InsertArrayElementAtIndex(i);
            scenes.GetArrayElementAtIndex(i + 1).stringValue = string.Empty;
        }
        
        private void CallAddNewAdditiveScene()
        {
            scenes.InsertArrayElementAtIndex(scenes.arraySize - 1);
            scenes.GetArrayElementAtIndex(scenes.arraySize - 1).stringValue = string.Empty;
        }
    }
}