using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Reflection;
using Harmony;
using System.Linq;

namespace Game
{
    [CustomEditor(typeof(LevelManager))]
    public class LevelManagerEditor : Editor
    {
        private static bool foldoutToggle = true;
        private static int selectedPopoutIndex = 0;
        private static string[] levelNames;
        private static string[] blacklist = { "Game", "Home", "Main" };
        LevelManager levelManager;

        static LevelManagerEditor()
        {
            levelNames = typeof(Scenes).GetFields(BindingFlags.Public | BindingFlags.Static)
                .Select(t => t.Name)
                .Where(s => !blacklist.Contains(s))
                .ToArray();

        }

        private void Awake()
        {
            levelManager = (LevelManager)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            foldoutToggle = EditorGUILayout.Foldout(foldoutToggle, "Level Loader");
            if (!foldoutToggle)
                return;
            selectedPopoutIndex = EditorGUILayout.Popup("Level", selectedPopoutIndex, levelNames);
            if (GUILayout.Button("Load Level"))
                levelManager.LoadLevel(levelNames[selectedPopoutIndex]);

            EditorGUILayout.HelpBox($"Current Level: {levelManager.CurrentLevel.ToString()}", MessageType.None);
        }

    }
}
