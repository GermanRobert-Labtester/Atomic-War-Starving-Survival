// SPDX-License-Identifier: MIT
using Ashfall.Core;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Shared player-facing wording for typed <see cref="ActionResult"/>
    /// refusals.
    ///
    /// Core owns the stable <see cref="ActionResult.FailureCode"/>; UI owns the
    /// sentence the player reads (see <c>docs/ACTION_RESULT_SURFACING_MATRIX.md</c>).
    /// Panels that bind a Core system directly (no host session with a
    /// <c>LastEvent</c> surface) use this formatter instead of duplicating the
    /// same switch in every panel. Unknown codes fall back to a readable
    /// de-underscored form so no refusal is ever silently dropped.
    /// </summary>
    internal static class ActionRefusalText
    {
        /// <summary>
        /// Render a refusal line, or the empty string for a success so callers
        /// can assign unconditionally.
        /// </summary>
        public static string Line(ActionResult result, string prefix = "REFUSED")
            => result.IsSuccess ? string.Empty : $"{prefix} — {Describe(result.FailureCode)}";

        /// <summary>Player-facing sentence for a stable Core failure code.</summary>
        public static string Describe(string? code)
        {
            if (string.IsNullOrEmpty(code)) return "the action was refused.";
            // P003 — the tutorial-ordering gate carries the pending stage after
            // the ':' so the refusal can name the next onboarding step.
            if (code.StartsWith("tutorial_step_locked", System.StringComparison.Ordinal))
            {
                string stage = code.Length > "tutorial_step_locked:".Length
                    ? code.Substring("tutorial_step_locked:".Length).Replace('_', ' ').ToUpperInvariant()
                    : string.Empty;
                return stage.Length > 0
                    ? $"finish the earlier onboarding steps first — next: {stage}."
                    : "finish the earlier onboarding steps first.";
            }
            return code switch
            {
                // Generic preflight / wiring
                "unbound" => "this station is not operational yet.",
                "no_catalog" => "the catalog for this station is missing.",
                "no_inventory" => "storage is unavailable.",
                "insufficient_materials" or "missing_materials" or "missing_inputs"
                    => "not enough materials on hand.",
                "inventory_full" => "storage is full.",
                "transaction_failed" or "commit_failed" => "the transaction could not be committed.",
                "stale_preview" => "planning data went stale. Retry.",
                "busy" or "not_idle" or "workshop_busy" or "lab_busy" => "the station is already busy.",

                // Workshop / relic restoration
                "cannot_start" => "the job cannot be started.",
                "not_active" => "there is no active job.",
                "not_ready" => "the job is not ready to collect.",
                "workshop_idle" => "the workbench is idle.",
                "already_complete" => "this relic has already been restored.",
                "already_salvaged" => "this relic has already been salvaged.",
                "missing_components" or "missing_source_item" => "the required relic components are missing.",
                "no_research_unlock" => "the restoration technique is not researched.",

                // Pharma lab
                "not_processing" => "no compounding run is in progress.",

                // Decontamination
                "empty_queue" => "the decon queue is empty.",
                "empty_tank" => "the decon tank is empty.",
                "no_water" => "there is no clean water for the cycle.",
                "no_soap" => "there is no decon soap on hand.",
                "no_chelator" => "there is no chelator for this cycle.",
                "no_filter" => "no replacement filter is fitted.",
                "filter_installed" => "a filter is already fitted.",
                "filter_exhausted" => "the filter is exhausted.",
                "active_case" => "a case is already in the chamber.",
                "already_queued" => "this gear is already queued.",
                "not_in_progress" => "the cycle is not in progress.",
                "survivor_busy" => "that survivor is busy elsewhere.",
                "unknown_protocol" => "the decon protocol is unknown.",
                "invalid_gear" or "gear_not_found" => "that gear cannot be decontaminated.",
                "no_active" => "there is no active case.",

                // Thermal / boiler
                "already_burning" => "the boiler is already lit.",
                "already_burst" or "not_burst" => "the pipe state does not allow that.",
                "insufficient_fuel" => "there is not enough fuel.",
                "no_blowtorch" => "a blowtorch is required.",
                "no_radiator" or "room_exists" or "pipe_exists" => "the target fixture does not exist.",
                "not_frozen" => "the room is not frozen.",
                "stove_already_exists" => "a stove is already installed there.",
                "storm_in_progress" => "seal the shelter before the storm, not during it.",

                // Sump / flooding
                "no_pump" => "no pump is installed at this node.",
                "pump_exists" => "a pump is already installed at this node.",
                "no_sludge" or "no_solids" or "no_cake" or "no_tailings" => "there is nothing to process yet.",
                "media_worn" => "the centrifuge media needs replacing.",
                "node_exists" => "this sump node already exists.",

                // Wildlife trapping
                "unknown_trap" => "that trap type is unknown.",
                "trap_active" => "a working trap already occupies this site.",
                "repair_unavailable" => "this trap cannot be repaired yet.",
                "nothing_to_butcher" or "no_catch" => "there is no catch to butcher.",
                "no_hide" => "there is no usable hide.",

                // Roster / relations / morale
                "insufficient_funds" => "the shelter cannot cover the cost.",
                "not_available" => "that offer is no longer available.",
                "offer_exists" => "an offer already exists for this candidate.",
                "already_resolved" => "this conflict is already resolved.",
                "not_owned" => "that record is not in the collection.",
                "not_playing" => "no record is playing.",
                "not_allowed" => "that schedule change is not permitted.",

                // Airlock / defense
                "incident_active" => "a visitor decision is already pending.",
                "no_incident" => "there is no pending visitor incident.",
                "door_breached" => "the blast door is breached.",
                "invalid_decision" or "invalid_door_state" or "invalid_visitor" or "invalid_amount"
                    => "that action is not valid right now.",
                "no_perimeter" => "the perimeter system is offline.",

                // Cloud seeding
                "already_installed" => "the dispenser is already installed.",
                "no_intelligence" => "there is no weather intelligence station.",
                "cooldown" => "the seeding rig is still recharging.",

                _ => code.Replace('_', ' ')
            };
        }
    }
}
