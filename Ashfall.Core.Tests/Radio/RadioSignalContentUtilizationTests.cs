// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    /// <summary>
    /// Flagship Task 17: Distress Signal Content Utilization & Catalog Cross-Reference Suite.
    ///
    /// Validates:
    /// - 100% of authored distress signals load cleanly with expected count (25 signals).
    /// - Every signal has unique frequency, valid MHz within broadcast spectrum (50-900 MHz).
    /// - Every revealed location resolves against canonical location or expedition destination catalogs.
    /// - Every revealed item resolves against canonical item catalog.
    /// - Every sender and deceptive faction resolves against canonical faction lore.
    /// - Every moral choice ID resolves against moral choice quest catalogs.
    /// - Every signal provides ordered message fragments with valid clarity and non-empty text.
    /// - Knowledge rewards are strictly positive with valid topic identifiers.
    /// - Structural categorization: genuine rescues, traps/lures, grim/stale memorials, and mystery beacons.
    /// </summary>
    public sealed class RadioSignalContentUtilizationTests : CatalogTestBase
    {
        private static List<DistressSignalDefinition> LoadSignals()
        {
            string path = Path.Combine(DataDirectory, "radio_distress_signals.json");
            using var doc = JsonDocument.Parse(File.ReadAllText(path));
            var arr = doc.RootElement.GetProperty("radio_broadcasts");
            var list = new List<DistressSignalDefinition>();
            foreach (var elem in arr.EnumerateArray())
            {
                var def = JsonSerializer.Deserialize<DistressSignalDefinition>(elem.GetRawText(), SystemTextJsonSerializer.Options);
                if (def != null) list.Add(def);
            }
            return list;
        }

        private static HashSet<string> LoadLocations()
        {
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            string locPath = Path.Combine(DataDirectory, "locations.json");
            if (File.Exists(locPath))
            {
                using var doc = JsonDocument.Parse(File.ReadAllText(locPath));
                if (doc.RootElement.TryGetProperty("locations", out var arr))
                {
                    foreach (var elem in arr.EnumerateArray())
                    {
                        if (elem.TryGetProperty("id", out var idElem))
                        {
                            set.Add(idElem.GetString()!);
                        }
                    }
                }
            }

            string expPath = Path.Combine(DataDirectory, "expeditions.json");
            if (File.Exists(expPath))
            {
                using var doc = JsonDocument.Parse(File.ReadAllText(expPath));
                if (doc.RootElement.TryGetProperty("expeditions", out var arr))
                {
                    foreach (var elem in arr.EnumerateArray())
                    {
                        if (elem.TryGetProperty("id", out var idElem))
                        {
                            set.Add(idElem.GetString()!);
                        }
                    }
                }
            }

            return set;
        }

        private static HashSet<string> LoadItems()
        {
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            string itemPath = Path.Combine(DataDirectory, "items.json");
            if (File.Exists(itemPath))
            {
                using var doc = JsonDocument.Parse(File.ReadAllText(itemPath));
                if (doc.RootElement.TryGetProperty("items", out var arr))
                {
                    foreach (var elem in arr.EnumerateArray())
                    {
                        if (elem.TryGetProperty("id", out var idElem))
                        {
                            set.Add(idElem.GetString()!);
                        }
                    }
                }
            }
            return set;
        }

        private static HashSet<string> LoadFactions()
        {
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            string lorePath = Path.Combine(DataDirectory, "faction_lore.json");
            if (File.Exists(lorePath))
            {
                using var doc = JsonDocument.Parse(File.ReadAllText(lorePath));
                if (doc.RootElement.TryGetProperty("items", out var arr))
                {
                    foreach (var elem in arr.EnumerateArray())
                    {
                        if (elem.TryGetProperty("faction_id", out var idElem))
                        {
                            set.Add(idElem.GetString()!);
                        }
                    }
                }
            }
            return set;
        }

        private static HashSet<string> LoadMoralQuests()
        {
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            string[] questFiles = { "moral_choice_quests.json", "moral_choice_quests_distress.json" };
            foreach (var qf in questFiles)
            {
                string path = Path.Combine(DataDirectory, qf);
                if (File.Exists(path))
                {
                    using var doc = JsonDocument.Parse(File.ReadAllText(path));
                    if (doc.RootElement.TryGetProperty("quests", out var arr))
                    {
                        foreach (var elem in arr.EnumerateArray())
                        {
                            if (elem.TryGetProperty("id", out var idElem))
                            {
                                set.Add(idElem.GetString()!);
                            }
                        }
                    }
                }
            }
            return set;
        }

        [Fact]
        public void AuthoringCatalog_ContainsExactly25DistressSignals()
        {
            var signals = LoadSignals();
            Assert.Equal(25, signals.Count);
        }

        [Fact]
        public void FrequencyIdentifiers_AreUnique_AndWithinRadioSpectrum()
        {
            var signals = LoadSignals();
            var seenIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var seenFreqs = new HashSet<float>();

            foreach (var sig in signals)
            {
                Assert.False(string.IsNullOrWhiteSpace(sig.FrequencyId), "Frequency ID must not be blank.");
                Assert.StartsWith("freq_distress_", sig.FrequencyId);
                Assert.True(seenIds.Add(sig.FrequencyId), $"Duplicate frequency ID found: {sig.FrequencyId}");

                float mhz = sig.FrequencyMhz;
                Assert.True(mhz >= 50.0f && mhz <= 1000.0f, $"Frequency {mhz} MHz outside standard radio spectrum (50-1000 MHz)");
                Assert.True(seenFreqs.Add(mhz), $"Duplicate frequency {mhz} MHz across signals");
            }
        }

        [Fact]
        public void RevealedLocations_ResolveAgainstCanonicalLocationOrExpeditionCatalogs()
        {
            var signals = LoadSignals();
            var validLocations = LoadLocations();
            Assert.NotEmpty(validLocations);

            foreach (var sig in signals)
            {
                string loc = sig.LocationReference;
                if (!string.IsNullOrEmpty(loc))
                {
                    Assert.True(validLocations.Contains(loc),
                        $"Signal '{sig.FrequencyId}' references unknown location '{loc}'");
                }
            }
        }

        [Fact]
        public void RevealedItems_ResolveAgainstCanonicalItemCatalog()
        {
            var signals = LoadSignals();
            var validItems = LoadItems();
            Assert.NotEmpty(validItems);

            foreach (var sig in signals)
            {
                if (sig.RevealedItems != null)
                {
                    foreach (var item in sig.RevealedItems)
                    {
                        Assert.False(string.IsNullOrWhiteSpace(item), $"Signal '{sig.FrequencyId}' has empty item ID");
                        Assert.True(validItems.Contains(item),
                            $"Signal '{sig.FrequencyId}' references unknown item '{item}'");
                    }
                }
            }
        }

        [Fact]
        public void FactionReferences_ResolveAgainstCanonicalFactionLore()
        {
            var signals = LoadSignals();
            var validFactions = LoadFactions();
            Assert.NotEmpty(validFactions);

            foreach (var sig in signals)
            {
                if (!string.IsNullOrEmpty(sig.SenderFactionId))
                {
                    Assert.True(validFactions.Contains(sig.SenderFactionId),
                        $"Signal '{sig.FrequencyId}' references unknown sender faction '{sig.SenderFactionId}'");
                }
                if (!string.IsNullOrEmpty(sig.DeceptiveFactionId))
                {
                    Assert.True(validFactions.Contains(sig.DeceptiveFactionId),
                        $"Signal '{sig.FrequencyId}' references unknown deceptive faction '{sig.DeceptiveFactionId}'");
                }
            }
        }

        [Fact]
        public void MoralChoiceReferences_ResolveAgainstMoralChoiceCatalogs()
        {
            var signals = LoadSignals();
            var validQuests = LoadMoralQuests();
            Assert.NotEmpty(validQuests);

            foreach (var sig in signals)
            {
                if (!string.IsNullOrEmpty(sig.MoralChoiceId))
                {
                    Assert.True(validQuests.Contains(sig.MoralChoiceId),
                        $"Signal '{sig.FrequencyId}' references unknown moral choice quest '{sig.MoralChoiceId}'");
                }
            }
        }

        [Fact]
        public void MessageFragments_AreValidAndChronologicallyOrdered()
        {
            var signals = LoadSignals();

            foreach (var sig in signals)
            {
                Assert.NotNull(sig.MessageFragments);
                Assert.NotEmpty(sig.MessageFragments!);

                int lastDay = 0;
                float lastClarity = -0.01f;
                foreach (var frag in sig.MessageFragments!)
                {
                    Assert.True(frag.Day > lastDay, $"Signal '{sig.FrequencyId}' fragments must have strictly ascending day numbers");
                    lastDay = frag.Day;

                    Assert.True(frag.Clarity >= 0.0f && frag.Clarity <= 1.0f,
                        $"Signal '{sig.FrequencyId}' clarity {frag.Clarity} must be in [0, 1]");
                    Assert.True(frag.Clarity >= lastClarity,
                        $"Signal '{sig.FrequencyId}' clarity must be non-decreasing over time");
                    lastClarity = frag.Clarity;

                    Assert.False(string.IsNullOrWhiteSpace(frag.Text),
                        $"Signal '{sig.FrequencyId}' has empty fragment text on day {frag.Day}");
                }
            }
        }

        [Fact]
        public void KnowledgeRewards_HaveValidTopic_AndPositivePoints()
        {
            var signals = LoadSignals();
            int knowledgeCount = 0;

            foreach (var sig in signals)
            {
                if (sig.KnowledgePoints > 0 || !string.IsNullOrEmpty(sig.RevealedKnowledge))
                {
                    knowledgeCount++;
                    Assert.True(sig.KnowledgePoints > 0, $"Signal '{sig.FrequencyId}' knowledge points must be positive");
                    Assert.False(string.IsNullOrWhiteSpace(sig.RevealedKnowledge),
                        $"Signal '{sig.FrequencyId}' has positive knowledge points but empty topic");
                }
            }

            Assert.True(knowledgeCount >= 3, $"Expected at least 3 knowledge signals, found {knowledgeCount}");
        }

        [Fact]
        public void OutcomeTypes_AndAuthenticity_AreStrictlyCategorized()
        {
            var signals = LoadSignals();
            var validOutcomeTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "bait_trap", "child_voice", "encrypted", "false_flag",
                "knowledge", "military", "narrative", "supply_cache", "survivor_community"
            };

            foreach (var sig in signals)
            {
                Assert.Contains(sig.OutcomeType, validOutcomeTypes);
                Assert.True(sig.IsGenuineRescue || sig.IsTrapOrDeception || sig.IsAutomated || sig.IsGrimOrMemorial,
                    $"Signal '{sig.FrequencyId}' must fall into genuine rescue, trap, automated, or grim/memorial category");
            }
        }

        [Fact]
        public void DeadlineDays_AndDaysToTrace_ArePositiveAndWithinBounds()
        {
            var signals = LoadSignals();

            foreach (var sig in signals)
            {
                int traceDays = sig.DaysToTrace;
                Assert.True(traceDays >= 1 && traceDays <= 30,
                    $"Signal '{sig.FrequencyId}' days_to_trace {traceDays} out of bounds [1, 30]");

                if (sig.DeadlineDays > 0)
                {
                    Assert.True(sig.DeadlineDays >= 1 && sig.DeadlineDays <= 30,
                        $"Signal '{sig.FrequencyId}' deadline_days {sig.DeadlineDays} out of bounds [1, 30]");
                }
            }
        }
    }
}
