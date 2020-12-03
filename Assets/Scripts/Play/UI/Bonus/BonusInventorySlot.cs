using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    // Author: David Pagotto
    public class BonusInventorySlot : MonoBehaviour
    {
        private Image image = null;
        public Sprite Icon 
        { 
            get => image.sprite;
            set { if(image) image.sprite = value; }
        }

        public bool HasItem
        {
            get => image?.enabled ?? false;
            set { if (image) image.enabled = value; }
        }

        private void Awake()
        {
            image = transform.Find(GameObjects.BonusIcon).GetComponent<Image>();
        }
    }
}
