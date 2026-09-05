// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Inventory;
using Ashfall.Core.IO;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    using SeededRng = Ashfall.Core.SeededRng;

    public sealed class NuclearCoreLifecycleTests : CatalogTestBase
    {
        private (NuclearCoreCatalog catalog, Inventory.Inventory inv) CreateFixture()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var cat = NuclearCoreCatalogLoader.Load(DataDirectory, files, json);
            Assert.NotNull(cat);

            var inv = new Inventory.Inventory();
            return (cat!, inv);
        }

        [Fact]
        public void Catalog_LoadsSixProfiles_AndValidatesAllFields()
        {
            var (catalog, _) = CreateFixture();
            Assert.Equal(6, catalog.Profiles.Count);

            foreach (var prof in catalog.Profiles.Values)
            {
                bool valid = prof.Validate(out string err);
                Assert.True(valid, $"Profile '{prof.id}' failed validation: {err}");
            }
        }

        [Fact]
        public void Catalog_EmergencyShutdownItems_ExistInItemCatalogs()
        {
            var (catalog, _) = CreateFixture();
            var allItemIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var itemFile in Directory.GetFiles(DataDirectory, "*item*.json"))
            {
                var doc = System.Text.Json.JsonDocument.Parse(File.ReadAllText(itemFile));
                if (doc.RootElement.TryGetProperty("items", out var items))
                {
                    foreach (var it in items.EnumerateArray())
                    {
                        if (it.TryGetProperty("id", out var idProp))
                        {
                            string id = idProp.GetString() ?? "";
                            if (!string.IsNullOrEmpty(id)) allItemIds.Add(id);
                        }
                    }
                }
            }

            foreach (var prof in catalog.Profiles.Values)
            {
                Assert.True(allItemIds.Contains(prof.emergencyShutdownItemId),
                    $"Core '{prof.id}' requires shutdown item '{prof.emergencyShutdownItemId}' which does not exist in item catalogs.");
            }
        }

        [Fact]
        public void TryInstallCore_RegistersCore_InitializesState()
        {
            var (catalog, inv) = CreateFixture();
            var sys = new NuclearCoreLifecycleSystem(inv, catalog, new SeededRng(42));

            bool installed = sys.TryInstallCore("core_unit_01", "core_strontium_rtg_100w", "sub_vault");
            Assert.True(installed);

            var core = sys.GetCore("core_unit_01");
            Assert.NotNull(core);
            Assert.Equal("core_strontium_rtg_100w", core!.profileId);
            Assert.Equal("sub_vault", core.roomId);
            Assert.Equal(100.0f, core.shieldingIntegrity);
            Assert.False(core.isScrammed);
        }

        [Fact]
        public void DuplicateCoreId_Blocked()
        {
            var (catalog, inv) = CreateFixture();
            var sys = new NuclearCoreLifecycleSystem(inv, catalog, new SeededRng(42));

            sys.TryInstallCore("core_unit_01", "core_strontium_rtg_100w");
            bool dup = sys.TryInstallCore("core_unit_01", "core_strontium_rtg_100w");
            Assert.False(dup);
        }

        [Fact]
        public void PassiveRTG_GeneratesPowerWithoutFuelOrCooling()
        {
            var (catalog, inv) = CreateFixture();
            var sys = new NuclearCoreLifecycleSystem(inv, catalog, new SeededRng(42));

            sys.TryInstallCore("core_rtg", "core_strontium_rtg_100w");
            // RTG output is 100W passively
            Assert.Equal(100.0f, sys.GetTotalGenerationWatts());

            sys.TickDay(1); // Tick without coolant
            var core = sys.GetCore("core_rtg")!;
            Assert.Equal("Nominal", core.heatState);
            Assert.Equal("Sufficient", core.coolantState);
            Assert.Equal(100.0f, sys.GetTotalGenerationWatts());
        }

        [Fact]
        public void ActiveReactor_RespectsOutputSettings()
        {
            var (catalog, inv) = CreateFixture();
            var sys = new NuclearCoreLifecycleSystem(inv, catalog, new SeededRng(42));

            sys.TryInstallCore("core_pebble", "core_naval_pebble_bed_2kw");
            var core = sys.GetCore("core_pebble")!;
            Assert.Equal("Shutdown", core.outputSetting);
            Assert.Equal(0.0f, sys.GetTotalGenerationWatts());

            sys.SetOutputSetting("core_pebble", "Low");
            Assert.Equal(1000.0f, sys.GetTotalGenerationWatts()); // 2000 * 0.5

            sys.SetOutputSetting("core_pebble", "Normal");
            Assert.Equal(2000.0f, sys.GetTotalGenerationWatts()); // 2000 * 1.0

            sys.SetOutputSetting("core_pebble", "High");
            Assert.Equal(3000.0f, sys.GetTotalGenerationWatts()); // 2000 * 1.5
        }

        [Fact]
        public void ActiveReactor_ConsumesCoolant()
        {
            var (catalog, inv) = CreateFixture();
            float coolantRequested = 0f;
            var sys = new NuclearCoreLifecycleSystem(inv, catalog, new SeededRng(42),
                coolantProvider: amount =>
                {
                    coolantRequested += amount;
                    return true;
                });

            sys.TryInstallCore("core_triga", "core_research_triga_5kw");
            sys.SetOutputSetting("core_triga", "Normal");

            sys.TickDay(1);
            Assert.Equal(35.0f, coolantRequested); // TRIGA normal cooling demand is 35.0
        }

        [Fact]
        public void CoolantDepletion_ElevatesHeatState()
        {
            var (catalog, inv) = CreateFixture();
            var sys = new NuclearCoreLifecycleSystem(inv, catalog, new SeededRng(42),
                coolantProvider: _ => false); // Always starved

            sys.TryInstallCore("core_pebble", "core_naval_pebble_bed_2kw");
            sys.SetOutputSetting("core_pebble", "Normal");

            sys.TickDay(1);
            var core = sys.GetCore("core_pebble")!;
            Assert.Equal("Elevated", core.heatState);
            Assert.Equal("Depleted", core.coolantState);

            sys.TickDay(2);
            Assert.Equal("Critical", core.heatState);
        }

        [Fact]
        public void ShieldingDegradation_EmitsRadiationLeakage()
        {
            var (catalog, inv) = CreateFixture();
            float totalLeakDose = 0f;
            var sys = new NuclearCoreLifecycleSystem(inv, catalog, new SeededRng(42),
                onRadiationLeakage: (room, dose) => totalLeakDose += dose);

            sys.TryInstallCore("core_pebble", "core_naval_pebble_bed_2kw");
            var core = sys.GetCore("core_pebble")!;
            core.shieldingIntegrity = 40.0f; // Pebble bed requirement is 60.0f -> deficit 20.0f

            sys.SetOutputSetting("core_pebble", "Normal");
            sys.TickDay(1);

            Assert.True(totalLeakDose > 0f, "Deficit shielding should emit radiation leakage.");
        }

        [Fact]
        public void TryRepairShielding_ConsumesLeadSheets_RestoresIntegrity()
        {
            var (catalog, inv) = CreateFixture();
            var sys = new NuclearCoreLifecycleSystem(inv, catalog, new SeededRng(42));

            sys.TryInstallCore("core_pebble", "core_naval_pebble_bed_2kw");
            var core = sys.GetCore("core_pebble")!;
            core.shieldingIntegrity = 30.0f;

            inv.AddById("lead_sheet", 2);

            bool repaired = sys.TryRepairShielding("core_pebble");
            Assert.True(repaired);
            Assert.Equal(100.0f, core.shieldingIntegrity);
            Assert.Equal(0, inv.CountById("lead_sheet"));
        }

        [Fact]
        public void TryRepairShielding_InsufficientLead_NoPartialLoss()
        {
            var (catalog, inv) = CreateFixture();
            var sys = new NuclearCoreLifecycleSystem(inv, catalog, new SeededRng(42));

            sys.TryInstallCore("core_pebble", "core_naval_pebble_bed_2kw");
            var core = sys.GetCore("core_pebble")!;
            core.shieldingIntegrity = 30.0f;

            inv.AddById("lead_sheet", 1); // Needs 2

            bool repaired = sys.TryRepairShielding("core_pebble");
            Assert.False(repaired);
            Assert.Equal(30.0f, core.shieldingIntegrity);
            Assert.Equal(1, inv.CountById("lead_sheet"));
        }

        [Fact]
        public void TryEmergencyScram_ConsumesBoronCanister_ShutsDownReactor()
        {
            var (catalog, inv) = CreateFixture();
            var sys = new NuclearCoreLifecycleSystem(inv, catalog, new SeededRng(42));

            sys.TryInstallCore("core_pebble", "core_naval_pebble_bed_2kw");
            sys.SetOutputSetting("core_pebble", "High");

            inv.AddById("scram_boron_canister", 1);

            bool scrammed = sys.TryEmergencyScram("core_pebble");
            Assert.True(scrammed);

            var core = sys.GetCore("core_pebble")!;
            Assert.True(core.isScrammed);
            Assert.Equal("Shutdown", core.outputSetting);
            Assert.Equal(0.0f, sys.GetTotalGenerationWatts());
            Assert.Equal(0, inv.CountById("scram_boron_canister"));
        }

        [Fact]
        public void TryEmergencyScram_InsufficientCanister_Blocked()
        {
            var (catalog, inv) = CreateFixture();
            var sys = new NuclearCoreLifecycleSystem(inv, catalog, new SeededRng(42));

            sys.TryInstallCore("core_pebble", "core_naval_pebble_bed_2kw");
            sys.SetOutputSetting("core_pebble", "High");

            bool scrammed = sys.TryEmergencyScram("core_pebble");
            Assert.False(scrammed);

            var core = sys.GetCore("core_pebble")!;
            Assert.False(core.isScrammed);
            Assert.Equal("High", core.outputSetting);
        }

        [Fact]
        public void ScrammedReactor_CannotAlterOutputSetting()
        {
            var (catalog, inv) = CreateFixture();
            var sys = new NuclearCoreLifecycleSystem(inv, catalog, new SeededRng(42));

            sys.TryInstallCore("core_pebble", "core_naval_pebble_bed_2kw");
            inv.AddById("scram_boron_canister", 1);
            sys.TryEmergencyScram("core_pebble");

            bool change = sys.SetOutputSetting("core_pebble", "Normal");
            Assert.False(change);
            Assert.Equal("Shutdown", sys.GetCore("core_pebble")!.outputSetting);
        }

        [Fact]
        public void SaveRestore_PreservesCoreWearShieldingAndScramStatus()
        {
            var (catalog, inv) = CreateFixture();
            var sysA = new NuclearCoreLifecycleSystem(inv, catalog, new SeededRng(42));

            sysA.TryInstallCore("core_triga", "core_research_triga_5kw", "deep_vault");
            var coreA = sysA.GetCore("core_triga")!;
            coreA.shieldingIntegrity = 72.5f;
            coreA.embrittlementWear = 14.2f;
            coreA.outputSetting = "Low";

            var save = sysA.CaptureState();

            var sysB = new NuclearCoreLifecycleSystem(inv, catalog, new SeededRng(999));
            sysB.RestoreState(save);

            var coreB = sysB.GetCore("core_triga")!;
            Assert.Equal("core_research_triga_5kw", coreB.profileId);
            Assert.Equal("deep_vault", coreB.roomId);
            Assert.Equal(72.5f, coreB.shieldingIntegrity);
            Assert.Equal(14.2f, coreB.embrittlementWear);
            Assert.Equal("Low", coreB.outputSetting);
        }
    }
}
