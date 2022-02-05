// Multi Scene - Core
// The editor script for the multi scene manager. 
// Author: Jonathan Carter - https://carter.games

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

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
            if (GUILayout.Button("Load All In Editor (Will throw editor error, ignore it)"))
            {
                LoadActiveSceneGroupInEditor();
            }
            
            if (GUILayout.Button("Load All Additive In Editor"))
            {
                LoadAdditiveActiveSceneGroupInEditor();
            }
            
            base.OnInspectorGUI();
        }

        
        private void LoadActiveSceneGroupInEditor()
        {
            var _sceneList = multiSceneManager.defaultGroup.scenes;
            var _paths = GetScenePaths();
            if (_sceneList.Count <= 0) return;

            Selection.objects = Array.Empty<Object>();

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
            var _sceneList = multiSceneManager.defaultGroup.scenes;
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