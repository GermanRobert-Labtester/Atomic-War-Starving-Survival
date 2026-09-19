using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Phantoms;
using Xunit;

namespace Ashfall.Core.Tests.Phantoms
{
    /// <summary>
    /// Tests for the PhantomMemoryHostSession contract:
    /// validating ScavengeItem, TickDemo, and LoadData error paths and catalog deserialization.
    /// </summary>
    public class PhantomMemoryHostSessionTests
    {
        private sealed class MemoryFileIO : IFileIO
        {
            public Dictionary<string, string> Files { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            public bool DirectoryExists(string path) => true;
            public bool FileExists(string path) => Files.ContainsKey(path);
            public string ReadAllText(string path)
            {
                if (Files.TryGetValue(path, out var content))
                    return content;
                throw new FileNotFoundException("File not found: " + path);
            }
            public void WriteAllText(string path, string contents) => Files[path] = contents;
            public string Combine(params string[] parts) => Path.Combine(parts);
        }

        private sealed class FaultyFileIo : IFileIO
        {
            private readonly IFileIO _inner;
            public bool ThrowOnRead { get; set; }

            public FaultyFileIo(IFileIO inner) => _inner = inner;

            public bool DirectoryExists(string path) => _inner.DirectoryExists(path);
            public bool FileExists(string path) => _inner.FileExists(path);
            public string ReadAllText(string path)
            {
                if (ThrowOnRead) throw new IOException("Simulated storage read failure");
                return _inner.ReadAllText(path);
            }
            public void WriteAllText(string path, string contents) => _inner.WriteAllText(path, contents);
            public string Combine(params string[] parts) => _inner.Combine(parts);
        }

        private sealed class TestPhantomMemoryHostSession : StatefulSessionBase
        {
            public PhantomMemoryEngine Engine { get; }
            private readonly List<PhantomSurvivorSnapshot> _demoSurvivors;
            public IReadOnlyList<PhantomSurvivorSnapshot> Survivors => _demoSurvivors;
            public List<PhantomSurvivorSnapshot> DemoSurvivors => _demoSurvivors;
            public Inventory.Inventory? Inventory { get; set; }
            public string LastEvent { get; private set; } = string.Empty;
            private ISeededRng _rng;

            public ISeededRng Rng
            {
                get => _rng;
                set => _rng = value ?? new SeededRng(42);
            }

            public TestPhantomMemoryHostSession(PhantomMemoryEngine? engine = null, bool loadDefaults = true, ISeededRng? rng = null)
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

