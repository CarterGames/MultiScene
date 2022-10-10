using System;

namespace CarterGames.Experimental.MultiScene
{
    /// <summary>
    /// An event class with no params
    /// </summary>
    public sealed class MultiSceneEvt
    {
        private event Action Action = delegate { };

        /// <summary>
        /// Raises the event to all listeners.
        /// </summary>
        public void Raise() => Action.Invoke();

        /// <summary>
        /// Adds the action/method to the event listeners.
        /// </summary>
        public void Add(Action listener)
        {
            Action -= listener;
            Action += listener;
        }

        /// <summary>
        /// Removes the action/method to the event listeners.
        /// </summary>
        public void Remove(Action listener) => Action -= listener;
        
        /// <summary>
        /// Clears all listeners from the event.
        /// </summary>
        public void Clear() => Action = null;
    }
    
    
    /// <summary>
    /// An event class with 1 parameter...
    /// </summary>
    public sealed class MultiSceneEvt<T>
    {
        private event Action<T> Action = delegate { };

        /// <summary>
        /// Raises the event to all listeners.
        /// </summary>
        public void Raise(T param) => Action.Invoke(param);

        /// <summary>
        /// Adds the action/method to the event listeners.
        /// </summary>
        public void Add(Action<T> listener)
        {
            Action -= listener;
            Action += listener;
        }

        /// <summary>
        /// Removes the action/method to the event listeners.
        /// </summary>
        public void Remove(Action<T> listener) => Action -= listener;

        /// <summary>
        /// Clears all listeners from the event.
        /// </summary>
        public void Clear() => Action = null;
    }
    
    
    /// <summary>
    /// An event class with 2 parameters...
    /// </summary>
    public sealed class MultiSceneEvt<T1,T2>
    {
        private event Action<T1,T2> Action = delegate { };

        /// <summary>
        /// Raises the event to all listeners.
        /// </summary>
        public void Raise(T1 param1, T2 param2) => Action.Invoke(param1, param2);

        /// <summary>
        /// Adds the action/method to the event listeners.
        /// </summary>
        public void Add(Action<T1,T2> listener)
        {
            Action -= listener;
            Action += listener;
        }

        /// <summary>
        /// Removes the action/method to the event listeners.
        /// </summary>
        public void Remove(Action<T1,T2> listener) => Action -= listener;
        
        /// <summary>
        /// Clears all listeners from the event.
        /// </summary>
        public void Clear() => Action = null;
    }
    
    
    /// <summary>
    /// An event class with 3 parameters...
    /// </summary>
    public sealed class MultiSceneEvt<T1,T2,T3>
    {
        private event Action<T1,T2,T3> Action = delegate { };

        /// <summary>
        /// Raises the event to all listeners.
        /// </summary>
        public void Raise(T1 param1, T2 param2, T3 param3) => Action.Invoke(param1, param2, param3);

        /// <summary>
        /// Adds the action/method to the event listeners.
        /// </summary>
        public void Add(Action<T1,T2,T3> listener)
        {
            Action -= listener;
            Action += listener;
        }

        /// <summary>
        /// Removes the action/method to the event listeners.
        /// </summary>
        public void Remove(Action<T1,T2,T3> listener) => Action -= listener;
        
        /// <summary>
        /// Clears all listeners from the event.
        /// </summary>
        public void Clear() => Action = null;
    }
    
    
    /// <summary>
    /// An event class with 4 parameters...
    /// </summary>
    public sealed class MultiSceneEvt<T1,T2,T3,T4>
    {
        private event Action<T1,T2,T3,T4> Action = delegate { };

        /// <summary>
        /// Raises the event to all listeners.
        /// </summary>
        public void Raise(T1 param1, T2 param2, T3 param3, T4 param4) => Action.Invoke(param1, param2, param3, param4);

        /// <summary>
        /// Adds the action/method to the event listeners.
        /// </summary>
        public void Add(Action<T1,T2,T3,T4> listener)
        {
            Action -= listener;
            Action += listener;
        }

        /// <summary>
        /// Removes the action/method to the event listeners.
        /// </summary>
        public void Remove(Action<T1,T2,T3,T4> listener) => Action -= listener;
        
