using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CarterGames.Experimental.MultiScene
{
    public static class AssetAccessor
    {
        //
        //
        //  Fields
        //
        //
        
        // a cache of all the assets found...
        private static MultiSceneAsset[] assets;

        
        //
        //
        //  Properties
        //
        //
        
        
        /// <summary>
        /// Gets all the assets from the build versions asset...
        /// </summary>
        private static IEnumerable<MultiSceneAsset> Assets
        {
            get
            {
                if (assets != null) return assets;
                assets = Resources.LoadAll("Carter Games/Multi Scene", typeof(MultiSceneAsset)).Cast<MultiSceneAsset>().ToArray();
                return assets;
            }
        }

        
        //
        //
        //  Methods
        //
        //
        
        
        /// <summary>
        /// Gets the Build Versions Asset requested...
        /// </summary>
        /// <typeparam name="T">The build versions asset to get.</typeparam>
        /// <returns>The asset if it exists.</returns>
        public static T GetAsset<T>() where T : MultiSceneAsset
        {
            return (T)Assets.FirstOrDefault(t => t.GetType() == typeof(T));
        }
    }
}