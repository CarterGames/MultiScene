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

#if UNITY_EDITOR

using CarterGames.Common;
using UnityEditor;

namespace CarterGames.Experimental.MultiScene.Editor
{
    /// <summary>
    /// Container class that holds all the popups editor dialogues that the system uses to prompt the user some info...
    /// </summary>
    public static class MultiScenePopups
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Events
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */        
        
        /// <summary>
        /// Broadcasts the result of a editor dialogue popups for use...
        /// </summary>
        public static readonly Evt<string, bool> OnPopupResolved = new Evt<string, bool>();
        
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Dialogue Popups
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */        
        
        /// <summary>
        /// Displays a dialogue popup for the user trying to use an invalid scene group setup...
        /// </summary>
        public static void ShowInvalidSceneGroup()
        {
            OnPopupResolved.Raise("InvalidSceneGroup", EditorUtility.DisplayDialog("Invalid Scene Group Detected!",
                "The scene group you are trying to load is invalid! Please ensure you have setup the group correctly in the editor.", "Close"));
        }
    }
}

#endif