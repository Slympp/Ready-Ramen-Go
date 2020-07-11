using System;
using System.Collections.Generic;
using System.Linq;
using Game.Elements;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game {
    public class Order {
        public BrothType BrothType { get; }
        public List<ToppingType> ToppingType { get; }

        public Order(Vector2Int toppingsCount) {
            BrothType = Random.value > 0.5 ? BrothType.Shio : BrothType.Miso;

            var toppingCount = Random.Range(toppingsCount.x, toppingsCount.y + 1);

            var list = new List<int>();
            var length = Enum.GetValues(typeof(ToppingType)).Length;
            
            for (int i = 1; i < length; i++)
                list.Add(i);
            
            
            ToppingType = new List<ToppingType>();
            for (var i = 0; i < toppingCount; i++) {
                var toppingIndex = Random.Range(0, list.Count);
                ToppingType.Add((ToppingType)list[toppingIndex]);
                list.RemoveAt(toppingIndex);
            }
        }
    }
}