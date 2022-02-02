// ----------------------------------------------------------------------------
// OrderAttribute.cs
// 
// Author: Jonathan Carter (A.K.A. J)
// Date: 01/02/2022
// ----------------------------------------------------------------------------

using System;

namespace MultiScene.Core
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MultiSceneOrderedAttribute : Attribute
    {
        public int order;

        public MultiSceneOrderedAttribute()
        {
            order = 0;
        }
        
        public MultiSceneOrderedAttribute(int order = 0)
        {
            this.order = order;
        }
    }
}