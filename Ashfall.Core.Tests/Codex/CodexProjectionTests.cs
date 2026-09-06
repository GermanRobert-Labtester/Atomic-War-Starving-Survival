using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Codex;
using Ashfall.Core.Journal;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.Codex
{
    public class CodexProjectionTests
    {
        [Fact]
        public void Projection_Aggregates_FromMultipleSources()
        {
            // 1. Setup Field Guide
            var fgCatalog = FieldGuideCatalog.LoadFromJson(@"{
                ""schema_version"": 1,
                ""entries"": [
                    {
                        ""id"": ""fg_glowing_lichen"",
                        ""common_name"": ""Glowing Lichen"",
                        ""scientific_name"": ""Caloplaca radiata"",
                        ""category"": ""Flora"",
                        ""observation"": ""Thrives near reactor fissures; emits faint bioluminescence."",
                        ""tags"": [""radiotrophic"", ""medicinal""]
                    },
                    {
                        ""id"": ""fg_blind_mole_rat"",
                        ""common_name"": ""Blind Mole Rat"",
                        ""scientific_name"": ""Spalax mutant"",
                        ""category"": ""Fauna"",
                        ""observation"": ""Subterranean scavenger."",
                        ""tags"": [""mammal"", ""prey""]
                    }
                ]
            }");
            // Unlock only lichen
            fgCatalog.UnlockEntry("fg_glowing_lichen");

            // 2. Setup Research
            var researchState = new ResearchState
            {
                completedIds = new List<string> { "tech_water_distillation" },
                activeResearchId = "tech_radon_scrubber",
                unlockedIds = new List<string> { "tech_solar_array" }
            };
            var researchCatalog = new Dictionary<string, ResearchKnowledgeDef>
            {
                ["tech_water_distillation"] = new ResearchKnowledgeDef("tech_water_distillation", "Solar Water Distillation", "Water", "Purifies heavy brine using solar heat.", 3),
                ["tech_radon_scrubber"] = new ResearchKnowledgeDef("tech_radon_scrubber", "Electrostatic Radon Scrubber", "Air", "Reduces radon contamination by 60%.", 5),
                ["tech_solar_array"] = new ResearchKnowledgeDef("tech_solar_array", "Photovoltaic Array", "Power", "Provides 40W auxiliary power.", 4)
            };

            // 3. Setup Journal
            var journal = new JournalSystem();
            journal.Knowledge.Discover(KnowledgeKeys.ColdCountBeforeTheLab);
            journal.UnlockLocationVisited("loc_silo_complex");

            // Build projection
            var entries = CodexProjectionBuilder.Build(fgCatalog, researchState, researchCatalog, journal, currentDay: 7);

            Assert.NotEmpty(entries);

            // Verify Ecology (Field Guide)
            var lichen = entries.FirstOrDefault(e => e.EntryId == "codex_fg_fg_glowing_lichen");
            Assert.NotNull(lichen);
            Assert.Equal(CodexCategory.Ecology, lichen.Category);
            Assert.Equal("Glowing Lichen", lichen.Title);
            Assert.Equal(CodexEntryState.Known, lichen.State);
            Assert.Contains("radiotrophic", lichen.Tags);

            // Blind mole rat was NOT unlocked, must not be projected
            Assert.Null(entries.FirstOrDefault(e => e.EntryId == "codex_fg_fg_blind_mole_rat"));

            // Verify Technology: Completed (Known)
            var waterTech = entries.FirstOrDefault(e => e.EntryId == "codex_tech_tech_water_distillation");
            Assert.NotNull(waterTech);
            Assert.Equal(CodexCategory.Technology, waterTech.Category);
            Assert.Equal(CodexEntryState.Known, waterTech.State);
            Assert.Equal("Purifies heavy brine using solar heat.", waterTech.Body);

            // Verify Technology: Active (Studying - masked text)
            var radonTech = entries.FirstOrDefault(e => e.EntryId == "codex_tech_tech_radon_scrubber");
            Assert.NotNull(radonTech);
            Assert.Equal(CodexEntryState.Studying, radonTech.State);
            Assert.Equal("Under active laboratory analysis...", radonTech.Body);

            // Verify Technology: Unlocked (Locked)
            var solarTech = entries.FirstOrDefault(e => e.EntryId == "codex_tech_tech_solar_array");
            Assert.NotNull(solarTech);
            Assert.Equal(CodexEntryState.Locked, solarTech.State);

            // Verify Wasteland Lore (Journal)
            var lore = entries.FirstOrDefault(e => e.EntryId == $"codex_journal_{KnowledgeKeys.ColdCountBeforeTheLab}");
            Assert.NotNull(lore);
            Assert.Equal(CodexCategory.WastelandLore, lore.Category);
            Assert.Equal(CodexEntryState.Known, lore.State);
        }

        [Fact]
        public void Deduplication_And_ProvenanceUnion()
        {
            var journal = new JournalSystem();
            journal.Knowledge.Discover("history_continuity_reclamation_decree");

            // Build projection
            var entries = CodexProjectionBuilder.Build(null, null, null, journal, currentDay: 10);
            var decree = entries.FirstOrDefault(e => e.EntryId == "codex_journal_history_continuity_reclamation_decree");
            Assert.NotNull(decree);
            Assert.Single(decree.Provenance);

            // Second build with simulated corroborated source
            var prov1 = new CampaignProvenanceRecord(KnowledgeSourceKind.JournalEvidence, "doc_archive_01", "journal", 3, InformationConfidence.Medium);
            var prov2 = new CampaignProvenanceRecord(KnowledgeSourceKind.RadioIntercept, "intercept_tower", "radio", 5, InformationConfidence.Confirmed);

            var mergedProv = CampaignProvenanceEvaluator.Merge(new[] { prov1 }, new[] { prov2 });
            Assert.Equal(2, mergedProv.Count);
        }

        [Fact]
        public void Deterministic_Ordering_PreservesStrictSequence()
        {
            var journal = new JournalSystem();
            journal.Knowledge.Discover(KnowledgeKeys.QuartermastersPaperwork);
            journal.Knowledge.Discover(KnowledgeKeys.HydroBaronRateCardOrigin);
            journal.Knowledge.Discover(KnowledgeKeys.HighCo2);

            var researchState = new ResearchState
            {
                completedIds = new List<string> { "tech_b", "tech_a" }
            };
            var researchCatalog = new Dictionary<string, ResearchKnowledgeDef>
            {
                ["tech_b"] = new ResearchKnowledgeDef("tech_b", "B-Tech", "General", "Desc B", 1),
                ["tech_a"] = new ResearchKnowledgeDef("tech_a", "A-Tech", "General", "Desc A", 1)
            };

            var run1 = CodexProjectionBuilder.Build(null, researchState, researchCatalog, journal, currentDay: 1);
            var run2 = CodexProjectionBuilder.Build(null, researchState, researchCatalog, journal, currentDay: 1);

            Assert.Equal(run1.Count, run2.Count);
            for (int i = 0; i < run1.Count; i++)
            {
                Assert.Equal(run1[i].EntryId, run2[i].EntryId);
                Assert.Equal(run1[i].Category, run2[i].Category);
                Assert.Equal(run1[i].State, run2[i].State);
                Assert.Equal(run1[i].Title, run2[i].Title);
            }
        }
    }
}
