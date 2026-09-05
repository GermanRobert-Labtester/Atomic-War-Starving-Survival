// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.AdvancedMachinery;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Medical;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Plans 146–149 flagship wiring: EB-PVD Thermal Barrier Coatings, Mine-Clearing Flail,
    /// Microfluidic Diagnostics, and Rail Grinding Corridors.
    /// Follows the standard triad pattern (SetupXxx / SaveXxx / TickPlans146To149)
    /// and the registry sections: route_infrastructure, ebpvd_coating, microfluidic_diagnostic,
    /// mine_clearing_flail, rail_grinding.
    /// </summary>
    public sealed partial class Main
    {
        private RouteInfrastructureSystem? _routeInfrastructure;
        private EbPvdCoatingHostSession? _ebPvdCoating;
        private MicrofluidicDiagnosticHostSession? _microfluidicDiagnostic;
        private MineClearingFlailHostSession? _mineClearingFlail;
        private RailGrindingHostSession? _railGrinding;

        private ISeededRng? _ebpvdRng;
        private ISeededRng? _microfluidicRng;
        private ISeededRng? _mineFlailRng;
        private ISeededRng? _railGrindingRng;
        private System.Collections.Generic.Dictionary<string, string>? _ebpvdSubstrateResults;

        private EbPvdCoatingPanel? _ebPvdCoatingPanel;
        private MicrofluidicDiagnosticPanel? _microfluidicDiagnosticPanel;
        private MineFlailPanel? _mineFlailPanel;
        private RailGrindingPanel? _railGrindingPanel;

        // ─── Setup ───

        private void SetupRouteInfrastructure()
        {
            if (_routeInfrastructure != null) return;
            var state = RouteInfrastructureSaveStore.TryLoad() ?? new RouteInfrastructureState();
            _routeInfrastructure = new RouteInfrastructureSystem(state);
        }

        private void SetupEbPvdCoating()
        {
            if (_ebPvdCoating != null) return;
            SetupCampaignDay();
            var state = EbPvdCoatingSaveStore.TryLoad() ?? new EbPvdCoatingState();
            var engine = new EbPvdCoatingEngine(state);
            LoadEbPvdCatalogInto(engine);
            _ebPvdCoating = new EbPvdCoatingHostSession(engine);
            _ebpvdRng = _campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.EbpvdCoating);
            // A finished coating mints the authored result item into inventory
            // (catalog substrate_classes mapping — engine records stay machine-local).
            _ebPvdCoating.System.OnJobCompleted += record =>
            {
                if (_ebpvdSubstrateResults != null &&
                    _ebpvdSubstrateResults.TryGetValue(record.SubstrateTag, out var resultItemId) &&
                    _inventory != null)
                {
                    _inventory.Inventory.AddById(resultItemId, 1);
                }
            };
        }

        private void SetupMicrofluidicDiagnostic()
        {
            if (_microfluidicDiagnostic != null) return;
            SetupCampaignDay();
            var state = MicrofluidicDiagnosticSaveStore.TryLoad() ?? new MicrofluidicDiagnosticState();
            var engine = new MicrofluidicDiagnosticEngine(state);
            LoadMicrofluidicCatalogInto(engine);
            _microfluidicDiagnostic = new MicrofluidicDiagnosticHostSession(engine);
            _microfluidicRng = _campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.MicrofluidicDiagnostics);
            // A finished manufacturing run mints the assay's cartridge item into
            // inventory; diagnostic runs consume one cartridge on start (host call).
            _microfluidicDiagnostic.System.OnCartridgeManufactured += (_, cartridgeItemId) =>
            {
                if (_inventory != null)
                {
                    _inventory.Inventory.AddById(cartridgeItemId, 1);
                }
            };
        }

        private void SetupMineClearingFlail()
        {
            if (_mineClearingFlail != null) return;
            SetupRouteInfrastructure();
            SetupCampaignDay();
            var state = MineClearingFlailSaveStore.TryLoad() ?? new MineClearingFlailState();
            var engine = new MineClearingFlailEngine(state);
            LoadMineFlailCatalogInto(engine);
            _mineClearingFlail = new MineClearingFlailHostSession(engine);
            _mineFlailRng = _campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.MineClearingFlail);
        }

        private void SetupRailGrinding()
        {
            if (_railGrinding != null) return;
            SetupRouteInfrastructure();
            SetupCampaignDay();
            var state = RailGrindingSaveStore.TryLoad() ?? new RailGrindingEngineState();
            var engine = new RailGrindingEngine(state);
            LoadRailGrindingCatalogInto(engine);
            _railGrinding = new RailGrindingHostSession(engine);
            _railGrindingRng = _campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.RailGrinding);
        }

        // ─── Catalog authority (JSON wins; engine seeds remain as fallback) ───

        private string ResolvePlans146DataDir() =>
            string.IsNullOrEmpty(_dataDir) ? CatalogPath.ResolveDataDir() : _dataDir;

        private void LoadEbPvdCatalogInto(EbPvdCoatingEngine engine)
        {
            try
            {
                string dataDir = ResolvePlans146DataDir();
                var fileIO = CatalogPath.CreateFileIOForDataDir(dataDir);
                var catalog = EbPvdCoatingCatalogLoader.Load(dataDir, fileIO, new SystemTextJsonSerializer());
                if (catalog.coatings.Count > 0)
                {
                    foreach (var def in catalog.ToCoatingDefs())
                    {
                        engine.RegisterCoating(def);
                    }
                    engine.SetFailureProfiles(catalog.ToFailureProfiles());
                }
                if (catalog.substrate_classes.Count > 0)
                {
                    _ebpvdSubstrateResults = new System.Collections.Generic.Dictionary<string, string>(StringComparer.Ordinal);
                    foreach (var s in catalog.substrate_classes)
                    {
                        if (!string.IsNullOrEmpty(s.tag) && !string.IsNullOrEmpty(s.result_item_id))
                        {
                            _ebpvdSubstrateResults[s.tag] = s.result_item_id;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[Plans146] ebpvd_coating_catalog.json load failed; engine seeds in force: {ex.Message}");
            }
        }

        private void LoadMicrofluidicCatalogInto(MicrofluidicDiagnosticEngine engine)
        {
            try
            {
                string dataDir = ResolvePlans146DataDir();
                var fileIO = CatalogPath.CreateFileIOForDataDir(dataDir);
                var catalog = MicrofluidicDiagnosticCatalogLoader.Load(dataDir, fileIO, new SystemTextJsonSerializer());
                if (catalog.assays.Count > 0)
                {
                    foreach (var def in catalog.ToAssayDefs())
                    {
                        engine.RegisterAssay(def);
                    }
                }
                if (catalog.machine.nominal_power_kw > 0f)
                {
                    engine.NominalPowerKw = catalog.machine.nominal_power_kw;
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[Plans148] microfluidic_diagnostic_catalog.json load failed; engine seeds in force: {ex.Message}");
            }
        }

        private void LoadMineFlailCatalogInto(MineClearingFlailEngine engine)
        {
            try
            {
                string dataDir = ResolvePlans146DataDir();
                var fileIO = CatalogPath.CreateFileIOForDataDir(dataDir);
                var catalog = MineFlailCatalogLoader.Load(dataDir, fileIO, new SystemTextJsonSerializer());
                foreach (var def in catalog.ToModuleDefs())
                {
                    engine.RegisterModule(def);
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[Plans147] mine_flail_catalog.json load failed; engine seeds in force: {ex.Message}");
            }
        }

        private void LoadRailGrindingCatalogInto(RailGrindingEngine engine)
        {
            try
            {
                string dataDir = ResolvePlans146DataDir();
                var fileIO = CatalogPath.CreateFileIOForDataDir(dataDir);
                var catalog = RailGrindingCatalogLoader.Load(dataDir, fileIO, new SystemTextJsonSerializer());
                foreach (var def in catalog.ToHeadDefs())
                {
                    engine.RegisterHead(def);
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[Plans149] rail_grinding_catalog.json load failed; engine seeds in force: {ex.Message}");
            }
        }

        private void SetupPlans146To149()
        {
            SetupRouteInfrastructure();
            SetupEbPvdCoating();
            SetupMicrofluidicDiagnostic();
            SetupMineClearingFlail();
            SetupRailGrinding();
        }

        // ─── Save (triad) ───

        private void SaveRouteInfrastructure()
        {
            if (_routeInfrastructure != null)
                CaptureSection("route_infrastructure", RouteInfrastructureSaveStore.TryCapturePersisted(_routeInfrastructure.CaptureState()));
        }

        private void SaveEbPvdCoating()
        {
            if (_ebPvdCoating != null)
                CaptureSection("ebpvd_coating", EbPvdCoatingSaveStore.TryCapturePersisted(_ebPvdCoating.System.CaptureState()));
        }

        private void SaveMicrofluidicDiagnostic()
        {
            if (_microfluidicDiagnostic != null)
                CaptureSection("microfluidic_diagnostic", MicrofluidicDiagnosticSaveStore.TryCapturePersisted(_microfluidicDiagnostic.System.CaptureState()));
        }

        private void SaveMineClearingFlail()
        {
            if (_mineClearingFlail != null)
                CaptureSection("mine_clearing_flail", MineClearingFlailSaveStore.TryCapturePersisted(_mineClearingFlail.System.CaptureState()));
        }

        private void SaveRailGrinding()
        {
            if (_railGrinding != null)
                CaptureSection("rail_grinding", RailGrindingSaveStore.TryCapturePersisted(_railGrinding.System.CaptureState()));
        }

        private void SavePlans146To149()
        {
            SaveRouteInfrastructure();
            SaveEbPvdCoating();
            SaveMicrofluidicDiagnostic();
            SaveMineClearingFlail();
            SaveRailGrinding();
        }

        // ─── Tick (day cadence) ───

        // One trained-operator working shift per campaign day. Jobs measured in
        // machine-hours (EB-PVD 3-6h) or machine-minutes (assays 25-60min) resolve
        // within a day-to-two-day cadence at this rate.
        private const float Plans146MachineHoursPerDay = 8f;

        private void TickPlans146To149(int day)
        {
            if (_ebPvdCoating != null)
            {
                float requiredKw = _ebPvdCoating.System.StateDto.ActiveJob?.BeamPowerKw ?? 10f;
                _ebPvdCoating.TickProcess(Plans146MachineHoursPerDay, BuildPlans146PowerContext(requiredKw), null, _ebpvdRng!);
            }

            if (_microfluidicDiagnostic != null)
            {
                // Clinical truth comes from the disease authority only; without it
                // the engine resolves runs as indeterminate rather than guessing.
                Func<string, string, bool>? clinicalTruth = null;
                if (_disease != null)
                {
                    var diseaseEngine = _disease.Engine;
                    clinicalTruth = (patientId, diseaseId) => diseaseEngine.IsInfected(patientId, diseaseId);
                }
                _microfluidicDiagnostic.Tick(
                    Plans146MachineHoursPerDay * 60f,
                    BuildPlans146PowerContext(_microfluidicDiagnostic.System.NominalPowerKw),
                    null,
                    _microfluidicRng!,
                    clinicalTruth);
            }

            if (_mineClearingFlail != null && _routeInfrastructure != null)
            {
                _mineClearingFlail.TickBreach(Plans146MachineHoursPerDay, day, _routeInfrastructure, null, _mineFlailRng!);
            }

            if (_railGrinding != null && _routeInfrastructure != null)
            {
                _railGrinding.TickGrinding(Plans146MachineHoursPerDay, day, _routeInfrastructure, null, _railGrindingRng!);
            }
        }

        /// <summary>
        /// Projects the power grid into the shared machine power context. Without a
        /// grid session the machines assume supply; otherwise net surplus (kW) feeds
        /// the brownout/throttle math owned by the Core engines.
        /// </summary>
        private PowerSupplyContext BuildPlans146PowerContext(float requiredKw)
        {
            var grid = _powerGrid?.System;
            if (grid == null)
            {
                return PowerSupplyContext.Full(requiredKw);
            }
            float availableKw = Math.Max(0f, grid.NetWatts) / 1000f;
            if (availableKw >= requiredKw)
            {
                return PowerSupplyContext.Full(Math.Max(availableKw, requiredKw));
            }
            return PowerSupplyContext.Throttled(availableKw, Math.Max(requiredKw, 0.001f));
        }

        // ─── UI Panel Construction & Binding ───

        private void BuildPlans146To149Panels()
        {
            SetupPlans146To149();

            _ebPvdCoatingPanel = new EbPvdCoatingPanel();
            _ebPvdCoatingPanel.Visible = false;
            _ebPvdCoatingPanel.OnClose += () => { if (_ebPvdCoatingPanel != null) _ebPvdCoatingPanel.Visible = false; };
            _ebPvdCoatingPanel.OnActionRequested += HandleEbPvdCoatingAction;
            if (_ebPvdCoating != null) _ebPvdCoatingPanel.Bind(_ebPvdCoating);
            AddChild(_ebPvdCoatingPanel);

            _microfluidicDiagnosticPanel = new MicrofluidicDiagnosticPanel();
            _microfluidicDiagnosticPanel.Visible = false;
            _microfluidicDiagnosticPanel.OnClose += () => { if (_microfluidicDiagnosticPanel != null) _microfluidicDiagnosticPanel.Visible = false; };
            _microfluidicDiagnosticPanel.OnActionRequested += HandleMicrofluidicDiagnosticAction;
            if (_microfluidicDiagnostic != null) _microfluidicDiagnosticPanel.Bind(_microfluidicDiagnostic);
            AddChild(_microfluidicDiagnosticPanel);

            _mineFlailPanel = new MineFlailPanel();
            _mineFlailPanel.Visible = false;
            _mineFlailPanel.OnClose += () => { if (_mineFlailPanel != null) _mineFlailPanel.Visible = false; };
            _mineFlailPanel.OnActionRequested += HandleMineFlailAction;
            if (_mineClearingFlail != null) _mineFlailPanel.Bind(_mineClearingFlail);
            AddChild(_mineFlailPanel);

            _railGrindingPanel = new RailGrindingPanel();
            _railGrindingPanel.Visible = false;
            _railGrindingPanel.OnClose += () => { if (_railGrindingPanel != null) _railGrindingPanel.Visible = false; };
            _railGrindingPanel.OnActionRequested += HandleRailGrindingAction;
            if (_railGrinding != null) _railGrindingPanel.Bind(_railGrinding);
            AddChild(_railGrindingPanel);
        }

        private void HandleEbPvdCoatingAction(string action, string param = "")
        {
            SetupEbPvdCoating();
            if (string.Equals(action, "OPEN", StringComparison.OrdinalIgnoreCase))
            {
                if (_ebPvdCoatingPanel != null)
                {
                    _ebPvdCoatingPanel.Visible = true;
                    _ebPvdCoatingPanel.RefreshView();
                }
            }
        }

        private void HandleMicrofluidicDiagnosticAction(string action, string param = "")
        {
            SetupMicrofluidicDiagnostic();
            if (string.Equals(action, "OPEN", StringComparison.OrdinalIgnoreCase))
            {
                if (_microfluidicDiagnosticPanel != null)
                {
                    _microfluidicDiagnosticPanel.Visible = true;
                    _microfluidicDiagnosticPanel.RefreshView();
                }
            }
        }

        private void HandleMineFlailAction(string action, string param = "")
        {
            SetupMineClearingFlail();
            if (string.Equals(action, "OPEN", StringComparison.OrdinalIgnoreCase))
            {
                if (_mineFlailPanel != null)
                {
                    _mineFlailPanel.Visible = true;
                    _mineFlailPanel.RefreshView();
                }
            }
        }

        private void HandleRailGrindingAction(string action, string param = "")
        {
            SetupRailGrinding();
            if (string.Equals(action, "OPEN", StringComparison.OrdinalIgnoreCase))
            {
                if (_railGrindingPanel != null)
                {
                    _railGrindingPanel.Visible = true;
                    _railGrindingPanel.RefreshView();
                }
            }
        }

        // ─── UI Tests ───

        private void RunEbPvdCoatingUiTestAndQuit()
        {
            BuildPlans146To149Panels();
            HandleEbPvdCoatingAction("OPEN");
            bool pass = _ebPvdCoatingPanel != null && _ebPvdCoatingPanel.IsBound;
            if (pass) GD.Print("EbPvdCoatingUiTest PASS");
            else GD.PrintErr("[FAIL] EbPvdCoatingPanel not ready");
            GetTree().Quit(pass ? 0 : 1);
        }

        private void RunMicrofluidicDiagnosticUiTestAndQuit()
        {
            BuildPlans146To149Panels();
            HandleMicrofluidicDiagnosticAction("OPEN");
            bool pass = _microfluidicDiagnosticPanel != null && _microfluidicDiagnosticPanel.IsBound;
            if (pass) GD.Print("MicrofluidicDiagnosticUiTest PASS");
            else GD.PrintErr("[FAIL] MicrofluidicDiagnosticPanel not ready");
            GetTree().Quit(pass ? 0 : 1);
        }

        private void RunMineFlailUiTestAndQuit()
        {
            BuildPlans146To149Panels();
            HandleMineFlailAction("OPEN");
            bool pass = _mineFlailPanel != null && _mineFlailPanel.IsBound;
            if (pass) GD.Print("MineFlailUiTest PASS");
            else GD.PrintErr("[FAIL] MineFlailPanel not ready");
            GetTree().Quit(pass ? 0 : 1);
        }

        private void RunRailGrindingUiTestAndQuit()
        {
            BuildPlans146To149Panels();
            HandleRailGrindingAction("OPEN");
            bool pass = _railGrindingPanel != null && _railGrindingPanel.IsBound;
            if (pass) GD.Print("RailGrindingUiTest PASS");
            else GD.PrintErr("[FAIL] RailGrindingPanel not ready");
            GetTree().Quit(pass ? 0 : 1);
        }
    }
}
