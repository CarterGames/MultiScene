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

using System;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Experimental.MultiScene.Editor
{
    /// <summary>
    /// Handles the editor settings per user, instead of an editor asset like before. 
    /// </summary>
    public static class PerUserSettings
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private const string UniqueIdId = "CarterGames_MultiScene_Editor_UUID";
        
        private static readonly string AutoValidationAutoCheckId = $"{UniqueId}_CarterGames_MultiScene_EditorSettings_AutoVersionCheck";
        
        private static readonly string SettingsEditorDropdownId = $"{UniqueId}_CarterGames_MultiScene_EditorSettings_EditorSettingsDropdownToggle";
        private static readonly string SettingsSceneManagementDropdownId = $"{UniqueId}_CarterGames_MultiScene_EditorSettings_EditorSceneManagementDropdownToggle";
        private static readonly string SettingsSceneGroupDropdownId = $"{UniqueId}_CarterGames_MultiScene_EditorSettings_EditorSceneGroupDropdownToggle";

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The unique if for the assets settings to be per project...
        /// </summary>
        /// <remarks>
        /// Saved to player pref to allow settings to be different per project in the same editor version.
        /// </remarks>
        private static string UniqueId => (string)GetOrCreateValue<string>(UniqueIdId, SettingType.PlayerPref, Guid.NewGuid().ToString());
        
        
        /// <summary>
        /// Should the validator check for a new version on load for the user?
        /// </summary>
        public static bool VersionValidationAutoCheckOnLoad
        {
            get => (bool) GetOrCreateValue<bool>(AutoValidationAutoCheckId, SettingType.EditorPref, true);
            set => SetValue<bool>(AutoValidationAutoCheckId, SettingType.EditorPref, value);
        }
        
        
        public static bool SettingsEditorDropdown
        {
            get => (bool) GetOrCreateValue<bool>(SettingsEditorDropdownId, SettingType.EditorPref, true);
            set => SetValue<bool>(SettingsEditorDropdownId, SettingType.EditorPref, value);
        }
        
        
        public static bool SettingsSceneManagementDropdown
        {
            get => (bool) GetOrCreateValue<bool>(SettingsSceneManagementDropdownId, SettingType.EditorPref, true);
            set => SetValue<bool>(SettingsSceneManagementDropdownId, SettingType.EditorPref, value);
        }
        
        
        public static bool SettingsSceneGroupDropdown
        {
            get => (bool) GetOrCreateValue<bool>(SettingsSceneGroupDropdownId, SettingType.EditorPref, true);
            set => SetValue<bool>(SettingsSceneGroupDropdownId, SettingType.EditorPref, value);
        }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private static object GetOrCreateValue<T>(string key, SettingType type, object defaultValue = null)
        {
            switch (type)
            {
                case SettingType.EditorPref:

                    if (EditorPrefs.HasKey(key))
                    {
                        switch (typeof(T))
                        {
                            case var x when x == typeof(bool):
                                return EditorPrefs.GetBool(key);
                            case var x when x == typeof(int):
                                return EditorPrefs.GetInt(key);
                            case var x when x == typeof(float):
                                return EditorPrefs.GetFloat(key);
                            case var x when x == typeof(string):
                                return EditorPrefs.GetString(key);
                            case var x when x == typeof(Vector2):
                                return JsonUtility.FromJson<Vector2>(EditorPrefs.GetString(key));
                            default:
                                return null;
                        }
                    }

                    switch (typeof(T))
                    {
                        case var x when x == typeof(bool):
                            EditorPrefs.SetBool(key, defaultValue == null ? false : (bool)defaultValue);
                            return EditorPrefs.GetBool(key);
                        case var x when x == typeof(int):
                            EditorPrefs.SetInt(key, defaultValue == null ? 0 : (int)defaultValue);
                            return EditorPrefs.GetInt(key);
                        case var x when x == typeof(float):
                            EditorPrefs.SetFloat(key, defaultValue == null ? 0 : (float)defaultValue);
                            return EditorPrefs.GetFloat(key);
                        case var x when x == typeof(string):
                            EditorPrefs.SetString(key, (string)defaultValue);
                            return EditorPrefs.GetString(key);
                        case var x when x == typeof(Vector2):
                            EditorPrefs.SetString(key,
                                defaultValue == null
                                    ? JsonUtility.ToJson(Vector2.zero)
                                    : JsonUtility.ToJson(defaultValue));
                            return JsonUtility.FromJson<Vector2>(EditorPrefs.GetString(key));
                        default:
                            return null;
                    }
                    
                case SettingType.PlayerPref:
                    
                    if (PlayerPrefs.HasKey(key))
                    {
                        switch (typeof(T))
                        {
                            case var x when x == typeof(bool):
                                return PlayerPrefs.GetInt(key) == 1;
                            case var x when x == typeof(int):
                                return PlayerPrefs.GetInt(key);
                            case var x when x == typeof(float):
                                return PlayerPrefs.GetFloat(key);
                            case var x when x == typeof(string):
                                return PlayerPrefs.GetString(key);
                            case var x when x == typeof(Vector2):
                                return JsonUtility.FromJson<Vector2>(PlayerPrefs.GetString(key));
                            default:
                                return null;
                        }
                    }

                    switch (typeof(T))
                    {
                        case var x when x == typeof(bool):
                            PlayerPrefs.SetInt(key,
                                defaultValue == null ? 0 : defaultValue.ToString().ToLower() == "true" ? 1 : 0);
                            return PlayerPrefs.GetInt(key) == 1;
                        case var x when x == typeof(int):
                            PlayerPrefs.SetInt(key, defaultValue == null ? 0 : (int)defaultValue);
                            return PlayerPrefs.GetInt(key);
                        case var x when x == typeof(float):
                            PlayerPrefs.SetFloat(key, defaultValue == null ? 0 : (float)defaultValue);
                            return PlayerPrefs.GetFloat(key);
                        case var x when x == typeof(string):
                            PlayerPrefs.SetString(key, (string)defaultValue);
                            return PlayerPrefs.GetString(key);
                        case var x when x == typeof(Vector2):
                            PlayerPrefs.SetString(key,
                                defaultValue == null
                                    ? JsonUtility.ToJson(Vector2.zero)
                                    : JsonUtility.ToJson(defaultValue));
                            return JsonUtility.FromJson<Vector2>(PlayerPrefs.GetString(key));
                        default:
                            return null;
                    }
                    
                case SettingType.SessionState:

                    switch (typeof(T))
                    {
                        case var x when x == typeof(bool):
                            return SessionState.GetBool(key, defaultValue == null ? false : (bool)defaultValue);
                        case var x when x == typeof(int):
                            return SessionState.GetInt(key, defaultValue == null ? 0 : (int)defaultValue);
                        case var x when x == typeof(float):
                            return SessionState.GetFloat(key, defaultValue == null ? 0 : (float)defaultValue);
                        case var x when x == typeof(string):
                            return SessionState.GetString(key, (string)defaultValue);
                        case var x when x == typeof(Vector2):
                            return JsonUtility.FromJson<Vector2>(SessionState.GetString(key,
                                JsonUtility.ToJson(defaultValue)));
                        default:
                            return null;
                    }
                    
                default:
                    return null;
            }
        }


        private static void SetValue<T>(string key, SettingType type, object value)
        {
            switch (type)
            {
                case SettingType.EditorPref:
                    
                    switch (typeof(T))
                    {
                        case var x when x == typeof(bool):
                            EditorPrefs.SetBool(key, (bool)value);
                            break;
                        case var x when x == typeof(int):
                            EditorPrefs.SetInt(key, (int)value);
                            break;
                        case var x when x == typeof(float):
                            EditorPrefs.SetFloat(key, (float)value);
                            break;
                        case var x when x == typeof(string):
                            EditorPrefs.SetString(key, (string)value);
                            break;
                        case var x when x == typeof(Vector2):
                            EditorPrefs.SetString(key, JsonUtility.ToJson(value));
                            break;
                    }
                    
                    break;
                case SettingType.PlayerPref:
                    
                    switch (typeof(T))
                    {
                        case var x when x == typeof(bool):
                            PlayerPrefs.SetInt(key, ((bool)value) ? 1 : 0);
                            break;
                        case var x when x == typeof(int):
                            PlayerPrefs.SetInt(key, (int)value);
                            break;
                        case var x when x == typeof(float):
                            PlayerPrefs.SetFloat(key, (float)value);
                            break;
                        case var x when x == typeof(string):
                            PlayerPrefs.SetString(key, (string)value);
                            break;
                        case var x when x == typeof(Vector2):
                            PlayerPrefs.SetString(key, JsonUtility.ToJson(value));
                            break;
                    }
                    
                    PlayerPrefs.Save();
                    
                    break;
                case SettingType.SessionState:
                    
                    switch (typeof(T))
                    {
                        case var x when x == typeof(bool):
                            SessionState.SetBool(key, (bool)value);
                            break;
                        case var x when x == typeof(int):
                            SessionState.SetInt(key, (int)value);
                            break;
                        case var x when x == typeof(float):
                            SessionState.SetFloat(key, (float)value);
                            break;
                        case var x when x == typeof(string):
                            SessionState.SetString(key, (string)value);
                            break;
                        case var x when x == typeof(Vector2):
                            SessionState.SetString(key, JsonUtility.ToJson(value));
                            break;
                    }
                    
                    break;
            }
        }


        /// <summary>
        /// Resets all user editor settings for this asset when called.
        /// </summary>
        [MenuItem("Tools/Carter Games/Multi Scene/Reset User Editor Settings")]
        public static void ResetPrefs()
        {
            
        }
    }
}