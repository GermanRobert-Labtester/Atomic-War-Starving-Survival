// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Inventory;
using Ashfall.Core.Radiation;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;
using Xunit;

namespace Ashfall.Core.Tests.Campaign
{
    public sealed class Plan17_21BriefingProtectiveIntegrationTests
    {
        [Fact]
        public void DayEventVocabulary_ClassifiesSemanticKinds_AndFiltersHeartbeats()
        {
            // Heartbeat events must be classified as Heartbeat
            Assert.Equal(SemanticKind.Heartbeat, DayEventVocabulary.GetSemanticKind("power_ticked"));
            Assert.Equal(SemanticKind.Heartbeat, DayEventVocabulary.GetSemanticKind("needs_ticked"));

            // Consequence / transitional events must not be suppressed heartbeats
            var radiationBreachKind = DayEventVocabulary.GetSemanticKind("radiation_storm_hit");
            Assert.NotEqual(SemanticKind.Heartbeat, radiationBreachKind);

            // Generic section title is stable
            Assert.Equal("System Activity", DayEventVocabulary.GenericSectionTitle);
        }

        [Fact]
        public void DailyBriefingReportBuilder_BuildsConsequenceSections_FromDayEvents()
        {
            var events = new[]
            {
                new DayStateChangeEvent("medical_admitted", "medical_ward", "surv_01", "bed_general_a", 5),
                new DayStateChangeEvent("duty_vacated", "duty_roster", "surv_01", "night_watch", 5)
            };

            var report = DailyBriefingReportBuilder.BuildFromDayEvents(5, 42, events);
            Assert.NotNull(report);
            Assert.NotEmpty(report.Sections);

            // Briefing entries must contain attributed causes
            var allEntries = report.Sections.SelectMany(s => s.Entries).ToList();
            Assert.Contains(allEntries, e => e.Text.Contains("medical ward") || e.Text.Contains("night_watch"));
        }

        [Fact]
        public void ProtectiveGear_DegradesThroughConditionSink_AndScalesProtection()
        {
            var inventory = new InventoryContainer { Capacity = 10, MaxWeight = 100f };

            var gasMaskDef = new ItemDefinition
            {
                id = "gas_mask_mk1",
                displayName = "Civilian Gas Mask",
                isEquipable = true,
                equipSlot = EquipSlot.Face,
                durability = 100f,
                radProtection = 50f,
                degradeRate = 1f,
                tags = new List<string> { "protective", "mask" }
            };

            // Equip gas mask
            inventory.Add(gasMaskDef, 1);
            Assert.True(inventory.Equip(gasMaskDef));

            var equipped = inventory.Equipped.FirstOrDefault(e => e.Item.id == "gas_mask_mk1");
            Assert.NotNull(equipped);
            Assert.Equal(100f, equipped.CurrentDurability);

            // Populate WornGear projection
            var wornBuffer = new List<WornGear>();
            inventory.FillWornGear(wornBuffer);
            Assert.Single(wornBuffer);
            var worn = wornBuffer[0];
            Assert.Equal(50f, worn.EffectiveProtection());

            // Degrade gear via canonical condition sink
            bool failedEventFired = false;
            inventory.OnProtectiveGearFailed += (item, cause) => failedEventFired = true;

            inventory.RecordWear(equipped, wearDelta: 40f, cause: "fallout_exposure");
            Assert.Equal(60f, equipped.CurrentDurability);
            Assert.False(failedEventFired);

            // Refresh worn projection and verify scaled protection (60% of 50 = 30)
            inventory.FillWornGear(wornBuffer);
            Assert.Equal(30f, wornBuffer[0].EffectiveProtection(), precision: 1);

            // Degrade past zero
            inventory.RecordWear(equipped, wearDelta: 70f, cause: "acid_rain");
            Assert.Equal(0f, equipped.CurrentDurability);
            Assert.True(failedEventFired);

            inventory.FillWornGear(wornBuffer);
            Assert.Equal(0f, wornBuffer[0].EffectiveProtection());
        }

        [Fact]
        public void InventorySave_PreservesDegradedConditionRoundTrip()
        {
            var original = new InventoryContainer { Capacity = 10, MaxWeight = 100f };
            var hazmatDef = new ItemDefinition
            {
                id = "hazmat_suit_heavy",
                displayName = "Heavy Hazmat Suit",
                isEquipable = true,
                equipSlot = EquipSlot.Body,
                durability = 200f,
                radProtection = 80f,
                degradeRate = 1f,
                tags = new List<string> { "protective", "suit" }
            };

            original.Add(hazmatDef, 1);
            Assert.True(original.Equip(hazmatDef));

            var equipped = original.Equipped.FirstOrDefault(e => e.Item.id == "hazmat_suit_heavy");
            Assert.NotNull(equipped);

            // Wear down to 75 durability
            original.RecordWear(equipped, wearDelta: 125f, cause: "hot_zone_salvage");
            Assert.Equal(75f, equipped.CurrentDurability);

            // Capture state
            var state = original.CaptureState();
            Assert.NotNull(state);

            // Restore into new inventory
            var restored = new InventoryContainer();
            var catalog = new Dictionary<string, ItemDefinition> { [hazmatDef.id] = hazmatDef };
            restored.RestoreState(state, id => catalog.TryGetValue(id, out var d) ? d : null);

            var restoredEquipped = restored.Equipped.FirstOrDefault(e => e.Item.id == "hazmat_suit_heavy");
            Assert.NotNull(restoredEquipped);
            Assert.Equal(75f, restoredEquipped.CurrentDurability);

            var wornBuffer = new List<WornGear>();
            restored.FillWornGear(wornBuffer);
            Assert.Single(wornBuffer);
            // 75 / 200 * 80 = 30f
            Assert.Equal(30f, wornBuffer[0].EffectiveProtection(), precision: 1);
        }
    }
}
