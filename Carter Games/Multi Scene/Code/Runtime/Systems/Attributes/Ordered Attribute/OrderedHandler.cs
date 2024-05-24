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

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CarterGames.Experimental.MultiScene
{
    /// <summary>
    /// A class to handle sorting ordered listeners.
    /// </summary>
    public static class OrderedHandler
    {
        /// <summary>
        /// Gets the listeners in the order they are defined via the attributes. 
        /// </summary>
        /// <param name="listeners">The listeners to process</param>
        /// <param name="methodName">The method name to look out for</param>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns>A list of ordered listeners based on their defined orders.</returns>
        /// <remarks>
        /// The order attribute only works if the method it is on is a Multi Scene Interface Implementation, other methods will be ignored by the system at present.
        /// If the interface implementation has no order it will be set to 0 as it is the default, just like in the scripting execution order system in Unity. 
        /// </remarks>
        public static List<OrderedListenerData<T>> OrderListeners<T>(List<T> listeners, string methodName)
        {
            var _data = new List<OrderedListenerData<T>>();
            
            foreach (var _listener in listeners)
            {
                var _method = _listener.GetType().GetMethod(methodName);
                if (_method == null) continue;
                var _hasOrder = _method.GetCustomAttributes(typeof(MultiSceneOrderedAttribute), true).Length > 0;
        
                if (!_hasOrder)
                {
                    _data.Add(new OrderedListenerData<T>(0, _listener));
                    continue;
                }
                
                _data.Add(new OrderedListenerData<T>(_method.GetCustomAttribute<MultiSceneOrderedAttribute>().order, _listener));
            }

            return _data.OrderBy(t => t.Order).ToList();
        }
    }
}
