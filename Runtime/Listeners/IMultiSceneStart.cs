// Multi Scene - Core
// A interface to implement when you want logic to run after the awake & enable interface implementations once a scene group has loaded via the multi-scene manager.
// Author: Jonathan Carter - https://carter.games

namespace CarterGames.Experimental.MultiScene
{
    public interface IMultiSceneStart
    {
        void OnMultiSceneStart();
    }
}