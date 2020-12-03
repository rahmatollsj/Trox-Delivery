using System.Collections;
using Harmony;
using TMPro;
using UnityEngine;

namespace Game
{
    //Author: Félix Bernier
    public class Achievement : MonoBehaviour
    {
        //Author: Félix Bernier
        [SerializeField] private TMP_Text achievementText;
        [SerializeField] private int smoothness;
        [SerializeField] private float travelDistance = 100f;
        private Vector2 targetPosition;
        private Vector2 initialPosition;
        private AchievementManager achievementManager;
        // Author: Benoit Simon-Turgeon
        public bool IsDone { get; private set; } = false;

        //Author: Félix Bernier
        private void Awake()
        {
            initialPosition = transform.position;
            targetPosition = new Vector2(initialPosition.x, initialPosition.y - travelDistance);
            achievementManager = Finder.AchievementManager;
        }

        //Author: Félix Bernier
        private void Start()
        {
            achievementText.text = achievementManager.LastAchievementType + "!";
            StartCoroutine(AchievementMove());
        }

        //Author: Félix Bernier
        private IEnumerator MoveBanner(Vector2 bannerTargetPosition)
        {
            yield return new WaitUntil(() =>
            {
                transform.position = Vector2.Lerp(transform.position, bannerTargetPosition, smoothness * Time.unscaledDeltaTime);
                return ((Vector2)transform.position).DistanceTo(bannerTargetPosition) < 0.1f;
            });
        }
        
        //Author: Félix Bernier
        private IEnumerator AchievementMove()
        {
            yield return MoveBanner(targetPosition);
            yield return new WaitForSecondsRealtime(3f);
            yield return MoveBanner(initialPosition);
            IsDone = true;
        }
    }
}