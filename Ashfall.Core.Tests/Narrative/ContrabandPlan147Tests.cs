using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Narrative;
using Xunit;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Tests.Narrative
{
    /// <summary>
    /// PLAN 147 — Bunker Contraband Barter runtime integration tests.
    /// Pins the Task A catalog-validation contract and the §13 minimal
    /// vertical slice: three representative entries (low/mid/high tier)
    /// with canonical identity, deterministic once-only acquisition,
    /// save/load stability, no-side-effect reads and fail-closed behavior.
    /// </summary>
    public sealed class ContrabandPlan147Tests : CatalogTestBase
    {
        private static string CatalogPath =>
            Path.Combine(DataDirectory, "narrative", "bunker_contraband_barter.json");

        private static BunkerContrabandCatalog LoadCatalog()
            => BunkerContrabandCatalog.LoadFromFile(CatalogPath);

        private static ItemCatalog LoadItemCatalog()
            => ItemCatalogLoader.LoadCatalog(
                DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());

        private static ContrabandStashSystem CreateSystem(
            BunkerContrabandCatalog? catalog = null,
            InventoryContainer? inventory = null,
            ItemCatalog? itemCatalog = null,
            bool withDefaultActivations = true)
        {
            catalog ??= LoadCatalog();
            inventory ??= new InventoryContainer();
            itemCatalog ??= LoadItemCatalog();

            Func<string, ItemDefinition?> lookup = id => itemCatalog.Get(id);
            var system = new ContrabandStashSystem(catalog, inventory, lookup);
            if (withDefaultActivations)
            {
                foreach (var activation in ContrabandStashSystem.DefaultActivations())
                    Assert.True(system.TryRegisterActivation(activation),
                        $"default activation should register: {activation.entryId}");
            }
            return system;
        }

        // ── Task A: catalog validation ───────────────────────────────────

        [Fact]
        public void Validator_AuthoredCatalog_IsValidWith20Entries()
        {
            var report = ContrabandCatalogValidator.ValidateJson(File.ReadAllText(CatalogPath));

            Assert.True(report.IsValid,
                "authored catalog must pass validation; errors: " + string.Join("; ", report.Errors));
            Assert.Equal(20, report.EntryCount);
        }

        [Fact]
        public void Validator_RejectsDuplicateIds()
        {
            string json = "{\"items\":[" +
                ValidEntryJson("contraband_dup_test") + "," +
                ValidEntryJson("contraband_dup_test") + "]}";
            var report = ContrabandCatalogValidator.ValidateJson(json);

            Assert.False(report.IsValid);
            Assert.Contains(report.Errors, e => e.Contains("duplicate contraband id"));
        }

        [Fact]
        public void Validator_RejectsNegativeAndNonIntegerPrices()
        {
            string json = "{\"items\":[" +
                ValidEntryJson("contraband_price_neg", priceOverride: "-5") + "," +
                ValidEntryJson("contraband_price_text", priceOverride: "\"free\"") + "]}";
            var report = ContrabandCatalogValidator.ValidateJson(json);

            Assert.False(report.IsValid);
            Assert.Contains(report.Errors, e => e.Contains("market_price_scrip"));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(4)]
        public void Validator_RejectsInvalidTiers(int tier)
        {
            string json = "{\"items\":[" +
                ValidEntryJson("contraband_tier_bad", tierOverride: tier.ToString()) + "]}";
            var report = ContrabandCatalogValidator.ValidateJson(json);

            Assert.False(report.IsValid);
            Assert.Contains(report.Errors, e => e.Contains("contraband_tier"));
        }

        [Fact]
        public void Validator_RejectsNonFiniteNumbersAtParseLevel()
        {
            // NaN / Infinity literals are invalid JSON; the validator must fail closed
            // with a parse error rather than treating the catalog as empty.
            string json = "{\"items\":[" +
                ValidEntryJson("contraband_nan_test",
                    extraMechanics: ",\"tribunal_suspicion_rate\": NaN") + "]}";
            var report = ContrabandCatalogValidator.ValidateJson(json);

            Assert.False(report.IsValid);
            Assert.Contains(report.Errors, e => e.Contains("catalog_json_unparseable"));
        }

        [Fact]
        public void Validator_RejectsUnknownMechanicsKey()
        {
            string json = "{\"items\":[" +
                ValidEntryJson("contraband_unknown_mech",
                    extraMechanics: ",\"player_hp_regeneration_per_second\": 5") + "]}";
            var report = ContrabandCatalogValidator.ValidateJson(json);

            Assert.False(report.IsValid);
            Assert.Contains(report.Errors,
                e => e.Contains("player_hp_regeneration_per_second") && e.Contains("unknown mechanics key"));
        }

        [Theory]
        [InlineData("tribunal_suspicion_rate", "1.5", "probability-class")]
        [InlineData("chemical_dependency_risk", "-0.1", "probability-class")]
        [InlineData("agricultural_yield_multiplier", "0", "multiplier-class")]
        [InlineData("calorie_surplus_kcal", "-100", "non-negative")]
        public void Validator_RejectsOutOfRangeMechanicsValues(string key, string value, string expectedFragment)
        {
            string json = "{\"items\":[" +
                ValidEntryJson("contraband_range_test",
                    extraMechanics: $",\"{key}\": {value}") + "]}";
            var report = ContrabandCatalogValidator.ValidateJson(json);

            Assert.False(report.IsValid);
            Assert.Contains(report.Errors, e => e.Contains(key) && e.Contains(expectedFragment));
        }

        [Fact]
        public void Validator_RejectsBadIdsAndMissingFields()
        {
            string json = "{\"items\":[" +
                ValidEntryJson("bad_prefix_no_contraband") + "," +
                "{\"id\": \"contraband_missing_fields\", \"title\": \"x\"}" + "]}";
            var report = ContrabandCatalogValidator.ValidateJson(json);

            Assert.False(report.IsValid);
            Assert.Contains(report.Errors, e => e.Contains("contraband_ prefix"));
            Assert.Contains(report.Errors, e => e.Contains("missing"));
        }

        // ── Task B slice: identity, discovery, acquisition ────────────────

        [Fact]
        public void Activation_CanonicalItems_ExistInAuthoritativeItemCatalog()
        {
            var itemCatalog = LoadItemCatalog();

            foreach (var activation in ContrabandStashSystem.DefaultActivations())
            {
                var def = itemCatalog.Get(activation.canonicalItemId);
                Assert.NotNull(def);
                Assert.Equal(activation.canonicalItemId, def.id);
            }
        }

        [Fact]
        public void Activation_CoversOneLowOneMidOneHighTierEntry()
        {
            var catalog = LoadCatalog();
            var activations = ContrabandStashSystem.DefaultActivations();

            Assert.Equal(4, activations.Count);
            var tiers = new HashSet<int>();
            foreach (var activation in activations)
            {
                var entry = catalog.GetById(activation.entryId);
                Assert.NotNull(entry);
                tiers.Add(entry!.ContrabandTier);
            }
            Assert.Contains(1, tiers);
            Assert.Contains(2, tiers);
            Assert.Contains(3, tiers);
        }

        [Fact]
        public void Registration_FailsClosed_ForUnknownEntryOrBadPayload()
        {
            var system = CreateSystem(withDefaultActivations: false);

            Assert.False(system.TryRegisterActivation(new ContrabandStashActivation
            { entryId = "contraband_does_not_exist", canonicalItemId = "sugar" }));
            Assert.False(system.TryRegisterActivation(new ContrabandStashActivation
            { entryId = "contraband_card_deck_pinned_kings", canonicalItemId = "" }));
            Assert.False(system.TryRegisterActivation(new ContrabandStashActivation
            { entryId = "contraband_card_deck_pinned_kings", canonicalItemId = "sugar", grantQuantity = 0 }));
            Assert.False(system.TryRegisterActivation(new ContrabandStashActivation
            { entryId = "contraband_card_deck_pinned_kings", canonicalItemId = "sugar", minDay = -1 }));

            Assert.True(system.TryRegisterActivation(new ContrabandStashActivation
            { entryId = "contraband_card_deck_pinned_kings", canonicalItemId = "item_playing_cards" }));
            Assert.False(system.TryRegisterActivation(new ContrabandStashActivation
            { entryId = "contraband_card_deck_pinned_kings", canonicalItemId = "item_playing_cards" }),
                "duplicate registration must be rejected");
        }

        [Fact]
        public void Stash_DayGate_BlocksEarlyClaim_AndOpensAtGate()
        {
            var system = CreateSystem();

            Assert.False(system.IsDiscoverable("contraband_card_deck_pinned_kings", 2));
            var early = system.TryClaimStash("contraband_card_deck_pinned_kings", 2);
            Assert.Equal(ActionResult.StatusKind.Blocked, early.Status);

            Assert.True(system.IsDiscoverable("contraband_card_deck_pinned_kings", 3));
            Assert.True(system.TryClaimStash("contraband_card_deck_pinned_kings", 3).IsSuccess);
        }

        [Fact]
        public void Stash_Claim_GrantsExactCanonicalItems()
        {
            var inventory = new InventoryContainer();
            var system = CreateSystem(inventory: inventory);

            Assert.True(system.TryClaimStash("contraband_card_deck_pinned_kings", 5).IsSuccess);
            Assert.Equal(1, inventory.CountById("item_playing_cards"));

            Assert.True(system.TryClaimStash("contraband_unrationed_sugar_brick", 10).IsSuccess);
            Assert.Equal(8, inventory.CountById("sugar"));

            Assert.True(system.TryClaimStash("contraband_century_seed_grain_vial", 25).IsSuccess);
            Assert.Equal(1, inventory.CountById("item_seed_wheat"));

            Assert.True(system.TryClaimStash("contraband_bootleg_morphine_ampoules", 30).IsSuccess);
            Assert.Equal(4, inventory.CountById("morphine"));

            // Nothing else leaked into inventory from the four claims.
            Assert.Equal(4, inventory.Slots.Count(s => s.Amount > 0));
        }

        [Fact]
        public void Stash_IsOnceOnly_NoReclaim()
        {
            var inventory = new InventoryContainer();
            var system = CreateSystem(inventory: inventory);

            Assert.True(system.TryClaimStash("contraband_unrationed_sugar_brick", 10).IsSuccess);
            Assert.Equal(8, inventory.CountById("sugar"));

            var second = system.TryClaimStash("contraband_unrationed_sugar_brick", 11);
            Assert.Equal(ActionResult.StatusKind.Blocked, second.Status);
            Assert.Equal("contraband_already_claimed", second.FailureCode);
            Assert.Equal(8, inventory.CountById("sugar"));
        }

        [Fact]
        public void Stash_UnknownOrUnactivatedEntry_FailsClosed()
        {
            var system = CreateSystem();

            // A still-deferred record (candle hoard has no canonical item) must
            // stay undiscoverable and unclaimable — never silently executable.
            var unknown = system.TryClaimStash("contraband_paraffin_candle_hoard", 100);
            Assert.False(unknown.IsSuccess);
            Assert.False(system.IsDiscoverable("contraband_paraffin_candle_hoard", 100),
                "records without a reviewed activation stay deferred, never silently executable");
        }

        [Fact]
        public void Stash_CapacityBlocked_ClaimIsNotRecorded_AndRetrySucceeds()
        {
            var inventory = new InventoryContainer { Capacity = 1, MaxWeight = 1f };
            // Occupy the single slot with something heavy.
            inventory.TryProduce("item_blowtorch", 1);

            var system = CreateSystem(inventory: inventory);
            Assert.False(system.IsClaimed("contraband_card_deck_pinned_kings"));

            var blocked = system.TryClaimStash("contraband_card_deck_pinned_kings", 5);
            // A full inventory must block the claim without destroying the stash.
            Assert.Equal(ActionResult.StatusKind.Blocked, blocked.Status);
            Assert.False(system.IsClaimed("contraband_card_deck_pinned_kings"));

            inventory.Remove("item_blowtorch", 1);
            Assert.True(system.TryClaimStash("contraband_card_deck_pinned_kings", 5).IsSuccess);
            Assert.Equal(1, inventory.CountById("item_playing_cards"));
        }

        [Fact]
        public void Stash_SaveRoundTrip_PreservesOnceOnlyClaims()
        {
            var inventory = new InventoryContainer();
            var system = CreateSystem(inventory: inventory);
            Assert.True(system.TryClaimStash("contraband_unrationed_sugar_brick", 10).IsSuccess);

            var captured = system.CaptureState();

            // Simulate save/load: a fresh system over a fresh inventory restores the claim book.
            var reloaded = CreateSystem(inventory: new InventoryContainer());
            reloaded.RestoreState(captured);

            Assert.True(reloaded.IsClaimed("contraband_unrationed_sugar_brick"));
            var replay = reloaded.TryClaimStash("contraband_unrationed_sugar_brick", 12);
            // Save/reload must not reopen a claimed stash.
            Assert.Equal(ActionResult.StatusKind.Blocked, replay.Status);
            Assert.False(reloaded.IsDiscoverable("contraband_unrationed_sugar_brick", 12));
            // Only the three unclaimed activated stashes remain discoverable.
            Assert.Equal(3, reloaded.ListDiscoverable(100).Count);
        }

        [Fact]
        public void Stash_OldSave_WithNoContrabandState_ChangesNothing()
        {
            var inventory = new InventoryContainer();
            var system = CreateSystem(inventory: inventory);
            int slotsBefore = inventory.Slots.Count(s => s.Amount > 0);

            // Legacy campaign: no contraband section at all.
            system.RestoreState(null);

            Assert.Equal(0, system.State.claimedDayByEntry.Count);
            // Loading old saves can neither grant nor remove wealth.
            Assert.Equal(slotsBefore, inventory.Slots.Count(s => s.Amount > 0));
            Assert.Equal(4, system.ListDiscoverable(100).Count);
        }

        [Fact]
        public void Stash_RestoreState_IsDeepClone_NotSharedReferences()
        {
            var inventory = new InventoryContainer();
            var system = CreateSystem(inventory: inventory);
            Assert.True(system.TryClaimStash("contraband_card_deck_pinned_kings", 5).IsSuccess);

            var captured = system.CaptureState();
            captured.claimedDayByEntry.Clear(); // mutate the snapshot

            Assert.True(system.IsClaimed("contraband_card_deck_pinned_kings"),
                "captured state must be a clone, not a live reference");
        }

        [Fact]
        public void Stash_LoadAndQuery_HasNoSimulationSideEffects()
        {
            var inventory = new InventoryContainer();
            var system = CreateSystem(inventory: inventory);

            int slotsBefore = inventory.Slots.Count(s => s.Amount > 0);
            int claimedBefore = system.State.claimedDayByEntry.Count;

            var catalog = LoadCatalog();
            _ = catalog.GetByTier(1);
            _ = catalog.GetByCategory("luxury_rations");
            _ = catalog.GetByTag("moonshine");
            _ = catalog.GetById("contraband_siphon_hose_and_bulb");
            _ = system.ListDiscoverable(50);
            _ = system.IsDiscoverable("contraband_century_seed_grain_vial", 50);
            _ = system.GetActivation("contraband_card_deck_pinned_kings");

            Assert.Equal(slotsBefore, inventory.Slots.Count(s => s.Amount > 0));
            Assert.Equal(claimedBefore, system.State.claimedDayByEntry.Count);
        }

        [Fact]
        public void Stash_Deterministic_IdenticalSystemsProduceIdenticalAvailabilitySequences()
        {
            var a = CreateSystem();
            var b = CreateSystem();

            for (int day = 0; day <= 40; day++)
            {
                var listA = string.Join(",", a.ListDiscoverable(day).Select(e => e.Id));
                var listB = string.Join(",", b.ListDiscoverable(day).Select(e => e.Id));
                Assert.Equal(listB, listA);
            }
        }

        // ── Authorized-effect ownership proofs ────────────────────────────

        [Fact]
        public void AuthorizedEffect_Morale_IsOwnedByCanonicalSugarItem_NotByContrabandCode()
        {
            var itemCatalog = LoadItemCatalog();

            // The sugar brick's morale_delta (12 in JSON) is NOT executed by the
            // contraband layer. The authorized effect is the canonical sugar item's
            // own moraleEffect, applied by the existing item-use pipeline.
            var sugar = itemCatalog.Get("sugar");
            Assert.NotNull(sugar);
            Assert.True(sugar!.moraleEffect > 0f,
                "canonical sugar must carry the morale effect through the item-use pipeline");
        }

        [Fact]
        public void AuthorizedEffect_Agriculture_IsOwnedByGreenhouseCropCatalog()
        {
            // The heirloom wheat vial's agricultural_yield_multiplier (2.0 in JSON)
            // is NOT executed by the contraband layer. The authorized route is the
            // canonical item_seed_wheat crop definition owned by the greenhouse
            // agriculture authority.
            var crop = System.Array.Find(
                GreenhouseExpansionCatalog.CropCatalog.All,
                c => c.SeedItemId == "item_seed_wheat");
            Assert.NotNull(crop);
            Assert.Equal("crop_wheat", crop!.YieldCleanId);
        }

        [Fact]
        public void AuthorizedEffect_TradeValue_IsOwnedByCanonicalItemDefinition()
        {
            var itemCatalog = LoadItemCatalog();

            // market_price_scrip (scrip) is descriptive at this layer; the tradable
            // value authority is the canonical item tradeValue, consumed by the
            // existing barter/economy systems.
            var cards = itemCatalog.Get("item_playing_cards");
            Assert.NotNull(cards);
            Assert.True(cards!.tradeValue > 0f);

            var wheatSeeds = itemCatalog.Get("item_seed_wheat");
            Assert.NotNull(wheatSeeds);
            Assert.True(wheatSeeds!.tradeValue > 0f);
        }

        // ── Narcotics slice (Plan 147 follow-up): canonical morphine ─────

        [Fact]
        public void AuthorizedEffect_Morphine_PainRelief_IsOwnedByCanonicalItemDefinition()
        {
            var itemCatalog = LoadItemCatalog();

            // The contraband row's instant_pain_relief_hp=40 is NOT executed.
            // The canonical morphine item's own healthEffect, applied by the
            // existing item-use pipeline, is the sole pain-relief authority.
            var morphine = itemCatalog.Get("morphine");
            Assert.NotNull(morphine);
            Assert.Equal(ItemType.Medical, morphine!.type);
            Assert.True(morphine.healthEffect > 0f,
                "canonical morphine must carry pain relief through the item-use pipeline");
            Assert.True(morphine.moraleEffect > 0f);
            Assert.True(morphine.tradeValue > 0f);
        }

        [Fact]
        public void Morphine_DependencyCatalog_LinksCanonicalItemId()
        {
            // The dependency authority keys on the SAME canonical id: one row in
            // chemical_dependency_items.json (opioid) — no second identity.
            string path = Path.Combine(DataDirectory, "chemical_dependency_items.json");
            Assert.True(File.Exists(path));

            using var doc = System.Text.Json.JsonDocument.Parse(File.ReadAllText(path));
            var items = doc.RootElement.GetProperty("items");
            int matches = 0;
            string? kind = null;
            foreach (var it in items.EnumerateArray())
            {
                if (it.TryGetProperty("item_id", out var idEl) &&
                    string.Equals(idEl.GetString(), "morphine", StringComparison.Ordinal))
                {
                    matches++;
                    kind = it.GetProperty("dependency_kind").GetString();
                }
            }
            Assert.Equal(1, matches);
            Assert.Equal("opioid", kind);
        }

        [Fact]
        public void ChemicalDependency_Morphine_ExactlyOneDosePerAuthorizedConsumptionEvent()
        {
            var system = new Ashfall.Core.Medical.ChemicalDependencySystem();
            const Ashfall.Core.Medical.ChemicalDependencyKind opioid = Ashfall.Core.Medical.ChemicalDependencyKind.Opioid;

            // One authorized consumption event → exactly one dose.
            system.OnSubstanceConsumed("survivor_test", "morphine", opioid);
            var dep = system.Ledger["survivor_test"].Single(d => d.itemId == "morphine");
            Assert.Equal(Ashfall.Core.Medical.ChemicalDependencySystem.DependencyIncreasePerDose, dep.dependencyLevel, 3);

            // Per-event (not once-ever) semantics: the next committed consumption
            // adds exactly one more dose — the host fires OnSubstanceConsumed from
            // the inventory OnConsumed hook, which commits exactly once per use.
            system.OnSubstanceConsumed("survivor_test", "morphine", opioid);
            Assert.Equal(2f * Ashfall.Core.Medical.ChemicalDependencySystem.DependencyIncreasePerDose, dep.dependencyLevel, 3);
        }

        // ── Helpers ──────────────────────────────────────────────────────

        private static string ValidEntryJson(
            string id,
            string priceOverride = "25",
            string tierOverride = "1",
            string extraMechanics = "")
        {
            return "{" +
                "\"id\": \"" + id + "\"," +
                "\"title\": \"Test Entry\"," +
                "\"category\": \"test_category\"," +
                "\"contraband_tier\": " + tierOverride + "," +
                "\"risk_profile\": \"test risk\"," +
                "\"market_price_scrip\": " + priceOverride + "," +
                "\"hidden_stash_location\": \"test_stash\"," +
                "\"mechanics\": {\"morale_delta\": 1, \"tribunal_suspicion_rate\": 0.1" + extraMechanics + "}," +
                "\"tags\": [\"test\"]," +
                "\"prose\": \"Test prose.\"" +
                "}";
        }
    }
}
