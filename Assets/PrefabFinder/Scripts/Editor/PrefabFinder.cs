using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Reflection;

public class PrefabFinder : EditorWindow
{
	private string folderPath = "Assets/Prefabs";
	private string componentFilterName = "";
	private string fieldFilterName = "";
	private string fieldFilterValue = "";

	private bool showAdvancedFilters = false;
	private Vector2 scrollPos;
    private List<GameObject> foundPrefabs = new List<GameObject>();

	[MenuItem("Tools/Prefab Finder")]
	public static void ShowWindow()
	{
		GetWindow<PrefabFinder>("Prefab Finder");
	}

	private void OnGUI()
	{
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Prefab Finder", EditorStyles.boldLabel);
		EditorGUILayout.HelpBox("Search for prefabs under a folder with optional component and field-based filters.", MessageType.Info);
		EditorGUILayout.Space();

		DrawSectionHeader("Search Location");
		folderPath = EditorGUILayout.TextField("Folder Path", folderPath);

		EditorGUILayout.Space();
		DrawSectionHeader("Component Filter");
		componentFilterName = EditorGUILayout.TextField("Component Name", componentFilterName);

		showAdvancedFilters = EditorGUILayout.Foldout(showAdvancedFilters, "Advanced Field Filter");

		if (showAdvancedFilters)
		{
			EditorGUI.indentLevel++;
			fieldFilterName = EditorGUILayout.TextField("Field Name", fieldFilterName);
			fieldFilterValue = EditorGUILayout.TextField("Expected Value", fieldFilterValue);
			EditorGUI.indentLevel--;
		}

		EditorGUILayout.Space();

		if (GUILayout.Button("Find Prefabs", GUILayout.Height(30)))
		{
			FindPrefabsAtPath(folderPath, componentFilterName, fieldFilterName, fieldFilterValue);
		}

		EditorGUILayout.Space();
		EditorGUILayout.LabelField($"Found {foundPrefabs.Count} prefab(s)", EditorStyles.helpBox);

		scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(300));

        foreach (GameObject prefab in foundPrefabs)
		{
			if (prefab != null)
            {
                EditorGUILayout.ObjectField(prefab.name, prefab, typeof(GameObject), false);
            }
        }

		EditorGUILayout.EndScrollView();
	}

	private void DrawSectionHeader(string title)
	{
		GUILayout.Space(5);
		EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
		GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
	}

	private void FindPrefabsAtPath(string path, string componentName, string fieldName, string expectedValue)
	{
		foundPrefabs.Clear();

		if (!path.StartsWith("Assets"))
		{
			Debug.LogError("Path must start with 'Assets/'.");
			return;
		}

		string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { path });

		foreach (string guid in guids)
		{
			string assetPath = AssetDatabase.GUIDToAssetPath(guid);
			GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

            if (prefab == null)
            {
                continue;
            }

            Component targetComponent = FindComponentByName(prefab, componentName);

			if (targetComponent == null)
            {
                continue;
            }

            if (!string.IsNullOrEmpty(fieldName))
			{
				if (!FieldMatches(targetComponent, fieldName, expectedValue))
                {
                    continue;
                }
            }

			foundPrefabs.Add(prefab);
		}
	}

	private Component FindComponentByName(GameObject prefab, string componentName)
	{
		Component[] components = prefab.GetComponentsInChildren<Component>(true);

		foreach (Component comp in components)
		{
			if (comp != null && comp.GetType().Name.Equals(componentName, StringComparison.OrdinalIgnoreCase))
            {
                return comp;
            }
        }
		return null;
	}

	private bool FieldMatches(Component component, string fieldName, string expectedValue)
	{
		Type type = component.GetType();
		FieldInfo field = type.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

		if (field == null)
		{
			Debug.LogWarning($"Field '{fieldName}' not found in {type.Name}");
			return false;
		}

		object value = field.GetValue(component);

		if (value == null)
		{
			return false;
		}

        return value.ToString().Equals(expectedValue, StringComparison.OrdinalIgnoreCase);
	}
}