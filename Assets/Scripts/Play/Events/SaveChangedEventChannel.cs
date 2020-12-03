using Harmony;
using UnityEngine;

namespace Game
{
    // Author: David Pagotto
    [Findable(Tags.MainController)]
    public class SaveChangedEventChannel : MonoBehaviour
    {
        public delegate void SaveChangedEvent();
        public event SaveChangedEvent OnSaveChanged;

        public void Publish()
            => OnSaveChanged?.Invoke();
    }
}
