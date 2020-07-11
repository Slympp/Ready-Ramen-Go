using System;
using System.Collections;
using System.Collections.Generic;
using Game.Elements;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Game.Manager {
    public class GameManager : MonoBehaviour {
        public static GameManager Instance { get; private set; } 
        
        [SerializeField] private Transform customersRoot;
        [SerializeField] private GameObject customerPrefab;
        [SerializeField] private List<LevelSettings> settings;
        [SerializeField] private OutlineManager outlineManager;
        [SerializeField] private Image scoreImage;
        [SerializeField] private TMP_Text completedOrdersText;

        [Header("Screens")] 
        [SerializeField] private GameObject gameScreen;
        [SerializeField] private GameObject endScreen;

        private List<CustomerInteractiveElement> _customers;
        private int _remainingOrders;
        private int _completedOrders;
        private int _scoredOrders;

        private LevelSettings _settings;
        private int _currentScore;

        private Coroutine _updateScoreRoutine;

        private void Awake() {
            if (Instance != null)
                return;

            Instance = this;
            
            SetLevelSettings();
            InstantiateCustomers();
        }
        
        private void SetLevelSettings() {
            var index = PlayerPrefs.GetInt(Constants.DifficultyPrefKey);
            if (index == 0) {
                Debug.Log("Failed to get DifficultyKey");
                _settings = settings[0];
            } else {
                _settings = settings[index - 1];
                Debug.Log($"Loaded settings: {_settings.name}");
            }
        }

        private void InstantiateCustomers() {
            _customers = new List<CustomerInteractiveElement>();
            _remainingOrders = _settings.OrdersToComplete;

            var materialList = new List<Tuple<InteractiveElement, Image>>();
            for (int i = 0; i < _settings.MaxCustomers; i++) {
                var go = Instantiate(customerPrefab, customersRoot);
                var customer = go.GetComponent<CustomerInteractiveElement>();
                
                if (customer == null) continue;
                
                _customers.Add(customer);

                var image = go.GetComponent<Image>();
                if (image != null) {
                    materialList.Add(new Tuple<InteractiveElement, Image>(customer, image));
                }
            }
            
            outlineManager.AddMaterialList(ElementType.Customer, materialList);
        }

        private IEnumerator Start() {
            yield return new WaitForSeconds(1.5f);
            TryGenerateNewCustomer();

            while (_remainingOrders > 0) {
                var timer = 0f;
                var timeBeforeNextOrder = Random.Range(_settings.TimeBetweenOrders.x, _settings.TimeBetweenOrders.y);
                
                while (timer < timeBeforeNextOrder) {
                    if (_remainingOrders <= 0) {
                        OnGameOver();
                        yield break;
                    }
                    
                    timer += Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }

                TryGenerateNewCustomer();
            }
            
            OnGameOver();
        }

        private void TryGenerateNewCustomer() {
            var c = GetInactiveCustomer();
            if (c != null)
                c.CreateOrderRoutine = StartCoroutine(c.Create());
        }
        
        private CustomerInteractiveElement GetInactiveCustomer() {
            if (_customers.TrueForAll(c => c.IsActive))
                return null;

            CustomerInteractiveElement customer = null;
            do {
                var c = _customers[Random.Range(0, _customers.Count)];
                customer = c.IsActive ? null : c;
            } while (customer == null);

            return customer;
        }

        public void OnOrderCompleted(CustomerInteractiveElement customer) {
            _completedOrders++;
            
            completedOrdersText.SetText($"Completed: {_completedOrders}");

            if (customer.CreateOrderRoutine != null) {
                StopCoroutine(customer.CreateOrderRoutine);
                customer.CreateOrderRoutine = null;
            }
        }

        public void AddScore(int score) {
            var startScore = GetScore;
            
            _scoredOrders++;
            _currentScore += score;
            
            if (_updateScoreRoutine != null) {
                StopCoroutine(_updateScoreRoutine);
                _updateScoreRoutine = null;
            }
            _updateScoreRoutine = StartCoroutine(nameof(UpdateScore), new Tuple<int, int>(startScore, GetScore));
        }
        
        private IEnumerator UpdateScore(Tuple<int, int> scores) {
            var start = scores.Item1 / 100f;
            var end = scores.Item2 / 100f;
            
            var time = 0f;
            var timeToComplete = .5f;
            while (time <= timeToComplete) {
                scoreImage.fillAmount = Mathf.Lerp(start, end, time / timeToComplete);

                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

        private void OnGameOver() {
            gameScreen.SetActive(false);
            endScreen.gameObject.SetActive(true);
        }
        
        public void OnCustomerLeave() => _remainingOrders--;

        public LevelSettings Settings => _settings;
        public int GetScore => _scoredOrders == 0 ? 0 : _currentScore / _scoredOrders;
    }
}