using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
    }
}
