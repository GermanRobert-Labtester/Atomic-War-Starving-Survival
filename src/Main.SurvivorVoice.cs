// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 42 / C2[18] — Survivor voice: authored barks from canonical triggers.
//
// Host composition for Ashfall.Core.Voice:
//   Setup   — load the authored catalog (survivor_voice_lines.json), restore the
//             registered "survivor_voice" section, bind the campaign RNG.
//   Triggers— canonical producers only, one per authored catalog trigger:
//               survivor_perished -> fate owner
//               ration_cut        -> economy/rationing owner
//               radiation_spike   -> survivors/radiation owner
//               season_changed    -> campaign calendar owner
//             ("visitor_arrived" has no reachable host producer while the
//             visitor authority itself is still an orphan; the seam stays
//             unbound instead of inventing a visitor.)
//   Deliver — journal entitlement with the authored English text, and the
//             dispatch bus subtitle payload for the presentation layer.
//   Save    — "survivor_voice" section via SaveSectionRegistry + SaveStore.
// ============================================================================
using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Random;
using Ashfall.Core.Economy;
using Ashfall.Core.Voice;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        /// <summary>Acute dose delta that counts as a radiation spike bark.</summary>
        private const float VoiceRadiationSpikeThreshold = 10f;

        private SurvivorVoiceHostSession? _survivorVoice;
        private bool _survivorVoiceDirty;

        public SurvivorVoiceHostSession? SurvivorVoice => _survivorVoice;

        private SurvivorVoiceHostSession EnsureSurvivorVoice()
        {
            if (_survivorVoice != null) return _survivorVoice;

            var system = new SurvivorVoiceSystem();
            // Catalog authority: the authored JSON is the only line source.
            system.LoadCatalog(SurvivorVoiceHostSession.LoadCatalogJson(_dataDir));

            var dispatch = new VoiceLineDispatchCoordinator();
            var session = new SurvivorVoiceHostSession(system, dispatch);
            session.RestoreState(SurvivorVoiceSaveStore.TryLoad());
            session.StateChanged += () => _survivorVoiceDirty = true;
            session.SetCampaignRngProvider(day =>
                _campaignDay != null ? _campaignDay.Rng.Fork(CampaignStreamIds.Social, day) : null);

            // Delivery: both seams record facts; the speech enters the journal
            // through the canonical journal owner like any other entitlement.
            system.VoiceLineDeliveredSink = payload => DeliverSurvivorVoiceLine(payload, "catalog");
            system.VoiceBarkAudioSeam = (survivorId, lineId) => DeliverSurvivorVoiceBark(survivorId, lineId);
            dispatch.OnVoiceDispatched += _ => MarkSurvivorVoiceDirty();

            BindSurvivorVoiceTriggers();
            _survivorVoice = session;
            return _survivorVoice;
        }

        private void BindSurvivorVoiceTriggers()
        {
            // Season owner: the calendar is the single season authority.
            if (_campaignDay?.Calendar != null)
            {
                _campaignDay.Calendar.OnSeasonChanged += TriggerSurvivorVoiceSeasonChanged;
            }

            // Radiation owner: dose events are the only spike source.
            if (_survivors?.Radiation != null)
            {
                _survivors.Radiation.OnDoseChanged += (state, dose) =>
                {
                    if (state != null) TriggerSurvivorVoiceRadiationSpike(state.Id, dose);
                };
            }
        }

        private void SetupSurvivorVoice()
        {
            EnsureSurvivorVoice();
        }

        private void SaveSurvivorVoice()
        {
            if (_survivorVoice == null) return;
            CaptureSection(
                SurvivorVoiceSaveStore.SectionName,
                SurvivorVoiceSaveStore.TryCapturePersisted(_survivorVoice.CaptureState()));
            _survivorVoiceDirty = false;
        }

        private void FlushSurvivorVoiceIfDirty()
        {
            if (_survivorVoiceDirty) SaveSurvivorVoice();
        }

        private void ResetSurvivorVoice()
        {
            _survivorVoice?.Dispose();
            _survivorVoice = null;
            _survivorVoiceDirty = false;
        }

        private void MarkSurvivorVoiceDirty()
        {
            _survivorVoiceDirty = true;
        }

        private void DeliverSurvivorVoiceLine(VoiceLinePayload payload, string origin)
        {
            if (payload == null) return;
            string speaker = FormatSurvivorName(payload.SpeakerSurvivorId);
            _journal?.TryAddRawEntry(
                "voice_line",
                $"{speaker} says: \"{payload.TextEnglish}\"",
                null!, Math.Max(1, payload.UtteredDay));
            _survivorVoiceDirty = true;
        }

        private void DeliverSurvivorVoiceBark(string survivorId, string lineId)
        {
            // Audio/subtitle presentation seam: the bark is announced to the
            // audio owner; the subtitle payload comes from the dispatch bus.
            MarkSurvivorVoiceDirty();
        }

        // ── Canonical triggers ─────────────────────────────────────────────

        private void TriggerSurvivorVoiceSeasonChanged(string previousSeasonId, string newSeasonId)
        {
            var speaker = PickLivingSurvivorSpeaker();
            if (speaker.Id == null) return;
            var payload = _survivorVoice?.Speak(
                "season_changed", Math.Max(1, _simDay), speaker.Id,
                speaker.Morale, speaker.Fatigue, speaker.Register, speaker.Profession,
                VoiceLinePriority.AmbientIdle);
            MarkIfSpoken(payload);
        }

        private void TriggerSurvivorVoicePerished(string fallenSurvivorId)
        {
            var speaker = PickLivingSurvivorSpeaker(excludeId: fallenSurvivorId);
            if (speaker.Id == null) return;
            var payload = _survivorVoice?.Speak(
                "survivor_perished", Math.Max(1, _simDay), speaker.Id,
                speaker.Morale, speaker.Fatigue, speaker.Register, speaker.Profession,
                VoiceLinePriority.CrisisBark);
            MarkIfSpoken(payload);
        }

        private void TriggerSurvivorVoiceRationCut(RationTarget target)
        {
            if (target == null) return;
            if (_survivorVoice != null && !_survivorVoice.IsRationCut(target.ResourceId, target.Tier)) return;
            var speaker = PickLivingSurvivorSpeaker();
            if (speaker.Id == null) return;
            var payload = _survivorVoice?.Speak(
                "ration_cut", Math.Max(1, _simDay), speaker.Id,
                speaker.Morale, speaker.Fatigue, speaker.Register, speaker.Profession,
                VoiceLinePriority.NeedsWarning);
            MarkIfSpoken(payload);
        }

        /// <summary>Raised from the radiation owner's dose event.</summary>
        private void TriggerSurvivorVoiceRadiationSpike(string survivorId, float newDose)
        {
            if (_survivorVoice == null) return;
            if (!_survivorVoice.ObserveRadiationDose(survivorId, newDose, VoiceRadiationSpikeThreshold)) return;

            var speaker = PickLivingSurvivorSpeaker(excludeId: survivorId);
            if (speaker.Id == null)
            {
                speaker = PickLivingSurvivorSpeaker();
                if (speaker.Id == null) return;
            }
            var payload = _survivorVoice.Speak(
                "radiation_spike", Math.Max(1, _simDay), speaker.Id,
                speaker.Morale, speaker.Fatigue, speaker.Register, speaker.Profession,
                VoiceLinePriority.NeedsWarning);
            MarkIfSpoken(payload);
        }

        private void MarkIfSpoken(VoiceLinePayload? payload)
        {
            if (payload != null) _survivorVoiceDirty = true;
        }

        private readonly struct LivingSpeaker
        {
            public LivingSpeaker(string? id, float morale, float fatigue, string register, string profession)
            {
                Id = id;
                Morale = morale;
                Fatigue = fatigue;
                Register = register;
                Profession = profession;
            }

            public string? Id { get; }
            public float Morale { get; }
            public float Fatigue { get; }
            public string Register { get; }
            public string Profession { get; }
        }

        /// <summary>
        /// Pick a deterministic living speaker from the roster owner for a bark.
        /// Never invents a survivor: returns a null Id when nobody is alive.
        /// </summary>
        private LivingSpeaker PickLivingSurvivorSpeaker(string? excludeId = null)
        {
            var roster = _survivors?.Roster?.Roster;
            if (roster == null || roster.Count == 0) return new LivingSpeaker(null, 50f, 0f, string.Empty, string.Empty);

            var day = Math.Max(1, _simDay);
            int start = _campaignDay != null
                ? _campaignDay.Rng.Fork(CampaignStreamIds.Social, day, 17).Next(0, roster.Count)
                : 0;

            for (int offset = 0; offset < roster.Count; offset++)
            {
                var entry = roster[(start + offset) % roster.Count];
                if (entry == null || string.IsNullOrEmpty(entry.survivorId)) continue;
                if (!entry.isAlive) continue;
                if (!string.IsNullOrEmpty(excludeId) && string.Equals(entry.survivorId, excludeId, StringComparison.OrdinalIgnoreCase)) continue;

                var needs = _survivors?.Needs.Get(entry.survivorId);
                var definition = _survivors?.Roster.FindDefinition(entry.definitionId);
                // The catalog matches speakers by profession (or the
                // "any" wildcard); no authored voice-profile data exists, so
                // the register preference stays empty and the catalog's own
                // weighting decides.
                return new LivingSpeaker(
                    entry.survivorId,
                    needs?.Morale ?? 50f,
                    needs?.Fatigue ?? 0f,
                    string.Empty,
                    definition?.profession ?? string.Empty);
            }
            return new LivingSpeaker(null, 50f, 0f, string.Empty, string.Empty);
        }
    }
}
