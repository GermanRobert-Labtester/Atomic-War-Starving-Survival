using Godot;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using AtomicWar.Journal;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Economy;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Foundry;
using Ashfall.Core.Inventory;
using Ashfall.Core.Journal;
using Ashfall.Core.Muster;
using Ashfall.Core.YearOfAsh;
using Ashfall.Core.Radio;
using Ashfall.Core.Survivors;
using AtomicWar.GodotApp.Economy;
using AtomicWar.GodotApp.YearOfAsh;
using AtomicWar.GodotApp.Muster;
using AtomicWar.GodotApp.Dose;
using AtomicWar.GodotApp.UtilityAI;
using AtomicWar.GodotApp.Radio;
using AtomicWar.GodotApp.Audio;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        /// <summary>Headless smoke: dose register surface builds, actions run, tabs render.</summary>
        private void RunDoseUiTestAndQuit()
        {
            BuildUserInterface();
            SetupDoseLedger();

            bool surface = _doseSurface != null;
            bool npcs = _doseLedger.Registers.npcs.Count == 4;

            _doseLedger.SealDemoSurvivors();
            string booked = _doseLedger.ScribeReading(120f, highEnergy: true);
            bool book = booked.Contains("band");
            bool diagnose = _doseLedger.DiagnoseDemo(DoseLedgerSystem.BandRed).Contains("Diagnosed");
            bool palliative = _doseLedger.SickList.AssignPalliative("survivor_gunner_mikhail", "plan_morphine_tray");
            string child = _doseLedger.BookDemoChild();
            bool cohort = child.Contains("corrected");
            bool volunteer = _doseLedger.SignDemoVolunteer().Contains("banked");

            string ledgerText = _doseLedger.LedgerLine();
            bool rendered = ledgerText.Contains("survivor_gunner_mikhail")
                && _doseLedger.SickList.Bands.Count == 1
                && _doseLedger.Cohort.Children.Count == 1
                && _doseLedger.Voluntary.Entries.Count == 1;

            bool pass = surface && npcs && book && diagnose && palliative && cohort && volunteer && rendered;

            // ---- Plan 60 / D5 + D3 + D7: illness reaches the sick list, and a
            // dose is spent from the one item authority, in a real host session. ----
            SetupDisease();
            SetupInventory();
            bool triageNamed = false, triageSource = false, triageBand = false;
            bool treated = false, spent = false, oddsImproved = false, windowHeld = false;

            if (_disease != null && _inventory?.Inventory != null)
            {
                int day = Math.Max(1, _simDay);
                // Stock is measured as a delta: the holdfast may already hold doses
                // from starting supplies, and "one dose spent" is the invariant,
                // not a magic absolute count.
                int radAwayBefore = _inventory.Inventory.CountById("rad_away");
                _inventory.Inventory.AddById("rad_away", 2);
                _inventory.Inventory.AddById("antibiotics", 2);
                radAwayBefore = _inventory.Inventory.CountById("rad_away");

                // ARS has no incubation, so triage must name it immediately — and it
                // has no cure, only care, so the honest claim is "better odds".
                _disease.Engine.Infect("survivor_gunner_mikhail", "disease_acute_radiation_syndrome", day);

                var triageEvents = new List<DayStateChangeEvent>();
                SyncDiseaseTriage(day, triageEvents);

                var illnessBand = _doseLedger.SickList.GetBand("survivor_gunner_mikhail");
                triageNamed = illnessBand != null && illnessBand.releaseDay < 0;
                triageSource = illnessBand != null
                    && illnessBand.severitySource == SickListSystem.SourceIllness;
                triageBand = illnessBand != null && illnessBand.band > DoseLedgerSystem.BandGreen;

                float rawLethality = _disease.Engine.GetEffectiveLethality(
                    "survivor_gunner_mikhail", "disease_acute_radiation_syndrome");
                var dose = _disease.Treat("survivor_gunner_mikhail",
                    "disease_acute_radiation_syndrome", "rad_away", day);
                treated = dose.Accepted;
                oddsImproved = _disease.Engine.GetEffectiveLethality(
                    "survivor_gunner_mikhail", "disease_acute_radiation_syndrome") < rawLethality;
                spent = _inventory.Inventory.CountById("rad_away") == radAwayBefore - 1;

                // A cure must be possible inside its window and impossible outside it.
                _disease.Engine.Infect("survivor_stoker_eyo", "disease_cholera", day);
                treated &= _disease.Treat("survivor_stoker_eyo", "disease_cholera", "antibiotics", day).Cured;

                _disease.Engine.Infect("survivor_harker_odell", "disease_cholera", day - 6);
                var latePatient = _disease.Engine.GetDiseaseState("disease_cholera")?.infected
                    .Find(p => p.survivor_id == "survivor_harker_odell");
                if (latePatient != null) latePatient.days_sick = 6;
                windowHeld = _disease.Treat("survivor_harker_odell", "disease_cholera", "antibiotics", day).Reason
                    == Ashfall.Core.Disease.DiseaseTreatmentRefusals.OutsideWindow;
            }

            // Plan 60 / D2 + D6 — the clinical picture and the bedside vigil have to
            // be reachable from the ward surface, not merely implemented underneath it.
            bool clinical = false, vigilHeld = false, vigilRecorded = false;
            if (_disease != null && _medical != null)
            {
                var note = _disease.ClinicalPicture("survivor_gunner_mikhail");
                clinical = note != null && !string.IsNullOrEmpty(note.Tell)
                    && !string.IsNullOrEmpty(note.TimingClue)
                    && !string.IsNullOrEmpty(note.DiseaseId);

                SetupMedicalWard();
                _medicalWardPanel?.BindVigil(_medical);
                vigilHeld = _medical.HoldVigil("survivor_gunner_mikhail").StartsWith("Vigil begun")
                    && _medical.VigilActive;
                // Keep it to the end, then confirm the campaign recorded the care.
                for (int i = 0; i < 600; i++) _medical.TickVigil(1d);
                vigilRecorded = Ashfall.Core.Medical.VigilCare.IsKept(_consequenceLedger, "survivor_gunner_mikhail");
            }

            bool medicalPass = triageNamed && triageSource && triageBand && treated
                && spent && oddsImproved && windowHeld && clinical && vigilHeld && vigilRecorded;

            pass = pass && medicalPass;

            // ---- Plan 81 UI fixes (docs/radiation/PLAN81_UI_AUDIT_81AU_81AX.md):
            // truthful sub-resolution dose units, display-name provenance lookup,
            // sector-aware content tab. Assert the rendered strings, not just
            // construction (UI-21 rule). ----
            string small = AshfallUiHelpers.FormatDoseMsv(0.00085f);
            bool unitPrecision = small == "0.85 µSv"
                && AshfallUiHelpers.FormatDoseMsv(120f) == "120.0 mSv"
                && AshfallUiHelpers.FormatDosePairMsv(0.00085f, 0.00085f) == "0.85/0.85 µSv"
                && AshfallUiHelpers.FormatDosePairMsv(120f, 72f) == "120.0/72.0 mSv"
                && AshfallUiHelpers.FormatDoseMsv(float.NaN) == "—";
            bool provenanceName = AshfallUiHelpers.FormatDoseSource(_doseLedger.Content, "loc_military_depot_perimeter") == "Military Depot Perimeter"
                && AshfallUiHelpers.FormatDoseSource(_doseLedger.Content, "loc_shelter_exterior_approach") == "Shelter Exterior Approach"
                && AshfallUiHelpers.FormatDoseSource(_doseLedger.Content, "demo_scan") == "demo_scan"
                && AshfallUiHelpers.FormatDoseSource(_doseLedger.Content, null) == "— exposure —";
            string contentTab = _doseSurface?.ContentTabText ?? string.Empty;
            bool sectorAware = contentTab.Contains("Irradiated Forest Edge")
                && contentTab.Contains("Military Depot Perimeter")
                && contentTab.Contains("expedition")
                && contentTab.Contains("faction")
                && contentTab.Contains("surface")
                && !contentTab.Contains("Rooms — standing places:");
            bool plan81UiPass = unitPrecision && provenanceName && sectorAware;
            pass = pass && plan81UiPass;

            GD.Print($"[DoseUiTest] surface={surface} npcs={npcs} book={book} diagnose={diagnose} " +
                     $"palliative={palliative} cohort={cohort} volunteer={volunteer} rendered={rendered}");
            GD.Print($"[DoseUiTest] medical: triage={triageNamed} source={triageSource} band={triageBand} " +
                     $"treated={treated} spent={spent} odds={oddsImproved} window={windowHeld} " +
                     $"clinical={clinical} vigil={vigilHeld}/{vigilRecorded}");
            GD.Print($"[DoseUiTest] plan81-ui: units={unitPrecision} provenance={provenanceName} sectors={sectorAware}");
            HostCli.EmitSummary("dose_uitest", pass, pass ? 0 : 1);
            QuitUiTestAfterFrame(pass ? 0 : 1);
        }

    }
}
