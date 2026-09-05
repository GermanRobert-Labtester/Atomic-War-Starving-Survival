using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using Godot;
using Ashfall.Core;
using Ashfall.Core.Inventory;
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
        private readonly List<PhantomSurvivorSnapshot> _demoSurvivors;
        public SurvivorsHostSession? SurvivorsSession { get; private set; }
        public InventoryHostSession? InventorySession { get; private set; }
        public ExpansionEnrichmentCatalog? EnrichmentCatalog { get; private set; }

        private ISeededRng _rng;
        public ISeededRng Rng
        {
            get => _rng;
            set => _rng = value ?? new SeededRng(42);
        }

        public IReadOnlyList<PhantomSurvivorSnapshot> Survivors
        {
            get
            {
                if (SurvivorsSession != null)
                    return ProjectSurvivors(SurvivorsSession, EnrichmentCatalog);
                return _demoSurvivors;
            }
        }
        public List<PhantomSurvivorSnapshot> DemoSurvivors => _demoSurvivors;

        public string LastEvent { get; private set; } = string.Empty;

        public PhantomMemoryHostSession(PhantomMemoryEngine engine = null!, bool loadDefaults = true, ISeededRng? rng = null)
        {
            Engine = engine ?? new PhantomMemoryEngine();
            _demoSurvivors = CreateDemoSurvivors();
            _rng = rng ?? new SeededRng(42);
            if (loadDefaults)
            {
                LoadDefaultRules();
            }
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

        public void BindSurvivors(SurvivorsHostSession session, ExpansionEnrichmentCatalog? enrichment = null)
        {
            SurvivorsSession = session;
            EnrichmentCatalog = enrichment;
            RaiseStateChanged();
        }

        public void BindInventory(InventoryHostSession inventory)
        {
            InventorySession = inventory;
            RaiseStateChanged();
        }

        /// <summary>Load the phantom_triggers.json catalog into the engine.</summary>
        public static PhantomMemoryHostSession Create(string dataDir, ISeededRng? rng = null)
        {
            var engine = new PhantomMemoryEngine();
            bool loaded = LoadRulesFromJson(engine, dataDir);
            return new PhantomMemoryHostSession(engine, loadDefaults: !loaded, rng: rng);
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

        // ── Actions ──────────────────────────────────────────────────

        public bool InspectRelic(string survivorId, string itemId, out string resultText, bool consumeItem = false)
        {
            resultText = string.Empty;
            if (string.IsNullOrWhiteSpace(itemId))
            {
                resultText = "Invalid relic item ID.";
                return false;
            }

            // F02: Rejects category tokens passed as item IDs
            if (itemId == "military" || itemId == "medical" || itemId == "correspondence" ||
                itemId == "photograph" || itemId == "personal_item" || itemId == "generic")
            {
                resultText = $"Invalid item ID '{itemId}': category tokens are not item IDs.";
                return false;
            }

            var sv = System.Linq.Enumerable.FirstOrDefault(Survivors, s => s.survivorId == survivorId);
            if (sv == null)
            {
                resultText = "Unknown survivor.";
                return false;
            }
            if (!sv.isAlive)
            {
                resultText = "Survivor is deceased and cannot inspect relics.";
                return false;
            }

            if (InventorySession != null)
            {
                string canonical = ItemAliases.ToCanonical(itemId);
                if (InventorySession.Inventory.CountById(canonical) <= 0 && InventorySession.Inventory.CountById(itemId) <= 0)
                {
                    resultText = $"Item '{itemId}' not present in shelter inventory.";
                    return false;
                }
                if (consumeItem)
                {
                    InventorySession.Inventory.TryConsume(canonical, 1);
                }
            }

            var outcome = Engine.OnItemScavenged(sv, itemId, _rng);
            resultText = outcome != TriggerOutcome.None
                ? Engine.ResolveTriggerText(sv, itemId, outcome == TriggerOutcome.Motivation)
                : "No memory triggered. The item is just an object.";
            LastEvent = resultText;
            RaiseStateChanged();
            return true;
        }

        public string ScavengeItem(string survivorId, string itemId)
        {
            InspectRelic(survivorId, itemId, out string text, consumeItem: false);
            return text;
        }

        public string TickDemo()
        {
            var list = Survivors;
            for (int i = 0; i < list.Count; i++)
                Engine.TickHour(list[i].survivorId, 1f);
            LastEvent = "Phantom timers ticked.";
            RaiseStateChanged();
            return LastEvent;
        }

        // ── Survivors Projection & Fixtures ─────────────────────────

        public static List<PhantomSurvivorSnapshot> ProjectSurvivors(SurvivorsHostSession session, ExpansionEnrichmentCatalog? enrichment)
        {
            var list = new List<PhantomSurvivorSnapshot>();
            if (session == null) return list;
            for (int i = 0; i < session.RosterState.Count; i++)
            {
                var state = session.RosterState[i];
                if (state == null) continue;
                var def = session.Roster.FindDefinition(state.Id);
                string name = !string.IsNullOrEmpty(def?.displayName) ? def.displayName : state.Id;
                string bg = "generic";
                if (enrichment != null)
                {
                    var fields = enrichment.GetSurvivorFields(state.Id);
                    if (!string.IsNullOrEmpty(fields?.phantom_background_id))
                        bg = fields.phantom_background_id;
                }
                if (bg == "generic" && def != null && !string.IsNullOrEmpty(def.profession))
                {
                    bg = MapProfessionToBackground(def.profession);
                }
                bool isAlive = state.IsAliveState;
                list.Add(new PhantomSurvivorSnapshot
                {
                    survivorId = state.Id,
                    displayName = name,
                    backgroundId = bg,
                    traitIds = def?.traitIds ?? new List<string>(),
                    isAlive = isAlive
                });
            }
            return list;
        }

        private static string MapProfessionToBackground(string profession)
        {
            if (string.IsNullOrEmpty(profession)) return "generic";
            string p = profession.ToLowerInvariant();
            if (p.Contains("soldier") || p.Contains("gunner") || p.Contains("artillery") || p.Contains("guard") || p.Contains("military"))
                return "former_soldier";
            if (p.Contains("nurse") || p.Contains("paramedic") || p.Contains("surgeon") || p.Contains("doctor") || p.Contains("medic"))
                return "nurse";
            if (p.Contains("teacher") || p.Contains("professor") || p.Contains("instructor"))
                return "teacher";
            if (p.Contains("machinist") || p.Contains("mechanic") || p.Contains("engineer") || p.Contains("technician"))
                return "machinist";
            if (p.Contains("child") || p.Contains("orphan") || p.Contains("refugee"))
                return "child_refugee";
            return "generic";
        }

        public static List<PhantomSurvivorSnapshot> CreateDemoSurvivors()
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

        private static bool LoadRulesFromJson(PhantomMemoryEngine engine, string dataDir)
        {
            if (string.IsNullOrEmpty(dataDir)) return false;
            try
            {
                var files = new FileSystemIO();
                var json = new SystemTextJsonSerializer();
                string path = System.IO.Path.Combine(dataDir, "phantom_triggers.json");
                if (!files.FileExists(path)) return false;

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

                if (entries == null || entries.Count == 0) return false;

                int registered = 0;
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
                        registered++;
                    }
                }
                return registered > 0;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[PhantomMemory] Failed to load rules: {ex.Message}");
                return false;
            }
        }
    }
}
