// SPDX-License-Identifier: MIT
// Plan 66 — Guilt Sources Expansion: 20 -> 40 Psychological Consequence Triggers
// Pinned contract tests for the expanded guilt_sources.json catalog, pattern uniqueness,
// severity calibration, description grammar, GuiltInsomniaSystem accumulation, and save round-trip.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Xunit;
using Ashfall.Core.Survivors;

namespace Ashfall.Core.Tests;

public class GuiltSourcesPlan66CatalogTests : CatalogTestBase
{
    private static string DataDir => DataDirectory;

    private static JsonDocument LoadCatalog(string filename)
    {
        var path = Path.Combine(DataDir, filename);
        Assert.True(File.Exists(path), $"Catalog file not found: {path}");
        var text = File.ReadAllText(path);
        return JsonDocument.Parse(text);
    }

    [Fact]
    public void Catalog_LoadsAndHasExactly40GuiltSources()
    {
        using var doc = LoadCatalog("guilt_sources.json");
        var root = doc.RootElement;
        Assert.True(root.TryGetProperty("schema_version", out var schemaProp));
        Assert.Equal(1, schemaProp.GetInt32());

        Assert.True(root.TryGetProperty("items", out var itemsProp));
        Assert.Equal(40, itemsProp.GetArrayLength());

        var patterns = new List<string>();
        foreach (var item in itemsProp.EnumerateArray())
        {
            Assert.True(item.TryGetProperty("choice_pattern", out var patProp));
            patterns.Add(patProp.GetString()!);
        }

        // 20 original baseline patterns preserved
        var baseline20 = new[]
        {
            "cut_ration",
            "reduce_food",
            "starve",
            "leave_behind",
            "abandon",
            "refuse_help",
            "turn_away",
            "execute",
            "kill",
            "shoot",
            "steal",
            "hoard",
            "take_all",
            "lie",
            "deceive",
            "betray",
            "harsh",
            "refuse",
            "deny",
            "sacrifice_other"
        };
        for (int i = 0; i < baseline20.Length; i++)
        {
            Assert.Equal(baseline20[i], patterns[i]);
        }

        // 20 new patterns present
        var new20 = new[]
        {
            "hoard_medicine_while_needed",
            "barter_away_needed_food",
            "issue_known_contaminated_supplies",
            "burn_critical_fuel_for_comfort",
            "refuse_refugee_entry",
            "expel_survivor_for_efficiency",
            "hide_cache_from_allies",
            "abandon_committed_rescue",
            "leave_wounded_behind",
            "retreat_from_rescue",
            "execute_surrendered_enemy",
            "use_civilians_as_bait",
            "kill_former_ally",
            "betray_faction_trust",
            "inform_on_survivor",
            "break_final_wish_promise",
            "withhold_pain_relief",
            "triage_by_utility",
            "take_family_last_supplies",
            "order_survivor_to_death"
        };
        for (int i = 0; i < new20.Length; i++)
        {
            Assert.Equal(new20[i], patterns[20 + i]);
        }
    }

    [Fact]
    public void Catalog_CategoryDistribution_MatchesPlan66Specification()
    {
        // 20 new sources categorized per Plan 66:
        // Resource (4), Shelter (3), Expedition (3), Combat (3), Social (3), Medical (2), Scavenging (1), Leadership (1)
        var resourcePatterns = new[]
        {
            "hoard_medicine_while_needed",
            "barter_away_needed_food",
            "issue_known_contaminated_supplies",
            "burn_critical_fuel_for_comfort"
        };
        var shelterPatterns = new[]
        {
            "refuse_refugee_entry",
            "expel_survivor_for_efficiency",
            "hide_cache_from_allies"
        };
        var expeditionPatterns = new[]
        {
            "abandon_committed_rescue",
            "leave_wounded_behind",
            "retreat_from_rescue"
        };
        var combatPatterns = new[]
        {
            "execute_surrendered_enemy",
            "use_civilians_as_bait",
            "kill_former_ally"
        };
        var socialPatterns = new[]
        {
            "betray_faction_trust",
            "inform_on_survivor",
            "break_final_wish_promise"
        };
        var medicalPatterns = new[]
        {
            "withhold_pain_relief",
            "triage_by_utility"
        };
        var scavengingPatterns = new[]
        {
            "take_family_last_supplies"
        };
        var leadershipPatterns = new[]
        {
            "order_survivor_to_death"
        };

        Assert.Equal(4, resourcePatterns.Length);
        Assert.Equal(3, shelterPatterns.Length);
        Assert.Equal(3, expeditionPatterns.Length);
        Assert.Equal(3, combatPatterns.Length);
        Assert.Equal(3, socialPatterns.Length);
        Assert.Equal(2, medicalPatterns.Length);
        Assert.Single(scavengingPatterns);
        Assert.Single(leadershipPatterns);

        int totalNew = resourcePatterns.Length + shelterPatterns.Length + expeditionPatterns.Length +
                       combatPatterns.Length + socialPatterns.Length + medicalPatterns.Length +
                       scavengingPatterns.Length + leadershipPatterns.Length;
        Assert.Equal(20, totalNew);

        using var doc = LoadCatalog("guilt_sources.json");
        var items = doc.RootElement.GetProperty("items").EnumerateArray().ToList();
        var catalogPatterns = items.Select(x => x.GetProperty("choice_pattern").GetString()!).ToHashSet(StringComparer.Ordinal);

        foreach (var p in resourcePatterns.Concat(shelterPatterns).Concat(expeditionPatterns)
                                         .Concat(combatPatterns).Concat(socialPatterns).Concat(medicalPatterns)
                                         .Concat(scavengingPatterns).Concat(leadershipPatterns))
        {
            Assert.Contains(p, catalogPatterns);
        }
    }

