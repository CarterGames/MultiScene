// ----------------------------------------------------------------------------
// MSLog.cs
// 
// Author: Jonathan Carter (A.K.A. J)
// Date: 31/08/2022
// ----------------------------------------------------------------------------

using UnityEngine;

namespace CarterGames.Experimental.MultiScene
{
    public class MsLog
    {
        private const string LogPrefix = "<color=#9379e0><b>Multi Scene</b></color> | ";
        private const string WarningPrefix = "<color=#D6BA64><b>Warning</b></color> | ";
        private const string ErrorPrefix = "<color=#E77A7A><b>Error</b></color> | ";

        
        /// <summary>
        /// Displays a normal debug message for the build versions asset...
        /// </summary>
        /// <param name="message">The message to show...</param>
        public static void Normal(string message)
        {
            Debug.Log($"{LogPrefix}{message}");
        }
        
        
        /// <summary>
        /// Displays a warning debug message for the build versions asset...
        /// </summary>
        /// <param name="message">The message to show...</param>
        public static void Warning(string message) 
        {
            Debug.LogWarning($"{LogPrefix}{WarningPrefix}{message}");
        }
        
        
        /// <summary>
        /// Displays a error debug message for the build versions asset...
        /// </summary>
        /// <param name="message">The message to show...</param>
        public static void Error(string message)
        {
            Debug.LogError($"{LogPrefix}{ErrorPrefix}{message}");
        }
    }
}