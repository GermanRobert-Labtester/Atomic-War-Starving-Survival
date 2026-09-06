using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Excavation;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Shelter
{
    [Serializable]
    public sealed class SeismicFaultDef
    {
        public string fault_id { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string depth_layer { get; set; } = "sub_surface";
        public float slip_threshold { get; set; } = 100f;
        public float base_accumulation_rate { get; set; } = 2.0f;
        public float depth_multiplier { get; set; } = 1.0f;
        public float strata_attenuation_factor { get; set; } = 0.3f;
        public float pipe_shear_probability { get; set; } = 0.5f;
        public float radiator_rupture_probability { get; set; } = 0.3f;
        public int methane_outgassing_ppm { get; set; } = 500;
        public List<string> affected_sectors { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class SeismicFaultCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<SeismicFaultDef> faults { get; set; } = new List<SeismicFaultDef>();
    }

    [Serializable]
    public sealed class FaultRuntimeState
    {
        public string faultId = string.Empty;
        public float currentTension;
        public int totalSlips;
        public int lastSlipDay = -1;
        public int temporaryShoringDays;
        public float temporaryShoringStrength;
    }

    [Serializable]
    public sealed class SeismicQuakeEvent
    {
        public int day;
        public string faultId = string.Empty;
        public float magnitude;
        public string depthLayer = string.Empty;
        public List<string> shearedPipes = new List<string>();
        public List<string> rupturedRadiators = new List<string>();
        public int outgassedMethanePpm;
        public string description = string.Empty;
    }

    [Serializable]
    public sealed class SeismicDynamicsSaveState
    {
        public int currentDay;
        public Dictionary<string, FaultRuntimeState> faults = new Dictionary<string, FaultRuntimeState>(StringComparer.Ordinal);
        public Dictionary<string, int> sectorReinforcementLevel = new Dictionary<string, int>(StringComparer.Ordinal);
        public bool seismographOperational = true;
        public List<SeismicQuakeEvent> recentQuakes = new List<SeismicQuakeEvent>();
        public List<string> activeEarlyWarnings = new List<string>();

        // Plan B68 — monitoring expansion. Legacy defaults keep old saves
        // safe: no geophone coverage, no dampeners (Plan 56 behavior).
        public List<string> geophoneSectors = new List<string>();
        public Dictionary<string, float> dampenerIntegrity = new Dictionary<string, float>(StringComparer.Ordinal);
    }

    public sealed partial class SeismicDynamicsSystem
    {
        public const string SystemId = "seismic_dynamics";
        public const string CatalogPath = "seismic_fault_catalog.json";

        private readonly ISeededRng _rng;
        private readonly ShelterThermalSystem? _thermalSystem;
        private readonly ExcavationHazardSystem? _hazardSystem;
        private readonly InventoryContainer? _inventory;
        private readonly ILog _log;

        private readonly Dictionary<string, SeismicFaultDef> _catalog = new Dictionary<string, SeismicFaultDef>(StringComparer.Ordinal);
        private SeismicDynamicsSaveState _state = new SeismicDynamicsSaveState();

        public IReadOnlyDictionary<string, SeismicFaultDef> Catalog => _catalog;
        public SeismicDynamicsSaveState State => _state;

        public event Action<SeismicQuakeEvent>? OnQuakeOccurred;
        public event Action<string, float>? OnEarlyWarning; // faultId, tensionPercent
        public event Action? OnSeismicStateChanged;

        public SeismicDynamicsSystem(
            ISeededRng rng,
            ShelterThermalSystem? thermalSystem = null,
            ExcavationHazardSystem? hazardSystem = null,
            InventoryContainer? inventory = null,
            ILog? log = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _thermalSystem = thermalSystem;
            _hazardSystem = hazardSystem;
            _inventory = inventory;
            _log = log ?? NullLog.Instance;

            RegisterDefaultFaults();
        }

        public void LoadCatalog(string jsonContent)
        {
            if (string.IsNullOrWhiteSpace(jsonContent)) return;
            try
            {
                var serializer = new SystemTextJsonSerializer();
                var cat = serializer.Deserialize<SeismicFaultCatalog>(jsonContent);
                if (cat?.faults != null)
                {
                    foreach (var f in cat.faults)
                    {
                        if (!string.IsNullOrEmpty(f.fault_id))
                        {
                            _catalog[f.fault_id] = f;
                            EnsureFaultState(f.fault_id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Warn($"[SeismicDynamics] Failed to parse fault catalog: {ex.Message}");
            }
        }

        private void RegisterDefaultFaults()
        {
            RegisterFault(new SeismicFaultDef
            {
                fault_id = "fault_sub_strata_rift",
                name = "Sub-Strata Tension Rift",
                description = "Shallow tectonic fracture beneath outer excavation zone.",
                depth_layer = "sub_surface",
                slip_threshold = 100f,
                base_accumulation_rate = 2.5f,
                depth_multiplier = 1.15f,
                strata_attenuation_factor = 0.35f,
                pipe_shear_probability = 0.60f,
                radiator_rupture_probability = 0.30f,
                methane_outgassing_ppm = 650,
                affected_sectors = new List<string> { "sector_excavation_alpha", "sector_excavation_beta" }
            });
            RegisterFault(new SeismicFaultDef
            {
                fault_id = "fault_basal_detachment",
                name = "Basal Bedrock Detachment Fault",
                description = "Deep continental boundary fracture under high confining pressure.",
                depth_layer = "deep_strata",
                slip_threshold = 250f,
                base_accumulation_rate = 1.8f,
                depth_multiplier = 1.45f,
                strata_attenuation_factor = 0.50f,
                pipe_shear_probability = 0.85f,
                radiator_rupture_probability = 0.70f,
                methane_outgassing_ppm = 1800,
                affected_sectors = new List<string> { "sector_excavation_gamma", "bunker_core" }
            });
            RegisterFault(new SeismicFaultDef
            {
                fault_id = "fault_orbital_crevasse",
                name = "Kinetic Impact Crevasse",
                description = "Fault plane formed along kinetic penetrator shockwave trajectory.",
                depth_layer = "crustal_breach",
                slip_threshold = 180f,
                base_accumulation_rate = 2.0f,
                depth_multiplier = 1.30f,
                strata_attenuation_factor = 0.25f,
                pipe_shear_probability = 0.75f,
                radiator_rupture_probability = 0.50f,
                methane_outgassing_ppm = 1200,
                affected_sectors = new List<string> { "sector_excavation_alpha", "bunker_core" }
            });
        }

        public void RegisterFault(SeismicFaultDef def)
        {
            if (def == null || string.IsNullOrEmpty(def.fault_id)) return;
            _catalog[def.fault_id] = def;
            EnsureFaultState(def.fault_id);
        }

        private FaultRuntimeState EnsureFaultState(string faultId)
        {
            if (!_state.faults.TryGetValue(faultId, out var state))
            {
                state = new FaultRuntimeState { faultId = faultId, currentTension = 0f };
                _state.faults[faultId] = state;
            }
            return state;
        }

        public void SetSeismographStatus(bool operational)
        {
            _state.seismographOperational = operational;
            OnSeismicStateChanged?.Invoke();
        }

        public ActionResult ApplyEmergencyShoring(string faultId, int durationDays = 5, InventoryContainer? inv = null)
        {
            if (!_catalog.ContainsKey(faultId))
                return ActionResult.Failed("unknown_fault", "seismic.unknown_fault");

            var invToUse = inv ?? _inventory;
            if (invToUse != null)
            {
                if (!invToUse.HasSufficient("item_scrap_metal", 10) && !invToUse.HasSufficient("scrap_mechanical", 10))
                    return ActionResult.Blocked("insufficient_materials", "seismic.insufficient_materials");

                if (invToUse.HasSufficient("item_scrap_metal", 10))
                    invToUse.TryConsume("item_scrap_metal", 10);
                else
                    invToUse.TryConsume("scrap_mechanical", 10);
            }

            var fState = EnsureFaultState(faultId);
            fState.temporaryShoringDays = Math.Max(fState.temporaryShoringDays, durationDays);
            fState.temporaryShoringStrength = 0.5f; // 50% tension damping
            fState.currentTension = Math.Max(0f, fState.currentTension - 15f);

            OnSeismicStateChanged?.Invoke();
            return ActionResult.Success("seismic.emergency_shoring_applied",
                new Dictionary<string, double> { { "duration", durationDays }, { "tension", fState.currentTension } });
        }

        public ActionResult ReinforceSector(string sectorId, InventoryContainer? inv = null)
        {
            if (string.IsNullOrEmpty(sectorId))
                return ActionResult.Failed("invalid_sector", "seismic.invalid_sector");

            int curLevel = 0;
            _state.sectorReinforcementLevel.TryGetValue(sectorId, out curLevel);
            if (curLevel >= 3)
                return ActionResult.Blocked("max_reinforcement_reached", "seismic.max_reinforcement");

            var invToUse = inv ?? _inventory;
            if (invToUse != null)
            {
                if (!invToUse.HasSufficient("item_scrap_metal", 20) && !invToUse.HasSufficient("scrap_mechanical", 20))
                    return ActionResult.Blocked("insufficient_materials", "seismic.insufficient_materials");

                if (invToUse.HasSufficient("item_scrap_metal", 20))
                    invToUse.TryConsume("item_scrap_metal", 20);
                else
                    invToUse.TryConsume("scrap_mechanical", 20);
            }

            curLevel++;
            _state.sectorReinforcementLevel[sectorId] = curLevel;
            OnSeismicStateChanged?.Invoke();
            return ActionResult.Success("seismic.sector_reinforced",
                new Dictionary<string, double> { { "level", curLevel } });
        }

        public void InjectKineticShock(float megajoules, string epicenterSector)
        {
            if (megajoules <= 0f) return;

            foreach (var kvp in _catalog)
            {
                var def = kvp.Value;
                var fState = EnsureFaultState(def.fault_id);

                bool isAffected = def.affected_sectors.Contains(epicenterSector);
                float shockTransfer = isAffected ? megajoules * 0.8f : megajoules * 0.25f;
                fState.currentTension += shockTransfer;

                if (fState.currentTension >= def.slip_threshold)
                {
                    TriggerFaultSlip(def, fState, _state.currentDay);
                }
            }
            OnSeismicStateChanged?.Invoke();
        }

        public void TickDay(int day)
        {
            _state.currentDay = day;
            _state.activeEarlyWarnings.Clear();
            _primaryWarningsToday.Clear();

            foreach (var kvp in _catalog)
            {
                var def = kvp.Value;
                var fState = EnsureFaultState(def.fault_id);

                // Handle temporary shoring expiry
                if (fState.temporaryShoringDays > 0)
                {
                    fState.temporaryShoringDays--;
                    if (fState.temporaryShoringDays <= 0)
                        fState.temporaryShoringStrength = 0f;
                }

                // Accumulate daily stress
                float dailyRate = def.base_accumulation_rate * def.depth_multiplier;
                if (fState.temporaryShoringStrength > 0f)
                    dailyRate *= (1f - fState.temporaryShoringStrength);

                fState.currentTension += dailyRate;

                // Check early warning
                float tensionRatio = fState.currentTension / Math.Max(1f, def.slip_threshold);
                float mainArrivalRatio = GetMainArrivalRatio(def.fault_id);
                if (_state.seismographOperational && tensionRatio >= mainArrivalRatio)
                {
                    _state.activeEarlyWarnings.Add(def.fault_id);
                    OnEarlyWarning?.Invoke(def.fault_id, tensionRatio);
                }

                // B68: primary-wave detection gives lead time before the
                // stronger main-arrival warning above.
                CheckPrimaryWave(def, fState, tensionRatio);

                // Check threshold slip
                if (fState.currentTension >= def.slip_threshold)
                {
                    TriggerFaultSlip(def, fState, day);
                }
            }

            OnSeismicStateChanged?.Invoke();
        }

        private void TriggerFaultSlip(SeismicFaultDef def, FaultRuntimeState fState, int day)
        {
            float ratio = fState.currentTension / Math.Max(1f, def.slip_threshold);
            float magnitude = 3.0f + Math.Min(3.5f, ratio * 2.0f);

            // Release 85% of accumulated tension
            fState.currentTension *= 0.15f;
            fState.totalSlips++;
            fState.lastSlipDay = day;

            var quake = new SeismicQuakeEvent
            {
                day = day,
                faultId = def.fault_id,
                magnitude = magnitude,
                depthLayer = def.depth_layer,
                description = $"Tectonic fracture slip along {def.name} (Magnitude {magnitude:F1})"
            };

            // Calculate sector reinforcement damping across affected sectors
            float avgDamping = 0f;
            if (def.affected_sectors.Count > 0)
            {
                int totalLevels = 0;
                foreach (var sec in def.affected_sectors)
                {
                    if (_state.sectorReinforcementLevel.TryGetValue(sec, out int lvl))
                        totalLevels += lvl;
                }
                avgDamping = (totalLevels / (float)def.affected_sectors.Count) * 0.20f; // 20% damping per level
            }

            // B68: hydraulic dampeners add up to 25% damping, proportional
            // to their remaining integrity, and wear on every slip.
            float dampenerDamping = ApplyDampenerDampingAndWear(def, magnitude);
            avgDamping += dampenerDamping;
            float effectiveSeverity = Math.Clamp((1f - def.strata_attenuation_factor - avgDamping) * (magnitude / 5f), 0.1f, 1.0f);

            // B68: rockburst class — a request the excavation authority owns.
            // This system never applies tunnel or site damage itself.
            if (effectiveSeverity >= 0.6f)
            {
                var rockburst = new RockburstRequest
                {
                    day = day,
                    faultId = def.fault_id,
                    sectors = new List<string>(def.affected_sectors),
                    severity = effectiveSeverity
                };
                _log.Warn($"[SeismicDynamics] Rockburst requested along {def.name} (severity {effectiveSeverity:F2})");
                OnRockburstRequested?.Invoke(rockburst);
            }

            // Utility Damage Routing: Pipes & Radiators via ShelterThermalSystem
            if (_thermalSystem != null)
            {
                // Damage pipes
                if (_rng.NextDouble() < def.pipe_shear_probability)
                {
                    var pipes = _thermalSystem.State.pipes;
                    if (pipes.Count > 0)
                    {
                        int targetIdx = _rng.Next(0, pipes.Count);
                        var targetPipe = pipes[targetIdx];
                        _thermalSystem.ShearPipe(targetPipe.pipeId, effectiveSeverity);
                        quake.shearedPipes.Add(targetPipe.pipeId);
                    }
                }

                // Damage radiator loops
                if (_rng.NextDouble() < def.radiator_rupture_probability)
                {
                    var radRooms = _thermalSystem.State.rooms.FindAll(r => r.hasRadiator);
                    if (radRooms.Count > 0)
                    {
                        int targetIdx = _rng.Next(0, radRooms.Count);
                        var targetRoom = radRooms[targetIdx];
                        _thermalSystem.ShearRadiatorLoop(targetRoom.roomId);
                        quake.rupturedRadiators.Add(targetRoom.roomId);
                    }
                }
            }

            // Hazard Routing: Gas outgassing via ExcavationHazardSystem
            if (_hazardSystem != null && def.methane_outgassing_ppm > 0)
            {
                int gasPpm = (int)(def.methane_outgassing_ppm * effectiveSeverity);
                quake.outgassedMethanePpm = gasPpm;
                foreach (var sec in def.affected_sectors)
                {
                    var sectorState = _hazardSystem.GetOrCreateSector(sec);
                    sectorState.MethanePpm = Math.Min(5000, sectorState.MethanePpm + gasPpm);
                    sectorState.ShoringHealthPermille = Math.Max(0, sectorState.ShoringHealthPermille - (int)(effectiveSeverity * 250));
                }
            }

            _state.recentQuakes.Add(quake);
            if (_state.recentQuakes.Count > 20)
                _state.recentQuakes.RemoveAt(0);

            _log.Warn($"[SeismicDynamics] {quake.description} -> Sheared pipes: {quake.shearedPipes.Count}, Radiators: {quake.rupturedRadiators.Count}, Gas: {quake.outgassedMethanePpm}ppm");
            OnQuakeOccurred?.Invoke(quake);
        }

        public SeismicDynamicsSaveState CaptureState() => CloneState(_state);

        public void RestoreState(SeismicDynamicsSaveState saved)
        {
            if (saved == null) return;
            _state = CloneState(saved);
        }

        private static SeismicDynamicsSaveState CloneState(SeismicDynamicsSaveState src)
        {
            if (src == null) return new SeismicDynamicsSaveState();
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(src);
            return s.Deserialize<SeismicDynamicsSaveState>(json) ?? new SeismicDynamicsSaveState();
        }
    }
}
