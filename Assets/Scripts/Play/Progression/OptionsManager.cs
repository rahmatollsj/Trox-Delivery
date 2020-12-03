using Harmony;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    [Findable(Tags.MainController)]
    public class OptionsManager : MonoBehaviour
    {
        [SerializeField] private string optionsFilePath = "options.dat";
        private const string InvertedRotationUID = "invertedRotation";
        private const string MusicVolumeUID = "musicVolume";
        private const string GameVolumeUID = "gameVolume";

        private SaveFile activeSaveFile = null;

        public void WriteCustomBindingPath(InputBinding binding)
            => activeSaveFile.SaveData(SaveTag.Control, binding.overridePath, binding.id.ToString());

        public string ReadCustomBindingPath(InputBinding binding)
            => activeSaveFile.ReadData<string>(SaveTag.Control, binding.id.ToString());

        public bool ReadInvertedRotationEnabled()
            => activeSaveFile.ReadData<bool>(SaveTag.Control, InvertedRotationUID);

        public void WriteInvertedRotationEnabled(bool enabled)
            => activeSaveFile.SaveData(SaveTag.Control, enabled, InvertedRotationUID);

        public float? ReadMusicVolume()
            => activeSaveFile.ReadData<float?>(SaveTag.Music, MusicVolumeUID);

        public void WriteMusicVolume(float value)
            => activeSaveFile.SaveData(SaveTag.Music, value, MusicVolumeUID);

        public float? ReadGameVolume()
            => activeSaveFile.ReadData<float?>(SaveTag.Music, GameVolumeUID);

        public void WriteGameVolume(float value)
            => activeSaveFile.SaveData<float?>(SaveTag.Music, value, GameVolumeUID);

        private void Awake()
        {
            activeSaveFile = new SaveFile(optionsFilePath);
        }

        private void OnDestroy()
        {
            activeSaveFile.Close();
        }
    }
}
