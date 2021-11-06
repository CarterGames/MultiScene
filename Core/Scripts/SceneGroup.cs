/*
 * 
 *  Multi-Scene Workflow
 *							  
 *	Scene Group
 *      A scriptable object that allows you to group scenes together to load in the multi-scene manager.
 *			
 *  Written by:
 *      Jonathan Carter
 *		
 *	Last Updated: 05/11/2021 (d/m/y)							
 * 
 */

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MultiScene.Core
{
    [CreateAssetMenu(fileName = "Scene Group", menuName = "Multi-Scene/Scene Group", order = 0)]
    public class SceneGroup : ScriptableObject
    {
        /// <summary>
        /// The scenes in this group.
        /// </summary>
        public List<string> scenes;

        /// <summary>
        /// Gets the base scene of this group.
        /// </summary>
        public string GetBaseScene
        {
            get
            {
                if (scenes == null) return string.Empty;
                if (scenes.Count <= 0) return string.Empty;
                return scenes[0];
            }
        }

        /// <summary>
        /// Gets all the additive scenes in this group.
        /// </summary>
        public List<string> GetAdditiveScenes
        {
            get
            {
                if (scenes == null) return null;
                if (scenes.Count <= 0) return null;
                return scenes.Where(t => !t.Equals(scenes[0])).ToList();
            }
        }
        
        /// <summary>
        /// Checks whether or not the group contains the scene name entered.
        /// </summary>
        /// <param name="toFind">The scene to find</param>
        /// <returns>True or False</returns>
        public bool ContainsScene(string toFind) => scenes.Contains(toFind);
    }
}