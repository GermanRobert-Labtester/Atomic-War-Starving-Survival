// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 42 / C2[18] — Survivor voice line host adapter.
//
// Authority boundaries (deliberate — three Core voice authorities exist and
// this adapter composes them without creating a fourth):
//   * SurvivorVoiceSystem owns the authored catalog (survivor_voice_lines.json),
//     day-granularity cooldowns, register weighting, utterance history and the
//     save state. It is the single selection authority.
//   * VoiceLineDispatchCoordinator owns playback arbitration only: the audio
//     bus's per-speaker/global cooldowns (ticks), priority preemption and the
//     subtitle payload. It never selects.
//   * VoiceLineSelectionEngine remains the pure candidate evaluator for explicit
//     candidate lists. The host does not run it over the authored catalog, so
//     no second selection path competes with the catalog authority.
//
// Order of operations is therefore fixed: select (catalog) -> arbitrate (bus).
// A bus rejection drops the audio only; the line was still uttered in world
// time, which is why the catalog cooldown legitimately stands.
//
// The trigger events come from canonical producers in the host (season owner,
// fate owner, rationing owner, radiation owner) — the adapter invents no barks.
// Delivery is a journal entitlement ("<survivor> says: ..."), plus the
// coordinator's subtitle payload for the presentation layer.
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Save;
using Ashfall.Core.Voice;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host persistence projection for Plan 42: the catalog system's state plus
    /// the dispatch bus state under one registered section.
    /// </summary>
    public sealed class SurvivorVoiceSaveState
    {
        public int schema_version { get; set; } = 1;
        public SurvivorVoiceState lines { get; set; } = new();
        public VoiceDispatchCoordinatorSaveState dispatch { get; set; } = new();
        public int total_utterances { get; set; }
        public Dictionary<string, float> last_radiation_dose { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    }

    public sealed class SurvivorVoiceHostSession : HostSessionBase
    {
        public SurvivorVoiceSystem Voice { get; }
        public VoiceLineDispatchCoordinator Dispatch { get; }

        /// <summary>Total catalog utterances delivered through the bus.</summary>
        public int TotalUtterances { get; private set; }

        /// <summary>
        /// Last observed radiation dose per survivor (read model) so a spike can
        /// be detected from the canonical dose owner's event.
        /// </summary>
        private readonly Dictionary<string, float> _lastRadiationDose = new(StringComparer.OrdinalIgnoreCase);

        public SurvivorVoiceHostSession(SurvivorVoiceSystem voice, VoiceLineDispatchCoordinator dispatch)
        {
            Voice = voice ?? throw new ArgumentNullException(nameof(voice));
            Dispatch = dispatch ?? throw new ArgumentNullException(nameof(dispatch));
        }

        /// <summary>
        /// Select a line from the authored catalog and arbitrate playback on the
        /// dispatch bus. Returns the delivered payload, or null when no line was
        /// selected or the bus was busy.
        /// </summary>
        public VoiceLinePayload? Speak(
            string trigger,
            int day,
            string speakerId,
            float morale,
            float fatigue,
            string? preferredRegister = null,
            string profession = "any",
            VoiceLinePriority priority = VoiceLinePriority.AmbientIdle,
            int durationTicks = 5)
        {
            if (string.IsNullOrWhiteSpace(trigger) || string.IsNullOrWhiteSpace(speakerId)) return null;

            var context = new SurvivorSpeechContext
            {
                SurvivorId = speakerId,
                Profession = string.IsNullOrWhiteSpace(profession) ? "any" : profession,
                Morale = morale,
                Fatigue = fatigue,
                PreferredRegister = preferredRegister ?? string.Empty
            };

            var rng = _campaignRngProvider?.Invoke(day);
            if (!Voice.TrySelectVoiceLine(context, trigger, day, rng, out var payload))
            {
                return null;
            }

            // Playback arbitration on the bus: the catalog authority already
            // committed the utterance to world time above, this only decides
            // whether the bus can carry it now.
            var selection = new VoiceLineSelectionResult(
                true, payload.LineId, payload.Register, payload.TextKey, string.Empty, payload.SpeakerSurvivorId);
            var result = Dispatch.TryDispatch(selection, priority, TickForDay(day), durationTicks);
            if (!result.Dispatched) return null;

            TotalUtterances++;
            RaiseStateChanged();
            return payload;
        }

        /// <summary>Deterministic playback tick for a campaign day (bus clock).</summary>
        internal static int TickForDay(int day) => Math.Max(0, day) * 100;

        private Func<int, ISeededRng?>? _campaignRngProvider;

        internal void SetCampaignRngProvider(Func<int, ISeededRng?> provider)
            => _campaignRngProvider = provider;

        /// <summary>
        /// Track a survivor's dose so the host can raise the radiation spike
        /// trigger from the canonical dose owner's event.
        /// </summary>
        public bool ObserveRadiationDose(string survivorId, float newDose, float spikeThreshold)
        {
            if (string.IsNullOrWhiteSpace(survivorId)) return false;
            float previous = _lastRadiationDose.TryGetValue(survivorId, out float prior) ? prior : newDose;
            _lastRadiationDose[survivorId] = newDose;
            return newDose - previous >= spikeThreshold;
        }

        public void ObserveRadiationDoseWithoutTrigger(string survivorId, float newDose)
        {
            if (string.IsNullOrWhiteSpace(survivorId)) return;
            _lastRadiationDose[survivorId] = newDose;
        }

        /// <summary>Track the rationing owner's tier so a cut is a real reduction.</summary>
        public bool IsRationCut(string resourceId, RationingTier tier)
        {
            if (string.IsNullOrWhiteSpace(resourceId)) return false;
            bool known = _lastRationTier.TryGetValue(resourceId, out var prior);
            _lastRationTier[resourceId] = tier;
            return known && tier < prior;
        }

        private readonly Dictionary<string, RationingTier> _lastRationTier = new(StringComparer.OrdinalIgnoreCase);

        public SurvivorVoiceSaveState CaptureState()
        {
            var state = new SurvivorVoiceSaveState
            {
                schema_version = 1,
                lines = Voice.CaptureState(),
                dispatch = Dispatch.CaptureState(),
                total_utterances = TotalUtterances
            };
            foreach (var pair in _lastRadiationDose) state.last_radiation_dose[pair.Key] = pair.Value;
            return state;
        }

        public void RestoreState(SurvivorVoiceSaveState? state)
        {
            if (state == null) return;
            if (state.lines != null) Voice.RestoreState(state.lines);
            if (state.dispatch != null) Dispatch.RestoreState(state.dispatch);
            TotalUtterances = Math.Max(0, state.total_utterances);
            _lastRadiationDose.Clear();
            if (state.last_radiation_dose != null)
            {
                foreach (var pair in state.last_radiation_dose) _lastRadiationDose[pair.Key] = pair.Value;
            }
            RaiseStateChanged();
        }

        /// <summary>Load the authored catalog through the plan's data owner.</summary>
        public static string LoadCatalogJson(string dataDir)
        {
            if (string.IsNullOrWhiteSpace(dataDir)) return "{}";
            string path = Path.Combine(dataDir, "survivor_voice_lines.json");
            return File.Exists(path) ? File.ReadAllText(path) : "{}";
        }

        public override void Save()
        {
            if (!IsDirty) return;
            if (SurvivorVoiceSaveStore.TrySave(CaptureState()))
                base.Save();
        }
    }

    public static class SurvivorVoiceSaveStore
    {
        public const string FileName = "survivor_voice_save.json";
        public const string SectionName = "survivor_voice";

        private static readonly SaveStore<SurvivorVoiceSaveState> s_store =
            SaveStoreHub.Checksummed<SurvivorVoiceSaveState>(FileName, nameof(SurvivorVoiceSaveStore));

        public static bool TrySave(SurvivorVoiceSaveState state) => s_store.TrySave(state);
        public static SurvivorVoiceSaveState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(SurvivorVoiceSaveState state) => s_store.CapturePersisted(state);
        public static SurvivorVoiceSaveState? TryRestore(string json) => s_store.RestoreBare(json);
    }
}
