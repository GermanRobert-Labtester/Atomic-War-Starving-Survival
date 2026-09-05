using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Economy;
using Ashfall.Core.Inventory;
using Ashfall.Core.Phantoms;
using Ashfall.Core.Save;
using Ashfall.Core.Survivors;

namespace Ashfall.Core.Tests.Remediation
{
    public sealed class FollowUpRemediationGateTests
    {
        // ── F11: Inventory migration & shadow-ledger drift ───────────────────

        [Fact]
        public void F11_InventoryMigrator_DoesNotResurrectLowerPhysicalCount()
        {
            var inv = new Inventory.Inventory { Capacity = 10, MaxWeight = 50f };
            var rationDef = new ItemDefinition { id = "item_ration_t1", displayName = "Ration", stackMax = 99, weight = 1f };
            var filterDef = new ItemDefinition { id = "item_filter_charcoal", displayName = "Filter", stackMax = 99, weight = 1f };

            // Player previously held 5 rations, but consumed 3, so physical count is 2
            inv.Add(rationDef, 2);

            var legacyState = new HoldfastTradeSaveState
            {
                schemaVersion = 1, // Legacy save
                held = new Dictionary<string, int>
                {
                    { "item_ration_t1", 5 },      // Lower physically: should NOT be topped up
                    { "item_filter_charcoal", 1 } // Completely missing: should be migrated
                }
            };

            int migrated = InventoryMigrator.MigrateHoldfastHeld(
                legacyState,
                inv,
                id => id == "item_filter_charcoal" ? filterDef : rationDef,
                allowResurrectLowerPhysicalCount: false);

            Assert.Equal(1, migrated); // Only filter migrated
            Assert.Equal(2, inv.CountById("item_ration_t1")); // Rations preserved at 2, not resurrected to 5
            Assert.Equal(1, inv.CountById("item_filter_charcoal"));
            Assert.Equal(2, legacyState.schemaVersion); // Upgraded
            Assert.Empty(legacyState.held); // Cleared to prevent future duplication
        }

        [Fact]
        public void F11_F12_HoldfastTradeSession_SynchronizesBackingInventory()
        {
            var catalog = new HoldfastCatalog();
            catalog.Items.Register(new HoldfastItemDefinition("item_ration_t1", "Ration", "food", 10f, 1f));

            var inv = new Inventory.Inventory { Capacity = 10, MaxWeight = 50f };
            var session = new HoldfastTradeSession(catalog, startingValue: 100, playerInventory: inv);

            // Buy through trade session
            var result = session.Buy("item_ration_t1", 2, "none");
            Assert.True(result.Success);

            // Physical inventory has 2
            Assert.Equal(2, inv.CountById("item_ration_t1"));
            // Trade session queries physical inventory directly
            Assert.Equal(2, session.GetHeld("item_ration_t1"));
            Assert.Equal(2, session.Held["item_ration_t1"]);

            // Consume 1 physically outside trade (e.g. by survival tick)
            inv.Remove("item_ration_t1", 1);
            Assert.Equal(1, session.GetHeld("item_ration_t1"));
            Assert.Equal(1, session.Held["item_ration_t1"]);

            // Capture state carries schemaVersion 2 and matching held count
            var state = session.CaptureState();
            Assert.Equal(2, state.schemaVersion);
            Assert.Equal(1, state.held["item_ration_t1"]);
        }

        // ── F12: Holdfast trade preflight & atomicity ─────────────────────────

        [Fact]
        public void F12_HoldfastTradeSession_BuyPreflightsWeightLimit()
        {
            var catalog = new HoldfastCatalog();
            // Heavy item: 10kg each
            catalog.Items.Register(new HoldfastItemDefinition("item_lead_plate", "Lead Plate", "material", 5f, 10f));

            // Max weight 15kg
            var inv = new Inventory.Inventory { Capacity = 10, MaxWeight = 15f };
            var session = new HoldfastTradeSession(catalog, startingValue: 100, playerInventory: inv);

            // Preview buy 2 (20kg): exceeds 15kg
            var preview = session.PreviewBuy("item_lead_plate", 2, "none");
            Assert.False(preview.IsAvailable);
            Assert.Equal("inventory_capacity", preview.FailureCode);

            // Attempting to buy 2 fails
            var buyResult = session.Buy("item_lead_plate", 2, "none");
            Assert.False(buyResult.Success);
            Assert.Equal(HoldfastTradeFailure.InventoryCapacity, buyResult.Failure);

            // Funds and stock unmutated
            Assert.Equal(100, session.Value);
            Assert.Equal(0, inv.CountById("item_lead_plate"));
        }

