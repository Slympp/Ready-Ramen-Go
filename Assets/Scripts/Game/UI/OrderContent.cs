using System;
using System.Collections.Generic;
using Game.Elements;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI {
    public class OrderContent : MonoBehaviour {

        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private List<BrothItem> brothItems;
        [SerializeField] private  List<ToppingItem> toppingItem;

        private Dictionary<BrothType, Sprite> _brothDictionary;
        private Dictionary<ToppingType, Sprite> _toppingDictionary;

        private Transform _transform;

        void Awake() {
            _transform = transform;
            
            _brothDictionary = new Dictionary<BrothType, Sprite>();
            foreach (var i in brothItems) {
                _brothDictionary.Add(i.brothType, i.sprite);
            }
            
            _toppingDictionary = new Dictionary<ToppingType, Sprite>();
            foreach (var i in toppingItem)
                _toppingDictionary.Add(i.toppingType, i.sprite);
        }

        public void Fill(Order o) {
            if (_brothDictionary.TryGetValue(o.BrothType, out var brothSprite))
                AddOrderItem(brothSprite);

            foreach (var t in o.ToppingType)
                if (_toppingDictionary.TryGetValue(t, out var toppingSprite))
                    AddOrderItem(toppingSprite);
        }

        public void Clear() {
            foreach (Transform t in _transform)
                Destroy(t.gameObject);
        }

        private void AddOrderItem(Sprite s) {
            var item = Instantiate(itemPrefab, _transform);
            
            item.GetComponent<Image>().sprite = s;
        }
    }

    [Serializable]
    public class BrothItem {
        public BrothType brothType;
        public Sprite sprite;
    }

    [Serializable]
    public class ToppingItem {
        public ToppingType toppingType;
        public Sprite sprite;
    }
}