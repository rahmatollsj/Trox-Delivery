using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    //Author :  François-Xavier Bernier
    public class VolcanicWave : MonoBehaviour
    {
        private List<VolcanicRock> volcanicRocks;

        private void Awake()
        {
            volcanicRocks = new List<VolcanicRock>();
        }

        private void Start()
        {
            VolcanicRock[] volRocArray = gameObject.GetComponentsInChildren<VolcanicRock>();
            if (volRocArray == null)
                return;

            foreach (VolcanicRock rockChild in volRocArray)
            {
                if (rockChild != null && transform.gameObject != null)
                    volcanicRocks.Add(rockChild);
            }
        }

        public void ActiveWave()
        {
            foreach (VolcanicRock rockChild in volcanicRocks)
            {
                if (rockChild != null)
                    rockChild.ActivateRock();
            }
        }
    }
}