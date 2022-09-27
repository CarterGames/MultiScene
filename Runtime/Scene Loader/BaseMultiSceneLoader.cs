// Multi Scene - Core
// Base class for loading scene groups easily
// Author: Jonathan Carter - https://carter.games

using UnityEngine;

namespace CarterGames.Experimental.MultiScene
{
    public class BaseMultiSceneLoader : MonoBehaviour
    {
        /// <summary>
        /// The scene group to load...
        /// </summary>
        [SerializeField] protected SceneGroup loadGroup;

        
        /// <summary>
        /// Gets whether or not the scene group is loading or not...
        /// </summary>
        protected bool IsLoading { get; set; }
        
        
        /// <summary>
        /// Loads a scene group...
        /// </summary>
        public virtual void LoadSceneGroup()
        {
            if (IsLoading) return;
            MultiSceneManager.LoadScenes(loadGroup);
            IsLoading = true;
        }
    }
}