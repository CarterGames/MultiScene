// Multi Scene - Core
// A interface to implement when you want logic to run after the awake & enable interface implementations once a scene group has loaded via the multi-scene manager.
// Author: Jonathan Carter - https://carter.games

namespace MultiScene.Core
{
    public interface IMultiSceneStart
    {
        void OnMultiSceneStart();
    }
}