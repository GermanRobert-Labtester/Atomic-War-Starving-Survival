// SPDX-License-Identifier: MIT
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
using Ashfall.Core.IO;
using Ashfall.Core.Radiation;
using Ashfall.Core.Shelter;
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
        // ── Survivor / UtilityAI fields (GAP-ARCH-01 Phase 1) ──
        private SurvivorsHostSession _survivors = null!;
        public SurvivorsHostSession? Survivors => _survivors;
        private UtilityAiHostSession _utilityAi = null!;
        private StartingCohortCatalog _startingCohortCatalog = null!;
        private string _startingCohortProfileId = StartingCohortCatalog.StandardProfileId;
        private bool _survivorInitializationApplied;
        private bool _isRestoringSurvivorState;

        private static string FormatSurvivorName(string id)
        {
            if (string.IsNullOrEmpty(id)) return "Unknown";
            return System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(id.Replace('_', ' '));
        }

        private void SetupSurvivors()
        {
            if (_survivors == null)
            {
                _survivors = new SurvivorsHostSession();
                _survivors.LoadCatalog(_dataDir);
                SetupEnrichment();

                // Wire environmental exposure from location catalogs, weather, and active expeditions
                var locRads = ExposureEnvironmentResolver.LoadLocationRadRates(
                    _dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
                _survivors.ExposureResolver.LocationRadRateProvider = locId =>
                    locRads.TryGetValue(locId, out float r) ? r : ExposureEnvironmentResolver.DefaultWastelandOutdoorRadRate;
                _survivors.ExposureResolver.WeatherRadModifierProvider = () => _world?.Weather?.OutdoorRadModifier ?? 0f;
                // C2 / Plan 21A (P7) — weather-scaled protective wear: the
                // canonical WeatherSystem melt multiplier (black rain ×5).
                _survivors.BindHazmatWearMultiplier(() => _world?.Weather?.HazmatDegradeMultiplier ?? 1f);
                // Fallout plume contamination overlays expedition/outdoor exposure when clouds overlap a location.
                _survivors.ExposureResolver.FalloutContaminationProvider = locId =>
                {
                    if (_fallout == null || string.IsNullOrEmpty(locId)) return 0f;
                    return _fallout.GetLocationContamination(locId);
                };
                // Plan 176 — anomaly/storm-front radiation overlays expedition exposure
                // the same way (typed handoff; RadiationSystem still owns the dose).
                _survivors.ExposureResolver.AnomalyRadRateProvider = locId =>
                {
                    if (_anomalyHazard == null || string.IsNullOrEmpty(locId)) return 0f;
                    return GetAnomalyLocationRate(locId);
                };
                // C2 / Plan 20B — one shelter shielding/interior-radiation model.
                // Every provider reads a canonical owner lazily so setup order
                // cannot drop a seam; unbound systems degrade to nominal values
                // (healthy filter/duct/airlock, no internal sources) which keeps
                // the legacy interior math byte-identical.
                var shieldCatalog = ShelterShieldingCatalog.LoadFromDirectory(_dataDir, new FileSystemIO());
                if (shieldCatalog.IsValid)
                {
                    _survivors.ExposureResolver.ShelterInteriorBaseRadRate =
                        shieldCatalog.interior_baseline_rad_rate;
                    var shielding = new ShelterShieldingModel(shieldCatalog)
                    {
                        StructuralAttenuationProvider = () => _survivors.Shelter.GetWeakestCeilingAttenuation(),
                        FilterHealthPercentProvider = () => _startingLevel?.System.State.airFilterHealthPercent ?? 100f,
                        VentilationDuctIntegrityProvider = () => _ventilation?.State.ductIntegrity ?? 100f,
                        VentilationFilterSaturationProvider = () => _ventilation?.State.exhaustFilterSaturation ?? 0f,
                        VentilationRecirculationProvider = () => _ventilation?.State.emergencyRecirculationMode ?? false,
                        AirlockSealProvider = () => AirlockSealFactor(_airlockSecurity?.System.State.doorState),
                        AirlockIncidentProvider = () => _airlockSecurity?.System.HasPendingIncident ?? false,
                        WeatherRadModifierProvider = () => _world?.Weather?.OutdoorRadModifier ?? 0f,
                        IndoorRadonProvider = () => _startingLevel?.System.State.radonLevelBqm3 ?? 0f,
                        FloodingContaminationProvider = () => MaxSumpContamination(),
                        ShelterContaminationProvider = () => _decontamination?.System.State.shelterContaminationLevel ?? 0f,
                        DeconActiveProvider = () => _decontamination?.System.HasActiveCase ?? false
                    };
                    _survivors.BindShelterShieldingModel(shielding);
                }
                _survivors.ExposureResolver.SurvivorLocationQuery = id =>
                {
                    if (_expeditions?.Engine != null &&
                        _expeditions.Engine.Active.TryGetValue(id, out var exp))
                    {
                        return (SurvivorExposureLocation.Expedition, exp.locationId);
                    }
                    return (SurvivorExposureLocation.ShelterInterior, "");
                };

                _survivors.StateChanged += () =>
                {
                    SaveSurvivors();
                    _survivorsOverlay?.RefreshView();
                    _medicalPanel?.RefreshView();
                    _shelterPanel?.RefreshView();
                    if (_state == GameState.Playing && !_isRestoringSurvivorState)
                        UpdateHud();
                };

                _survivors.OnSurvivorExposed += (survivorId, delta) =>
                {
                    ApplyRadiationExposure(survivorId, delta, _simDay);
                };
            }

            if (_inventory != null)
            {
                _survivors.Inventory = _inventory;
                _inventory.Survivors = _survivors;
            }
            if (_holdfastRuntime != null)
            {
                _holdfastRuntime.Survivors = _survivors;
            }

            if (_campaignInitializationMode == CampaignInitializationMode.FreshInitialize &&
                _survivors.RosterState.Count == 0)
            {
                var cohort = ResolveStartingCohort(_startingCohortProfileId);
                _survivors.LoadStartingCohort(cohort);
                _survivorInitializationApplied = true;
            }
            else if (_campaignInitializationMode == CampaignInitializationMode.Restore &&
                     !_survivorInitializationApplied)
            {
                var save = SurvivorsSaveStore.TryLoad();
                if (save != null)
                {
                    _isRestoringSurvivorState = true;
                    try
                    {
                        _survivors.RestoreSave(save);
                    }
                    finally
                    {
                        _isRestoringSurvivorState = false;
                    }
                    GD.Print(
                        $"[Ashfall Godot] Survivors restore applied: slices={save.survivors?.Count ?? 0} " +
                        $"roster={save.roster?.entries?.Count ?? 0} live={_survivors.RosterState.Count}.");
                }
                // A missing save is also a completed restore decision. Never
                // fall through to a fresh cohort on a later SetupSurvivors.
                _survivorInitializationApplied = true;
            }
        }

        /// <summary>C2 / Plan 20B — canonical airlock seal factor from the
        /// authored door state (Secure 1 … Breached 0); unbound = secure.</summary>
        private static float AirlockSealFactor(AirlockDoorState? doorState)
        {
            return doorState switch
            {
                AirlockDoorState.Secure => 1f,
                AirlockDoorState.Cycling => 0.5f,
                AirlockDoorState.Open => 0f,
                AirlockDoorState.Breached => 0f,
                _ => 1f
            };
        }

        /// <summary>C2 / Plan 20B — worst-room sump contamination (0..1) from the
        /// canonical flooding authority; unbound/empty = dry.</summary>
        private float MaxSumpContamination()
        {
            var nodes = _sumpFlooding?.System.State.nodes;
            if (nodes == null) return 0f;
            float max = 0f;
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i] == null) continue;
                if (nodes[i].contaminationLevel > max) max = nodes[i].contaminationLevel;
            }
            return max;
        }

        private StartingCohortCatalog EnsureStartingCohortCatalog()
        {
            if (_startingCohortCatalog != null) return _startingCohortCatalog;

            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();
            var canonical = SurvivorCatalogLoader.Load(_dataDir, fileIO, serializer);
            var loaded = StartingCohortCatalogLoader.LoadDetailed(
                _dataDir,
                fileIO,
                serializer,
                canonical);

            if (loaded.Errors.Count > 0)
            {
                throw new InvalidOperationException(
                    "Starting cohort catalog is invalid: " +
                    string.Join("; ", loaded.Errors));
            }

            _startingCohortCatalog = loaded.Catalog;
            return _startingCohortCatalog;
        }

        private StartingCohortProfile ResolveStartingCohort(string profileId)
        {
            var catalog = EnsureStartingCohortCatalog();
            if (catalog.TryGet(profileId, out var profile))
                return profile;

            GD.PushWarning(
                $"[Ashfall Godot] Unknown starting cohort '{profileId}', using Standard Holdfast.");
            return catalog.DefaultProfile;
        }

        private void SetupUtilityAi()
        {
            if (_utilityAi != null) return;
            _utilityAi = UtilityAiHostSession.Create(_dataDir);

            if (_utilityAiPanel == null && _rightColumn != null)
            {
                _utilityAiPanel = new UtilityAiPanel();
                _rightColumn.AddChild(_utilityAiPanel);
            }
            if (_utilityAiPanel != null)
            {
                _utilityAiPanel.BindSession(_utilityAi);
                _utilityAiPanel.RefreshView();
            }
        }

        private void OnUtilityAiEvaluateClicked()
        {
            SetupUtilityAi();
            _statusLabel.Text = _utilityAi.EvaluateDemo("survivor_gunner_mikhail", 30f, 0.7f);
            _utilityAiPanel.RefreshView();
        }

        private void OnSurvivorsOpenClicked()
        {
            SetupSurvivors();
            _statusLabel.Text = "Survivors panel open.";
            _codexViewer.Text = _survivors.StatusLine();
        }

        private void OnSurvivorsTickClicked()
        {
            SetupSurvivors();
            _survivors.TickHour(6f);
            SetupPhase0();
            _phase0.TickHour(6f);
            _statusLabel.Text = _survivors.LastEvent + "\n" + _phase0.LastEvent;
            _codexViewer.Text = _survivors.StatusLine();
        }

        private void OnSurvivorsExposeClicked(string id, float rads)
        {
            SetupSurvivors();
            _statusLabel.Text = _survivors.ExposeToZone(id, rads);
            _codexViewer.Text = _survivors.StatusLine();
        }

        private void OnSurvivorsIodineClicked(string id)
        {
            SetupSurvivors();
            _statusLabel.Text = _survivors.AdministerIodine(id);
            _codexViewer.Text = _survivors.StatusLine();
        }

        private void OnSurvivorsAntiRadClicked(string id, float rads)
        {
            SetupSurvivors();
            _statusLabel.Text = _survivors.AdministerAntiRad(id, rads);
            _codexViewer.Text = _survivors.StatusLine();
        }

        private void SaveSurvivors()
        {
            if (_survivors == null) return;
            if (CaptureSection("survivors", SurvivorsSaveStore.TryCapturePersisted(_survivors.CaptureSave())))
                GD.Print("[Ashfall Godot] Survivors save written.");
        }

        private void CloseSurvivorsOverlay()
        {
            _survivorsOverlay.Visible = false;
        }

        private void CloseSurvivorDetailPanel()
        {
            _survivorDetailPanel.Visible = false;
        }

    }
}
