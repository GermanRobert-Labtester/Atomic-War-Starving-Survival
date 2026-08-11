using AtomicWar._Game.Shelter;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        /// <summary>Open or close the clean/dirty/irradiated cistern terminal ([Y]).</summary>
        public void ToggleWaterPurification()
        {
            var terminal = _hud != null ? _hud.WaterPurificationHUD : null;
            if (terminal == null) return;
            if (!terminal.IsOpen)
            {
                _hud.ScavengeDispatchHUD?.Close();
                _hud.OverflowCrateHUD?.Close();
                _hud.FieldGearLoadoutHUD?.Close();
                _hud.BunkerRationingHUD?.Close();
                _hud.AirHeatManagementHUD?.Close();
                _hud.BunkerMaintenanceHUD?.Close();
                _hud.SurvivorTaskBoardHUD?.Close();
            }
            terminal.Toggle();
        }

        public bool CycleWaterPurifierQueuePrevious()
        {
            return _hud?.WaterPurificationHUD != null && _hud.WaterPurificationHUD.QueuePrevious();
        }

        public bool CycleWaterPurifierQueueNext()
        {
            return _hud?.WaterPurificationHUD != null && _hud.WaterPurificationHUD.QueueNext();
        }

        private void HandleWaterQueueCycleRequested(int direction)
        {
            var terminal = _hud != null ? _hud.WaterPurificationHUD : null;
            if (WaterEconomySystem == null)
            {
                terminal?.ReportQueueResult("Purifier system is offline.");
                return;
            }

            bool changed = WaterEconomySystem.CyclePurifierQueue(direction);
            if (!changed)
            {
                terminal?.ReportQueueResult("Purifier queue is unchanged.");
                return;
            }

            string label = QueueLabel(WaterEconomySystem.CurrentPurifierQueue);
            terminal?.ReportQueueResult("Purifier queue set to " + label + ".");
        }

        private static string QueueLabel(PurifierQueueMode queueMode)
        {
            switch (queueMode)
            {
                case PurifierQueueMode.IrradiatedFirst: return "IRRADIATED FIRST";
                case PurifierQueueMode.DirtyFirst: return "DIRTY FIRST";
                default: return "AUTO";
            }
        }
    }
}
