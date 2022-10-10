namespace CarterGames.Experimental.MultiScene
{
    /// <summary>
    /// An interface for syncing logic to when scene groups are loaded.
    /// </summary>
    /// <remarks>Start is run last out of the interfaces.</remarks>
    public interface IMultiSceneStart
    {
        void OnMultiSceneStart();
    }
}