using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using Game.Manager;
using Game.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Game.Elements {
    public class CustomerInteractiveElement : InteractiveElement {

        public Order Order { get; private set; }
        public Coroutine CreateOrderRoutine { get; set; }

        [SerializeField] private GameObject progressionBar;
        [SerializeField] private Image progressionBarImage;
        [SerializeField] private Gradient progressionBarGradient;
        [SerializeField] private OrderContent orderContent;
        
        [Space]
        
        [SerializeField] private float timeToConsume;
        [SerializeField] private List<Sprite> customerSprites;
        // ReSharper disable once InconsistentNaming
        [SerializeField] private AudioClip SFXSound;

        private GameManager _gameManager;
        private float _timeToReceive;
        private float _currentTime;
        
        public bool IsActive => HasContent || CanInteractWith;

        protected override void Awake() {
            base.Awake();
            CanInteractWith = false;
        }

        public IEnumerator Create() {
            if (_gameManager == null)
                _gameManager = GameManager.Instance;

            Order = new Order(_gameManager.Settings.ToppingsCount);

            CanInteractWith = true;
            SetSprite(customerSprites[Random.Range(0, customerSprites.Count)]);
            
            progressionBar.SetActive(true);
            orderContent.Fill(Order);
            
            _timeToReceive = Random.Range(_gameManager.Settings.TimeToReceive.x, _gameManager.Settings.TimeToReceive.y);
            _currentTime = 0;
            while (_currentTime <= _timeToReceive) {
                var v = 1 - (_currentTime / _timeToReceive);

                progressionBarImage.fillAmount = v;
                progressionBarImage.color = progressionBarGradient.Evaluate(v);
                
                _currentTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            progressionBar.SetActive(false);
            orderContent.Clear();
            
            CanInteractWith = false;
            SetSprite(null);
            
            _gameManager.AddScore(0);
            _gameManager.OnCustomerLeave();
        }

        private IEnumerator Consume() {
            progressionBar.SetActive(false);
            orderContent.Clear();
            
            yield return new WaitForSeconds(timeToConsume);
            
            HasContent = false;
            CanInteractWith = false;
            SetSprite(null);
            
            _gameManager.OnCustomerLeave();
        }
        
        public override void Receive(InteractiveElement element) {
            if (element is PlateInteractiveElement plate) {
                HasContent = true;
                CanInteractWith = false;
                
                AudioManager.PlaySound(SFXSound);
                
                _gameManager.OnOrderCompleted(this);
                _gameManager.AddScore(CalculateScore(plate));

                StartCoroutine(nameof(Consume));
            }
        }

        private int CalculateScore(PlateInteractiveElement plate) {
            // Time
            var halfTime = _timeToReceive / 2;
            var v = _currentTime <= _timeToReceive / 2 ? 1 : 1 - (_currentTime - halfTime) / (_timeToReceive - halfTime);
            var score = (int)(35 * v);
            
            // Broth
            if (plate.BrothType == Order.BrothType)
                score += 10;
            
            if (plate.HasNoodles)
                score += 5;
            
            // Toppings
            var scorePerTopping = 50f / Order.ToppingType.Count;
            foreach (var t in Order.ToppingType) {
                if (plate.ToppingTypes.Contains(t))
                    score += (int)scorePerTopping;
            }

            return score;
        }

        public override bool CanReceive(InteractiveElement element) => CanInteractWith && !HasContent;

        // Forbid drag'n'drop customers
        public override void OnPointerDown(PointerEventData eventData) { }
        public override void OnPointerUp(PointerEventData eventData) { }
        public override bool CanBeSelected() => false;
    }
}