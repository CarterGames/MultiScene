namespace CarterGames.Experimental.MultiScene
{
    /// <summary>
    /// An interface for syncing logic to when scene groups are loaded.
    /// </summary>
    /// <remarks>Enable is run second out of the interfaces.</remarks>
    public interface IMultiSceneEnable
    {
        void OnMultiSceneEnable();
    }
}