using UnityEngine;

namespace Game.Manager {
    public class AudioManager : MonoBehaviour {
        public static AudioManager Instance { get; private set; }

        [SerializeField] private AudioClip pickSound;
        [SerializeField] private AudioClip dropSound;

        // ReSharper disable once InconsistentNaming
        [SerializeField] private AudioSource SFXSource;

        void Awake() {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void PlayPickSound() => PlaySound(pickSound);
        public void PlayDropSound() => PlaySound(dropSound);
        
        public void PlaySound(AudioClip clip) {
            if (clip != null)
                SFXSource.PlayOneShot(clip);
        }
    }
}