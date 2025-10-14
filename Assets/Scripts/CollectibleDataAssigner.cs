using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

public static class CollectibleDataAssigner
{
    [MenuItem("Assets/Assign CollectibleData to selected prefabs", true)]
    private static bool ValidateAssignMenu()
    {
        return Selection.objects != null && Selection.objects.Length > 0;
    }

    [MenuItem("Assets/Assign CollectibleData to selected prefabs")]
    private static void AssignToSelectedPrefabs()
    {
        var dataGuids = AssetDatabase.FindAssets("t:CollectibleData");
        var dataAssets = new CollectibleData[dataGuids.Length];
        for (int i = 0; i < dataGuids.Length; i++)
            dataAssets[i] = AssetDatabase.LoadAssetAtPath<CollectibleData>(AssetDatabase.GUIDToAssetPath(dataGuids[i]));

        int assigned = 0;

        foreach (var obj in Selection.objects)
        {
            var path = AssetDatabase.GetAssetPath(obj);
            if (string.IsNullOrEmpty(path)) continue;

            var go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (go == null) continue;

            var prefabRoot = PrefabUtility.LoadPrefabContents(path);
            try
            {
                var collectible = prefabRoot.GetComponentInChildren<Collectible>();
                if (collectible == null)
                {
                    Debug.LogWarning($"Aucun composant Collectible trouvé dans prefab {go.name}");
                    continue;
                }

                CollectibleData match = Array.Find(dataAssets, d => d != null && d.name.Equals(go.name, StringComparison.OrdinalIgnoreCase));

                if (match == null)
                {
                    if (Enum.TryParse(typeof(CollectibleType), go.name, true, out var parsed))
                    {
                        var type = (CollectibleType)parsed;
                        match = Array.Find(dataAssets, d => d != null && d.type == type);
                    }
                }

                if (match != null)
                {
                    var so = new SerializedObject(collectible);
                    var prop = so.FindProperty("data");
                    if (prop != null)
                    {
                        prop.objectReferenceValue = match;
                        so.ApplyModifiedProperties();
                        PrefabUtility.SaveAsPrefabAsset(prefabRoot, path);
                        assigned++;
                        Debug.Log($"Assigné {match.name} à {go.name}");
                    }
                    else
                    {
                        Debug.LogWarning($"Propriété 'data' introuvable sur {collectible.GetType().Name} dans {go.name}");
                    }
                }
                else
                {
                    Debug.LogWarning($"Aucun CollectibleData trouvé pour prefab {go.name}");
                }
            }
            finally
            {
                PrefabUtility.UnloadPrefabContents(prefabRoot);
            }
        }

        Debug.Log($"Assignation terminée — {assigned} prefabs mis à jour.");
    }
}
#endif
