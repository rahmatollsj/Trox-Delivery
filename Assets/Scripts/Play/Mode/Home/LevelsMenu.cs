using UnityEngine;
using UnityEngine.UI;
using Harmony;

namespace Game
{
    // Author: Seyed-Rahmatoll Javadi
    public class LevelsMenu : MonoBehaviour
    {
        private HomeMenu homeMenu;
        private ProgressionManager progressionManager;

        private Button tuorialBtn;
        private Button desertBtn;
        private Button jungleBtn;
        private Button moonBtn;
        private Button arcticBtn;
        private Button volcanoBtn;

        private void Awake()
        {
            homeMenu = Finder.HomeMenu;

            tuorialBtn = transform.Find(GameObjects.Tutorial).GetComponent<Button>();
            desertBtn = transform.Find(GameObjects.Desert).GetComponent<Button>();
            jungleBtn = transform.Find(GameObjects.Jungle).GetComponent<Button>();
            moonBtn = transform.Find(GameObjects.Moon).GetComponent<Button>();
            arcticBtn = transform.Find(GameObjects.Arctic).GetComponent<Button>();
            volcanoBtn = transform.Find(GameObjects.Volcano).GetComponent<Button>();
            progressionManager = Finder.ProgressionManager;

            homeMenu.OnHomeStateChanged += OnHomeStateChanged;
        }

        private void OnEnable()
        {
            tuorialBtn.onClick.AddListener(StartTutorialLevel);
            desertBtn.onClick.AddListener(StartConstructionSiteLevel);
            jungleBtn.onClick.AddListener(StartJungleLevel);
            moonBtn.onClick.AddListener(StartMoonLevel);
            arcticBtn.onClick.AddListener(StartArcticLevel);
            volcanoBtn.onClick.AddListener(StartVolcanoLevel);
        }

        private void OnDisable()
        {
            tuorialBtn.onClick.RemoveListener(StartTutorialLevel);
            desertBtn.onClick.RemoveListener(StartConstructionSiteLevel);
            jungleBtn.onClick.RemoveListener(StartJungleLevel);
            moonBtn.onClick.RemoveListener(StartMoonLevel);
            arcticBtn.onClick.RemoveListener(StartArcticLevel);
            volcanoBtn.onClick.RemoveListener(StartVolcanoLevel);
        }

        private void OnDestroy()
        {
            homeMenu.OnHomeStateChanged -= OnHomeStateChanged;
        }

        private void OnHomeStateChanged()
        {
            gameObject.SetActive(homeMenu.State == HomeState.LevelsMenu);
            if (isActiveAndEnabled)
            {
                desertBtn.interactable = progressionManager.HasUnlockedLevel(Scenes.Desert);
                jungleBtn.interactable = progressionManager.HasUnlockedLevel(Scenes.Jungle);
                moonBtn.interactable = progressionManager.HasUnlockedLevel(Scenes.Moon);
                arcticBtn.interactable = progressionManager.HasUnlockedLevel(Scenes.Arctic);
                volcanoBtn.interactable = progressionManager.HasUnlockedLevel(Scenes.Volcan);
            }
        }
        
        //Author: Seyed-Rahmatoll Javadi
        private void StartTutorialLevel()
        {
            homeMenu.StartGame(Scenes.Tutorial);
        }

        //Author: Seyed-Rahmatoll Javadi
        private void StartConstructionSiteLevel()
        {
            homeMenu.StartGame(Scenes.Desert);
        }

        //Author: Seyed-Rahmatoll Javadi
        private void StartJungleLevel()
        {
            homeMenu.StartGame(Scenes.Jungle);
        }

        //Author: Seyed-Rahmatoll Javadi
        private void StartMoonLevel()
        {
            homeMenu.StartGame(Scenes.Moon);
        }

        //Author: Seyed-Rahmatoll Javadi
        private void StartArcticLevel()
        {
            homeMenu.StartGame(Scenes.Arctic);
        }

        //Author: Seyed-Rahmatoll Javadi
        private void StartVolcanoLevel()
        {
            homeMenu.StartGame(Scenes.Volcan);
        }
    }
}