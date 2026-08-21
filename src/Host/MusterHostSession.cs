using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Journal;
using Ashfall.Core.Muster;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin Godot-host session for the Muster (Expansion 06) escalation layer.
    /// Wraps MusterSystem, loads the 15-current roster from currents.json,
    /// escalates the day clock, and persists to user:// via MusterSaveStore.
    /// No gameplay rules here — hosts only present.
    /// </summary>
    public sealed class MusterHostSession
    {
        public MusterSystem Engine { get; }
        public CoalitionCampSystem Camp { get; }
        public ColdCountSystem ColdCount { get; }
        public ProvisionedSystem Provisioned { get; }
        public LongWalkSystem LongWalk { get; }
        public ScavengerGuildSystem ScavengerGuild { get; }
        public IronRaidersSystem IronRaiders { get; }
        public HydroBaronsSystem HydroBarons { get; }
        public List<CurrentDefinition> Roster { get; }
        public List<WitnessDefinition> Witnesses { get; }
        public List<EndingDefinition> Epilogues { get; }

        public string LastEvent { get; private set; } = string.Empty;

        /// <summary>The survivor who records the witness accounts (Section III):
        /// framing is keyed to this trait, never to the witness.</summary>
        public RiskBiasTrait AuthorBias { get; private set; } = RiskBiasTrait.Realist;

        public event Action? StateChanged;
        public event Action<MusterRecord>? OnQuestlineResolved;

        public MusterHostSession(
            MusterSystem engine = null!,
            CoalitionCampSystem camp = null!,
            ColdCountSystem coldCount = null!,
            ProvisionedSystem provisioned = null!,
            LongWalkSystem longWalk = null!,
            ScavengerGuildSystem scavengerGuild = null!,
            IronRaidersSystem ironRaiders = null!,
            HydroBaronsSystem hydroBarons = null!,
            List<CurrentDefinition> roster = null!,
            List<WitnessDefinition> witnesses = null!,
            List<EndingDefinition> epilogues = null!)
        {
            Engine = engine ?? new MusterSystem();
            Camp = camp ?? new CoalitionCampSystem();
            ColdCount = coldCount ?? new ColdCountSystem();
            Provisioned = provisioned ?? new ProvisionedSystem();
            LongWalk = longWalk ?? new LongWalkSystem();
            ScavengerGuild = scavengerGuild ?? new ScavengerGuildSystem();
            IronRaiders = ironRaiders ?? new IronRaidersSystem();
            HydroBarons = hydroBarons ?? new HydroBaronsSystem();
            Roster = roster ?? new List<CurrentDefinition>();
            Witnesses = witnesses ?? new List<WitnessDefinition>();
            Epilogues = epilogues ?? new List<EndingDefinition>();
            Engine.OnQuestlineResolved += record =>
            {
                LastEvent = $"Resolved {record.questlineId} via approach {record.selectedApproach} → {record.endingKey}";
                OnQuestlineResolved?.Invoke(record);
                StateChanged?.Invoke();
            };
            Engine.OnStateChanged += _ => StateChanged?.Invoke();
            Camp.OnStateChanged += _ => StateChanged?.Invoke();
            ColdCount.OnStateChanged += _ => StateChanged?.Invoke();
            Provisioned.OnStateChanged += _ => StateChanged?.Invoke();
            LongWalk.OnStateChanged += _ => StateChanged?.Invoke();
            ScavengerGuild.OnStateChanged += _ => StateChanged?.Invoke();
            IronRaiders.OnStateChanged += _ => StateChanged?.Invoke();
            HydroBarons.OnStateChanged += _ => StateChanged?.Invoke();
        }

        public static MusterHostSession Create(string dataDir)
        {
            var roster = new List<CurrentDefinition>();
            var witnesses = new List<WitnessDefinition>();
            var epilogues = new List<EndingDefinition>();
            if (!string.IsNullOrEmpty(dataDir))
            {
                var fileIO = new FileSystemIO();
                var serializer = new SystemTextJsonSerializer();
                roster = CurrentsCatalogLoader.LoadCurrents(dataDir, fileIO, serializer);
                witnesses = WitnessCatalogLoader.LoadWitnesses(dataDir, fileIO, serializer);
                epilogues = EpilogueMatrixLoader.LoadEpilogues(dataDir, fileIO, serializer);
            }

            var session = new MusterHostSession(roster: roster, witnesses: witnesses, epilogues: epilogues);
            var save = MusterSaveStore.TryLoad();
            if (save != null)
            {
                session.RestoreSave(save);
                session.LastEvent = "Muster state restored from save.";
            }
            return session;
        }

        /// <summary>Epilogue-matrix prose for a resolved ending key (Section XII).</summary>
        public string EndingProseFor(string endingKey)
        {
            for (int i = 0; i < Epilogues.Count; i++)
                if (Epilogues[i].endingKey == endingKey)
                    return Epilogues[i].title + "\n\n" + Epilogues[i].prose;
            return string.Empty;
        }

        // ── Day escalation ─────────────────────────────────────────────

        /// <summary>Feed the sector clock. The Muster triggers at Day 260+,
        /// and with it the Coalition's holding ground forms.</summary>
        public string Escalate(int day)
        {
            Engine.SetEscalationDay(day);
            if (Engine.MusterTriggered && !Camp.Formed)
                Camp.Form(day);
            LastEvent = Engine.MusterTriggered
                ? $"Day {day}: the Muster is open. Coalition camp holds {Camp.MembersRallied}."
                : $"Day {day}: escalation tracked (Muster opens Day {MusterSystem.MusterOpeningDay}).";
            StateChanged?.Invoke();
            return LastEvent;
        }

        // ── Witness authorship (Section III) ───────────────────────────

        /// <summary>Cycle the recording survivor's trait; the same three
        /// accounts read differently in a different hand.</summary>
        public string CycleAuthorBias()
        {
            var all = (RiskBiasTrait[])Enum.GetValues(typeof(RiskBiasTrait));
            int next = ((int)AuthorBias + 1) % all.Length;
            AuthorBias = all[next];
            LastEvent = $"Witness accounts now recorded by a {AuthorBias} author.";
            StateChanged?.Invoke();
            return LastEvent;
        }

        // ── Coalition camp ─────────────────────────────────────────────

        public string RallyDeserter()
        {
            bool ok = Camp.RallyDeserter();
            LastEvent = ok
                ? $"A deserter has walked in. Camp holds {Camp.MembersRallied}."
                : "No holding ground yet — the Muster has not opened.";
            StateChanged?.Invoke();
            return LastEvent;
        }

        public string SetStrategy(QuestApproach strategy)
        {
            bool ok = Camp.SetStrategy(strategy);
            LastEvent = ok
                ? $"Strategy {strategy} chosen. Lockout risk {Camp.GarrisonLockoutRisk}%."
                : "Strategy rejected: not formed, or already chosen.";
            StateChanged?.Invoke();
            return LastEvent;
        }

        // ── Approach selection ─────────────────────────────────────────

        public string SelectApproach(string questlineId, QuestApproach approach)
        {
            bool ok = Engine.SelectApproachFor(questlineId, approach);
            LastEvent = ok
                ? $"Approach {approach} selected for {questlineId}."
                : $"Rejected: {questlineId} does not offer {approach} or is resolved.";
            StateChanged?.Invoke();
            return LastEvent;
        }

        // ── Save / Load ────────────────────────────────────────────────

        public MusterHostSave CaptureSave() => new MusterHostSave
        {
            Muster = Engine.CaptureState(),
            Camp = Camp.CaptureState(),
            ColdCount = ColdCount.CaptureState(),
            Provisioned = Provisioned.CaptureState(),
            LongWalk = LongWalk.CaptureState(),
            ScavengerGuild = ScavengerGuild.CaptureState(),
            IronRaiders = IronRaiders.CaptureState(),
            HydroBarons = HydroBarons.CaptureState()
        };

        public void RestoreSave(MusterHostSave save)
        {
            if (save == null) return;
            if (save.Muster != null)
                Engine.RestoreState(save.Muster);
            if (save.Camp != null)
                Camp.RestoreState(save.Camp);
            if (save.ColdCount != null)
                ColdCount.RestoreState(save.ColdCount);
            if (save.Provisioned != null)
                Provisioned.RestoreState(save.Provisioned);
            if (save.LongWalk != null)
                LongWalk.RestoreState(save.LongWalk);
            if (save.ScavengerGuild != null)
                ScavengerGuild.RestoreState(save.ScavengerGuild);
            if (save.IronRaiders != null)
                IronRaiders.RestoreState(save.IronRaiders);
            if (save.HydroBarons != null)
                HydroBarons.RestoreState(save.HydroBarons);
        }
    }
}
