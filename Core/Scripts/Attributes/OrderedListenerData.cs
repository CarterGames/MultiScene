// ----------------------------------------------------------------------------
// OrderedListenerData.cs
// 
// Author: Jonathan Carter (A.K.A. J)
// Date: 01/02/2022
// ----------------------------------------------------------------------------

using System;

namespace MultiScene.Core
{
    [Serializable]
    public class OrderedListenerData<T>
    {
        public int order;
        public T listener;
        
        public OrderedListenerData(T listener, int order)
        {
            this.listener = listener;
            this.order = order;
        }
    }
}