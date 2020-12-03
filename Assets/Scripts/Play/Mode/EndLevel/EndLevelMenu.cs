using Harmony;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Game
{
    public class EndLevelMenu : MonoBehaviour
    {
        
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text endGameText;
        [SerializeField] private GameObject statsPanel;
        [SerializeField] private TMP_Text airTimeText;
        [SerializeField] private TMP_Text timeLeftText;
        [SerializeField] private TMP_Text boxesText;

        //Author : Félix Bernier
        [SerializeField] private string scoreFormat = "00000";
        [SerializeField] private string score = "SCORE : ";
        [SerializeField] private string airTimeMessageConst = "Air time : ";
        [SerializeField] private string timeLeftMessageConst = "Time left : ";
        [SerializeField] private string boxesLeftMessageConst = "Boxes left : x";
        [SerializeField] private string winMessageConst = "You arrived on time for \n";
        [SerializeField] private string loseMessageConst = "You arrived late for \n";

        [SerializeField] private string points = "pts";
        
        [SerializeField] private string[] messages =
        {
            "the birth of your 9th child!",
            "your important family diner",
            "your lawn bowls class!",
            "your 4th daughter's prom!",
            "the meeting of alcoholics anonymous!",
            "your 5th child's talent show!",
            "your wedding!",
            "the triplets birthday!",
            "the Christmas party!",
            "your flight to Cuba!",
            "dentist's appointment!",
            "your apprentice pilot course!",
            "your meeting with the prime minister!",
            "your sons' cricket game!",
            "your online dance show!",
            "your majorette lesson!",
            "your oil change!",
            "Thomas soccer game!",
            "the kindergarten music show!",
            "supper!",
            "thursday's special at IGA on bologna",
        };

        // Author François-Xavier Bernier 
        private GameBox gameBox;
        private Inputs inputs;
        private Button buttonNextLvl;
        private ScoreCalculator scoreCalculator;
        private TimeManagement timeManagement;
        
        private LevelSuccessEventChannel levelSuccessEventChannel;

        // Author: Seyed-Rahmatoll Javadi
        private LevelFailedEventChannel levelFailedEventChannel;

        // Author: David Pagotto
        private InputAction pauseInputAction; 

        private void Awake()
        {
            inputs = Finder.Inputs;
            var buttons = GetComponentsInChildren<Button>();
            buttonNextLvl = buttons.WithName(GameObjects.NextLvlBtn);
            // Author: Félix Bernier
            levelSuccessEventChannel = Finder.LevelSuccessEventChannel;
            levelSuccessEventChannel.OnLevelSuccess += OnLevelCompleted;
            buttonNextLvl.interactable = true;
            levelFailedEventChannel = Finder.LevelFailedEventChannel;
            levelFailedEventChannel.OnLevelFailedEventChannel += OnLevelFailed;

            gameBox = Finder.GameBox;
            scoreCalculator = Finder.ScoreCalculator;
            timeManagement = Finder.TimeManagement;

            // Author: David Pagotto
            pauseInputAction = Finder.Inputs.Game.Exit;
        }

        private void OnDestroy()
        {
            // Author: Félix Bernier
            levelSuccessEventChannel.OnLevelSuccess -= OnLevelCompleted;
            levelFailedEventChannel.OnLevelFailedEventChannel -= OnLevelFailed;
        }

        private void OnEnable()
        {
            pauseInputAction.Disable();
        }

        private void OnDisable()
        {
            pauseInputAction.Enable();
        }

        private void Start()
        {
            // Author: Félix Bernier
            gameObject.SetActive(timeManagement.GameIsPaused);
        }
        
        private void Update()
        {  
            //Author François-Xavier Bernier 
            gameObject.SetActive(timeManagement.GameIsPaused);
        }

        // Author: David Pagotto
        private void ToggleNextLevelBtn(bool enabled)
        {
            buttonNextLvl.interactable = enabled;
            buttonNextLvl.gameObject.SetActive(enabled);
        }

        // Author: Félix Bernier
        private void OnLevelFailed()
        {
            gameObject.SetActive(true);
            ToggleNextLevelBtn(false);
            DrawEndLevelText(false);
            scoreText.alpha = 0f;
            statsPanel.SetActive(false);
            Finder.Vehicle.enabled = false;
        }

        //Author François-Xavier Bernier 
        public void NextLevel()
        {
            Finder.LevelManager.LoadNextScene();
            timeManagement.UnfreezeGame();
            gameObject.SetActive(false);
        }

        private void OnLevelCompleted()
        {
            gameObject.SetActive(true);
            ToggleNextLevelBtn(true);
            //Author François-Xavier Bernier
            if (Finder.LevelSpecs.IsLastLevelOfTheGame)
            {
                  buttonNextLvl.interactable = false;
                  buttonNextLvl.gameObject.SetActive(false);
            }
            DrawEndLevelText(true);
            //Author Félix Bernier
            UpdateStats();
            scoreText.text = score + scoreCalculator.CalculateScore(scoreCalculator.Score, gameBox.BoxCount).ToString(scoreFormat);
            scoreText.alpha = 1f;
            statsPanel.SetActive(true);
        }
        
        // Author: Félix Bernier
        private void UpdateStats()
        {
            airTimeText.text = airTimeMessageConst + scoreCalculator.Score + points;
            timeLeftText.text = timeLeftMessageConst + (int)Finder.Vehicle.CurrentFuel;
            boxesText.text = boxesLeftMessageConst + gameBox.BoxCount;
        }

        private void DrawEndLevelText(bool gameWon)
        {
            //Author François-Xavier Bernier
            if (gameWon && gameBox.BoxCount >= Finder.LevelSpecs.NbBoxNeededToPassLevel)
                endGameText.text = winMessageConst + messages.Random();
            else
                endGameText.text = loseMessageConst + messages.Random();
        }
    }
}