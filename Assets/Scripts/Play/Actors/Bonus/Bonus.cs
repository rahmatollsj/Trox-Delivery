using Harmony;
using UnityEngine;

namespace Game
{
    // Author: David Pagotto
    [RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
    public abstract class Bonus : MonoBehaviour
    {
        [SerializeField] private float upDownAnimationPeriod = 2.5f;
        [SerializeField] private float upDownAnimationAmplitude = 0.2f;
        [SerializeField] private SoundEffectType soundOnCollect = SoundEffectType.Bonus;

        public abstract BonusType BonusType { get; }
        public abstract IBonusData Data { get; }
        private bool IsBonusActivated { get; set; }
        protected Vehicle Target { get; private set; }

        protected virtual void InitializeBonus() { }
        protected abstract void OnBonusActivated();
        protected virtual void OnUpdate() { }
        protected virtual void DestroyBonus() { }
        public abstract void UpdateData(IBonusData data);

        private Vector3 originalPosition;
        //Author: Seyed-Rahmatoll Javadi
        private SoundEffectsManager soundEffectsManager;

        protected Bonus() { }

        private void Awake()
        {
            
            InitializeBonus();
            IsBonusActivated = false;
            Target = null;
            Finder.BonusCollectedEventChannel.OnBonusCollected += OnBonusCollected;
            originalPosition = transform.position;
            
            //Author: Seyed-Rahmatoll Javadi
            soundEffectsManager = Finder.SoundEffectsManager;
        }

        private void OnDestroy()
        {
            if (Finder.BonusCollectedEventChannel != null)
            {
                Finder.BonusCollectedEventChannel.OnBonusCollected -= OnBonusCollected;
                Finder.BonusFinishedEventChannel.Publish(this);
            }
            DestroyBonus();
            Destroy(gameObject);
        }

        /// <summary>
        /// Indique que le bonus a été récolté ( le rend invisible sur la scène du niveau )
        /// </summary>
        protected void MarkAsCollected()
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }

        private void OnBonusCollected(Bonus bonus)
        {
            if (bonus == this)
            {
                //Author: Seyed-Rahmatoll Javdi
                soundEffectsManager.Play(soundOnCollect);
                
                MarkAsCollected();
                Destroy(this);
            }
        }
        private void Update()
        {
            if (IsBonusActivated)
                OnUpdate();
            else
                transform.position = originalPosition + Vector3.up * (upDownAnimationAmplitude * Mathf.Sin(Time.time * upDownAnimationPeriod));
        }

        public void TriggerBonus(Vehicle vehicle)
        {
            if (!IsBonusActivated)
            {
                IsBonusActivated = true;
                Target = vehicle;
                Finder.BonusActivatedEventChannel.Publish(this);
                OnBonusActivated();
            }
#if UNITY_EDITOR
            else
                Debug.LogWarning("Unexpected action: Attempting to trigger already active bonus!");
#endif
        }

        public static GameObject GetBonusPrefab(BonusType type)
        {
            switch (type)
            {
                case BonusType.Nitro:
                    return Prefabs.NitroBonus;
                case BonusType.Magnet:
                    return Prefabs.MagnetBonus;
                case BonusType.Fuel:
                    return Prefabs.FuelBonus;
                case BonusType.Repair:
                    return Prefabs.RepairBonus;
                default:
                    throw new System.Exception("Unsupported BonusType");
            }
        }
    }
}
