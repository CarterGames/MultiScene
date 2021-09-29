// ----------------------------------------------------------------------------
// SceneGroupEditor.cs
// 
// Author: Jonathan Carter (A.K.A. J)
// Date: 14/09/2021
// ----------------------------------------------------------------------------

using System;
using UnityEditor;
using UnityEngine;

namespace MultiScene.Editor
{
    [CustomEditor(typeof(SceneGroup))]
    public class SceneGroupEditor : UnityEditor.Editor
    {
        public static Color Green => new Color32(72, 222, 55, 255);
        public static Color Yellow => new Color32(245, 234, 56, 255);
        public static Color Orange => new Color32(255, 170, 134, 255);
        public static Color Red => new Color32(255, 150, 157, 255);
        public static Color Blue => new Color32(151, 196, 255, 255);
        public static Color Purple => new Color32(196, 151, 255, 255);
        public static Color Pink => new Color32(255, 151, 242, 255);
        public static Color LightGrey => new Color32(199, 199, 199, 255);
        public static Color DarkGrey => new Color32(88, 88, 88, 255);
        public static Color White => Color.white;
        public static Color Black => Color.black;
        
        
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
            
            HorizontalCentered(() =>
            {
                BoldCompact(" Scene Group ");
            });
            
            if (scenes == null || scenes.arraySize <= 0)
            {
                GreenButton("Add Base Scene", CallAddBaseField);
            }

            if (scenes.arraySize > 0)
            {
                RenderBaseSceneField();

                if (scenes.arraySize > 1)
                    RenderAdditiveSceneFields();
                else
                    YellowButton("Add Scene", CallAddNewAdditiveScene);
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
            GUI.backgroundColor = Green;
            GUI.color = Green;
            Bold("Base Scene");
            GUI.color = defaultGUICol;
            EditorGUILayout.PropertyField(scenes.GetArrayElementAtIndex(0), GUIContent.none);
            GUI.backgroundColor = defaultBGCol;
        }


        private void RenderAdditiveSceneFields()
        {
            GUI.backgroundColor = Yellow;
            GUI.color = Yellow;
            Bold("Additive Scene(s)");
            GUI.color = defaultGUICol;
            
            for (var i = 1; i < scenes.arraySize; i++)
            {
                Horizontal(() =>
                {
                    EditorGUILayout.PropertyField(scenes.GetArrayElementAtIndex(i), GUIContent.none);
                
                    GreenButton("+", () => CallAddNewAdditiveScene(i));
                    RedButton("-", () => CallRemoveElementAtIndex(i));
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
        
        /// <summary>
        /// Shows a compact button where the label controls the size of the button.
        /// </summary>
        /// <param name="label">String | Label for the button.</param>
        /// <returns>Bool</returns>
        private static bool CompactButton(string label)
        {
            label += " ";
            return GUILayout.Button(label, GUILayout.Width(TextWidth(label)));
        }
        
        /// <summary>
        /// Shows a green button.
        /// </summary>
        /// <param name="label">String | Label for the button.</param>
        /// <param name="callback">Action | The actions to perform on button press.</param>
        /// <param name="options">GUILayoutOption[] | Layout options.</param>
        private static void GreenButton(string label, Action callback, params GUILayoutOption[] options)
        {
            var _default = GUI.backgroundColor;
            GUI.backgroundColor = Green;
            if (GUILayout.Button(label, options))
                callback();
            GUI.backgroundColor = _default;
        }
        

        /// <summary>
        /// Shows a yellow button.
        /// </summary>
        /// <param name="label">String | Label for the button.</param>
        /// <param name="callback">Action | The actions to perform on button press.</param>
        /// <param name="options">GUILayoutOption[] | Layout options.</param>
        private static void YellowButton(string label, Action callback, params GUILayoutOption[] options)
        {
            var _default = GUI.backgroundColor;
            GUI.backgroundColor = Yellow;
            if (GUILayout.Button(label, options))
                callback();
            GUI.backgroundColor = _default;
        }
        
        
        /// <summary>
        /// Shows a red button.
        /// </summary>
        /// <param name="label">String | Label for the button.</param>
        /// <param name="callback">Action | The actions to perform on button press.</param>
        /// <param name="options">GUILayoutOption[] | Layout options.</param>
        public static void RedButton(string label, Action callback, params GUILayoutOption[] options)
        {
            var _default = GUI.backgroundColor;
            GUI.backgroundColor = Red;
            if (GUILayout.Button(label, options))
                callback();
            GUI.backgroundColor = _default;
        }
        
        /// <summary>
        /// Adds a bold label with no other special styling or effects.
        /// </summary>
        /// <param name="labelString">The text to display.</param>
        /// <param name="options">any special params you wish to pass in.</param>
        private static void Bold(string labelString, params GUILayoutOption[] options)
        {
            EditorGUILayout.LabelField(labelString, EditorStyles.boldLabel, options);
        }
        
        
        /// <summary>
        /// Adds a compact label with a bold style.
        /// </summary>
        /// <param name="labelString">The text to display.</param>
        private static void BoldCompact(string labelString)
        {
            labelString += "    ";
            EditorGUILayout.LabelField(labelString, EditorStyles.boldLabel, GUILayout.Width(TextWidth(labelString)));
        }
        
        
        /// <summary>
        /// Gets the text width of the text entered.
        /// </summary>
        /// <param name="text">The text to get the width of.</param>
        /// <returns>Float</returns>
        private static float TextWidth(string text)
        {
            return GUI.skin.label.CalcSize(new GUIContent(text)).x;
        }
        
        /// <summary>
        /// Make a EditorGUILayout.BeginHorizontal.
        /// </summary>
        /// <param name="blockElements">Action | Stuff to display</param>
        /// <remarks>Actions can be () => {} or a method.</remarks>
        public static void Horizontal(Action blockElements)
        {
            EditorGUILayout.BeginHorizontal();
            blockElements();
            EditorGUILayout.EndHorizontal();
        }


        /// <summary>
        /// Make a EditorGUILayout.BeginHorizontal...with content that is centered.
        /// </summary>
        /// <param name="blockElements">Action | Stuff to display</param>
        /// <remarks>Actions can be () => {} or a method.</remarks>
        public static void HorizontalCentered(Action blockElements)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            blockElements();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
    }
}