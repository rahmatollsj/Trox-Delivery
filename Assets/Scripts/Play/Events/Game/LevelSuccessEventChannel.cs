using Harmony;
using UnityEngine;

namespace Game
{
    //Author: Félix Bernier
    [Findable(Tags.MainController)]
    public class LevelSuccessEventChannel : MonoBehaviour
    {
        public event LevelSuccessEvent OnLevelSuccess;

        public void Publish()
        {
            if (OnLevelSuccess != null)
                OnLevelSuccess();
        }
        public delegate void LevelSuccessEvent();
    }
}