using Harmony;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    // Author: David Pagotto
    [Findable(Tags.MainController)]
    public class LevelManager : MonoBehaviour
    {
        private Scene currentLevel;
        
        public Scene CurrentLevel
        {
            get => currentLevel;
        }

        //Author: Seyed-Rahmatoll Javadi
        private IEnumerator UnloadCurrentLevelAsync()
        {
            if (currentLevel.IsValid() && currentLevel.isLoaded)
                yield return SceneManager.UnloadSceneAsync(currentLevel);
        }

        private IEnumerator LoadLevelAsync(string level)
        {
            //Author: François-Xavier Bernier
            yield return SceneManager.LoadSceneAsync(Scenes.Loading, LoadSceneMode.Additive);
            //Author: David Pagotto
            yield return UnloadCurrentLevelAsync();
            yield return SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive);
            Finder.LevelLoadedEventChannel.Publish();
            Finder.MusicManager.Play(level);
            currentLevel = SceneManager.GetSceneByName(level);
            SceneManager.SetActiveScene(currentLevel);
            //Author: François-Xavier Bernier
            yield return SceneManager.UnloadSceneAsync(Scenes.Loading);
        }

        public void LoadLevel(string level)
        {
            if (!SceneHelper.DoesLevelExist(level))
                throw new Exception("Can't load non-existing level !");
            StartCoroutine(LoadLevelAsync(level));
        }
        
        //Author: Seyed-Rahmatoll Javadi
        public void UnloadCurrentLevel()
        {
            StartCoroutine(UnloadCurrentLevelAsync());
        }

        public void RestartCurrentLevel()
        {
            LoadLevel(currentLevel.name);
        }

        public string GetNextLevel()
        {
            switch (currentLevel.name)
            {
                case Scenes.Tutorial:
                    return Scenes.Desert;
                case Scenes.Desert:
                    return Scenes.Jungle;
                case Scenes.Jungle:
                    return Scenes.Moon;
                case Scenes.Moon:
                    return Scenes.Arctic;
                case Scenes.Arctic:
                    return Scenes.Volcan;
                default:
                    return Scenes.Tutorial;
            }
        }

        // Author: François-Xavier Bernier & David Pagotto (mise à jour code pour bug fix)
        private IEnumerator LoadNextSceneAsync()
        {
            var nextLevel = GetNextLevel();
            yield return UnloadCurrentLevelAsync();
            yield return LoadLevelAsync(nextLevel);
        }

        // Author: François-Xavier Bernier
        public void LoadNextScene()
        {
            StartCoroutine(LoadNextSceneAsync());
        }
    }
}