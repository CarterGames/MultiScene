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

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CarterGames.Experimental.MultiScene
{
    /// <summary>
    /// Contains the data required to load a group of scenes...
    /// </summary>
    [CreateAssetMenu(fileName = "New Scene Group", menuName = "Carter Games/Multi Scene/New Scene Group", order = 0)]
    public class SceneGroup : MultiSceneAsset
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        public string groupCategory = string.Empty;
        public int groupCategoryIndex;
        public int buttonIndex;
        public string buttonLabel = string.Empty;
        public Color buttonColor = Color.white;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties / Getters
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// The scenes in this group...
        /// </summary>
        public List<SceneData> scenes = new List<SceneData>();


        /// <summary>
        /// Confirms if the scene group is setup with at-least 1 scene... If not it'll return false...
        /// </summary>
        public bool IsValid
        {
            get
            {
                if (scenes == null) return false;
                if (scenes.Count <= 0) return false;
                if (scenes[0].sceneName.Length <= 0) return false;
                
                foreach (var data in scenes)
                {
                    if (data.sceneName.Equals(string.Empty)) return false;
                }

                if (!scenes.Distinct().Count().Equals(scenes.Count)) return false;

                return true;
            }
        }
        

        /// <summary>
        /// Gets the base scene of this group...
        /// </summary>
        public string GetBaseScene
        {
            get
            {
                if (scenes == null) return string.Empty;
                if (scenes.Count <= 0) return string.Empty;
                return scenes[0].sceneName;
            }
        }

        
        /// <summary>
        /// Gets all the additive scenes in this group...
        /// </summary>
        public List<string> GetAdditiveScenes
        {
            get
            {
                if (scenes == null) return null;
                if (scenes.Count <= 0) return null;
                
                var list = new List<string>();
                
                foreach (var data in scenes)
                {
                    if (data.sceneName.Equals(scenes[0].sceneName)) continue;
                    list.Add(data.sceneName);
                }

                return list;
            }
        }


        /// <summary>
        /// Checks whether or not the group contains the scene name entered...
        /// </summary>
        /// <param name="toFind">The scene to find</param>
        /// <returns>True or False</returns>
        public bool ContainsScene(string toFind)
        {
            for (var i = 0; i < scenes.Count; i++)
            {
                if (scenes[i].sceneName != toFind) continue;
                return true;
            }

            return false;
        }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */        
        
        /// <summary>
        /// Clears the asset of all the scenes setup...
        /// </summary>
        private void ClearAsset()
        {
            scenes?.Clear();
        }
    }
}