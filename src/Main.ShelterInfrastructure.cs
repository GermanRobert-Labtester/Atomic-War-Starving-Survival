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
        private ShelterScheduleHostSession _shelterSchedule = null!;
        private ShelterSchedulePanel _shelterSchedulePanel = null!;
        private bool _shelterScheduleDirty;
        private AutopsyHostSession _autopsy = null!;
        private AutopsyReportPanel _autopsyReportPanel = null!;
        private bool _autopsyDirty;
        private WaystationHostSession _waystation = null!;
        private WaystationNetworkPanel _waystationPanel = null!;
        private bool _waystationDirty;


        private void SetupWaterTreatment()
        {
            if (_waterTreatment != null) return;
            var wtState = WaterTreatmentSaveStore.TryLoad() ?? new WaterTreatmentState();
            var wtSys = new WaterTreatmentSystem(new GodotLog());
            wtSys.RestoreState(wtState);
            _waterTreatment = new WaterTreatmentHostSession(wtSys);
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

        private void SetupAutopsy(ResearchSystem sharedResearch)
        {
            if (_autopsy != null) return;
            var auState = AutopsySaveStore.TryLoad() ?? new AutopsyState();
            var auInv = _inventory.Inventory;
            var auRad = _survivors.Radiation;
            var auStarting = _startingLevel.System;
            var auVent = new VentilationSystem(auStarting);
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
