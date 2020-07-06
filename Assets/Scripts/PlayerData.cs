using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int AchievementPoints;
    public Dictionary<Achievements.EAchievements, int> AchievementsStatus;

    public PlayerData()
    {
        AchievementPoints = 0;
        AchievementsStatus = new Dictionary<Achievements.EAchievements, int>();
    }
}
