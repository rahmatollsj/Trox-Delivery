﻿using System.Linq;
using UnityEditor;
using Harmony;
using UnityEngine;
using System.Reflection;

namespace Game
{
    // Author: David Pagotto
    // Ajoute un bouton qui permet de remplir la classe auto-générée Prefabs automatiquement.
    [CustomEditor(typeof(Prefabs))]
    public class PrefabsFillerEditor : Editor
    {
        public override void OnInspectorGUI() 
        {
            if (EditorApplication.isPlaying)
                EditorGUILayout.HelpBox("You are currently in Play Mode. Updated prefabs won't be saved!", MessageType.Warning);
            if(GUILayout.Button("Update prefabs GameObject"))
                UpdateAutoGeneratedPrefabsGameObjects();
            DrawDefaultInspector();
        }

        public void UpdateAutoGeneratedPrefabsGameObjects()
        {
            // On trouve les ressources qui sont des préfabs et on les lie à leur SerializeField dans la classe auto-générée Prefabs.
            var prefabs = AssetDatabase.GetAllAssetPaths().Where(p => p.EndsWith(".prefab"));
            //var mainController = mainScene.GetRootGameObjects().First(o => o.name == GameObjects.MainController);
            var generatedPrefabs = (Prefabs)target;
            var fields = typeof(Prefabs).GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Where(f => f.GetCustomAttribute<SerializeField>() != null);

            Undo.RecordObject(generatedPrefabs, "Updated Prefabs GameObjects");
            foreach (var field in fields)
            {
               if ((field.GetValue(generatedPrefabs) as GameObject)?.Exists() ?? false)
                   continue;
                string fieldName = field.Name;
                var prefabName = fieldName.Substring(0, fieldName.LastIndexOf("Prefab"));

                var assetName = prefabs.Where(file => file.Contains(prefabName)).FirstOrDefault();
                if(assetName == null)
                {
                    Debug.LogError($"Couldn't find prefab file for {prefabName}");
                    continue;
                }
                GameObject prefabObject = AssetDatabase.LoadAssetAtPath<GameObject>(assetName);
                var prefabType = PrefabUtility.GetPrefabAssetType(prefabObject);
                if (prefabObject == null || prefabType == PrefabAssetType.NotAPrefab || prefabType == PrefabAssetType.MissingAsset)
                {
                    Debug.LogError($"Failed to link prefab {prefabName}: Invalid prefab or not a prefab.");
                    continue;
                }
                field.SetValue(generatedPrefabs, prefabObject);
            }
        }
    }
}
