using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Achievements
{
    [CreateAssetMenu(fileName = "Achiements", menuName = "ScriptableObjects/Achievements")]
    public class Achievements : ScriptableObject
    {
        [SerializeField]
        public List<AchievementData> achievementData;

    }
}
