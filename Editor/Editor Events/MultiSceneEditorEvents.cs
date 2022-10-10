namespace CarterGames.Experimental.MultiScene.Editor
{
    /// <summary>
    /// Container class for events used to broadcast info in the editor space only...
    /// </summary>
    public struct MultiSceneEditorEvents
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Scene Group Events
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */     
        
        /// <summary>
        /// Contains any events that relate to scene group categories...
        /// </summary>
        public struct SceneGroups
        {
            /// <summary>
            /// Raises when a category is changed...
            /// </summary>
            public static readonly MultiSceneEvt OnSceneGroupCreated = new MultiSceneEvt();
            

            /// <summary>
            /// Raises when a category is changed...
            /// </summary>
            public static readonly MultiSceneEvt OnSceneGroupCategoryChanged = new MultiSceneEvt();
            
            
            /// <summary>
            /// Raises when a scene group is loaded in the editor only...
            /// </summary>
            public static readonly MultiSceneEvt OnSceneGroupLoadedInEditor = new MultiSceneEvt();
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Settings Events
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */ 
        
        /// <summary>
        /// Contains any events that relate to the settings asset being modified...
        /// </summary>
        public struct Settings
        {
            /// <summary>
            /// Raises when the settings asset is modified...
            /// </summary>
            public static readonly MultiSceneEvt OnSettingChanged = new MultiSceneEvt();
            
            
            /// <summary>
            /// Raises when a new settings asset is generated after an existing one is deleted...
            /// </summary>
            public static readonly MultiSceneEvt OnSettingsAssetRegenerated = new MultiSceneEvt();
            
            
            /// <summary>
            /// Raises when the group categories are edited in the settings window...
            /// </summary>
            public static readonly MultiSceneEvt OnGroupCategoriesChanged = new MultiSceneEvt();
        }
    }
}