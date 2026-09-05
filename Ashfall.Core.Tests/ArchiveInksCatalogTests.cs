using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Journal;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class ArchiveInksCatalogTests
    {
        private static string FindDataDir()
        {
            string dataDir;
            if (!CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out dataDir))
                CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dataDir);
            return dataDir ?? string.Empty;
        }

        private static List<InkMaterialDefinition> LoadInks()
        {
            string dataDir = FindDataDir();
            Assert.False(string.IsNullOrEmpty(dataDir), "Could not locate StreamingAssets/Data directory");
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var defs = ArchiveInkCatalogLoader.Load(dataDir, io, json);
            Assert.NotNull(defs);
            return defs;
        }

        [Fact]
        public void Catalog_LoadsExact12Inks()
        {
            var defs = LoadInks();
            Assert.Equal(12, defs.Count);
        }

        [Fact]
        public void Catalog_OriginalThreeInksPreserved()
        {
            var defs = LoadInks();

            var ironGall = defs.Find(d => d.ink_id == "ink_iron_gall");
            Assert.NotNull(ironGall);
            Assert.Equal("Iron Gall Ink", ironGall.display_name);
            Assert.Equal(0.9f, ironGall.legibilityScore, 2);
            Assert.Equal(500f, ironGall.archivalLongevityDays, 1);
            Assert.Equal(0.0008f, ironGall.fadeRatePerDay, 4);
            Assert.Equal("charcoal", ironGall.requiredItemId);
            Assert.Equal(2, ironGall.requiredAmount);

            var sootLamp = defs.Find(d => d.ink_id == "ink_soot_lamp");
            Assert.NotNull(sootLamp);
            Assert.Equal("Soot Lamp Ink", sootLamp.display_name);
            Assert.Equal(0.7f, sootLamp.legibilityScore, 2);
            Assert.Equal(300f, sootLamp.archivalLongevityDays, 1);
            Assert.Equal(0.0015f, sootLamp.fadeRatePerDay, 4);
            Assert.Equal("charcoal", sootLamp.requiredItemId);
            Assert.Equal(1, sootLamp.requiredAmount);

            var plantDye = defs.Find(d => d.ink_id == "ink_plant_dye");
            Assert.NotNull(plantDye);
            Assert.Equal("Plant Dye Ink", plantDye.display_name);
            Assert.Equal(0.6f, plantDye.legibilityScore, 2);
            Assert.Equal(200f, plantDye.archivalLongevityDays, 1);
            Assert.Equal(0.002f, plantDye.fadeRatePerDay, 4);
            Assert.Equal("cloth", plantDye.requiredItemId);
            Assert.Equal(1, plantDye.requiredAmount);
        }

        [Fact]
        public void Catalog_AllTwelveIdsUniqueAndPrefixed()
        {
            var defs = LoadInks();
            var seen = new HashSet<string>(StringComparer.Ordinal);

            foreach (var ink in defs)
            {
                Assert.False(string.IsNullOrWhiteSpace(ink.ink_id));
                Assert.StartsWith("ink_", ink.ink_id);
                Assert.True(seen.Add(ink.ink_id), $"Duplicate ink ID found: {ink.ink_id}");
            }
            Assert.Equal(12, seen.Count);
        }

        [Fact]
        public void Catalog_AllDisplayNamesNonEmptyAndDistinct()
        {
            var defs = LoadInks();
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var ink in defs)
            {
                Assert.False(string.IsNullOrWhiteSpace(ink.display_name));
                Assert.True(seen.Add(ink.display_name), $"Duplicate display name found: {ink.display_name}");
            }
            Assert.Equal(12, seen.Count);
        }

        [Fact]
        public void Catalog_AllIngredientsResolveInItemCatalog()
        {
            string dataDir = FindDataDir();
            var io = new FileSystemIO();
            string itemsRaw = io.ReadAllText(Path.Combine(dataDir, "items.json"));
            using var doc = System.Text.Json.JsonDocument.Parse(itemsRaw);
            var itemIds = new HashSet<string>(StringComparer.Ordinal);
            if (doc.RootElement.TryGetProperty("items", out var itemsArr) && itemsArr.ValueKind == System.Text.Json.JsonValueKind.Array)
            {
                foreach (var item in itemsArr.EnumerateArray())
                {
                    if (item.TryGetProperty("id", out var idProp))
                    {
                        string id = idProp.GetString() ?? "";
                        if (!string.IsNullOrEmpty(id)) itemIds.Add(id);
                    }
                }
            }

            var defs = LoadInks();
            foreach (var ink in defs)
            {
                Assert.False(string.IsNullOrWhiteSpace(ink.requiredItemId), $"Ink {ink.ink_id} has empty requiredItemId");
                Assert.True(itemIds.Contains(ink.requiredItemId),
                    $"Ink {ink.ink_id} references requiredItemId '{ink.requiredItemId}' which does not exist in items.json");
            }
        }

        [Fact]
        public void Catalog_AllAmountsPositiveValidIntegers()
        {
            var defs = LoadInks();
            foreach (var ink in defs)
            {
                Assert.True(ink.requiredAmount >= 1 && ink.requiredAmount <= 5,
                    $"Ink {ink.ink_id} has out-of-range requiredAmount: {ink.requiredAmount}");
            }
        }

        [Fact]
        public void Catalog_AllNumericRangesValid()
        {
            var defs = LoadInks();
            foreach (var ink in defs)
            {
                Assert.InRange(ink.legibilityScore, 0.3f, 1.0f);
                Assert.InRange(ink.archivalLongevityDays, 50f, 1000f);
                Assert.InRange(ink.fadeRatePerDay, 0.0005f, 0.02f);
            }
        }

        [Fact]
        public void Catalog_NoTwoInksHaveIdenticalProfiles()
        {
            var defs = LoadInks();
            var seen = new HashSet<string>(StringComparer.Ordinal);

            foreach (var ink in defs)
            {
                string key = $"{ink.legibilityScore:F2}_{ink.archivalLongevityDays:F0}_{ink.fadeRatePerDay:F4}_{ink.requiredItemId}_{ink.requiredAmount}";
                Assert.True(seen.Add(key), $"Duplicate ink profile found: {ink.ink_id} shares {key}");
            }
        }

        [Fact]
        public void Catalog_NoUniversalDominance()
        {
            var defs = LoadInks();
            for (int i = 0; i < defs.Count; i++)
            {
                for (int j = 0; j < defs.Count; j++)
                {
                    if (i == j) continue;
                    var a = defs[i];
                    var b = defs[j];

                    // Check if A strictly dominates B across all 5 dimensions simultaneously
                    bool strictlyBetterLegibility = a.legibilityScore > b.legibilityScore;
                    bool strictlyBetterLongevity = a.archivalLongevityDays > b.archivalLongevityDays;
                    bool strictlyBetterFade = a.fadeRatePerDay < b.fadeRatePerDay;
                    bool strictlyCheaperAmount = a.requiredAmount < b.requiredAmount;
                    bool sameOrFreeIngredient = a.requiredItemId == b.requiredItemId;

                    if (sameOrFreeIngredient && strictlyBetterLegibility && strictlyBetterLongevity && strictlyBetterFade && strictlyCheaperAmount)
                    {
                        Assert.Fail($"Ink {a.ink_id} strictly dominates {b.ink_id} across all quality and cost dimensions!");
                    }
                }
            }
        }

        [Fact]
        public void Runtime_ArchiveDeskQueuesAndConsumesCorrectIngredientAmount()
        {
            string dataDir = FindDataDir();
            var journal = new JournalSystem();
            var knowledge = new KnowledgeBase();
            var inv = new Inventory.Inventory();
            var roster = new DutyRosterSystem();
            var desk = new ArchiveDeskSystem(journal, knowledge, inv, roster);

            ArchiveInkCatalogLoader.LoadAndRegister(desk, dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            // Provide 10 charcoal
            inv.AddById("charcoal", 10);
            Assert.Equal(10, inv.CountById("charcoal"));

            // Queue using archival carbon (requires 3 charcoal)
            var res = desk.QueueTranscription("doc_evidence_archival", "archivist_test", "ink_archival_carbon");
            Assert.True(res.IsSuccess, res.MessageKey);
            Assert.Equal(7, inv.CountById("charcoal")); // 10 - 3 = 7

            // Cancel and verify 3 charcoal refunded
            var job = desk.GetActiveJobs()[0];
            var cancelRes = desk.CancelJob(job.jobId);
            Assert.True(cancelRes.IsSuccess, cancelRes.MessageKey);
            Assert.Equal(10, inv.CountById("charcoal"));
        }

        [Fact]
        public void Runtime_ArchiveDeskTranscribesWithConfiguredLegibility()
        {
            string dataDir = FindDataDir();
            var journal = new JournalSystem();
            var knowledge = new KnowledgeBase();
            var inv = new Inventory.Inventory();
            var roster = new DutyRosterSystem();
            var desk = new ArchiveDeskSystem(journal, knowledge, inv, roster);

            ArchiveInkCatalogLoader.LoadAndRegister(desk, dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            inv.AddById("blood_sample", 2);
            var res = desk.QueueTranscription("doc_emergency_evidence", "archivist_med", "ink_blood_emergency");
            Assert.True(res.IsSuccess);

            var job = desk.GetActiveJobs()[0];
            Assert.Equal(0.4f, job.legibilityScore, 2);

            desk.TickDay(1);
            Assert.True(job.isComplete);
            Assert.True(desk.IsEvidenceUnlocked("doc_emergency_evidence"));
        }

        [Fact]
        public void Runtime_SaveLoadPreservesTranscriptionQueueAndState()
        {
            string dataDir = FindDataDir();
            var journal = new JournalSystem();
            var knowledge = new KnowledgeBase();
            var inv = new Inventory.Inventory();
            var roster = new DutyRosterSystem();
            var desk = new ArchiveDeskSystem(journal, knowledge, inv, roster);

            ArchiveInkCatalogLoader.LoadAndRegister(desk, dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            inv.AddById("chemical_solvent", 2);
            desk.QueueTranscription("doc_chemical_plans", "archivist_chem", "ink_chemical_marker");

            var state = desk.CaptureState();
            Assert.Single(state.queue);
            Assert.Equal("ink_chemical_marker", state.queue[0].inkId);
            Assert.Equal(0.8f, state.queue[0].legibilityScore, 2);

            var deskRestored = new ArchiveDeskSystem(journal, knowledge, inv, roster);
            deskRestored.RestoreState(state);
            Assert.Single(deskRestored.State.queue);
            Assert.Equal("ink_chemical_marker", deskRestored.State.queue[0].inkId);
            Assert.Equal("doc_chemical_plans", deskRestored.State.queue[0].evidenceId);
        }
    }
}
