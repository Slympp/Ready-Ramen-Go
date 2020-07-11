using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI {
    public class ProgressBarController : MonoBehaviour {
        [SerializeField] private GameObject background;
        [SerializeField] private Image filler;

        public void Animate(float time) => StartCoroutine(nameof(Progress), time);
        
        private IEnumerator Progress(float time) {
            Toggle(true);
            
            float timer = 0;
            while (timer <= time) {
                filler.fillAmount = 1 - timer / time;
                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            
            Toggle(false);
        }

        private void Toggle(bool active) => background.SetActive(active);
    }
}
