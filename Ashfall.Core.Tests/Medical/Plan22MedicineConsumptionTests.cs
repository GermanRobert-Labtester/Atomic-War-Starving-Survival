// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Medical;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    public sealed class Plan22MedicineConsumptionTests
    {
        [Fact]
        public void DoseLedger_RecordAntiRadTreatment_SetsLastAntiRadDay()
        {
            var doseLedger = new DoseLedgerSystem();
            doseLedger.AssignDosimeter("survivor_mikhail", "tag_01");

            var entryBefore = doseLedger.GetEntry("survivor_mikhail");
            Assert.NotNull(entryBefore);
            Assert.Equal(-1, entryBefore.lastAntiRadDay);

            doseLedger.RecordAntiRadTreatment("survivor_mikhail", 7);

            var entryAfter = doseLedger.GetEntry("survivor_mikhail");
            Assert.NotNull(entryAfter);
            Assert.Equal(7, entryAfter.lastAntiRadDay);
        }

        [Fact]
        public void ChemicalDependency_OnSubstanceConsumed_AdvancesDependencyAndTriggersEvent()
        {
            var depSys = new ChemicalDependencySystem();
            string? formedSurvivor = null;
            string? formedItem = null;

            depSys.OnDependencyFormed += (sId, iId) =>
            {
                formedSurvivor = sId;
                formedItem = iId;
            };

            // Dose 1: +0.15 (0.15 total) -> below threshold (0.3)
            depSys.OnSubstanceConsumed("survivor_vasquez", "morphine", ChemicalDependencyKind.Opioid);
            Assert.Null(formedSurvivor);
            Assert.Equal(0.15f, depSys.DependencyLevel("survivor_vasquez", "morphine"), 2);

            // Dose 2: +0.15 (0.30 total) -> hits threshold (0.3) -> OnDependencyFormed fires!
            depSys.OnSubstanceConsumed("survivor_vasquez", "morphine", ChemicalDependencyKind.Opioid);
            Assert.Equal("survivor_vasquez", formedSurvivor);
            Assert.Equal("morphine", formedItem);
            Assert.Equal(0.30f, depSys.DependencyLevel("survivor_vasquez", "morphine"), 2);
        }

        [Fact]
        public void MedicalRecordLog_AppendsClinicalFactsAndEvictsOldestWhenFull()
        {
            var log = new MedicalRecordLog();

            log.Append(3, "treatment_completed", "survivor_sasha", "anti_rad_pills");
            Assert.Equal(1, log.Count);
            Assert.Equal(3, log.Entries[0].day);
            Assert.Equal("treatment_completed", log.Entries[0].kind);
            Assert.Equal("survivor_sasha", log.Entries[0].survivorId);
            Assert.Equal("anti_rad_pills", log.Entries[0].detail);

            // Fill beyond capacity (MaxEntries = 64)
            for (int i = 0; i < 70; i++)
            {
                log.Append(10 + i, "treatment_completed", "survivor_test", $"med_{i}");
            }

            Assert.Equal(MedicalRecordLog.MaxEntries, log.Count);
            // Oldest entry (day 3) should be evicted
            Assert.NotEqual(3, log.Entries[0].day);
            // Most recent entries should be present
            Assert.Equal("med_69", log.Entries[log.Count - 1].detail);
        }

        [Fact]
        public void DoseLedger_BookReading_WithAntiRad_UpdatesLastAntiRadDayAndAttenuates()
        {
            var doseLedger = new DoseLedgerSystem();
            doseLedger.AssignDosimeter("survivor_lead", "tag_02");

            var res = doseLedger.BookReading(
                "survivor_lead",
                day: 4,
                nominalMsv: 100f,
                source: "exposure_reactor",
                highEnergyEvent: false,
                antiRadBefore: true,
                antiRadAfter: true,
                rng: new SeededRng(42));

            var entry = doseLedger.GetEntry("survivor_lead");
            Assert.NotNull(entry);
            Assert.Equal(4, entry.lastAntiRadDay);
            // nominal 100 * 0.5 (before) * 1.0 (shielding) * 0.6 (after) = 30 booked mSv
            Assert.Equal(30f, entry.cumulativeMsv, 1);
        }
    }
}
