using UnityEngine;
using UnityEngine.SceneManagement;
using Harmony;
using UnityEditor.SceneManagement;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;

namespace Game
{
    // Author: David Pagotto
    // https://docs.unity3d.com/ScriptReference/RuntimeInitializeOnLoadMethodAttribute-ctor.html
	// Permet en mode editeur de charger les scenes necessaires pour pouvoir tester un niveau rapidement
    public class Startup
    {
        private static Scene targetScene;
        private static GameObject[] disabledObjects;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoadRuntimeMethod()
        {
            if (SceneManager.sceneCount != 1)
                return;
            if (!SceneManager.GetSceneAt(0).path.Contains("Levels"))
                return;
            targetScene = EditorSceneManager.GetSceneAt(0);
            disabledObjects = GameObject.FindObjectsOfType<GameObject>();
            foreach (var obj in disabledObjects.Where(o => o.activeSelf))
                obj.SetActive(false);
            SceneManager.LoadScene(Scenes.Main, LoadSceneMode.Additive);
            SceneManager.sceneLoaded += OnSceneLoaded;

        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
        {
            if (scene.name == Scenes.Home)
                SceneManager.UnloadSceneAsync(scene);
            else if (scene.name == Scenes.Main)
            {
                SceneManager.SetActiveScene(scene);
                Finder.Main.LoadGame(targetScene.name);
                typeof(LevelManager).GetField("currentLevel", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(Finder.LevelManager, targetScene);
            } 
            else if (scene.name == Scenes.Loading)
            {
                SceneManager.UnloadSceneAsync(scene);
            } 
            else if (scene.name == Scenes.Game)
            {
               Task.Run(() =>
               {
                   Task.Delay(1000);
                   foreach (var obj in disabledObjects)
                       obj.SetActive(true);
               });

            }
        }
    }

}