using System.Collections.Generic;
using AtomicWar.Core.Save;
using AtomicWar.Core.Services;
using AtomicWar.Runtime.AI;
using AtomicWar.Runtime.Crafting;
using AtomicWar.Runtime.GameState;
using AtomicWar.Runtime.Inventory;
using AtomicWar.Runtime.RandomEvents;
using AtomicWar.Runtime.Scavenging;
using AtomicWar.Runtime.Survivors;
using AtomicWar.Runtime.Time;
using UnityEngine;

namespace AtomicWar.Core
{
    /// <summary>
    /// Master composition root MonoBehaviour. 
    /// Initializes all pure C# domain systems, registers them in ServiceLocator, and orchestrates frame updates.
    /// </summary>
    public class GameController : MonoBehaviour
    {
        [Header("Initial Configuration")]
        [SerializeField] private bool _autoLoadSave = false;
        [SerializeField] private List<UtilityActionSO> _defaultAIActions = new List<UtilityActionSO>();
        [SerializeField] private float _aiEvaluationIntervalSeconds = 3.0f;

        // Domain Systems
        public GameStateSystem GameStateSystem { get; private set; }
        public TimeSystem TimeSystem { get; private set; }
        public SurvivorSystem SurvivorSystem { get; private set; }
        public LegacyVitalsSystem LegacyVitalsSystem { get; private set; }
        public InventorySystem InventorySystem { get; private set; }
        public CraftingSystem CraftingSystem { get; private set; }
        public ScavengingSystem ScavengingSystem { get; private set; }
        public EventSystem EventSystem { get; private set; }
        public UtilityAISystem UtilityAISystem { get; private set; }
        public SaveSystem SaveSystem { get; private set; }

        private void Awake()
        {
            InitializeSystems();
        }

        private void InitializeSystems()
        {
            ServiceLocator.Clear();

            // 1. Create Core Services
            SaveSystem = new SaveSystem();
            ServiceLocator.Register(SaveSystem);

            // 2. Create Domain Systems
            GameStateSystem = new GameStateSystem();
            ServiceLocator.Register(GameStateSystem);

            TimeSystem = new TimeSystem(GameStateSystem);
            ServiceLocator.Register(TimeSystem);

            SurvivorSystem = new SurvivorSystem();
            ServiceLocator.Register(SurvivorSystem);

            LegacyVitalsSystem = new LegacyVitalsSystem(SurvivorSystem);
            ServiceLocator.Register(LegacyVitalsSystem);

            InventorySystem = new InventorySystem();
            ServiceLocator.Register(InventorySystem);

            CraftingSystem = new CraftingSystem(InventorySystem);
            ServiceLocator.Register(CraftingSystem);

            ScavengingSystem = new ScavengingSystem(InventorySystem);
            ServiceLocator.Register(ScavengingSystem);

            EventSystem = new EventSystem(InventorySystem, SurvivorSystem);
            ServiceLocator.Register(EventSystem);

            var nightRaidSystem = new NightRaidSystem(InventorySystem, SurvivorSystem);
            ServiceLocator.Register(nightRaidSystem);

            // 3. Create Utility AI System
            var aiContext = new UtilityAIContext(
                GameStateSystem,
                InventorySystem,
                CraftingSystem,
                LegacyVitalsSystem,
                SurvivorSystem
            );

            UtilityAISystem = new UtilityAISystem(aiContext, _defaultAIActions, _aiEvaluationIntervalSeconds);
            ServiceLocator.Register(UtilityAISystem);

            Debug.Log("[GameController] All domain systems successfully registered in ServiceLocator.");
        }

        private void Update()
        {
            float dt = Time.deltaTime;

            // Tick time, crafting, and Utility AI engine
            TimeSystem.Tick(dt);
            CraftingSystem.TickCrafting(dt);
            UtilityAISystem.Tick(dt);
        }

        private void OnDestroy()
        {
            ServiceLocator.Clear();
        }
    }
}
