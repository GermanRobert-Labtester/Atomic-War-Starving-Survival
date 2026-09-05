using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Medical;
using Ashfall.Core.Radiation;
using Ashfall.Core.Shelter;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.Survivors;
using Ashfall.Core.YearOfAsh;
using Ashfall.Core.World;
using Ashfall.Core.Crafting;
using Ashfall.Core.Journal;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Waystation;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private WaterTreatmentHostSession _waterTreatment = null!;
        private WaterTreatmentPanel _waterTreatmentPanel = null!;
        private AirlockSecurityHostSession _airlockSecurity = null!;
        private AirlockSecurityPanel _airlockSecurityPanel = null!;
        private bool _airlockSecurityDirty;
        private ShelterThermalHostSession _shelterThermal = null!;
        private ShelterThermalPanel _shelterThermalPanel = null!;
        private bool _shelterThermalDirty;
        private Ashfall.Core.VentilationSystem _ventilation = null!; // Plan 29 29B: machine tell readings
        private VentilationHostSession? _ventilationHost;                    // Plan 72 stage console session
        private Ashfall.Core.Shelter.ShelterFireHazardSystem? _stageFireHazard; // Plan 72 arc-fault fire handoff
        private ShelterScheduleHostSession _shelterSchedule = null!;
        private ShelterSchedulePanel _shelterSchedulePanel = null!;
        private bool _shelterScheduleDirty;
        private AutopsyHostSession _autopsy = null!;
        private AutopsyReportPanel _autopsyReportPanel = null!;
        private bool _autopsyDirty;
        private WaystationHostSession _waystation = null!;
        private WaystationNetworkPanel _waystationPanel = null!;
        private bool _waystationDirty;

        // Plan 29 Task 29A — shelter room identity overlay (read-only data projection,
        // loaded once; no condition state, no save section of its own).
        private ShelterRoomIdentityCatalog? _shelterRoomIdentity;

        /// <summary>Lazy-load the room identity catalog from the data authority. Missing file → empty catalog (overlay, never a dependency).</summary>
        private ShelterRoomIdentityCatalog? GetShelterRoomIdentityCatalog()
        {
            if (_shelterRoomIdentity != null) return _shelterRoomIdentity;
            _shelterRoomIdentity = ShelterRoomIdentityCatalog.Load(
                new FileSystemIO(), new SystemTextJsonSerializer(), _dataDir);
            return _shelterRoomIdentity;
        }

        // Plan 29 Task 29B — machine tell catalog (read-only data projection, loaded once).
        private Ashfall.Core.Shelter.ShelterMachineTellCatalog? _machineTellCatalog;

        private Ashfall.Core.Shelter.ShelterMachineTellCatalog GetMachineTellCatalog()
        {
            if (_machineTellCatalog != null) return _machineTellCatalog;
            _machineTellCatalog = Ashfall.Core.Shelter.ShelterMachineTellCatalog.Load(
                new FileSystemIO(), new SystemTextJsonSerializer(), _dataDir);
            return _machineTellCatalog;
        }

        /// <summary>
        /// Plan 29 29A: a shelter room hotspot was clicked — treat it as inspection.
        /// Marks the authoritative Day-1 roster inspection (legacy ids tolerated via
        /// the catalog alias map) and unlocks inspect_room vignettes through the
        /// JournalSystem knowledge key (journal save owns persistence; old saves
        /// simply default locked and unlock on the next inspection).
        /// </summary>
        private void HandleShelterRoomSelected(string roomId)
        {
            if (string.IsNullOrEmpty(roomId)) return;
            var catalog = GetShelterRoomIdentityCatalog();
            string canonical = catalog?.ResolveRoomId(roomId) ?? roomId;

            if (_startingLevel != null && !_startingLevel.System.InspectRoom(canonical))
            {
                var aliases = catalog?.GetLegacyAliases(canonical);
                if (aliases != null)
                {
                    for (int i = 0; i < aliases.Count; i++)
                        if (_startingLevel.System.InspectRoom(aliases[i])) break;
                }
            }

            UnlockRoomHistories(catalog,
                catalog?.GetUnlockableVignettes(canonical,
                    ShelterRoomIdentityCatalog.RoomHistoryTrigger.RoomInspected));
        }

        /// <summary>
        /// Plan 29 29A: a real repair/maintenance action completed in a shelter room
        /// (filter service/replace). Raises the repair_performed unlock path only —
        /// authored vignettes never fire from a decorative interaction.
        /// </summary>
        private void HandleShelterRoomRepairPerformed(string roomId)
        {
            if (string.IsNullOrEmpty(roomId)) return;
            var catalog = GetShelterRoomIdentityCatalog();
            UnlockRoomHistories(catalog,
                catalog?.GetUnlockableVignettes(catalog.ResolveRoomId(roomId),
                    ShelterRoomIdentityCatalog.RoomHistoryTrigger.RepairPerformed));
        }

        /// <summary>
        /// Plan 29 29A: daily milestone pass. Runs once per campaign day from the
        /// day coordinator (never per frame) and unlocks at most the vignettes whose
        /// required day has been reached. Journal keys make it idempotent, so a late
        /// load of an older save catches up once rather than spamming every tick.
        /// </summary>
        private void TickShelterRoomHistoryMilestones(int day)
        {
            var catalog = GetShelterRoomIdentityCatalog();
            if (catalog == null) return;
            UnlockRoomHistories(catalog, catalog.GetDayMilestoneVignettes(day));

            // Plan 29 29B: daily machine glitch pass — journal one-shots, evaluate continuous.
            TickMachineGlitchEvents(day);

            // Plan 29 §29B.21: machine tell audio — quirk cues start on threshold
            // crossings and stop on recovery; personality beds sustain.
            TickMachineTellAudio();
        }

        /// <summary>
        /// Plan 29 §29B.21 consumer side: daily machine tell audio sync. Evaluates
        /// the same readings the text tells use and diffs the fired quirks against
        /// the live audio conditions — newly degraded tells start their ElevenLabs
        /// cue, recovered tells stop it, personality beds stay continuous. The
        /// condition system's already_active guard makes repeated applies no-ops,
        /// so audio fires on threshold transitions (§14), never per frame. No new
        /// state authority: tells re-derive from the owning systems' live condition.
        /// </summary>
        private void TickMachineTellAudio()
        {
            var catalog = GetMachineTellCatalog();
            if (catalog == null || catalog.MachineCount == 0) return;

            var readings = BuildMachineReadings();
            if (readings == null) return;

            Ashfall.Core.Shelter.MachineTellAudioSync.Apply(
                catalog, readings, _audioConditions,
                cueId => AtomicWar.GodotApp.Audio.AudioCueCatalog.Resolve(cueId)?.Loop ?? false);
        }

        /// <summary>Apply an unlock batch through the journal (the single persistence authority).</summary>
        private void UnlockRoomHistories(ShelterRoomIdentityCatalog? catalog,
            System.Collections.Generic.IReadOnlyList<RoomHistoryVignette>? vignettes)
        {
            if (catalog == null || vignettes == null || _journal == null) return;
            for (int i = 0; i < vignettes.Count; i++)
            {
                if (_journal.UnlockRoomHistorySeen(vignettes[i].id))
                    _journalDirty = true;
            }
        }

        /// <summary>
        /// Plan 29 29B: daily machine glitch pass. Journals one-shot glitches (idempotent
        /// via journal keys) and evaluates continuous glitches for UI surfacing. Old saves
        /// default un-noted and reveal once; continuous events re-fire on their cooldown,
        /// paced by the caller's day bookkeeping.
        /// </summary>
        private void TickMachineGlitchEvents(int day)
        {
            var catalog = GetMachineTellCatalog();
            if (catalog == null || catalog.GlitchEvents.Count == 0 || _journal == null) return;

            var readings = BuildMachineReadings();
            if (readings == null) return;

            bool isNoted(string id) => _journal.IsGlitchNoted(id);
            for (int m = 0; m < catalog.MachineCount; m++)
            {
                string mid = catalog.Machines[m].id;
                var glitches = catalog.EvaluateGlitchEvents(mid, readings, isNoted);
                for (int g = 0; g < glitches.Count; g++)
                {
                    var gl = glitches[g];
                    if (string.Equals(gl.repeat_policy, "once", System.StringComparison.Ordinal))
                    {
                        _journal.UnlockGlitchNoted(gl.id);
                    }
                }
            }
        }

        /// <summary>Build MachineConditionReadings from live host systems for tell evaluation.</summary>
        private Ashfall.Core.Shelter.MachineConditionReadings? BuildMachineReadings()
        {
            try
            {
                return new Ashfall.Core.Shelter.MachineConditionReadings
                {
                    HepaFilterHealth = (float)Math.Clamp(_startingLevel?.System.State.airFilterHealthPercent ?? 100, 0, 100),
                    HepaRadon = (float)Math.Clamp(_startingLevel?.System.State.radonLevelBqm3 ?? 12, 0, 200),
                    PowerFuelUnits = (float)Math.Clamp(_powerGrid?.System.State.FuelUnits ?? 0, 0, 200),
                    PowerBatteryReserve = _powerGrid != null ? (_powerGrid.System.State.BatteryReserveWh / 4000f * 100f) : 100f,
                    VentilationFilterSaturation = (float)Math.Clamp(_ventilation?.FilterSaturation ?? 0, 0, 100),
                    WaterFilterIntegrity = (float)Math.Clamp(_waterTreatment?.System.FilterIntegrity ?? 100, 0, 100),
                    ThermalBoilerFuel = (float)Math.Clamp(_shelterThermal?.System.BoilerFuelLevel ?? 0, 0, 200),
                    AirlockIncidentActive = _airlockSecurity?.System.HasPendingIncident ?? false,
                    HazardWeather = _world?.Weather.Current is Ashfall.Core.WeatherKind.FalloutStorm or Ashfall.Core.WeatherKind.BlackRain or Ashfall.Core.WeatherKind.Ashfall
                };
            }
            catch
            {
                return null;
            }
        }

        /// <summary>Build a one-line dashboard tell string from live machine readings (§29B.9–29B.13).</summary>
        public string BuildMachineTellText(ISeededRng? rng = null)
        {
            try
            {
                var catalog = GetMachineTellCatalog();
                if (catalog == null || catalog.MachineCount == 0) return string.Empty;

                var readings = BuildMachineReadings();
                if (readings == null) return string.Empty;

                var fired = new System.Collections.Generic.List<string>();
                bool isNoted(string id) => _journal != null && _journal.IsGlitchNoted(id);
                for (int m = 0; m < catalog.MachineCount; m++)
                {
                    string mid = catalog.Machines[m].id;
                    string label = catalog.Machines[m].display_name;
                    if (string.IsNullOrWhiteSpace(label))
                    {
                        label = mid;
                        if (label.StartsWith("machine_", StringComparison.Ordinal))
                            label = label.Substring("machine_".Length);
                    }
                    // Shorten to a readable tag: "Main Generator & Battery Bank" → "Generator"
                    if (label.Contains("&", StringComparison.Ordinal))
                        label = label.Split('&')[0].Trim();
                    label = label.Replace("Filtration Stack", "HEPA").Replace("Exhaust Plant", "Ventilation").Replace("Brine Still", "Still").Replace("Shelter ", "").Replace("Airlock Machinery", "Airlock");
                    label = label.ToUpperInvariant();

                    var quirks = catalog.EvaluateQuirks(mid, readings);
                    for (int q = 0; q < quirks.Count; q++)
                    {
                        var qk = quirks[q];
                        if (string.Equals(qk.kind, "diagnostic", System.StringComparison.Ordinal))
                            fired.Add($"[{label}] {qk.text_cue}");
                    }

                    var glitches = catalog.EvaluateGlitchEvents(mid, readings, isNoted);
                    for (int g = 0; g < glitches.Count; g++)
                    {
                        var gl = glitches[g];
                        fired.Add($"[{label}] {gl.title}");
                        if (string.Equals(gl.repeat_policy, "once", System.StringComparison.Ordinal) && _journal != null)
                            _journal.UnlockGlitchNoted(gl.id);
                    }
                }

                if (fired.Count == 0) return "NOMINAL";
                return string.Join(" // ", fired);
            }
            catch
            {
                return string.Empty;
            }
        }


        private void SetupWaterTreatment()
        {
            if (_waterTreatment != null) return;
            SetupInventory();
            var wtState = WaterTreatmentSaveStore.TryLoad() ?? new WaterTreatmentState();
            var wtSys = new WaterTreatmentSystem(new GodotLog());
            wtSys.RestoreState(wtState);
            _waterTreatment = new WaterTreatmentHostSession(wtSys, _inventory);
            if (_waterTreatmentPanel != null && _waterTreatmentPanel.IsInsideTree())
                RemoveChild(_waterTreatmentPanel);
            _waterTreatmentPanel = new WaterTreatmentPanel();
            _waterTreatmentPanel.Bind(_waterTreatment);
            _waterTreatmentPanel.Visible = false;
            AddChild(_waterTreatmentPanel);
        }

        private void SaveWaterTreatment()
        {
            if (_waterTreatment != null)
                CaptureSection("water_treatment", WaterTreatmentSaveStore.TryCapturePersisted(_waterTreatment.System.CaptureState()));
        }

        private void SetupAirlockSecurity()
        {
            if (_airlockSecurity != null) return;
            var asState = AirlockSecuritySaveStore.TryLoad() ?? new AirlockSecurityState();
            var asSys = new AirlockSecuritySystem(new SeededRng(1986), new GodotLog());
            asSys.RestoreState(asState);
            _airlockSecurity = new AirlockSecurityHostSession(asSys);
            if (_airlockSecurityPanel != null && _airlockSecurityPanel.IsInsideTree())
                RemoveChild(_airlockSecurityPanel);
            _airlockSecurityPanel = new AirlockSecurityPanel();
            _airlockSecurityPanel.Bind(_airlockSecurity);
            _airlockSecurityPanel.Visible = false;
            AddChild(_airlockSecurityPanel);
        }

        private void SaveAirlockSecurity()
        {
            if (_airlockSecurity != null)
                CaptureSection("airlock_security", AirlockSecuritySaveStore.TryCapturePersisted(_airlockSecurity.System.CaptureState()));
        }

        private void SetupShelterThermal()
        {
            if (_shelterThermal != null) return;
            var stState = ShelterThermalSaveStore.TryLoad() ?? new ShelterThermalState();
            var stNeeds = _survivors.Needs;
            var stStarting = _startingLevel.System;
            var stDeepFreeze = new YearOfAshDeepFreezeSystem(new YearOfAshDeepFreezeState());
            var stSys = new ShelterThermalSystem(new SeededRng(1986), stNeeds, stStarting, stDeepFreeze, new GodotLog());
            stSys.RestoreState(stState);
            _shelterThermal = new ShelterThermalHostSession(stSys);
            if (_shelterThermalPanel != null && _shelterThermalPanel.IsInsideTree())
                RemoveChild(_shelterThermalPanel);
            _shelterThermalPanel = new ShelterThermalPanel();
            _shelterThermalPanel.Bind(_shelterThermal);
            _shelterThermalPanel.Visible = false;
            AddChild(_shelterThermalPanel);
        }

        private void SaveShelterThermal()
        {
            if (_shelterThermal != null)
                CaptureSection("shelter_thermal", ShelterThermalSaveStore.TryCapturePersisted(_shelterThermal.System.CaptureState()));
        }

        private void SetupShelterSchedule()
        {
            if (_shelterSchedule != null) return;
            var ssState = ShelterScheduleSaveStore.TryLoad() ?? new ShelterScheduleState();
            var ssPower = _powerGrid.System;
            var ssSys = new ShelterScheduleSystem(ssPower, new GodotLog());
            ssSys.RestoreState(ssState);
            _shelterSchedule = new ShelterScheduleHostSession(ssSys);
            _shelterSchedule.LoadCatalog(_dataDir);
            if (_shelterSchedulePanel != null && _shelterSchedulePanel.IsInsideTree())
                RemoveChild(_shelterSchedulePanel);
            _shelterSchedulePanel = new ShelterSchedulePanel();
            _shelterSchedulePanel.Bind(_shelterSchedule);
            _shelterSchedulePanel.Visible = false;
            AddChild(_shelterSchedulePanel);
        }

        private void SaveShelterSchedule()
        {
            if (_shelterSchedule != null)
                CaptureSection("shelter_schedule", ShelterScheduleSaveStore.TryCapturePersisted(_shelterSchedule.System.CaptureState()));
        }

        private void SetupAutopsy(ResearchSystem? sharedResearch = null)
        {
            if (_autopsy != null) return;
            sharedResearch ??= _sharedResearch;
            var auState = AutopsySaveStore.TryLoad() ?? new AutopsyState();
            var auInv = _inventory.Inventory;
            var auRad = _survivors.Radiation;
            var auStarting = _startingLevel.System;
            var auVent = new VentilationSystem(auStarting);
            _ventilation = auVent; // Plan 29 29B: expose for machine tell readings
            // Plan 72: electrostatic stage catalog + persistent arc-fire hazard.
            auVent.ApplyElectrostaticCatalog(Ashfall.Core.ElectrostaticFiltrationCatalogLoader.Load(
                _dataDir, new FileSystemIO(), new SystemTextJsonSerializer()));
            _stageFireHazard = new Ashfall.Core.Shelter.ShelterFireHazardSystem();
            _ventilationHost = new VentilationHostSession(auVent);
            var auRes = sharedResearch;
            var auMedical = _medicalWard;
            var auSys = new AutopsySystem(new SeededRng(1986), auInv, auRad, auVent, auRes, auMedical, new GodotLog());
            auSys.RestoreState(auState);
            _autopsy = new AutopsyHostSession(auSys);
            _autopsy.LoadCatalog(_dataDir);
            if (_autopsyReportPanel != null && _autopsyReportPanel.IsInsideTree())
                RemoveChild(_autopsyReportPanel);
            _autopsyReportPanel = new AutopsyReportPanel();
            _autopsyReportPanel.Bind(_autopsy);
            _autopsyReportPanel.Visible = false;
            AddChild(_autopsyReportPanel);
        }

        private void SaveAutopsy()
        {
            if (_autopsy != null)
                CaptureSection("autopsy", AutopsySaveStore.TryCapturePersisted(_autopsy.System.CaptureState()));
        }

        private void SetupWaystation()
        {
            if (_waystation != null) return;
            var wsState = WaystationSaveStore.TryLoad() ?? new WaystationSystemState();
            var wsSys = new WaystationSystem();
            wsSys.RestoreState(wsState);
            _waystation = new WaystationHostSession(wsSys);

            // Plan 56 phase 6 — the multi-node trade-stock network: its 7-day
            // resupply is provenance-aware (locally produced + general stock
            // survive a market shortage; pure imports lapse). The shortage
            // policy reads the live market; the closure is null-safe because
            // the economy session may not be set up yet when it is bound.
            SetupEconomy();
            var network = new WaystationNetworkSystem();
            _waystation.AttachNetwork(
                network,
                _economy.Catalog,
                () => _economy?.Market.IsSuppliesShort() ?? false);
            if (_waystationPanel != null && _waystationPanel.IsInsideTree())
                RemoveChild(_waystationPanel);
            _waystationPanel = new WaystationNetworkPanel();
            _waystationPanel.Bind(_waystation);
            _waystationPanel.Visible = false;
            AddChild(_waystationPanel);
        }

        private void SaveWaystation()
        {
            if (_waystation != null)
                CaptureSection("waystation", WaystationSaveStore.TryCapturePersisted(_waystation.System.CaptureState()));
        }
    }
}