        /// <summary>
        /// Clears all listeners from the event.
        /// </summary>
        public void Clear() => Action = null;
    }

    
    /// <summary>
    /// An event class with 5 parameters...
    /// </summary>
    public sealed class MultiSceneEvt<T1,T2,T3,T4,T5>
    {
        private event Action<T1,T2,T3,T4,T5> Action = delegate { };

        /// <summary>
        /// Raises the event to all listeners.
        /// </summary>
        public void Raise(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5) 
            => Action.Invoke(param1, param2, param3, param4, param5);

        /// <summary>
        /// Adds the action/method to the event listeners.
        /// </summary>
        public void Add(Action<T1,T2,T3,T4,T5> listener)
        {
            Action -= listener;
            Action += listener;
        }

        /// <summary>
        /// Removes the action/method to the event listeners.
        /// </summary>
        public void Remove(Action<T1,T2,T3,T4,T5> listener) => Action -= listener;
        
        /// <summary>
        /// Clears all listeners from the event.
        /// </summary>
        public void Clear() => Action = null;
    }
    
    
    /// <summary>
    /// An event class with 6 parameters...
    /// </summary>
    public sealed class MultiSceneEvt<T1,T2,T3,T4,T5,T6>
    {
        private event Action<T1,T2,T3,T4,T5,T6> Action = delegate { };

        /// <summary>
        /// Raises the event to all listeners.
        /// </summary>
        public void Raise(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6) 
            => Action.Invoke(param1, param2, param3, param4, param5, param6);

        /// <summary>
        /// Adds the action/method to the event listeners.
        /// </summary>
        public void Add(Action<T1,T2,T3,T4,T5,T6> listener)
        {
            Action -= listener;
            Action += listener;
        }

        /// <summary>
        /// Removes the action/method to the event listeners.
        /// </summary>
        public void Remove(Action<T1,T2,T3,T4,T5,T6> listener) => Action -= listener;
        
        /// <summary>
        /// Clears all listeners from the event.
        /// </summary>
        public void Clear() => Action = null;
    }
    
    
    /// <summary>
    /// An event class with 7 parameters...
    /// </summary>
    public sealed class MultiSceneEvt<T1,T2,T3,T4,T5,T6,T7>
    {
        private event Action<T1,T2,T3,T4,T5,T6,T7> Action = delegate { };

        /// <summary>
        /// Raises the event to all listeners.
        /// </summary>
        public void Raise(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7) 
            => Action.Invoke(param1, param2, param3, param4, param5, param6, param7);

        /// <summary>
        /// Adds the action/method to the event listeners.
        /// </summary>
        public void Add(Action<T1,T2,T3,T4,T5,T6,T7> listener)
        {
            Action -= listener;
            Action += listener;
        }

        /// <summary>
        /// Removes the action/method to the event listeners.
        /// </summary>
        public void Remove(Action<T1,T2,T3,T4,T5,T6,T7> listener) => Action -= listener;
        
        /// <summary>
        /// Clears all listeners from the event.
        /// </summary>
        public void Clear() => Action = null;
    }
    
    
    /// <summary>
    /// An event class with 8 parameters...
    /// </summary>
    public sealed class MultiSceneEvt<T1,T2,T3,T4,T5,T6,T7,T8>
    {
        private event Action<T1,T2,T3,T4,T5,T6,T7,T8> Action = delegate { };

        /// <summary>
        /// Raises the event to all listeners.
        /// </summary>
        public void Raise(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8) 
            => Action.Invoke(param1, param2, param3, param4, param5, param6, param7, param8);

        /// <summary>
        /// Adds the action/method to the event listeners.
        /// </summary>
        public void Add(Action<T1,T2,T3,T4,T5,T6,T7,T8> listener)
        {
            Action -= listener;
            Action += listener;
        }

        /// <summary>
        /// Removes the action/method to the event listeners.
        /// </summary>
        public void Remove(Action<T1,T2,T3,T4,T5,T6,T7,T8> listener) => Action -= listener;
        
        /// <summary>
        /// Clears all listeners from the event.
        /// </summary>
        public void Clear() => Action = null;
    }
}