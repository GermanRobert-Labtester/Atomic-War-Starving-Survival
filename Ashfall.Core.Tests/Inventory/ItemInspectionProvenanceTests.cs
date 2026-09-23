// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Xunit;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Tests
{
    public sealed class ItemInspectionProvenanceTests
    {
        [Fact]
        public void InspectionModel_WithoutProvenance_HasProvenanceIsFalse()
        {
            var def = new ItemDefinition
            {
                id = "item_wrench",
                displayName = "Pipe Wrench",
                type = ItemType.Tool
            };

            var model = ItemInspectionModel.Create(def);

            Assert.False(model.HasProvenance);
            Assert.Null(model.Provenance);
            Assert.Equal("item_wrench", model.ItemId);
            Assert.Equal("Pipe Wrench", model.DisplayName);
        }

        [Fact]
        public void InspectionModel_WithProvenance_ExposesProvenanceChain()
        {
            var def = new ItemDefinition
            {
                id = "item_rifle",
                displayName = "Hunting Rifle",
                type = ItemType.Weapon
            };

            var loreSystem = new ItemLoreSystem();
            var prov = loreSystem.RegisterItem(
                "instance_rifle_01",
                crafterId: "survivor_viktor",
                craftingDay: 4,
                context: "Handmade in machine shop");
            loreSystem.TransferOwnership("instance_rifle_01", "survivor_elena", day: 10);

            var model = ItemInspectionModel.Create(
                def,
                catalog: null,
                enrichment: null,
                loreSystem: loreSystem,
                itemInstanceId: "instance_rifle_01");

            Assert.True(model.HasProvenance);
            Assert.NotNull(model.Provenance);
            Assert.Equal("instance_rifle_01", model.Provenance!.ItemInstanceId);
            Assert.Equal("survivor_viktor", model.Provenance.CrafterSurvivorId);
            Assert.Equal(4, model.Provenance.CraftingDay);
            Assert.Equal(2, model.Provenance.OwnershipChain.Count);
            Assert.Equal("survivor_viktor", model.Provenance.OwnershipChain[0]);
            Assert.Equal("survivor_elena", model.Provenance.OwnershipChain[1]);
        }

        [Fact]
        public void InspectionModel_WithUnregisteredInstanceId_HasProvenanceReturnsFalse()
        {
            var def = new ItemDefinition
            {
                id = "item_knife",
                displayName = "Combat Knife",
                type = ItemType.Weapon
            };

            var loreSystem = new ItemLoreSystem();

            var model = ItemInspectionModel.Create(
                def,
                catalog: null,
                enrichment: null,
                loreSystem: loreSystem,
                itemInstanceId: "instance_nonexistent");

            Assert.False(model.HasProvenance);
            Assert.Null(model.Provenance);
        }
    }
}
