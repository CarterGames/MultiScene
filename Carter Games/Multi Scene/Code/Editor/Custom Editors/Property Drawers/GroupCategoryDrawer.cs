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
    /// Draws a group category in a single row for each of use. 
    /// </summary>
    [CustomPropertyDrawer(typeof(GroupCategory))]
    public sealed class GroupCategoryDrawer : PropertyDrawer
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */ 

        private static SerializedProperty nameProp;
        private static SerializedProperty indexProp;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Drawer Method
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */ 

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);
            
            nameProp = property.Fpr("groupName");
            indexProp = property.Fpr("groupIndex");

            EditorGUI.BeginChangeCheck();

            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var leftRect = new Rect(position.x, position.y, (position.width / 4) * 3 - 1.5f, EditorGUIUtility.singleLineHeight);
            var rightRect = new Rect(position.x + position.width / 4 * 3 + 1.5f, position.y, (position.width / 4) - 1.5f, EditorGUIUtility.singleLineHeight);
            
            EditorGUI.PropertyField(leftRect, nameProp, GUIContent.none);
            EditorGUI.PropertyField(rightRect, indexProp, GUIContent.none);
            
            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
            }          
            
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}