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

        public string LastEvent { get; private set; } = string.Empty;

        public event Action StateChanged;

        public ExpeditionHostSession(ExpeditionSystem engine = null)
        {
            Engine = engine ?? new ExpeditionSystem();
            DemoDefinitions = new List<ExpeditionDefinition>();
            RegisterDemoDefinitions();
            Engine.OnExpeditionStarted += s => { LastEvent = $"Expedition started: {s.survivorId} -> {s.displayName}."; StateChanged?.Invoke(); };
            Engine.OnExpeditionCompleted += s => { LastEvent = $"Expedition completed: {s.survivorId} returned with {s.loot.Count} loot lines."; StateChanged?.Invoke(); };
            Engine.OnExpeditionFailed += (s, r) => { LastEvent = $"Expedition failed: {s.survivorId} — {r}"; StateChanged?.Invoke(); };
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

        public string StartDemoExpedition(string survivorId, string locationId)
        {
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
    }
}
