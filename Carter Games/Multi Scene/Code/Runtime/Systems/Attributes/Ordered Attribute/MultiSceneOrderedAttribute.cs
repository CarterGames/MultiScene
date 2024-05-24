/*
 * Copyright (c) 2024 Carter Games
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

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