using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Catalogs;
using Ashfall.Core.Culture;
using Ashfall.Core.Diplomacy;
using Ashfall.Core.Sanatorium;
using Ashfall.Core.Inventory;
using Ashfall.Core.SkyDefense;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Flagship institutions (Tasks 5-8) Phase B gates: all four authored
    /// catalogs load with expected counts, item references resolve against
    /// the live item catalog, and malformed fixtures fail loudly through
    /// InstitutionCatalogException with deterministically ordered findings.
    /// </summary>
    public class InstitutionCatalogValidationTests
    {
        private static string DataDir
        {
            get
            {
                // Walk parents so this passes both from Ashfall.Core.Tests/bin and
                // from the repo-root _verify_flagship gate output.
                if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out string found))
                    return found;
                throw new InvalidOperationException("could not locate Assets/StreamingAssets/Data from " + AppContext.BaseDirectory);
            }
        }

        private static (IFileIO Files, IJsonSerializer Json) Ports() =>
            (new FileSystemIO(), new SystemTextJsonSerializer());

        // ------------------------------------------------------------------
        // NORMAL — the four shipped catalogs load with authored counts.
        // ------------------------------------------------------------------

        [Fact]
        public void TomeCatalog_LoadsTwelveTomes_FromShippedData()
        {
            var (files, json) = Ports();
            var tomes = CulturalArchiveTomeCatalogLoader.Load(DataDir, files, json);
            Assert.Equal(CulturalArchiveTomeCatalogLoader.ExpectedTomeCount, tomes.Count);
            Assert.All(tomes, t => Assert.False(string.IsNullOrEmpty(t.tome_id)));
            Assert.Contains(tomes, t => t.tome_id == "tome_mechanics_handbook_1974");
        }

        [Fact]
        public void TreatyCatalog_LoadsEightFrameworks_FromShippedData()
        {
            var (files, json) = Ports();
            var treaties = DiplomaticTreatyCatalogLoader.Load(DataDir, files, json);
            Assert.Equal(DiplomaticTreatyCatalogLoader.ExpectedTreatyCount, treaties.Count);
            Assert.Contains(treaties, t => t.treaty_id == "treaty_non_aggression_compact");
            Assert.Contains(treaties, t => t.IsDmzZone("high_scarp_ridgeline"));
        }

        [Fact]
        public void OrdnanceCatalog_LoadsSixDefinitions_FromShippedData()
        {
            var (files, json) = Ports();
            var ordnance = SkyDefenseOrdnanceCatalogLoader.Load(DataDir, files, json);
            Assert.Equal(SkyDefenseOrdnanceCatalogLoader.ExpectedOrdnanceCount, ordnance.Count);
            Assert.Contains(ordnance, o => o.ordnance_id == "ammo_76mm_proximity_fuse");
        }

        [Fact]
        public void TherapyCatalog_LoadsEightTherapies_AndConditions_FromShippedData()
        {
            var (files, json) = Ports();
            var container = PsychologicalTherapyCatalogLoader.Load(DataDir, files, json);
            Assert.Equal(PsychologicalTherapyCatalogLoader.ExpectedTherapyCount, container.therapies.Count);
            Assert.True(container.conditions.Count >= 6);
            Assert.All(container.therapies, t => Assert.True(t.eligible_conditions!.Count > 0));
        }

        // ------------------------------------------------------------------
        // Item references resolve against the live item catalog authority.
        // ------------------------------------------------------------------

        [Fact]
        public void AllCatalogItemReferences_ResolveAgainstItemCatalog()
        {
            var (files, json) = Ports();
            var itemCatalog = ItemCatalogLoader.LoadCatalog(DataDir, files, json);

            var tomes = CulturalArchiveTomeCatalogLoader.Load(DataDir, files, json);
            var treaties = DiplomaticTreatyCatalogLoader.Load(DataDir, files, json);
            var ordnance = SkyDefenseOrdnanceCatalogLoader.Load(DataDir, files, json);
            var therapyContainer = PsychologicalTherapyCatalogLoader.Load(DataDir, files, json);

            var missing = new List<string>();

            foreach (var t in tomes)
            {
                foreach (var c in t.restoration_costs ?? new())
                    if (itemCatalog.Get(c.item_id) == null) missing.Add($"{t.tome_id}/restoration/{c.item_id}");
                foreach (var c in t.microfiche_costs ?? new())
                    if (itemCatalog.Get(c.item_id) == null) missing.Add($"{t.tome_id}/microfiche/{c.item_id}");
            }
            foreach (var t in treaties)
                foreach (var c in t.required_concessions ?? new())
                    if (itemCatalog.Get(c.item_id) == null) missing.Add($"{t.treaty_id}/concession/{c.item_id}");
            foreach (var o in ordnance)
                if (itemCatalog.Get(o.item_id) == null) missing.Add($"{o.ordnance_id}/item/{o.item_id}");
            foreach (var t in therapyContainer.therapies)
                foreach (var c in t.resource_costs ?? new())
                    if (itemCatalog.Get(c.item_id) == null) missing.Add($"{t.therapy_id}/cost/{c.item_id}");

            Assert.True(missing.Count == 0,
                "unresolved item references: " + string.Join(", ", missing));
        }

        // ------------------------------------------------------------------
        // INVALID — malformed fixtures fail loudly (deterministic findings).
        // ------------------------------------------------------------------

        private static List<CulturalArchiveTomeDefinition> Tome(params CulturalArchiveTomeDefinition[] defs) =>
            new(defs);

        private static CulturalArchiveTomeDefinition ValidTome(string id = "tome_valid") => new()
        {
            tome_id = id,
            display_name = "Valid Tome",
            category = "technical",
            transcription_days = 2,
            paper_brittleness_tier = 2,
            initial_degradation_permille = 100,
            microfiche_frame_density = 48,
            knowledge_bonus = 1,
            morale_effect = 0.5f,
        };

        [Fact]
        public void TomeCatalog_DuplicateIds_Fail()
        {
            var f = new InstitutionCatalogParse.Findings();
            var ex = Assert.Throws<InstitutionCatalogException>(() =>
                CulturalArchiveTomeCatalogLoader.Validate("tomes.json", Tome(ValidTome("tome_a"), ValidTome("tome_a"))));
            Assert.Contains(ex.Findings, m => m.Contains("duplicate"));
        }

        [Fact]
        public void TomeCatalog_InvalidRanges_FailWithDeterministicOrder()
        {
            var bad = ValidTome("tome_bad");
            bad.transcription_days = 0;
            bad.paper_brittleness_tier = 9;
            bad.initial_degradation_permille = 2000;
            bad.morale_effect = 99f;

            var ex = Assert.Throws<InstitutionCatalogException>(() =>
                CulturalArchiveTomeCatalogLoader.Validate("tomes.json", Tome(bad)));
            // deterministic aggregate: sorted by (id, field)
            Assert.Equal(new[]
            {
                "tome_bad/initial_degradation_permille",
                "tome_bad/morale_effect",
                "tome_bad/paper_brittleness_tier",
                "tome_bad/transcription_days",
            }, ex.Findings.Select(x => x.Split(':')[0].Trim()).ToArray());
        }

        [Fact]
        public void TomeCatalog_NegativeCostAmount_Fails()
        {
            var bad = ValidTome();
            bad.restoration_costs = new()
            {
                new InstitutionCatalogParse.CatalogCostEntry { item_id = "clean_water", amount = -1 }
            };
            var ex = Assert.Throws<InstitutionCatalogException>(() =>
                CulturalArchiveTomeCatalogLoader.Validate("tomes.json", Tome(bad)));
            Assert.Contains(ex.Findings, m => m.Contains("restoration_costs[0].amount"));
        }

        [Fact]
        public void TreatyCatalog_UnknownDmzZone_FailsWhenZoneListProvided()
        {
            var bad = new DiplomaticTreatyDefinition
            {
                treaty_id = "treaty_bad_zone",
                display_name = "Bad Zone Treaty",
                minimum_signatories = 2,
                duration_days = 10,
                stability_rating = 50,
                dmz_zone_ids = new() { "zone_this_does_not_exist" },
            };
            var ex = Assert.Throws<InstitutionCatalogException>(() =>
                {
                    DiplomaticTreatyCatalogLoader.Validate("treaties.json", new List<DiplomaticTreatyDefinition> { bad },
                        new HashSet<string>(StringComparer.Ordinal) { "high_scarp_ridgeline" });
                });
            Assert.Contains(ex.Findings, m => m.Contains("zone_this_does_not_exist"));
        }

        [Fact]
        public void TreatyCatalog_NoTreaties_Fails()
        {
            var ex = Assert.Throws<InstitutionCatalogException>(() =>
                {
                    DiplomaticTreatyCatalogLoader.Validate("treaties.json", new List<DiplomaticTreatyDefinition>());
                });
            Assert.Contains(ex.Findings, m => m.Contains("no treaty frameworks"));
        }

        [Fact]
        public void OrdnanceCatalog_DuplicateOrdnance_Fails()
        {
            var valid = new SkyDefenseOrdnanceDefinition
            {
                ordnance_id = "ammo_valid",
                display_name = "Valid",
                ammo_type = "flak",
                item_id = "ammo_valid",
                magazine_units = 4,
                heat_per_volley = 10,
                radar_lock_units = 1,
            };
            var ex = Assert.Throws<InstitutionCatalogException>(() =>
                {
                    SkyDefenseOrdnanceCatalogLoader.Validate("ordnance.json", new List<SkyDefenseOrdnanceDefinition> { valid, valid });
                });
            Assert.Contains(ex.Findings, m => m.Contains("duplicate"));
        }

        [Fact]
        public void TherapyCatalog_UnknownConditionRef_Fails()
        {
            var conditions = new List<TherapyConditionDefinition>()
            {
                new TherapyConditionDefinition
                {
                    condition_id = "condition_known",
                    display_name = "Known",
                    canonical_surface = "none",
                },
            };
            var therapies = new List<PsychologicalTherapyDefinition>()
            {
                new PsychologicalTherapyDefinition
                {
                    therapy_id = "therapy_bad_ref",
                    display_name = "Bad Ref",
                    duration_days = 1,
                    staff_skill_id = "skill_watchful",
                    eligible_conditions = new() { "condition_not_authored" },
                },
            };
            var ex = Assert.Throws<InstitutionCatalogException>(() =>
                PsychologicalTherapyCatalogLoader.Validate("therapies.json", conditions, therapies));
            Assert.Contains(ex.Findings, m => m.Contains("condition_not_authored"));
        }

        [Fact]
        public void TherapyCatalog_TherapyWithoutEligibleCondition_Fails()
        {
            var therapies = new List<PsychologicalTherapyDefinition>()
            {
                new PsychologicalTherapyDefinition
                {
                    therapy_id = "therapy_no_conditions",
                    display_name = "No Conditions",
                    duration_days = 1,
                    staff_skill_id = "skill_watchful",
                    eligible_conditions = new(),
                },
            };
            var ex = Assert.Throws<InstitutionCatalogException>(() =>
                {
                    PsychologicalTherapyCatalogLoader.Validate("therapies.json", new List<TherapyConditionDefinition>(), therapies);
                });
            Assert.Contains(ex.Findings, m => m.Contains("at least one eligible condition"));
        }

        [Fact]
        public void TherapyCatalog_UnknownCanonicalSurface_Fails()
        {
            var conditions = new List<TherapyConditionDefinition>()
            {
                new TherapyConditionDefinition
                {
                    condition_id = "condition_bad_surface",
                    display_name = "Bad",
                    canonical_surface = "aura_flavor",
                },
            };
            var ex = Assert.Throws<InstitutionCatalogException>(() =>
                {
                    PsychologicalTherapyCatalogLoader.Validate("therapies.json", conditions, new List<PsychologicalTherapyDefinition>());
                });
            Assert.Contains(ex.Findings, m => m.Contains("canonical_surface"));
        }

        [Fact]
        public void MissingCatalogFile_FailsLoudly()
        {
            var (files, json) = Ports();
            string emptyDir = Path.Combine(Path.GetTempPath(), "ashfall_flagship_missing_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(emptyDir);
            try
            {
                Assert.Throws<InstitutionCatalogException>(() =>
                    CulturalArchiveTomeCatalogLoader.Load(emptyDir, files, json));
            }
            finally
            {
                Directory.Delete(emptyDir, recursive: true);
            }
        }
    }
}
