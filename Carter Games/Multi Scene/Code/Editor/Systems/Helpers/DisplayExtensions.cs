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