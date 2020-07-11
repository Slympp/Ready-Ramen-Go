using UnityEngine;

namespace Game.Elements {
    public class BrothInteractiveElement : InteractiveElement {
        [SerializeField] private BrothType brothType;

        public BrothType GetBrothType => brothType;
    }

    public enum BrothType {
        None = 0,
        Miso = 1,
        Shio = 2
    }
}