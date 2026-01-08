using Codice.CM.Common;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting.YamlDotNet.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static PlasticPipe.Server.MonitorStats;

public class TowerStatImporterTool : EditorWindow
{
    private readonly string[] requiredFields = {"Tower Name", "Level", "Health", "Attack Speed", "Attack Damage", "Area Range"};

    private string statsFilePath;
    private bool csvFileValid = false;
    private string csvFileValidityText = "";
    private string csvFieldsFound = "";
    private string missingField = "";
    private string towersFoundCount = "";

    private string towerDataFolderPath;
    private int towerDataAssetsFound = 0;
    private string towerDataAssetsFoundText = "";

    private Dictionary<string, int> fieldPositions = new Dictionary<string, int>();

    [System.Serializable]
    struct TowerIdentifier
    {
        public string towerName;
        public int towerLevel;
    }
    [System.Serializable]
    struct TowerStats
    {
        public int health;
        public float attackSpeed;
        public int attackDamage;
        public float areaRange;
    }
    private Dictionary<TowerIdentifier, TowerStats> towerData = new Dictionary<TowerIdentifier, TowerStats>();

    [MenuItem("Tools/TowerStatImporter")]
    public static void ShowEditor()
    {
        EditorWindow window = GetWindow<TowerStatImporterTool>();
        window.titleContent = new GUIContent("Tower Stat Importer");
    }

