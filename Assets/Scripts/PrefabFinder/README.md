# 🧰 Prefab Finder Editor Window for Unity

A powerful and user-friendly Unity Editor Window that helps you **search for prefabs** within a specified folder path. This tool allows filtering by:

* Specific **component types**
* Matching **field names**
* Matching **field values**

---

## ✨ Key Features

* 🔍 Search all prefabs within a chosen folder (e.g., `Assets/...`)
* 🎯 Filter by specific component type (e.g., `HealthComponent`)
* ⚙️ Optional advanced field filter: match a field name and its value
* 📋 Displays matching prefabs in a scrollable, readable list
* 🧼 Clean, intuitive, and organized user interface

---

## 📦 Installation Instructions

1. Place the `PrefabFinderWindow.cs` script in your Unity project under an `Editor` folder:

   ```
   Assets/Editor/PrefabFinderWindow.cs
   ```

2. Open the editor window via the Unity menu:

   ```
   Tools → Prefab Finder
   ```

---

## 🖥️ Usage Guide

### 🔍 Basic Search

* **Folder Path**: Specify the folder to search (e.g., `Assets/Prefabs`).
* **Component Name**: Enter the name of the component script to filter (e.g., `HealthComponent`).

### ⚙️ Advanced Field Filtering (Optional)

* Expand the **Advanced Field Filter** section.
* **Field Name**: Specify the name of the field you want to match (e.g., `maxHealth`).
* **Expected Value**: Provide the expected field value as a string (e.g., `100`).

> Note: Matching is **case-insensitive**, and field values are compared using `.ToString()`.

---

## 📊 Example Scenarios

| Folder Path      | Component Name | Field Name  | Field Value | Matches If...                                |
| ---------------- | -------------- | ----------- | ----------- | -------------------------------------------- |
| `Assets/Prefabs` | `EnemyAI`      | —           | —           | Prefab has `EnemyAI` component               |
| `Assets/Units`   | `Health`       | `maxHealth` | `100`       | `Health.maxHealth == 100`                    |
| `Assets/Props`   | `LootItem`     | `rarity`    | `Legendary` | `LootItem.rarity == "Legendary"` (as string) |

---

## ⚠️ Known Limitations

* Field values are compared as **strings** only (e.g., "true", "10", "Boss").
* Matching is performed using **reflection**, so both private and public fields are supported. However, **properties are currently not supported**.
* Component name must exactly match the **type name**, not the file name.

---

## 🚀 Planned Improvements

* [ ] Support for comparison operators (e.g., `>`, `<`, `==`)
* [ ] Dropdown for selecting available component types
* [ ] Auto-complete for fields based on component selection
* [ ] Support for property (getter) matching
* [ ] Ability to apply multiple filters simultaneously

---

## 🛠 Technical Details

* Utilizes `AssetDatabase.FindAssets("t:Prefab")` to locate prefabs.
* Uses `GameObject.GetComponentsInChildren<Component>(true)` to account for nested components.
* Reflection is used for field matching:

  ```csharp
  field = type.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
  ```

---

## 👨‍💻 Author

Crafted with Unity Editor scripting and reflection for efficient asset management workflows in any Unity project.
