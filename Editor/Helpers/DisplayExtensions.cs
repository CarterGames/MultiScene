using System.Collections.Generic;
using System.Linq;

namespace CarterGames.Experimental.MultiScene.Editor
{
    /// <summary>
    /// An extension class to get help formatting of the group categories & scene groups into something legible...
    /// </summary>
    public static class DisplayExtensions
    {
        /// <summary>
        /// Converts the scene group names into a usable array of options to select from...
        /// </summary>
        /// <param name="input">The strings to edit</param>
        /// <returns>The edited list as an array...</returns>
        public static string[] ToDisplayOptions(this List<string> input)
        {
            var array = new string[input.Count];

            for (var i = 0; i < input.Count; i++)
            {
                array[i] = (input[i].Equals(string.Empty)
                    ? "Unassigned (Blank)"
                    : input[i]);
            }

            return array;
        }
        
        
        /// <summary>
        /// Converts the scene group names into a usable array of options to select from...
        /// </summary>
        /// <param name="input">The strings to edit</param>
        /// <returns>The edited list as an array...</returns>
        public static string[] ToDisplayOptions<T>(this Dictionary<string, T> input)
        {
            var array = new string[input.Count];
            var keys = input.Keys.ToArray();

            for (var i = 0; i < input.Count; i++)
            {
                array[i] = (keys[i].Equals(string.Empty)
                    ? "Unassigned (Blank)"
                    : keys[i]);
            }

            return array;
        }
    }
}