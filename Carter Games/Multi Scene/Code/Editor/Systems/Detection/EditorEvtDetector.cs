/*
 * Copyright (c) 2024 Carter Games
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using UnityEditor;

namespace CarterGames.Experimental.MultiScene.Editor
{
    /// <summary>
    /// Detects events in the engine and processes them for use in the asset. 
    /// </summary>
    public sealed class EditorEvtDetector : AssetPostprocessor, IAssetEditorReload
    {
        private static string[] importedAssetsCache;
        
        
        /// <summary>
        /// Runs when any file has finished being added to the project.
        /// </summary>
        /// <param name="importedAssets">array of all imported assets.</param>
        /// <param name="deletedAssets">array of all deleted assets.</param>
        /// <param name="movedAssets">array of all moved assets.</param>
        /// <param name="movedFromAssetPaths">array of all moved assets to a new path.</param>
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            importedAssetsCache = importedAssets;
        }
        

        public void OnEditorReloaded()
        {
            if (importedAssetsCache == null) return;
            
            foreach (var asset in importedAssetsCache)
            {
                if (!asset.Contains(".asset")) continue;
                MultiSceneEditorEvents.SceneGroups.OnSceneGroupCreated.Raise();
                return;
            }
        }
    }
}