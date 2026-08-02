using AtomicWar.Runtime.Crafting;
using AtomicWar.Runtime.GameState;
using AtomicWar.Runtime.Inventory;
using AtomicWar.Runtime.Survivors;

namespace AtomicWar.Runtime.AI
{
    /// <summary>
    /// Context container passed to Utility AI evaluators containing current environment and system references.
    /// </summary>
    public class UtilityAIContext
    {
        public GameStateSystem GameStateSystem { get; }
        public InventorySystem InventorySystem { get; }
        public CraftingSystem CraftingSystem { get; }
        public LegacyVitalsSystem LegacyVitalsSystem { get; }
        public SurvivorSystem SurvivorSystem { get; }

        public UtilityAIContext(
            GameStateSystem gameStateSystem,
            InventorySystem inventorySystem,
            CraftingSystem craftingSystem,
            LegacyVitalsSystem legacyVitalsSystem,
            SurvivorSystem survivorSystem)
        {
            GameStateSystem = gameStateSystem;
            InventorySystem = inventorySystem;
            CraftingSystem = craftingSystem;
            LegacyVitalsSystem = legacyVitalsSystem;
            SurvivorSystem = survivorSystem;
        }
    }
}
