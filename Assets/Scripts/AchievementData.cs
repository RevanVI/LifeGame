using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Achievements
{
    [System.Serializable]
    public class AchievementData
    {
        public Sprite Icon;
        public string Title;
        public string Description;
        public int Score;
        public int Steps;

        public string EnumId;
        public int Id;
    }
}
