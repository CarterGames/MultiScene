#if UNITY_EDITOR

using UnityEditor;

namespace CarterGames.Experimental.MultiScene.Editor
{
    /// <summary>
    /// Container class that holds all the popups editor dialogues that the system uses to prompt the user some info...
    /// </summary>
    public static class MultiScenePopups
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Events
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */        
        
        /// <summary>
        /// Broadcasts the result of a editor dialogue popups for use...
        /// </summary>
        public static readonly MultiSceneEvt<string, bool> OnPopupResolved = new MultiSceneEvt<string, bool>();
        
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Dialogue Popups
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */        
        
        /// <summary>
        /// Displays a dialogue popup for the user trying to use an invalid scene group setup...
        /// </summary>
        public static void ShowInvalidSceneGroup()
        {
            OnPopupResolved.Raise("InvalidSceneGroup", EditorUtility.DisplayDialog("Invalid Scene Group Detected!",
                "The scene group you are trying to load is invalid! Please ensure you have setup the group correctly in the editor.", "Close"));
        }
    }
}

#endif