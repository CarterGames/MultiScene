using UnityEditor;

namespace CarterGames.Experimental.MultiScene.Editor
{
    public class EditorEvtDetector : AssetPostprocessor
    {
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