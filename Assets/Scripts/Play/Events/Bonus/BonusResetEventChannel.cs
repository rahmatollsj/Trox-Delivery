using Harmony;
using UnityEngine;

namespace Game
{
    //Author: Seyed-Rahmatoll Javadi
    [Findable(Tags.MainController)]
    public class BonusResetEventChannel : MonoBehaviour
    {
        public event BonusResetEvent OnBonusReset;

        public void Publish()
        {
            if (OnBonusReset != null)
                OnBonusReset();
        }
    }
    
    public delegate void BonusResetEvent();
}