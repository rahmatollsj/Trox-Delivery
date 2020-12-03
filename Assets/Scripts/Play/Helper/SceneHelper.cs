using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    // Author: David Pagotto
    public class SceneHelper
    {
        public static string[] ScenesPaths { get => scenesPaths; }

        private const string levelsFolder = "Levels";
        private static string[] scenesPaths;

        public static bool DoesLevelExist(string level)
        {
            return ScenesPaths.Any(s => s.Contains(level));
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnStartup() {
            // Obtient toutes les scènes disponibles dans les build.
            var nScenes = SceneManager.sceneCountInBuildSettings;
            scenesPaths = new string[nScenes];
            for (int i = 0; i < nScenes; i++)
                scenesPaths[i] = SceneUtility.GetScenePathByBuildIndex(i);
        }
    }
}
