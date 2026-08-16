using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Clock;
using Ashfall.Core.Events;
using Ashfall.Core.Flags;
using Ashfall.Core.Verdict;
using ClockSimClock = Ashfall.Core.Clock.SimClock;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// ASHFALL: THE VERDICT (Expansion 08) — thin Godot host session.
    /// Wraps MachineLogSystem + ReckoningSystem + EvidenceLedger + the 99.0 MHz
    /// census broadcast, wires the sim clock / event bus / flag ledger / census
    /// port, and persists to user:// via VerdictSaveStore. No gameplay rules
    /// here — hosts only present.
    /// </summary>
    public sealed class VerdictHostSession
    {
        private static readonly FileSystemIO s_files = new FileSystemIO();
        private static readonly SystemTextJsonSerializer s_json = new SystemTextJsonSerializer();

        public MachineLogSystem MachineLog { get; }
        public ReckoningSystem Reckoning { get; }
        public EvidenceLedger Evidence { get; }
        public VerdictNpcSystem Npcs { get; }
        public VerdictCensusBroadcast Census { get; }
        public VerdictRadioSystem Radio { get; internal set; }
        public IReadOnlyList<VerdictCatalogLoader.VerdictLocationEntry> Locations { get; }
        public IReadOnlyList<VerdictCatalogLoader.VerdictItemEntry> Items { get; }
        public IReadOnlyList<VerdictCatalogLoader.VerdictRadioEntry> RadioEntries { get; }
        public IReadOnlyList<string> CorruptionCorpus { get; private set; }
        private readonly ISeededRng _machineRng;

        /// <summary>Flag-gate materialization: the six Verdict figures unlock from
        /// real progress (evidence, phase, call). Hosts present this; no NPC gate is
        /// a debug backdoor — each flag traces to an actual machine milestone.</summary>
        public System.Collections.Generic.HashSet<string> MaterializedNpcFlags()
        {
            var flags = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (MachineLog.ReadCount() >= 1) flags.Add("flag_verdict_fuse_world_read");
            if (MachineLog.ReadCount() >= 1) flags.Add("flag_verdict_relay_read");
            if (Evidence.IsEnrolled("evidence_eden_log")) flags.Add("flag_verdict_eden_log_recovered");
            if (Evidence.IsEnrolled("evidence_fuse_linen")) flags.Add("flag_verdict_fuse_world_read");
            if (Evidence.IsEnrolled("evidence_fuse_linen")) flags.Add("flag_verdict_shift_charter_restored");
            if (Evidence.IsEnrolled("evidence_geophone_hymn")) flags.Add("flag_verdict_clerk_met");
            if (Reckoning.State.callResolved) flags.Add("flag_verdict_call_resolved");
            return flags;
        }

        /// <summary>NPCs currently available given live progress (flag + phase + optional site).</summary>
        public System.Collections.Generic.List<Ashfall.Core.Verdict.VerdictNpcEntry> AvailableNpcs(string locationId = null)
        {
            int phase = (int)Reckoning.Phase;
            return Npcs.GetAvailable(MaterializedNpcFlags(), phase, locationId);
        }

        public string LastEvent { get; private set; } = string.Empty;

        public event Action StateChanged;

        public VerdictHostSession(
            MachineLogSystem machineLog = null,
            ReckoningSystem reckoning = null,
            EvidenceLedger evidence = null,
            VerdictNpcSystem npcs = null,
            VerdictCensusBroadcast census = null,
            IReadOnlyList<VerdictCatalogLoader.VerdictLocationEntry> locations = null,
            IReadOnlyList<VerdictCatalogLoader.VerdictItemEntry> items = null,
            IReadOnlyList<VerdictCatalogLoader.VerdictRadioEntry> radio = null)
        {
            MachineLog = machineLog ?? new MachineLogSystem();
            Reckoning = reckoning ?? new ReckoningSystem();
            Evidence = evidence ?? new EvidenceLedger();
            Npcs = npcs ?? new VerdictNpcSystem();
            Census = census;
            Locations = locations ?? new List<VerdictCatalogLoader.VerdictLocationEntry>();
            Items = items ?? new List<VerdictCatalogLoader.VerdictItemEntry>();
            RadioEntries = radio ?? new List<VerdictCatalogLoader.VerdictRadioEntry>();
            CorruptionCorpus = new List<string>();
            _machineRng = new SeededRng(8841209 + 17);

            MachineLog.OnLogPosted += e => { LastEvent = $"log:{e.facilityId}@{e.day}:{e.kind}"; StateChanged?.Invoke(); };
            MachineLog.OnEntryRead += e => { LastEvent = $"read:{e.evidenceTag}"; StateChanged?.Invoke(); };
            Reckoning.OnPhaseChanged += p => { LastEvent = $"phase:{p}"; StateChanged?.Invoke(); };
            Reckoning.OnReckoningCall += n => { LastEvent = $"reckoning_call:{n}"; StateChanged?.Invoke(); };
            Reckoning.OnVerdictResolved += key => { LastEvent = $"resolved:{key}"; StateChanged?.Invoke(); };
            Evidence.OnEnrolled += id => { LastEvent = $"evidence:{id}"; StateChanged?.Invoke(); };
            Npcs.OnSpoken += n => { LastEvent = $"npc:{n.id}"; StateChanged?.Invoke(); };
        }

        public static VerdictHostSession Create(
            string dataDir,
            ISimClock clock = null,
            IEventBus bus = null,
            IFlagLedger flags = null,
            ISeededRng radioRng = null,
            IWorldCensus census = null)
        {
            clock = clock ?? new ClockSimClock();
            bus = bus ?? new SimpleEventBus();
            flags = flags ?? new InMemoryFlagLedger();
            radioRng = radioRng ?? new SeededRng(8841209);

            var locations = VerdictCatalogLoader.LoadLocations(dataDir, s_files, s_json);
            var items = VerdictCatalogLoader.LoadItems(dataDir, s_files, s_json);
            var radioEntries = VerdictCatalogLoader.LoadRadio(dataDir, s_files, s_json);
            var censusBroadcast = new VerdictCensusBroadcast(clock, bus, flags, radioRng, census);
            var session = new VerdictHostSession(census: censusBroadcast, locations: locations, items: items, radio: radioEntries);
            session.Radio = new VerdictRadioSystem(bus, clock, radioEntries);
            VerdictNpcCatalogLoader.LoadAndRegister(session.Npcs, dataDir, s_files, s_json);
            session.CorruptionCorpus = VerdictCatalogLoader.LoadCorruptionCorpus(dataDir, s_files, s_json);

            var save = VerdictSaveStore.TryLoad();
            if (save != null)
            {
                VerdictSaveCodec.Restore(save, session.MachineLog, session.Reckoning, session.Evidence, session.Npcs, session.Radio);
                // Observability: remember which save version loaded and whether it migrated (C).
                session.LoadedSaveVersion = save.saveVersion;
                session.WasSaveMigrated = save.saveVersion != VerdictSave.CurrentSaveVersion;
                session.LastEvent = "Verdict state restored from save.";
            }
            return session;
        }

        /// <summary>Save version loaded at startup (observability; 0 = none).</summary>
        public int LoadedSaveVersion { get; private set; }
        /// <summary>True when the loaded save was migrated to the current version (v1→v2).</summary>
        public bool WasSaveMigrated { get; private set; }

        /// <summary>Coarse game-time step — call once per sim-day, not per-frame.</summary>
        public void AdvanceDay(int day, int livingCount, int logReadCount)
        {
            var fired = Reckoning.Poll(day, livingCount, logReadCount, Evidence.Count);
            if (fired.Count > 0) LastEvent = string.Join(";", fired);
        }

        /// <summary>Feed the census broadcast with the current clock (window check).</summary>
        public void TickCensus()
        {
            Census.BroadcastIfDue();
        }

        /// <summary>Evaluate the diegetic radio corpus against the current day and phase.
        /// Broadcasts fire once each, gated on the Culpable+ census carrier window.
        /// Returns the ids fired this call (observability).</summary>
        public System.Collections.Generic.List<string> TickRadio(int day)
        {
            if (Radio == null) return new System.Collections.Generic.List<string>();
            var fired = Radio.Poll(day, Reckoning.Phase);
            if (fired.Count > 0) LastEvent = "radio:" + string.Join(";", fired);
            if (fired.Count > 0) StateChanged?.Invoke();
            return fired;
        }

        /// <summary>
        /// Enroll evidence from the reachable verdict items where the authored
        /// mechanical_effects.enrolled_evidence is non-zero. Idempotent via the
        /// EvidenceLedger; never double-enrolls. Returns count enrolled this call.
        /// </summary>
        public int EnrollEvidenceFromItems(int day)
        {
            if (Items == null) return 0;
            int enrolled = 0;
            for (int i = 0; i < Items.Count; i++)
            {
                var it = Items[i];
                if (it == null || string.IsNullOrEmpty(it.id)) continue;
                int amount = it.mechanical_effects != null ? it.mechanical_effects.enrolled_evidence : 0;
                if (amount <= 0) continue;
                if (Evidence.Enroll(it.id, day)) enrolled++;
            }
            if (enrolled > 0)
            {
                LastEvent = "evidence_from_items:" + enrolled;
                StateChanged?.Invoke();
            }
            return enrolled;
        }

        /// <summary>Corruption tick: in Culpable+ phases, post a data-corrupted log entry
        /// at a deterministic schedule (every 11th day of the countdown). Data-driven corpus.</summary>
        public void TickCorruption(int day)
        {
            if (Reckoning.Phase < ReckoningPhase.Culpable) return;
            if (day <= 0 || day % 11 != 0) return; // deterministic cadence, not per-frame
            MachineLog.InsertCorruptionMarker(day, _machineRng, (IReadOnlyList<string>)CorruptionCorpus);
        }

        public VerdictSave CaptureSave()
        {
            return VerdictSaveCodec.Capture(
                CurrentDaySafe(), MachineLog, Reckoning, Evidence, Census.LastWindowDay, Npcs, Radio);
        }

        public void RestoreSave(VerdictSave save)
        {
            VerdictSaveCodec.Restore(save, MachineLog, Reckoning, Evidence, Npcs, Radio);
            LastEvent = "Verdict state restored.";
        }

        public string StatusLine()
        {
            return $"Verdict phase: {Reckoning.Phase}; evidence: {Evidence.Count}; " +
                   $"logs read: {MachineLog.ReadCount()}/{MachineLog.Entries.Count}; " +
                   $"locations: {Locations.Count}; " +
                   $"call: {(Reckoning.State.callResolved ? "RESOLVED" : "OPEN")}";
        }

        public VerdictCatalogLoader.VerdictLocationEntry FindLocation(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            foreach (var loc in Locations)
                if (loc.id == id) return loc;
            return null;
        }

        private int CurrentDaySafe()
        {
            return 0; // replaced by the host's real sim-day when wired in Main.cs
        }
    }
}
