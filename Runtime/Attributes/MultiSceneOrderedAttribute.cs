using System;

namespace CarterGames.Experimental.MultiScene
{
    /// <summary>
    /// Attribute | When applied to an Multi Scene interface, the method will be called in the execution order defined. 
    /// </summary>
    /// <remarks>
    /// The order attribute only works if the method it is on is a Multi Scene Interface Implementation, other methods will be ignored by the system at present.
    /// If the interface implementation has no order it will be set to 0 as it is the default, just like in the scripting execution order system in Unity. 
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class MultiSceneOrderedAttribute : Attribute
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        public int order;
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Constructors
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// A default ordered attribute.
        /// </summary>
        public MultiSceneOrderedAttribute()
        {
            order = 0;
        }
        
        /// <summary>
        /// A attribute with a defined order.
        /// </summary>
        public MultiSceneOrderedAttribute(int order = 0)
        {
            this.order = order;
        }
    }
}