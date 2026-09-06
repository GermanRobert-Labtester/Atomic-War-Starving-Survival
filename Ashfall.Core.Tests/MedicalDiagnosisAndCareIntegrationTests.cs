// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Disease;
using Ashfall.Core.Flags;
using Ashfall.Core.Medical;
using Ashfall.Core.Memorial;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class MedicalDiagnosisAndCareIntegrationTests
    {
        private static readonly string DataDir = FindDataDir();

        private static string FindDataDir()
        {
            string? dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            while (dir != null)
            {
                string probe = Path.Combine(dir, "Assets", "StreamingAssets", "Data", "disease_catalog.json");
                if (File.Exists(probe)) return Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                dir = Path.GetDirectoryName(dir.TrimEnd(Path.DirectorySeparatorChar));
            }
            throw new DirectoryNotFoundException("data authority not found");
        }

        [Fact]
        public void DiseaseDiagnosisFlow_HidesIdentityUntilDiagnosedByMedic()
        {
            var catalog = DiseaseCatalogLoader.Load(DataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.False(catalog.HasErrors, $"Catalog loaded with errors: {string.Join(", ", catalog.Errors)}");

            var system = new DiseaseSystem();
            system.BindCatalog(catalog);
            string survivorId = "survivor_patient_01";
            string diseaseId = DiseaseIds.Cholera;

            // Infect survivor
            system.Infect(survivorId, diseaseId, day: 5);

            // Initially undiagnosed
            Assert.False(system.IsDiagnosed(survivorId, diseaseId));

            var pictureUndiagnosed = system.GetClinicalPicture(survivorId, diseaseId);
            Assert.False(pictureUndiagnosed.Diagnosed);
            Assert.Equal(string.Empty, pictureUndiagnosed.DisplayName); // Identity hidden
            Assert.False(string.IsNullOrEmpty(pictureUndiagnosed.Tell)); // Symptoms visible!

            // Medic performs active diagnosis
            bool diagResult = system.Diagnose(survivorId, diseaseId);
            Assert.True(diagResult);
            Assert.True(system.IsDiagnosed(survivorId, diseaseId));

            var pictureDiagnosed = system.GetClinicalPicture(survivorId, diseaseId);
            Assert.True(pictureDiagnosed.Diagnosed);
            Assert.Equal("Cholera", pictureDiagnosed.DisplayName); // Identity revealed!
            Assert.True(pictureDiagnosed.HasTreatmentPath);

            // Save and restore preserves diagnosis state
            var state = system.CaptureState();
            var system2 = new DiseaseSystem();
            system2.BindCatalog(catalog);
            system2.RestoreState(state);

            Assert.True(system2.IsDiagnosed(survivorId, diseaseId));
            Assert.True(system2.GetClinicalPicture(survivorId, diseaseId).Diagnosed);
        }

        [Fact]
        public void ChemicalDependency_DetoxProgressionAndStaffingBonus()
        {
            var system = new ChemicalDependencySystem();
            string survivorId = "survivor_recovering";
            string itemId = "item_morphine";

            // Form dependency (each dose is 0.15f, threshold is 0.3f, 3 doses = 0.45f)
            system.OnSubstanceConsumed(survivorId, itemId, ChemicalDependencyKind.Opioid);
            system.OnSubstanceConsumed(survivorId, itemId, ChemicalDependencyKind.Opioid);
            system.OnSubstanceConsumed(survivorId, itemId, ChemicalDependencyKind.Opioid);
            Assert.True(system.DependencyLevel(survivorId, itemId) >= ChemicalDependencySystem.DependencyThreshold);

            // Start managed detox
            bool started = system.BeginManagedDetox(survivorId, itemId);
            Assert.True(started);

            float moraleDrained = 0;
            system.OnMoraleDrainRequested += (id, amt) => moraleDrained += amt;

            // Advance with medical staff (isStaffed = true):
            // 24 hours should advance detox progress by 24 * 1.25 = 30 hours
            system.TickHours(survivorId, gameHours: 24f, isStaffed: true);

            var ledger = system.Ledger[survivorId];
            var dep = ledger.Find(d => d.itemId == itemId);
            Assert.NotNull(dep);
            Assert.Equal(30f, dep.detoxProgressHours);
            Assert.True(moraleDrained > 0f);

            // Complete detox by ticking up to threshold (96 hours total)
            // Currently at 30h. Remaining = 66h. At 1.25x speed, 53h game time yields 66.25h progress.
            bool completed = false;
            system.OnDetoxCompleted += (id, item) => completed = true;

            system.TickHours(survivorId, gameHours: 60f, isStaffed: true);
            Assert.True(completed);
        }

        [Fact]
        public void VigilCare_ConnectsBedsidePresenceToDeathQuality()
        {
            var vsm = new VigilStateMachine();
            string dwellerId = "dweller_terminal_case";
            var relatives = new List<string> { "Elena", "Mikhail", "Sonya" };

            vsm.StartVigil(dwellerId, relatives, duration: 240f);
            Assert.True(vsm.IsActive);
            Assert.False(vsm.IsCompleted);

            bool phantomKnocked = false;
            vsm.OnPhantomKnock += () => phantomKnocked = true;

            // Tick past 95%
            vsm.Tick(230f);
            Assert.True(phantomKnocked);

            // Complete the vigil
            vsm.Tick(15f);
            Assert.True(vsm.IsCompleted);
            Assert.False(vsm.WasSkipped);

            // Record into consequence flag ledger
            var flags = new InMemoryFlagLedger();
            VigilCare.RecordKept(flags, dwellerId, day: 40);
            Assert.True(VigilCare.IsKept(flags, dwellerId));

            // Resolve death quality:
            // Without vigil or attendance -> Unattended
            Assert.Equal(DeathQuality.Unattended, VigilCare.ResolveQuality(attended: false, wishResolved: false, vigilKept: false));

            // Attended but no wish or vigil -> Rushed
            Assert.Equal(DeathQuality.Rushed, VigilCare.ResolveQuality(attended: true, wishResolved: false, vigilKept: false));

            // Vigil kept -> Peaceful!
            Assert.Equal(DeathQuality.Peaceful, VigilCare.ResolveQuality(attended: false, wishResolved: false, vigilKept: true));
            Assert.Equal(DeathQuality.Peaceful, VigilCare.ResolveQuality(attended: true, wishResolved: false, vigilKept: true));
        }
    }
}
