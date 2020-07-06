using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        private Achievements AchievementsData;

        //UI

        [SerializeField]
        private GameObject _achRow;
        [SerializeField]
        private GameObject _achContent;
        //end ui

        private void Awake()
        {
            if (Instance == null)
                _instance = this;
            else
                Destroy(gameObject);
            gameObject.SetActive(false);
        }

        static void Process()
        {

        }

        private void LoadAchievements()
        {
            for (int i = 0; i < AchievementsData.achievementData.Count; ++i)
            {
                GameObject gameObject = Instantiate(_achRow, _achContent.transform);
                AchievementRowHandler achRow = gameObject.GetComponent<AchievementRowHandler>();
                achRow.SetData(AchievementsData.achievementData[i]);
            }
        }

        public void UpdateAchievements()
        {
            if (_achContent.transform.childCount == 0)
                LoadAchievements();
            else
                for (int i = 0; i < _achContent.transform.childCount; ++i)
                {
                    AchievementRowHandler achRow = _achContent.transform.GetChild(i).GetComponent<AchievementRowHandler>();
                    achRow.SetData(AchievementsData.achievementData[i]);
                }
        }

        // Start is called before the first frame update
        void Start()
        {
            gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
