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
            resolutionDropdown.RefreshShownValue();
            
            LoadPlayerSettings();
            SaveCurrentSettings();
        }
        
        public void OnOpenOptionsMenu()
        {
            LoadPlayerSettings();
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
            SaveCurrentSettings();
        }

        public void OnButtonCancel()
        {
            LoadOldSettings();
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
            
            PlayerPrefs.SetInt(ResolutionKey, resolutionDropdown.value);
            PlayerPrefs.SetInt(FullScreenKey, fullscreenToggle.isOn ? 1 : 0);
            PlayerPrefs.SetInt(QualityKey, qualityDropdown.value);
            PlayerPrefs.SetFloat(VolumeKey, volumeSlider.value);
            PlayerPrefs.Save();
        }
        
        private void LoadOldSettings()
        {
            if (_oldSettings == null) return;
            
            Screen.SetResolution(_resolutions[(int)_oldSettings[ResolutionKey]].width, _resolutions[(int)_oldSettings[ResolutionKey]].height, (bool)_oldSettings[FullScreenKey]);
            Screen.fullScreen = (bool)_oldSettings[FullScreenKey];
            QualitySettings.SetQualityLevel((int)_oldSettings[QualityKey]);
            audioMixer.SetFloat(volumeParameter, (float)_oldSettings[VolumeKey]);
            
            resolutionDropdown.value = (int)_oldSettings[ResolutionKey];
            fullscreenToggle.isOn = (bool)_oldSettings[FullScreenKey];
            qualityDropdown.value = (int)_oldSettings[QualityKey];
            volumeSlider.value = (float)_oldSettings[VolumeKey];
        }
        
        private void LoadPlayerSettings()
        {
            // Load unity's default settings            
            int defaultResolution = Array.IndexOf(_resolutions, Screen.currentResolution);
            bool defaultFullScreen = Screen.fullScreen;
            int defaultQualityLevel = QualitySettings.GetQualityLevel();
            float defaultVolume = audioMixer.GetFloat(volumeParameter, out float volume) ? volume : -5;
            
            // Load player's saved settings (player prefs)
            int playerResolution = PlayerPrefs.GetInt(ResolutionKey, defaultResolution);
            bool playerFullScreen = PlayerPrefs.GetInt(FullScreenKey, defaultFullScreen ? 1 : 0) == 1;
            int playerQualityLevel = PlayerPrefs.GetInt(QualityKey, defaultQualityLevel);
            float playerVolume = PlayerPrefs.GetFloat(VolumeKey, defaultVolume);
            
            // Apply settings
            Screen.SetResolution(_resolutions[playerResolution].width, _resolutions[playerResolution].height, playerFullScreen);
            Screen.fullScreen = playerFullScreen;
            QualitySettings.SetQualityLevel(playerQualityLevel);
            audioMixer.SetFloat(volumeParameter, playerVolume);
            
            // Set UI elements
            resolutionDropdown.value = playerResolution;
            fullscreenToggle.isOn = playerFullScreen;
            qualityDropdown.value = playerQualityLevel;
            volumeSlider.value = playerVolume;
        }
    }
}
