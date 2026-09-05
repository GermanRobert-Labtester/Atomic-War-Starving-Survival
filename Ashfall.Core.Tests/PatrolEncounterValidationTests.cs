// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Narrative;

namespace Ashfall.Core.Tests
{
    public class PatrolEncounterValidationTests
    {
        private readonly string _dataDir;
        private readonly FileSystemIO _fileIO;
        private readonly TravelEncounterCatalog _catalog;
        private readonly HashSet<string> _factions;
        private readonly HashSet<string> _items;

        public PatrolEncounterValidationTests()
        {
            _dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "StreamingAssets", "Data");
            if (!Directory.Exists(_dataDir))
            {
                _dataDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data"));
            }
            _fileIO = new FileSystemIO();
            _catalog = TravelEncounterCatalog.LoadFromDirectory(_dataDir, _fileIO);

            _factions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            string lorePath = Path.Combine(_dataDir, "faction_lore.json");
            if (File.Exists(lorePath))
            {
                using var doc = JsonDocument.Parse(File.ReadAllText(lorePath));
                if (doc.RootElement.TryGetProperty("items", out var itemsArr))
                {
                    foreach (var f in itemsArr.EnumerateArray())
                    {
                        if (f.TryGetProperty("id", out var idProp))
                            _factions.Add(idProp.GetString()!);
                        if (f.TryGetProperty("faction_id", out var fidProp))
                            _factions.Add(fidProp.GetString()!);
                    }
                }
            }
            _factions.Add("iron_garrison");
            _factions.Add("ash_militia");
            _factions.Add("cult_of_ash_sign");
            _factions.Add("warlords_sector_4");
            _factions.Add("military_remnants");
            _factions.Add("upland_militia");

            _items = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            string itemsPath = Path.Combine(_dataDir, "items.json");
            if (File.Exists(itemsPath))
            {
                using var doc = JsonDocument.Parse(File.ReadAllText(itemsPath));
                if (doc.RootElement.TryGetProperty("items", out var itemsArr))
                {
                    foreach (var it in itemsArr.EnumerateArray())
                    {
                        if (it.TryGetProperty("id", out var idProp))
                            _items.Add(idProp.GetString()!);
                    }
                }
            }
        }

        [Fact]
        public void ProductionCatalog_PassesValidationWithZeroErrors()
        {
            var errors = PatrolEncounterValidator.Validate(_catalog.Encounters, _factions, _items);
            Assert.Empty(errors);
        }

        private TravelEncounterDefinition CreateValidPatrol(string id = "enc_patrol_test_checkpoint")
        {
            return new TravelEncounterDefinition
            {
                Id = id,
                Title = "Test Checkpoint",
                Category = "Human",
                FactionId = "iron_garrison",
                TerritoryState = "controlled",
                CooldownGroup = "patrol_test_checkpoint",
                RegionTags = new List<string> { "high_scarp" },
                SeasonTags = new List<string> { "all" },
                MinDangerLevel = 1.0f,
                MaxDangerLevel = 3.0f,
                BaseWeight = 1.0f,
                Choices = new List<TravelEncounterChoice>
                {
                    new TravelEncounterChoice
                    {
                        ChoiceId = "choice_comply",
                        Text = "Comply with inspection.",
                        IsNonviolent = true,
                        IsAvoidance = false,
                        MoraleDelta = 0,
                        GuiltDelta = 0,
                        FactionId = "iron_garrison",
                        FactionStandingDelta = 1,
                        CostItems = new List<string> { "canned_food" }
                    },
                    new TravelEncounterChoice
                    {
                        ChoiceId = "choice_avoid",
                        Text = "Avoid the checkpoint.",
                        IsNonviolent = true,
                        IsAvoidance = true,
                        MoraleDelta = -1,
                        GuiltDelta = 0
                    }
                }
            };
        }

        [Fact]
        public void MalformedFixture_NonHumanCategory_Reported()
        {
            var enc = CreateValidPatrol();
            enc.Category = "Beast";
            var errors = PatrolEncounterValidator.Validate(new[] { enc }, _factions, _items);
            Assert.Contains(errors, e => e.Contains("must have category 'Human'"));
        }

