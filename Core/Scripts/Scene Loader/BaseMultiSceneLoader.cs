// Multi Scene - Core
// Base class for loading scene groups easily
// Author: Jonathan Carter - https://carter.games

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
        /// Gets whether or not the scene group is loading or not...
        /// </summary>
        private bool IsLoading { get; set; }
        
        
        /// <summary>
        /// Loads a scene group...
        /// </summary>
        public virtual void LoadSceneGroup()
        {
            if (IsLoading) return;
            MultiSceneElly.GetComponentFromAllScenes<MultiSceneManager>().LoadScenes(loadGroup);
            IsLoading = true;
        }
    }
}