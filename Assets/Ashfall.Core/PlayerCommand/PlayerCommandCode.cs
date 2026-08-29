// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core.PlayerCommand
{
    /// <summary>
    /// Stable snake_case command codes for every significant player-facing action.
    /// Codes are grouped by domain and must never change once published.
    /// </summary>
    public static class PlayerCommandCode
    {
        // ── Crafting ──────────────────────────────────────────────────
        public const string CraftStart = "craft.start";
        public const string CraftCancel = "craft.cancel";

        // ── Expeditions ──────────────────────────────────────────────
        public const string ExpeditionDispatch = "expedition.dispatch";
        public const string ExpeditionPushLuck = "expedition.push_luck";
        public const string ExpeditionRetreat = "expedition.retreat";
        public const string ExpeditionEnterCamp = "expedition.enter_camp";
        public const string ExpeditionReserveSupplies = "expedition.reserve_supplies";
        public const string ExpeditionCampTick = "expedition.camp_tick";
        public const string ExpeditionResolveEncounter = "expedition.resolve_encounter";
        public const string ExpeditionBreakCamp = "expedition.break_camp";

        // ── Trade ─────────────────────────────────────────────────────
        public const string TradeConfirm = "trade.confirm";
        public const string TradeDemandParley = "trade.demand_parley";

        // ── Treatment ─────────────────────────────────────────────────
        public const string TreatmentStart = "treatment.start";
        public const string TreatmentTick = "treatment.tick";
        public const string TreatmentCancel = "treatment.cancel";
        public const string TreatmentReplaceFilter = "treatment.replace_filter";

        // ── Shelter Repair ────────────────────────────────────────────
        public const string RepairPipe = "repair.pipe";
        public const string RepairDoor = "repair.door";
        public const string RepairVehicle = "repair.vehicle";
        public const string RepairBerth = "repair.berth";
        public const string RepairWeapon = "repair.weapon";

        // ── Assignments ───────────────────────────────────────────────
        public const string AssignRole = "assign.role";
        public const string AssignBed = "assign.bed";
        public const string AssignCaregiver = "assign.caregiver";
        public const string AssignWatch = "assign.watch";
        public const string AssignWorkers = "assign.workers";

        // ── Greenhouse ────────────────────────────────────────────────
        public const string GreenhouseTreatBlight = "greenhouse.treat_blight";
    }
}