        [Fact]
        public void MalformedFixture_UnknownFaction_Reported()
        {
            var enc = CreateValidPatrol();
            enc.FactionId = "phantom_faction_99";
            var errors = PatrolEncounterValidator.Validate(new[] { enc }, _factions, _items);
            Assert.Contains(errors, e => e.Contains("references unknown faction"));
        }

        [Fact]
        public void MalformedFixture_InvalidTerritoryState_Reported()
        {
            var enc = CreateValidPatrol();
            enc.TerritoryState = "lawless_zone";
            var errors = PatrolEncounterValidator.Validate(new[] { enc }, _factions, _items);
            Assert.Contains(errors, e => e.Contains("invalid territory_state"));
        }

        [Fact]
        public void MalformedFixture_ChoiceCountOutOfRange_Reported()
        {
            var enc = CreateValidPatrol();
            enc.Choices.RemoveAt(0); // only 1 choice left
            var errors = PatrolEncounterValidator.Validate(new[] { enc }, _factions, _items);
            Assert.Contains(errors, e => e.Contains("between 2 and 4 choices"));
        }

        [Fact]
        public void MalformedFixture_DuplicateChoiceId_Reported()
        {
            var enc = CreateValidPatrol();
            enc.Choices[1].ChoiceId = enc.Choices[0].ChoiceId;
            var errors = PatrolEncounterValidator.Validate(new[] { enc }, _factions, _items);
            Assert.Contains(errors, e => e.Contains("duplicate choice_id"));
        }

        [Fact]
        public void MalformedFixture_DuplicateChoiceText_Reported()
        {
            var enc = CreateValidPatrol();
            enc.Choices[1].Text = enc.Choices[0].Text;
            var errors = PatrolEncounterValidator.Validate(new[] { enc }, _factions, _items);
            Assert.Contains(errors, e => e.Contains("duplicate choice text"));
        }

        [Fact]
        public void MalformedFixture_StandingDeltaOutOfRange_Reported()
        {
            var enc = CreateValidPatrol();
            enc.Choices[0].FactionStandingDelta = 25; // max allowed is 10
            var errors = PatrolEncounterValidator.Validate(new[] { enc }, _factions, _items);
            Assert.Contains(errors, e => e.Contains("standing delta 25 outside allowed range"));
        }

        [Fact]
        public void MalformedFixture_UnknownCostItem_Reported()
        {
            var enc = CreateValidPatrol();
            enc.Choices[0].CostItems = new List<string> { "unobtainium_crystal" };
            var errors = PatrolEncounterValidator.Validate(new[] { enc }, _factions, _items);
            Assert.Contains(errors, e => e.Contains("unknown cost item 'unobtainium_crystal'"));
        }

        [Fact]
        public void MalformedFixture_RequiredItemAlsoInCosts_Reported()
        {
            var enc = CreateValidPatrol();
            enc.Choices[0].RequiredItemId = "canned_food";
            enc.Choices[0].RequiredItemQuantity = 1;
            enc.Choices[0].CostItems = new List<string> { "canned_food" };
            var errors = PatrolEncounterValidator.Validate(new[] { enc }, _factions, _items);
            Assert.Contains(errors, e => e.Contains("cannot also be consumed in costs"));
        }

        [Fact]
        public void MalformedFixture_BaseWeightOutOfRange_Reported()
        {
            var enc = CreateValidPatrol();
            enc.BaseWeight = 0.01f;
            var errors = PatrolEncounterValidator.Validate(new[] { enc }, _factions, _items);
            Assert.Contains(errors, e => e.Contains("base_weight 0.01 outside allowed range"));
        }

        [Fact]
        public void MalformedFixture_VariantFamilyDivergence_Reported()
        {
            var v1 = CreateValidPatrol("enc_patrol_test_v1");
            var v2 = CreateValidPatrol("enc_patrol_test_v2");
            v2.Choices[0].MoraleDelta = -5; // Mechanics divergence!

            var errors = PatrolEncounterValidator.Validate(new[] { v1, v2 }, _factions, _items);
            Assert.Contains(errors, e => e.Contains("choice mechanics mismatch"));
        }
    }
}