            public void LoadDefaultRules()
            {
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

            public static List<PhantomSurvivorSnapshot> CreateDemoSurvivors()
            {
                return new List<PhantomSurvivorSnapshot>
                {
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

                var sv = _demoSurvivors.Find(s => s.survivorId == survivorId);
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

                if (Inventory != null)
                {
                    string canonical = ItemAliases.ToCanonical(itemId);
                    bool hasCanonical = Inventory.CountById(canonical) > 0;
                    bool hasRaw = !string.Equals(canonical, itemId, StringComparison.Ordinal)
                        && Inventory.CountById(itemId) > 0;
                    if (!hasCanonical && !hasRaw)
                    {
                        resultText = $"Item '{itemId}' not present in shelter inventory.";
                        return false;
                    }
                    if (consumeItem)
                    {
                        string consumeId = hasCanonical ? canonical : itemId;
                        if (!Inventory.TryConsume(consumeId, 1)
                            && !(hasCanonical && hasRaw && Inventory.TryConsume(itemId, 1)))
                        {
                            resultText = $"Could not consume '{itemId}' from shelter inventory.";
                            return false;
                        }
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
                {
                    var sv = list[i];
                    if (sv != null && !string.IsNullOrEmpty(sv.survivorId))
                        Engine.TickHour(sv.survivorId, 1f);
                }
                LastEvent = "Phantom timers ticked.";
                RaiseStateChanged();
                return LastEvent;
            }

            public static TestPhantomMemoryHostSession Create(
                string dataDir,
                ISeededRng? rng = null,
                IFileIO? fileIO = null,
                IJsonSerializer? jsonSerializer = null,
                Action<string>? onError = null)
            {
                var engine = new PhantomMemoryEngine();
                bool loaded = LoadRulesFromJson(engine, dataDir, fileIO, jsonSerializer, onError);
                return new TestPhantomMemoryHostSession(engine, loadDefaults: !loaded, rng: rng);
            }

            public bool LoadData(
                string dataDir,
                IFileIO? fileIO = null,
                IJsonSerializer? jsonSerializer = null,
                Action<string>? onError = null) =>
                LoadRulesFromJson(Engine, dataDir, fileIO, jsonSerializer, onError);

            public static bool LoadRulesFromJson(
                PhantomMemoryEngine engine,
                string dataDir,
                IFileIO? fileIO = null,
                IJsonSerializer? jsonSerializer = null,
                Action<string>? onError = null)
            {
                if (engine == null || string.IsNullOrEmpty(dataDir)) return false;
                try
                {
                    var files = fileIO ?? new FileSystemIO();
                    var json = jsonSerializer ?? new SystemTextJsonSerializer();
                    string path = files.Combine(dataDir, "phantom_triggers.json");
                    if (!files.FileExists(path)) return false;

                    string text = files.ReadAllText(path);
                    if (string.IsNullOrWhiteSpace(text)) return false;

                    List<PhantomTriggerJsonEntry>? entries = null;
                    try
                    {
                        var catalog = json.Deserialize<PhantomTriggerCatalogJson>(text);
                        entries = catalog?.items;
                    }
                    catch
                    {
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
                    string msg = $"[PhantomMemory] Failed to load rules: {ex.Message}";
                    onError?.Invoke(msg);
                    return false;
                }
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ScavengeItem_NullOrWhitespaceItemId_ReturnsInvalidRelicMessage(string? itemId)
        {
            var session = new TestPhantomMemoryHostSession();
            bool stateChanged = false;
            session.StateChanged += () => stateChanged = true;

            string result = session.ScavengeItem("survivor_gunner_mikhail", itemId!);

            Assert.Equal("Invalid relic item ID.", result);
            Assert.False(stateChanged);
            Assert.Empty(session.LastEvent);
        }

        [Theory]
        [InlineData("military")]
        [InlineData("medical")]
        [InlineData("correspondence")]
        [InlineData("photograph")]
        [InlineData("personal_item")]
        [InlineData("generic")]
        public void ScavengeItem_CategoryTokensPassedAsItemId_RejectedWithCategoryTokenError(string categoryToken)
        {
            var session = new TestPhantomMemoryHostSession();
            bool stateChanged = false;
            session.StateChanged += () => stateChanged = true;

            string result = session.ScavengeItem("survivor_gunner_mikhail", categoryToken);

            Assert.Equal($"Invalid item ID '{categoryToken}': category tokens are not item IDs.", result);
            Assert.False(stateChanged);
            Assert.Empty(session.LastEvent);
        }

        [Fact]
        public void ScavengeItem_UnknownSurvivor_ReturnsUnknownSurvivorMessage()
        {
            var session = new TestPhantomMemoryHostSession();
            bool stateChanged = false;
            session.StateChanged += () => stateChanged = true;

            string result = session.ScavengeItem("nonexistent_survivor_999", "item_dog_tags");

            Assert.Equal("Unknown survivor.", result);
            Assert.False(stateChanged);
            Assert.Empty(session.LastEvent);
        }

        [Fact]
        public void ScavengeItem_DeceasedSurvivor_ReturnsDeceasedMessage()
        {
            var session = new TestPhantomMemoryHostSession();
            session.DemoSurvivors.Add(new PhantomSurvivorSnapshot
            {
                survivorId = "sv_deceased",
                displayName = "Casualty",
                backgroundId = "former_soldier",
                isAlive = false
            });

            bool stateChanged = false;
            session.StateChanged += () => stateChanged = true;

            string result = session.ScavengeItem("sv_deceased", "item_dog_tags");

            Assert.Equal("Survivor is deceased and cannot inspect relics.", result);
            Assert.False(stateChanged);
            Assert.Empty(session.LastEvent);
        }

        [Fact]
        public void ScavengeItem_NonMatchingRelic_ReturnsNoMemoryTriggeredMessage_AndUpdatesLastEvent()
        {
            var session = new TestPhantomMemoryHostSession();
            bool stateChanged = false;
            session.StateChanged += () => stateChanged = true;

            // clean_water maps to generic category which has no rule for former_soldier
            string result = session.ScavengeItem("survivor_gunner_mikhail", "clean_water");

            Assert.Equal("No memory triggered. The item is just an object.", result);
            Assert.Equal("No memory triggered. The item is just an object.", session.LastEvent);
            Assert.True(stateChanged);
            Assert.True(session.IsDirty);
            Assert.True(session.StateVersion > 0);
        }

        [Fact]
        public void ScavengeItem_MatchingRelic_TriggersMotivation_ReturnsFormattedVignette_AndBoostsWork()
        {
            var session = new TestPhantomMemoryHostSession();
            session.Engine.TriggerChanceOverride = 1.0f;
            // Register rule with 1.0 motivation chance to guarantee motivation outcome
            session.Engine.RegisterRule("former_soldier", "military", 1.0f, "desc",
                "{name} pockets the tags. 'I'll remember them,' they say.",
                "{name} reads the name on the tag and goes pale.");

            bool stateChanged = false;
            session.StateChanged += () => stateChanged = true;

            string result = session.ScavengeItem("survivor_gunner_mikhail", "item_dog_tags");

            Assert.Contains("Gunner Mikhail (Heavy Artillery Loader)", result);
            Assert.Contains("pockets the tags", result);
            Assert.Equal(result, session.LastEvent);
            Assert.True(stateChanged);
            Assert.True(session.IsDirty);

            // Engine state assertion: motivation boost active and efficiency boosted
            Assert.True(session.Engine.HasMotivationBoost("survivor_gunner_mikhail"));
            Assert.Equal(1f + PhantomMemoryEngine.MotivationWorkSpeedBonus,
                session.Engine.GetWorkEfficiencyMultiplier("survivor_gunner_mikhail"), 4);
            Assert.Equal(1, session.Engine.GetTriggersExperienced("survivor_gunner_mikhail"));
        }

        [Fact]
        public void ScavengeItem_MatchingRelic_TriggersBreakdown_ReturnsFormattedVignette_AndSetsRefusalHours()
        {
            var session = new TestPhantomMemoryHostSession();
            session.Engine.TriggerChanceOverride = 1.0f;
            // Register rule with 0.0 motivation chance to guarantee breakdown outcome
            session.Engine.RegisterRule("former_soldier", "military", 0.0f, "desc",
                "{name} pockets the tags.",
                "{name} reads the name on the tag and goes pale. They knew this person.");

            bool stateChanged = false;
            session.StateChanged += () => stateChanged = true;

            string result = session.ScavengeItem("survivor_gunner_mikhail", "item_dog_tags");

            Assert.Contains("Gunner Mikhail (Heavy Artillery Loader)", result);
            Assert.Contains("reads the name on the tag and goes pale", result);
            Assert.Equal(result, session.LastEvent);
            Assert.True(stateChanged);

            // Engine state assertion: breakdown work refusal hours set
            Assert.Equal(PhantomMemoryEngine.BreakdownWorkRefusalHours,
                session.Engine.GetWorkRefusalHours("survivor_gunner_mikhail"), 4);
            Assert.False(session.Engine.HasMotivationBoost("survivor_gunner_mikhail"));
        }

        [Fact]
        public void ScavengeItem_WithInventoryBound_ItemMissing_ReturnsMissingError()
        {
            var session = new TestPhantomMemoryHostSession();
            session.Inventory = new Inventory.Inventory(); // Empty inventory

            bool stateChanged = false;
            session.StateChanged += () => stateChanged = true;

            string result = session.ScavengeItem("survivor_gunner_mikhail", "item_dog_tags");

            Assert.Equal("Item 'item_dog_tags' not present in shelter inventory.", result);
            Assert.False(stateChanged);
            Assert.Empty(session.LastEvent);
        }

        [Fact]
        public void ScavengeItem_WithInventoryBound_ItemPresent_DoesNotConsumeItem()
        {
            var session = new TestPhantomMemoryHostSession();
            session.Engine.TriggerChanceOverride = 1.0f;
            session.Inventory = new Inventory.Inventory();
            session.Inventory.Add(new ItemDefinition
            {
                id = "item_dog_tags",
                displayName = "Dog Tags",
                stackMax = 10
            }, 1);

            Assert.Equal(1, session.Inventory.CountById("item_dog_tags"));

            string result = session.ScavengeItem("survivor_gunner_mikhail", "item_dog_tags");

            Assert.Contains("Gunner Mikhail (Heavy Artillery Loader)", result);
            // Verify item was NOT consumed (ScavengeItem specifies consumeItem: false)
            Assert.Equal(1, session.Inventory.CountById("item_dog_tags"));
        }

        [Fact]
        public void ScavengeItem_WithInventoryBound_CanonicalAlias_ResolvesAndSucceeds()
        {
            var session = new TestPhantomMemoryHostSession();
            session.Engine.TriggerChanceOverride = 1.0f;
            session.Inventory = new Inventory.Inventory();
            // Store canonical "bandage" in inventory
            session.Inventory.Add(new ItemDefinition
            {
                id = "bandage",
                displayName = "Clean Bandage",
                stackMax = 10
            }, 2);

            // Elena is a nurse; "item_bandage" canonicalizes to "bandage" via ItemAliases
            string result = session.ScavengeItem("elena_vasquez", "item_bandage");

            Assert.Contains("Elena Vasquez (Paramedic)", result);
            Assert.Equal(2, session.Inventory.CountById("bandage"));
        }

        [Fact]
        public void ScavengeItem_DeterministicAcrossIdenticalSeeds()
        {
            var session1 = new TestPhantomMemoryHostSession(rng: new SeededRng(1337));
            var session2 = new TestPhantomMemoryHostSession(rng: new SeededRng(1337));

            string res1 = session1.ScavengeItem("the_teacher", "letter_from_home");
            string res2 = session2.ScavengeItem("the_teacher", "letter_from_home");

            Assert.Equal(res1, res2);
            Assert.Equal(session1.LastEvent, session2.LastEvent);
            Assert.Equal(session1.Engine.HasMotivationBoost("the_teacher"), session2.Engine.HasMotivationBoost("the_teacher"));
        }

        [Fact]
        public void ScavengeItem_OneShotRelic_DoesNotRetriggerOnSecondScavenge()
        {
            var session = new TestPhantomMemoryHostSession(loadDefaults: false);
            session.Engine.TriggerChanceOverride = 1.0f;
            session.Engine.RegisterRuleDetailed(new PhantomTriggerRule
            {
                triggerId = "rule_unique_pocket_watch",
                itemCategory = "personal_item",
                itemId = "engraved_pocket_watch",
                repeatable = false,
                motivationChance = 1.0f,
                motivationText = "{name} grips the watch tightly. 'Time to move forward.'"
            }, "former_soldier");

            string firstResult = session.ScavengeItem("survivor_gunner_mikhail", "engraved_pocket_watch");
            Assert.Contains("grips the watch tightly", firstResult);
            Assert.Equal(1, session.Engine.GetTriggersExperienced("survivor_gunner_mikhail"));

            // Second scavenge of the same non-repeatable relic must return No memory triggered
            string secondResult = session.ScavengeItem("survivor_gunner_mikhail", "engraved_pocket_watch");
            Assert.Equal("No memory triggered. The item is just an object.", secondResult);
            Assert.Equal(1, session.Engine.GetTriggersExperienced("survivor_gunner_mikhail"));
        }

        [Fact]
        public void ScavengeItem_MultipleSurvivors_TrackIndependentTriggers()
        {
            var session = new TestPhantomMemoryHostSession();
            session.Engine.TriggerChanceOverride = 1.0f;

            session.ScavengeItem("survivor_gunner_mikhail", "item_dog_tags");
            session.ScavengeItem("elena_vasquez", "stethoscope");

            Assert.Equal(1, session.Engine.GetTriggersExperienced("survivor_gunner_mikhail"));
            Assert.Equal(1, session.Engine.GetTriggersExperienced("elena_vasquez"));
            Assert.Equal(0, session.Engine.GetTriggersExperienced("the_teacher"));
        }

        // ── TickDemo contract tests ──────────────────────────────────────────

        [Fact]
        public void TickDemo_ReturnsSuccessMessageAndUpdatesLastEvent()
        {
            var session = new TestPhantomMemoryHostSession();

            string result = session.TickDemo();

            Assert.Equal("Phantom timers ticked.", result);
            Assert.Equal("Phantom timers ticked.", session.LastEvent);
        }

        [Fact]
        public void TickDemo_RaisesStateChangedAndMarksSessionDirty()
        {
            var session = new TestPhantomMemoryHostSession();
            bool stateChangedFired = false;
            session.StateChanged += () => stateChangedFired = true;
            long initialVersion = session.StateVersion;

            session.TickDemo();

            Assert.True(stateChangedFired);
            Assert.True(session.IsDirty);
            Assert.True(session.StateVersion > initialVersion);
        }

        [Fact]
        public void TickDemo_DecrementsMotivationBoostHours_AndRestoresWorkEfficiencyUponExpiry()
        {
            var session = new TestPhantomMemoryHostSession();
            session.Engine.TriggerChanceOverride = 1.0f;
            // Force guaranteed motivation
            session.Engine.RegisterRule("former_soldier", "military", 1.0f, "desc",
                "{name} pockets the tags.", "{name} reads the name.");

            session.ScavengeItem("survivor_gunner_mikhail", "item_dog_tags");

            Assert.True(session.Engine.HasMotivationBoost("survivor_gunner_mikhail"));
            Assert.Equal(1f + PhantomMemoryEngine.MotivationWorkSpeedBonus,
                session.Engine.GetWorkEfficiencyMultiplier("survivor_gunner_mikhail"), 4);

            // Tick 1 hour: 7 hours remaining, motivation boost remains active
            session.TickDemo();
            Assert.True(session.Engine.HasMotivationBoost("survivor_gunner_mikhail"));
            Assert.Equal(1f + PhantomMemoryEngine.MotivationWorkSpeedBonus,
                session.Engine.GetWorkEfficiencyMultiplier("survivor_gunner_mikhail"), 4);

            // Tick remaining 7 hours (total 8 hours = MotivationBoostDurationHours)
            for (int i = 0; i < 7; i++)
            {
                session.TickDemo();
            }

            // Motivation boost has expired, multiplier resets to 1.0f
            Assert.False(session.Engine.HasMotivationBoost("survivor_gunner_mikhail"));
            Assert.Equal(1.0f, session.Engine.GetWorkEfficiencyMultiplier("survivor_gunner_mikhail"), 4);
        }

        [Fact]
        public void TickDemo_DecrementsBreakdownWorkRefusalHours_AndClearsRefusalUponExpiry()
        {
            var session = new TestPhantomMemoryHostSession();
            session.Engine.TriggerChanceOverride = 1.0f;
            // Force guaranteed breakdown
            session.Engine.RegisterRule("former_soldier", "military", 0.0f, "desc",
                "{name} pockets the tags.", "{name} reads the name on the tag.");

            session.ScavengeItem("survivor_gunner_mikhail", "item_dog_tags");

            Assert.Equal(PhantomMemoryEngine.BreakdownWorkRefusalHours,
                session.Engine.GetWorkRefusalHours("survivor_gunner_mikhail"), 4);

            // Tick 1 hour: 3 hours remaining
            session.TickDemo();
            Assert.Equal(PhantomMemoryEngine.BreakdownWorkRefusalHours - 1f,
                session.Engine.GetWorkRefusalHours("survivor_gunner_mikhail"), 4);

            // Tick remaining 3 hours (total 4 hours = BreakdownWorkRefusalHours)
            for (int i = 0; i < 3; i++)
            {
                session.TickDemo();
            }

            // Work refusal has fully cleared
            Assert.Equal(0f, session.Engine.GetWorkRefusalHours("survivor_gunner_mikhail"), 4);
        }

        [Fact]
        public void TickDemo_MultipleSurvivors_TicksAllActiveTimersSimultaneously()
        {
            var session = new TestPhantomMemoryHostSession();
            session.Engine.TriggerChanceOverride = 1.0f;
            session.Engine.RegisterRule("former_soldier", "military", 1.0f, "desc",
                "{name} stands tall.", "{name} falters.");
            session.Engine.RegisterRule("nurse", "medical", 0.0f, "desc",
                "{name} saves a life.", "{name} remembers the screams.");

            // Mikhail receives motivation boost (8h)
            session.ScavengeItem("survivor_gunner_mikhail", "item_dog_tags");
            // Elena receives breakdown refusal (4h)
            session.ScavengeItem("elena_vasquez", "stethoscope");

            Assert.True(session.Engine.HasMotivationBoost("survivor_gunner_mikhail"));
            Assert.Equal(4f, session.Engine.GetWorkRefusalHours("elena_vasquez"), 4);

            // Single TickDemo call advances both survivors' clocks by 1 hour
            session.TickDemo();

            Assert.True(session.Engine.HasMotivationBoost("survivor_gunner_mikhail"));
            Assert.Equal(3f, session.Engine.GetWorkRefusalHours("elena_vasquez"), 4);
        }

        [Fact]
        public void TickDemo_EmptySurvivorsList_CompletesGracefully()
        {
            var session = new TestPhantomMemoryHostSession();
            session.DemoSurvivors.Clear();
            bool stateChanged = false;
            session.StateChanged += () => stateChanged = true;

            string result = session.TickDemo();

            Assert.Equal("Phantom timers ticked.", result);
            Assert.Equal("Phantom timers ticked.", session.LastEvent);
            Assert.True(stateChanged);
            Assert.True(session.IsDirty);
        }

        [Fact]
        public void TickDemo_NullAndEmptySurvivorEntries_HandledWithoutException()
        {
            var session = new TestPhantomMemoryHostSession();
            session.DemoSurvivors.Add(null!);
            session.DemoSurvivors.Add(new PhantomSurvivorSnapshot { survivorId = string.Empty });
            session.DemoSurvivors.Add(new PhantomSurvivorSnapshot { survivorId = null! });

            var ex = Record.Exception(() => session.TickDemo());

            Assert.Null(ex);
            Assert.Equal("Phantom timers ticked.", session.LastEvent);
        }

        [Fact]
        public void TickDemo_SurvivorWithoutPriorRecords_DoesNotThrow()
        {
            var session = new TestPhantomMemoryHostSession();
            // Survivor exists in DemoSurvivors, but has zero records registered in engine
            Assert.Equal(0, session.Engine.GetTriggersExperienced("survivor_gunner_mikhail"));

            var ex = Record.Exception(() => session.TickDemo());

            Assert.Null(ex);
            Assert.Equal("Phantom timers ticked.", session.LastEvent);
        }

        // ── LoadData & LoadRulesFromJson Error Path & Contract Tests ─────────

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void LoadData_NullOrEmptyDataDir_ReturnsFalse(string? dir)
        {
            var session = new TestPhantomMemoryHostSession(loadDefaults: false);
            var fileIo = new MemoryFileIO();

            bool result = session.LoadData(dir!, fileIo);

            Assert.False(result);
            Assert.Empty(session.Engine.GetRules("former_soldier"));
        }

        [Fact]
        public void LoadData_FileDoesNotExist_ReturnsFalse()
        {
            var session = new TestPhantomMemoryHostSession(loadDefaults: false);
            var fileIo = new MemoryFileIO();
            // Data dir exists, but phantom_triggers.json does not exist

            bool result = session.LoadData("data_dir", fileIo);

            Assert.False(result);
            Assert.Empty(session.Engine.GetRules("former_soldier"));
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("\r\n\t")]
        public void LoadData_EmptyOrWhitespaceFile_ReturnsFalse(string whitespaceContent)
        {
            var session = new TestPhantomMemoryHostSession(loadDefaults: false);
            var fileIo = new MemoryFileIO();
            fileIo.WriteAllText(Path.Combine("data_dir", "phantom_triggers.json"), whitespaceContent);

            bool result = session.LoadData("data_dir", fileIo);

            Assert.False(result);
            Assert.Empty(session.Engine.GetRules("former_soldier"));
        }

        [Fact]
        public void LoadData_WhenIoExceptionThrown_CatchesExceptionAndReturnsFalse_AndInvokesOnError()
        {
            var session = new TestPhantomMemoryHostSession(loadDefaults: false);
            var memIo = new MemoryFileIO();
            memIo.WriteAllText(Path.Combine("data_dir", "phantom_triggers.json"), "{ \"schema_version\": 1, \"items\": [] }");
            var faultyIo = new FaultyFileIo(memIo) { ThrowOnRead = true };
            string capturedError = string.Empty;

            bool result = session.LoadData("data_dir", faultyIo, onError: err => capturedError = err);

            Assert.False(result);
            Assert.Contains("Simulated storage read failure", capturedError);
            Assert.Empty(session.Engine.GetRules("former_soldier"));
        }

        [Fact]
        public void LoadData_WhenJsonIsMalformedSyntax_CatchesExceptionAndReturnsFalse_AndInvokesOnError()
        {
            var session = new TestPhantomMemoryHostSession(loadDefaults: false);
            var fileIo = new MemoryFileIO();
            fileIo.WriteAllText(Path.Combine("data_dir", "phantom_triggers.json"), "{ invalid: json : syntax [not_closed]");
            string capturedError = string.Empty;

            bool result = session.LoadData("data_dir", fileIo, onError: err => capturedError = err);

            Assert.False(result);
            Assert.Contains("[PhantomMemory] Failed to load rules:", capturedError);
            Assert.Empty(session.Engine.GetRules("former_soldier"));
        }

        [Fact]
        public void LoadData_WhenCatalogHasNoItems_ReturnsFalse()
        {
            var session = new TestPhantomMemoryHostSession(loadDefaults: false);
            var fileIo = new MemoryFileIO();
            fileIo.WriteAllText(Path.Combine("data_dir", "phantom_triggers.json"), "{\"schema_version\":1,\"items\":[]}");

            bool result = session.LoadData("data_dir", fileIo);

            Assert.False(result);
            Assert.Empty(session.Engine.GetRules("former_soldier"));
        }

        [Fact]
        public void LoadData_WhenCatalogHasNullOrEmptyBackground_SkipsInvalidEntriesAndRegistersValidOnes()
        {
            var session = new TestPhantomMemoryHostSession(loadDefaults: false);
            var fileIo = new MemoryFileIO();
            string json = @"
{
  ""schema_version"": 1,
  ""items"": [
    null,
    { ""background_id"": """", ""triggers"": [ { ""item_category"": ""personal_item"" } ] },
    {
      ""background_id"": ""former_soldier"",
      ""triggers"": [
        null,
        {
          ""trigger_id"": ""rule_soldier_medal"",
          ""item_category"": ""personal_item"",
          ""item_id"": ""medal_valor"",
          ""motivation_chance"": 0.75,
          ""description"": ""A soldier's medal"",
          ""motivation_text"": ""{name} polishes the medal."",
          ""breakdown_text"": ""{name} turns away."",
          ""repeatable"": true
        }
      ]
    }
  ]
}";
            fileIo.WriteAllText(Path.Combine("data_dir", "phantom_triggers.json"), json);

            bool result = session.LoadData("data_dir", fileIo);

            Assert.True(result);
            var soldierRules = session.Engine.GetRules("former_soldier");
            Assert.Single(soldierRules);
            Assert.Equal("rule_soldier_medal", soldierRules[0].triggerId);
            Assert.Equal("medal_valor", soldierRules[0].itemId);
        }

        [Fact]
        public void LoadData_WhenValidCatalogJson_RegistersRulesAndReturnsTrue()
        {
            var session = new TestPhantomMemoryHostSession(loadDefaults: false);
            var fileIo = new MemoryFileIO();
            string json = @"
{
  ""schema_version"": 1,
  ""items"": [
    {
      ""background_id"": ""former_soldier"",
      ""triggers"": [
        {
          ""trigger_id"": ""rule_soldier_tags"",
          ""item_category"": ""military"",
          ""item_id"": ""dog_tags"",
          ""motivation_chance"": 0.80,
          ""description"": ""Dog tags"",
          ""motivation_text"": ""{name} remembers."",
          ""breakdown_text"": ""{name} weeps."",
          ""repeatable"": true
        }
      ]
    },
    {
      ""background_id"": ""nurse"",
      ""triggers"": [
        {
          ""trigger_id"": ""rule_nurse_stethoscope"",
          ""item_category"": ""medical"",
          ""item_id"": ""stethoscope"",
          ""motivation_chance"": 0.50,
          ""description"": ""Stethoscope"",
          ""motivation_text"": ""{name} listens."",
          ""breakdown_text"": ""{name} remembers triage."",
          ""repeatable"": false
        }
      ]
    }
  ]
}";
            fileIo.WriteAllText(Path.Combine("data_dir", "phantom_triggers.json"), json);

            bool result = session.LoadData("data_dir", fileIo);

            Assert.True(result);
            Assert.Single(session.Engine.GetRules("former_soldier"));
            Assert.Single(session.Engine.GetRules("nurse"));
        }

        [Fact]
        public void LoadData_WhenBareArrayJson_RegistersRulesAndReturnsTrue()
        {
            var session = new TestPhantomMemoryHostSession(loadDefaults: false);
            var fileIo = new MemoryFileIO();
            string bareArrayJson = @"
[
  {
    ""background_id"": ""teacher"",
    ""triggers"": [
      {
        ""trigger_id"": ""rule_teacher_book"",
        ""item_category"": ""correspondence"",
        ""item_id"": ""old_textbook"",
        ""motivation_chance"": 0.90,
        ""description"": ""Textbook"",
        ""motivation_text"": ""{name} reads."",
        ""breakdown_text"": ""{name} remembers the classroom."",
        ""repeatable"": true
      }
    ]
  }
]";
            fileIo.WriteAllText(Path.Combine("data_dir", "phantom_triggers.json"), bareArrayJson);

            bool result = session.LoadData("data_dir", fileIo);

            Assert.True(result);
            var teacherRules = session.Engine.GetRules("teacher");
            Assert.Single(teacherRules);
            Assert.Equal("rule_teacher_book", teacherRules[0].triggerId);
        }

        [Fact]
        public void Create_WhenFileIoFailsDueToIoException_FallsBackToDefaultRules()
        {
            var memIo = new MemoryFileIO();
            memIo.WriteAllText(Path.Combine("data_dir", "phantom_triggers.json"), "{ \"schema_version\": 1, \"items\": [] }");
            var faultyIo = new FaultyFileIo(memIo) { ThrowOnRead = true };
            string capturedError = string.Empty;

            var session = TestPhantomMemoryHostSession.Create(
                "data_dir",
                rng: new SeededRng(42),
                fileIO: faultyIo,
                onError: err => capturedError = err);

            Assert.NotNull(session);
            Assert.Contains("Simulated storage read failure", capturedError);
            // Fallback default rules loaded: former_soldier has 2 default rules
            Assert.NotEmpty(session.Engine.GetRules("former_soldier"));
            Assert.NotEmpty(session.Engine.GetRules("nurse"));
        }

        [Fact]
        public void Create_WhenFileIoFailsDueToCorruptJson_FallsBackToDefaultRules()
        {
            var fileIo = new MemoryFileIO();
            fileIo.WriteAllText(Path.Combine("data_dir", "phantom_triggers.json"), "{ corrupt json !!!");
            string capturedError = string.Empty;

            var session = TestPhantomMemoryHostSession.Create(
                "data_dir",
                rng: new SeededRng(42),
                fileIO: fileIo,
                onError: err => capturedError = err);

            Assert.NotNull(session);
            Assert.Contains("[PhantomMemory] Failed to load rules:", capturedError);
            Assert.NotEmpty(session.Engine.GetRules("former_soldier"));
        }

        [Fact]
        public void Create_WhenFileIoSucceeds_LoadsAuthoredRulesWithoutDefaults()
        {
            var fileIo = new MemoryFileIO();
            string json = @"
{
  ""schema_version"": 1,
  ""items"": [
    {
      ""background_id"": ""special_operative"",
      ""triggers"": [
        {
          ""trigger_id"": ""rule_special_op"",
          ""item_category"": ""military"",
          ""item_id"": ""knife"",
          ""motivation_chance"": 1.0,
          ""repeatable"": true
        }
      ]
    }
  ]
}";
            fileIo.WriteAllText(Path.Combine("data_dir", "phantom_triggers.json"), json);

            var session = TestPhantomMemoryHostSession.Create(
                "data_dir",
                rng: new SeededRng(42),
                fileIO: fileIo);

            Assert.NotNull(session);
            // Authored rule loaded
            Assert.Single(session.Engine.GetRules("special_operative"));
            // Defaults were NOT loaded (loadDefaults was false)
            Assert.Empty(session.Engine.GetRules("former_soldier"));
        }
    }
}
