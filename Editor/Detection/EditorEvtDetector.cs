using UnityEditor;

namespace CarterGames.Experimental.MultiScene.Editor
{
    /// <summary>
    /// Detects events in the engine and processes them for use in the asset. 
    /// </summary>
    public sealed class EditorEvtDetector : AssetPostprocessor
    {
        /// <summary>
        /// Runs when any file has finished being added to the project.
        /// </summary>
        /// <param name="importedAssets">array of all imported assets.</param>
        /// <param name="deletedAssets">array of all deleted assets.</param>
        /// <param name="movedAssets">array of all moved assets.</param>
        /// <param name="movedFromAssetPaths">array of all moved assets to a new path.</param>
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (var asset in importedAssets)
            {
                if (!asset.Contains(".asset")) continue;
                MultiSceneEditorEvents.SceneGroups.OnSceneGroupCreated.Raise();
                return;
            }
        }
    }
}