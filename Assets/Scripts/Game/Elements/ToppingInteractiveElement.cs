using UnityEngine;

namespace Game.Elements {
    public class ToppingInteractiveElement : InteractiveElement {
        [SerializeField] private Sprite cutSprite;
        [SerializeField] private Sprite finalSprite;
        [SerializeField] private ToppingType toppingType;
        
        public Sprite GetDefaultSprite => DefaultSprite;
        public Sprite GetCutSprite => cutSprite;
        public Sprite GetFinalSprite => finalSprite;
        public ToppingType GetToppingType => toppingType;
    }
    
    public enum ToppingType {
        None = 0,
        Pork = 1,
        Mushroom = 2,
        Egg = 3,
        Naruto = 4,
        Wakame = 5,
        Nori = 6,
        GreenOnion = 7,
        SoyBeans = 8,
    }
}