    [Fact]
    public void Catalog_AllChoicePatternsAndTitles_AreUniqueAndValid()
    {
        using var doc = LoadCatalog("guilt_sources.json");
        var patterns = new HashSet<string>(StringComparer.Ordinal);
        var titles = new HashSet<string>(StringComparer.Ordinal);

        int index = 0;
        foreach (var item in doc.RootElement.GetProperty("items").EnumerateArray())
        {
            var pattern = item.GetProperty("choice_pattern").GetString()!;
            var title = item.GetProperty("title").GetString()!;
            var desc = item.GetProperty("description").GetString()!;
            var severity = item.GetProperty("severity").GetSingle();

            // Uniqueness
            Assert.True(patterns.Add(pattern), $"Duplicate choice_pattern: {pattern}");
            Assert.True(titles.Add(title), $"Duplicate title: {title}");

            // Title word count: 2 to 5 words
            var wordCount = title.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
            Assert.InRange(wordCount, 2, 5);

            // Description must be non-empty
            Assert.False(string.IsNullOrWhiteSpace(desc));

            // All 20 new items must use {name} template
            if (index >= 20)
            {
                Assert.Contains("{name}", desc);
            }

            // Severity strictly within 0.1 to 1.0
            Assert.InRange(severity, 0.1f, 1.0f);

            index++;
        }

        Assert.Equal(40, patterns.Count);
        Assert.Equal(40, titles.Count);
    }

    [Fact]
    public void Catalog_SeverityDistribution_IsWellCalibrated()
    {
        using var doc = LoadCatalog("guilt_sources.json");
        int minorModerate = 0;   // 0.10 - 0.50
        int moderateHigh = 0;    // 0.55 - 0.70
        int severeDevastating = 0; // 0.75 - 1.00

        foreach (var item in doc.RootElement.GetProperty("items").EnumerateArray())
        {
            var severity = item.GetProperty("severity").GetSingle();
            if (severity <= 0.50f)
                minorModerate++;
            else if (severity <= 0.70f)
                moderateHigh++;
            else
                severeDevastating++;
        }

        // Must not be top-heavy (>60% in severe) or bottom-heavy
        Assert.True(minorModerate >= 8, $"Expected >= 8 minor/moderate, got {minorModerate}");
        Assert.True(moderateHigh >= 10, $"Expected >= 10 moderate/high, got {moderateHigh}");
        Assert.True(severeDevastating >= 12, $"Expected >= 12 severe/devastating, got {severeDevastating}");
        Assert.Equal(40, minorModerate + moderateHigh + severeDevastating);
    }

