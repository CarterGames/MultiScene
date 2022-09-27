using System;

namespace CarterGames.Experimental.MultiScene
{
    /// <summary>
    /// Controls the data required for each scene in a scene group...
    /// </summary>
    [Serializable]
    public struct SceneData
    {
        /// <summary>
        /// The name of the scene without any extensions such as (.unity)...
        /// </summary>
        public string sceneName;
        
        
        /// <summary>
        /// The path in the project for the scene...
        /// </summary>
        public string scenePath;
        
        
        /// <summary>
        /// Defines if the scene is in the build settings of the project...
        /// </summary>
        public bool isInBuildSettings;
    }
}