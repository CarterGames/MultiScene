using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CarterGames.Experimental.MultiScene.Editor
{
    public class MultiSceneSettingsAsset : MultiSceneAsset
    {
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

        public SceneGroup StartGroup => startGroup;
        public int ListenerFrequency => listenerFrequency;
        public bool UseUnloadResources => useUnloadResources;
        public List<GroupCategory> AllGroupCategories => defaultCategories.Concat(userGroupCategories).ToList();
        public bool UseLogs => showLogs;
        public SceneGroupEditorLoadMode LoadMode => sceneGroupLoadMode;

        public SceneGroup LastGroup
        {
            get => lastGroupLoaded;
            set => lastGroupLoaded = value;
        }
        

        public void ApplyShowGroupChange(GroupCategory groupCat)
        {
            if (defaultCategories.Contains(groupCat))
            {
                for (var i = 0; i < defaultCategories.Count; i++)
                {
                    if (!defaultCategories[i].Equals(groupCat)) continue;
                    defaultCategories[i].showGroup = groupCat.showGroup;
                    return;
                }
            }
            else
            {
                for (var i = 0; i < userGroupCategories.Count; i++)
                {
                    if (!userGroupCategories[i].Equals(groupCat)) continue;
                    userGroupCategories[i].showGroup = groupCat.showGroup;
                    return;
                }
            }
        }
    }
}