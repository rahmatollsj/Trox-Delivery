using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    //Author : François-Xavier Bernier
    public class EngineStateSign : MonoBehaviour
    {
        [SerializeField] private float fadeInTime = 0.3f;
        [SerializeField] private float fadeOutTime = 0.5f;
        [SerializeField] private int nbFlashPossible = 3;

        private Image signImg;
        private int nbFlashed;

        private void Awake()
        {
            gameObject.SetActive(false);
            nbFlashPossible = 3;
            nbFlashed = 0;
        }

        private void Start()
        {
            signImg = gameObject.GetComponent<Image>();
            signImg.canvasRenderer.SetAlpha(1f);
        }

        private void LateUpdate()
        {
            if (nbFlashed < nbFlashPossible)
            {
                if (signImg.canvasRenderer.GetAlpha() >= 0.9f)
                {
                    FadeOut();
                }
                else if (signImg.canvasRenderer.GetAlpha() <= 0.1f)
                {
                    FadeIn();
                    nbFlashed++;
                }
            }
            else
            {
                signImg.gameObject.SetActive(false);
            }
        }

        private void FadeOut()
        {
            signImg.CrossFadeAlpha(0, fadeOutTime, false);
        }

        private void FadeIn()
        {
            signImg.CrossFadeAlpha(1, fadeInTime, false);
        }

        public void ResetNbFlashed()
        {
            nbFlashed = 0;
        }
    }
}