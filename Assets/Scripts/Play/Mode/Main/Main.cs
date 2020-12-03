using System;
using System.Collections;
using Harmony;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    //Author: Seyed-Rahmatoll Javadi
    [Findable(Tags.MainController)]
    public class Main : MonoBehaviour
    {
        private IEnumerator Start()
        {
            yield return SceneManager.LoadSceneAsync(Scenes.Home, LoadSceneMode.Additive);
        }

        public void LoadHome()
        {
            IEnumerator Routine()
            {
                yield return SceneManager.LoadSceneAsync(Scenes.Home, LoadSceneMode.Additive);
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(Scenes.Home));
            }
            StartCoroutine(Routine());
        }

        public void UnloadHome()
        {
            IEnumerator Routine()
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(Scenes.Main));
                yield return SceneManager.UnloadSceneAsync(Scenes.Home);
            }

            if (SceneManager.GetSceneByName(Scenes.Home).isLoaded)
                StartCoroutine(Routine());
        }

        private IEnumerator LoadGameAsync(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(Scenes.Game, LoadSceneMode.Additive);
            yield return SceneManager.LoadSceneAsync(Scenes.UI, LoadSceneMode.Additive);

            Finder.LevelManager.LoadLevel(sceneName);
        }

        public void LoadGame(string sceneName)
        {
            StartCoroutine(LoadGameAsync(sceneName));
        }

        public void UnloadGame()
        {
            IEnumerator Routine()
            {
                if (SceneManager.GetSceneByName(Scenes.Game).IsValid())
                    yield return SceneManager.UnloadSceneAsync(Scenes.Game);
                if (SceneManager.GetSceneByName(Scenes.UI).IsValid())
                    yield return SceneManager.UnloadSceneAsync(Scenes.UI);
            }

            StartCoroutine(Routine());
        }

#if UNITY_EDITOR
        // Author: David Pagotto
        // Utilisé par le script éditeur "CheatMenu"
        public event Action MainControllerOnGUI;
        private void OnGUI()
            => MainControllerOnGUI?.Invoke();
#endif
    }
}