// SPDX-License-Identifier: MIT
using Ashfall.Core.Expeditions;
using AtomicWar.GodotApp.Localization;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Localized display label for an <see cref="ExpeditionPhase"/>. One owner
    /// for the phase wording so the sortie planner and the radar console never
    /// drift (and so the phase values match their localized column headers).
    /// </summary>
    public static class ExpeditionPhaseText
    {
        public static string Label(ExpeditionPhase phase) => phase switch
        {
            ExpeditionPhase.Outbound => AshfallLocalization.Tr("ui.expedition.phase.outbound", "OUTBOUND"),
            ExpeditionPhase.Looting => AshfallLocalization.Tr("ui.expedition.phase.looting", "LOOTING"),
            ExpeditionPhase.Inbound => AshfallLocalization.Tr("ui.expedition.phase.inbound", "INBOUND"),
            ExpeditionPhase.Completed => AshfallLocalization.Tr("ui.expedition.phase.completed", "COMPLETED"),
            ExpeditionPhase.Failed => AshfallLocalization.Tr("ui.expedition.phase.failed", "FAILED"),
            ExpeditionPhase.Camp => AshfallLocalization.Tr("ui.expedition.phase.camp", "CAMP"),
            _ => phase.ToString().ToUpperInvariant()
        };
    }
}