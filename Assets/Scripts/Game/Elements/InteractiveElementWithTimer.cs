using System.Collections;
using Game.UI;
using UnityEngine;

namespace Game.Elements {
    public class InteractiveElementWithTimer : InteractiveElement {
        [SerializeField] private float time;
        [SerializeField] private Sprite finalSprite;
        [SerializeField] private Sprite selectionSprite;
        // ReSharper disable once InconsistentNaming
        [SerializeField] private AudioClip SFXSound;

        private ProgressBarController _progressBarController;
        
        protected override void Awake() {
            base.Awake();

            _progressBarController = GetComponentInChildren<ProgressBarController>();
        }

        public override void Receive(InteractiveElement element) {
            base.Receive(element);
            
            StartCoroutine(nameof(Interact));
        }

        private IEnumerator Interact() {
            CanInteractWith = false;
            
            AudioManager.PlaySound(SFXSound);

            Image.sprite = LastSprite = finalSprite;

            _progressBarController.Animate(time);
            yield return new WaitForSeconds(time);
            
            HasContent = true;
            CanInteractWith = true;
        }

        protected override Sprite GetSelectionSprite() =>
                selectionSprite == null ? base.GetSelectionSprite() : selectionSprite;
    }
}