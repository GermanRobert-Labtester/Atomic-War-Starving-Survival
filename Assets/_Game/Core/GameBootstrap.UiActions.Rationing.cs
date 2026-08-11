using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        /// <summary>Open or close the daily bunker ration terminal ([T]).</summary>
        public void ToggleBunkerRationing()
        {
            var rationing = _hud != null ? _hud.BunkerRationingHUD : null;
            if (rationing == null) return;
            if (!rationing.IsOpen)
            {
                _hud.ScavengeDispatchHUD?.Close();
                _hud.OverflowCrateHUD?.Close();
                _hud.FieldGearLoadoutHUD?.Close();
                _hud.WaterPurificationHUD?.Close();
                _hud.AirHeatManagementHUD?.Close();
                _hud.BunkerMaintenanceHUD?.Close();
                _hud.SurvivorTaskBoardHUD?.Close();
            }
            rationing.Toggle();
        }

        public bool ToggleSelectedRationResource()
        {
            return _hud?.BunkerRationingHUD != null && _hud.BunkerRationingHUD.ToggleSelectedResource();
        }

        public bool IncreaseSelectedRationLevel()
        {
            return _hud?.BunkerRationingHUD != null && _hud.BunkerRationingHUD.IncreaseSelected();
        }

        public bool DecreaseSelectedRationLevel()
        {
            return _hud?.BunkerRationingHUD != null && _hud.BunkerRationingHUD.DecreaseSelected();
        }

        private void HandleRationLevelAdjustmentRequested(RationResource resource, int direction)
        {
            var rationing = _hud != null ? _hud.BunkerRationingHUD : null;
            if (BunkerRationingSystem == null)
            {
                rationing?.ReportAdjustment("Policy system is offline.");
                return;
            }

            bool changed = BunkerRationingSystem.AdjustLevel(resource, direction);
            string resourceLabel = resource == RationResource.Food ? "Food" : "Water";
            if (!changed)
            {
                rationing?.ReportAdjustment(resourceLabel + " is already at that limit.");
                return;
            }

            var snapshot = BunkerRationingSystem.GetSnapshot(Survivors);
            var level = resource == RationResource.Food ? snapshot.FoodLevel : snapshot.WaterLevel;
            rationing?.ReportAdjustment(resourceLabel + " policy set to " + level.ToString().ToUpperInvariant() + ".");
        }
    }
}
