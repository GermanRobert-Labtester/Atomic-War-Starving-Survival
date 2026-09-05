using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Catalogs;
using Ashfall.Core.Culture;
using Ashfall.Core.Diplomacy;
using Ashfall.Core.Institutions;
using Ashfall.Core.Sanatorium;
using Ashfall.Core.Shelter;
using Ashfall.Core.SkyDefense;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Flagship institutions (Tasks 5-8) host wiring: composition, catalog
    /// loading, save-section triads (SetupXxx/SaveXxx), daily tick owner and
    /// availability claim re-registration after restore (plan §10 step 11).
    /// Presentation-only rules respected: gameplay lives in Core; this partial
    /// only constructs, binds, ticks, persists.
    /// </summary>
    public partial class Main
    {
        private const int FlagshipMasterSeed = 42;

        private InstitutionAssignmentLedger? _institutionLedger;
        private CulturalArchiveVaultSystem? _culturalArchive;
        private DiplomaticSummitSystem? _diplomaticSummit;
        private SkyDefenseBatterySystem? _skyDefense;
        private PsychologicalSanatoriumSystem? _sanatorium;

        private bool _culturalArchiveDirty;
        private bool _diplomaticSummitDirty;
        private bool _skyDefenseDirty;
        private bool _sanatoriumDirty;

        private static string FlagshipDataDir =>
            System.IO.Path.Combine(
                ProjectSettings.GlobalizePath("res://"), "Assets", "StreamingAssets", "Data");

        private InstitutionAssignmentLedger EnsureInstitutionLedger() =>
            _institutionLedger ??= new InstitutionAssignmentLedger();

        /// <summary>
        /// The live campaign telemetry (world-owned). Sky defense consumes the
        /// SAME instance the radio/warning UIs consume — never a parallel one
        /// (plan §9.7).
        /// </summary>
        private OrbitalHarrowTelemetrySystem EnsureOrbitalHarrowTelemetry()
        {
            var telemetry = _world?.WeatherIntelligence.Orbital;
            if (telemetry == null)
                telemetry = new OrbitalHarrowTelemetrySystem(new SkyLayerArmorSystem(), new SeededRng(FlagshipMasterSeed));
            return telemetry;
        }

        private void MarkCulturalArchiveDirty() => _culturalArchiveDirty = true;
        private void MarkDiplomaticSummitDirty() => _diplomaticSummitDirty = true;
        private void MarkSkyDefenseDirty() => _skyDefenseDirty = true;
        private void MarkSanatoriumDirty() => _sanatoriumDirty = true;

        // -----------------------------------------------------------------
        // Composition (idempotent; each system constructed exactly once)
        // -----------------------------------------------------------------

        private CulturalArchiveVaultSystem EnsureCulturalArchive()
        {
            if (_culturalArchive != null) return _culturalArchive;
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            // No authoritative shelter-humidity authority exists (plan §5.9
            // "if available") — the deep vault reads as a dry environment.
            _culturalArchive = new CulturalArchiveVaultSystem(
                _inventory.Inventory,
                availability: EnsureInstitutionLedger());
            _culturalArchive.LoadTomeCatalog(
                CulturalArchiveTomeCatalogLoader.Load(FlagshipDataDir, fileIO, json));
            _culturalArchive.OnDocumentRestored += _ => MarkCulturalArchiveDirty();
            _culturalArchive.OnMicroficheCreated += _ => MarkCulturalArchiveDirty();
            _culturalArchive.OnTomeTranscribed += _ => MarkCulturalArchiveDirty();
            _culturalArchive.OnDocumentLost += _ => MarkCulturalArchiveDirty();
            _culturalArchive.OnArchiveRecordingCreated += (_, _) => MarkCulturalArchiveDirty();
            _culturalArchive.OnSalonStarted += _ => MarkCulturalArchiveDirty();
            _culturalArchive.OnSalonEnded += _ => MarkCulturalArchiveDirty();
            _culturalArchive.OnChronicleEntryAdded += _ => MarkCulturalArchiveDirty();

            var saved = CulturalArchiveSaveStore.TryLoad();
            if (saved != null)
                _culturalArchive.RestoreState(saved);
            return _culturalArchive;
        }

        private DiplomaticSummitSystem EnsureDiplomaticSummit()
        {
            if (_diplomaticSummit != null) return _diplomaticSummit;
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            _diplomaticSummit = new DiplomaticSummitSystem(
                FlagshipMasterSeed,
                inventory: _inventory.Inventory,
                availability: EnsureInstitutionLedger());
            _diplomaticSummit.LoadTreatyCatalog(
                DiplomaticTreatyCatalogLoader.Load(FlagshipDataDir, fileIO, json));
            _diplomaticSummit.OnSummitScheduled += _ => MarkDiplomaticSummitDirty();
            _diplomaticSummit.OnTreatyRatified += _ => MarkDiplomaticSummitDirty();
            _diplomaticSummit.OnTreatyViolationRecorded += _ => MarkDiplomaticSummitDirty();
            _diplomaticSummit.OnTreatyEnded += (_, _) => MarkDiplomaticSummitDirty();
            _diplomaticSummit.OnGuaranteeExchanged += _ => MarkDiplomaticSummitDirty();
            _diplomaticSummit.OnGuaranteeReleased += _ => MarkDiplomaticSummitDirty();

            var saved = DiplomaticSummitSaveStore.TryLoad();
            if (saved != null)
                _diplomaticSummit.RestoreState(saved);
            return _diplomaticSummit;
        }

        private SkyDefenseBatterySystem EnsureSkyDefense()
        {
            if (_skyDefense != null) return _skyDefense;
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            _skyDefense = new SkyDefenseBatterySystem(
                FlagshipMasterSeed,
                inventory: _inventory.Inventory,
                telemetry: EnsureOrbitalHarrowTelemetry(),
                availability: EnsureInstitutionLedger());
            _skyDefense.LoadOrdnanceCatalog(
                SkyDefenseOrdnanceCatalogLoader.Load(FlagshipDataDir, fileIO, json));
            _skyDefense.OnOrbitalTrackAcquired += _ => MarkSkyDefenseDirty();
            _skyDefense.OnVolleyFired += (_, _, _) => MarkSkyDefenseDirty();
            _skyDefense.OnInterceptResolved += (_, _, _, _) => MarkSkyDefenseDirty();
            _skyDefense.OnServiced += _ => MarkSkyDefenseDirty();

            var saved = SkyDefenseBatterySaveStore.TryLoad();
            if (saved != null)
                _skyDefense.RestoreState(saved);
            _skyDefense.EnsureDefaultTurret();
            return _skyDefense;
        }

        private PsychologicalSanatoriumSystem EnsureSanatorium()
        {
            if (_sanatorium != null) return _sanatorium;
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            _sanatorium = new PsychologicalSanatoriumSystem(
                FlagshipMasterSeed,
                inventory: _inventory.Inventory,
                availability: EnsureInstitutionLedger());
            _sanatorium.LoadTherapyCatalog(
                PsychologicalTherapyCatalogLoader.Load(FlagshipDataDir, fileIO, json));
            _sanatorium.OnPatientAdmitted += _ => MarkSanatoriumDirty();
            _sanatorium.OnTherapyStarted += (_, _) => MarkSanatoriumDirty();
            _sanatorium.OnTherapyCompleted += (_, _) => MarkSanatoriumDirty();
            _sanatorium.OnPatientRelapsed += (_, _, _) => MarkSanatoriumDirty();
            _sanatorium.OnPatientDischarged += _ => MarkSanatoriumDirty();

            var saved = PsychologicalSanatoriumSaveStore.TryLoad();
            if (saved != null)
                _sanatorium.RestoreState(saved);
            return _sanatorium;
        }

        // -----------------------------------------------------------------
        // Setup triad (restore-order chain) + claim re-registration
        // -----------------------------------------------------------------

        private void SetupCulturalArchive()
        {
            EnsureCulturalArchive();
            foreach (var doc in _culturalArchive.Documents)
                if (!string.IsNullOrEmpty(doc.active_scholar_id))
                    EnsureInstitutionLedger().TryClaim(
                        doc.active_scholar_id, CulturalArchiveVaultSystem.InstitutionId, "scholar");
        }

        private void SetupDiplomaticSummit()
        {
            EnsureDiplomaticSummit();
            foreach (var g in _diplomaticSummit.Guarantees.Where(g => g.status == "exchanged"))
                EnsureInstitutionLedger().TryClaim(
                    g.survivor_id, DiplomaticSummitSystem.InstitutionId, "guarantee");
        }

        private void SetupSkyDefense()
        {
            EnsureSkyDefense();
            foreach (var turret in _skyDefense.Turrets)
                foreach (var crew in turret.assigned_crew_ids)
                    EnsureInstitutionLedger().TryClaim(
                        crew, SkyDefenseBatterySystem.InstitutionId, "gunner");
        }

        private void SetupSanatorium()
        {
            EnsureSanatorium();
            foreach (var p in _sanatorium.Patients.Where(p => p.status == "admitted"))
                EnsureInstitutionLedger().TryClaim(
                    p.survivor_id, PsychologicalSanatoriumSystem.InstitutionId, "patient");
        }

        /// <summary>All four institutions in dependency order (after world/telemetry bases).</summary>
        private void SetupFlagshipInstitutions()
        {
            SetupSkyDefense();
            SetupCulturalArchive();
            SetupDiplomaticSummit();
            SetupSanatorium();
        }

        // -----------------------------------------------------------------
        // Save triad
        // -----------------------------------------------------------------

        private void SaveCulturalArchive()
        {
            if (_culturalArchive == null) return;
            if (!CaptureSection("cultural_archives",
                    CulturalArchiveSaveStore.TryCapturePersisted(_culturalArchive.CaptureState())))
                return;
            _culturalArchiveDirty = false;
        }

        private void SaveDiplomaticSummit()
        {
            if (_diplomaticSummit == null) return;
            if (!CaptureSection("diplomatic_summits",
                    DiplomaticSummitSaveStore.TryCapturePersisted(_diplomaticSummit.CaptureState())))
                return;
            _diplomaticSummitDirty = false;
        }

        private void SaveSkyDefense()
        {
            if (_skyDefense == null) return;
            if (!CaptureSection("sky_defense_battery",
                    SkyDefenseBatterySaveStore.TryCapturePersisted(_skyDefense.CaptureState())))
                return;
            _skyDefenseDirty = false;
        }

        private void SaveSanatorium()
        {
            if (_sanatorium == null) return;
            if (!CaptureSection("psychological_sanatorium",
                    PsychologicalSanatoriumSaveStore.TryCapturePersisted(_sanatorium.CaptureState())))
                return;
            _sanatoriumDirty = false;
        }

        private void SaveFlagshipInstitutionsIfDirty()
        {
            if (_culturalArchiveDirty) SaveCulturalArchive();
            if (_diplomaticSummitDirty) SaveDiplomaticSummit();
            if (_skyDefenseDirty) SaveSkyDefense();
            if (_sanatoriumDirty) SaveSanatorium();
        }

        // -----------------------------------------------------------------
        // Daily tick owner (phase 5 — after survivors/medical, before final)
        // -----------------------------------------------------------------

        private sealed class FlagshipInstitutionsDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public FlagshipInstitutionsDayOwner(Main m) => _m = m;

            public void CapturePreDaySnapshot(int day) { }

            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupSkyDefense();
                _m.SetupCulturalArchive();
                _m.SetupDiplomaticSummit();
                _m.SetupSanatorium();

                _m._skyDefense!.TickDay(day);
                _m._culturalArchive!.TickDay(day);
                _m._diplomaticSummit!.TickDay(day);
                _m._sanatorium!.TickDay(day);

                _m.SaveFlagshipInstitutionsIfDirty();
                events.Add(new DayStateChangeEvent("flagship_institutions_ticked",
                    "flagship_institutions", null, null, day));
            }
        }

        /// <summary>Called from RegisterProductionCampaignOwners (phase 5).</summary>
        private void RegisterFlagshipInstitutionsOwner()
        {
            if (_campaignDay == null) return;
            _campaignDay.Register("flagship_institutions", new FlagshipInstitutionsDayOwner(this), phase: 5);
        }
    }
}
