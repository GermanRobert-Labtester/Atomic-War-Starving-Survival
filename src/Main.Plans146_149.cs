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
            // Fresh saves have no authored corridor state — seed the flagship
            // minefield and corrugated rail so flail/grinder commands and
            // expedition modifiers have a real segment to act on.
            BootstrapDefaultRouteInfrastructure(_routeInfrastructure, _simDay > 0 ? _simDay : 1);
            WirePlans146ExpeditionRouteModifiers();
        }

        /// <summary>
        /// Registers the canonical demining and rail corridors when the route
        /// section is empty. Never overwrites a restored save that already has
        /// segments.
        /// </summary>
        private static void BootstrapDefaultRouteInfrastructure(RouteInfrastructureSystem routes, int day)
        {
            if (routes == null || routes.GetAllSegments().Count > 0) return;
            routes.RegisterMinefield("expedition_corridor_north", "seg_mine_gap", density01: 0.85f, day: day);
            routes.RegisterMinefield("route_ashfall_pass", "seg_pass_alpha", density01: 0.65f, day: day);
            routes.RegisterRailSegment("rail_trunk_iron_vein", "sector_deep_quarry", initialRoughness: 0.90f, safeSpeedKph: 25.0f, day: day, designSpeedKph: 55f);
            routes.RegisterRailSegment("rail_trunk_iron_reach", "sector_main_line", initialRoughness: 0.82f, safeSpeedKph: 30.0f, day: day, designSpeedKph: 60f);
        }

        /// <summary>
        /// Binds route hazard/travel modifiers into the expedition estimate
        /// preview and refreshes the live encounter composer so minefields
        /// and ground rails affect player-facing travel numbers.
        /// </summary>
        private void WirePlans146ExpeditionRouteModifiers()
        {
            if (_routeInfrastructure == null) return;
            if (_expeditions != null)
            {
                var routes = _routeInfrastructure;
                _expeditions.SetEstimateRouteModifiers(
                    locationId => routes.GetHazardModifier(locationId),
                    locationId => routes.GetTravelModifier(locationId));
            }
            // Reinstall the encounter composer so GetHazardModifier is included
            // even when evolving-world wiring ran before route setup.
            if (_expeditions != null)
            {
                _expeditionDangerComposer = ComposeExpeditionDangerMultiplier();
                _expeditions.SetEncounterChanceMultiplier(_expeditionDangerComposer);
            }
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
            // Assay results write into the medical diagnosis ledger (disease ids
            // are affliction definition ids). Presentation stays in LastEvent.
            _microfluidicDiagnostic.System.OnRunCompleted += ApplyMicrofluidicResultToDiagnosis;
        }

        private void ApplyMicrofluidicResultToDiagnosis(MicrofluidicDiagnosticResult res)
        {
            if (res == null || string.IsNullOrEmpty(res.PatientId) || string.IsNullOrEmpty(res.TargetDiseaseId))
                return;
            if (res.ResultKind == DiagnosticResultKind.Invalid
                || res.ResultKind == DiagnosticResultKind.Indeterminate
                || res.ResultKind == DiagnosticResultKind.Pending)
                return;

            EnsureMedicalPipeline();
            var pipeline = _medical?.Pipeline;
            if (pipeline == null) return;
            if (!Ashfall.Core.Survivors.SurvivorId.TryParse(res.PatientId, out var survivor))
                return;
            if (!Ashfall.Core.Medical.AfflictionId.IsValid(res.TargetDiseaseId, out _))
                return;

            var definition = new Ashfall.Core.Medical.AfflictionId(res.TargetDiseaseId);
            var episode = Ashfall.Core.Medical.AfflictionEpisodeId.Create(survivor, definition);
            int day = _simDay > 0 ? _simDay : 1;
            string detail = $"microfluidic:{res.AssayId}:{res.ResultKind}:{res.Confidence01:F2}";

            if (res.ResultKind == DiagnosticResultKind.Positive)
            {
                if (res.Confidence01 >= 0.85f)
                    pipeline.Diagnosis.Confirm(episode, day, detail);
                else
                    pipeline.SuspectFromEvidence(survivor, definition, day, detail);
            }
            else if (res.ResultKind == DiagnosticResultKind.Negative)
            {
                pipeline.Diagnosis.RuleOut(episode, day, detail);
            }
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
            _mineClearingFlail.Routes = _routeInfrastructure;
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
            _railGrinding.Routes = _routeInfrastructure;
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
            WirePlans146ExpeditionRouteModifiers();
        }

        private string ResolvePlans146OperatorId()
        {
            if (_survivors != null)
            {
                foreach (var s in _survivors.RosterState)
                {
                    if (s != null && s.IsAliveState && !string.IsNullOrEmpty(s.Id))
                        return s.Id;
                }
            }
            return "shelter_operator";
        }

        /// <summary>
        /// Plans 146–149 MED: push living roster ids into the assay panel so
        /// START ASSAY RUN emits assayId|patientId (defaults to first living).
        /// </summary>
        private void SyncMicrofluidicPatientCandidates()
        {
            if (_microfluidicDiagnosticPanel == null) return;
            var patients = new System.Collections.Generic.List<string>();
            if (_survivors != null)
            {
                foreach (var s in _survivors.RosterState)
                {
                    if (s != null && s.IsAliveState && !string.IsNullOrEmpty(s.Id))
                        patients.Add(s.Id);
                }
            }
            if (patients.Count == 0)
                patients.Add(ResolvePlans146OperatorId());
            _microfluidicDiagnosticPanel.SetPatientCandidates(patients);
        }

        private bool TryConsumePlans146Demands(System.Collections.Generic.IReadOnlyList<InventoryDemand> demands)
        {
            if (_inventory == null || demands == null || demands.Count == 0) return false;
            var bill = new System.Collections.Generic.Dictionary<string, int>(StringComparer.Ordinal);
            for (int i = 0; i < demands.Count; i++)
            {
                var d = demands[i];
                if (d == null || string.IsNullOrEmpty(d.ItemId) || d.Quantity <= 0) continue;
                if (bill.TryGetValue(d.ItemId, out int existing))
                    bill[d.ItemId] = existing + d.Quantity;
                else
                    bill[d.ItemId] = d.Quantity;
            }
            if (bill.Count == 0) return false;
            return _inventory.Inventory.TryConsumeBill(bill);
        }

        private static bool TrySplitPlans146Param(string param, out string left, out string right)
        {
            left = string.Empty;
            right = string.Empty;
            if (string.IsNullOrEmpty(param)) return false;
            int sep = param.IndexOf('|');
            if (sep <= 0 || sep >= param.Length - 1) return false;
            left = param.Substring(0, sep);
            right = param.Substring(sep + 1);
            return !string.IsNullOrEmpty(left) && !string.IsNullOrEmpty(right);
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

        // Composite orchestration only; the child SaveXxx methods are the
        // registered campaign sections.
        private void PersistPlans146To149()
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

        private void EnsurePlans146To149Panels()
        {
            if (_ebPvdCoatingPanel != null) return;
            BuildPlans146To149Panels();
        }

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
            EnsurePlans146To149Panels();
            if (_ebPvdCoating == null) return;

            if (string.Equals(action, "CLOSE", StringComparison.OrdinalIgnoreCase))
            {
                if (_ebPvdCoatingPanel != null) _ebPvdCoatingPanel.Visible = false;
                return;
            }

            if (string.Equals(action, "OPEN", StringComparison.OrdinalIgnoreCase))
            {
                if (_ebPvdCoatingPanel != null)
                {
                    _ebPvdCoatingPanel.Visible = true;
                    _ebPvdCoatingPanel.RefreshView();
                }
                return;
            }

            if (string.Equals(action, "start_coating", StringComparison.OrdinalIgnoreCase))
            {
                // param: coatingId|substrateTag[|bond]
                string coatingId = param ?? string.Empty;
                string substrateTag = "superalloy_blade";
                bool applyBond = true;
                if (!string.IsNullOrEmpty(param))
                {
                    var parts = param.Split('|');
                    if (parts.Length >= 1 && !string.IsNullOrEmpty(parts[0])) coatingId = parts[0];
                    if (parts.Length >= 2 && !string.IsNullOrEmpty(parts[1])) substrateTag = parts[1];
                    if (parts.Length >= 3)
                        applyBond = !string.Equals(parts[2], "nobond", StringComparison.OrdinalIgnoreCase);
                }
                if (string.IsNullOrEmpty(coatingId))
                    coatingId = "ebpvd_tbc_yttria_stabilized_zirconia";

                SetupInventory();
                string jobId = $"ebpvd_{_simDay}_{coatingId}";
                bool ok = _ebPvdCoating.StartCoatingJob(
                    jobId, coatingId, substrateTag, ResolvePlans146OperatorId(),
                    _simDay > 0 ? _simDay : 1, applyBond, TryConsumePlans146Demands, out string reason);
                _ebPvdCoatingPanel?.ShowFeedback(
                    ok ? $"Coating started: {coatingId} on {substrateTag}."
                       : $"Cannot start coating: {reason}",
                    !ok);
            }
            else if (string.Equals(action, "install_coated_part", StringComparison.OrdinalIgnoreCase))
            {
                // Plans 146–149 MED: mint ≠ install. Consume inventory then
                // publish watts into PowerGrid under ebpvd_installed.
                string itemId = string.IsNullOrEmpty(param) ? string.Empty : param.Trim();
                if (string.IsNullOrEmpty(itemId))
                {
                    _ebPvdCoatingPanel?.ShowFeedback("Cannot install: missing_item.", true);
                }
                else if (string.IsNullOrEmpty(Ashfall.Core.Shelter.PowerGridSystem.ResolveCoatedPartFamily(itemId)))
                {
                    _ebPvdCoatingPanel?.ShowFeedback($"Cannot install: unsupported_coated_part ({itemId}).", true);
                }
                else
                {
                    SetupInventory();
                    SetupPowerGrid();
                    if (_inventory == null || !_inventory.Inventory.TryConsumeById(itemId, 1))
                    {
                        _ebPvdCoatingPanel?.ShowFeedback($"Cannot install: missing inventory for {itemId}.", true);
                    }
                    else
                    {
                        string installReason = "power_grid_unavailable";
                        bool installed = _powerGrid != null
                            && _powerGrid.TryInstallCoatedPart(itemId, out installReason);
                        if (!installed)
                        {
                            // Refund the consumed part if the grid rejected install.
                            _inventory.Inventory.AddById(itemId, 1);
                            _ebPvdCoatingPanel?.ShowFeedback(
                                $"Cannot install: {installReason}.", true);
                        }
                        else
                        {
                            float watts = Ashfall.Core.Shelter.PowerGridSystem.ResolveCoatedPartWatts(itemId);
                            _ebPvdCoatingPanel?.ShowFeedback(
                                $"Installed {itemId} into generator (+{watts:F0} W).", false);
                        }
                    }
                }
            }
            else if (string.Equals(action, "maintain", StringComparison.OrdinalIgnoreCase))
            {
                string maint = string.IsNullOrEmpty(param) ? "replace_filament" : param;
                _ebPvdCoating.PerformMaintenance(maint);
                _ebPvdCoatingPanel?.ShowFeedback($"Maintenance complete: {maint}.", false);
            }

            _ebPvdCoatingPanel?.RefreshView();
        }

        private void HandleMicrofluidicDiagnosticAction(string action, string param = "")
        {
            SetupMicrofluidicDiagnostic();
            EnsurePlans146To149Panels();
            if (_microfluidicDiagnostic == null) return;

            if (string.Equals(action, "CLOSE", StringComparison.OrdinalIgnoreCase))
            {
                if (_microfluidicDiagnosticPanel != null) _microfluidicDiagnosticPanel.Visible = false;
                return;
            }

            if (string.Equals(action, "OPEN", StringComparison.OrdinalIgnoreCase))
            {
                if (_microfluidicDiagnosticPanel != null)
                {
                    SyncMicrofluidicPatientCandidates();
                    _microfluidicDiagnosticPanel.Visible = true;
                    _microfluidicDiagnosticPanel.RefreshView();
                }
                return;
            }

            if (string.Equals(action, "start_manufacture", StringComparison.OrdinalIgnoreCase))
            {
                string assayId = string.IsNullOrEmpty(param) ? "microfluidic_assay_cholera" : param;
                SetupInventory();
                string jobId = $"mfg_{_simDay}_{assayId}";
                bool ok = _microfluidicDiagnostic.StartManufacturing(
                    jobId, assayId, ResolvePlans146OperatorId(), TryConsumePlans146Demands, out string reason);
                _microfluidicDiagnosticPanel?.ShowFeedback(
                    ok ? $"Cartridge casting started for {assayId}."
                       : $"Cannot manufacture cartridge: {reason}",
                    !ok);
            }
            else if (string.Equals(action, "select_patient", StringComparison.OrdinalIgnoreCase))
            {
                SyncMicrofluidicPatientCandidates();
                if (_microfluidicDiagnosticPanel != null && !string.IsNullOrEmpty(param))
                {
                    _microfluidicDiagnosticPanel.SelectPatient(param);
                    _microfluidicDiagnosticPanel.ShowFeedback($"Patient selected: {param}.", false);
                }
            }
            else if (string.Equals(action, "start_run", StringComparison.OrdinalIgnoreCase))
            {
                // param: assayId|patientId  (patient defaults to first living survivor)
                SyncMicrofluidicPatientCandidates();
                string assayId = "microfluidic_assay_cholera";
                string patientId = ResolvePlans146OperatorId();
                if (!string.IsNullOrEmpty(param))
                {
                    var parts = param.Split('|');
                    if (parts.Length >= 1 && !string.IsNullOrEmpty(parts[0])) assayId = parts[0];
                    if (parts.Length >= 2 && !string.IsNullOrEmpty(parts[1])) patientId = parts[1];
                }
                SetupInventory();
                string runId = $"run_{_simDay}_{assayId}_{patientId}";
                bool ok = _microfluidicDiagnostic.StartRun(
                    runId, patientId, assayId, ResolvePlans146OperatorId(),
                    _simDay > 0 ? _simDay : 1, 0f,
                    (itemId, qty) => _inventory != null && _inventory.Inventory.TryConsumeById(itemId, qty),
                    out string reason);
                _microfluidicDiagnosticPanel?.ShowFeedback(
                    ok ? $"Assay run started: {assayId} for {patientId}."
                       : $"Cannot start assay: {reason}",
                    !ok);
            }
            else if (string.Equals(action, "maintain", StringComparison.OrdinalIgnoreCase))
            {
                string maint = string.IsNullOrEmpty(param) ? "recalibrate_optics" : param;
                _microfluidicDiagnostic.PerformMaintenance(maint);
                _microfluidicDiagnosticPanel?.ShowFeedback($"Analyzer serviced: {maint}.", false);
            }

            _microfluidicDiagnosticPanel?.RefreshView();
        }

        private void HandleMineFlailAction(string action, string param = "")
        {
            SetupMineClearingFlail();
            EnsurePlans146To149Panels();
            if (_mineClearingFlail == null) return;
            if (_mineClearingFlail.Routes == null)
                _mineClearingFlail.Routes = _routeInfrastructure;

            if (string.Equals(action, "CLOSE", StringComparison.OrdinalIgnoreCase))
            {
                if (_mineFlailPanel != null) _mineFlailPanel.Visible = false;
                return;
            }

            if (string.Equals(action, "OPEN", StringComparison.OrdinalIgnoreCase))
            {
                if (_mineFlailPanel != null)
                {
                    _mineFlailPanel.Visible = true;
                    _mineFlailPanel.RefreshView();
                }
                return;
            }

            if (string.Equals(action, "start_breach", StringComparison.OrdinalIgnoreCase))
            {
                SetupRouteInfrastructure();
                string routeId = "expedition_corridor_north";
                string segmentId = "seg_mine_gap";
                if (TrySplitPlans146Param(param, out string left, out string right))
                {
                    routeId = left;
                    segmentId = right;
                }
                const float defaultLengthMeters = 1200f;
                bool ok = _mineClearingFlail.StartBreach(
                    routeId, segmentId, ResolvePlans146OperatorId(),
                    _simDay > 0 ? _simDay : 1, defaultLengthMeters, _routeInfrastructure!, out string reason);
                _mineFlailPanel?.ShowFeedback(
                    ok ? $"Breach started on {routeId}:{segmentId}."
                       : $"Cannot start breach: {reason}",
                    !ok);
            }
            else if (string.Equals(action, "maintain", StringComparison.OrdinalIgnoreCase))
            {
                string maint = string.IsNullOrEmpty(param) ? "replace_chains" : param;
                _mineClearingFlail.PerformMaintenance(maint);
                _mineFlailPanel?.ShowFeedback($"Flail serviced: {maint}.", false);
            }

            _mineFlailPanel?.RefreshView();
        }

        private void HandleRailGrindingAction(string action, string param = "")
        {
            SetupRailGrinding();
            EnsurePlans146To149Panels();
            if (_railGrinding == null) return;
            if (_railGrinding.Routes == null)
                _railGrinding.Routes = _routeInfrastructure;

            if (string.Equals(action, "CLOSE", StringComparison.OrdinalIgnoreCase))
            {
                if (_railGrindingPanel != null) _railGrindingPanel.Visible = false;
                return;
            }

            if (string.Equals(action, "OPEN", StringComparison.OrdinalIgnoreCase))
            {
                if (_railGrindingPanel != null)
                {
                    _railGrindingPanel.Visible = true;
                    _railGrindingPanel.RefreshView();
                }
                return;
            }

            if (string.Equals(action, "start_grind", StringComparison.OrdinalIgnoreCase))
            {
                SetupRouteInfrastructure();
                string routeId = "rail_trunk_iron_vein";
                string segmentId = "sector_deep_quarry";
                if (TrySplitPlans146Param(param, out string left, out string right))
                {
                    routeId = left;
                    segmentId = right;
                }
                const float defaultLengthKm = 8f;
                bool ok = _railGrinding.StartGrindingJob(
                    routeId, segmentId, ResolvePlans146OperatorId(),
                    defaultLengthKm, _routeInfrastructure!, out string reason);
                _railGrindingPanel?.ShowFeedback(
                    ok ? $"Grinding started on {routeId}:{segmentId}."
                       : $"Cannot start grinding: {reason}",
                    !ok);
            }
            else if (string.Equals(action, "maintain", StringComparison.OrdinalIgnoreCase))
            {
                string maint = string.IsNullOrEmpty(param) ? "replace_stones" : param;
                _railGrinding.PerformMaintenance(maint);
                _railGrindingPanel?.ShowFeedback($"Grinder serviced: {maint}.", false);
            }

            _railGrindingPanel?.RefreshView();
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
