using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CarterGames.Experimental.MultiScene.Editor
{
    /// <summary>
    /// Handles the settings for the asset.
    /// </summary>
    public sealed class MultiSceneSettingsAsset : MultiSceneAsset
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        [SerializeField] private SceneGroupEditorLoadMode sceneGroupLoadMode;
        [SerializeField] private SceneGroup startGroup;
        [SerializeField] private SceneGroup lastGroupLoaded;

        [SerializeField] private int listenerFrequency = 5;
        [SerializeField] private bool useUnloadResources;
        [SerializeField] private bool showLogs = true;
        
        
        [SerializeField] private List<GroupCategory> userGroupCategories = new List<GroupCategory>();

        [SerializeField] private List<DefaultGroupCategory> defaultCategories = new List<DefaultGroupCategory>()
        {
            new DefaultGroupCategory(string.Empty),
            new DefaultGroupCategory("Menu"), 
            new DefaultGroupCategory("Game"),
            new DefaultGroupCategory("Levels"),
            new DefaultGroupCategory("Player"),
            new DefaultGroupCategory("Enemies"),
            new DefaultGroupCategory("World"),
            new DefaultGroupCategory("UI"),
        };


        [SerializeField, HideInInspector] private bool showSceneGroupOptions;
        [SerializeField, HideInInspector] private bool showGeneralOptions;
        [SerializeField, HideInInspector] private bool showGroupCategoryOptions;
        [SerializeField, HideInInspector] private bool showDefaultGroupsInSetAsset;
        [SerializeField, HideInInspector] private bool showUserGroupsInSetAsset;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The scene group to load first.
        /// </summary>
        public SceneGroup StartGroup => startGroup;
        
        /// <summary>
        /// How many of the listeners of IMultiSceneAwake/Enable/Start invoke per frame.
        /// </summary>
        public int ListenerFrequency => listenerFrequency;
        
        /// <summary>
        /// Defines if the Resources.UnloadUnusedAssets is used when changing scene groups.
        /// </summary>
        public bool UseUnloadResources => useUnloadResources;
        
        /// <summary>
        /// All the scene group categories defined in the settings.
        /// </summary>
        public List<GroupCategory> AllGroupCategories => defaultCategories.Concat(userGroupCategories).ToList();
        
        /// <summary>
        /// Should the asset should log messages?
        /// </summary>
        public bool UseLogs => showLogs;
        
        /// <summary>
        /// The load mode to use when loading scene groups.
        /// </summary>
        public SceneGroupEditorLoadMode LoadMode => sceneGroupLoadMode;

        /// <summary>
        /// The last scene group loaded if saved.
        /// </summary>
        public SceneGroup LastGroup
        {
            get => lastGroupLoaded;
            set => lastGroupLoaded = value;
        }
    }
}