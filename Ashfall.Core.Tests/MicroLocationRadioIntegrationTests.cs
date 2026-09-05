using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Crafting;
using Ashfall.Core.IO;
using Ashfall.Core.Journal;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    using Inventory = Ashfall.Core.Inventory.Inventory;
    /// <summary>
    /// F19 flagship integration — the radio tower's authored rewards are real
    /// radio-progression inputs, not dead loot.
    ///
    ///   micro_radio_tower / open_radio_cabinet → 1 × antenna_coil
    ///     → consumed by an authoritative relic repair (relic_recipes.json,
    ///       "ham_radio" Vintage Ham Radio Set et al.) through the canonical
    ///       WorkshopReverseEngineeringSystem.StartRepair → TryConsumeBill path.
    ///   micro_radio_tower / read_radio_log → journal micro_radio_tower_log
    ///     → canonical JournalSystem.TryDiscoverKnowledge, exactly once.
    ///
    /// No source-specific "use radio tower coil" path exists or is added: the
    /// coil is consumed by the same bill every radio repair uses. Progression
    /// gates stay canonical — the coil alone cannot complete a repair that
    /// requires more components.
    /// </summary>
    public class MicroLocationRadioIntegrationTests
    {
        private const string RadioTowerId = "micro_radio_tower";
        private const string OpenCabinetChoiceId = "open_radio_cabinet";
        private const string ReadLogChoiceId = "read_radio_log";
        private const string IgnoreChoiceId = "ignore_radio";
        private const string CoilItemId = "antenna_coil";
        private const string RadioTowerLogKey = "micro_radio_tower_log";
        private const string HamRadioRelicId = "ham_radio";

        private static string DataDir()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null && !File.Exists(Path.Combine(dir.FullName, "Ashfall.csproj")))
                dir = dir.Parent!;
            return Path.Combine(dir!.FullName, "Assets", "StreamingAssets", "Data");
        }

        private static NarrativeEncounterSystem CreateProductionNarrativeSystem()
        {
            var sys = new NarrativeEncounterSystem();
            var defs = NarrativeEncounterCatalogLoader.Load(
                DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            sys.RegisterRange(defs);
            return sys;
        }

        private static RelicCatalog LoadRelicCatalog()
        {
            var catalog = RelicCatalogLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.False(catalog.relics.Count == 0, "relic_recipes.json must load — it is the coil's downstream consumer");
            return catalog;
        }

        private static (WorkshopReverseEngineeringSystem workshop, Inventory inventory) CreateWorkshop(
            params (string itemId, int count)[] stock)
        {
            var inventory = new Inventory();
            foreach (var (itemId, count) in stock)
                inventory.AddById(itemId, count);
            var research = new ResearchSystem();
            var crafting = new CraftingSystem(inventory);
            var workshop = new WorkshopReverseEngineeringSystem(inventory, research, crafting);
            workshop.LoadCatalog(LoadRelicCatalog());
            return (workshop, inventory);
        }

        private sealed class TestAuthor : ISurvivorAuthor
        {
            public string Id => "surv_expedition";
            public string DisplayName => "Scavenger";
            public RiskBiasTrait RiskBias => RiskBiasTrait.Realist;
        }

        // ── Reward: the coil grant ─────────────────────────────────────

        [Fact]
        public void F19_01_OpenRadioCabinet_GrantsExactlyOneCoil_DepletesSite()
        {
            var sys = CreateProductionNarrativeSystem();
            var res = sys.TryResolve(RadioTowerId, OpenCabinetChoiceId, "loc_radio_hill", 6);
            Assert.NotNull(res);
            Assert.Equal(CoilItemId, res!.GrantItemId);
            Assert.Equal(1, res.GrantItemQuantity);
            Assert.True(res.DepletesOnResolveFlag());
            Assert.True(string.IsNullOrEmpty(res.SetWorldFlagId), "coil grant must not fake a world flag");
            Assert.True(sys.IsDepleted(RadioTowerId));
        }

        [Fact]
        public void F19_02_CoilGrant_IsOneShot_AcrossSaveReload()
        {
            var sys = CreateProductionNarrativeSystem();
            Assert.NotNull(sys.TryResolve(RadioTowerId, OpenCabinetChoiceId, "loc_radio_hill", 6));

            var json = new SystemTextJsonSerializer();
            var restored = CreateProductionNarrativeSystem();
            restored.RestoreState(json.Deserialize<NarrativeEncounterState>(json.Serialize(sys.CaptureState()))!);
            Assert.True(restored.IsDepleted(RadioTowerId));

            // The production selector can never re-surface the site for any seed.
            for (int seed = 0; seed < 64; seed++)
            {
                var picked = restored.SelectEncounter("Cautious", 0f, "loc_radio_hill", new SeededRng(seed));
                Assert.NotEqual(RadioTowerId, picked?.id);
            }
        }

        // ── Functional use: the coil enters real radio progression ─────

        [Fact]
        public void F19_03_Coil_IsAuthoredInputOfRadioRelicRepairs()
        {
            // The coil must appear in at least one authoritative relic bill.
            var catalog = LoadRelicCatalog();
            var consumers = catalog.relics
                .Where(r => r?.required_components != null && r.required_components.Contains(CoilItemId))
                .Select(r => r.relic_id)
                .ToList();
            Assert.Contains(HamRadioRelicId, consumers);
            Assert.True(consumers.Count >= 4, $"expected the authored radio relic family to consume the coil, found: {string.Join(", ", consumers)}");
        }

        [Fact]
        public void F19_04_CoilAlone_CannotCompleteRadioRepair_ProgressionGatesHold()
        {
            // §9.8 — an item grant is not a free upgrade: with only the coil in
            // inventory, the canonical repair must refuse (missing bill lines).
            var (workshop, inventory) = CreateWorkshop((CoilItemId, 1));
            var result = workshop.StartRepair(HamRadioRelicId, "surv_researcher");
            Assert.False(result.IsSuccess);
            Assert.Equal("missing_components", result.FailureCode);
            Assert.Equal(1, inventory.CountById(CoilItemId)); // bill is atomic — nothing consumed
        }

        [Fact]
        public void F19_05_Coil_IsConsumedByCanonicalRadioRepair_BillAtomic()
        {
            // Full authored bill for the Vintage Ham Radio Set.
            var catalog = LoadRelicCatalog();
            var relic = catalog.relics.First(r => r.relic_id == HamRadioRelicId);
            var stock = relic.required_components.Select(c => (c, 1)).ToArray();
            var (workshop, inventory) = CreateWorkshop(stock);

            Assert.True(inventory.CountById(CoilItemId) >= 1);
            var result = workshop.StartRepair(HamRadioRelicId, "surv_researcher");
            Assert.True(result.IsSuccess, $"repair start failed: {result.FailureCode}");
            Assert.Equal(0, inventory.CountById(CoilItemId)); // consumed by the canonical transaction

            // The grant → consumption loop is deterministic: the same bill
            // always costs exactly one coil.
            var coilLines = relic.required_components.Count(c => c == CoilItemId);
            Assert.Equal(1, coilLines);
        }

        [Fact]
        public void F19_06_CoilGrant_PlusRepair_EqualsVanillaEconomy_NoSourceBias()
        {
            // A coil from the micro-location must behave identically to a coil
            // from any other source (workshop fabrication): same bill, same
            // cost — exactly one coil consumed, nothing source-specific.
            var (workshopA, inventoryA) = CreateWorkshop();
            inventoryA.AddById(CoilItemId, 1); // simulated micro-location grant

            var catalog = LoadRelicCatalog();
            var relic = catalog.relics.First(r => r.relic_id == HamRadioRelicId);
            foreach (var comp in relic.required_components)
                inventoryA.AddById(comp, 1); // grant + bill line = 2 coils in stock

            Assert.Equal(2, inventoryA.CountById(CoilItemId));
            var resultA = workshopA.StartRepair(HamRadioRelicId, "surv_researcher");
            Assert.True(resultA.IsSuccess);
            Assert.Equal(1, inventoryA.CountById(CoilItemId)); // exactly the bill's one coil consumed
        }

        // ── Journal integration ────────────────────────────────────────

        [Fact]
        public void F19_07_ReadRadioLog_UnlocksJournalEntry_ExactlyOnce()
        {
            var sys = CreateProductionNarrativeSystem();
            var journal = new JournalSystem();

            var res = sys.TryResolve(RadioTowerId, ReadLogChoiceId, "loc_radio_hill", 6);
            Assert.NotNull(res);
            Assert.Equal(RadioTowerLogKey, res!.JournalUnlockId);

            var first = journal.TryDiscoverKnowledge(res.JournalUnlockId, new TestAuthor(), 6);
            Assert.NotNull(first);
            Assert.Equal(RadioTowerLogKey, first!.KnowledgeKey);

            // Dedup gate: a second unlock attempt returns null and writes nothing.
            var second = journal.TryDiscoverKnowledge(RadioTowerLogKey, new TestAuthor(), 7);
            Assert.Null(second);
            Assert.Equal(1, journal.Entries.Count(e => e.KnowledgeKey == RadioTowerLogKey));
        }

        [Fact]
        public void F19_08_RadioLogChoice_IsNonDepleting_SiteStaysLiveForCoil()
        {
            // Authored multi-stage design: reading the log does not deplete;
            // the coil choice does. Both facts pinned so the ordering contract
            // (log first, salvage later — or reverse) stays intact.
            var sys = CreateProductionNarrativeSystem();
            Assert.NotNull(sys.TryResolve(RadioTowerId, ReadLogChoiceId, "loc_radio_hill", 6));
            Assert.False(sys.IsDepleted(RadioTowerId));

            Assert.NotNull(sys.TryResolve(RadioTowerId, OpenCabinetChoiceId, "loc_radio_hill", 7));
            Assert.True(sys.IsDepleted(RadioTowerId));
        }

        [Fact]
        public void F19_09_JournalUnlockKey_StayedInMicroNamespace_EntryRecorded()
        {
            // §9.6 — no brittle prose assertions: the canonical checks are the
            // key's namespace convention (same rule as the Plan-49 audit) and
            // a real, non-empty composed journal entry under that key.
            Assert.StartsWith("micro_", RadioTowerLogKey, StringComparison.Ordinal);

            var text = JournalVoice.ComposeFullText(RadioTowerLogKey, RiskBiasTrait.Realist, 6);
            Assert.False(string.IsNullOrWhiteSpace(text));

            var journal = new JournalSystem();
            var entry = journal.TryDiscoverKnowledge(RadioTowerLogKey, new TestAuthor(), 6);
            Assert.NotNull(entry);
            Assert.False(string.IsNullOrWhiteSpace(entry!.Text));
        }

        [Fact]
        public void F19_10_IgnoreChoice_NeutralPath()
        {
            var sys = CreateProductionNarrativeSystem();
            var res = sys.TryResolve(RadioTowerId, IgnoreChoiceId, "loc_radio_hill", 6);
            Assert.NotNull(res);
            Assert.True(string.IsNullOrEmpty(res!.GrantItemId));
            Assert.True(string.IsNullOrEmpty(res.JournalUnlockId));
            Assert.False(res.DepletesEncounter);
        }

        // ── Persistence: grant-then-use across save boundaries (§12.3) ─

        [Fact]
        public void F19_11_GrantedCoil_SurvivesSaveReload_AsUsableInventory()
        {
            // Save AFTER the reward, BEFORE using it — the coil must remain a
            // normal consumable component post-restore (§12.3 partial progression).
            var inventory = new Inventory();
            inventory.AddById(CoilItemId, 1); // the micro-location grant, canonical transaction
            var json = new SystemTextJsonSerializer();
            string saved = json.Serialize(inventory.CaptureState());

            var restored = new Inventory();
            restored.RestoreState(json.Deserialize<Ashfall.Core.Inventory.InventorySaveState>(saved)!, id => new Ashfall.Core.Inventory.ItemDefinition { id = id });
            Assert.Equal(1, restored.CountById(CoilItemId));

            // And it still completes a real radio repair after the round trip.
            var catalog = LoadRelicCatalog();
            var relic = catalog.relics.First(r => r.relic_id == HamRadioRelicId);
            foreach (var comp in relic.required_components)
                if (comp != CoilItemId) restored.AddById(comp, 1);

            var research = new ResearchSystem();
            var crafting = new CraftingSystem(restored);
            var workshop = new WorkshopReverseEngineeringSystem(restored, research, crafting);
            workshop.LoadCatalog(catalog);
            var result = workshop.StartRepair(HamRadioRelicId, "surv_researcher");
            Assert.True(result.IsSuccess);
            Assert.Equal(0, restored.CountById(CoilItemId));
        }
    }

    /// <summary>Small assertion helper kept off the payload type to avoid
    /// leaking test-only members into Core.</summary>
    internal static class NarrativeResolutionTestExtensions
    {
        public static bool DepletesOnResolveFlag(this NarrativeEncounterResolutionResult r)
            => r.DepletesEncounter;
    }
}
