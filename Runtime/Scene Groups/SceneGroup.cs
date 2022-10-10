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
                    if (data.sceneName.Equals(string.Empty)) return false;

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
                
                var _list = new List<string>();
                
                foreach (var _t in scenes)
                {
                    if (_t.sceneName.Equals(scenes[0].sceneName)) continue;
                    _list.Add(_t.sceneName);
                }

                return _list;
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