        // ── F14, F15, F16: CampaignDayCoordinator ───────────────────────────

        private sealed class TestOwner : IDayAdvanceOwner, IPreDaySnapshotRestore
        {
            public int TickCount { get; private set; }
            public int SnapshotCount { get; private set; }
            public int RestoreCount { get; private set; }
            public bool ThrowOnSnapshot { get; set; }
            public bool ThrowOnTick { get; set; }
            public List<int> TickedDays { get; } = new List<int>();

            public void CapturePreDaySnapshot(int day)
            {
                SnapshotCount++;
                if (ThrowOnSnapshot)
                    throw new InvalidOperationException("Simulated snapshot preflight failure");
            }

            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                TickCount++;
                TickedDays.Add(day);
                if (ThrowOnTick)
                    throw new InvalidOperationException("Simulated tick failure");
            }

            public void RestorePreDaySnapshot(int day)
            {
                RestoreCount++;
            }
        }

        private sealed class ThrowingPersistence : IDayAdvancePersistence
        {
            public bool ShouldThrow { get; set; } = true;
            public void PersistBeforeBriefing(int day, IReadOnlyList<DayOwnerReport> ownerReports)
            {
                if (ShouldThrow)
                    throw new IOException("Simulated persistence write error");
            }
        }

        [Fact]
        public void F14_CampaignDayCoordinator_AdvanceTo_AdvancesSequentially()
        {
            var coord = new CampaignDayCoordinator();
            var owner = new TestOwner();
            coord.Register("test_owner", owner);

            // AdvanceTo day 4 (from day 1)
            var result = coord.AdvanceTo(4);
            Assert.NotNull(result);
            Assert.True(result.Succeeded);
            Assert.Equal(4, coord.Calendar.CurrentDay);
            Assert.Equal(4, coord.LastAdvancedDay);
            Assert.Equal(new[] { 2, 3, 4 }, owner.TickedDays);
        }

        [Fact]
        public void F15_CampaignDayCoordinator_SnapshotPreflightFailure_AbortsWithoutTickingOwners()
        {
            var coord = new CampaignDayCoordinator();
            var badOwner = new TestOwner { ThrowOnSnapshot = true };
            var goodOwner = new TestOwner();

            coord.Register("bad_owner", badOwner, phase: 1);
            coord.Register("good_owner", goodOwner, phase: 2);

            var result = coord.Advance(2);
            Assert.NotNull(result);
            Assert.True(result.HasFailures);

            // Good owner was NEVER ticked because preflight failed in Phase 0!
            Assert.Equal(0, goodOwner.TickCount);
            Assert.Equal(0, badOwner.TickCount);
            Assert.Equal(-1, coord.LastAdvancedDay);
            Assert.Equal(1, coord.Calendar.CurrentDay);
        }

        [Fact]
        public void F16_CampaignDayCoordinator_PersistenceFailure_ArmsPendingRestore()
        {
            var coord = new CampaignDayCoordinator();
            var owner = new TestOwner();
            coord.Register("test_owner", owner);

            var persistence = new ThrowingPersistence();

            // First advance attempt: owners tick, but persistence throws
            var failResult = coord.Advance(2, persistence);
            Assert.NotNull(failResult);
            Assert.True(failResult.HasFailures);
            Assert.Equal(1, owner.TickCount);
            Assert.Equal(-1, coord.LastAdvancedDay);

            // Second advance attempt: persistence now succeeds.
            // Coordinator rolls back snapshot before re-ticking.
            persistence.ShouldThrow = false;
            var successResult = coord.Advance(2, persistence);
            Assert.NotNull(successResult);
            Assert.True(successResult.Succeeded);

            Assert.Equal(1, owner.RestoreCount); // Restored pre-day snapshot before retry
            Assert.Equal(2, coord.LastAdvancedDay);
            Assert.Equal(2, coord.Calendar.CurrentDay);
        }

