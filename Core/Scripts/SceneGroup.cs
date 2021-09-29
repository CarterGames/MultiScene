﻿// ----------------------------------------------------------------------------
// SceneGroup.cs
// 
// Author: Jonathan Carter (A.K.A. J)
// Date: 31/08/2021
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace JTools.MultiScene
{
    [CreateAssetMenu(fileName = "Scene Group", menuName = "JTools/Multi-Scene/Scene Group", order = 0)]
    public class SceneGroup : ScriptableObject
    {
        public List<string> scenes;
    }
}