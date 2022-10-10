using System;

namespace CarterGames.Experimental.MultiScene
{
    /// <summary>
    /// The data class for the default scene groups.
    /// </summary>
    [Serializable]
    public sealed class DefaultGroupCategory : GroupCategory
    {
        public DefaultGroupCategory(string groupName) : base(groupName) { }
    }
}