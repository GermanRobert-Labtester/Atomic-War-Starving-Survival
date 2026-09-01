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
using Ashfall.Core.Narrative;
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
            if (_survivorRelations != null) return;
            SetupCampaignDay();
            var srState = SurvivorRelationsSaveStore.TryLoad() ?? new SurvivorRelationsState();
            var srSys = new SurvivorRelationsSystem(_campaignDay.Rng.GetStream(Ashfall.Core.Random.CampaignStreamIds.Social).Rng, new GodotLog());
            _survivorRelationsCore = srSys;
            srSys.RestoreState(srState);
            _survivorRelations = new SurvivorRelationsHostSession(srSys);
            if (_survivorRelationsPanel != null && _survivorRelationsPanel.IsInsideTree())
                RemoveChild(_survivorRelationsPanel);
            _survivorRelationsPanel = new SurvivorRelationsPanel();
            _survivorRelationsPanel.Bind(_survivorRelations);
            _survivorRelationsPanel.Visible = false;
            AddChild(_survivorRelationsPanel);
        }

        private void SaveSurvivorRelations()
        {
            if (_survivorRelations != null)
                CaptureSection("survivor_relations", SurvivorRelationsSaveStore.TryCapturePersisted(_survivorRelations.System.CaptureState()));
        }

        private void SetupRegionalTreaty()
        {
            if (_regionalTreaty != null) return;
            var rtState = RegionalTreatySaveStore.TryLoad() ?? new RegionalTreatyState();
            var rtSys = new RegionalTreatySystem(new GodotLog());
            rtSys.RestoreState(rtState);
            // Plan 25 (25G.7): feed the canonical narrative treaty corpus into the
            // mechanical system — until now the host never called LoadCatalog, so
            // Propose/Ratify had nothing to act on in production.
            if (!string.IsNullOrEmpty(_dataDir))
            {
                var fileIO = CatalogPath.CreateFileIOForDataDir(_dataDir);
                var json = new SystemTextJsonSerializer();
                string path = fileIO.Combine(_dataDir, "narrative/regional_treaty_protocols.json");
                if (fileIO.FileExists(path))
                {
                    var catalog = new Ashfall.Core.Narrative.RegionalTreatyCatalog();
                    catalog.Load(fileIO.ReadAllText(path), json);
                    rtSys.LoadCatalog(
                        Ashfall.Core.RegionalTreatyFeed.Map(catalog.AllTreaties));
                }
            }
            _regionalTreaty = new RegionalTreatyHostSession(rtSys);
            if (_regionalTreatyPanel != null && _regionalTreatyPanel.IsInsideTree())
                RemoveChild(_regionalTreatyPanel);
            _regionalTreatyPanel = new RegionalTreatyPanel();
            _regionalTreatyPanel.Bind(_regionalTreaty);
            _regionalTreatyPanel.Visible = false;
            AddChild(_regionalTreatyPanel);
        }

        private void SaveRegionalTreaty()
        {
            if (_regionalTreaty != null)
                CaptureSection("regional_treaty", RegionalTreatySaveStore.TryCapturePersisted(_regionalTreaty.System.CaptureState()));
        }

        private void SetupVinylMorale()
        {
            if (_vinylMorale != null) return;
            var vmState = VinylMoraleSaveStore.TryLoad() ?? new VinylMoraleState();
            var vmSys = new VinylMoraleSystem(new GodotLog());
            vmSys.RestoreState(vmState);
            LoadVinylRecordCatalog(vmSys);
            _vinylMorale = new VinylMoraleHostSession(vmSys);
            _vinylMorale.DayProvider = () => _simDay;
            if (_vinylMoralePanel != null && _vinylMoralePanel.IsInsideTree())
                RemoveChild(_vinylMoralePanel);
            _vinylMoralePanel = new VinylMoralePanel();
            _vinylMoralePanel.Bind(_vinylMorale);
            _vinylMoralePanel.Visible = false;
            AddChild(_vinylMoralePanel);
        }

        /// <summary>
        /// Load the pre-war vinyl record archive (narrative/vinyl_record_archive.json)
        /// into the VinylMoraleSystem. The archive uses the Narrative VinylRecordEntry
        /// shape (rich archival metadata); the morale system uses VinylRecordDefinition
        /// (playback-focused). This bridges the two without a second catalog file.
        /// Missing file is non-fatal — the system runs with an empty catalog (headless tests).
        /// </summary>
        private void LoadVinylRecordCatalog(VinylMoraleSystem system)
        {
            string path = System.IO.Path.Combine(_dataDir, "narrative", "vinyl_record_archive.json");
            if (!System.IO.File.Exists(path)) return;
            string json = System.IO.File.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(json)) return;

            var file = new SystemTextJsonSerializer().Deserialize<VinylRecordsFile>(json);
            if (file?.records == null) return;

            var defs = new List<VinylRecordDefinition>(file.records.Count);
            foreach (var r in file.records)
            {
                if (r == null || string.IsNullOrEmpty(r.record_id)) continue;
                // Genre: prefer the first tag (e.g. "classical", "jazz", "folk");
                // IsRareCulturalRecord checks genre for classical/jazz/symphony/hymnal.
                string genre = (r.tags != null && r.tags.Length > 0) ? r.tags[0] : string.Empty;
                defs.Add(new VinylRecordDefinition
                {
                    record_id = r.record_id,
                    display_name = !string.IsNullOrEmpty(r.title) ? r.title : r.record_id,
                    genre = genre,
                    morale_daily_bonus = r.daily_morale_modifier,
                    flashback_suppression = 0f,
                    audio_cue_id = string.Empty,
                    description = !string.IsNullOrEmpty(r.dweller_resonance_notes)
                        ? r.dweller_resonance_notes
                        : (r.needle_audio_texture ?? string.Empty)
                });
            }
            system.LoadCatalog(defs);
        }

        private void SaveVinylMorale()
        {
            if (_vinylMorale != null)
                CaptureSection("vinyl_morale", VinylMoraleSaveStore.TryCapturePersisted(_vinylMorale.System.CaptureState()));
        }

        private void SetupWildlifeTrapping()
        {
            if (_wildlifeTrapping != null) return;
            SetupCampaignDay();
            var wtrapState = WildlifeTrappingSaveStore.TryLoad() ?? new WildlifeTrappingState();
            var wtrapSys = new WildlifeTrappingSystem(_campaignDay.Rng.GetStream(Ashfall.Core.Random.CampaignStreamIds.Shelter).Rng, new GodotLog());
            // Plan 36: load trapping catalog and register prey/bait definitions
            WildlifeTrappingCatalog? trapCatalog = null;
            if (!string.IsNullOrEmpty(_dataDir))
            {
                var fileIO = CatalogPath.CreateFileIOForDataDir(_dataDir);
                var json = new SystemTextJsonSerializer();
                trapCatalog = WildlifeTrappingCatalogLoader.Load(_dataDir, fileIO, json, new GodotLog());
                if (trapCatalog != null) trapCatalog.RegisterWith(wtrapSys);
            }
            wtrapSys.RestoreState(wtrapState);
            _wildlifeTrapping = new WildlifeTrappingHostSession(wtrapSys);
            _wildlifeTrapping.Catalog = trapCatalog;
            _wildlifeTrapping.Inventory = _inventory;
            // Plan 36 Closure II: wire disease/contamination delegates to live authorities
            _wildlifeTrapping.ApplyDisease = (survivorId, diseaseId, day) =>
            {
                if (_disease == null) SetupDisease();
                _disease?.Engine?.Infect(survivorId, diseaseId, day);
            };
            _wildlifeTrapping.ApplyContamination = (survivorId, dose) =>
            {
                if (_survivors != null && dose > 0f)
                    _survivors.ExposeToZone(survivorId, dose);
            };
            // Plan 28 Phase 3 (overhunt): snare catches thin the local packs
            // through the migration system's bounded harvest pressure.
            _wildlifeTrapping.OnCatchPressure += caught =>
            {
                if (_world == null) return;
                var sector = _world.ShelterSectorId;
                if (!string.IsNullOrEmpty(sector))
                    _world.Wildlife.ApplyHarvestPressure(sector, caught);
            };
            if (_wildlifeTrappingPanel != null && _wildlifeTrappingPanel.IsInsideTree())
                RemoveChild(_wildlifeTrappingPanel);
            _wildlifeTrappingPanel = new WildlifeTrappingPanel();
            _wildlifeTrappingPanel.Bind(_wildlifeTrapping);
            _wildlifeTrappingPanel.Visible = false;
            AddChild(_wildlifeTrappingPanel);
        }

        private void SaveWildlifeTrapping()
        {
            if (_wildlifeTrapping != null)
                CaptureSection("wildlife_trapping", WildlifeTrappingSaveStore.TryCapturePersisted(_wildlifeTrapping.System.CaptureState()));
        }

        private void SetupExcavation()
        {
            if (_excavation != null) return;
            SetupCampaignDay();
            var exState = ExcavationSaveStore.TryLoad() ?? new ExcavationState();
            var exSys = new ExcavationSystem(_campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.Shelter, 0, 2), new GodotLog());
            exSys.RestoreState(exState);
            _excavation = new ExcavationHostSession(exSys);
            if (_excavationPanel != null && _excavationPanel.IsInsideTree())
                RemoveChild(_excavationPanel);
            _excavationPanel = new ExcavationPanel();
            _excavationPanel.Bind(_excavation);
            _excavationPanel.Visible = false;
            AddChild(_excavationPanel);
        }

        private void SaveExcavation()
        {
            if (_excavation != null)
                CaptureSection("excavation", ExcavationSaveStore.TryCapturePersisted(_excavation.System.CaptureState()));
        }

        private void SetupApprenticeship()
        {
            if (_apprenticeship != null) return;
            SetupCampaignDay();
            var appState = ApprenticeshipSaveStore.TryLoad() ?? new ApprenticeshipState();
            var appSkills = new SkillProgressionSystem();
            var appSys = new ApprenticeshipSystem(_campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.Social, 0, 3), appSkills, _expandedShelterRoster, _survivorRelationsCore, new GodotLog());
            appSys.RestoreState(appState);
            _apprenticeship = new ApprenticeshipHostSession(appSys);
            if (_apprenticeshipPanel != null && _apprenticeshipPanel.IsInsideTree())
                RemoveChild(_apprenticeshipPanel);
            _apprenticeshipPanel = new ApprenticeshipPanel();
            _apprenticeshipPanel.Bind(_apprenticeship);
            _apprenticeshipPanel.Visible = false;
            AddChild(_apprenticeshipPanel);
        }

        private void SaveApprenticeship()
        {
            if (_apprenticeship != null)
                CaptureSection("apprenticeship", ApprenticeshipSaveStore.TryCapturePersisted(_apprenticeship.System.CaptureState()));
        }

        private void SetupCaregiving()
        {
            if (_caregiving != null) return;
            var cgState = CaregivingSaveStore.TryLoad() ?? new CaregivingSaveState();
            var cgSys = new CaregivingSystem();
            cgSys.RestoreState(cgState);
            _caregiving = new CaregivingHostSession(cgSys);
            if (_caregivingPanel != null && _caregivingPanel.IsInsideTree())
                RemoveChild(_caregivingPanel);
            _caregivingPanel = new CaregivingPanel();
            _caregivingPanel.Bind(_caregiving);
            _caregivingPanel.Visible = false;
            AddChild(_caregivingPanel);
        }

        private void SaveCaregiving()
        {
            if (_caregiving != null)
                CaptureSection("caregiving", CaregivingSaveStore.TryCapturePersisted(_caregiving.System.CaptureState()));
        }
    }
}