    [Fact]
    public void GuiltInsomniaSystem_All20NewSources_AccumulateSeverityDeterministically()
    {
        using var doc = LoadCatalog("guilt_sources.json");
        var items = doc.RootElement.GetProperty("items").EnumerateArray().Skip(20).ToList();
        Assert.Equal(20, items.Count);

        for (int i = 0; i < items.Count; i++)
        {
            var pattern = items[i].GetProperty("choice_pattern").GetString()!;
            var severity = items[i].GetProperty("severity").GetSingle();

            var sys = new GuiltInsomniaSystem();
            string recordedSurvivor = null;
            GuiltRecord recordedRecord = null;
            sys.OnGuiltRecorded += (sId, r) =>
            {
                recordedSurvivor = sId;
                recordedRecord = r;
            };

            var survId = $"sv_test_{i}";
            sys.RecordGuilt(survId, pattern, severity, currentDay: 10 + i);

            Assert.Equal(survId, recordedSurvivor);
            Assert.NotNull(recordedRecord);
            Assert.Equal(pattern, recordedRecord.sourceId);
            Assert.Equal(severity, recordedRecord.severity, 3);
            Assert.Equal(10 + i, recordedRecord.dayRecorded);

            Assert.Equal(1, sys.GetGuiltSourceCount(survId));
            Assert.Equal(severity, sys.GetInsomniaSeverity(survId), 3);
        }
    }

    [Fact]
    public void GuiltInsomniaSystem_HighSeveritySources_TriggerCriticalInsomniaThreshold()
    {
        using var doc = LoadCatalog("guilt_sources.json");
        var highSources = doc.RootElement.GetProperty("items").EnumerateArray()
            .Skip(20)
            .Where(x => x.GetProperty("severity").GetSingle() >= GuiltInsomniaSystem.HighSeverityThreshold)
            .ToList();

        Assert.NotEmpty(highSources);

        foreach (var src in highSources)
        {
            var pattern = src.GetProperty("choice_pattern").GetString()!;
            var severity = src.GetProperty("severity").GetSingle();

            var sys = new GuiltInsomniaSystem();
            string criticalSurvivor = null;
            sys.OnGuiltInsomniaCritical += id => criticalSurvivor = id;

            sys.RecordGuilt("sv_critical", pattern, severity, currentDay: 1);

            Assert.Equal("sv_critical", criticalSurvivor);
            Assert.True(sys.GetInsomniaSeverity("sv_critical") >= GuiltInsomniaSystem.HighSeverityThreshold);
        }
    }

    [Fact]
    public void GuiltInsomniaSystem_SaveLoad_FullRoundTrip_PreservesGuiltRecords()
    {
        var sys1 = new GuiltInsomniaSystem();
        sys1.RecordGuilt("sv_leader", "order_survivor_to_death", 0.90f, 5);
        sys1.RecordGuilt("sv_leader", "burn_critical_fuel_for_comfort", 0.30f, 6);
        sys1.RecordGuilt("sv_scout", "leave_wounded_behind", 0.80f, 7);
        sys1.RecordGuilt("sv_medic", "withhold_pain_relief", 0.70f, 8);

        var save = sys1.CaptureState();
        Assert.Equal(3, save.survivors.Count);

        var sys2 = new GuiltInsomniaSystem();
        sys2.RestoreState(save);

        Assert.Equal(2, sys2.GetGuiltSourceCount("sv_leader"));
        Assert.Equal(1f, sys2.GetInsomniaSeverity("sv_leader")); // 0.9 + 0.3 clamped at 1.0

        Assert.Equal(1, sys2.GetGuiltSourceCount("sv_scout"));
        Assert.Equal(0.80f, sys2.GetInsomniaSeverity("sv_scout"), 3);

        Assert.Equal(1, sys2.GetGuiltSourceCount("sv_medic"));
        Assert.Equal(0.70f, sys2.GetInsomniaSeverity("sv_medic"), 3);
    }

    [Fact]
    public void GuiltInsomniaSystem_ExpiryAndDialogueResolution_OperateCorrectly()
    {
        var sys = new GuiltInsomniaSystem();
        sys.RecordGuilt("sv_1", "break_final_wish_promise", 0.85f, 1);
        sys.RecordGuilt("sv_1", "hide_cache_from_allies", 0.45f, 10);

        Assert.Equal(2, sys.GetGuiltSourceCount("sv_1"));
        Assert.Equal(1f, sys.GetInsomniaSeverity("sv_1")); // 0.85 + 0.45 clamped to 1.0

        // Dialogue resolves newest (hide_cache_from_allies)
        bool dialogueOk = sys.ResolveGuiltThroughDialogue("sv_1");
        Assert.True(dialogueOk);
        Assert.Equal(1, sys.GetGuiltSourceCount("sv_1"));
        Assert.Equal(0.85f, sys.GetInsomniaSeverity("sv_1"), 3);

        // Advance 32 days -> first record (break_final_wish_promise at day 1) expires (>30 days)
        sys.Tick("sv_1", 24f, 33);
        Assert.Equal(0, sys.GetGuiltSourceCount("sv_1"));
        Assert.Equal(0f, sys.GetInsomniaSeverity("sv_1"));
    }
}
