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

using CarterGames.Common;

namespace CarterGames.Experimental.MultiScene.Editor
{
    /// <summary>
    /// Container class for events used to broadcast info in the editor space only...
    /// </summary>
    public struct MultiSceneEditorEvents
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Scene Group Events
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */     
        
        /// <summary>
        /// Contains any events that relate to scene group categories...
        /// </summary>
        public struct SceneGroups
        {
            /// <summary>
            /// Raises when a category is changed...
            /// </summary>
            public static readonly Evt OnSceneGroupCreated = new Evt();
            

            /// <summary>
            /// Raises when a category is changed...
            /// </summary>
            public static readonly Evt OnSceneGroupCategoryChanged = new Evt();
            
            
            /// <summary>
            /// Raises when a scene group is loaded in the editor only...
            /// </summary>
            public static readonly Evt OnSceneGroupLoadedInEditor = new Evt();
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Settings Events
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */ 
        
        /// <summary>
        /// Contains any events that relate to the settings asset being modified...
        /// </summary>
        public struct Settings
        {
            /// <summary>
            /// Raises when the settings asset is modified...
            /// </summary>
            public static readonly Evt OnSettingChanged = new Evt();
            
            
            /// <summary>
            /// Raises when a new settings asset is generated after an existing one is deleted...
            /// </summary>
            public static readonly Evt OnSettingsAssetRegenerated = new Evt();
            
            
            /// <summary>
            /// Raises when the group categories are edited in the settings window...
            /// </summary>
            public static readonly Evt OnGroupCategoriesChanged = new Evt();
        }
    }
}