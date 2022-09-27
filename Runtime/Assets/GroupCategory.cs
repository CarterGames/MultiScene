// ----------------------------------------------------------------------------
// GroupCategory.cs
// 
// Author: Jonathan Carter (A.K.A. J)
// Date: 25/08/2022
// ----------------------------------------------------------------------------

using System;

namespace CarterGames.Experimental.MultiScene
{
    [Serializable]
    public class GroupCategory
    {
        public string groupName;
        public int groupIndex;
        public bool showGroup;

        
        public GroupCategory(string groupName)
        {
            this.groupName = groupName;
            groupIndex = 0;
            showGroup = true;
        }
    }
}