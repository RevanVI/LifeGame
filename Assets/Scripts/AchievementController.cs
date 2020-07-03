using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Achievements
{
    public class AchievementController : MonoBehaviour
    {
        static AchievementController Instance;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

        }

        static void Process()
        {

        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
