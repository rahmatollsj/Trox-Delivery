using Harmony;
using UnityEngine;

namespace Game
    {
        // Author: Benoit Simon-Turgeon
        [Findable(Tags.MainController)]
        public class HighJumpSuccessfulEventChannel : MonoBehaviour
        {
            public event HighJumpSuccessful OnHighJumpSuccessful;

            public void Publish()
            {
                OnHighJumpSuccessful?.Invoke();
            }

            public delegate void HighJumpSuccessful();
        }
    }