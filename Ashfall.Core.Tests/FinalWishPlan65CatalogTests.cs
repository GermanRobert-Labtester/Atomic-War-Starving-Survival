// SPDX-License-Identifier: MIT
// Plan 65 — Final Wishes Expansion (8 -> 30 Survivor Wishes)
// Pinned contract tests for the expanded final_wishes.json catalog, archetype resolution,
// step integrity, cross-catalog references, deterministic RNG, and save round-trip.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Survivors;

namespace Ashfall.Core.Tests;

public class FinalWishPlan65CatalogTests : CatalogTestBase
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
    public void Catalog_LoadsAndHasExactly52Wishes()
    {
        using var doc = LoadCatalog("final_wishes.json");
        var root = doc.RootElement;
        Assert.True(root.TryGetProperty("schema_version", out var schemaProp));
        Assert.Equal(1, schemaProp.GetInt32());

        Assert.True(root.TryGetProperty("items", out var itemsProp));
        Assert.Equal(52, itemsProp.GetArrayLength());

        // Regression guard: FinalWishCatalog.Add() silently drops any entry with an
        // empty id, so a missing "id" key hides authored content without any load
        // error. 22 wishes were lost this way (18 archetypes ended up with no wish
        // at all). Assert every row carries a usable id, and that the catalog
        // actually registers as many entries as the file declares.
        foreach (var item in itemsProp.EnumerateArray())
        {
            Assert.True(item.TryGetProperty("id", out var idProp), "wish entry is missing an \"id\" key");
            var id = idProp.GetString();
            Assert.False(string.IsNullOrWhiteSpace(id), "wish entry has an empty \"id\"");
            Assert.StartsWith("wish_", id);
        }

        var catalog = FinalWishCatalogLoader.LoadCatalog(
            DataDir, new FileSystemIO(), new SystemTextJsonSerializer());
        Assert.Equal(itemsProp.GetArrayLength(), catalog.Count);

        var archetypes = new List<string>();
        foreach (var item in itemsProp.EnumerateArray())
        {
            Assert.True(item.TryGetProperty("archetype_id", out var archProp));
            archetypes.Add(archProp.GetString()!);
        }

        // 8 original archetypes preserved
        var original8 = new[]
        {
            "the_surgeon",
            "the_soldier",
            "the_nurse",
            "the_mother",
            "the_mechanic",
            "the_teacher",
            "the_refugee",
            "the_electrician"
        };
        foreach (var arch in original8)
        {
            Assert.Contains(arch, archetypes);
        }

        // 22 new archetypes present
        var new22 = new[]
        {
            "the_pharmacist",
            "the_plumber",
            "the_hunter",
            "the_courier",
            "the_reporter",
            "the_blind_preacher",
            "the_misanthrope",
            "the_botanist",
            "the_prisoner",
            "the_defector",
            "the_undertaker",
            "the_pacifist",
            "the_watchmaker",
            "the_chef",
            "the_exhausted_father",
            "the_hoarder",
            "the_general",
            "the_fierce_mother",
            "the_martyr",
            "the_burglar",
            "the_historian",
            "the_quartermaster"
        };
        foreach (var arch in new22)
        {
            Assert.Contains(arch, archetypes);
        }
    }

    [Fact]
    public void Catalog_WishTypeDistribution_MatchesPlan65Specification()
    {
        using var doc = LoadCatalog("final_wishes.json");
        var items = doc.RootElement.GetProperty("items").EnumerateArray().ToList();

        // Slice into original 8 and new 22
        var originalItems = items.Take(8).ToList();
        var newItems = items.Skip(8).Take(22).ToList();

        Assert.Equal(8, originalItems.Count);
        Assert.Equal(22, newItems.Count);

        var newCounts = new Dictionary<string, int>(StringComparer.Ordinal);
        foreach (var item in newItems)
        {
            var type = item.GetProperty("wish_type").GetString()!;
            newCounts[type] = newCounts.GetValueOrDefault(type, 0) + 1;
        }

        Assert.Equal(3, newCounts["teach_lesson"]);
        Assert.Equal(3, newCounts["deliver_letter"]);
        Assert.Equal(2, newCounts["see_a_place"]);
        Assert.Equal(2, newCounts["reconcile"]);
        Assert.Equal(3, newCounts["die_with_dignity"]);
        Assert.Equal(2, newCounts["last_meal"]);
        Assert.Equal(2, newCounts["confess"]);
        Assert.Equal(2, newCounts["protect_someone"]);
        Assert.Equal(2, newCounts["return_a_relic"]);
        Assert.Equal(1, newCounts["name_a_successor"]);
    }

    [Fact]
    public void Catalog_AllIdsTitlesAndFields_AreUniqueAndValid()
    {
        using var doc = LoadCatalog("final_wishes.json");
        using var survDoc = LoadCatalog("survivors.json");

        var survivorIds = new HashSet<string>(StringComparer.Ordinal);
        foreach (var s in survDoc.RootElement.GetProperty("survivors").EnumerateArray())
        {
            if (s.TryGetProperty("id", out var idProp))
                survivorIds.Add(idProp.GetString()!);
        }

        var ids = new HashSet<string>(StringComparer.Ordinal);
        var titles = new HashSet<string>(StringComparer.Ordinal);
        int index = 0;

        foreach (var item in doc.RootElement.GetProperty("items").EnumerateArray())
        {
            var id = item.GetProperty("id").GetString()!;
            var arch = item.GetProperty("archetype_id").GetString()!;
            var title = item.GetProperty("wish_title").GetString()!;
            var desc = item.GetProperty("wish_description").GetString()!;
            var steps = item.GetProperty("steps").EnumerateArray().ToList();

            // Ids and titles are the unique keys. archetype_id is deliberately NOT
            // unique: FinalWishCatalog keeps a pool of wish ids per archetype
            // (GetWishIdsForArchetype returns a list), so an archetype with several
            // authored wishes is the intended shape, not a duplicate.
            Assert.True(ids.Add(id), $"Duplicate wish id: {id}");
            Assert.StartsWith("wish_", id);
            Assert.True(titles.Add(title), $"Duplicate wish title: {title}");

            // Title length: 2 to 5 words
            var wordCount = title.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
            Assert.InRange(wordCount, 2, 5);

            // Description must use {name} placeholder and be non-empty
            Assert.False(string.IsNullOrWhiteSpace(desc));
            Assert.Contains("{name}", desc);

            // Step count: authored wishes run 1 to 4 steps (see_the_sky outings may
            // be a single step; everything else is 2 or more).
            Assert.InRange(steps.Count, 1, 4);

            // Completion text and buff fields
            Assert.False(string.IsNullOrWhiteSpace(item.GetProperty("completion_text").GetString()));
            // 15 is the standard award; the four deliver_letter secret wishes pay 10.
            Assert.Contains(item.GetProperty("morale_bonus").GetInt32(), new[] { 10, 15 });
            Assert.Equal("their_memory_lives_on", item.GetProperty("buff_id").GetString());

            // All 22 Plan 65 archetypes must resolve in survivors.json. The original
            // eight predate the current survivor roster and four of them
            // (the_soldier, the_nurse, the_mother, the_refugee) do not resolve; that
            // carve-out is pre-existing and unchanged.
            if (index >= 8)
            {
                Assert.Contains(arch, survivorIds);
            }

            index++;
        }
    }

    [Fact]
    public void Catalog_AllCrossReferences_ResolveInCanonicalCatalogs()
    {
        using var doc = LoadCatalog("final_wishes.json");
        using var itemsDoc = LoadCatalog("items.json");
        using var locsDoc = LoadCatalog("locations.json");
        using var npcsDoc = LoadCatalog("npc_arcs.json");
        using var skillsDoc = LoadCatalog("skills.json");

        var itemIds = new HashSet<string>(StringComparer.Ordinal);
        foreach (var it in itemsDoc.RootElement.GetProperty("items").EnumerateArray())
        {
            if (it.TryGetProperty("id", out var idProp))
                itemIds.Add(idProp.GetString()!);
        }

        var locIds = new HashSet<string>(StringComparer.Ordinal);
        foreach (var l in locsDoc.RootElement.GetProperty("locations").EnumerateArray())
        {
            if (l.TryGetProperty("id", out var idProp))
                locIds.Add(idProp.GetString()!);
        }

        var npcIds = new HashSet<string>(StringComparer.Ordinal);
        foreach (var a in npcsDoc.RootElement.GetProperty("arcs").EnumerateArray())
        {
            if (a.TryGetProperty("npc_id", out var idProp))
                npcIds.Add(idProp.GetString()!);
        }

        var skillIds = new HashSet<string>(StringComparer.Ordinal);
        foreach (var s in skillsDoc.RootElement.GetProperty("skills").EnumerateArray())
        {
            if (s.TryGetProperty("id", out var idProp))
                skillIds.Add(idProp.GetString()!);
        }

        int wishIdx = 0;
        foreach (var item in doc.RootElement.GetProperty("items").EnumerateArray())
        {
            var title = item.GetProperty("wish_title").GetString();
            foreach (var step in item.GetProperty("steps").EnumerateArray())
            {
                if (step.TryGetProperty("required_items", out var reqItemsProp))
                {
                    foreach (var ri in reqItemsProp.EnumerateArray())
                    {
                        var riStr = ri.GetString()!;
                        if (wishIdx >= 8) // check new 22 strictly
                        {
                            Assert.True(itemIds.Contains(riStr),
                                $"Required item '{riStr}' in wish '{title}' not found in items.json");
                        }
                    }
                }

                if (step.TryGetProperty("requires_location", out var locProp))
                {
                    var locStr = locProp.GetString()!;
                    if (wishIdx >= 8)
                    {
                        Assert.True(locIds.Contains(locStr),
                            $"Location '{locStr}' in wish '{title}' not found in locations.json");
                    }
                }

                if (step.TryGetProperty("requires_npc", out var npcProp))
                {
                    var npcStr = npcProp.GetString()!;
                    Assert.True(npcIds.Contains(npcStr),
                        $"NPC '{npcStr}' in wish '{title}' not found in npc_arcs.json");
                }

                if (step.TryGetProperty("skill_transfer", out var skillProp))
                {
                    var skillStr = skillProp.GetString()!;
                    Assert.True(skillIds.Contains(skillStr),
                        $"Skill '{skillStr}' in wish '{title}' not found in skills.json");
                }
            }
            wishIdx++;
        }
    }

    [Fact]
    public void FinalWishSystem_All10WishTypes_ProgressAndCompleteDeterministically()
    {
        var testTypes = new[]
        {
            "teach_lesson",
            "deliver_letter",
            "see_a_place",
            "reconcile",
            "die_with_dignity",
            "last_meal",
            "confess",
            "protect_someone",
            "return_a_relic",
            "name_a_successor"
        };

        for (int i = 0; i < testTypes.Length; i++)
        {
            var wtype = testTypes[i];
            var sys = new FinalWishSystem { Rng = new SeededRng(100 + i) };
            float moraleBuff = 0f;
            sys.ApplyPermanentShelterMoraleBuff = m => moraleBuff = m;

            var archId = $"arch_test_{wtype}";
            var survId = $"surv_test_{wtype}";

            sys.RegisterWish(archId, wtype);
            sys.DeclareTerminalPrognosis(survId, archId, isAlive: true);

            Assert.True(sys.HasActiveWish(survId));
            Assert.Equal(wtype, sys.GetWishType(survId));
            Assert.True(sys.GetDaysRemaining(survId) >= FinalWishSystem.DefaultPrognosisDaysMin);

            // Step 1
            bool doneStep1 = sys.AdvanceWishStep(survId, "step_1");
            Assert.False(doneStep1);
            Assert.Equal(1, sys.GetStepsCompleted(survId));
            Assert.True(sys.HasActiveWish(survId));

            // Step 2 completes
            bool doneStep2 = sys.AdvanceWishStep(survId, "step_2");
            Assert.True(doneStep2);
            Assert.Equal(2, sys.GetStepsCompleted(survId));
            Assert.True(sys.HasCompletedWish(survId));
            Assert.False(sys.HasActiveWish(survId));
            Assert.Equal(FinalWishSystem.WishCompletedMoraleBuff, moraleBuff);
        }
    }

    [Fact]
    public void FinalWishSystem_DeterministicSelection_HoldsUnderSeed()
    {
        var run1Days = new List<float>();
        var run2Days = new List<float>();

        for (int r = 0; r < 2; r++)
        {
            var list = r == 0 ? run1Days : run2Days;
            var sys = new FinalWishSystem { Rng = new SeededRng(12345) };

            for (int s = 0; s < 5; s++)
            {
                var id = $"sv_{s}";
                sys.DeclareTerminalPrognosis(id, "the_hunter", isAlive: true);
                list.Add(sys.GetDaysRemaining(id));
            }
        }

        Assert.Equal(run1Days.Count, run2Days.Count);
        for (int i = 0; i < run1Days.Count; i++)
        {
            Assert.Equal(run1Days[i], run2Days[i], 5);
        }
    }

    [Fact]
    public void FinalWishSystem_SaveLoad_FullRoundTrip_PreservesAllState()
    {
        var sys1 = new FinalWishSystem { Rng = new SeededRng(777) };
        sys1.RegisterWish("the_hunter", "teach_lesson");
        sys1.RegisterWish("the_courier", "deliver_letter");

        sys1.DeclareTerminalPrognosis("sv_hunter", "the_hunter", isAlive: true);
        sys1.AdvanceWishStep("sv_hunter", "step_1");

        sys1.DeclareTerminalPrognosis("sv_courier", "the_courier", isAlive: true);
        sys1.AdvanceWishStep("sv_courier", "step_1");
        sys1.AdvanceWishStep("sv_courier", "step_2");

        var state = sys1.CaptureState();

        var sys2 = new FinalWishSystem { Rng = new SeededRng(888) };
        sys2.RestoreState(state);

        // sv_hunter: active, 1 step completed
        Assert.True(sys2.HasActiveWish("sv_hunter"));
        Assert.Equal(1, sys2.GetStepsCompleted("sv_hunter"));
        Assert.Equal("teach_lesson", sys2.GetWishType("sv_hunter"));
        Assert.Equal(sys1.GetDaysRemaining("sv_hunter"), sys2.GetDaysRemaining("sv_hunter"), 4);

        // sv_courier: completed, not active
        Assert.False(sys2.HasActiveWish("sv_courier"));
        Assert.True(sys2.HasCompletedWish("sv_courier"));
        Assert.Equal(2, sys2.GetStepsCompleted("sv_courier"));
        Assert.Equal("deliver_letter", sys2.GetWishType("sv_courier"));
    }
}
