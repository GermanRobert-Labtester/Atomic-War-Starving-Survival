using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using Godot;
using Ashfall.Core;
using Ashfall.Core.Phantoms;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin Godot-host session for the Phantom Memory Engine (Antigravity #41).
    /// Wraps PhantomMemoryEngine with demo rules, save/load, and a demo survivor.
    /// </summary>
    public sealed class PhantomMemoryHostSession
    : HostSessionBase{
        public PhantomMemoryEngine Engine { get; }
        public List<PhantomSurvivorSnapshot> Survivors { get; }
        public List<PhantomSurvivorSnapshot> DemoSurvivors => Survivors;

        public string LastEvent { get; private set; } = string.Empty;
        public PhantomMemoryHostSession(PhantomMemoryEngine engine = null!)
        {
            Engine = engine ?? new PhantomMemoryEngine();
            Survivors = CreateDemoSurvivors();
            LoadDefaultRules();
            Engine.OnPhantomTriggered += (svId, itemId, isMotivation) =>
            {
                LastEvent = $"Phantom triggered for {svId}: {(isMotivation ? "motivation" : "breakdown")}";
                RaiseStateChanged();
            };
            Engine.OnPhantomBreakdown += (svId, itemId) =>
            {
                LastEvent = $"Breakdown for {svId}";
                RaiseStateChanged();
            };
            Engine.OnStateChanged += _ => RaiseStateChanged();
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
            var sv = Survivors.Find(s => s.survivorId == survivorId);
            if (sv == null) return "Unknown survivor.";
            var rng = new SystemSeededRng(42);
            var outcome = Engine.OnItemScavenged(sv, itemId, rng);
            string text = outcome != TriggerOutcome.None
                ? Engine.ResolveTriggerText(sv, itemId, outcome == TriggerOutcome.Motivation)
                : "No memory triggered. The item is just an object.";
            LastEvent = text;
            RaiseStateChanged();
            return text;
        }

        public string TickDemo()
        {
            for (int i = 0; i < Survivors.Count; i++)
                Engine.TickHour(Survivors[i].survivorId, 1f);
            LastEvent = "Phantom timers ticked.";
            RaiseStateChanged();
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

                string text = files.ReadAllText(path);
                List<PhantomTriggerJsonEntry>? entries = null;
                try
                {
                    var catalog = json.Deserialize<PhantomTriggerCatalogJson>(text);
                    entries = catalog?.items;
                }
                catch
                {
                    // Fallback in case of bare array JSON
                    entries = json.Deserialize<List<PhantomTriggerJsonEntry>>(text);
                }

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
                        engine.RegisterRuleDetailed(new PhantomTriggerRule
                        {
                            triggerId = !string.IsNullOrEmpty(t.trigger_id) ? t.trigger_id : $"rule_{entry.background_id}_{t.item_category}",
                            itemCategory = t.item_category ?? string.Empty,
                            itemId = t.item_id ?? string.Empty,
                            motivationChance = t.motivation_chance,
                            descriptionKey = t.description ?? string.Empty,
                            motivationText = t.motivation_text ?? string.Empty,
                            breakdownText = t.breakdown_text ?? string.Empty,
                            affinityTrait = t.affinity_trait ?? string.Empty,
                            loreOnly = t.lore_only,
                            moralePayload = t.morale_payload,
                            guiltPayload = t.guilt_payload,
                            gatingFlag = t.gating_flag ?? string.Empty,
                            repeatable = t.repeatable
                        }, entry.background_id);
                    }
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[PhantomMemory] Failed to load rules: {ex.Message}");
            }
        }

        // ── JSON DTOs: shared with Phase0HostSession (see Assets/Ashfall.Core/Phantoms/PhantomTriggerDto.cs) ─

        /// <summary>A11: ISeededRng adapter now delegates to the core SeededRng
        /// (deterministic xorshift64) — no System.Random in decision paths.</summary>
        private sealed class SystemSeededRng : ISeededRng
        {
            private readonly SeededRng _rng;
            public int Seed { get; }
            public SystemSeededRng(int seed) { Seed = seed; _rng = new SeededRng(seed); }
            public int Next(int min, int max) => _rng.Next(min, max);
            public float NextFloat() => _rng.NextFloat();
            public double NextDouble() => _rng.NextDouble();
        }
    }
}
