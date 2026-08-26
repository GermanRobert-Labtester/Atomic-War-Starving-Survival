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
        private SurvivorRelationsHostSession _survivorRelations = null!;
        private SurvivorRelationsPanel _survivorRelationsPanel = null!;
        private bool _survivorRelationsDirty;
        private RegionalTreatyHostSession _regionalTreaty = null!;
        private RegionalTreatyPanel _regionalTreatyPanel = null!;
        private bool _regionalTreatyDirty;
        private VinylMoraleHostSession _vinylMorale = null!;
        private VinylMoralePanel _vinylMoralePanel = null!;
        private bool _vinylMoraleDirty;
        private WildlifeTrappingHostSession _wildlifeTrapping = null!;
        private WildlifeTrappingPanel _wildlifeTrappingPanel = null!;
        private bool _wildlifeTrappingDirty;
        private ExcavationHostSession _excavation = null!;
        private ExcavationPanel _excavationPanel = null!;
        private bool _excavationDirty;
        private ApprenticeshipHostSession _apprenticeship = null!;
        private ApprenticeshipPanel _apprenticeshipPanel = null!;
        private bool _apprenticeshipDirty;
        private CaregivingHostSession _caregiving = null!;
        private CaregivingPanel _caregivingPanel = null!;
        private bool _caregivingDirty;

        private void SetupSurvivorRelations()
        {
            var srState = SurvivorRelationsSaveStore.TryLoad() ?? new SurvivorRelationsState();
            var srSys = new SurvivorRelationsSystem(new SeededRng(1986), new GodotLog());
            _survivorRelationsCore = srSys;
            srSys.RestoreState(srState);
            _survivorRelations = new SurvivorRelationsHostSession(srSys);
            _survivorRelations.StateChanged += () => _survivorRelations.MarkDirty();
            _survivorRelationsPanel = new SurvivorRelationsPanel();
            _survivorRelationsPanel.Bind(_survivorRelations);
            _survivorRelationsPanel.Visible = false;
            AddChild(_survivorRelationsPanel);
        }

        private void SaveSurvivorRelations()
        {
            _survivorRelations?.Save();
        }

        private void SetupRegionalTreaty()
        {
            var rtState = RegionalTreatySaveStore.TryLoad() ?? new RegionalTreatyState();
            var rtSys = new RegionalTreatySystem(new GodotLog());
            rtSys.RestoreState(rtState);
            _regionalTreaty = new RegionalTreatyHostSession(rtSys);
            _regionalTreaty.StateChanged += () => _regionalTreaty.MarkDirty();
            _regionalTreatyPanel = new RegionalTreatyPanel();
            _regionalTreatyPanel.Bind(_regionalTreaty);
            _regionalTreatyPanel.Visible = false;
            AddChild(_regionalTreatyPanel);
        }

        private void SaveRegionalTreaty()
        {
            _regionalTreaty?.Save();
        }

        private void SetupVinylMorale()
        {
            var vmState = VinylMoraleSaveStore.TryLoad() ?? new VinylMoraleState();
            var vmSys = new VinylMoraleSystem(new GodotLog());
            vmSys.RestoreState(vmState);
            _vinylMorale = new VinylMoraleHostSession(vmSys);
            _vinylMorale.StateChanged += () => _vinylMorale.MarkDirty();
            _vinylMoralePanel = new VinylMoralePanel();
            _vinylMoralePanel.Bind(_vinylMorale);
            _vinylMoralePanel.Visible = false;
            AddChild(_vinylMoralePanel);
        }

        private void SaveVinylMorale()
        {
            _vinylMorale?.Save();
        }

        private void SetupWildlifeTrapping()
        {
            var wtrapState = WildlifeTrappingSaveStore.TryLoad() ?? new WildlifeTrappingState();
            var wtrapSys = new WildlifeTrappingSystem(new SeededRng(1986), new GodotLog());
            wtrapSys.RestoreState(wtrapState);
            _wildlifeTrapping = new WildlifeTrappingHostSession(wtrapSys);
            _wildlifeTrapping.StateChanged += () => _wildlifeTrapping.MarkDirty();
            _wildlifeTrappingPanel = new WildlifeTrappingPanel();
            _wildlifeTrappingPanel.Bind(_wildlifeTrapping);
            _wildlifeTrappingPanel.Visible = false;
            AddChild(_wildlifeTrappingPanel);
        }

        private void SaveWildlifeTrapping()
        {
            _wildlifeTrapping?.Save();
        }

        private void SetupExcavation()
        {
            var exState = ExcavationSaveStore.TryLoad() ?? new ExcavationState();
            var exSys = new ExcavationSystem(new SeededRng(1986), new GodotLog());
            exSys.RestoreState(exState);
            _excavation = new ExcavationHostSession(exSys);
            _excavation.StateChanged += () => _excavation.MarkDirty();
            _excavationPanel = new ExcavationPanel();
            _excavationPanel.Bind(_excavation);
            _excavationPanel.Visible = false;
            AddChild(_excavationPanel);
        }

        private void SaveExcavation()
        {
            _excavation?.Save();
        }

        private void SetupApprenticeship()
        {
            var appState = ApprenticeshipSaveStore.TryLoad() ?? new ApprenticeshipState();
            var appSkills = new SkillProgressionSystem();
            var appSys = new ApprenticeshipSystem(new SeededRng(1986), appSkills, _expandedShelterRoster, _survivorRelationsCore, new GodotLog());
            appSys.RestoreState(appState);
            _apprenticeship = new ApprenticeshipHostSession(appSys);
            _apprenticeship.StateChanged += () => _apprenticeship.MarkDirty();
            _apprenticeshipPanel = new ApprenticeshipPanel();
            _apprenticeshipPanel.Bind(_apprenticeship);
            _apprenticeshipPanel.Visible = false;
            AddChild(_apprenticeshipPanel);
        }

        private void SaveApprenticeship()
        {
            _apprenticeship?.Save();
        }

        private void SetupCaregiving()
        {
            var cgState = CaregivingSaveStore.TryLoad() ?? new CaregivingSaveState();
            var cgSys = new CaregivingSystem();
            cgSys.RestoreState(cgState);
            _caregiving = new CaregivingHostSession(cgSys);
            _caregiving.StateChanged += () => _caregiving.MarkDirty();
            _caregivingPanel = new CaregivingPanel();
            _caregivingPanel.Bind(_caregiving);
            _caregivingPanel.Visible = false;
            AddChild(_caregivingPanel);
        }

        private void SaveCaregiving()
        {
            _caregiving?.Save();
        }
    }
}
