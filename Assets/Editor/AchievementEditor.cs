using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(Achievements.Achievements))]
public class AchievementEditor : Editor
{
    private Achievements.Achievements data;

    private void OnEnable()
    {
        data = target as Achievements.Achievements;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Generate enum"))
        {
            GenerateEnum();
        }
    }

    private void GenerateEnum()
    {
        string filePath = Path.Combine(Application.dataPath, "Scripts/AchievementEnum.cs");
        string code = "namespace Achievements\n{\n\tpublic enum EAchievements\n\t{\n\t\t";
        for (int i = 0; i < data.achievementData.Count; ++i)
        {
            var ach = data.achievementData[i];
            code += ach.EnumId + ",\n\t";
            if (i != data.achievementData.Count - 1)
                code += "\t";
        }
        code += "}\n}";
        File.WriteAllText(filePath, code);
        AssetDatabase.ImportAsset("Assets/Scripts/AchievementEnum.cs");
    }
}
