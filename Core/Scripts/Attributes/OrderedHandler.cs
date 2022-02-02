// ----------------------------------------------------------------------------
// OrderedHandler.cs
// 
// Author: Jonathan Carter (A.K.A. J)
// Date: 01/02/2022
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MultiScene.Core
{
    public static class OrderedHandler
    {
        public static List<OrderedListenerData<T>> OrderListeners<T>(List<T> listeners, string methodName)
        {
            var _data = new List<OrderedListenerData<T>>();
            
            foreach (var listener in listeners)
            {
                var method = listener.GetType().GetMethod(methodName);
                if (method == null) continue;
                var hasOrder = method.GetCustomAttributes(typeof(MultiSceneOrderedAttribute), true).Length > 0;
        
                if (!hasOrder)
                {
                    _data.Add(new OrderedListenerData<T>(listener, 0));
                    continue;
                }
                
                _data.Add(new OrderedListenerData<T>(listener, method.GetCustomAttribute<MultiSceneOrderedAttribute>().order));
            }

            return _data.OrderBy(t => t.order).ToList();
        }
    }
}
