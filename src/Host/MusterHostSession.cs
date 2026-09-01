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
    : HostSessionBase
    {
        public MusterSystem Engine { get; }
        public CoalitionCampSystem Camp { get; }
        public ColdCountSystem ColdCount { get; }
        public ProvisionedSystem Provisioned { get; }
        public LongWalkSystem LongWalk { get; }
        public ScavengerGuildSystem ScavengerGuild { get; }
        public IronRaidersSystem IronRaiders { get; }
        public HydroBaronsSystem HydroBarons { get; }
        public FactionActionBoard Board { get; }
        public List<CurrentDefinition> Roster { get; }
        public List<WitnessDefinition> Witnesses { get; }
        public List<EndingDefinition> Epilogues { get; }
        public List<CampSceneDefinition> CampScenes { get; }

        /// <summary>Camp scenes already staged this campaign (Plan 25 · 25F progression).</summary>
        public List<string> CampScenesSeen { get; } = new List<string>();

        public string LastEvent { get; private set; } = string.Empty;

        /// <summary>The survivor who records the witness accounts (Section III):
        /// framing is keyed to this trait, never to the witness.</summary>
        public RiskBiasTrait AuthorBias { get; private set; } = RiskBiasTrait.Realist;

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
            FactionActionBoard board = null!,
            List<CurrentDefinition> roster = null!,
            List<WitnessDefinition> witnesses = null!,
            List<EndingDefinition> epilogues = null!,
            List<CampSceneDefinition> campScenes = null!)
        {
            Engine = engine ?? new MusterSystem();
            Camp = camp ?? new CoalitionCampSystem();
            ColdCount = coldCount ?? new ColdCountSystem();
            Provisioned = provisioned ?? new ProvisionedSystem();
            LongWalk = longWalk ?? new LongWalkSystem();
            ScavengerGuild = scavengerGuild ?? new ScavengerGuildSystem();
            IronRaiders = ironRaiders ?? new IronRaidersSystem();
            HydroBarons = hydroBarons ?? new HydroBaronsSystem();
            Board = board ?? new FactionActionBoard(ScavengerGuild, HydroBarons, IronRaiders, Camp);
            Roster = roster ?? new List<CurrentDefinition>();
            Witnesses = witnesses ?? new List<WitnessDefinition>();
            Epilogues = epilogues ?? new List<EndingDefinition>();
            CampScenes = campScenes ?? new List<CampSceneDefinition>();
            Engine.OnQuestlineResolved += record =>
            {
                LastEvent = $"Resolved {record.questlineId} via approach {record.selectedApproach} → {record.endingKey}";
                OnQuestlineResolved?.Invoke(record);
                RaiseStateChanged();
            };
            Engine.OnStateChanged += _ => RaiseStateChanged();
            Camp.OnStateChanged += _ => RaiseStateChanged();
            ColdCount.OnStateChanged += _ => RaiseStateChanged();
            Provisioned.OnStateChanged += _ => RaiseStateChanged();
            LongWalk.OnStateChanged += _ => RaiseStateChanged();
            ScavengerGuild.OnStateChanged += _ => RaiseStateChanged();
            IronRaiders.OnStateChanged += _ => RaiseStateChanged();
            HydroBarons.OnStateChanged += _ => RaiseStateChanged();
            Board.OnStateChanged += _ => RaiseStateChanged();
        }

        public static MusterHostSession Create(string dataDir)
        {
            var roster = new List<CurrentDefinition>();
            var witnesses = new List<WitnessDefinition>();
            var epilogues = new List<EndingDefinition>();
            var campScenes = new List<CampSceneDefinition>();
            var factionActions = new List<FactionActionDefinition>();
            if (!string.IsNullOrEmpty(dataDir))
            {
                var fileIO = CatalogPath.CreateFileIOForDataDir(dataDir);
                var serializer = new SystemTextJsonSerializer();
                roster = CurrentsCatalogLoader.LoadCurrents(dataDir, fileIO, serializer);
                witnesses = WitnessCatalogLoader.LoadWitnesses(dataDir, fileIO, serializer);
                epilogues = EpilogueMatrixLoader.LoadEpilogues(dataDir, fileIO, serializer);
                campScenes = CampSceneCatalogLoader.LoadScenes(dataDir, fileIO, serializer);
                factionActions = FactionActionCatalogLoader.LoadActions(dataDir, fileIO, serializer);
            }

            var session = new MusterHostSession(
                roster: roster, witnesses: witnesses, epilogues: epilogues, campScenes: campScenes);
            session.Board.SetCatalog(factionActions);
            var save = MusterSaveStore.TryLoad();
            if (save != null)
            {
                session.RestoreSave(save);
                session.LastEvent = "Muster state restored from save.";
            }
            return session;
        }

        // ── Faction actions (Plan 25 · 25A) ────────────────────────────

        /// <summary>Resolve a player choice on an available faction action.
        /// Effects apply through the faction systems' own seams; the resolution
        /// record persists so a reload cannot re-apply it.</summary>
        public bool ResolveFactionAction(string actionId, string choiceId, int day)
        {
            bool ok = Board.Resolve(actionId, choiceId, day);
            LastEvent = ok
                ? $"Faction action resolved: {actionId} / {choiceId} (day {day})."
                : $"Faction action rejected: {actionId} is not available on day {day}.";
            RaiseStateChanged();
            return ok;
        }

        // ── Camp scenes (Plan 25 · 25F) ────────────────────────────────

        /// <summary>Stage an authored camp scene with the variant matching the
        /// current muster path and campaign flags. Marks it seen; a seen scene
        /// does not restage.</summary>
        public CampSceneSelection? StageCampScene(string sceneId, int day)
        {
            var selection = CampSceneDirector.Select(
                CampScenes, sceneId, day, Engine.MusterPath,
                Board.IsFlagSet, id => CampScenesSeen.Contains(id));
            if (selection != null && !CampScenesSeen.Contains(sceneId))
            {
                CampScenesSeen.Add(sceneId);
                LastEvent = $"Camp scene staged: {sceneId} / {selection.VariantId}.";
                RaiseStateChanged();
            }
            return selection;
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
            RaiseStateChanged();
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
            RaiseStateChanged();
            return LastEvent;
        }

        // ── Coalition camp ─────────────────────────────────────────────

        public string RallyDeserter()
        {
            bool ok = Camp.RallyDeserter();
            LastEvent = ok
                ? $"A deserter has walked in. Camp holds {Camp.MembersRallied}."
                : "No holding ground yet — the Muster has not opened.";
            RaiseStateChanged();
            return LastEvent;
        }

        public string SetStrategy(QuestApproach strategy)
        {
            bool ok = Camp.SetStrategy(strategy);
            LastEvent = ok
                ? $"Strategy {strategy} chosen. Lockout risk {Camp.GarrisonLockoutRisk}%."
                : "Strategy rejected: not formed, or already chosen.";
            RaiseStateChanged();
            return LastEvent;
        }

        // ── Approach selection ─────────────────────────────────────────

        public string SelectApproach(string questlineId, QuestApproach approach)
        {
            bool ok = Engine.SelectApproachFor(questlineId, approach);
            LastEvent = ok
                ? $"Approach {approach} selected for {questlineId}."
                : $"Rejected: {questlineId} does not offer {approach} or is resolved.";
            RaiseStateChanged();
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
            HydroBarons = HydroBarons.CaptureState(),
            FactionActions = Board.CaptureState(),
            CampScenesSeen = new List<string>(CampScenesSeen)
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
            if (save.FactionActions != null)
                Board.RestoreState(save.FactionActions);
            CampScenesSeen.Clear();
            if (save.CampScenesSeen != null)
                CampScenesSeen.AddRange(save.CampScenesSeen);
        }
    }
}
