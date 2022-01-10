/*
 * 
 *  Multi-Scene Workflow
 *							  
 *	Base Multi-Scene Loader
 *      A base class that you can extend to load scenes in a variety of ways.
 *			
 *  Written by:
 *      Jonathan Carter
 *		
 *	Last Updated: 05/11/2021 (d/m/y)							
 * 
 */

using UnityEngine;

namespace MultiScene.Core
{
    public class BaseMultiSceneLoader : MonoBehaviour
    {
        /// <summary>
        /// The scene group to load...
        /// </summary>
        [SerializeField] protected SceneGroup loadGroup;

        /// <summary>
        /// Loads a scene group.
        /// </summary>
        public virtual void LoadSceneGroup()
        {
            MultiSceneElly.GetComponentFromAllScenes<MultiSceneManager>().LoadScenes(loadGroup);
        }
    }
}