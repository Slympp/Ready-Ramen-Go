using Game.Manager;
using UnityEngine;

namespace Game.Controller {
    public class CharacterController : MonoBehaviour {
        [SerializeField] private float movementSpeed = 4;
        [SerializeField] private float rotationSpeed = 5;
        [SerializeField] private Vector2 bounds;

        private RectTransform _rectTransform;

        private MousePositionManager _mousePositionManager;
        
        void Start() => _mousePositionManager = MousePositionManager.Instance;

        private void Awake() {
            _rectTransform = transform as RectTransform;
        }

        void LateUpdate() {
            var pos = _mousePositionManager.GetMousePosition();

            Vector2 anchoredPosition;
            var targetPosition = new Vector2(
                Mathf.Clamp(pos.x, bounds.x, bounds.y), 
                (anchoredPosition = _rectTransform.anchoredPosition).y
            );
            
            _rectTransform.anchoredPosition = Vector2.Lerp(anchoredPosition, targetPosition, movementSpeed * Time.deltaTime);
            _rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, pos.y > anchoredPosition.y ? 0 : 180));
        }

        float GetAngle(Vector2 p1, Vector2 p2) => Mathf.Atan2(p2.y - p1.y, p2.x - p1.x) * Mathf.Rad2Deg;
    }
}