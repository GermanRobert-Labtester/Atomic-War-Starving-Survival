using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Expeditions;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin Godot-host session for the expedition core (Encounters port).
    /// Registers demo definitions, drives day/hour ticks with a seeded RNG,
    /// persists active expeditions. No gameplay rules here — hosts only
    /// present and wire.
    /// </summary>
    public sealed class ExpeditionHostSession
    {
        public const int DemoSeed = 7071;

        public ExpeditionSystem Engine { get; }
        public List<ExpeditionDefinition> DemoDefinitions { get; }
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

        public string LastEvent { get; private set; } = string.Empty;

        public event Action StateChanged;

        /// <summary>Fired when Core rolls an encounter. Host UI subscribes here to surface the notice.</summary>
        public event Action<ExpeditionState>? OnEncounterSurfaced;

        /// <summary>When true (default), the UI shows a modal encounter notice. When false, a transient autoplay banner.</summary>
        public static bool UseEncounterModal { get; set; } = true;

        public ExpeditionHostSession(ExpeditionSystem engine = null)
        {
            Engine = engine ?? new ExpeditionSystem();
            DemoDefinitions = new List<ExpeditionDefinition>();
            RegisterDemoDefinitions();
            Engine.OnExpeditionStarted += s => { LastEvent = $"Expedition started: {s.survivorId} -> {s.displayName}."; StateChanged?.Invoke(); };
            Engine.OnExpeditionCompleted += s => { LastEvent = $"Expedition completed: {s.survivorId} returned with {s.loot.Count} loot lines."; StateChanged?.Invoke(); };
            Engine.OnExpeditionFailed += (s, r) => { LastEvent = $"Expedition failed: {s.survivorId} — {r}"; StateChanged?.Invoke(); };
            Engine.OnEncounterTriggered += s =>
            {
                LastEvent = $"Encounter triggered: {s.survivorId} at {s.displayName} (#{s.encounterCount}).";
                StateChanged?.Invoke();
                OnEncounterSurfaced?.Invoke(s);
            };
            Engine.OnStateChanged += _ => StateChanged?.Invoke();
        }

        private void RegisterDemoDefinitions()
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
            DemoDefinitions.Add(allotments);
            DemoDefinitions.Add(cut);
        }

        public static ExpeditionHostSession Create(string dataDir)
        {
            var session = new ExpeditionHostSession();
            var save = ExpeditionSaveStore.TryLoad();
            if (save != null)
            {
                session.Engine.RestoreState(save);
                session.LastEvent = "Expedition state restored from save.";
            }
            return session;
        }

        // ── Demo actions ─────────────────────────────────────────────

        /// <summary>True when the player cannot dispatch to this location right now.</summary>
        public bool IsLocationBlocked(string locationId)
        {
            if (CrossingGate != null && CrossingSession.IsCrossingNode(locationId) && !CrossingGate.HasAccess)
                return true;
            if (ExtraBlocked != null && ExtraBlocked(locationId))
                return true;
            return false;
        }

        public string StartDemoExpedition(string survivorId, string locationId)
        {
            if (CrossingGate != null && CrossingSession.IsCrossingNode(locationId) && !CrossingGate.HasAccess)
                return $"Crossing gate is closed — no vouch. Cannot dispatch to {locationId}.";
            if (ExtraBlocked != null && ExtraBlocked(locationId))
                return $"Route blocked: cannot dispatch to {locationId} right now (seasonal or sealed).";
            var def = ExpeditionDefinitionRegistry.Get(locationId);
            if (def == null) return $"Unknown expedition target: {locationId}";
            bool ok = Engine.Start(def, survivorId, 40, ExpeditionStance.Stealth);
            return ok ? $"Sent {survivorId} to {def.displayName}." : "Expedition start refused (already active or invalid).";
        }

        public string TickDemoHours(float hours)
        {
            Engine.TickHours(hours, new SeededRng(DemoSeed));
            return $"Tick: {Engine.ActiveCount} active expedition(s).";
        }

        public string PushLuckDemo(string survivorId)
        {
            return Engine.PushLuck(survivorId) ? $"{survivorId} is pushing luck." : "Cannot push luck (not looting).";
        }

        public string RetreatDemo(string survivorId)
        {
            return Engine.Retreat(survivorId) ? $"{survivorId} is retreating." : "Cannot retreat (not looting).";
        }

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

        public string StartDiveDemo(string siteId = "site_exp09_ss_sovereign")
        {
            var site = new DiveSiteDefinition(siteId, 120, 0.5, "q_keeper_of_logs");
            DiveRunner = new DiveInstanceRunner(new Ashfall.Core.Events.SimpleEventBus(),
                new Ashfall.Core.Flags.InMemoryFlagLedger(), new SeededRng(DemoSeed), site);
            return $"Dive started at {siteId}. Oxygen: {DiveRunner.OxygenRemaining} ticks.";
        }

        public string AdvanceDiveDemo()
        {
            if (DiveRunner == null) return "No active dive.";
            bool ok = DiveRunner.Advance();
            return ok ? $"Advanced to {DiveRunner.CurrentRoom}. O2: {DiveRunner.OxygenRemaining}." : "Cannot advance (at end or no oxygen).";
        }

        public string TickDiveOxygenDemo()
        {
            if (DiveRunner == null) return "No active dive.";
            DiveRunner.TickOxygen();
            return $"O2: {DiveRunner.OxygenRemaining}. Room: {DiveRunner.CurrentRoom}.";
        }

        public string CommitDiveChoiceDemo(string choice)
        {
            if (DiveRunner == null) return "No active dive.";
            if (choice == "flood") DiveRunner.CommitChoice(SovereignChoice.flood_the_market);
            else if (choice == "burn") DiveRunner.CommitChoice(SovereignChoice.burn_the_hold);
            else return $"Unknown choice: {choice}";
            return $"Choice committed: {DiveRunner.Choice}.";
        }

        public string DiveStatusLine()
        {
            if (DiveRunner == null) return "Dive: idle";
            return $"Dive: {DiveRunner.CurrentRoom} · O2 {DiveRunner.OxygenRemaining} · " +
                   $"choice {DiveRunner.Choice} · risk {DiveRunner.DetectionRisk(0.5, false):F2}";
        }
    }
}
