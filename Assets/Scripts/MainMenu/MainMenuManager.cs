using Game.Manager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainMenu {
    public class MainMenuManager : MonoBehaviour {

        [SerializeField] private GameObject mainScreen;
        [SerializeField] private GameObject playScreen;

        private AudioManager _audioManager;

        void Start() => _audioManager = AudioManager.Instance;

        public void StartGame(int difficulty) {
            var difficultyType = (DifficultyType) difficulty;
            
            PlayerPrefs.SetInt(Constants.DifficultyPrefKey, (int)difficultyType);
            SceneManager.LoadScene("GameScene");
        }

        public void ToggleScreen(int screen) {
            var screenType = (ScreenType) screen;
            
            mainScreen.SetActive(screenType == ScreenType.MainScreen);
            playScreen.SetActive(screenType == ScreenType.PlayScreen);
        }

        public void QuitGame() {
            Application.Quit();
        }

        public void PlayClickSound() => _audioManager.PlayPickSound();
    
        public enum ScreenType {
            MainScreen = 0,
            PlayScreen = 1
        }

        public enum DifficultyType {
            Easy = 1,
            Medium = 2,
            Hard = 3
        }
    }
}
