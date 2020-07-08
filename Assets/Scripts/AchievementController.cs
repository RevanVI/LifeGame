using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Achievements
{
    public class AchievementController : MonoBehaviour
    {
        private static AchievementController _instance;
        public static AchievementController Instance
        {
            get
            {
                return _instance;
            }
        }

        [SerializeField]
        private Achievements _achievementsData;

        //UI

        [SerializeField]
        private GameObject _achRow;
        [SerializeField]
        private GameObject _achContent;
        [SerializeField]
        private Text _pointsText;
        //end ui

        private void Awake()
        {
            if (Instance == null)
                _instance = this;
            else
                Destroy(gameObject);  
        }

        private void ProcessAchievement(EAchievements ach)
        {
            int achNum = (int)ach;
            if (PlayerDataController.Instance.Data.AchievementsStatus[ach] == _achievementsData.achievementData[achNum].Steps)
                return;
            PlayerDataController.Instance.Data.AchievementsStatus[ach] += 1;
            if (PlayerDataController.Instance.Data.AchievementsStatus[ach] == _achievementsData.achievementData[achNum].Steps)
                PlayerDataController.Instance.Data.AchievementPoints += _achievementsData.achievementData[achNum].Score;
            PlayerDataController.Instance.WriteData();
        }

        void ProcessSpawn()
        {
            ProcessAchievement(EAchievements.CreateLife5);
        }

        void ProcessDie()
        {
            ProcessAchievement(EAchievements.Die10);
        }

        public void UpdateAchievements()
        {
            if (_achContent.transform.childCount == 0)
            {
                for (int i = 0; i < _achievementsData.achievementData.Count; ++i)
                {
                    GameObject gameObject = Instantiate(_achRow, _achContent.transform);
                }
            }
            for (int i = 0; i < _achContent.transform.childCount; ++i)
            {
                AchievementRowHandler achRow = _achContent.transform.GetChild(i).GetComponent<AchievementRowHandler>();
                achRow.SetData(_achievementsData.achievementData[i]);
                int currentCount = PlayerDataController.Instance.Data.AchievementsStatus[(EAchievements)i];
                achRow.Steps = (currentCount, _achievementsData.achievementData[i].Steps);
            }
            _pointsText.text = PlayerDataController.Instance.Data.AchievementPoints.ToString();
        }

        public void InitializePlayerData(PlayerData data)
        {
            if (data.AchievementsStatus.Count > 0)
                return;
            for (int i = 0; i < _achievementsData.achievementData.Count; ++i)
            {
                data.AchievementsStatus.Add((EAchievements)i, 0);
            }
        }

        void Start()
        {
            LifeDot.OnDie.AddListener(ProcessDie);
            GameController.Instance.OnSpawn.AddListener(ProcessSpawn);
            gameObject.SetActive(false);
        }

        void Update()
        {

        }
    }
}