    public void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        statsFilePath = EditorGUILayout.TextField("Stats File Path", statsFilePath);
        if (GUILayout.Button("Browse", GUILayout.Width(75)))
        {
            SelectStatsFile();
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Label(csvFileValidityText);

        if (csvFileValid)
        {
            GUILayout.Label(csvFieldsFound);
            GUILayout.Label(towersFoundCount);

            EditorGUILayout.BeginHorizontal();
            towerDataFolderPath = EditorGUILayout.TextField("Tower Data Folder Path", towerDataFolderPath);
            if (GUILayout.Button("Browse", GUILayout.Width(75)))
            {
                SelectTowerDataFolder();
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Label(towerDataAssetsFoundText);

            if (towerDataAssetsFound > 0)
            {
                if (GUILayout.Button($"Populate {towerDataAssetsFound} Tower Stats"))
                {
                    PopulateTowerAssetStats();
                }
            }
        }
    }

    private void SelectStatsFile()
    {
        string selectedPath = EditorUtility.OpenFilePanel("Open Stats File", "", "csv");
        if (!string.IsNullOrEmpty(selectedPath))
        {
            statsFilePath = selectedPath;
            csvFileValid = OpenStatsFile();
            if (csvFileValid)
            {
                if (ParseCsvFile())
                {
                    csvFileValid = true;
                    csvFileValidityText = $"File {statsFilePath} valid";
                }
                else
                {
                    csvFileValidityText = $"File {statsFilePath} does not contain required field {missingField}";
                }
            }
            else
            {
                csvFileValidityText = $"Unable to open file {statsFilePath}";
            }
        }
    }

    private bool OpenStatsFile()
    {
        if (File.Exists(statsFilePath))
        {
            return true;
        }

        return false;
    }

    private bool ParseCsvFile()
    {
        if (!OpenStatsFile()) { return false; }

        string contents = File.ReadAllText(statsFilePath);

        string[] lines = contents.Split('\n');

        towerData.Clear();

        csvFieldsFound = "Fields Found: |";
        for (int i = 0; i < lines.Length; i++)
        {
            string[] fields = lines[i].Split(',');
            if (i == 0)
            {
                fieldPositions.Clear();
                for (int j = 0; j < fields.Length; j++)
                {
                    csvFieldsFound += $" {fields[j]} | ";
                    fieldPositions.Add(fields[j], j);
                }
                if (!ValidateCsvFile())
                {
                    return false;
                }
            }
            else
            {
                try
                {
                    TowerIdentifier towerIdentity = new TowerIdentifier();
                    towerIdentity.towerName = fields[fieldPositions["Tower Name"]];
                    float level = 0.0f;
                    float.TryParse(fields[fieldPositions["Level"]], out level);
                    towerIdentity.towerLevel = Mathf.RoundToInt(level);

                    TowerStats towerStats = new TowerStats();
                    float health = 0.0f;
                    float.TryParse(fields[fieldPositions["Health"]], out health);
                    towerStats.health = Mathf.RoundToInt(health);

                    float attackSpeed = 0.0f;
                    float.TryParse(fields[fieldPositions["Attack Speed"]], out attackSpeed);
                    towerStats.attackSpeed = attackSpeed;

                    float attackDamage = 0.0f;
                    float.TryParse(fields[fieldPositions["Attack Damage"]], out attackDamage);
                    towerStats.attackDamage = Mathf.RoundToInt(attackDamage);

                    float areaRange = 0.0f;
                    float.TryParse(fields[fieldPositions["Area Range"]], out areaRange);
                    towerStats.areaRange = areaRange;

                    towerData.Add(towerIdentity, towerStats);
                }
                catch
                {

                }
            }
        }
        towersFoundCount = $"Tower Stats Found: {towerData.Count}";

        return true;
    }

    private bool ValidateCsvFile()
    {
        for (int i = 0; i < requiredFields.Length; i++)
        {
            if (!fieldPositions.ContainsKey(requiredFields[i]))
            {
                missingField = requiredFields[i];
                return false;
            }
        }

        return true;
    }

    private void SelectTowerDataFolder()
    {
        string selectedPath = EditorUtility.OpenFolderPanel("Open Tower Data Folder", "", "");
        if (!string.IsNullOrEmpty(selectedPath))
        {
            towerDataFolderPath = selectedPath;

            string relativePath = string.Empty;
            if (towerDataFolderPath.StartsWith(Application.dataPath))
            {
                relativePath = "Assets" + towerDataFolderPath.Substring(Application.dataPath.Length);
            }

            towerDataAssetsFound = 0;
            List<string> foundAssets = new List<string>();
            foreach (TowerIdentifier towerIdentifier in towerData.Keys)
            {
                string towerPath = $"{relativePath}/{towerIdentifier.towerName}.asset";
                TowerData dataFile = AssetDatabase.LoadAssetAtPath(towerPath, typeof(TowerData)) as TowerData;
                if (dataFile != null && !foundAssets.Contains(towerPath))
                {
                    foundAssets.Add(towerPath);
                    towerDataAssetsFound++;
                }
            }
            towerDataAssetsFoundText = $"Matching Tower Data Assets found: {towerDataAssetsFound}";
        }
    }

    private void PopulateTowerAssetStats()
    {
        string relativePath = string.Empty;
        if (towerDataFolderPath.StartsWith(Application.dataPath))
        {
            relativePath = "Assets" + towerDataFolderPath.Substring(Application.dataPath.Length);
        }

        foreach (TowerIdentifier towerIdentifier in towerData.Keys)
        {
            string towerPath = $"{relativePath}/{towerIdentifier.towerName}.asset";
            TowerData dataFile = AssetDatabase.LoadAssetAtPath(towerPath, typeof(TowerData)) as TowerData;
            if (dataFile != null)
            {
                var serializedObject = new SerializedObject(dataFile);
                EditorGUI.BeginChangeCheck();
                serializedObject.Update();

                if (dataFile.stats.Count != 3)
                {
                    dataFile.stats.Clear();
                    dataFile.stats.Add(new TowerData.LevelStats());
                    dataFile.stats.Add(new TowerData.LevelStats());
                    dataFile.stats.Add(new TowerData.LevelStats());
                }

                dataFile.stats[towerIdentifier.towerLevel - 1].health = towerData[towerIdentifier].health;
                dataFile.stats[towerIdentifier.towerLevel - 1].hitSpeed = towerData[towerIdentifier].attackSpeed;
                dataFile.stats[towerIdentifier.towerLevel - 1].damage = towerData[towerIdentifier].attackDamage;
                dataFile.stats[towerIdentifier.towerLevel - 1].areaRange = towerData[towerIdentifier].areaRange;

                if (EditorGUI.EndChangeCheck())
                {
                    // depends a bit on the context, here "myObject" was the thing we actually changed, and the custom editor code used "serializedObject" to facilitate the UI functionality
                    Undo.RecordObject(dataFile, "Editing Tower File");
                    serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(dataFile);
                }

                Debug.Log($"{towerPath} at level {towerIdentifier.towerLevel} updated!");
                EditorUtility.SetDirty(dataFile);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
