using System.IO;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Experimental.MultiScene.Editor
{
    /// <summary>
    /// Editor Utility Class to help with the visuals & settings of the asset...
    /// </summary>
    public static class MultiSceneEditorUtil
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Paths & Filters
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        public const string SettingsWindowPath = "Project/Carter Games/Multi Scene";
        private const string SettingsAssetDefaultFullPath = "Assets/Resources/Carter Games/Multi Scene/Multi Scene Settings.asset";
        private const string SettingsAssetFilter = "t:multiscenesettingsasset";
        private const string AssetHeaderGraphicFilter = "MultiSceneEditorHeader";
        private const string AssetLogoTransparentGraphicFilter = "MultiSceneLogo";
        private const string SettingsIconTransparentGraphicFilter = "SettingsIcon";
        private const string CarterGamesBannerGraphicFilter = "CarterGamesBanner";
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Colours
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        public static Color TitleColour = new Color32(151, 121, 209, 255);
        public static Color Green = new Color32(72, 222, 55, 255);
        public static Color Yellow = new Color32(245, 234, 56, 255);
        public static Color Red = new Color32(255, 150, 157, 255);
        public static Color Blue = new Color32(151, 196, 255, 255);
        public static Color Hidden = new Color(0, 0, 0, .3f);
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Asset Art Reference Cache
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */      
        
        private static Texture2D cachedLogoImg;
        private static Texture2D cachedManagerHeaderImg;
        private static Texture2D cachedCarterGamesBannerImg;
        private static Texture2D cachedSettingsLogoImg;

        
        public static Texture2D ManagerHeader
        {
            get
            {
                if (cachedManagerHeaderImg) return cachedManagerHeaderImg;
                cachedManagerHeaderImg = GetTextureFile(AssetHeaderGraphicFilter);
                return cachedManagerHeaderImg;
            }
        }
        
        
        public static Texture2D CarterGamesBanner 
        {
            get
            {
                if (cachedCarterGamesBannerImg) return cachedCarterGamesBannerImg;
                cachedCarterGamesBannerImg = GetTextureFile(CarterGamesBannerGraphicFilter);
                return cachedCarterGamesBannerImg;
            }
        }

        
        public static Texture2D LogoTransparent
        {
            get
            {
                if (cachedLogoImg) return cachedLogoImg;
                cachedLogoImg = GetTextureFile(AssetLogoTransparentGraphicFilter);
                return cachedLogoImg;
            }
        }
        
        
        public static Texture2D SettingsLogoTransparent
        {
            get
            {
                if (cachedSettingsLogoImg) return cachedSettingsLogoImg;
                cachedSettingsLogoImg = GetTextureFile(SettingsIconTransparentGraphicFilter);
                return cachedSettingsLogoImg;
            }
        }

        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Scriptable Assets
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */     

        
        private static MultiSceneSettingsAsset _settingsAsset;
        
        
        /// <summary>
        /// Confirms if there is a settings asset in the project or not...
        /// </summary>
        public static bool HasSettingsFile => Settings != null;
        
        
        /// <summary>
        /// Gets the settings asset for use (Editor Space Only!)...
        /// </summary>
        public static MultiSceneSettingsAsset Settings 
        {
            get
            {
                if (_settingsAsset != null) return _settingsAsset;

                if (AssetDatabase.FindAssets(SettingsAssetFilter, null).Length <= 0)
                {
                    CreateFile<MultiSceneSettingsAsset>(SettingsAssetDefaultFullPath);
                    MultiSceneEditorEvents.Settings.OnSettingsAssetRegenerated.Raise();
                }
                
                _settingsAsset = (MultiSceneSettingsAsset)GetFile<MultiSceneSettingsAsset>(SettingsAssetFilter);
                return _settingsAsset;
            }
        }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   File Management Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */     
        
        /// <summary>
        /// Creates a file of the type requested...
        /// </summary>
        /// <param name="path">The path to create the file in (Editor Only)</param>
        /// <typeparam name="T">The type the file is</typeparam>
        private static void CreateFile<T>(string path)
        {
            var instance = ScriptableObject.CreateInstance(typeof(T));

            var currentPath = string.Empty;
            
            foreach (var element in path.Split('/'))
            {
                if (!element.Equals("Assets"))
                    currentPath += "/" + element;
                else
                    currentPath = element;
                
                if (Directory.Exists(element.Replace("Multi Scene Settings.asset", ""))) continue;
                Directory.CreateDirectory(currentPath.Replace("Multi Scene Settings.asset", ""));
            }
            
            AssetDatabase.CreateAsset(instance, path);
            AssetDatabase.Refresh();
        }
        
        
        /// <summary>
        /// Gets the first file of the type requested that isn't the class (Editor Only)
        /// </summary>
        /// <param name="filter">the search filter</param>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns>object</returns>
        private static object GetFile<T>(string filter)
        {
            var asset = AssetDatabase.FindAssets(filter, null);
            var path = AssetDatabase.GUIDToAssetPath(asset[0]);
            return AssetDatabase.LoadAssetAtPath(path, typeof(T));
        }


        /// <summary>
        /// Runs GetFile() but for a texture & returns the result...
        /// </summary>
        /// <param name="filter">The filter to apply</param>
        /// <returns>The texture found</returns>
        private static Texture2D GetTextureFile(string filter)
        {
            return (Texture2D)GetFile<Texture2D>(filter);
        }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Editor Draw Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Draws the default Banner Logo header for the asset...
        /// </summary>
        public static void DrawHeader()
        {
            var managerHeader = ManagerHeader;
            
            GUILayout.Space(5f);
                    
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
                    
            if (managerHeader != null)
            {
                if (GUILayout.Button(managerHeader, GUIStyle.none, GUILayout.MaxHeight(110)))
                {
                    GUI.FocusControl(null);
                }
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
                    
            GUILayout.Space(5f);
        }


        /// <summary>
        /// Draws the logo only header for the asset...
        /// </summary>
        public static void DrawLogoOnly()
        {
            GUILayout.Space(5f);
                    
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
                    
            if (LogoTransparent != null)
            {
                if (GUILayout.Button(LogoTransparent, GUIStyle.none, GUILayout.MaxHeight(65)))
                {
                    GUI.FocusControl(null);
                }
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
                    
            GUILayout.Space(5f);
        }
        
        
        /// <summary>
        /// Draws the logo only header for the asset...
        /// </summary>
        public static void DrawSettingsIconOnly()
        {
            GUILayout.Space(5f);
                    
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
                    
            if (SettingsLogoTransparent != null)
            {
                if (GUILayout.Button(SettingsLogoTransparent, GUIStyle.none, GUILayout.MaxHeight(65)))
                {
                    GUI.FocusControl(null);
                }
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
                    
            GUILayout.Space(5f);
        }
        
        
        /// <summary>
        /// Gets the width of the text entered...
        /// </summary>
        /// <param name="text">The text the gauge</param>
        /// <returns>The width of the text entered</returns>
        public static float TextWidth(string text)
        {
            return GUI.skin.label.CalcSize(new GUIContent(text)).x;
        }
    }
}