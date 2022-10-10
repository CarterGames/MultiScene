using UnityEngine;

namespace CarterGames.Experimental.MultiScene
{
    /// <summary>
    /// Provides a base set of logic for loading a scene group to inherit from and override as needed.
    /// </summary>
    /// <remarks>
    /// Not abstract as it can be used as is for basic implementations.
    /// </remarks>
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