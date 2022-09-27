using UnityEditor;
using UnityEngine;

namespace CarterGames.Experimental.MultiScene.Editor
{
    [CustomPropertyDrawer(typeof(GroupCategory))]
    public class GroupCategoryDrawer : PropertyDrawer
    {
        
/* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
 {  Fields  }
───────────────────────────────────────────────────────────────────────────────────────────────────────────────────── */ 

        private static SerializedProperty _nameProp;
        private static SerializedProperty _indexProp;

/* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
 {  Drawer Methods  }
───────────────────────────────────────────────────────────────────────────────────────────────────────────────────── */ 

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);
            
            _nameProp = property.FindPropertyRelative("groupName");
            _indexProp = property.FindPropertyRelative("groupIndex");

            EditorGUI.BeginChangeCheck();

            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var _left = new Rect(position.x, position.y, (position.width / 4) * 3 - 1.5f, EditorGUIUtility.singleLineHeight);
            var _right = new Rect(position.x + position.width / 4 * 3 + 1.5f, position.y, (position.width / 4) - 1.5f, EditorGUIUtility.singleLineHeight);
            
            EditorGUI.PropertyField(_left, _nameProp, GUIContent.none);
            EditorGUI.PropertyField(_right, _indexProp, GUIContent.none);
            
            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();            
            
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}