// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class SurvivorOriginModifierTests
    {
        [Fact]
        public void UnenrichedSurvivor_ReturnsEmptyModifier()
        {
            var service = new SurvivorEnrichmentService();
            var mod = service.GetOriginModifier("survivor_unknown");

            Assert.Equal("survivor_unknown", mod.SurvivorId);
            Assert.False(mod.HasMechanicalOrigin);
            Assert.Empty(mod.PrimarySkillId);
            Assert.Equal(0, mod.SkillBonus);
            Assert.Empty(mod.TradeSpecialtyId);
            Assert.Empty(mod.GrantedKeepsakeItemId);
        }

        [Fact]
        public void EnrichedNurse_ResolvesMedicalSkillAndTradeSpecialty()
        {
            var catalog = new ExpansionEnrichmentCatalog();
            catalog.AddSurvivorFields(new ExpansionSurvivorFields
            {
                survivor_id = "survivor_nurse_01",
                pre_war_profession_id = "nurse",
                personal_keepsake_item_id = "item_stethoscope",
                phantom_background_id = "nurse"
            });

            var service = new SurvivorEnrichmentService(catalog);
            var mod = service.GetOriginModifier("survivor_nurse_01");

            Assert.Equal("survivor_nurse_01", mod.SurvivorId);
            Assert.True(mod.HasMechanicalOrigin);
            Assert.Equal("medical", mod.PrimarySkillId);
            Assert.Equal(1, mod.SkillBonus);
            Assert.Equal("medical", mod.TradeSpecialtyId);
            Assert.Equal("item_stethoscope", mod.GrantedKeepsakeItemId);
        }

        [Fact]
        public void EnrichedMachinist_ResolvesMechanicSkillAndToolsSpecialty()
        {
            var catalog = new ExpansionEnrichmentCatalog();
            catalog.AddSurvivorFields(new ExpansionSurvivorFields
            {
                survivor_id = "survivor_machinist_01",
                pre_war_profession_id = "machinist",
                personal_keepsake_item_id = "item_brass_calipers",
                phantom_background_id = "machinist"
            });

            var service = new SurvivorEnrichmentService(catalog);
            var mod = service.GetOriginModifier("survivor_machinist_01");

            Assert.Equal("survivor_machinist_01", mod.SurvivorId);
            Assert.True(mod.HasMechanicalOrigin);
            Assert.Equal("mechanic", mod.PrimarySkillId);
            Assert.Equal(1, mod.SkillBonus);
            Assert.Equal("tools", mod.TradeSpecialtyId);
            Assert.Equal("item_brass_calipers", mod.GrantedKeepsakeItemId);
        }

        [Fact]
        public void EnrichedElectrician_ResolvesElectronicsSkillAndComponentsSpecialty()
        {
            var catalog = new ExpansionEnrichmentCatalog();
            catalog.AddSurvivorFields(new ExpansionSurvivorFields
            {
                survivor_id = "survivor_electrician_01",
                pre_war_profession_id = "electrician",
                personal_keepsake_item_id = "item_multimeter",
                phantom_background_id = "electrician"
            });

            var service = new SurvivorEnrichmentService(catalog);
            var mod = service.GetOriginModifier("survivor_electrician_01");

            Assert.Equal("survivor_electrician_01", mod.SurvivorId);
            Assert.True(mod.HasMechanicalOrigin);
            Assert.Equal("electronics", mod.PrimarySkillId);
            Assert.Equal(1, mod.SkillBonus);
            Assert.Equal("components", mod.TradeSpecialtyId);
            Assert.Equal("item_multimeter", mod.GrantedKeepsakeItemId);
        }
    }
}
