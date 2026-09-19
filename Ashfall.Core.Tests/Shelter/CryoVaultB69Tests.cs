// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan B69 — Cryogenic Sample Preservation &amp; Genetic Cultivar Seed Vault.
    /// Verifies catalog load, atomic registration (no duplication), coolant
    /// economy, thermal decay profiles, radiation sensitivity, insulation
    /// upgrades via B66 shielding plates, recovery pipeline with viability
    /// gating, breach triage, save round-trip without rerolls, legacy-safe
    /// defaults and paired determinism.
    /// </summary>
    public sealed class CryoVaultB69Tests
    {
        private sealed class Harness
        {
            public readonly InventoryContainer Inventory = new InventoryContainer();
            public readonly CryoVaultSystem Vault;
            public bool PowerAvailable = true;
            public float RadiationExposure = 0f;
            public readonly List<string> Warnings = new List<string>();
            public readonly List<string> Released = new List<string>();
            public readonly List<string> Failed = new List<string>();
            public readonly List<string> Breaches = new List<string>();

            public Harness(int seed = 42, CryoVaultSaveState? state = null)
            {
                Inventory.AddById("item_hermetic_sample_ampoule", 50);
                Inventory.AddById("item_nitrogen_supply", 50);
                Inventory.AddById("item_metallurgy_shielding_plate", 10);

                var catalog = CryoCultivarCatalogLoader.Load(FindDataDir());
                Vault = new CryoVaultSystem(
                    new SeededRng(seed),
                    Inventory,
                    () => RadiationExposure,
                    () => PowerAvailable,
                    state: state);
                Vault.LoadCatalogContent(catalog);
                Vault.OnVaultWarning += w => Warnings.Add(w);
                Vault.OnSampleReleased += (c, item, amt, v) => Released.Add(c);
                Vault.OnSampleFailed += c => Failed.Add(c);
                Vault.OnBreachStarted += r => Breaches.Add(r);
            }

            private static string FindDataDir()
            {
                string start = Directory.GetCurrentDirectory();
                if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
                if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
                throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found");
            }
        }

        // ── 1. Catalog ──────────────────────────────────────────────

        [Fact]
        public void Catalog_LoadsEighteenCultivarsWithoutErrors()
        {
            var h = new Harness();
            Assert.Equal(18, h.Vault.Catalog.Count);
        }

        [Fact]
        public void Catalog_AllRecoveryItemsResolveToCanonicalSeeds()
        {
            // Spot-check: every recovery item is a canonical seed/ampoule id
            // owned by greenhouse/pharma — the vault creates no parallel registry.
            var h = new Harness();
            var allowed = new HashSet<string>
            {
                "item_seed_wheat", "item_seed_cold_legume", "item_seed_hardy_tuber",
                "item_seed_ash_grain", "item_seed_biolum_mushroom", "item_seed_mushroom",
                "item_seed_nutrient_algae", "item_seed_medicinal_herb",
                "item_seed_leafy_green", "item_seed_oilseed", "item_hermetic_sample_ampoule"
            };
            foreach (var def in h.Vault.Catalog.Values)
                Assert.True(allowed.Contains(def.recovery_item_id),
                    def.id + " releases non-canonical item " + def.recovery_item_id);
        }

        // ── 2. Atomic registration ──────────────────────────────────

        [Fact]
        public void RegisterSample_ConsumesSourceOnce_NoDuplication()
        {
            var h = new Harness();
            int before = h.Inventory.CountById("item_hermetic_sample_ampoule");

            string result = h.Vault.RegisterSample("cryo_seed_radiant_wept_wheat");

            Assert.Contains("registered", result);
            Assert.Equal(before - 1, h.Inventory.CountById("item_hermetic_sample_ampoule"));
            Assert.Single(h.Vault.State.canisters);
            Assert.Equal(1000, h.Vault.State.canisters[0].viability_permille);
        }

        [Fact]
        public void RegisterSample_MissingSource_Refused()
        {
            var h = new Harness();
            h.Inventory.TryConsume("item_hermetic_sample_ampoule", 50);
            string result = h.Vault.RegisterSample("cryo_seed_radiant_wept_wheat");
            Assert.Contains("Missing", result);
            Assert.Empty(h.Vault.State.canisters);
        }

        // ── 3. Coolant + insulation ─────────────────────────────────

        [Fact]
        public void Coolant_BurnsDailyWhilePopulated_ReplenishWorks()
        {
            var h = new Harness();
            h.Vault.RegisterSample("cryo_seed_verity_wheat_line");
            float before = h.Vault.CoolantReserve;

            h.Vault.TickDay(1);
            Assert.True(h.Vault.CoolantReserve < before, "populated vault burns coolant");

            float afterBurn = h.Vault.CoolantReserve;
            h.Vault.ReplenishCoolant();
            Assert.True(h.Vault.CoolantReserve > afterBurn);
            Assert.True(h.Inventory.CountById("item_nitrogen_supply") < 50);
        }

        [Fact]
        public void Insulation_SlowsCoolantBurn_AndUpgradeConsumesB66Plate()
        {
            var insulated = new Harness(seed: 9);
            Assert.Contains("upgraded", insulated.Vault.UpgradeInsulation());
            Assert.Equal(1, insulated.Vault.State.insulation_level);
            Assert.True(insulated.Inventory.CountById("item_metallurgy_shielding_plate") < 10);

            var plain = new Harness(seed: 9);
            insulated.Vault.RegisterSample("cryo_seed_verity_wheat_line");
            plain.Vault.RegisterSample("cryo_seed_verity_wheat_line");
            insulated.Vault.TickDay(1);
            plain.Vault.TickDay(1);
            Assert.True(insulated.Vault.CoolantReserve > plain.Vault.CoolantReserve,
                "insulated vault should burn coolant more slowly");
        }

        // ── 4. Thermal decay + power ────────────────────────────────

        [Fact]
        public void PowerLoss_DecaysViability_WithWarningWindow()
        {
            var h = new Harness();
            h.Vault.RegisterSample("cryo_seed_verity_wheat_line");
            h.PowerAvailable = false;

            h.Vault.TickDay(1);
            int v1 = h.Vault.State.canisters[0].viability_permille;
            Assert.True(v1 < 1000, "unstable vault loses viability");

            // Bounded degradation — the sample is not instantly destroyed.
            h.Vault.TickDay(2);
            h.Vault.TickDay(3);
            int v3 = h.Vault.State.canisters[0].viability_permille;
            Assert.True(v3 > 0, "three days of decay must not zero viability");
            Assert.NotEmpty(h.Warnings);
        }

        [Fact]
        public void Radiation_MultipliesDecay_BySensitivity()
        {
            var clean = new Harness(seed: 5);
            var hot = new Harness(seed: 5) { RadiationExposure = 0.8f };
            clean.Vault.RegisterSample("cryo_seed_verity_wheat_line");
            hot.Vault.RegisterSample("cryo_seed_verity_wheat_line");
            clean.PowerAvailable = false;
            hot.PowerAvailable = false;

            clean.Vault.TickDay(1);
            hot.Vault.TickDay(1);

            int cleanV = clean.Vault.State.canisters[0].viability_permille;
            int hotV = hot.Vault.State.canisters[0].viability_permille;
            Assert.True(hotV < cleanV, "irradiated decay should exceed clean decay");
        }

        // ── 5. Recovery pipeline ────────────────────────────────────

        [Fact]
        public void Recovery_HighViability_ReleasesCanonicalItem_ClearsCanister()
        {
            var h = new Harness();
            h.Vault.RegisterSample("cryo_seed_verity_wheat_line");
            string canisterId = h.Vault.State.canisters[0].canister_id;

            Assert.Contains("queued", h.Vault.QueueRecovery(canisterId));
            h.Vault.TickDay(1); // recovery_days = 1

            Assert.Single(h.Released);
            Assert.Equal(3, h.Inventory.CountById("item_seed_wheat")); // recovery_amount 3
            Assert.Empty(h.Vault.State.canisters); // canister cleared exactly once
        }

        [Fact]
        public void Recovery_LowViability_Fails_SampleLost()
        {
            var h = new Harness();
            h.Vault.RegisterSample("cryo_seed_verity_wheat_line");
            string canisterId = h.Vault.State.canisters[0].canister_id;

            // Drain viability below the recovery floor via power loss
            // (7 permille/day unstable decay for this cultivar → ~120 days).
            h.PowerAvailable = false;
            for (int day = 1; day <= 120; day++) h.Vault.TickDay(day);
            Assert.True(h.Vault.State.canisters[0].viability_permille < 200);

            h.PowerAvailable = true;
            h.Vault.QueueRecovery(canisterId);
            h.Vault.TickDay(121);

            Assert.Single(h.Failed);
            Assert.Equal(0, h.Inventory.CountById("item_seed_wheat"));
            Assert.Empty(h.Released);
        }

        // ── 6. Breach + triage ──────────────────────────────────────

        [Fact]
        public void Breach_ProtectedSample_DrainsSlower()
        {
            var h = new Harness();
            h.Vault.RegisterSample("cryo_seed_verity_wheat_line");
            string protectedId = h.Vault.State.canisters[0].canister_id;
            h.Vault.RegisterSample("cryo_seed_verity_wheat_line");
            string exposedId = h.Vault.State.canisters[1].canister_id;

            h.Vault.SetTriageProtection(protectedId, true);
            h.Vault.TriggerBreach("coolant line sheared by seismic event");
            Assert.Single(h.Breaches);
            Assert.True(h.Vault.IsBreachActive);

            h.Vault.TickDay(1);
            int vProtected = h.Vault.State.canisters.Find(c => c.canister_id == protectedId)!.viability_permille;
            int vExposed = h.Vault.State.canisters.Find(c => c.canister_id == exposedId)!.viability_permille;
            Assert.True(vProtected > vExposed, "triage-protected sample should drain slower");

            h.Vault.ResolveBreach();
            Assert.False(h.Vault.IsBreachActive);
        }

        // ── 7. Save round-trip — no rerolls ─────────────────────────

        [Fact]
        public void SaveRoundTrip_ViabilityPersists_NoReroll()
        {
            var h = new Harness();
            h.Vault.RegisterSample("cryo_seed_verity_wheat_line");
            h.PowerAvailable = false;
            h.Vault.TickDay(1);
            h.Vault.TickDay(2);
            int savedViability = h.Vault.State.canisters[0].viability_permille;

            var saved = h.Vault.CaptureState();
            var restored = new Harness(seed: 42, state: saved);

            Assert.Equal(savedViability, restored.Vault.State.canisters[0].viability_permille);
            // Continuing the run decays from the persisted value, not a reroll.
            restored.PowerAvailable = false;
            restored.Vault.TickDay(3);
            int after = restored.Vault.State.canisters[0].viability_permille;
            Assert.True(after < savedViability);
        }

        // ── 8. Legacy defaults ──────────────────────────────────────

        [Fact]
        public void LegacyState_EmptyVault_SafeBaseline()
        {
            var h = new Harness(state: new CryoVaultSaveState());
            Assert.Empty(h.Vault.State.canisters);
            Assert.False(h.Vault.IsBreachActive);
            Assert.Equal(60f, h.Vault.CoolantReserve);
            Assert.Equal(0, h.Vault.State.insulation_level);
        }

        // ── 9. Paired determinism ───────────────────────────────────

        [Fact]
        public void PairedRuns_SameSeed_IdenticalViabilityCurve()
        {
            var a = new Harness(seed: 777);
            var b = new Harness(seed: 777);
            foreach (var h in new[] { a, b })
            {
                h.Vault.RegisterSample("cryo_seed_verity_wheat_line");
                h.Vault.RegisterSample("cryo_seed_radiant_wept_wheat");
                h.PowerAvailable = false;
            }

            for (int day = 1; day <= 5; day++)
            {
                a.Vault.TickDay(day);
                b.Vault.TickDay(day);
            }

            Assert.Equal(
                a.Vault.State.canisters[0].viability_permille,
                b.Vault.State.canisters[0].viability_permille);
            Assert.Equal(
                a.Vault.State.canisters[1].viability_permille,
                b.Vault.State.canisters[1].viability_permille);
        }

        // ── 10. Cultivar release event & traits ───────────────────────

        [Fact]
        public void CompleteRecovery_FiresOnCultivarReleasedWithTraits()
        {
            var h = new Harness();
            h.Vault.RegisterSample("cryo_seed_radiant_wept_wheat");
            string canisterId = h.Vault.State.canisters[0].canister_id;

            string releasedCanister = "";
            CryoCultivarDef? releasedDef = null;
            int releasedViability = 0;
            h.Vault.OnCultivarReleased += (cId, def, viability) =>
            {
                releasedCanister = cId;
                releasedDef = def;
                releasedViability = viability;
            };

            h.Vault.QueueRecovery(canisterId);
            // Def specifies recovery_days = 2
            h.Vault.TickDay(1);
            h.Vault.TickDay(2);

            Assert.Equal(canisterId, releasedCanister);
            Assert.NotNull(releasedDef);
            Assert.Equal("cryo_seed_radiant_wept_wheat", releasedDef!.id);
            Assert.Equal("item_seed_wheat", releasedDef.recovery_item_id);
            Assert.Contains("radiation_tolerant", releasedDef.traits);
            Assert.Contains("heirloom", releasedDef.traits);
            Assert.True(releasedViability >= CryoVaultSystem.MinViabilityForRecovery);
        }
    }
}
