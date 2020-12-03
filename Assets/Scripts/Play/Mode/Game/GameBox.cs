using Harmony;
using TMPro;
using UnityEngine;

namespace Game
{
    [Findable(Tags.HeadUpDisplay)]
    public class GameBox : MonoBehaviour
    {
        //Author François-Xavier Bernier
        [SerializeField]private TextMeshProUGUI textNbBox;

        private BoxAddedInTruckBedEventChannel boxAddedInTruckBedEventChannel;
        private BoxOutOfTruckBedEventChannel boxOutOfTruckBedEventChannel;
        private LevelFailedEventChannel levelFailedEventChannel;
        private LevelLoadedEventChannel levelLoadedEventChannel;

        //Author : François-Xavier Bernier
        public int BoxCount { get; private set; }

        private LevelSpecs currentLevelSpecs;

        private void Awake()
        {
            //Author François-Xavier Bernier
            levelLoadedEventChannel = Finder.LevelLoadedEventChannel;
            levelLoadedEventChannel.OnLevelLoaded += OnLevelSpecsLoaded;
            ResetData();
        }

        //Author: Seyed-Rahmatoll Javadi
        private void OnDisable()
        {
            //Author François-Xavier Bernier
            boxAddedInTruckBedEventChannel.OnBoxAdded -= OnAddBox;
            boxOutOfTruckBedEventChannel.OnBoxOut -= OnBoxRemove;
            levelFailedEventChannel.OnLevelFailedEventChannel -= ResetData;
        }

        private void OnLevelSpecsLoaded()
        {
            currentLevelSpecs = Finder.LevelSpecs;
            boxAddedInTruckBedEventChannel = Finder.BoxAddedInTruckBedEventChannel;
            boxAddedInTruckBedEventChannel.OnBoxAdded += OnAddBox;
            boxOutOfTruckBedEventChannel = Finder.BoxOutOfTruckBedEventChannel;
            boxOutOfTruckBedEventChannel.OnBoxOut += OnBoxRemove;
            levelFailedEventChannel = Finder.LevelFailedEventChannel;
            levelFailedEventChannel.OnLevelFailedEventChannel += ResetData;
            UpdateNbBox();
            ResetData();
        }

        private void OnDestroy()
        {
            levelLoadedEventChannel.OnLevelLoaded -= OnLevelSpecsLoaded;
        }
        private void ResetData()
        {
            BoxCount = 0;
        }
        
        //Author : François-Xavier Bernier
        private void OnAddBox()
        {
            BoxCount++;
            UpdateNbBox();
        }
        
        //Author : François-Xavier Bernier
        private void OnBoxRemove()
        {
            BoxCount--;
            UpdateNbBox();
        }
        
        //Author : François-Xavier Bernier
        private void UpdateNbBox()
        {
            textNbBox.SetText(BoxCount + "/" + currentLevelSpecs.NbBoxNeededToPassLevel);
        }
    }
}