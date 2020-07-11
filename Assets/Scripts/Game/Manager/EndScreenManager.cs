using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Manager {
    public class EndScreenManager : MonoBehaviour {

        [SerializeField] private Image endScoreFiller;

        private void OnEnable() {
            endScoreFiller.fillAmount = GameManager.Instance.GetScore == 0 ? 0 : GameManager.Instance.GetScore / 100f;
        }

        public void Replay() {
            SceneManager.LoadScene("GameScene");
        }

        public void Quit() {
            SceneManager.LoadScene("MainMenu");
        }
    }
}