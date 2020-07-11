using UnityEngine;
using UnityEngine.UI;

namespace Game.Manager {
    public class MousePositionManager : MonoBehaviour {
        public static MousePositionManager Instance { get; private set; }

        private Canvas _canvas;
        private RectTransform _canvasTransform;

        private Vector2 _mousePosition;
        
        void Awake() {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            if (TryGetComponent(out _canvas))
                _canvasTransform = _canvas.transform as RectTransform;
        }

        void Update() {
            _mousePosition = Input.mousePosition / _canvas.scaleFactor;
        }

        public Vector2 GetMousePosition() => _mousePosition;
        public Vector2 GetCanvasSize() => _canvasTransform.sizeDelta;
    }
}