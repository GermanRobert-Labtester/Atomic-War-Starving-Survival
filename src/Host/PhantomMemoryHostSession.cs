using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin Godot-host session for the Phantom Memory Engine (Antigravity #41).
    /// Wraps PhantomMemoryEngine with demo rules, save/load, and a demo survivor.
    /// </summary>
    public sealed class PhantomMemoryHostSession
    {
        public PhantomMemoryEngine Engine { get; }
        public List<PhantomSurvivorSnapshot> DemoSurvivors { get; }

        public string LastEvent { get; private set; } = string.Empty;

        public event Action StateChanged;

        public PhantomMemoryHostSession(PhantomMemoryEngine engine = null)
        {
            Engine = engine ?? new PhantomMemoryEngine();
            DemoSurvivors = CreateDemoSurvivors();
            LoadDefaultRules();
            Engine.OnPhantomTriggered += (svId, itemId, isMotivation) =>
            {
                LastEvent = $"Phantom triggered for {svId}: {(isMotivation ? "motivation" : "breakdown")}";
                StateChanged?.Invoke();
            };
            Engine.OnPhantomBreakdown += (svId, itemId) =>
            {
                LastEvent = $"Breakdown for {svId}";
                StateChanged?.Invoke();
            };
            Engine.OnStateChanged += _ => StateChanged?.Invoke();
        }

        /// <summary>Load the phantom_triggers.json catalog into the engine.</summary>
        public static PhantomMemoryHostSession Create(string dataDir)
        {
            var engine = new PhantomMemoryEngine();
            LoadRulesFromJson(engine, dataDir);
            return new PhantomMemoryHostSession(engine);
        }

        public void LoadDefaultRules()
        {
            // Built-in fallback rules if JSON is not loaded
            Engine.RegisterRule("former_soldier", "military", 0.20f, "desc",
                "{name} pockets the tags. 'I'll remember them,' they say. Their posture straightens.",
                "{name} reads the name on the tag and goes pale. They knew this person.");
            Engine.RegisterRule("former_soldier", "personal_item", 0.40f, "desc",
                "{name} sets the medal on the shelf. 'Someone earned this,' they say.",
                "{name} stares at the medal for a long time. 'We all earned medals,' they say.");
            Engine.RegisterRule("nurse", "medical", 0.50f, "desc",
                "{name} taps the bell of the stethoscope. 'Still works,' they say.",
                "{name} listens to their own heartbeat through the stethoscope.");
            Engine.RegisterRule("teacher", "correspondence", 0.50f, "desc",
                "{name} finds a blank page and writes a new lesson at the top.",
                "{name} reads a name written in clumsy letters on the cover.");
            Engine.RegisterRule("generic", "photograph", 0.50f, "desc",
                "{name} props the photograph against the wall. 'They'd want us to keep going.'",
                "{name} can't stop looking at the photograph. 'These people had lives.'");
            Engine.RegisterRule("generic", "correspondence", 0.40f, "desc",
                "{name} reads the letter and folds it neatly. 'We're still here to read it.'",
                "{name} reads the letter twice, then sets it down.");
            Engine.RegisterRule("generic", "personal_item", 0.20f, "desc",
                "{name} says a quiet word over the remains. 'Rest now,' they say.",
                "{name} sits beside the remains for an hour.");
        }

        // ── Save / Load ──────────────────────────────────────────────

        public PhantomMemoryEngineState CaptureSave() => Engine.CaptureState();
        public void RestoreSave(PhantomMemoryEngineState state) => Engine.RestoreState(state);

        // ── Demo actions ─────────────────────────────────────────────

        public string ScavengeItem(string survivorId, string itemId)
        {
            var sv = DemoSurvivors.Find(s => s.survivorId == survivorId);
            if (sv == null) return "Unknown survivor.";
            var rng = new SystemSeededRng(42);
            var outcome = Engine.OnItemScavenged(sv, itemId, rng);
            string text = outcome != TriggerOutcome.None
                ? Engine.ResolveTriggerText(sv, itemId, outcome == TriggerOutcome.Motivation)
                : "No memory triggered. The item is just an object.";
            LastEvent = text;
            StateChanged?.Invoke();
            return text;
        }

        public string TickDemo()
        {
            for (int i = 0; i < DemoSurvivors.Count; i++)
                Engine.TickHour(DemoSurvivors[i].survivorId, 1f);
            LastEvent = "Phantom timers ticked.";
            StateChanged?.Invoke();
            return LastEvent;
        }

        // ── Demo survivors ───────────────────────────────────────────

        private static List<PhantomSurvivorSnapshot> CreateDemoSurvivors()
        {
            return new List<PhantomSurvivorSnapshot>
            {
                // Ids from the master survivor list, never invented locally.
                new PhantomSurvivorSnapshot
                {
                    survivorId = "survivor_gunner_mikhail",
                    displayName = "Gunner Mikhail (Heavy Artillery Loader)",
                    backgroundId = "former_soldier",
                    isAlive = true
                },
                new PhantomSurvivorSnapshot
                {
                    survivorId = "elena_vasquez",
                    displayName = "Elena Vasquez (Paramedic)",
                    backgroundId = "nurse",
                    isAlive = true
                },
                new PhantomSurvivorSnapshot
                {
                    survivorId = "the_teacher",
                    displayName = "The Teacher",
                    backgroundId = "teacher",
                    isAlive = true
                }
            };
        }

        private static void LoadRulesFromJson(PhantomMemoryEngine engine, string dataDir)
        {
            if (string.IsNullOrEmpty(dataDir)) return;
            try
            {
                var files = new FileSystemIO();
                var json = new SystemTextJsonSerializer();
                string path = System.IO.Path.Combine(dataDir, "phantom_triggers.json");
                if (!files.FileExists(path)) return;

                var entries = json.Deserialize<List<PhantomTriggerJsonEntry>>(files.ReadAllText(path));
                if (entries == null) return;

                for (int i = 0; i < entries.Count; i++)
                {
                    var entry = entries[i];
                    if (entry == null || string.IsNullOrEmpty(entry.background_id)) continue;
                    if (entry.triggers == null) continue;
                    for (int j = 0; j < entry.triggers.Count; j++)
                    {
                        var t = entry.triggers[j];
                        if (t == null) continue;
                        engine.RegisterRule(
                            entry.background_id,
                            t.item_category,
                            t.motivation_chance,
                            t.description,
                            t.motivation_text,
                            t.breakdown_text);
                    }
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[PhantomMemory] Failed to load rules: {ex.Message}");
            }
        }

        // ── JSON DTOs ────────────────────────────────────────────────

        private class PhantomTriggerJsonEntry
        {
            public string background_id;
            public List<PhantomTriggerRuleJson> triggers;
        }

        private class PhantomTriggerRuleJson
        {
            public string item_category;
            public float motivation_chance;
            public string description;
            public string motivation_text;
            public string breakdown_text;
        }

        private sealed class SystemSeededRng : ISeededRng
        {
            private readonly System.Random _rng;
            public int Seed { get; }
            public SystemSeededRng(int seed) { Seed = seed; _rng = new System.Random(seed); }
            public int Next(int min, int max) => _rng.Next(min, max);
            public float NextFloat() => (float)_rng.NextDouble();
            public double NextDouble() => _rng.NextDouble();
        }
    }
}
