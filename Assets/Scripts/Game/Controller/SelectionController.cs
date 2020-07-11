using System.Collections.Generic;
using Game.Elements;
using Game.Manager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Controller {
    public class SelectionController : MonoBehaviour {
        public static SelectionController Instance { get; private set; }
        public InteractiveElement CurrentSelection { get; private set; }
        public bool IsDragging { get; private set; }

        [SerializeField] private float smoothSpeed;
        [SerializeField] private Vector2 padding;
        [SerializeField] private GraphicRaycaster raycaster;
        [SerializeField] private EventSystem eventSystem;
        
        private RectTransform _rectTransform;
        private Image _image;
        private PointerEventData _pointerEventData;
        
        private InteractiveElement _hoveredElement;
        private bool _hasHoveringCache;

        private OutlineManager _outlineManager;
        private bool _hasSelection;

        private MousePositionManager _mousePositionManager;

        private void Awake() {
            if (Instance != null)
                return;
            
            Instance = this;
            
            _rectTransform = transform as RectTransform;
            TryGetComponent(out _image);
            TryGetComponent(out _outlineManager);

        }

        void Start() {
            _mousePositionManager = MousePositionManager.Instance;
            SetSelection(null, null);
        }

        void Update() {
            if (_hasSelection)
                return;
            
            HandleHovering();
        }

        void LateUpdate() { 
            var mousePosition = _mousePositionManager.GetMousePosition();
            var screenSize = _mousePositionManager.GetCanvasSize();

            var targetPosition = new Vector2(
                Mathf.Clamp(mousePosition.x, padding.x, screenSize.x - padding.x),
                Mathf.Clamp(mousePosition.y, padding.y, screenSize.y - padding.y)
            );

            _rectTransform.anchoredPosition = Vector2.Lerp(_rectTransform.anchoredPosition, targetPosition, smoothSpeed * Time.deltaTime);
        }

        public void SetSelection(InteractiveElement element, Sprite sprite) {
            CurrentSelection = element;
            _hasSelection = CurrentSelection != null;
            
            IsDragging = sprite != null;
            
            _image.sprite = sprite;
            _image.color = sprite == null ? Color.clear : Color.white;
            
            _outlineManager.ToggleTargetOutline(
                element != null ? element.GetTargetType : ElementType.None, 
                CurrentSelection
            );
            
            if (!_hasSelection) {
                ResetHoveringCache();
                HandleHovering();
            }
        }

        public void OnSelectionReleased() {
            HandleHovering();
            if (_hoveredElement != null && _hoveredElement.CanReceive(CurrentSelection) && 
                (CurrentSelection.GetTargetType == _hoveredElement.GetElementType || 
                 _hoveredElement.GetElementType == ElementType.TrashBin)) {
                
                _hoveredElement.Receive(CurrentSelection);
                CurrentSelection.Empty();
            }
        }

        private void HandleHovering() {
            _pointerEventData = new PointerEventData(eventSystem) { position = Input.mousePosition };

            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(_pointerEventData, results);

            foreach (var r in results) {
                var element = r.gameObject.GetComponent<InteractiveElement>();
                if (element != null) {
                    if (element != _hoveredElement) {
                        _hoveredElement = element;
                        _hasHoveringCache = true;
                        
                        _outlineManager.ToggleHoverOutline(_hoveredElement);
                    }
                    return;
                }
            }

            if (_hasHoveringCache) {
                ResetHoveringCache();
                _outlineManager.ToggleHoverOutline();
            }
        }

        private void ResetHoveringCache() {
            _hoveredElement = null;
            _hasHoveringCache = false;
        }
    }
}
