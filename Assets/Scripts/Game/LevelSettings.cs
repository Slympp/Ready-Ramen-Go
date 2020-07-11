using UnityEngine;

namespace Game {
    
    [CreateAssetMenu(fileName = "LevelSettings", menuName = "Settings/Level Settings")]
    public class LevelSettings : ScriptableObject {
        [SerializeField] private int ordersToComplete;
        [SerializeField] private int maxCustomers;

        [SerializeField] private Vector2Int toppingsCountCount;
        [SerializeField] private Vector2 timeBetweenOrders;
        [SerializeField] private Vector2 timeToReceive;

        public int OrdersToComplete => ordersToComplete;
        public int MaxCustomers => maxCustomers;
        
        public Vector2Int ToppingsCount => toppingsCountCount;
        public Vector2 TimeBetweenOrders => timeBetweenOrders;
        public Vector2 TimeToReceive => timeToReceive;
    }
}