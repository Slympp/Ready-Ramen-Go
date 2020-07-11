using System;
using System.Collections.Generic;
using Game.Controller;
using Game.Manager;
using UnityEngine;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;

namespace Game.Elements {
    public class InteractiveElement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
        public bool CanInteractWith { get; protected set; } = true;
        public bool HasContent { get; protected set; } = false;

        [SerializeField] protected bool infinite;
        [SerializeField] protected ElementType elementType;
        [SerializeField] protected ElementType targetType;

        private SelectionController _selectionController;
        
        protected Image Image;
        protected Sprite DefaultSprite;
        protected Sprite LastSprite;

        protected AudioManager AudioManager;
        
        public ElementType GetElementType => elementType;
        public ElementType GetTargetType => targetType;

        protected virtual void Awake() {
            TryGetComponent(out Image);
            if (Image != null) {
                Image.alphaHitTestMinimumThreshold = .3f;
                LastSprite = DefaultSprite = Image.sprite;
            }
        }

        protected virtual void Start() {
            _selectionController = SelectionController.Instance;
            AudioManager = AudioManager.Instance;
        }

        public virtual void OnPointerDown(PointerEventData eventData) {
            if (eventData.pointerId != -1 || !CanBeSelected())
                return;

            _selectionController.SetSelection(this, GetSelectionSprite());
            AudioManager.PlayPickSound();
            
            OnPointerDownSetSprite();
        }
        
        public virtual void OnPointerUp(PointerEventData eventData) {
            if (_selectionController.CurrentSelection != this)
                return;

            _selectionController.OnSelectionReleased();
            AudioManager.PlayDropSound();
            
            _selectionController.SetSelection(null, null);
            
            OnPointerUpSetSprite();
        }
        
        protected virtual void OnPointerDownSetSprite() {
            SetSprite(null);
        }

        protected virtual void OnPointerUpSetSprite() {
            SetSprite(LastSprite);
        }

        public virtual void Receive(InteractiveElement element) { }

        public virtual bool CanReceive(InteractiveElement element) => CanInteractWith && !HasContent;

        public virtual bool CanBeSelected() => !_selectionController.IsDragging && CanInteractWith && (HasContent || infinite);
        
        public virtual void Empty() {
            HasContent = false;

            LastSprite = DefaultSprite;
            SetSprite(DefaultSprite);
        }
        
        protected void SetSprite(Sprite sprite) {
            Image.sprite = sprite;
            Image.color = sprite != null ? Color.white : Color.clear;
        }
        
        protected virtual Sprite GetSelectionSprite() => Image.sprite;
    }

    public enum ElementType {
        None = 0,
        Pot = 1,
        Strainer = 2,
        CuttingBoard = 3,
        Broth = 4,
        PlateStack = 5,
        Plate = 6,
        Customer = 7,
        Topping = 8,
        TrashBin = 9
    }
}