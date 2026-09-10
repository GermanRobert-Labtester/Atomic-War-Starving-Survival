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

            // Plan VIII · Task 21 — typed treaty transitions become world
            // consequences through the canonical consumers: faction-war standing
            // (escalation spine, 21.10) and the radio broadcast wire (21.6).
            // RestoreState never emits transitions, so neither consumer can
            // double-apply across a save/load.
            rtSys.OnTreatyTransition += transition =>
                OnTreatyTransitionWorldConsequences(rtSys, transition);

            if (_regionalTreatyPanel != null && _regionalTreatyPanel.IsInsideTree())
                RemoveChild(_regionalTreatyPanel);
            _regionalTreatyPanel = new RegionalTreatyPanel();
            _regionalTreatyPanel.Bind(_regionalTreaty);
            _regionalTreatyPanel.Visible = false;
            AddChild(_regionalTreatyPanel);
        }

        private void OnTreatyTransitionWorldConsequences(RegionalTreatySystem treatySystem, TreatyTransition transition)
        {
            if (transition.IsBreach && !string.IsNullOrEmpty(transition.FactionId))
            {
                // Task 21.10 — through the canonical escalation API only
                // (FactionWarSystem.ModifyStanding clamps and raises its own event).
                var def = treatySystem.GetDefinition(transition.TreatyId);
                int penalty = def != null ? (int)def.violation_penalty_affinity : -20;
                _yearOfAsh?.FactionWar.ModifyStanding(transition.FactionId, penalty);
            }

            if (_radio != null)
            {
                var def = treatySystem.GetDefinition(transition.TreatyId);
                _radio.ScheduleCoordinator.InjectTreatyAlert(TreatyBulletins.Compose(transition, def));
            }
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
            _vinylMorale.System.OnMoraleApplied += amount =>
            {
                if (amount > 0 && _survivors?.Needs != null)
                {
                    foreach (var sv in _survivors.Needs.Registered)
                    {
                        _survivors.Needs.Modify(sv, NeedKind.Morale, amount);
                    }
                }
            };
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
            _wildlifeTrapping.DeliverButcheryFood = foodUnits =>
                _inventory != null && _inventory.TryAdd("raw_meat", foodUnits);
            _wildlifeTrapping.ApplyMorale = (survivorId, delta, source) =>
            {
                if (_survivors == null) SetupSurvivors();
                var needs = _survivors?.Needs;
                if (needs == null || needs.Get(survivorId) == null)
                {
                    GD.PushWarning($"[WildlifeTrapping] Cannot apply morale {delta:0.###} to missing survivor '{survivorId}' ({source}).");
                    return;
                }
                needs.Modify(survivorId, NeedKind.Morale, delta);
            };

            // Plan VI: bycatch is a domain fact; the authored narrative
            // authority owns its prose and presentation. The source key is
            // stable across restore so a repeated host subscription cannot
            // duplicate the notification.
            _wildlifeTrapping.OnBycatchOccurred += occurrence =>
            {
                if (occurrence == null) return;
                SetupEventsHost();
                if (!_eventsHost.TryGetEvent("event_trapping_bycatch_entanglement", out var authored))
                {
                    GD.PushWarning("[WildlifeTrapping] Bycatch narrative event is not authored; domain fact remains in the journal.");
                    return;
                }
                SetupEventAdapter();
                string sourceId = $"wildlife-trap:{occurrence.siteId}:bycatch:{occurrence.day}:{occurrence.bycatchSpeciesId}";
                _hostEventAdapter?.DispatchCatalogEvent(
                    authored.Id,
                    authored.BodyText,
                    occurrence.day,
                    sourceId);
            };
            SetupWorld();
            _wildlifeTrapping.Map = _world?.WastelandMap;

            // Contextual trapping lessons are routed through the persisted
            // onboarding authority, never opened by the Core domain itself.
            SetupOnboarding();
            _wildlifeTrapping.OnTrapCrafted += _ =>
                _onboardingJourney?.RequestContextualTutorial(
                    Ashfall.Core.Localization.WildlifeTrappingLocalization.FirstSnareTutorialId);
            wtrapSys.OnTrapDeployed += _ =>
                _onboardingJourney?.RequestContextualTutorial(
                    Ashfall.Core.Localization.WildlifeTrappingLocalization.FirstSnareTutorialId);
            wtrapSys.OnTrapBroken += _ =>
                _onboardingJourney?.RequestContextualTutorial(
                    Ashfall.Core.Localization.WildlifeTrappingLocalization.WearOutTutorialId);
            wtrapSys.OnBycatchOccurred += (_, _, _, _, _, _) =>
                _onboardingJourney?.RequestContextualTutorial(
                    Ashfall.Core.Localization.WildlifeTrappingLocalization.BycatchTutorialId);
            // Plan 36 Closure II: wire disease/contamination delegates to live authorities
            _wildlifeTrapping.ApplyDisease = (survivorId, diseaseId, day) =>
            {
                if (string.IsNullOrEmpty(survivorId)) return;
                var def = _survivors?.Roster?.FindDefinition(survivorId);
                if (def != null && def.traitIds != null && def.traitIds.Contains("skill_sanitization_expert"))
                    return;
                if (_disease == null) SetupDisease();
                if (_disease?.Engine != null)
                {
                    _disease.Engine.Infect(survivorId, diseaseId, day);
                }
                else
                {
                    GD.PrintErr($"[Ashfall Godot] Trapping: cannot apply disease '{diseaseId}' to '{survivorId}' - disease authority offline.");
                }
            };
            _wildlifeTrapping.ApplyContamination = (survivorId, dose) =>
            {
                if (string.IsNullOrEmpty(survivorId) || dose <= 0f) return;
                if (_survivors == null) SetupSurvivors();
                if (_survivors != null)
                {
                    _survivors.ExposeToZone(survivorId, dose);
                }
                else
                {
                    GD.PrintErr($"[Ashfall Godot] Trapping: cannot apply contamination dose {dose} to '{survivorId}' - survivors authority offline.");
                }
            };

            // ── Plan IV: destination-authority adapters ──
            // Trapping emits pending domain facts; these adapters hand each
            // one to the owning authority. A rejected/absent destination
            // leaves the fact pending for retry — nothing is dropped.

            // Task 5 — moral authority. The dilemma itself is an authored
            // quest in the moral catalog; surfacing is derived from the
            // pending outbox (GetAvailableMoralChoices), and the ack happens
            // at RESOLUTION so a save before the player decides replays the
            // exact same dilemma. Unknown quests stay pending with a warning.
            _wildlifeTrapping.DeliverMoralConsequence = (questId, speciesId, survivorId) =>
            {
                SetupMoralChoice();
                if (_moralChoice.GetQuest(questId) == null)
                {
                    GD.PushWarning($"[WildlifeTrapping] Moral quest '{questId}' not registered; consequence stays pending.");
                    return false;
                }
                // The moral ledger owns persistence after acceptance; a
                // resolved quest is acked immediately so restore never
                // re-dispatches an already-resolved dilemma.
                return _moralChoice.IsResolved(questId);
            };

            // Task 6 — encounter authority. Pending interference encounters
            // persist in the narrative encounter state and surface through
            // the existing pending-encounter projection.
            _wildlifeTrapping.DeliverTrapEncounter = (encounterId, siteId, day) =>
            {
                if (_narrative == null) return false;
                var engine = _narrative.Engine;
                if (engine == null || engine.Find(encounterId) == null)
                {
                    GD.PushWarning($"[WildlifeTrapping] Encounter '{encounterId}' not registered; fact stays pending.");
                    return false;
                }
                var pendingList = engine.State.pending;
                for (int i = 0; i < pendingList.Count; i++)
                {
                    var p = pendingList[i];
                    if (p != null && string.Equals(p.encounterId, encounterId, StringComparison.Ordinal)
                        && string.Equals(p.locationId, siteId, StringComparison.Ordinal))
                        return true; // already queued — idempotent re-ack
                }
                engine.EnqueuePending(encounterId, siteId, 0, day);
                return true;
            };

            // Plan VI: miss-only atmospheric incidents are delivered through
            // the same catalog/event adapter. A failed dispatch leaves the
            // Core outbox pending for a later retry.
            _wildlifeTrapping.DeliverNarrativeIncident = (eventId, siteId, day, sourceId) =>
            {
                SetupEventsHost();
                if (!_eventsHost.TryGetEvent(eventId, out var authored))
                {
                    GD.PushWarning($"[WildlifeTrapping] Narrative incident '{eventId}' is not registered; fact stays pending.");
                    return false;
                }
                SetupEventAdapter();
                return _hostEventAdapter != null
                    && _hostEventAdapter.DispatchCatalogEvent(eventId, authored.BodyText, day, sourceId);
            };

            // Task 7 — radio authority. One dynamic wildlife-net slot: while
            // an unsurfaced report occupies it, later facts stay pending and
            // deliver in sequence order as the slot frees.
            _wildlifeTrapping.DeliverTrappingBroadcast = message =>
            {
                if (_radio == null) return false;
                var coordinator = _radio.ScheduleCoordinator;
                if (coordinator == null || coordinator.HasTrappingAlert) return false;
                coordinator.InjectTrappingAlert(message);
                return true;
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

            // WT-INT-01: wire first-catch species discovery to Journal and Codex
            wtrapSys.OnNewSpeciesDiscovered += (speciesId, siteId, hunterId) =>
            {
                if (_journal == null) SetupJournal();
                if (_journal == null) return;

                string knowledgeKey = Ashfall.Core.Journal.KnowledgeKeys.WildlifeSpeciesCaught(speciesId);

                // Resolve display name for species from catalog if available
                string speciesName = speciesId;
                if (trapCatalog != null && trapCatalog.Prey.TryGetValue(speciesId, out var preyDef) && !string.IsNullOrEmpty(preyDef.displayName))
                {
                    speciesName = preyDef.displayName;
                }

                // Resolve author: assigned hunter -> shelter fallback
                Ashfall.Core.Journal.ISurvivorAuthor? author = null;
                if (_survivors?.Roster != null && !string.IsNullOrEmpty(hunterId))
                {
                    var survivorDef = _survivors.Roster.FindDefinition(hunterId);
                    if (survivorDef != null)
                    {
                        author = new TrappingJournalAuthor(survivorDef.id, survivorDef.displayName);
                    }
                }
                author ??= new TrappingJournalAuthor(
                    string.IsNullOrEmpty(hunterId) ? "shelter_crew" : hunterId,
                    string.IsNullOrEmpty(hunterId) ? "Shelter Trapper" : hunterId);

                string text = $"Captured first specimen of {speciesName} at trap site {siteId} (hunter: {author.DisplayName}).";
                _journal.TryDiscoverRawKnowledge(knowledgeKey, text, author, _simDay);
            };

            // Plan 36 III: wire bycatch occurrences to Journal
            wtrapSys.OnBycatchOccurred += (siteId, trapId, primarySpecies, bycatchSpecies, day, hunterId) =>
            {
                if (_journal == null) SetupJournal();
                if (_journal == null) return;

                string knowledgeKey = $"wildlife.bycatch.{bycatchSpecies}";
                string text = $"Secondary quarry entangled at trap site {siteId}: {bycatchSpecies} (primary catch: {primarySpecies}, trap: {trapId}).";
                _journal.TryDiscoverRawKnowledge(knowledgeKey, text, null, _simDay);
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
            var appSkills = EnsureSharedSkillProgression();
            if (appState.skillProgression != null)
                appSkills.RestoreState(appState.skillProgression);
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
            {
                var state = _apprenticeship.System.CaptureState();
                state.skillProgression = EnsureSharedSkillProgression().CaptureState();
                CaptureSection("apprenticeship", ApprenticeshipSaveStore.TryCapturePersisted(state));
            }
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

        private sealed class TrappingJournalAuthor : Ashfall.Core.Journal.ISurvivorAuthor
        {
            public string Id { get; }
            public string DisplayName { get; }
            public Ashfall.Core.Journal.RiskBiasTrait RiskBias => Ashfall.Core.Journal.RiskBiasTrait.Realist;

            public TrappingJournalAuthor(string id, string displayName)
            {
                Id = id;
                DisplayName = displayName;
            }
        }
    }
}
