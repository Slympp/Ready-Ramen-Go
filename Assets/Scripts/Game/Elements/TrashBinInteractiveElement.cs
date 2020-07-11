using UnityEngine.EventSystems;

namespace Game.Elements {
    public class TrashBinInteractiveElement : InteractiveElement {

        protected override void Awake() {
            base.Awake();
            CanInteractWith = false;
        }
        
        public override bool CanReceive(InteractiveElement element) => true;

        public override void OnPointerDown(PointerEventData eventData) { }
        public override void OnPointerUp(PointerEventData eventData) { }
        public override bool CanBeSelected() => false;
    }
}