// ----------------------------------------------------------------------------
// MultiSceneManagerEditor.cs
// 
// Author: Jonathan Carter (A.K.A. J)
// Date: 10/11/2021
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JTools.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MultiScene.Core.Editor
{
    [CustomEditor(typeof(MultiSceneManager))]
    public class MultiSceneManagerEditor : UnityEditor.Editor
    {
        private MultiSceneManager multiSceneManager;

        private void OnEnable()
        {
            multiSceneManager = target as MultiSceneManager;
        }

        public override void OnInspectorGUI()
        {
            Button.ColourButton("Load All In Editor (Will throw editor error, ignore it)", Colours.Green, LoadActiveSceneGroupInEditor);
            Button.ColourButton("Load All Additive In Editor", Colours.Yellow, LoadAdditiveActiveSceneGroupInEditor);
            base.OnInspectorGUI();
        }

        
        private void LoadActiveSceneGroupInEditor()
        {
            var _sceneList = multiSceneManager.scenesToLoad.scenes;
            var _paths = GetScenePaths();
            if (_sceneList.Count <= 0) return;

            for (var i = 0; i < _sceneList.Count; i++)
            {
                var _scene = _sceneList[i];
                var _path = _paths.FirstOrDefault(t => t.Contains(_scene));

                if (i.Equals(0))
                    EditorSceneManager.OpenScene(_path, OpenSceneMode.Single);
                else
                    EditorSceneManager.OpenScene(_path, OpenSceneMode.Additive);
            }
        }
        

        private void LoadAdditiveActiveSceneGroupInEditor()
        {
            var _sceneList = multiSceneManager.scenesToLoad.scenes;
            var _paths = GetScenePaths();
            if (_sceneList.Count <= 0) return;

            for (var i = 0; i < _sceneList.Count; i++)
            {
                var _scene = _sceneList[i];
                var _path = _paths.FirstOrDefault(t => t.Contains(_scene));

                if (i.Equals(0)) continue;
                EditorSceneManager.OpenScene(_path, OpenSceneMode.Additive);
            }
        }


        private List<string> GetScenePaths()
        {
            var sceneNumber = SceneManager.sceneCountInBuildSettings;
            string[] arrayOfNames;
            arrayOfNames = new string[sceneNumber];
            
            for (int i = 0; i < sceneNumber; i++)
                arrayOfNames[i] = SceneUtility.GetScenePathByBuildIndex(i);

            return arrayOfNames.ToList();
        }
    }
}