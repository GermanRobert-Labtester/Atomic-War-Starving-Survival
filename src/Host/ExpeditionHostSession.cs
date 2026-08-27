using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.IO;
using Ashfall.Core.Narrative;

#pragma warning disable CS8618

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin Godot-host session for the expedition core.
    /// Manages expedition definitions, drives day/hour ticks with a seeded RNG,
    /// and persists active expeditions. Presentation and wiring only — all rules in Core.
    /// </summary>
    public sealed class ExpeditionHostSession : HostSessionBase
    {
        public const int DemoSeed = 7071;

        public ExpeditionSystem Engine { get; }
        public List<ExpeditionDefinition> Definitions { get; }
        public List<ExpeditionDefinition> DemoDefinitions => Definitions;
        public DiveInstanceRunner DiveRunner { get; private set; }

        /// <summary>Optional crossing gate — when set, crossing-node expeditions require vouch access.</summary>
        public VouchAccessSystem CrossingGate { get; set; }

        /// <summary>
        /// Optional extra dispatch gate (ice road seasonal + deep-coast route
        /// stage). When set, any location it blocks cannot be dispatched.
        /// </summary>
        public Func<string, bool> ExtraBlocked { get; set; }

        /// <summary>Passthrough to the Core per-location encounter-chance multiplier (faction/territory danger).</summary>
        public void SetEncounterChanceMultiplier(Func<string, float> multiplier) => Engine.SetEncounterChanceMultiplier(multiplier);

        /// <summary>Current sim day, supplied by Main so EncounterApplyChoice can pass day to Core.</summary>
        public int CurrentDay { get; set; }

        public string LastEvent { get; private set; } = string.Empty;
        /// <summary>Fired when Core rolls an encounter and the bridge surfaces a DTO. Host UI subscribes here.</summary>
        public event Action<ExpeditionEncounterBridge.EncounterSurfaced>? OnEncounterSurfaced;

        /// <summary>When true (default), the UI shows a modal encounter notice. When false, a transient autoplay banner.</summary>
        public static bool UseEncounterModal { get; set; } = true;

        /// <summary>
        /// Encounter surfacing bridge. Read-only so hosts can drive the surface
        /// pipeline; Core owns all encounter rules.
        /// </summary>
        public ExpeditionEncounterBridge Bridge => _bridge;

        private readonly ExpeditionEncounterBridge _bridge;
        private readonly ISeededRng _rng;
        private readonly NarrativeEncounterSystem _narrative;

        private static readonly IReadOnlyList<PendingSurfacedEncounter> NoPending =
            new List<PendingSurfacedEncounter>(0);

        /// <summary>
        /// Surfaced-but-unresolved encounters from this trip, straight off
        /// NarrativeEncounterState.pending (the save DTO). Read-only for UI.
        /// </summary>
        public IReadOnlyList<PendingSurfacedEncounter> Pending =>
            _narrative?.State?.pending ?? NoPending;

        /// <summary>Resolve a pending entry's catalog definition, or null when the catalog has no record.</summary>
        public EncounterDefinition? FindEncounter(string encounterId) => _narrative?.Find(encounterId);

        /// <summary>Drop the pending queue without resolving. No invented outcomes.</summary>
        public void ClearAllPending() => _narrative?.ClearAllPending();

        public ExpeditionHostSession(ExpeditionSystem engine = null!, NarrativeEncounterSystem narrative = null!)
        {
            Engine = engine ?? new ExpeditionSystem();
            _rng = new SeededRng(DemoSeed);
            _narrative = narrative ?? new NarrativeEncounterSystem();
            _bridge = new ExpeditionEncounterBridge(_narrative, _rng);
            Definitions = new List<ExpeditionDefinition>();
            RegisterDefaultDefinitions();
            Engine.OnExpeditionStarted += s => { LastEvent = $"Expedition started: {s.survivorId} -> {s.displayName}."; RaiseStateChanged(); };
            Engine.OnExpeditionCompleted += s => { LastEvent = $"Expedition completed: {s.survivorId} returned with {s.loot.Count} loot lines."; RaiseStateChanged(); };
            Engine.OnExpeditionFailed += (s, r) => { LastEvent = $"Expedition failed: {s.survivorId} — {r}"; RaiseStateChanged(); };
            _bridge.OnSurfaced += dto =>
            {
                LastEvent = $"Encounter triggered: {dto.trigger.survivorId} at {dto.trigger.displayName} (#{dto.trigger.encounterCount}) -> {dto.encounter_id ?? "bare-notice"}.";
                if (!string.IsNullOrEmpty(dto.encounter_id))
                    _narrative.EnqueuePending(dto.encounter_id, dto.trigger.locationId, dto.trigger.encounterCount, CurrentDay);
                RaiseStateChanged();
                OnEncounterSurfaced?.Invoke(dto);
            };
            Engine.OnEncounterTriggered += s => _bridge.Surface(s);
            Engine.OnStateChanged += _ => RaiseStateChanged();
        }

        private void RegisterDefaultDefinitions()
        {
            var allotments = new ExpeditionDefinition
            {
                id = "loc_the_allotments",
                displayName = "The Works Allotment Commune",
                distanceTicks = 5,
                dangerLevel = 2,
                encounterChancePerTick = 0.12f,
                baseStaminaDrainPerHour = 2.0f,
                lootCategories = new List<string> { "scrap_metal", "clean_water", "bandages", "food_rations" }
            };
            var cut = new ExpeditionDefinition
            {
                id = "loc_denial_cut_substation",
                displayName = "The Denial Cut Substation",
                distanceTicks = 8,
                dangerLevel = 4,
                encounterChancePerTick = 0.18f,
                baseStaminaDrainPerHour = 3.0f,
                lootCategories = new List<string> { "dosimeter", "copper_wire", "fuel", "item_hydro_baron_queue_chit" }
            };
            ExpeditionDefinitionRegistry.Register(allotments);
            ExpeditionDefinitionRegistry.Register(cut);
            Definitions.Add(allotments);
            Definitions.Add(cut);
        }

        public static ExpeditionHostSession Create(string dataDir, NarrativeEncounterSystem narrative = null!)
        {
            var session = new ExpeditionHostSession(null!, narrative);
            if (!string.IsNullOrEmpty(dataDir))
            {
                var fileIO = new FileSystemIO();
                var serializer = new SystemTextJsonSerializer();
                var loaded = ExpeditionCatalogLoader.Load(dataDir, fileIO, serializer);
                if (loaded != null && loaded.Count > 0)
                {
                    session.Definitions.Clear();
                    session.Definitions.AddRange(loaded);
                    foreach (var def in loaded)
                    {
                        if (def != null && !string.IsNullOrEmpty(def.id))
                            ExpeditionDefinitionRegistry.Register(def);
                    }
                }
            }

            var save = ExpeditionSaveStore.TryLoad();
            if (save != null)
            {
                session.Engine.RestoreState(save);
                session.LastEvent = "Expedition state restored from save.";
            }
            return session;
        }

        // ── Production Expedition Actions ─────────────────────────────

        /// <summary>True when the player cannot dispatch to this location right now.</summary>
        public bool IsLocationBlocked(string locationId)
        {
            if (CrossingGate != null && CrossingSession.IsCrossingNode(locationId) && !CrossingGate.HasAccess)
                return true;
            if (ExtraBlocked != null && ExtraBlocked(locationId))
                return true;
            return false;
        }

        /// <summary>Production API to start an expedition to a specified location.</summary>
        public string StartExpedition(string survivorId, string locationId, ExpeditionStance stance = ExpeditionStance.Stealth, int staminaBudget = 40)
        {
            if (CrossingGate != null && CrossingSession.IsCrossingNode(locationId) && !CrossingGate.HasAccess)
                return $"Crossing gate is closed — no vouch. Cannot dispatch to {locationId}.";
            if (ExtraBlocked != null && ExtraBlocked(locationId))
                return $"Route blocked: cannot dispatch to {locationId} right now (seasonal or sealed).";
            var def = ExpeditionDefinitionRegistry.Get(locationId)
                      ?? Definitions.Find(d => d.id == locationId);
            if (def == null) return $"Unknown expedition target: {locationId}";
            bool ok = Engine.Start(def, survivorId, staminaBudget, stance);
            return ok ? $"Sent {survivorId} to {def.displayName}." : "Expedition start refused (already active or invalid).";
        }

        public string StartDemoExpedition(string survivorId, string locationId)
            => StartExpedition(survivorId, locationId);

        /// <summary>Production API to advance active expeditions by the specified duration.</summary>
        public string TickHours(float hours)
        {
            Engine.TickHours(hours, _rng);
            return $"Tick: {Engine.ActiveCount} active expedition(s).";
        }

        public string TickDemoHours(float hours)
            => TickHours(hours);

        /// <summary>
        /// Apply a player choice for a surfaced encounter through Core. The
        /// location is taken from that encounter's own pending entry when one
        /// exists, so resolving a backlog row records where that row actually
        /// happened rather than wherever the newest encounter surfaced.
        /// </summary>
        public bool EncounterApplyChoice(string encounterId, string choiceId, int day)
            => EncounterApplyChoice(encounterId, choiceId, day, null!);

        /// <summary>
        /// Apply a player choice with an explicit locationId. Pass null to let the
        /// pending queue supply it, falling back to the last surfaced encounter.
        /// </summary>
        public bool EncounterApplyChoice(string encounterId, string choiceId, int day, string locationId)
        {
            if (_bridge == null || string.IsNullOrEmpty(encounterId)) return false;

            string effectiveLocation = locationId ?? PendingLocationFor(encounterId)!;
            bool ok = _bridge.ResolveChoice(encounterId, choiceId, day, effectiveLocation!);

            // The player has acknowledged this one — shrink the pending list.
            if (ok) _narrative.ClearPending(encounterId);
            return ok;
        }

        /// <summary>The pending entry's recorded location for this encounter, or null when it is not pending.</summary>
        private string? PendingLocationFor(string encounterId)
        {
            var pending = _narrative?.State?.pending;
            if (pending == null) return null;
            for (int i = 0; i < pending.Count; i++)
            {
                if (pending[i] != null && pending[i].encounterId == encounterId)
                    return pending[i].locationId;
            }
            return null;
        }

        public string PushLuck(string survivorId)
        {
            return Engine.PushLuck(survivorId) ? $"{survivorId} is pushing luck." : "Cannot push luck (not looting).";
        }

        public string PushLuckDemo(string survivorId) => PushLuck(survivorId);

        public string Retreat(string survivorId)
        {
            return Engine.Retreat(survivorId) ? $"{survivorId} is retreating." : "Cannot retreat (not looting).";
        }

        public string RetreatDemo(string survivorId) => Retreat(survivorId);

        // ── Camp actions ──────────────────────────────────────────────

        /// <summary>Enter camp phase for an outbound expedition.</summary>
        public string EnterCamp(
            string survivorId,
            float temperatureC = -10f,
            string weatherCondition = "Clear",
            float firewood = 8f,
            float water = 4f,
            float food = 4f,
            bool hasTent = true,
            bool hasBedroll = true,
            string shelterType = "tent",
            bool hasSentry = true)
        {
            bool ok = Engine.EnterCamp(
                survivorId, CurrentDay, 18f,
                temperatureC, weatherCondition,
                firewood, water, food,
                hasTent, hasBedroll, shelterType, hasSentry);
            return ok
                ? $"{survivorId} established camp. Night begins."
                : "Cannot enter camp (not outbound or unknown expedition).";
        }

        public string EnterCampDemo(
            string survivorId,
            float temperatureC = -10f,
            string weatherCondition = "Clear",
            float firewood = 8f,
            float water = 4f,
            float food = 4f,
            bool hasTent = true,
            bool hasBedroll = true,
            string shelterType = "tent",
            bool hasSentry = true)
            => EnterCamp(survivorId, temperatureC, weatherCondition, firewood, water, food, hasTent, hasBedroll, shelterType, hasSentry);

        /// <summary>Advance one night segment. Returns dawn message when complete.</summary>
        public string CampTick(string survivorId)
        {
            bool dawn = Engine.CampTick(survivorId, _rng);
            var camp = Engine.GetCampState(survivorId);
            if (camp == null) return "No active camp.";
            if (dawn)
                return $"Dawn. Night complete. Segments: {camp.nightSegmentsCompleted}/{camp.totalNightSegments}.";
            return $"Night segment {camp.nightSegmentsCompleted}/{camp.totalNightSegments}. " +
                   $"Firewood: {camp.firewoodRemaining:F1}. Temp: {camp.temperatureC + camp.heatOutput:F1}C.";
        }

        public string CampTickDemo(string survivorId) => CampTick(survivorId);

        /// <summary>Resolve a camp encounter.</summary>
        public string ResolveCampEncounter(string survivorId, string outcome)
        {
            bool ok = Engine.ResolveCampEncounter(survivorId, outcome, outcome == "injury" ? 15f : 0f);
            return ok ? $"Camp encounter resolved: {outcome}." : "No unresolved encounter.";
        }

        public string ResolveCampEncounterDemo(string survivorId, string outcome)
            => ResolveCampEncounter(survivorId, outcome);

        /// <summary>Break camp at dawn.</summary>
        public string BreakCamp(string survivorId, bool retreat = false)
        {
            bool ok = Engine.BreakCamp(survivorId, retreat);
            return ok
                ? $"Camp broken. {(retreat ? "Retreating to shelter." : "Resuming travel.")}"
                : "Cannot break camp (night not over or no camp).";
        }

        public string BreakCampDemo(string survivorId, bool retreat = false) => BreakCamp(survivorId, retreat);

        /// <summary>Get camp status for UI display.</summary>
        public CampState? GetCampState(string survivorId) => Engine.GetCampState(survivorId);

        public string StatusLine()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append($"Expeditions active: {Engine.ActiveCount}\n");
            var ids = new List<string>(Engine.Active.Keys);
            ids.Sort(string.CompareOrdinal);
            for (int i = 0; i < ids.Count; i++)
            {
                var e = Engine.Active[ids[i]];
                sb.Append($"  {e.survivorId} -> {e.displayName} [{((ExpeditionPhase)e.phase)}] " +
                          $"travel {e.travelTicksCompleted}/{e.distanceTicks} loot {e.loot.Count} " +
                          $"stamina {e.stamina:F0}%");
                if (e.isPushingLuck) sb.Append(" [PUSHING LUCK]");
                sb.Append('\n');
            }
            return sb.ToString().TrimEnd();
        }

        // ── Save / Load ──────────────────────────────────────────────

        public List<ExpeditionState> CaptureSave() => Engine.CaptureState();
        public void RestoreSave(List<ExpeditionState> state) => Engine.RestoreState(state);

        // ── Dive Instance (Exp 09) ──────────────────────────────────

        public string StartDive(string siteId = "site_exp09_ss_sovereign")
        {
            var site = new DiveSiteDefinition(siteId, 120, 0.5, "q_keeper_of_logs");
            DiveRunner = new DiveInstanceRunner(new Ashfall.Core.Events.SimpleEventBus(),
                new Ashfall.Core.Flags.InMemoryFlagLedger(), new SeededRng(DemoSeed), site);
            return $"Dive started at {siteId}. Oxygen: {DiveRunner.OxygenRemaining} ticks.";
        }

        public string StartDiveDemo(string siteId = "site_exp09_ss_sovereign") => StartDive(siteId);

        public string AdvanceDive()
        {
            if (DiveRunner == null) return "No active dive.";
            bool ok = DiveRunner.Advance();
            return ok ? $"Advanced to {DiveRunner.CurrentRoom}. O2: {DiveRunner.OxygenRemaining}." : "Cannot advance (at end or no oxygen).";
        }

        public string AdvanceDiveDemo() => AdvanceDive();

        public string TickDiveOxygen()
        {
            if (DiveRunner == null) return "No active dive.";
            DiveRunner.TickOxygen();
            return $"O2: {DiveRunner.OxygenRemaining}. Room: {DiveRunner.CurrentRoom}.";
        }

        public string TickDiveOxygenDemo() => TickDiveOxygen();

        public string CommitDiveChoice(string choice)
        {
            if (DiveRunner == null) return "No active dive.";
            if (choice == "flood") DiveRunner.CommitChoice(SovereignChoice.flood_the_market);
            else if (choice == "burn") DiveRunner.CommitChoice(SovereignChoice.burn_the_hold);
            else return $"Unknown choice: {choice}";
            return $"Choice committed: {DiveRunner.Choice}.";
        }

        public string CommitDiveChoiceDemo(string choice) => CommitDiveChoice(choice);

        public string DiveStatusLine()
        {
            if (DiveRunner == null) return "Dive: idle";
            return $"Dive: {DiveRunner.CurrentRoom} · O2 {DiveRunner.OxygenRemaining} · " +
                   $"choice {DiveRunner.Choice} · risk {DiveRunner.DetectionRisk(0.5, false):F2}";
        }
    }
}
