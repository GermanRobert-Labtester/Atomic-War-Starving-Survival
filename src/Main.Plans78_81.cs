using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Plans 78–81 flagship wiring: geodetic survey, kinetic flywheel storage,
    /// and chemical reconnaissance (decon airlock extension lives in the
    /// existing decontamination section). Follows the standard triad pattern
    /// (SetupXxx / SaveXxx / TickPlans78To81) and the registry sections
    /// geodetic_survey / kinetic_storage / chemical_recon.
    ///
    /// Wave 6 note: the four UI panels (DeconAirlockPanel, GeodeticSurveyPanel,
    /// KineticStoragePanel, ChemicalReconPanel) are google-stitch design
    /// deliverables — see the "Missing UI panels" registry in AGENTS.md.
    /// This partial wires sessions and persistence only; panels attach here
    /// once Stitch output is reconciled with the runtime theme.
    /// </summary>
    public sealed partial class Main
    {
        private GeodeticSurveyHostSession? _geodeticSurvey;
        private KineticStorageHostSession? _kineticStorage;
        private ChemicalReconHostSession? _chemicalRecon;

        // ─── Setup ───

        private void SetupGeodeticSurvey()
        {
            if (_geodeticSurvey != null) return;
            SetupCampaignDay();
            var fileIO = CatalogPath.CreateFileIOForDataDir(_dataDir);
            var json = new SystemTextJsonSerializer();
            var catalog = GeodeticSurveyCatalogLoader.Load(_dataDir, fileIO, json);

            var gsState = GeodeticSurveySaveStore.TryLoad() ?? new GeodeticSurveyState();
            var gsSys = new GeodeticSurveyEngine(
                catalog,
                _campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.WorldEvolution, 0, 15),
                new GodotLog());
            gsSys.RestoreState(gsState);
            _geodeticSurvey = new GeodeticSurveyHostSession(gsSys);
        }

        private void SetupKineticStorage()
        {
            if (_kineticStorage != null) return;
            SetupCampaignDay();
            var fileIO = CatalogPath.CreateFileIOForDataDir(_dataDir);
            var json = new SystemTextJsonSerializer();
            var catalog = KineticFlywheelCatalogLoader.Load(_dataDir, fileIO, json);

            var ksState = KineticStorageSaveStore.TryLoad() ?? new KineticStorageState();
            var ksSys = new KineticStorageSystem(
                catalog,
                _campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.Shelter, 0, 15),
                new GodotLog());
            ksSys.RestoreState(ksState);
            _kineticStorage = new KineticStorageHostSession(ksSys);
        }

        private void SetupChemicalRecon()
        {
            if (_chemicalRecon != null) return;
            SetupCampaignDay();
            var fileIO = CatalogPath.CreateFileIOForDataDir(_dataDir);
            var json = new SystemTextJsonSerializer();
            var catalog = ToxicChemicalCatalogLoader.Load(_dataDir, fileIO, json);

            var crState = ChemicalReconSaveStore.TryLoad() ?? new ChemicalReconState();
            var crSys = new ChemicalReconEngine(
                catalog,
                _campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.Expedition, 0, 15),
                new GodotLog());
            crSys.RestoreState(crState);
            _chemicalRecon = new ChemicalReconHostSession(crSys);
        }

        private void SetupPlans78To81()
        {
            SetupGeodeticSurvey();
            SetupKineticStorage();
            SetupChemicalRecon();
        }

        // ─── Save (triad) ───

        private void SaveGeodeticSurvey()
        {
            if (_geodeticSurvey != null)
                CaptureSection("geodetic_survey", GeodeticSurveySaveStore.TryCapturePersisted(_geodeticSurvey.System.CaptureState()));
        }

        private void SaveKineticStorage()
        {
            if (_kineticStorage != null)
                CaptureSection("kinetic_storage", KineticStorageSaveStore.TryCapturePersisted(_kineticStorage.System.CaptureState()));
        }

        private void SaveChemicalRecon()
        {
            if (_chemicalRecon != null)
                CaptureSection("chemical_recon", ChemicalReconSaveStore.TryCapturePersisted(_chemicalRecon.System.CaptureState()));
        }

        private void SavePlans78To81()
        {
            SaveGeodeticSurvey();
            SaveKineticStorage();
            SaveChemicalRecon();
        }

        // ─── Tick (single simulation owner per system; day cadence) ───

        private void TickPlans78To81(int day)
        {
            _geodeticSurvey?.System.TickDay(day);
            _kineticStorage?.System.TickDay(day);
            _chemicalRecon?.System.TickDay(day);
        }
    }
}
