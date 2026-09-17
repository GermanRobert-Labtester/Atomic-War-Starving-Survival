// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Memorial
{
    /// <summary>
    /// Plan 09 / 9C Core — how the death was managed. Drives the grief cascade
    /// inside <see cref="MemorialSystem"/> via the <see cref="IGriefSink"/>
    /// port: scaling grief magnitude and broadcast language. Default
    /// <see cref="Peaceful"/> is what existing captures load as — the new
    /// field is additive on every save shape and never breaks a round-trip.
    /// </summary>
    public enum DeathQuality
    {
        Unattended = 0, // no medic present, no vigil held
        Rushed = 1,     // medic present but no time / no comfort
        Peaceful = 2,  // medic + caregiver + vigil completed
    }

    /// <summary>
    /// Plan 09 / 9C Core — how the body's remains were returned to the
    /// community. Survives on the <see cref="MemorialEntry"/> so the
    /// memorial wall decor (white space #18) can render the right
    /// artefact. Default <see cref="Burial"/> mirrors the existing
    /// resting-place behaviour.
    /// </summary>
    public enum MemorialOutcome
    {
        Burial = 0,
        WallEntry = 1,  // ashes pressed into the bunk's memorial wall
        AshScatter = 2, // remains released to the outside (open ground, river)
    }

    /// <summary>
    /// Plan 09 / 9C Core — grief cascade port. Hosts attach a sink so the
    /// memorial entry routes grief to the survivor-relations ledger (and,
    /// later, to audio + narrative beats). Engine-agnostic: a default
    /// no-op implementation logs the grief rather than failing the
    /// memorial pipeline if no host wire is bound.
    /// </summary>
    public interface IGriefSink
    {
        /// <summary>
        /// Apply grief to the surrounding relationships for a freshly
        /// memorialized survivor. The <paramref name="qualityScale"/> is the
        /// grief multiplier: <see cref="DeathQuality.Peaceful"/> = 0.5,
        /// <see cref="DeathQuality.Rushed"/> = 1.0,
        /// <see cref="DeathQuality.Unattended"/> = 1.25. Implementations
        /// should be deterministic given <paramref name="qualityScale"/>.
        /// </summary>
        void ApplyDispersion(
            string deceasedId,
            IReadOnlyList<string> survivingRelationshipIds,
            float baseGriefAmount,
            DeathQuality quality,
            int day);
    }

    /// <summary>
    /// Default no-op grief sink. Routes grief to a callback rather than
    /// mutating any host state, so Core-side tests can assert determinism
    /// without wiring SurvivorRelationsSystem.
    /// </summary>
    public sealed class CapturingGriefSink : IGriefSink
    {
        public sealed class DispersionRecord
        {
            public string DeceasedId = string.Empty;
            public List<string> SurvivngRelationshipIds = new List<string>();
            public float GriefApplied;
            public DeathQuality Quality;
            public int Day;
            public float QualityScale;
            public List<string> Warnings = new List<string>();
        }

        public List<DispersionRecord> Records { get; } = new List<DispersionRecord>();

        public void ApplyDispersion(
            string deceasedId,
            IReadOnlyList<string> survivingRelationshipIds,
            float baseGriefAmount,
            DeathQuality quality,
            int day)
        {
            float scale = quality switch
            {
                DeathQuality.Peaceful => 0.5f,
                DeathQuality.Rushed => 1.0f,
                DeathQuality.Unattended => 1.25f,
                _ => 1.0f,
            };
            Records.Add(new DispersionRecord
            {
                DeceasedId = deceasedId ?? string.Empty,
                SurvivngRelationshipIds = survivingRelationshipIds == null
                    ? new List<string>()
                    : new List<string>(survivingRelationshipIds),
                GriefApplied = baseGriefAmount * scale,
                Quality = quality,
                Day = day,
                QualityScale = scale,
            });
        }

        public static float QualityScale(DeathQuality quality) => quality switch
        {
            DeathQuality.Peaceful => 0.5f,
            DeathQuality.Rushed => 1.0f,
            DeathQuality.Unattended => 1.25f,
            _ => 1.0f,
        };
    }

    /// <summary>
    /// ASHFALL Memorial System (item 15).
    ///
    /// Single Core authority for the death-to-memorial pipeline. Subscribes
    /// to roster, needs, radiation, combat, and trauma death paths through
    /// one idempotent death bridge. Records cause, day, survival duration,
    /// final-wish status, epitaph, heirloom, morale effect, the death
    /// quality (Plan 09 9C), and the disposition outcome (burial / wall /
    /// ash-scatter).
    ///
    /// The system completes or fails final wishes before recording the
    /// memorial, transfers heirlooms atomically, and returns unresolved
    /// recipients' items to storage when a recipient is not alive.
    /// </summary>
    public sealed class MemorialSystem
    {
        private readonly MemorialState _state;

        /// <summary>Raised when a survivor is memorialized.</summary>
        public event Action<MemorialEntry>? OnMemorialized;

        /// <summary>Plan 24C (A3) — raised exactly once per deceased when the
        /// shelter's mourning vigil is held. The host applies the morale
        /// recovery and journal line through the attributed seam.</summary>
        public event Action<MemorialEntry>? OnMourned;

        /// <summary>
        /// Plan 09 / 9C Core. Set by the host so <see cref="Memorialize"/>
        /// can route grief to SurvivorRelations and downstream systems.
        /// Null = the <see cref="Memorialize"/> path is silent on grief
        /// (existing pre-9C behaviour preserved for tests that don't bind).
        /// </summary>
        public IGriefSink? GriefSink { get; set; }

        public MemorialSystem(MemorialState state)
        {
            _state = state ?? throw new ArgumentNullException(nameof(state));
        }

        public IReadOnlyList<MemorialEntry> Entries => _state.Entries;

        /// <summary>
        /// Plan 24C (A3) — the mourning vigil: a bounded, once-per-death
        /// player action. The memorial owner holds the exactly-once state
        /// (persisted `MournedDay` on the entry); the host applies the morale
        /// recovery and journal line on <see cref="OnMourned"/>. Rejected
        /// states are explicit results — never silent no-ops.
        /// </summary>
        public ActionResult Mourn(string deceasedId, int day)
        {
            if (string.IsNullOrEmpty(deceasedId))
                return ActionResult.Blocked("missing_memorial_id", "memorial.mourn_missing_id");
            for (int i = 0; i < _state.Entries.Count; i++)
            {
                var entry = _state.Entries[i];
                if (entry == null || !string.Equals(entry.SurvivorId, deceasedId, StringComparison.Ordinal)) continue;
                if (entry.MournedDay >= 0)
                    return ActionResult.Blocked("already_mourned", "memorial.mourn_already_mourned");
                entry.MournedDay = day;
                OnMourned?.Invoke(entry);
                return ActionResult.Success("memorial.mourned");
            }
            return ActionResult.Blocked("unknown_memorial", "memorial.mourn_unknown");
        }

        /// <summary>The most recent deceased not yet mourned, or null when every
        /// entry has had its vigil (the mourning surface's read model).</summary>
        public MemorialEntry? LatestUnmourned()
        {
            MemorialEntry? latest = null;
            for (int i = 0; i < _state.Entries.Count; i++)
            {
                var entry = _state.Entries[i];
                if (entry == null || entry.MournedDay >= 0) continue;
                if (latest == null || entry.Day > latest.Day) latest = entry;
            }
            return latest;
        }

        /// <summary>
        /// Idempotent memorialization. If <paramref name="survivorId"/>
        /// is already in the ledger, returns the existing entry without
        /// duplicating it (and does NOT re-fire grief — grief fires on
        /// the first call only).
        /// </summary>
        public MemorialEntry Memorialize(MemorialInput input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (string.IsNullOrEmpty(input.SurvivorId))
                throw new ArgumentException("survivorId required", nameof(input));

            for (int i = 0; i < _state.Entries.Count; i++)
                if (_state.Entries[i].SurvivorId == input.SurvivorId)
                    return _state.Entries[i];

            var entry = new MemorialEntry
            {
                SurvivorId = input.SurvivorId,
                Cause = string.IsNullOrEmpty(input.Cause) ? "unspecified" : input.Cause,
                Day = input.Day,
                SurvivedDays = input.Day - input.BirthDay,
                FinalWishResolved = input.FinalWishResolved,
                Epitaph = input.Epitaph ?? string.Empty,
                HeirloomItemId = input.HeirloomItemId ?? string.Empty,
                HeirloomRecipientId = input.HeirloomRecipientId ?? string.Empty,
                MoraleDelta = input.MoraleDelta,
                DeathQuality = input.DeathQuality,
                Outcome = input.Outcome,
            };
            _state.Entries.Add(entry);
            OnMemorialized?.Invoke(entry);

            // Fire grief cascade — single subscriber, deterministic per
            // (deceased, quality, day) input. Preserves the original
            // CaptureState/RestoreState load-restore invariants because
            // grief is not persisted; it's recomputed from the entry on
            // the first Memorialize call.
            GriefSink?.ApplyDispersion(
                entry.SurvivorId,
                input.SurvivingRelationshipIds ?? Array.Empty<string>(),
                entry.MoraleDelta,
                entry.DeathQuality,
                entry.Day);

            return entry;
        }

        public MemorialState CaptureState() => _state.Capture();

        public void RestoreState(MemorialState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            _state.RestoreInto(state);
        }
    }

    [Serializable]
    public sealed class MemorialEntry
    {
        public string SurvivorId;
        public string Cause;
        public int Day;
        public int SurvivedDays;
        public bool FinalWishResolved;
        public string Epitaph;
        public string HeirloomItemId;
        public string HeirloomRecipientId;
        public float MoraleDelta;
        // Plan 09 9C Core — additive save fields. Existing captures that
        // lack these will load with default Peaceful / Burial.
        public DeathQuality DeathQuality = DeathQuality.Peaceful;
        public MemorialOutcome Outcome = MemorialOutcome.Burial;
        /// <summary>Plan 24C (A3) — campaign day the shelter held this loss's
        /// mourning vigil (−1 = never). Additive save field; legacy captures
        /// load as never-mourned. The exactly-once contract rides this field.</summary>
        public int MournedDay = -1;
    }

    [Serializable]
    public sealed class MemorialInput
    {
        public string SurvivorId;
        public string Cause;
        public int Day;
        public int BirthDay;
        public bool FinalWishResolved;
        public string Epitaph;
        public string HeirloomItemId;
        public string HeirloomRecipientId;
        public float MoraleDelta;
        // Plan 09 9C Core — grief-cascade input. Optional; null = no
        // surviving relationship ids, host supplies gist from the
        // roster-side path that called Memorialize.
        public DeathQuality DeathQuality = DeathQuality.Peaceful;
        public MemorialOutcome Outcome = MemorialOutcome.Burial;
        public IReadOnlyList<string>? SurvivingRelationshipIds;
    }

    [Serializable]
    public sealed class MemorialState
    {
        public List<MemorialEntry> Entries = new List<MemorialEntry>();

        public MemorialState Capture() => new MemorialState
        {
            Entries = new List<MemorialEntry>(Entries)
        };

        public void RestoreInto(MemorialState state)
        {
            Entries = state.Entries ?? new List<MemorialEntry>();
        }
    }
}
