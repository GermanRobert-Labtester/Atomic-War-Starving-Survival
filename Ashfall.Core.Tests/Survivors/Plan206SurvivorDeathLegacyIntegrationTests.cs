// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class Plan206SurvivorDeathLegacyIntegrationTests
    {
        private static string GetDataPath(string filename)
        {
            var current = new DirectoryInfo(AppContext.BaseDirectory);
            while (current != null)
            {
                string candidate = Path.Combine(current.FullName, "Assets", "StreamingAssets", "Data", filename);
                if (File.Exists(candidate)) return candidate;
                current = current.Parent;
            }
            return Path.Combine("Assets", "StreamingAssets", "Data", filename);
        }

        [Fact]
        public void DeathRecord_AndWill_DistributesInheritanceCorrectly()
        {
            var system = new SurvivorDeathLegacySystem();

            var will = system.CreateWill(
                survivorId: "surv_mechanic",
                beneficiaries: new[]
                {
                    new BeneficiaryEntry { BeneficiaryId = "surv_apprentice", Category = InheritanceCategory.Tools, Percentage = 100f }
                },
                specialBequests: new[]
                {
                    new SpecialBequest { ItemId = "wrench_golden", RecipientId = "surv_bestfriend" }
                },
                residuaryBeneficiary: "commons",
                currentDay: 1
            );

            Assert.NotNull(will);
            Assert.True(will.IsValid);

            var death = system.RecordDeath(
                survivorId: "surv_mechanic",
                survivorName: "Bob the Builder",
                cause: DeathCause.Accident,
                deathDay: 15,
                location: "Generator Bay"
            );

            Assert.NotNull(death);
            Assert.Equal(1, system.DeathCount);

            var items = system.DistributeInheritance(death.RecordId, new[] { "wrench_golden", "tool_kit", "rations" });
            Assert.Equal(3, items.Count);

            // Special bequest
            Assert.Equal("surv_bestfriend", items.First(i => i.ItemId == "wrench_golden").RecipientId);
            // Beneficiary
            Assert.Equal("surv_apprentice", items.First(i => i.ItemId == "tool_kit").RecipientId);
        }

        [Fact]
        public void InheritanceDispute_Lifecycle_CanBeRaisedAndResolved()
        {
            var system = new SurvivorDeathLegacySystem();
            var will = system.CreateWill("surv_a");

            var disp = system.RaiseDispute(will.WillId, "surv_b", "Unfair division of rations");
            Assert.NotNull(disp);
            Assert.Equal(1, system.ActiveDisputeCount);
            Assert.Equal(DisputeResolution.Pending, disp.Resolution);

            bool resolved = system.ResolveDispute(disp.DisputeId, DisputeResolution.Mediated);
            Assert.True(resolved);
            Assert.Equal(0, system.ActiveDisputeCount);
            Assert.Equal(DisputeResolution.Mediated, disp.Resolution);
        }

        [Fact]
        public void DeathLegacy_PersistenceRoundtrip_PreservesFullState()
        {
            var system = new SurvivorDeathLegacySystem();
            system.CreateWill("surv_1");
            system.RecordDeath("surv_2", "Jane Doe", DeathCause.Radiation, 5);

            var state = system.CaptureState();
            string json = JsonSerializer.Serialize(state);
            var restored = JsonSerializer.Deserialize<SurvivorDeathLegacyState>(json);
            Assert.NotNull(restored);

            var newSystem = new SurvivorDeathLegacySystem();
            newSystem.RestoreState(restored!);

            Assert.Equal(1, newSystem.WillCount);
            Assert.Equal(1, newSystem.DeathCount);
            Assert.Equal("Jane Doe", newSystem.DeathRecords[0].SurvivorName);
        }

        [Fact]
        public void LoadCatalog_FromCanonicalJson_LoadsAllTemplates()
        {
            var system = new SurvivorDeathLegacySystem();
            string jsonPath = GetDataPath("death_legacy_templates.json");

            Assert.True(File.Exists(jsonPath), $"Canonical file must exist at {jsonPath}");
            string json = File.ReadAllText(jsonPath);

            system.LoadCatalog(json);
            Assert.True(system.Templates.Count >= 6);

            var elder = system.GetTemplate("template_elder_guardian");
            Assert.NotNull(elder);
            Assert.Equal(InheritanceCategory.Valuables, elder.GetInheritanceCategory());
            Assert.Equal(0.05f, elder.DisputeProbability);
            Assert.Equal(1.80f, elder.BereavementComfortScale);
            Assert.Equal(100.0f, elder.SentimentalValueBonus);

            var soldier = system.GetTemplate("template_fallen_soldier");
            Assert.NotNull(soldier);
            Assert.Equal(InheritanceCategory.Weapons, soldier.GetInheritanceCategory());
            Assert.Equal(0.20f, soldier.DisputeProbability);
            Assert.Equal(1.50f, soldier.BereavementComfortScale);
        }

        [Fact]
        public void CreateWillFromTemplate_AppliesCategoryDefaults_AndDistributesInheritance()
        {
            var system = new SurvivorDeathLegacySystem();
            string jsonPath = GetDataPath("death_legacy_templates.json");
            system.LoadCatalog(File.ReadAllText(jsonPath));

            var will = system.CreateWillFromTemplate("surv_soldier", "template_fallen_soldier", "surv_cadet", currentDay: 5);
            Assert.NotNull(will);
            Assert.True(will.IsValid);
            Assert.Single(will.Beneficiaries);
            Assert.Equal(InheritanceCategory.Weapons, will.Beneficiaries[0].Category);

            var death = system.RecordDeath("surv_soldier", "Sgt. Miller", DeathCause.Combat, 10);
            var items = system.DistributeInheritance(death.RecordId, new[] { "item_rifle_marksman" });

            Assert.Single(items);
            Assert.Equal("surv_cadet", items[0].RecipientId);
        }

        [Fact]
        public void DistributeInheritance_InvokesInventoryBequestDeliverer()
        {
            var system = new SurvivorDeathLegacySystem();
            var delivered = new List<(string recipient, string item)>();
            system.InventoryBequestDeliverer = (rec, itm) => delivered.Add((rec, itm));

            var will = system.CreateWill(
                survivorId: "surv_craftsman",
                beneficiaries: new[] { new BeneficiaryEntry { BeneficiaryId = "surv_heir", Percentage = 100f } },
                specialBequests: new[] { new SpecialBequest { ItemId = "relic_watch", RecipientId = "surv_friend" } }
            );

            var death = system.RecordDeath("surv_craftsman", "Old Smith", DeathCause.OldAge, 20);
            system.DistributeInheritance(death.RecordId, new[] { "relic_watch", "hammer_steel" });

            Assert.Equal(2, delivered.Count);
            Assert.Contains(("surv_friend", "relic_watch"), delivered);
            Assert.Contains(("surv_heir", "hammer_steel"), delivered);
        }

        [Fact]
        public void BereavementComfort_CalculatesAndAppliesComfort_ToMourningLovedOnes()
        {
            var system = new SurvivorDeathLegacySystem();
            string jsonPath = GetDataPath("death_legacy_templates.json");
            system.LoadCatalog(File.ReadAllText(jsonPath));

            string comfortedSurvivor = "";
            float receivedComfort = 0f;
            system.BereavementComfortApplier = (s, c) =>
            {
                comfortedSurvivor = s;
                receivedComfort = c;
            };

            var will = system.CreateWill(
                survivorId: "surv_deceased",
                beneficiaries: new[] { new BeneficiaryEntry { BeneficiaryId = "surv_griever", Percentage = 100f } }
            );

            var death = system.RecordDeath("surv_deceased", "Elder Mary", DeathCause.OldAge, 30);

            system.ApplyBereavementComfort("surv_griever", "surv_deceased", baseComfort: 10f);

            Assert.Equal("surv_griever", comfortedSurvivor);
            // Beneficiary bonus (1.5x) * Elder comfort scale contribution (1.8 * 0.8 = 1.44) => 10 * 1.5 * 1.44 = 21.6f
            Assert.True(receivedComfort > 15f);
        }
    }
}
