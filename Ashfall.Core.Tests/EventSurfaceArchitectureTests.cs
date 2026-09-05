using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Ashfall.Core;
using Ashfall.Core.Crafting;
using Ashfall.Core.Events;
using Ashfall.Core.Inventory;
using Ashfall.Core.Journal;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 46 — Event Surface Architecture Tests.
    /// Verifies:
    ///   1. Restore Suppression: RestoreState must NEVER emit mutation events.
    ///   2. Mutation -> Event Ordering: internal state updates before event fires.
    ///   3. Architectural Guard: IEventBus bounded strictly to authorized files.
    /// </summary>
    public class EventSurfaceArchitectureTests
    {
        private sealed class TestAuthor : ISurvivorAuthor
        {
            public string Id => "sv_lead";
            public string DisplayName => "Leader";
            public RiskBiasTrait RiskBias => RiskBiasTrait.Realist;
        }

        [Fact]
        public void RestoreState_SuppressesMutationEvents_AcrossKeyDomains()
        {
            // 1. JournalSystem
            var journal = new JournalSystem();
            journal.TryDiscover("k_init_1", new TestAuthor(), 1);
            journal.TryDiscoverKnowledge("k_init_2", new TestAuthor(), 2);
            journal.UnlockItemSeen("item_water_purifier");
            var journalSave = journal.CaptureState();

            var restoredJournal = new JournalSystem();
            int journalEvents = 0;
            restoredJournal.OnEntryAdded += _ => journalEvents++;
            restoredJournal.OnNotificationPing += _ => journalEvents++;
            restoredJournal.OnTabChanged += _ => journalEvents++;
            restoredJournal.OnCodexUnlocked += _ => journalEvents++;

            restoredJournal.RestoreState(journalSave);
            Assert.Equal(0, journalEvents);

            // 2. ProceduralEulogyEngine
            var eulogyEngine = new ProceduralEulogyEngine();
            eulogyEngine.ComposeEulogy(new DwellerLifeRecord
            {
                dwellerId = "dw_1",
                dwellerName = "Sarah",
                preWarProfession = "Nurse",
                daysSurvived = 10,
                causeOfDeath = "Hypothermia"
            });
            var eulogySave = eulogyEngine.CaptureState();

            var restoredEulogy = new ProceduralEulogyEngine();
            int eulogyEvents = 0;
            restoredEulogy.OnEulogySpoken += (_, _) => eulogyEvents++;

            restoredEulogy.RestoreState(eulogySave);
            Assert.Equal(0, eulogyEvents);

            // 3. ResearchSystem
            var research = new ResearchSystem();
            research.Register(new ResearchKnowledgeDef
            {
                id = "tech_hydroponics",
                displayName = "Hydroponics",
                daysToComplete = 1,
                breakthroughItem = "item_hydro_core"
            });
            research.StartResearch("tech_hydroponics", 1);
            research.Tick(2); // Completes research
            var researchSave = research.CaptureState();

            var restoredResearch = new ResearchSystem();
            int researchEvents = 0;
            restoredResearch.OnResearchCompleted += _ => researchEvents++;

            restoredResearch.RestoreState(researchSave);
            Assert.Equal(0, researchEvents);

            // 4. WorkshopReverseEngineeringSystem
            var inventory = new Ashfall.Core.Inventory.Inventory();
            var crafting = new CraftingSystem(inventory);
            var workshop = new WorkshopReverseEngineeringSystem(inventory, research, crafting);
            var workshopSave = workshop.CaptureState();
            workshopSave.isComplete = true;

            var restoredWorkshop = new WorkshopReverseEngineeringSystem(inventory, research, crafting);
            int workshopActionEvents = 0;
            restoredWorkshop.OnActionCompleted += _ => workshopActionEvents++;

            restoredWorkshop.RestoreState(workshopSave);
            Assert.Equal(0, workshopActionEvents);
        }

        [Fact]
        public void MutationToEventOrdering_StateIsConsistent_WhenHandlerExecutes()
        {
            // When OnEntryAdded fires, state must already reflect the new entry
            var journal = new JournalSystem();
            int inspectedCount = -1;
            string? inspectedKey = null;

            journal.OnEntryAdded += entry =>
            {
                inspectedCount = journal.EntryCount;
                inspectedKey = journal.Entries[0].KnowledgeKey;
            };

            journal.TryDiscover("k_state_ordering", new TestAuthor(), 5);

            Assert.Equal(1, inspectedCount);
            Assert.Equal("k_state_ordering", inspectedKey);

            // When OnCodexUnlocked fires, CodexUnlockCount must already be updated
            int inspectedCodexCount = -1;
            bool isDiscoveredInHandler = false;

            journal.OnCodexUnlocked += key =>
            {
                inspectedCodexCount = journal.CodexUnlockCount;
                isDiscoveredInHandler = journal.Knowledge.Has(key);
            };

            journal.UnlockItemSeen("item_rad_suit");

            Assert.Equal(1, inspectedCodexCount);
            Assert.True(isDiscoveredInHandler);
        }

        [Fact]
        public void ArchitectureGuard_IEventBus_IsStrictlyBoundedToAllowlist()
        {
            var allowedFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "IEventBus.cs",
                "VerdictCensusBroadcast.cs",
                "VerdictRadioSystem.cs",
                "DiveInstanceRunner.cs",
                "Ports.cs"
            };

            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null && !File.Exists(Path.Combine(dir.FullName, "Ashfall.csproj")))
                dir = dir.Parent;

            string coreDir = Path.Combine(dir!.FullName, "Assets", "Ashfall.Core");
            var csFiles = Directory.GetFiles(coreDir, "*.cs", SearchOption.AllDirectories);

            var violations = new List<string>();
            var pattern = new Regex(@"\bIEventBus\b", RegexOptions.Compiled);

            foreach (var file in csFiles)
            {
                string fileName = Path.GetFileName(file);
                if (allowedFiles.Contains(fileName)) continue;

                var lines = File.ReadAllLines(file);
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i].Trim();
                    if (line.StartsWith("//") || line.StartsWith("/*") || line.StartsWith("*") || line.StartsWith("///"))
                        continue;

                    if (pattern.IsMatch(line))
                    {
                        violations.Add($"{fileName}:{i + 1} -> {line}");
                    }
                }
            }

            Assert.True(violations.Count == 0,
                $"Unsanctioned IEventBus usage found outside bounded allowlist in Assets/Ashfall.Core:\n" +
                string.Join("\n", violations));
        }
    }
}
