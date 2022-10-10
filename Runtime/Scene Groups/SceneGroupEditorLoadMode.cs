// ----------------------------------------------------------------------------
// SceneGroupLoadMode.cs
// 
// Author: Jonathan Carter (A.K.A. J)
// Date: 31/08/2022
// ----------------------------------------------------------------------------

namespace CarterGames.Experimental.MultiScene
{
    /// <summary>
    /// The load-modes for the editor to load a scene group when entering playmode.
    /// </summary>
    public enum SceneGroupEditorLoadMode
    {
        Default,
        LastLoaded,
        None,
    }
}