// ----------------------------------------------------------------------------
// GroupCategory.cs
// 
// Author: Jonathan Carter (A.K.A. J)
// Date: 25/08/2022
// ----------------------------------------------------------------------------

using System;

namespace CarterGames.Experimental.MultiScene
{
    /// <summary>
    /// The data class for the default scene groups.
    /// </summary>
    [Serializable]
    public class GroupCategory
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The name of the group.
        /// </summary>
        public string groupName;
        
        /// <summary>
        /// The sorting index of the group.
        /// </summary>
        public int groupIndex;
        
        /// <summary>
        /// Should the group be shown.
        /// </summary>
        public bool showGroup;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Constructors
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Creates a group category will the name entered.
        /// </summary>
        /// <param name="groupName">The name of the group.</param>
        public GroupCategory(string groupName)
        {
            this.groupName = groupName;
            groupIndex = 0;
            showGroup = true;
        }
    }
}