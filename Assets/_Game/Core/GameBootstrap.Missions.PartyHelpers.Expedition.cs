using System;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        private string FindActiveHatchDilemmaExpeditionId()
        {
            // Best-effort: walk active expeditions for AtHatchDilemma; empty = no-op resolve.
            if (ExpeditionSystem?.ActiveExpeditions == null) return string.Empty;
            for (int i = 0; i < ExpeditionSystem.ActiveExpeditions.Count; i++)
            {
                var e = ExpeditionSystem.ActiveExpeditions[i];
                if (e != null && e.Phase == ExpeditionPhase.AtHatchDilemma)
                    return e.ExpeditionId;
            }
            return string.Empty;
        }

        private bool IsSurvivorOnExpedition(Survivor s)
        {
            if (s == null || ExpeditionSystem?.ActiveExpeditions == null) return false;
            for (int i = 0; i < ExpeditionSystem.ActiveExpeditions.Count; i++)
            {
                var e = ExpeditionSystem.ActiveExpeditions[i];
                if (e?.Survivor != null && e.Survivor.Id == s.Id) return true;
            }
            return false;
        }

        private bool IsSurvivorHatchListener(Survivor s)
        {
            if (s == null || !s.IsAlive) return false;
            if (string.Equals(s.CurrentRoomId, HatchEntrapmentSystem.EntryRoomId, StringComparison.OrdinalIgnoreCase))
                return true;
            return IsSealedHatchRainListener();
        }

        private bool IsSealedHatchRainListener()
        {
            // Sealed hatch transmits the hammer of rain into the bunker.
            return HatchEntrapmentSystem != null
                && HatchEntrapmentSystem.State != HatchState.Clear
                && BlackRainHazardSystem != null
                && BlackRainHazardSystem.IsActive;
        }
    }
}
