using System.Collections;
using Game.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Elements {
    public class CuttingBoardInteractiveElement : InteractiveElement {

        public ToppingType ToppingType { get; private set; }
        public Sprite ToppingFinalSprite { get; private set; }

        [SerializeField] private float time;
        [SerializeField] private Image content;
        // ReSharper disable once InconsistentNaming
        [SerializeField] private AudioClip SFXSound;

        private Sprite _lastContentSprite;
        private Sprite _toppingCutSprite;

        private ProgressBarController _progressBarController;
        
        protected override void Awake() {
            base.Awake();

            _progressBarController = GetComponentInChildren<ProgressBarController>();
        }

        public override void Receive(InteractiveElement element) {
            base.Receive(element);

            if (element is ToppingInteractiveElement topping) {
                StartCoroutine(nameof(Interact), topping);
            }
        }

        private IEnumerator Interact(ToppingInteractiveElement topping) {
            CanInteractWith = false;
            
            SetContentSprite(topping.GetDefaultSprite);
            
            AudioManager.PlaySound(SFXSound);
            
            ToppingType = topping.GetToppingType;
            _lastContentSprite = _toppingCutSprite = topping.GetCutSprite;
            ToppingFinalSprite = topping.GetFinalSprite;
            
            _progressBarController.Animate(time);
            yield return new WaitForSeconds(time);

            SetContentSprite(_toppingCutSprite);

            HasContent = true;
            CanInteractWith = true;
        }
        
        public override void Empty() {
            base.Empty();
            
            ToppingType = ToppingType.None;
            _lastContentSprite = _toppingCutSprite = null;
            ToppingFinalSprite = null;
            SetContentSprite(_lastContentSprite);
        }

        protected override void OnPointerDownSetSprite() {
            SetContentSprite(null);
        }

        protected override void OnPointerUpSetSprite() {
            SetContentSprite(_lastContentSprite);
        }

        private void SetContentSprite(Sprite sprite) {
            content.sprite = sprite;
            content.color = sprite != null ? Color.white : Color.clear;
        }
        
        protected override Sprite GetSelectionSprite() => _toppingCutSprite;
    }
}