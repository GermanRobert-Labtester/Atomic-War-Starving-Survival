// SPDX-License-Identifier: MIT
// ASHFALL Core: machine tell → audio condition sync (Plan 29 consumer side,
// §29B.21 audio hooks; semantics in docs/shelter/PLAN29_AUDIO_HOOKS.md).
//
// The tell catalog stays a pure projection (§1.2): this sync diffs the quirk
// evaluation against the live AudioConditionSystem and starts/stops one
// condition per fired quirk that carries an authored audio_cue. Audio state
// changes therefore fire on threshold transitions (§14), never per frame:
// the caller re-runs Apply on the day cadence (or after maintenance), and
// the condition system's already_active guard makes repeated applies no-ops.
//
// Presentation only — persists nothing of its own. The loop/one-shot meaning
// belongs to the host cue definitions, so the caller injects that knowledge
// through the isLoopingCue resolver (null → treat tells as sustained loops).
// Deterministic: authored machine/quirk order, no RNG, no dictionary-order
// exposure.
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Shelter
{
    /// <summary>
    /// Diff-syncs machine quirk tells into the Core audio condition system.
    /// Stateless between calls: all memory of what plays lives in the
    /// AudioConditionSystem the caller owns.
    /// </summary>
    public static class MachineTellAudioSync
    {
        /// <summary>
        /// Condition-id namespace owned by this sync: quirk ids are already
        /// authored machine_quirk_* — the prefix doubles as the ownership marker
        /// the stop pass scans for, so it never collides with other condition users.
        /// </summary>
        public const string ConditionPrefix = "machine_quirk_";

        /// <summary>Outcome of one Apply — deterministic authored order.</summary>
        public sealed class Outcome
        {
            /// <summary>Quirk ids whose condition started on this apply.</summary>
            public readonly List<string> Started = new List<string>();
            /// <summary>Quirk ids whose condition stopped on this apply.</summary>
            public readonly List<string> Stopped = new List<string>();
            /// <summary>Machine-quirk conditions active after the apply.</summary>
            public int ActiveTotal;
            /// <summary>True when nothing changed (steady state).</summary>
            public bool Clean => Started.Count == 0 && Stopped.Count == 0;
        }

        /// <summary>
        /// AudioConditionSystem bus for a machine's tells: filtration and exhaust
        /// plant ride the ventilation bus, the generator rides its own bus, every
        /// other plant room (foundry, boiler, still, airlock) rides ambient.
        /// The Godot-side playback bus still comes from each cue's own definition.
        /// </summary>
        public static string BusForMachine(string machineId)
        {
            switch (MachineConditionKeys.FamilyOf(machineId))
            {
                case "hepa":
                case "ventilation":
                    return "ventilation";
                case "generator":
                case "power":
                    return "generator";
                default:
                    return "ambient";
            }
        }

        /// <summary>
        /// One deterministic sync pass. Fired quirks with a non-empty audio_cue
        /// become active conditions; machine-quirk conditions whose tell no
        /// longer fires are stopped. Quirks without an audio_cue stay text-only
        /// (the muted game keeps every tell readable, §29B.22).
        /// </summary>
        /// <param name="catalog">Machine identity + quirk catalog (read-only).</param>
        /// <param name="readings">Condition snapshot from the owning systems.</param>
        /// <param name="audio">The Core audio condition authority.</param>
        /// <param name="isLoopingCue">
        /// Host cue knowledge: true when the cue sustains as a loop, false for a
        /// one-shot on the crossing. Null treats every tell as sustained.
        /// </param>
        public static Outcome Apply(
            ShelterMachineTellCatalog catalog,
            MachineConditionReadings readings,
            AudioConditionSystem audio,
            Func<string, bool>? isLoopingCue = null)
        {
            var outcome = new Outcome();
            if (catalog == null || readings == null || audio == null) return outcome;

            var wanted = new HashSet<string>(StringComparer.Ordinal);
            for (int m = 0; m < catalog.MachineCount; m++)
            {
                var machine = catalog.Machines[m];
                string bus = BusForMachine(machine.id);
                var quirks = catalog.EvaluateQuirks(machine.id, readings);
                for (int q = 0; q < quirks.Count; q++)
                {
                    var quirk = quirks[q];
                    if (string.IsNullOrWhiteSpace(quirk.audio_cue)) continue;
                    if (!quirk.id.StartsWith(ConditionPrefix, StringComparison.Ordinal)) continue;

                    wanted.Add(quirk.id);
                    bool looping = isLoopingCue?.Invoke(quirk.audio_cue) ?? true;
                    var result = audio.StartCondition(quirk.id, bus, quirk.audio_cue, 1f, looping);
                    if (result.IsSuccess) outcome.Started.Add(quirk.id);
                }
            }

            var conditions = audio.State.activeConditions;
            for (int i = 0; i < conditions.Count; i++)
            {
                var condition = conditions[i];
                if (!condition.isActive) continue;
                if (!condition.conditionId.StartsWith(ConditionPrefix, StringComparison.Ordinal)) continue;
                if (wanted.Contains(condition.conditionId)) continue;

                var result = audio.StopCondition(condition.conditionId);
                if (result.IsSuccess)
                    outcome.Stopped.Add(condition.conditionId);
            }

            audio.ClearStopped();

            for (int i = 0; i < audio.State.activeConditions.Count; i++)
            {
                var condition = audio.State.activeConditions[i];
                if (condition.isActive &&
                    condition.conditionId.StartsWith(ConditionPrefix, StringComparison.Ordinal))
                    outcome.ActiveTotal++;
            }
            return outcome;
        }
    }
}
