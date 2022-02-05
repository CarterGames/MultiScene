// Multi Scene - Core
// The definition of the MultiSceneOrdered attribute.
// Author: Jonathan Carter - https://carter.games

using System;

namespace MultiScene.Core
{
    /// <summary>
    /// Attribute | When applied to an Multi Scene interface, the method will be called in the execution order defined. 
    /// </summary>
    /// <remarks>
    /// The order attribute only works if the method it is on is a Multi Scene Interface Implementation, other methods will be ignored by the system at present.
    /// If the interface implementation has no order it will be set to 0 as it is the default, just like in the scripting execution order system in Unity. 
    /// </remarks>
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