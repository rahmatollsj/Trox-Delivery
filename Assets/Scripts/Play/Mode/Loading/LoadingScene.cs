using System.Collections;
using TMPro;
using UnityEngine;

namespace Game
{
    //Author : François-Xavier Bernier
    public class LoadingScene : MonoBehaviour
    {
        [SerializeField] private float timeBetweenTextWaves = 0.05f;
        
        private const string FirstText = "Loading .  ";
        private const string SecondText = "Loading .. ";
        private const string ThirdText = "Loading ...";
        private TextMeshProUGUI titleLoading;

        private void Awake()
        {
            titleLoading = GetComponentInChildren<TextMeshProUGUI>();
            StartCoroutine(LoadingTextFade());
        }

        private IEnumerator LoadingTextFade()
        {   
            yield return  new WaitForSeconds(timeBetweenTextWaves);
            titleLoading.text = SecondText;
            yield return  new WaitForSeconds(timeBetweenTextWaves);
            titleLoading.text = ThirdText;
            yield return  new WaitForSeconds(timeBetweenTextWaves);
            titleLoading.text = FirstText;
        }

    }
}