        // ── F17 & F18: SaveSlotService content validation & virtualized IFileIO ──

        private sealed class MemoryFileIO : IFileIO
        {
            private readonly HashSet<string> _dirs = new HashSet<string>(StringComparer.Ordinal);
            private readonly Dictionary<string, string> _files = new Dictionary<string, string>(StringComparer.Ordinal);

            public bool DirectoryExists(string path) => _dirs.Contains(path);
            public bool FileExists(string path) => _files.ContainsKey(path);
            public string ReadAllText(string path) => _files.TryGetValue(path, out var text) ? text : throw new FileNotFoundException(path);
            public void WriteAllText(string path, string contents)
            {
                string? dir = GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dir)) CreateDirectory(dir);
                _files[path] = contents ?? string.Empty;
            }
            public string Combine(params string[] parts) => string.Join("/", parts).Replace("//", "/");
            public void CreateDirectory(string path)
            {
                if (string.IsNullOrEmpty(path)) return;
                string current = path.TrimEnd('/');
                while (!string.IsNullOrEmpty(current))
                {
                    _dirs.Add(current);
                    int idx = current.LastIndexOf('/');
                    if (idx <= 0) break;
                    current = current.Substring(0, idx);
                }
            }
            public void DeleteFile(string path) => _files.Remove(path);
            public string[] EnumerateFiles(string directory, string searchPattern, SearchOption searchOption)
            {
                var list = new List<string>();
                foreach (var k in _files.Keys)
                {
                    if (k.StartsWith(directory.TrimEnd('/') + "/"))
                        list.Add(k);
                }
                return list.ToArray();
            }
            public string[] GetDirectories(string path, string searchPattern = "*")
            {
                var list = new List<string>();
                string prefix = path.TrimEnd('/') + "/";
                foreach (var d in _dirs)
                {
                    if (d.StartsWith(prefix) && d.Length > prefix.Length)
                    {
                        string sub = d.Substring(prefix.Length);
                        if (!sub.Contains('/'))
                        {
                            if (searchPattern == "*" || (searchPattern.EndsWith("*") && sub.StartsWith(searchPattern.TrimEnd('*'))))
                                list.Add(d);
                        }
                    }
                }
                return list.ToArray();
            }
            public void DeleteDirectory(string path, bool recursive = false)
            {
                _dirs.Remove(path);
                if (recursive)
                {
                    string prefix = path.TrimEnd('/') + "/";
                    _dirs.RemoveWhere(d => d.StartsWith(prefix));
                    var fileKeys = new List<string>(_files.Keys);
                    foreach (var k in fileKeys)
                    {
                        if (k.StartsWith(prefix)) _files.Remove(k);
                    }
                }
            }
            public string GetFileName(string path)
            {
                int idx = path.LastIndexOf('/');
                return idx >= 0 ? path.Substring(idx + 1) : path;
            }
            public string? GetDirectoryName(string path)
            {
                int idx = path.LastIndexOf('/');
                return idx > 0 ? path.Substring(0, idx) : null;
            }
        }

        [Fact]
        public void F17_SaveSlotService_SlotExists_RequiresManifestOrAggregate()
        {
            var mem = new MemoryFileIO();
            var service = new SaveSlotService(mem, new SystemTextJsonSerializer(), new TestLog(), "/virtual_saves");
            var profile = new SaveProfileId("p1");
            var slot = new SaveSlotId("s1");

            // 1. Initially does not exist
            Assert.False(service.SlotExists(profile, slot));

            // 2. Directory alone does NOT make slot exist
            mem.CreateDirectory(service.GetSlotRoot(profile, slot));
            Assert.False(service.SlotExists(profile, slot));

            // 3. Creating slot writes manifest, so SlotExists becomes true
            Assert.True(service.CreateSlot(profile, slot));
            Assert.True(service.SlotExists(profile, slot));
        }

        [Fact]
        public void F18_SaveSlotService_FullyVirtualizedIFileIO()
        {
            var mem = new MemoryFileIO();
            var service = new SaveSlotService(mem, new SystemTextJsonSerializer(), new TestLog(), "/virtual_saves");
            var profile = new SaveProfileId("p1");
            var slot = new SaveSlotId("slot_virtual");

            Assert.True(service.CreateSlot(profile, slot, campaignName: "Virtual Run", mode: CampaignMode.Normal));
            var slots = service.ListSlots(profile);
            Assert.Single(slots);
            Assert.Equal("slot_virtual", slots[0].Value);

            var manifest = service.LoadManifest(profile, slot);
            Assert.NotNull(manifest);
            Assert.Equal("Virtual Run", manifest.campaignName);

            Assert.True(service.DeleteSlot(profile, slot));
            Assert.False(service.SlotExists(profile, slot));
        }

        // ── F19: Duplicate ID rejection on restore ───────────────────────────

        [Fact]
        public void F19_SurvivorCatalog_RestoreState_RejectsDuplicateIds()
        {
            var roster = new SurvivorRosterSystem();
            roster.RegisterDefinition(new SurvivorDefinition { id = "survivor_alpha", displayName = "Alpha" });
            roster.RegisterDefinition(new SurvivorDefinition { id = "survivor_beta", displayName = "Beta" });

            roster.Join("survivor_alpha", day: 1);
            roster.Join("survivor_beta", day: 1);
            Assert.Equal(2, roster.LivingCount);

            // Attempt to restore malformed payload with duplicate IDs
            var malformed = new SurvivorRosterState
            {
                systemId = "survivor_catalog",
                entries = new List<SurvivorRosterEntry>
                {
                    new SurvivorRosterEntry { survivorId = "survivor_charlie", definitionId = "survivor_charlie", isAlive = true },
                    new SurvivorRosterEntry { survivorId = "survivor_charlie", definitionId = "survivor_charlie", isAlive = true }
                }
            };

            roster.RestoreState(malformed);

            // State was NOT cleared or mutated: alpha and beta still remain
            Assert.Equal(2, roster.LivingCount);
            Assert.NotNull(roster.Find("survivor_alpha"));
            Assert.NotNull(roster.Find("survivor_beta"));
            Assert.Null(roster.Find("survivor_charlie"));
        }

        [Fact]
        public void F19_PhantomMemoryEngine_RestoreState_RejectsDuplicateIds()
        {
            var engine = new PhantomMemoryEngine();
            engine.TriggerChanceOverride = 1.0f;
            var sv = new PhantomSurvivorSnapshot { survivorId = "survivor_1", backgroundId = "generic", displayName = "One", isAlive = true };
            engine.RegisterRule("generic", "work_tool", 0.5f, "desc", "motivation", "breakdown");

            // Seed initial state
            engine.OnItemScavenged(sv, "item_tool_whistle", new SeededRng(1));

            var originalState = engine.CaptureState();
            Assert.NotEmpty(originalState.records);

            // Attempt to restore payload with duplicate records
            var malformed = new PhantomMemoryEngineState
            {
                systemId = "phantom_memory_engine",
                records = new List<PhantomMemoryRecord>
                {
                    new PhantomMemoryRecord { survivorId = "dup_survivor", triggersExperienced = 3 },
                    new PhantomMemoryRecord { survivorId = "dup_survivor", triggersExperienced = 5 }
                }
            };

            engine.RestoreState(malformed);

            // Original state remains unmutated
            Assert.Equal(0, engine.GetTriggersExperienced("dup_survivor"));
            Assert.Equal(originalState.records[0].triggersExperienced, engine.GetTriggersExperienced("survivor_1"));
        }

        private sealed class TestLog : ILog
        {
            public void Info(string message) { }
            public void Warn(string message) { }
            public void Error(string message) { }
        }
    }
}
