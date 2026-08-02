namespace AtomicWar._Game.Crafting
{
    /// <summary>
    /// Validates recipes against the inventory and an available station, runs
    /// craft timers, and consumes/produces items. Raises events on start and
    /// completion.
    /// </summary>
    public class CraftingSystem
    {
        /// <summary>Whether a recipe can be started right now (inputs + station present).</summary>
        public bool CanCraft(Recipe recipe) => throw new System.NotImplementedException();

        /// <summary>Begin crafting a recipe, reserving its ingredients.</summary>
        public void StartCraft(Recipe recipe) => throw new System.NotImplementedException();

        /// <summary>Advance in-progress crafts over elapsed game hours.</summary>
        public void Tick(float gameHours) => throw new System.NotImplementedException();
    }
}
