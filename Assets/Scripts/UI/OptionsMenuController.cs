using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UI
{
    public class OptionsMenuController : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [SerializeField] private Toggle fullscreenToggle;
        [SerializeField] private TMP_Dropdown qualityDropdown;
        [SerializeField] private Slider volumeSlider;
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private string volumeParameter = "Volume";

        private Resolution[] _resolutions;
        private Dictionary<string, object> _oldSettings;

        private const string ResolutionKey = "Resolution";
        private const string FullScreenKey = "FullScreen";
        private const string QualityKey = "Quality";
        private const string VolumeKey = "Volume";

        private void Start()
        {
            _resolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();
            foreach (var resolution in _resolutions)
            {
                resolutionDropdown.options.Add(new TMP_Dropdown.OptionData($"{resolution.width} x {resolution.height}"));
            }
            resolutionDropdown.value = Array.IndexOf(_resolutions, Screen.currentResolution);
            resolutionDropdown.RefreshShownValue();
            fullscreenToggle.isOn = Screen.fullScreen;
            qualityDropdown.value = QualitySettings.GetQualityLevel();
            volumeSlider.value = audioMixer.GetFloat(volumeParameter, out float volume) ? volume : -5;
            
            SaveCurrentSettings();
        }

        private void SaveCurrentSettings()
        {
            _oldSettings = new Dictionary<string, object>
            {
                { ResolutionKey, resolutionDropdown.value },
                { FullScreenKey, fullscreenToggle.isOn },
                { QualityKey, qualityDropdown.value },
                { VolumeKey, volumeSlider.value }
            };
        }

        public void OnDropDownResolution(int value)
        {
            Resolution resolution = _resolutions[value];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        public void OnToggleFullScreen(bool isFullScreen)
        {
            Screen.fullScreen = isFullScreen;
        }

        public void OnDropDownQuality(int value)
        {
            QualitySettings.SetQualityLevel(value);
        }

        public void OnSliderVolumeChange(float volume)
        {
            audioMixer.SetFloat(volumeParameter, volume);
        }

        public void OnButtonApply()
        {
            PlayerPrefs.SetInt(ResolutionKey, resolutionDropdown.value);
            PlayerPrefs.SetInt(FullScreenKey, fullscreenToggle.isOn ? 1 : 0);
            PlayerPrefs.SetInt(QualityKey, qualityDropdown.value);
            PlayerPrefs.SetFloat(VolumeKey, volumeSlider.value);
            PlayerPrefs.Save();
            
            SaveCurrentSettings();
        }

        public void OnButtonCancel()
        {
            resolutionDropdown.value = (int)_oldSettings[ResolutionKey];
            fullscreenToggle.isOn = (bool)_oldSettings[FullScreenKey];
            qualityDropdown.value = (int)_oldSettings[QualityKey];
            volumeSlider.value = (float)_oldSettings[VolumeKey];
        }
    }
}
