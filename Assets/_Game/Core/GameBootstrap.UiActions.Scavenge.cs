using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;
using UnityEngine;
using Ashfall.Core;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        /// <summary>
        /// Player-facing safety gate for location scavenging. The location system
        /// remains focused on one mission type; Core resolves constraints that
        /// belong to more than one system (hatch state and expeditions).
        /// </summary>
        private string GetScavengeDispatchBlockReason(Survivor survivor)
        {
            if (ScavengingSystem == null) return "Scavenge system offline.";
            if (survivor == null || !survivor.IsAlive) return "No living operative available.";
            if (HatchEntrapmentSystem != null && HatchEntrapmentSystem.AreExpeditionsLocked)
                return "Hatch sealed — no one leaves.";
            if (ExpeditionSystem != null && ExpeditionSystem.IsOnExpedition(survivor.Id))
                return "Operative is already on expedition.";
            if (CensusClaimSystem != null && CensusClaimSystem.IsAssignedAway(survivor.Id))
                return "Operative is on the levy column.";

            string reservation = SurvivorTaskBoardSystem != null
                ? SurvivorTaskBoardSystem.GetAssignmentConflictReason(survivor)
                : null;
            if (!string.IsNullOrEmpty(reservation)) return reservation;

            var active = ScavengingSystem.ActiveMissions;
            for (int i = 0; i < active.Count; i++)
            {
                if (active[i] != null && active[i].SurvivorId == survivor.Id)
                    return "Operative is already scavenging.";
            }

            if (survivor.State == SurvivorState.Working)
                return "Operative is assigned elsewhere.";
            if (survivor.State == SurvivorState.Incapacitated)
                return "Operative is incapacitated.";
            return null;
        }

        /// <summary>
        /// Core-only task label for the UI roster. It resolves scavenging
        /// progress before generic survivor state so an active saved mission
        /// remains intelligible after load.
        /// </summary>
        private string GetScavengeDispatchTaskLabel(Survivor survivor)
        {
            if (survivor == null) return "—";
            if (!survivor.IsAlive) return "dead";

            var active = ScavengingSystem != null ? ScavengingSystem.ActiveMissions : null;
            if (active != null)
            {
                for (int i = 0; i < active.Count; i++)
                {
                    var mission = active[i];
                    if (mission == null || mission.SurvivorId != survivor.Id) continue;
                    string destination = string.IsNullOrEmpty(mission.LocationName)
                        ? mission.LocationId
                        : mission.LocationName;
                    return $"{mission.Kind.ToString().ToLowerInvariant()} {destination} — "
                        + $"{mission.HoursRemaining:0.#}h left";
                }
            }

            if ((ExpeditionSystem != null && ExpeditionSystem.IsOnExpedition(survivor.Id))
                || survivor.IsOnExpedition)
                return "on expedition";

            return survivor.State.ToString().ToLowerInvariant();
        }

        /// <summary>
        /// Formats the active, saveable location missions for the player-facing
        /// board without exposing Core mission types to the UI assembly.
        /// </summary>
        private string BuildScavengeMissionRoster()
        {
            var active = ScavengingSystem != null ? ScavengingSystem.ActiveMissions : null;
            if (active == null || active.Count == 0) return "ACTIVE MISSIONS: none.";

            var roster = new System.Text.StringBuilder("ACTIVE MISSIONS:");
            for (int i = 0; i < active.Count; i++)
            {
                var mission = active[i];
                if (mission == null) continue;

                string operative = mission.Survivor != null
                    && !string.IsNullOrEmpty(mission.Survivor.DisplayName)
                    ? mission.Survivor.DisplayName
                    : mission.SurvivorId;
                string destination = string.IsNullOrEmpty(mission.LocationName)
                    ? mission.LocationId
                    : mission.LocationName;
                roster.Append("\n- ").Append(operative)
                    .Append(" · ").Append(mission.Kind.ToString().ToLowerInvariant())
                    .Append(" · ").Append(destination)
                    .Append(" · ").Append(mission.HoursRemaining.ToString("0.#"))
                    .Append("h remaining");
            }
            return roster.ToString();
        }

        /// <summary>
        /// Builds a player-facing mission preflight from fog-of-war knowledge and
        /// the current shared field equipment. It never reads TrueRad for display:
        /// unknown or stale map data remains unknown or uncertain here as well.
        /// </summary>
        private string BuildScavengePreflightSummary(Survivor survivor, LocationDefinitionSO location)
        {
            if (location == null) return "PREFLIGHT: no destination selected.";

            bool hasWorkingGeiger = Inventory != null && Inventory.HasWorkingGeiger();
            int day = TimeSystem != null ? TimeSystem.CurrentDay : 0;
            MapTilePlayerView view = KnowledgeMap != null
                ? KnowledgeMap.GetPlayerView(location.id, day, hasWorkingGeiger)
                : new MapTilePlayerView { LocationId = location.id, DisplayedRad = float.NaN, IsUnknown = true };

            float protection = Inventory != null ? Inventory.GetEquippedProtection() : 0f;
            var preflight = new System.Text.StringBuilder("--- PREFLIGHT ---");
            preflight.Append("\nRETURN: expected in ").Append(location.travelHours.ToString("0.#"))
                .Append("h when the mission clock closes.");

            if (view.IsDark || view.IsUnknown || float.IsNaN(view.DisplayedRad))
            {
                preflight.Append("\nPROJECTED DOSE: unknown — field estimate unavailable.");
            }
            else
            {
                // The preview stays conservative: iodine is time-limited and may
                // expire before the return clock closes, so it is shown below but
                // never discounted from this whole-mission estimate.
                float estimatePerHour = Mathf.Max(0f, view.DisplayedRad - protection);
                float estimateDose = estimatePerHour * Mathf.Max(0f, location.travelHours);
                preflight.Append("\nPROJECTED DOSE: ~").Append(estimateDose.ToString("0.#"))
                    .Append(" over ").Append(location.travelHours.ToString("0.#")).Append("h");
                if (view.IsUnreliable)
                    preflight.Append(" — estimate uncertain.");
                else
                    preflight.Append(" — surveyed estimate.");
            }

            preflight.Append("\nGEAR CHECK: face ").Append(GetScavengeGearLabel(EquipSlot.Face))
                .Append(" · body ").Append(GetScavengeGearLabel(EquipSlot.Body))
                .Append(" · shielding ").Append(protection.ToString("0.#")).Append(" rad/h.");
            preflight.Append("\nMETER: ").Append(hasWorkingGeiger ? "working geiger." : "no working geiger — map dark.");
            preflight.Append("\nIODINE: ").Append(survivor != null && survivor.HasRadResistance
                ? "active — time-limited; estimate remains conservative."
                : "not active.");
            if (protection <= 0f)
                preflight.Append("\nCAUTION: no field shielding is currently equipped.");
            return preflight.ToString();
        }

        /// <summary>
        /// Player-facing loot preview. Reads the exact shared curve
        /// LocationScavengingSystem.RollLoot rolls against, so this can never
        /// drift from what a mission actually resolves.
        /// </summary>
        private string GetScavengeLootPreview(float dangerLevel)
        {
            LocationScavengingSystem.PreviewLootChances(dangerLevel, out int searchSlots, out float recoveryChancePercent);
            return searchSlots + " search slot" + (searchSlots == 1 ? string.Empty : "s")
                + " · " + recoveryChancePercent.ToString("0") + "% baseline recovery each.";
        }

        private string GetScavengeGearLabel(EquipSlot slot)
        {
            var equipped = Inventory != null ? Inventory.GetEquipped(slot) : null;
            if (equipped?.Item == null) return "none";
            return string.IsNullOrEmpty(equipped.Item.displayName)
                ? equipped.Item.id
                : equipped.Item.displayName;
        }

        /// <summary>Turns save-safe after-action facts into concise player-facing terminal text.</summary>
        private string BuildScavengeAfterActionSummary(ScavengeAfterActionReport report)
        {
            if (report == null) return "RETURNED: report unavailable.";

            string location = string.IsNullOrEmpty(report.LocationName) ? report.LocationId : report.LocationName;
            var summary = new System.Text.StringBuilder("RETURNED: ").Append(location).Append(" — ");
            if (report.RecoveredItemIds == null || report.RecoveredItemIds.Count == 0)
                summary.Append("no items reached the field bag.");
            else
                summary.Append("bagged ").Append(report.RecoveredItemIds.Count).Append(" item")
                    .Append(report.RecoveredItemIds.Count == 1 ? "." : "s.");

            int overflowCount = report.OverflowItemIds != null ? report.OverflowItemIds.Count : 0;
            if (overflowCount > 0)
                summary.Append(" ").Append(overflowCount).Append(" moved to the bunker overflow crate.");

            int unsecuredCount = report.UnsecuredItemIds != null ? report.UnsecuredItemIds.Count : 0;
            if (unsecuredCount > 0)
                summary.Append(" WARNING: ").Append(unsecuredCount).Append(" item")
                    .Append(unsecuredCount == 1 ? " could not be secured." : "s could not be secured.");

            summary.Append(" Dose +").Append(report.RadiationGained.ToString("0.#"));
            summary.Append(". Gear: face ").Append(FormatScavengeGearCondition(report.FaceGear));
            summary.Append(" · body ").Append(FormatScavengeGearCondition(report.BodyGear)).Append(".");
            return summary.ToString();
        }

        private static string FormatScavengeGearCondition(ScavengeGearCondition gear)
        {
            if (gear == null || string.IsNullOrEmpty(gear.ItemId)) return "none";
            string label = string.IsNullOrEmpty(gear.ItemName) ? gear.ItemId : gear.ItemName;
            return gear.DurabilityPercent < 0f
                ? label
                : label + " " + gear.DurabilityPercent.ToString("0") + "%";
        }

        /// <summary>Open or close the location scavenging dispatch board ([G]).</summary>
        public void ToggleScavengeDispatch()
        {
            _hud?.OverflowCrateHUD?.Close();
            _hud?.FieldGearLoadoutHUD?.Close();
            _hud?.BunkerRationingHUD?.Close();
            _hud?.WaterPurificationHUD?.Close();
            _hud?.AirHeatManagementHUD?.Close();
            _hud?.BunkerMaintenanceHUD?.Close();
            _hud?.SurvivorTaskBoardHUD?.Close();
            _hud?.ScavengeDispatchHUD?.Toggle();
        }

        public bool SelectNextScavengeLocation()
        {
            return _hud?.ScavengeDispatchHUD != null
                && _hud.ScavengeDispatchHUD.SelectNext();
        }

        public bool SelectPreviousScavengeLocation()
        {
            return _hud?.ScavengeDispatchHUD != null
                && _hud.ScavengeDispatchHUD.SelectPrevious();
        }

        public bool SelectNextScavengeSurvivor()
        {
            return _hud?.ScavengeDispatchHUD != null
                && _hud.ScavengeDispatchHUD.SelectNextSurvivor();
        }

        public bool SelectPreviousScavengeSurvivor()
        {
            return _hud?.ScavengeDispatchHUD != null
                && _hud.ScavengeDispatchHUD.SelectPreviousSurvivor();
        }

        public bool DispatchSelectedScavenge()
        {
            return _hud?.ScavengeDispatchHUD != null
                && _hud.ScavengeDispatchHUD.DispatchSelected();
        }

        /// <summary>
        /// Fulfil the UI request through the guarded Core API, then push only
        /// presentation data back to the board. The mission-start event writes
        /// the accepted report; this path writes failures immediately. The
        /// return value is the dispatch's real outcome — DispatchSelected()
        /// forwards it as-is, so callers can no longer read "true" from a
        /// request Core actually rejected.
        /// </summary>
        private bool HandleScavengeDispatchRequested(Survivor survivor, LocationDefinitionSO location)
        {
            var board = _hud != null ? _hud.ScavengeDispatchHUD : null;
            if (location == null)
            {
                board?.ReportDispatchHeld("No scavenge location available.");
                return false;
            }

            string reason = GetScavengeDispatchBlockReason(survivor);
            if (!string.IsNullOrEmpty(reason))
            {
                board?.ReportDispatchHeld(reason);
                return false;
            }

            if (IceRoadSystem != null && IceRoadSystem.IsTravelBlocked(location.id))
            {
                board?.ReportDispatchHeld(IceRoadSystem.IsUnlocked
                    ? "The Cut is dark. Wait for a lit window."
                    : "The estuary is a rumour. No road yet.");
                return false;
            }

            if (!StartScavengeMission(survivor, location))
            {
                board?.ReportDispatchHeld("Dispatch rejected by the mission system.");
                return false;
            }
            return true;
        }
    }
}
