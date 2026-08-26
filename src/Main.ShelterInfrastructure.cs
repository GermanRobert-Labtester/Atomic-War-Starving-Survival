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
            var wtState = WaterTreatmentSaveStore.TryLoad() ?? new WaterTreatmentState();
            var wtSys = new WaterTreatmentSystem(new GodotLog());
            wtSys.RestoreState(wtState);
            _waterTreatment = new WaterTreatmentHostSession(wtSys);
            _waterTreatment.StateChanged += () => _waterTreatment.MarkDirty();
            _waterTreatmentPanel = new WaterTreatmentPanel();
            _waterTreatmentPanel.Bind(_waterTreatment);
            _waterTreatmentPanel.Visible = false;
            AddChild(_waterTreatmentPanel);
        }

        private void SaveWaterTreatment()
        {
            _waterTreatment?.Save();
        }

        private void SetupAirlockSecurity()
        {
            var asState = AirlockSecuritySaveStore.TryLoad() ?? new AirlockSecurityState();
            var asSys = new AirlockSecuritySystem(new SeededRng(1986), new GodotLog());
            asSys.RestoreState(asState);
            _airlockSecurity = new AirlockSecurityHostSession(asSys);
            _airlockSecurity.StateChanged += () => _airlockSecurity.MarkDirty();
            _airlockSecurityPanel = new AirlockSecurityPanel();
            _airlockSecurityPanel.Bind(_airlockSecurity);
            _airlockSecurityPanel.Visible = false;
            AddChild(_airlockSecurityPanel);
        }

        private void SaveAirlockSecurity()
        {
            _airlockSecurity?.Save();
        }

        private void SetupShelterThermal()
        {
            var stState = ShelterThermalSaveStore.TryLoad() ?? new ShelterThermalState();
            var stNeeds = _survivors.Needs;
            var stStarting = _startingLevel.System;
            var stDeepFreeze = new YearOfAshDeepFreezeSystem(new YearOfAshDeepFreezeState());
            var stSys = new ShelterThermalSystem(new SeededRng(1986), stNeeds, stStarting, stDeepFreeze, new GodotLog());
            stSys.RestoreState(stState);
            _shelterThermal = new ShelterThermalHostSession(stSys);
            _shelterThermal.StateChanged += () => _shelterThermal.MarkDirty();
            _shelterThermalPanel = new ShelterThermalPanel();
            _shelterThermalPanel.Bind(_shelterThermal);
            _shelterThermalPanel.Visible = false;
            AddChild(_shelterThermalPanel);
        }

        private void SaveShelterThermal()
        {
            _shelterThermal?.Save();
        }

        private void SetupShelterSchedule()
        {
            var ssState = ShelterScheduleSaveStore.TryLoad() ?? new ShelterScheduleState();
            var ssPower = _powerGrid.System;
            var ssSys = new ShelterScheduleSystem(ssPower, new GodotLog());
            ssSys.RestoreState(ssState);
            _shelterSchedule = new ShelterScheduleHostSession(ssSys);
            _shelterSchedule.LoadCatalog(_dataDir);
            _shelterSchedule.StateChanged += () => _shelterSchedule.MarkDirty();
            _shelterSchedulePanel = new ShelterSchedulePanel();
            _shelterSchedulePanel.Bind(_shelterSchedule);
            _shelterSchedulePanel.Visible = false;
            AddChild(_shelterSchedulePanel);
        }

        private void SaveShelterSchedule()
        {
            _shelterSchedule?.Save();
        }

        private void SetupAutopsy(ResearchSystem sharedResearch)
        {
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
            _autopsy.StateChanged += () => _autopsy.MarkDirty();
            _autopsyReportPanel = new AutopsyReportPanel();
            _autopsyReportPanel.Bind(_autopsy);
            _autopsyReportPanel.Visible = false;
            AddChild(_autopsyReportPanel);
        }

        private void SaveAutopsy()
        {
            _autopsy?.Save();
        }

        private void SetupWaystation()
        {
            var wsState = WaystationSaveStore.TryLoad() ?? new WaystationSystemState();
            var wsSys = new WaystationSystem();
            wsSys.RestoreState(wsState);
            _waystation = new WaystationHostSession(wsSys);
            _waystation.StateChanged += () => _waystation.MarkDirty();
            _waystationPanel = new WaystationNetworkPanel();
            _waystationPanel.Bind(_waystation);
            _waystationPanel.Visible = false;
            AddChild(_waystationPanel);
        }

        private void SaveWaystation()
        {
            _waystation?.Save();
        }
    }
}
