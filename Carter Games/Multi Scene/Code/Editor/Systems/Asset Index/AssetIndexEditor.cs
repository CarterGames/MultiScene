﻿/*
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
    /// Handles the custom inspector for the Audio Manager asset index.
    /// </summary>
    [CustomEditor(typeof(AssetIndex))]
    public sealed class AudioManagerAssetIndexEditor : UnityEditor.Editor
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private Dictionary<string, int> entryLookup = new Dictionary<string, int>();
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Unity Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private void OnEnable()
        {
            entryLookup ??= new Dictionary<string, int>();
            entryLookup?.Clear();

            if (serializedObject.Fp("assets").Fpr("list").arraySize <= 0) return;
            
            for (var i = 0; i < serializedObject.Fp("assets").Fpr("list").arraySize; i++)
            {
                entryLookup.Add(serializedObject.Fp("assets").Fpr("list").GetIndex(i).Fpr("key").stringValue, i);
            }
        }


        public override void OnInspectorGUI()
        {
            GUILayout.Space(12.5f);
            
            UtilEditor.DrawSoScriptSection((AssetIndex) target);
            GUILayout.Space(12.5f);

            DrawRequireReferencesSection();
            
            GUILayout.Space(7.5f);
            
            DrawAllReferencesSection();
            
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Draws the required references GUI.
        /// </summary>
        private void DrawRequireReferencesSection()
        {
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(1.5f);
            
            EditorGUILayout.LabelField("Required References", EditorStyles.boldLabel);
            UtilEditor.DrawHorizontalGUILine();

            if (entryLookup.ContainsKey(typeof(AssetGlobalRuntimeSettings).FullName) && serializedObject.Fp("assets").Fpr("list").arraySize > 0)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Settings Reference: ", GUILayout.Width("Settings Reference:".Width()));

                var hasLibRef = serializedObject.Fp("assets").Fpr("list")
                    .GetIndex(entryLookup[typeof(AssetGlobalRuntimeSettings).FullName]).Fpr("value").arraySize > 0;
                
                GUI.contentColor = hasLibRef ? UtilEditor.Green : UtilEditor.Red;
                EditorGUILayout.LabelField(hasLibRef ? "True" : "False");
                GUI.contentColor = Color.white;
                
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Settings Reference: ", GUILayout.Width("Settings Reference:".Width()));
                
                GUI.contentColor = UtilEditor.Red;
                EditorGUILayout.LabelField("False");
                GUI.contentColor = Color.white;
                
                EditorGUILayout.EndHorizontal();
            }
            
            GUILayout.Space(1.5f);
            EditorGUILayout.EndVertical();
        }


        /// <summary>
        /// Draws the all references GUI.
        /// </summary>
        private void DrawAllReferencesSection()
        {
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(1.5f);
            
            EditorGUILayout.LabelField("All References", EditorStyles.boldLabel);
            UtilEditor.DrawHorizontalGUILine();

            EditorGUI.indentLevel++;
            
            EditorGUI.BeginDisabledGroup(Application.isPlaying);
            EditorGUILayout.PropertyField(serializedObject.Fp("assets"));
            EditorGUI.EndDisabledGroup();
            
            EditorGUI.indentLevel--;
            
            GUILayout.Space(1.5f);
            EditorGUILayout.EndVertical();
        }
    }
}