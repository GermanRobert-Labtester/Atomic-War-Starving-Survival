// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.IO;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Progression
{
    /// <summary>
    /// Pins that trade_specialties.json content survives the loader boundary.
    /// Before this the DTOs parsed display_name, milestone titles, milestone
    /// narrative ids, mastery_narrative, mastery_bonus_text and skill_bonus and
    /// then dropped all six: LoadAndRegister kept only profession id + patterns,
    /// and the host re-derived the mastery narrative id by string interpolation.
    /// </summary>
    public sealed class TradeSpecialtyCatalogMetadataTests
    {
        private const string ElectricianTier1 = "narrative_electrician_milestone_1";
        private const string ElectricianTier2 = "narrative_electrician_milestone_2";
        private const string ElectricianTier3 = "narrative_electrician_mastery";
        private const string ElectricianMastery = "narrative_trade_mastery_electrician";
        private const string UnlistedProfession = "unlisted_profession";

        /// <summary>Never used by another test; kept out of the shared static tables.</summary>
        private const string TestProfession = "metadata_test_profession";

        private static string ResolveDataDir()
        {
            string baseDir = AppContext.BaseDirectory;
            string probe = Path.Combine(baseDir, "Assets", "StreamingAssets", "Data");
            if (Directory.Exists(probe)) return probe;

            string dir = baseDir;
            for (int i = 0; i < 6; i++)
            {
                probe = Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(probe)) return probe;
                var parent = Directory.GetParent(dir);
                if (parent == null) break;
                dir = parent.FullName;
            }
            return probe;
        }

        private static List<TradeSpecialtyItemDto> LoadCatalog()
        {
            return TradeSpecialtyCatalogLoader.Load(
                ResolveDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
        }

        private static TradeSpecialtySystem LoadAndRegister()
        {
            var system = new TradeSpecialtySystem();
            int count = TradeSpecialtyCatalogLoader.LoadAndRegister(
                system, ResolveDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.Equal(16, count);
            return system;
        }

        [Fact]
        public void LoadAndRegister_RetainsEveryAuthoredContentField()
        {
            var dtos = LoadCatalog();
            Assert.Equal(16, dtos.Count);
            LoadAndRegister();

            int titles = 0, narratives = 0;
            foreach (var dto in dtos)
            {
                var info = TradeSpecialtySystem.GetProfessionInfo(dto.ProfessionId);
                Assert.NotNull(info);
                Assert.Equal(dto.DisplayName, info!.DisplayName);
                Assert.Equal(dto.MasteryNarrative, info.MasteryNarrativeId);
                Assert.Equal(dto.MasteryBonusText, info.MasteryBonusText);
                Assert.False(string.IsNullOrWhiteSpace(info.DisplayName),
                    $"{dto.ProfessionId} lost its display_name");
                Assert.False(string.IsNullOrWhiteSpace(info.MasteryNarrativeId),
                    $"{dto.ProfessionId} lost its mastery_narrative");
                Assert.False(string.IsNullOrWhiteSpace(info.MasteryBonusText),
                    $"{dto.ProfessionId} lost its mastery_bonus_text");

                Assert.Equal(dto.Milestones.Count, info.Milestones.Count);
                foreach (var m in dto.Milestones)
                {
                    var retained = TradeSpecialtySystem.GetMilestone(dto.ProfessionId, m.Tier);
                    Assert.NotNull(retained);
                    Assert.Equal(m.Title, retained!.Title);
                    Assert.Equal(m.Narrative, retained.NarrativeId);
                    Assert.Equal(m.SkillBonus, retained.SkillBonus);
                    Assert.False(string.IsNullOrWhiteSpace(retained.Title),
                        $"{dto.ProfessionId} tier {m.Tier} lost its title");
                    Assert.False(string.IsNullOrWhiteSpace(retained.NarrativeId),
                        $"{dto.ProfessionId} tier {m.Tier} lost its narrative id");
                    titles++;
                    narratives++;
                }
            }

            Assert.Equal(48, titles);
            Assert.Equal(48, narratives);
        }

        [Fact]
        public void GetMasteryNarrativeId_ComesFromCatalog_NotFromInterpolation()
        {
            var dtos = LoadCatalog();
            LoadAndRegister();

            foreach (var dto in dtos)
            {
                Assert.Equal(dto.MasteryNarrative,
                    TradeSpecialtySystem.GetMasteryNarrativeId(dto.ProfessionId));
                Assert.False(string.IsNullOrWhiteSpace(dto.MasteryNarrative),
                    $"{dto.ProfessionId} has no authored mastery_narrative");
            }
        }

        [Fact]
        public void GetMasteryNarrativeId_UnknownProfession_ReturnsEmpty()
        {
            LoadAndRegister();
            Assert.Equal(string.Empty, TradeSpecialtySystem.GetMasteryNarrativeId(UnlistedProfession));
            Assert.Equal(string.Empty, TradeSpecialtySystem.GetDisplayName(UnlistedProfession));
            Assert.Equal(string.Empty, TradeSpecialtySystem.GetMasteryBonusText(UnlistedProfession));
            Assert.Equal(string.Empty, TradeSpecialtySystem.GetMilestoneTitle(UnlistedProfession, 1));
            Assert.Equal(string.Empty, TradeSpecialtySystem.GetMilestoneNarrativeId(UnlistedProfession, 1));
            Assert.Null(TradeSpecialtySystem.GetProfessionInfo(UnlistedProfession));
            Assert.Null(TradeSpecialtySystem.GetProfessionInfo(string.Empty));
        }

        [Fact]
        public void GetMasteryNarrativeId_MatchesCatalogForElectrician()
        {
            LoadAndRegister();
            Assert.Equal(ElectricianMastery, TradeSpecialtySystem.GetMasteryNarrativeId("electrician"));
            Assert.Equal("Electrician", TradeSpecialtySystem.GetDisplayName("electrician"));
            Assert.Equal("Apprentice Spark", TradeSpecialtySystem.GetMilestoneTitle("electrician", 1));
            Assert.Equal("Journeyman Current", TradeSpecialtySystem.GetMilestoneTitle("electrician", 2));
            Assert.Equal("Master of the Grid", TradeSpecialtySystem.GetMilestoneTitle("electrician", 3));
            Assert.Equal("Master of the Grid", TradeSpecialtySystem.GetMilestone("electrician", 3)!.Title);
            Assert.Contains("{name}", TradeSpecialtySystem.GetMasteryBonusText("electrician"));
        }

        [Fact]
        public void OnItemCrafted_FiresAuthoredTierNarrativeForEachMilestone()
        {
            var system = LoadAndRegister();
            var fired = new List<string>();
            system.FireNarrativeEvent = (id, sv) => fired.Add(id);

            system.OnItemCrafted("survivor_a", "electrician", "item_battery_cell");
            system.OnItemCrafted("survivor_a", "electrician", "item_solar_cell");

            Assert.Equal(new[] { ElectricianTier1, ElectricianTier2 }, fired);
            Assert.Equal(2, system.GetMasteryTier("survivor_a"));
            Assert.False(system.HasMasteredTrade("survivor_a"));
        }

        [Fact]
        public void OnItemCrafted_MasteryFiresTierThreeAndMasteryNarratives()
        {
            var system = LoadAndRegister();
            var fired = new List<string>();
            system.FireNarrativeEvent = (id, sv) => fired.Add(id);

            system.OnItemCrafted("survivor_b", "electrician", "item_battery_cell");
            system.OnItemCrafted("survivor_b", "electrician", "item_solar_cell");
            system.OnItemCrafted("survivor_b", "electrician", "item_advanced_circuit");

            Assert.True(system.HasMasteredTrade("survivor_b"));
            Assert.Equal(
                new[] { ElectricianTier1, ElectricianTier2, ElectricianTier3, ElectricianMastery },
                fired);
        }

        [Fact]
        public void OnItemCrafted_UnregisteredProfession_FiresNoNarrative()
        {
            var system = LoadAndRegister();
            var fired = new List<string>();
            system.FireNarrativeEvent = (id, sv) => fired.Add(id);

            // Pattern matching still works off the legacy hardcoded table, but no
            // authored narrative exists for this profession, so nothing may fire.
            // Unique id + cleanup: ProfessionItemCategories is process-static and
            // OnItemCrafted_UnknownProfession_Ignored depends on "unlisted_profession"
            // staying empty.
            TradeSpecialtySystem.RegisterProfessionPatterns(TestProfession, new[] { "testwidget" });
            try
            {
                system.OnItemCrafted("survivor_c", TestProfession, "testwidget_one");
                system.OnItemCrafted("survivor_c", TestProfession, "testwidget_two");
                system.OnItemCrafted("survivor_c", TestProfession, "testwidget_three");

                Assert.True(system.HasMasteredTrade("survivor_c"));
                Assert.Empty(fired);
            }
            finally
            {
                TradeSpecialtySystem.ProfessionItemCategories.Remove(TestProfession);
            }
        }

        [Fact]
        public void MasterTrade_FallsBackToHostHook_OnlyWhenCatalogHasNoEntry()
        {
            var system = LoadAndRegister();
            var fired = new List<string>();
            system.FireNarrativeEvent = (id, sv) => fired.Add(id);

            // Catalog-covered profession: the authored id wins, the hook is never consulted.
            bool hookCalled = false;
            system.GetNarrativeEventId = prof => { hookCalled = true; return "hook_should_not_win"; };
            system.OnItemCrafted("survivor_f", "electrician", "item_battery_cell");
            system.OnItemCrafted("survivor_f", "electrician", "item_solar_cell");
            system.OnItemCrafted("survivor_f", "electrician", "item_advanced_circuit");

            Assert.Contains(ElectricianMastery, fired);
            Assert.DoesNotContain("hook_should_not_win", fired);
            Assert.False(hookCalled);

            // Uncatalogued profession: the legacy host hook still supplies the id.
            fired.Clear();
            TradeSpecialtySystem.RegisterProfessionPatterns(TestProfession, new[] { "testwidget" });
            try
            {
                system.GetNarrativeEventId = prof => prof == TestProfession ? "hook_fallback_id" : string.Empty;
                system.OnItemCrafted("survivor_g", TestProfession, "testwidget_one");
                system.OnItemCrafted("survivor_g", TestProfession, "testwidget_two");
                system.OnItemCrafted("survivor_g", TestProfession, "testwidget_three");

                Assert.Equal(new[] { "hook_fallback_id" }, fired);
            }
            finally
            {
                TradeSpecialtySystem.ProfessionItemCategories.Remove(TestProfession);
            }
        }

        [Fact]
        public void OnItemCrafted_NoHookAssigned_DoesNotThrow()
        {
            var system = LoadAndRegister();
            system.OnItemCrafted("survivor_d", "electrician", "item_battery_cell");
            system.OnItemCrafted("survivor_d", "electrician", "item_solar_cell");
            system.OnItemCrafted("survivor_d", "electrician", "item_advanced_circuit");
            Assert.True(system.HasMasteredTrade("survivor_d"));
        }

        [Fact]
        public void RegisterProfessionInfo_ReplacesEntryWithoutDuplicating()
        {
            LoadAndRegister();
            int before = TradeSpecialtySystem.ProfessionInfo.Count;

            try
            {
                TradeSpecialtySystem.RegisterProfessionInfo(new TradeSpecialtyProfessionInfo
                {
                    ProfessionId = TestProfession,
                    DisplayName = "First"
                });
                Assert.Equal(before + 1, TradeSpecialtySystem.ProfessionInfo.Count);

                TradeSpecialtySystem.RegisterProfessionInfo(new TradeSpecialtyProfessionInfo
                {
                    ProfessionId = TestProfession,
                    DisplayName = "Second"
                });

                Assert.Equal(before + 1, TradeSpecialtySystem.ProfessionInfo.Count);
                Assert.Equal("Second", TradeSpecialtySystem.GetDisplayName(TestProfession));
            }
            finally
            {
                TradeSpecialtySystem.ProfessionInfo.Remove(TestProfession);
            }

            Assert.Equal(before, TradeSpecialtySystem.ProfessionInfo.Count);
        }

        [Fact]
        public void RegisterProfessionInfo_NullOrEmptyId_IsNoOp()
        {
            int before = TradeSpecialtySystem.ProfessionInfo.Count;
            TradeSpecialtySystem.RegisterProfessionInfo(null);
            TradeSpecialtySystem.RegisterProfessionInfo(new TradeSpecialtyProfessionInfo());
            TradeSpecialtySystem.RegisterProfessionInfo(
                new TradeSpecialtyProfessionInfo { ProfessionId = string.Empty });
            Assert.Equal(before, TradeSpecialtySystem.ProfessionInfo.Count);
        }

        [Fact]
        public void IntermediateMilestones_GrantAuthoredSkillBonus_MasteryKeepsConstant()
        {
            var dtos = LoadCatalog();
            var system = LoadAndRegister();

            // Authored value retained and queryable for every milestone.
            Assert.Equal(0.05f, TradeSpecialtySystem.GetMilestone("electrician", 1)!.SkillBonus, 3);
            foreach (var dto in dtos)
                foreach (var m in dto.Milestones)
                    Assert.Equal(m.SkillBonus,
                        TradeSpecialtySystem.GetMilestone(dto.ProfessionId, m.Tier)!.SkillBonus, 3);

            var grants = new List<float>();
            system.GrantSkillBonus = (sv, prof, amount) => grants.Add(amount);

            system.OnItemCrafted("survivor_g", "electrician", "item_battery_cell");
            system.OnItemCrafted("survivor_g", "electrician", "item_solar_cell");
            system.OnItemCrafted("survivor_g", "electrician", "item_advanced_circuit");

            Assert.True(system.HasMasteredTrade("survivor_g"));
            Assert.Equal(3, grants.Count);
            // Tiers 1-2 take the authored 0.05. Mastery keeps MasterySkillBonus
            // because the catalog authors no separate mastery bonus, and applying
            // the milestone value at tier 3 would cut the payoff to a third.
            Assert.Equal(0.05f, grants[0], 3);
            Assert.Equal(0.05f, grants[1], 3);
            Assert.Equal(TradeSpecialtySystem.MasterySkillBonus, grants[2], 3);
        }

        [Fact]
        public void UncataloguedProfession_FallsBackToDerivedMilestoneBonus()
        {
            var system = LoadAndRegister();
            var grants = new List<float>();
            system.GrantSkillBonus = (sv, prof, amount) => grants.Add(amount);

            TradeSpecialtySystem.RegisterProfessionPatterns(TestProfession, new[] { "testwidget" });
            try
            {
                system.OnItemCrafted("survivor_h", TestProfession, "testwidget_one");

                Assert.Single(grants);
                Assert.Equal(
                    TradeSpecialtySystem.MasterySkillBonus * TradeSpecialtySystem.MilestoneSkillBonusFactor,
                    grants[0], 3);
            }
            finally
            {
                TradeSpecialtySystem.ProfessionItemCategories.Remove(TestProfession);
            }
        }

        [Fact]
        public void RuntimeMasteryConstants_AreUnchanged()
        {
            Assert.Equal(0.15f, TradeSpecialtySystem.MasterySkillBonus);
            Assert.Equal(0.3f, TradeSpecialtySystem.MilestoneSkillBonusFactor);
            Assert.Equal(10f, TradeSpecialtySystem.MasteryMoraleBonus);
            Assert.Equal(3, TradeSpecialtySystem.MilestonesToMaster);
        }

        [Fact]
        public void CaptureRestore_RoundTripsWithContentRegistryPopulated()
        {
            var system = LoadAndRegister();
            system.OnItemCrafted("survivor_e", "machinist", "wrench_standard");
            system.OnItemCrafted("survivor_e", "machinist", "gear_standard");
            system.OnItemCrafted("survivor_e", "machinist", "lever_standard");

            var save = system.CaptureState();
            var restored = new TradeSpecialtySystem();
            restored.RestoreState(save);

            Assert.True(restored.HasMasteredTrade("survivor_e"));
            Assert.Equal(3, restored.GetMasteryTier("survivor_e"));
            Assert.Equal(TradeSpecialtySystem.SystemId, save.systemId);
        }

        [Fact]
        public void AllAuthoredNarrativeIds_AreNonEmptyAndDistinct()
        {
            var dtos = LoadCatalog();
            var ids = new HashSet<string>(StringComparer.Ordinal);
            foreach (var dto in dtos)
            {
                Assert.True(ids.Add(dto.MasteryNarrative),
                    $"duplicate mastery_narrative: {dto.MasteryNarrative}");
                foreach (var m in dto.Milestones)
                    Assert.True(ids.Add(m.Narrative),
                        $"duplicate milestone narrative: {m.Narrative}");
            }
            Assert.Equal(64, ids.Count);
        }

        // ── Profession label → specialty resolution ────────────────────

        [Serializable]
        private sealed class SurvivorsRoot
        {
            public List<SurvivorRow> survivors { get; set; } = new List<SurvivorRow>();
        }

        [Serializable]
        private sealed class SurvivorRow
        {
            public string id { get; set; } = string.Empty;
            public string profession { get; set; } = string.Empty;
        }

        private static List<SurvivorRow> LoadSurvivors()
        {
            string path = Path.Combine(ResolveDataDir(), "survivors.json");
            var root = new SystemTextJsonSerializer().Deserialize<SurvivorsRoot>(File.ReadAllText(path));
            Assert.NotNull(root);
            return root!.survivors;
        }

        [Fact]
        public void ProfessionAliases_AreUniqueAcrossSpecialties()
        {
            // A label claimed by two specialties would make resolution depend on
            // catalog iteration order, which the ordinal rebuild is meant to avoid.
            var dtos = LoadCatalog();
            var seen = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var dto in dtos)
            {
                foreach (var alias in dto.ProfessionAliases)
                {
                    Assert.False(seen.TryGetValue(alias, out var other),
                        $"alias '{alias}' claimed by both '{other}' and '{dto.ProfessionId}'");
                    seen[alias] = dto.ProfessionId;
                }
            }
            Assert.Equal(42, seen.Count);
        }

        [Fact]
        public void ProfessionAliases_AllMatchRealSurvivorLabels()
        {
            var labels = new HashSet<string>(
                LoadSurvivors().Select(s => s.profession), StringComparer.OrdinalIgnoreCase);

            foreach (var dto in LoadCatalog())
                foreach (var alias in dto.ProfessionAliases)
                    Assert.Contains(alias, labels);
        }

        [Fact]
        public void ResolveProfessionIdFromLabel_MapsAuthoredLabels()
        {
            LoadAndRegister();

            Assert.Equal("bone_setter", TradeSpecialtySystem.ResolveProfessionIdFromLabel("Trauma Surgeon"));
            Assert.Equal("salvage_appraiser", TradeSpecialtySystem.ResolveProfessionIdFromLabel("Scavenger"));
            Assert.Equal("machinist", TradeSpecialtySystem.ResolveProfessionIdFromLabel("CNC Operator"));
            Assert.Equal("wireman", TradeSpecialtySystem.ResolveProfessionIdFromLabel("Telecomm Tech"));
            Assert.Equal("greenhouse_grower", TradeSpecialtySystem.ResolveProfessionIdFromLabel("Fungus Farmer"));
            Assert.Equal("water_technician", TradeSpecialtySystem.ResolveProfessionIdFromLabel("Pump Specialist"));

            // Case-insensitive and trimmed, matching the index comparer.
            Assert.Equal("nurse", TradeSpecialtySystem.ResolveProfessionIdFromLabel("  paramedic "));
        }

        [Fact]
        public void ResolveProfessionId_ExplicitIdWinsOverLabel()
        {
            LoadAndRegister();
            Assert.Equal("electrician",
                TradeSpecialtySystem.ResolveProfessionId("electrician", "Scavenger"));
            Assert.Equal("salvage_appraiser",
                TradeSpecialtySystem.ResolveProfessionId(string.Empty, "Scavenger"));
            Assert.Equal("nurse",
                TradeSpecialtySystem.ResolveProfessionId(null, "Nurse"));
        }

        [Fact]
        public void ResolveProfessionId_UnmappedLabel_ReturnsEmpty()
        {
            LoadAndRegister();
            foreach (var label in new[] { "Politician", "Child", "Addict", "Guard", "Storyteller" })
                Assert.Equal(string.Empty, TradeSpecialtySystem.ResolveProfessionIdFromLabel(label));

            Assert.Equal(string.Empty, TradeSpecialtySystem.ResolveProfessionIdFromLabel(null));
            Assert.Equal(string.Empty, TradeSpecialtySystem.ResolveProfessionIdFromLabel("   "));
        }

        [Fact]
        public void Roster_ReachesThirteenOfSixteenSpecialties()
        {
            LoadAndRegister();

            var resolved = LoadSurvivors()
                .Select(s => TradeSpecialtySystem.ResolveProfessionId(string.Empty, s.profession))
                .ToList();
            var reached = resolved.Where(id => !string.IsNullOrEmpty(id)).Distinct().ToList();

            // Known gap: miller, apiarist and metallurgist have no authored alias
            // because no roster profession label fits grain milling, beekeeping or
            // smelting. Update this pin deliberately when those are cast.
            Assert.Equal(13, reached.Count);
            Assert.Equal(59, resolved.Count(id => !string.IsNullOrEmpty(id)));
            foreach (var orphan in new[] { "miller", "apiarist", "metallurgist" })
                Assert.DoesNotContain(orphan, reached);
        }

        [Fact]
        public void EverySpecialtyEntry_DeclaresProfessionAliases()
        {
            // Schema uniformity: the key is authored on all 16 entries (empty where
            // nothing maps) so a missing key is never mistaken for a deliberate gap.
            string raw = File.ReadAllText(Path.Combine(ResolveDataDir(), "trade_specialties.json"));
            int occurrences = raw.Split("\"profession_aliases\"").Length - 1;
            Assert.Equal(16, occurrences);
        }

        // ── Craft matching (exposed for unassigned-craft attribution) ────

        [Fact]
        public void ProfessionMatchesItem_MatchesAuthoredPatterns()
        {
            LoadAndRegister();

            Assert.True(TradeSpecialtySystem.ProfessionMatchesItem("electrician", "item_battery_cell"));
            Assert.True(TradeSpecialtySystem.ProfessionMatchesItem("electrician", "wire_copper"));
            Assert.False(TradeSpecialtySystem.ProfessionMatchesItem("electrician", "bandage_clean"));
            Assert.True(TradeSpecialtySystem.ProfessionMatchesItem("nurse", "antiseptic_bottle"));
            Assert.True(TradeSpecialtySystem.ProfessionMatchesItem("machinist", "wrench_standard"));

            // Case-insensitive substring match, matching OnItemCrafted.
            Assert.True(TradeSpecialtySystem.ProfessionMatchesItem("electrician", "SOLAR_CELL"));
        }

        [Fact]
        public void ProfessionMatchesItem_EmptyOrUnknownInputs_ReturnFalse()
        {
            LoadAndRegister();

            Assert.False(TradeSpecialtySystem.ProfessionMatchesItem(string.Empty, "wire_copper"));
            Assert.False(TradeSpecialtySystem.ProfessionMatchesItem("electrician", string.Empty));
            Assert.False(TradeSpecialtySystem.ProfessionMatchesItem(null!, "wire_copper"));
            Assert.False(TradeSpecialtySystem.ProfessionMatchesItem("electrician", null!));
            Assert.False(TradeSpecialtySystem.ProfessionMatchesItem(UnlistedProfession, "wire_copper"));
        }

        [Fact]
        public void ProfessionMatchesItem_AgreesWithOnItemCraftedForEveryProfession()
        {
            // Guards the refactor: OnItemCrafted now delegates to this predicate, so
            // the two must never disagree or attribution would credit crafts that
            // cannot progress (or skip ones that can).
            var dtos = LoadCatalog();
            LoadAndRegister();

            string[] probeItems =
            {
                "item_battery_cell", "wire_copper", "bandage_clean", "wrench_standard",
                "book_childrens", "seed_packet", "scrap_alloy", "charcoal_filter", "needle_bone"
            };

            int checkedPairs = 0;
            foreach (var dto in dtos)
            {
                foreach (var item in probeItems)
                {
                    bool predicted = TradeSpecialtySystem.ProfessionMatchesItem(dto.ProfessionId, item);

                    var system = new TradeSpecialtySystem();
                    system.OnItemCrafted("survivor_probe", dto.ProfessionId, item);
                    bool advanced = system.GetMasteryTier("survivor_probe") == 1;

                    Assert.Equal(predicted, advanced);
                    checkedPairs++;
                }
            }

            Assert.Equal(dtos.Count * probeItems.Length, checkedPairs);
            Assert.True(checkedPairs > 0);
        }

        [Fact]
        public void ProfessionMatchesItem_RejectsSubstringAccidents_AcceptsInflection()
        {
            LoadAndRegister();

            // Whole-token anchoring: a short pattern must not match inside a word.
            Assert.False(TradeSpecialtySystem.ProfessionMatchesItem("radio_technician", "item_pickled_tubers"));
            Assert.False(TradeSpecialtySystem.ProfessionMatchesItem("teacher", "item_comm_codebook_alpha"));

            // Inflection is still accepted, so authored items keep matching.
            Assert.True(TradeSpecialtySystem.ProfessionMatchesItem("preservationist", "item_brined_legume_mash"));
            Assert.True(TradeSpecialtySystem.ProfessionMatchesItem("preservationist", "item_salted_meat"));
            Assert.True(TradeSpecialtySystem.ProfessionMatchesItem("preservationist", "item_smoked_meat"));
            Assert.True(TradeSpecialtySystem.ProfessionMatchesItem("greenhouse_grower", "item_cloud_seeding_canister"));

            // Exact token, plural, and a token that is not the first one.
            Assert.True(TradeSpecialtySystem.ProfessionMatchesItem("electrician", "battery"));
            Assert.True(TradeSpecialtySystem.ProfessionMatchesItem("electrician", "battery_pack"));
            Assert.True(TradeSpecialtySystem.ProfessionMatchesItem("electrician", "solar_cells"));
            Assert.True(TradeSpecialtySystem.ProfessionMatchesItem("electrician", "item_car_battery"));

            // Suffix-like tails that are not inflections stay rejected.
            Assert.False(TradeSpecialtySystem.ProfessionMatchesItem("electrician", "batteryfluid"));
            Assert.False(TradeSpecialtySystem.ProfessionMatchesItem("electrician", "wiremesh_panel"));
        }

        [Serializable]
        private sealed class RecipesRoot
        {
            public List<RecipeRow> recipes { get; set; } = new List<RecipeRow>();
        }

        [Serializable]
        private sealed class RecipeRow
        {
            public string resultItemId { get; set; } = string.Empty;
        }

        [Fact]
        public void ReachabilityPin_MasteryRequiresThreeCraftableMatches()
        {
            var dtos = LoadCatalog();
            LoadAndRegister();

            string path = Path.Combine(ResolveDataDir(), "recipes.json");
            var root = new SystemTextJsonSerializer().Deserialize<RecipesRoot>(File.ReadAllText(path));
            Assert.NotNull(root);
            var craftable = root!.recipes
                .Where(r => r != null && !string.IsNullOrEmpty(r.resultItemId))
                .Select(r => r.resultItemId)
                .Distinct()
                .ToList();
            Assert.True(craftable.Count > 0);

            var masterable = new List<string>();
            int deficit = 0;
            foreach (var dto in dtos)
            {
                int matches = craftable.Count(id =>
                    TradeSpecialtySystem.ProfessionMatchesItem(dto.ProfessionId, id));
                if (matches >= TradeSpecialtySystem.MilestonesToMaster)
                    masterable.Add(dto.ProfessionId);
                deficit += Math.Max(0, TradeSpecialtySystem.MilestonesToMaster - matches);
            }

            // KNOWN CONTENT GAP, measured 2026-09-17. Mastery needs 3 distinct
            // craftable matches, and only recipes.json feeds CraftingSystem — the
            // workshop / relic / pharma / metallurgy / glassworks catalogs run on
            // separate systems whose completions never reach the specialty bridge.
            // 33 more craftable items are needed to make all 16 masterable. This pin
            // exists so the gap cannot be silently forgotten: update it as content lands.
            masterable.Sort(StringComparer.Ordinal);
            Assert.Equal(new[] { "electrician", "preservationist" }, masterable);
            Assert.Equal(33, deficit);
        }
    }
}
