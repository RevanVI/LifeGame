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

        private void ProcessAchievement(EAchievements ach, int val = -1)
        {
            int achNum = (int)ach;
            //already complete
            if (PlayerDataController.Instance.Data.AchievementsStatus[ach] == _achievementsData.achievementData[achNum].Steps)
                return;
            
            if (val == -1)
            {
                PlayerDataController.Instance.Data.AchievementsStatus[ach] += 1;
            }
            //if val != -1 then achievement track the greatest value
            else if (PlayerDataController.Instance.Data.AchievementsStatus[ach] < val)
            {
                PlayerDataController.Instance.Data.AchievementsStatus[ach] = (val < _achievementsData.achievementData[achNum].Steps) ? val : _achievementsData.achievementData[achNum].Steps;
            }
            if (PlayerDataController.Instance.Data.AchievementsStatus[ach] == _achievementsData.achievementData[achNum].Steps)
                PlayerDataController.Instance.Data.AchievementPoints += _achievementsData.achievementData[achNum].Score;
            PlayerDataController.Instance.WriteData();
        }

        void ProcessSpawn()
        {
            ProcessAchievement(EAchievements.Create10);
            ProcessAchievement(EAchievements.Create25);
            ProcessAchievement(EAchievements.Create50);
            ProcessAchievement(EAchievements.Create100);
        }

        void ProcessDie()
        {
            ProcessAchievement(EAchievements.Die10);
            ProcessAchievement(EAchievements.Die25);
            ProcessAchievement(EAchievements.Die50);
            ProcessAchievement(EAchievements.Die100);
        }

        void ProcessGrow(int age)
        {
            ProcessAchievement(EAchievements.Age10, age);
            ProcessAchievement(EAchievements.Age20, age);
            ProcessAchievement(EAchievements.Age50, age);
            ProcessAchievement(EAchievements.Age100, age);
        }

        void ProcessRemove()
        {
            ProcessAchievement(EAchievements.Remove10);
            ProcessAchievement(EAchievements.Remove25);
            ProcessAchievement(EAchievements.Remove50);
            ProcessAchievement(EAchievements.Remove100);
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
            LifeDot.OnGrow.AddListener(ProcessGrow);
            GameController.Instance.OnSpawn.AddListener(ProcessSpawn);
            GameController.Instance.OnRemove.AddListener(ProcessRemove);
            gameObject.SetActive(false);
        }

        void Update()
        {

        }
    }
}
