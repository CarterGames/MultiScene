namespace CarterGames.Experimental.MultiScene
{
    /// <summary>
    /// An interface for syncing logic to when scene groups are loaded.
    /// </summary>
    /// <remarks>Awake is run first out of the interfaces.</remarks>
    public interface IMultiSceneAwake
    {
        void OnMultiSceneAwake();
    }
}