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
using Ashfall.Core.Narrative;
using Ashfall.Core.YearOfAsh;
using Ashfall.Core.Radio;
using Ashfall.Core.Radiation;
using Ashfall.Core.Factions;
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
        // ── Narrative fields (GAP-ARCH-01 Phase 1) ──
        private NarrativeHostSession _narrative = null!;
        private bool _narrativeDirty;
        private RadioHostSession _radio = null!;
        private CraftingHostSession _crafting = null!;
        private bool _craftingDirty;
        private JournalSystem _journal = null!;
        private BureaucraticDocumentDiscoverySystem _bureaucraticDocumentDiscovery = null!;
        private bool _hostEventAdapterDirty;

        private void SetupJournal()
        {
            if (_journal != null) return;

            var catalogs = CatalogJsonLoader.Load(new FileSystemIO(), _dataDir);
            _journal = new JournalSystem();
            if (catalogs.BureaucraticDocuments != null)
            {
                _bureaucraticDocumentDiscovery = new BureaucraticDocumentDiscoverySystem(
                    catalogs.BureaucraticDocuments);
            }
            // Diegetic journal prose — without this binding every entry renders
            // the generic "Something changed." placeholder. LoadDefault resolves
            // the data dir and degrades to an empty catalog if absent.
            JournalVoice.BindCatalog(JournalVoiceProseCatalogLoader.LoadDefault());
            var authoredCorpus = new JournalCorpusCatalogLoader(
                new FileSystemIO(),
                new SystemTextJsonSerializer()).Load(_dataDir);
            var authoredAuthors = JournalDemoHarness.BuildAuthors(catalogs).Values;
            _journal.BindAuthoredCorpus(new JournalCorpusAdapter(authoredCorpus, authoredAuthors));
            BindJournalWorldProducerIfReady();
            // Mark dirty rather than writing the whole save file per entry; the
            // _Process tick flushes it. Seeding adds many entries in one frame and
            // used to rewrite journal_save.json once for each of them.
            _journal.OnEntryAdded += _ => _journalDirty = true;
            _journal.OnTabChanged += _ => _journalDirty = true;
            _journal.OnCodexUnlocked += _ => _journalDirty = true;

            _journalCodex = new JournalCodex(_journal, catalogs);

            if (_journalBook == null || !_journalBook.IsInsideTree())
            {
                _journalBook = new JournalBookUI();
                _journalBook.SetAnchorsPreset(LayoutPreset.FullRect);
                AddChild(_journalBook);
            }
            _journalBook.Bind(
                _journal,
                tab => _journalCodex.BuildRows(tab),
                tab => _journal.HasUnreadForTab(tab),
                () => _simDay);
            _journalBook.OnClosed += SaveJournal;

            if (JournalSaveStore.Exists)
            {
                var save = JournalSaveStore.Load();
                if (save != null) _journal.RestoreState(save);
                _journalBook.SetEntries(_journal.Entries);
                _journalBook.ApplyUiState(
                    _journal.HudIsOpen,
                    _journal.HasUnread,
                    _journal.NotificationPing,
                    _journal.ActiveTab);
                GD.Print("[Ashfall Godot] Journal restored from save.");
            }
            else
            {
                JournalDemoHarness.Seed(_journal, catalogs);
                _journalBook.SetEntries(_journal.Entries);
                SaveJournal();
                GD.Print("[Ashfall Godot] Journal seeded with opening-day entries.");
            }

            UpdateStatus();
        }

        /// <summary>
        /// Discover authored shelter paperwork through an explicit physical or
        /// administrative producer. The journal knowledge ledger is the only
        /// persisted discovery state; reading a document has no simulation effect.
        /// </summary>
        private void DiscoverBureaucraticDocuments(string producerId)
        {
            if (_journal == null || _bureaucraticDocumentDiscovery == null) return;

            int day = _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay;
            var results = _bureaucraticDocumentDiscovery.DiscoverByProducer(
                producerId,
                day,
                _journal);
            int discovered = 0;
            for (int i = 0; i < results.Count; i++)
            {
                if (results[i].Changed) discovered++;
            }

            if (discovered > 0)
            {
                _journalDirty = true;
                if (_statusLabel != null)
                    _statusLabel.Text = $"[DOCUMENTS] {discovered} institutional record(s) added to the journal.";
            }
        }

        /// <summary>
        /// Plan 153: reveal only fringe-cult records assigned to the explicit
        /// physical/archive producer. This shares the Plan 135 narrative
        /// discovery ledger; doctrine is never sent to a simulation authority.
        /// </summary>
        private void DiscoverFringeCultRecords(string producerId)
        {
            var catalog = _journalCodex?.Catalogs?.NarrativeDiscoveries;
            if (_journal == null || catalog == null || string.IsNullOrEmpty(producerId)) return;

            int day = _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay;
            int discovered = 0;
            var records = catalog.GetByProducer(producerId);
            for (int i = 0; i < records.Count; i++)
            {
                var record = records[i];
                if (!FringeCultRuntimeContract.IsSourceCatalog(record.SourceCatalog)
                    || record.MinDay > day)
                    continue;
                if (catalog.TryDiscover(record.DiscoveryId, _journal, out _))
                    discovered++;
            }

            if (discovered > 0)
            {
                _journalDirty = true;
                if (_statusLabel != null)
                    _statusLabel.Text = $"[ARCHIVE] {discovered} fringe-cult record(s) added to the journal.";
            }
        }

        /// <summary>
        /// Plan 156: reveal authored paper-making and printing records from an
        /// explicit room, archive, or map-location producer. The source
        /// measurements are projected into Journal only and never enter
        /// inventory, crafting, faction, research, or document authority.
        /// </summary>
        private void DiscoverPaperPrintingRecords(string producerId)
        {
            var catalog = _journalCodex?.Catalogs?.NarrativeDiscoveries;
            if (_journal == null || catalog == null || string.IsNullOrEmpty(producerId)) return;

            int day = _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay;
            int discovered = 0;
            var records = catalog.GetByProducer(producerId);
            for (int i = 0; i < records.Count; i++)
            {
                var record = records[i];
                if (!PaperPrintRuntimeContract.IsSourceCatalog(record.SourceCatalog)
                    || record.MinDay > day)
                    continue;
                if (catalog.TryDiscover(record.DiscoveryId, _journal, out _))
                    discovered++;
            }

            if (discovered > 0)
            {
                _journalDirty = true;
                if (_statusLabel != null)
                    _statusLabel.Text = $"[ARCHIVE] {discovered} paper/print record(s) added to the journal.";
            }
        }

        /// <summary>
        /// Plan 160: reveal only bone, horn and antler records assigned to an
        /// explicit workshop, archive or world producer. Source animal labels
        /// remain historical provenance and never target living companions.
        /// </summary>
        private void DiscoverBoneHornRecords(string producerId)
        {
            var catalog = _journalCodex?.Catalogs?.NarrativeDiscoveries;
            if (_journal == null || catalog == null || string.IsNullOrEmpty(producerId)) return;

            int day = _yearOfAsh != null ? _yearOfAsh.Timeline.CurrentDay : _simDay;
            int discovered = 0;
            var records = catalog.GetByProducer(producerId);
            for (int i = 0; i < records.Count; i++)
            {
                var record = records[i];
                if (!BoneHornRuntimeContract.IsSourceCatalog(record.SourceCatalog)
                    || record.MinDay > day)
                    continue;
                if (catalog.TryDiscover(record.DiscoveryId, _journal, out _))
                    discovered++;
            }

            if (discovered > 0)
            {
                _journalDirty = true;
                if (_statusLabel != null)
                    _statusLabel.Text = $"[ARCHIVE] {discovered} bone/horn craft record(s) added to the journal.";
            }
        }

        private void ToggleJournal()
        {
            if (_journalBook != null) _journalBook.Toggle();
            UpdateStatus();
        }

        private void SaveJournal()
        {
            if (_journal == null) return;
            if (CaptureSection("journal", JournalSaveStore.TryCapturePersisted(_journal.CaptureState())))
                _journalDirty = false;
        }

        private void SetupEventAdapter(bool reloadFromDisk = false)
        {
            if (_hostEventAdapter != null && !reloadFromDisk) return;

            if (_hostEventAdapter != null)
            {
                _hostEventAdapter.Dispose();
                _hostEventAdapter = null!;
            }

            SetupJournal();
            if (_eventBus == null) _eventBus = new Ashfall.Core.Events.SimpleEventBus();

            // The adapter is the sole owner of mutable event progress. Restore the
            // selected campaign's projected host_event payload before any day tick
            // can evaluate triggers; the catalog session remains read-only.
            _hostEventAdapter = new AtomicWar.GodotApp.Host.HostEventAdapter(_eventBus, _journal);
            var loadedEventState = HostEventSaveStore.TryLoad();
            if (loadedEventState != null)
            {
                _hostEventAdapter.RestoreState(loadedEventState);
            }
            _hostEventAdapter.OnEventDispatched += (id, desc) =>
            {
                if (_statusLabel != null)
                    _statusLabel.Text = $"[EVENT DISPATCHED] {id}: {desc}";
                _journalDirty = true;
            };
            _hostEventAdapter.StateChanged += () => _hostEventAdapterDirty = true;
        }

        /// <summary>
        /// Writes the journal only when something actually changed. Called from the
        /// throttled _Process tick so a burst of entries costs one file write.
        /// </summary>
        private void FlushJournalIfDirty()
        {
            if (_journalDirty) SaveJournal();
        }

        private void FlushNarrativeIfDirty()
        {
            if (_narrativeDirty) SaveNarrative();
        }

        private void FlushEventAdapterIfDirty()
        {
            if (_hostEventAdapterDirty) SaveEventAdapter();
        }

        private void SetupNarrative(bool reloadEventAdapter = false)
        {
            EnsureNarrativeSession();
            ConfigureNarrativeArcRuntime();

            // Narrative setup is part of both composition and restore. Initialize
            // the event adapter here so campaign state is loaded before its first
            // day-owner evaluation, without coupling it to the catalog read-model.
            SetupEventAdapter(reloadEventAdapter);

            // F3 — the expedition host's journal seam binds once the journal
            // authority exists (SetupEventAdapter → SetupJournal created it).
            BindExpeditionJournalIfReady();
        }

        /// <summary>F3 — share the one journal authority with the expedition
        /// host's consequence applier. No-op until both sessions exist.</summary>
        private void BindExpeditionJournalIfReady()
        {
            if (_expeditions == null || _journal == null) return;
            if (_expeditions.Journal == _journal) return;
            _expeditions.Journal = _journal;
        }

        /// <summary>
        /// F1–F4 — ensure the ONE narrative-encounter engine exists (catalog
        /// loaded, save restored) without touching the event adapter. The
        /// expedition host shares this engine so depletion, resolution history,
        /// and the pending queue have a single save-backed authority.
        /// </summary>
        private void EnsureNarrativeSession()
        {
            if (_narrative != null) return;
            _narrative = NarrativeHostSession.Create(_dataDir);
            _narrative.StateChanged += () => _narrativeDirty = true;
            GD.Print("[Ashfall Godot] Narrative host ready.");
        }

        private void ConfigureNarrativeArcRuntime()
        {
            if (_narrative == null) return;

            var adapter = new NarrativeArcConsequenceAdapter
            {
                MoralePreflight = CanApplyNarrativeMorale,
                MoraleCommit = ApplyNarrativeMorale,
                IntelPreflight = CanGrantNarrativeIntel,
                IntelCommit = GrantNarrativeIntel,
                ExpeditionPreflight = CanOfferNarrativeExpedition,
                ExpeditionCommit = OfferNarrativeExpedition,
                StandingPreflight = CanApplyNarrativeStanding,
                StandingCommit = ApplyNarrativeStanding
            };
            _narrative.ConfigureArcRuntime(IsNarrativeSurvivorPresent, adapter);
        }

        private bool IsNarrativeSurvivorPresent(string survivorId)
        {
            SetupSurvivors();
            if (_survivors == null) return false;
            var survivor = _survivors.Find(survivorId);
            if (survivor == null || !survivor.IsAliveState) return false;
            return _survivors.GetSurvivorLocation(survivorId).Kind == SurvivorExposureLocation.ShelterInterior;
        }

        private (bool ok, string reason) CanApplyNarrativeMorale(string survivorId, int delta, bool shelterWide)
        {
            SetupSurvivors();
            if (shelterWide)
            {
                bool resident = _survivors.RosterState.Any(s => s != null && s.IsAliveState &&
                    _survivors.GetSurvivorLocation(s.Id).Kind == SurvivorExposureLocation.ShelterInterior);
                return resident ? (true, string.Empty) : (false, "no living resident can receive morale");
            }
            return IsNarrativeSurvivorPresent(survivorId)
                ? (true, string.Empty)
                : (false, "the addressed survivor is unavailable");
        }

        private void ApplyNarrativeMorale(string survivorId, int delta, bool shelterWide)
        {
            SetupSurvivors();
            if (!shelterWide)
            {
                var survivor = _survivors.Find(survivorId);
                if (survivor != null) _survivors.Needs.Modify(survivor, NeedKind.Morale, delta);
                return;
            }

            foreach (var survivor in _survivors.RosterState
                .Where(s => s != null && s.IsAliveState &&
                    _survivors.GetSurvivorLocation(s.Id).Kind == SurvivorExposureLocation.ShelterInterior)
                .OrderBy(s => s.Id, StringComparer.Ordinal))
            {
                _survivors.Needs.Modify(survivor, NeedKind.Morale, delta);
            }
        }

        private (bool ok, string reason) CanGrantNarrativeIntel(string canonicalFactionId)
        {
            SetupJournal();
            return FactionStandingIdResolver.IsKnownFaction(canonicalFactionId) && _journal != null
                ? (true, string.Empty)
                : (false, "canonical faction intel or journal authority is unavailable");
        }

        private void GrantNarrativeIntel(string canonicalFactionId)
        {
            SetupJournal();
            if (_journal == null) return;
            _journal.Knowledge.Discover(KnowledgeKeys.FactionIntel(canonicalFactionId));
            _journalDirty = true;
        }

        private (bool ok, string reason) CanOfferNarrativeExpedition(string locationId)
        {
            SetupExpeditions();
            bool found = _expeditions != null && _expeditions.Definitions.Any(d => d != null && d.id == locationId);
            return found
                ? (true, string.Empty)
                : (false, "narrative expedition target is not in the expedition catalog");
        }

        private void OfferNarrativeExpedition(string locationId)
        {
            if (_statusLabel != null)
                _statusLabel.Text = "Expedition opportunity recorded. Review it in Expeditions; normal dispatch requirements apply.";
        }

        private (bool ok, string reason) CanApplyNarrativeStanding(string canonicalFactionId, int delta)
        {
            SetupYearOfAsh();
            return FactionStandingIdResolver.IsKnownFaction(canonicalFactionId) && _yearOfAsh != null
                ? (true, string.Empty)
                : (false, "canonical faction standing authority is unavailable");
        }

        private void ApplyNarrativeStanding(string canonicalFactionId, int delta)
        {
            SetupYearOfAsh();
            _yearOfAsh?.FactionWar.ModifyStanding(canonicalFactionId, delta);
            _yearOfAshDirty = true;
        }

        private void OpenNarrativeArcModal()
        {
            SetupNarrative();
            var pending = _narrative.PendingArcEvent;
            if (pending == null)
            {
                if (_statusLabel != null) _statusLabel.Text = "No narrative arc event is waiting for a decision.";
                return;
            }
            _narrativeArcModal.Display(pending, _simDay);
        }

        private void OnNarrativeArcChoiceSelected(string eventId, string choiceId)
        {
            SetupNarrative();
            var result = _narrative.ResolveArcChoice(eventId, choiceId, _simDay);
            if (!result.Succeeded)
            {
                if (_statusLabel != null) _statusLabel.Text = "Narrative choice refused: " + result.Reason;
                return;
            }
            SaveNarrative();
            if (_statusLabel != null) _statusLabel.Text = _narrative.LastEvent;
            _narrativeArcModal.DisplayOutcome(_narrative.LastEvent);
        }

        private void OnNarrativeArcAcknowledged(string eventId)
        {
            SetupNarrative();
            var result = _narrative.AcknowledgeArcEvent(eventId, _simDay);
            if (!result.Succeeded)
            {
                if (_statusLabel != null) _statusLabel.Text = "Narrative event refused: " + result.Reason;
                return;
            }
            SaveNarrative();
            if (_statusLabel != null) _statusLabel.Text = _narrative.LastEvent;
            _narrativeArcModal.DisplayOutcome(_narrative.LastEvent);
        }

        private void SaveNarrative()
        {
            if (_narrative == null) return;
            if (CaptureSection("narrative", NarrativeSaveStore.TryCapturePersisted(_narrative.CaptureSave())))
            {
                _narrativeDirty = false;
                GD.Print("[Ashfall Godot] Narrative save written.");
            }
        }

        private void SaveEventAdapter()
        {
            if (_hostEventAdapter == null) return;
            if (CaptureSection("host_event", HostEventSaveStore.TryCapturePersisted(_hostEventAdapter.CaptureState())))
            {
                _hostEventAdapterDirty = false;
            }
        }

        private void OnNarrativeOpenClicked()
        {
            SetupNarrative();
            _statusLabel.Text = _narrative.SelectDemo("cautious", 0.5f, "loc_denial_cut_substation")
                + "\n" + _narrative.StatusLine();
        }

        private void SetupRadio()
        {
            if (_radio != null)
            {
                _radio.SetDay(_simDay);
                return;
            }

            SetupJournal();
            _radio = RadioHostSession.Create(_dataDir, _core != null ? _core.Clock.Day : _simDay);
            _radio.StateChanged += () => _radioPanel?.RefreshView();
            _radio.Triangulation.OnLocationRevealed += locId =>
            {
                _journal?.TryAddRawEntry(
                    $"sig_disc_{locId}_{_radio.Day}",
                    $"Direction-finding telemetry confirmed active radio emissions at {locId}.",
                    null!,
                    _radio.Day);
                // Continuous DF yields a rumor fix — not an instant surveyed Discover.
                // Exact-fix HF intercepts remain on ShelterRadioStationSystem → Discover.
                bool rumored = _world?.WastelandMap?.DiscoverRumor(
                    locId,
                    sourceId: "signal_triangulation",
                    day: _radio.Day,
                    confidence: InformationConfidence.Medium) == true;
                TryBridgeDistressFromTriangulation(locId);
                GD.Print(rumored
                    ? $"[Ashfall Godot] Triangulation rumored wasteland location '{locId}'."
                    : $"[Ashfall Godot] Triangulation revealed '{locId}' (no map node / already known).");
            };
            GD.Print("[Ashfall Godot] Radio host ready.");
        }

        /// <summary>
        /// When continuous DF resolves a fingerprint-mapped location, mark any
        /// active distress whose signal id or revealed location matches.
        /// </summary>
        private void TryBridgeDistressFromTriangulation(string locationOrSignalId)
        {
            if (_radio?.DistressSystem == null || string.IsNullOrEmpty(locationOrSignalId)) return;

            if (_radio.DistressSystem.MarkTriangulated(locationOrSignalId))
                return;

            var catalog = _radio.Triangulation.Catalog;
            if (catalog == null) return;
            foreach (var fp in catalog.FingerprintsBySignal.Values)
            {
                if (fp == null) continue;
                if (string.Equals(fp.mapped_location_id, locationOrSignalId, StringComparison.Ordinal)
                    || string.Equals(fp.signal_id, locationOrSignalId, StringComparison.Ordinal))
                {
                    _radio.DistressSystem.MarkTriangulated(fp.signal_id);
                }
            }
        }

        private void SaveRadio()
        {
            if (_radio == null) return;
            if (CaptureSection("radio", RadioSaveStore.TryCapturePersisted(_radio.CaptureSave())))
            {
                GD.Print("[Ashfall Godot] Radio save written.");
            }
        }

        private void CloseRadioPanel()
        {
            _radioPanel.Visible = false;
        }

        private void CloseJournalPanel()
        {
            _journalPanel.Visible = false;
        }

        private void CloseJournalDetailPanel()
        {
            _journalDetailPanel.Visible = false;
        }

    }
}
