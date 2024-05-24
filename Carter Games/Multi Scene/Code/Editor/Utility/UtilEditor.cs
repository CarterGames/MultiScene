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

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Experimental.MultiScene.Editor
{
    /// <summary>
    /// The primary utility class for the multi scene's editor logic. Should only be used in the editor space!
    /// </summary>
    public static class UtilEditor
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        // Paths
        /* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        public const string SettingsWindowPath = "Project/Carter Games/Multi Scene";


        // Filters
        /* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        // Graphics
        private const string MultiSceneLogoFilter = "T_MultiScene_Logo";
        private const string MultiSceneLogoTransparentFilter = "T_MultiScene_Logo_Transparent";
        private const string CarterGamesBannerFilter = "T_MultiScene_CarterGamesBanner";


        // Texture Caches
        /* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        private static Texture2D multiSceneLogoCache;
        private static Texture2D multiSceneLogoTransparentCache;
        private static Texture2D carterGamesBannerCache;


        // Asset Caches
        /* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        private static AssetGlobalRuntimeSettings assetGlobalRuntimeSettingsCache;
        private static SerializedObject settingsObjectCache;
        private static SerializedObject editorSettingsObjectCache;
        private static AssetIndex assetIndexCache;


        // Colours
        /* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Green Apple (Green) Color
        /// </summary>
        /// <see cref="https://www.computerhope.com/cgi-bin/htmlcolor.pl?c=4CC417"/>
        public static readonly Color Green = new Color32(76, 196, 23, 255);

        
        /// <summary>
        /// Rubber Ducky Yellow (Yellow) Color
        /// </summary>
        /// <see cref="https://www.computerhope.com/cgi-bin/htmlcolor.pl?c=FFD801"/>
        public static readonly Color Yellow = new Color32(255, 216, 1, 255);
        

        /// <summary>
        /// Scarlet Red (Red) Color
        /// </summary>
        /// <see cref="https://www.computerhope.com/cgi-bin/htmlcolor.pl?c=FF2400"/>
        public static readonly Color Red = new Color32(255, 36, 23, 255);


        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Gets the path where the asset code is located.
        /// </summary>
        private static string AssetBasePath => FileEditorUtil.AssetBasePath;


        // Textures/Graphics
        /* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Gets the multi scene logo.
        /// </summary>
        public static Texture2D MultiSceneLogo => FileEditorUtil.GetOrAssignCache(ref multiSceneLogoCache, MultiSceneLogoFilter);
        

        /// <summary>
        /// Gets the carter games banner.
        /// </summary>
        public static Texture2D CarterGamesBanner =>
            FileEditorUtil.GetOrAssignCache(ref carterGamesBannerCache, CarterGamesBannerFilter);


        /// <summary>
        /// Gets the multi scene transparent logo.
        /// </summary>
        public static Texture2D MultiSceneTransparentLogo =>
            FileEditorUtil.GetOrAssignCache(ref multiSceneLogoTransparentCache, MultiSceneLogoTransparentFilter);
        
        
        // Assets
        /* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Gets if there is a settings asset in the project.
        /// </summary>
        public static bool HasInitialized
        {
            get
            {
                AssetIndexHandler.UpdateIndex();
                return ScriptableRef.HasAllAssets;
            }
        }


        /// <summary>
        /// Gets/Sets the save manager settings asset.
        /// </summary>
        public static AssetGlobalRuntimeSettings RuntimeSettings => ScriptableRef.RuntimeAssetGlobalRuntimeSettings;


        /// <summary>
        /// Gets/Sets the save manager editor settings asset.
        /// </summary>
        public static SerializedObject SettingsObject => ScriptableRef.RuntimeSettingsObject;
        
        
        /// <summary>
        /// Gets/Sets the save manager save data asset.
        /// </summary>
        public static AssetIndex AssetIndex => ScriptableRef.AssetIndex;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Draw Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Draws the default Banner Logo header for the asset...
        /// </summary>
        public static void DrawHeader()
        {
            GUILayout.Space(5f);

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (MultiSceneLogo != null)
            {
                if (GUILayout.Button(MultiSceneLogo, GUIStyle.none, GUILayout.MaxHeight(110)))
                {
                    GUI.FocusControl(null);
                }
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5f);
        }


        /// <summary>
        /// Draws the script fields in the custom inspector...
        /// </summary>
        public static void DrawMonoScriptSection<T>(T target) where T : MonoBehaviour
        {
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(1.5f);

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("Script:", MonoScript.FromMonoBehaviour(target), typeof(T), false);
            EditorGUI.EndDisabledGroup();

            GUILayout.Space(1.5f);
            EditorGUILayout.EndVertical();
        }


        /// <summary>
        /// Draws the script fields in the custom inspector...
        /// </summary>
        public static void DrawSoScriptSection(object target)
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("Script:", MonoScript.FromScriptableObject((ScriptableObject)target),
                typeof(object), false);
            EditorGUI.EndDisabledGroup();
        }


        /// <summary>
        /// Draws a horizontal line
        /// </summary>
        public static void DrawHorizontalGUILine()
        {
            GUIStyle boxStyle = new GUIStyle(GUI.skin.box);
            boxStyle.normal.background = new Texture2D(1, 1);
            boxStyle.normal.background.SetPixel(0, 0, Color.grey);
            boxStyle.normal.background.Apply();

            var one = EditorGUILayout.BeginHorizontal();
            GUILayout.Box("", boxStyle, GUILayout.ExpandWidth(true), GUILayout.Height(2));
            EditorGUILayout.EndHorizontal();
        }


        /// <summary>
        /// Creates a deselect zone to let users click outside of any editor window to unfocus from their last selected field.
        /// </summary>
        /// <param name="rect">The rect to draw on.</param>
        public static void CreateDeselectZone(ref Rect rect)
        {
            if (rect.width <= 0)
            {
                rect = new Rect(0, 0, Screen.width, Screen.height);
            }

            if (GUI.Button(rect, string.Empty, GUIStyle.none))
            {
                GUI.FocusControl(null);
            }
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Helper Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        public static void Initialize()
        {
            AssetDatabase.Refresh();
            
            if (assetIndexCache == null)
            {
                assetIndexCache = AssetIndex;
            }
            
            if (assetGlobalRuntimeSettingsCache == null)
            {
                assetGlobalRuntimeSettingsCache = RuntimeSettings;
            }

            AssetIndexHandler.UpdateIndex();
            EditorUtility.SetDirty(AssetIndex);
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        
        public static T[] GetAllInstances<T>() where T : MultiSceneAsset
        {
            var guids = AssetDatabase.FindAssets("t:"+ typeof(T).Name);
            var a = new T[guids.Length];
            
            for (var i =0;i<guids.Length;i++)         //probably could get optimized 
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }
 
            return a;
        }


        /// <summary>
        /// Checks to see if a list has any null entries.
        /// </summary>
        /// <param name="list">The list to check.</param>
        /// <returns>If there was a null entry or not.</returns>
        public static bool HasNullEntries(this IList list)
        {
            if (list == null || list.Count <= 0) return false;

            for (var i = list.Count - 1; i > -1; i--)
            {
                if (list[i] != null) continue;
                return true;
            }

            return false;
        }


        /// <summary>
        /// Removes missing entries in a list when called.
        /// </summary>
        /// <param name="list">The list to edit.</param>
        /// <returns>The list with no null entries.</returns>
        public static IList RemoveMissing(this IList list)
        {
            for (var i = list.Count - 1; i > -1; i--)
            {
                if (list[i] == null)
                {
                    list.RemoveAt(i);
                }
            }

            return list;
        }


        /// <summary>
        /// Removed any duplicate entries from the list entered.
        /// </summary>
        /// <param name="list">The list to edit.</param>
        /// <typeparam name="T">The type for the list.</typeparam>
        /// <returns>The list with no null entries.</returns>
        public static IList RemoveDuplicates<T>(this IList list)
        {
            var validateList = new List<T>();

            for (var i = list.Count - 1; i > -1; i--)
            {
                if (validateList.Contains((T)list[i])) continue;
                validateList.Add((T)list[i]);
            }

            return validateList;
        }
        

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Editor Extension Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Gets the width of a string's GUI.
        /// </summary>
        /// <param name="text">The text to size.</param>
        /// <returns>The resulting size.</returns>
        public static float Width(this string text)
        {
            return GUI.skin.label.CalcSize(new GUIContent(text)).x + 1.5f;
        }
    }
}