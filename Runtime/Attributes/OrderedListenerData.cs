// Multi Scene - Core
// A data class to help handle the ordering of the listeners based on the ordering attribute.
// Author: Jonathan Carter - https://carter.games

using System;
using UnityEngine;

namespace CarterGames.Experimental.MultiScene
{
    /// <summary>
    /// A data class for holding ordered listeners for the multi scene manager to use
    /// </summary>
    /// <typeparam name="T">The interface type to use</typeparam>
    [Serializable]
    public sealed class OrderedListenerData<T>
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        [SerializeField] private int order;
        [SerializeField] private T listener;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The order to use for this listener
        /// </summary>
        public int Order => order;
        
        /// <summary>
        /// The listener for to use
        /// </summary>
        public T Listener => listener;
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Constructors
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Constructor | Implements the order & listener automatically
        /// </summary>
        /// <param name="order">The order to set</param>
        /// <param name="listener">The listener to set</param>
        public OrderedListenerData(int order, T listener)
        {
            this.listener = listener;
            this.order = order;
        }
    }
}