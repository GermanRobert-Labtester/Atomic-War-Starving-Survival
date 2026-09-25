// SPDX-License-Identifier: MIT
using Godot;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using AtomicWar.Journal;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Crafting;
using Ashfall.Core.Economy;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Foundry;
using Ashfall.Core.Inventory;
using Ashfall.Core.Journal;
using Ashfall.Core.Muster;
using Ashfall.Core.YearOfAsh;
using Ashfall.Core.Radio;
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
        // ── Phase 0 / Dose fields (GAP-ARCH-01 Phase 1) ──
        private PhantomMemoryHostSession _phantomMemory = null!;
        private Phase0HostSession _phase0 = null!;
        private bool _phase0Dirty;
        private DoseLedgerHostSession _doseLedger = null!;
        private bool _doseLedgerDirty;
        private DoseRegisterSurface _doseSurface = null!;

        private void SetupPhantom()
        {
            if (_phantomMemory != null) return;
            SetupCampaignDay();
            var rng = _campaignDay?.Rng.GetStream(Ashfall.Core.Random.CampaignStreamIds.Psychology).Rng;
            _phantomMemory = PhantomMemoryHostSession.Create(_dataDir, rng);
            _phantomMemory.StateChanged += () => SavePhantomMemory();
            SetupSurvivors();
            SetupEnrichment();
            if (_survivors != null)
            {
                // Enrichment is the explicit background authority when present;
                // profession mapping remains the fallback for roster entries
                // that do not have an enrichment row.
                _phantomMemory.BindSurvivors(_survivors, _enrichment);
            }
            SetupInventory();
            if (_inventory != null)
            {
                _phantomMemory.BindInventory(_inventory);
            }
            _phantomMemory.Engine.OnPhantomMemoryResolved += (svId, itemId, isMotivation, moraleDelta, guiltDelta) =>
            {
                var sv = _survivors?.Find(svId);
                if (sv != null && moraleDelta != 0f)
                {
                    _survivors!.Needs.Modify(sv, NeedKind.Morale, moraleDelta);
                }
            };

            var save = PhantomMemorySaveStore.TryLoad();
            if (save != null)
            {
                _phantomMemory.RestoreSave(save);
                GD.Print("[Ashfall Godot] Phantom Memory state restored.");
            }
        }

        private void OnPhantomScavengeClicked()
        {
            SetupPhantom();
            _statusLabel.Text = _phantomMemory.ScavengeItem("survivor_gunner_mikhail", "dog_tags");
        }

        private void OnPhantomTickClicked()
        {
            SetupPhantom();
            _statusLabel.Text = _phantomMemory.TickDemo();
        }

        private void SavePhantomMemory()
        {
            if (_phantomMemory == null) return;
            if (CaptureSection("phantom_memory", PhantomMemorySaveStore.TryCapturePersisted(_phantomMemory.CaptureSave())))
                GD.Print("[Ashfall Godot] Phantom Memory save written.");
        }

        private void SetupPhase0()
        {
            if (_phase0 != null) return;
            SetupMedical();
            // Task #133: share the MedicalHostSession-owned dependency ledger so
            // there is exactly one chem-dep authority, and Phase-0 does not tick it.
            _phase0 = new Phase0HostSession(dependency: _medical.Engine);
            _phase0.StateChanged += () => _phase0Dirty = true;
            // Feed the specialty catalog — without it the wired specialty loop
            // runs patternless and mastery can never progress.
            _phase0.LoadTradeSpecialties(_dataDir);

            // ── Wire every Phase-0 effect to the REAL gameplay consumer ──
            SetupSurvivors();
            SetupJournal();
            SetupCrafting();
            SetupExpeditions();
            SetupMedical();
            SetupEnrichment();

            // Recorded craft-attribution contract: crafting owns recipe completion,
            // this host supplies the survivor, profession, and result item. Both
            // _phase0 and _crafting reset through the lifecycle registry, so this
            // subscription is rebuilt against the fresh engine on a new campaign.
            _crafting.Engine.OnCraftCompleted += OnCraftCompletedForSpecialty;

            _phase0.Consumers = new Phase0EffectConsumers(
                applyMoraleDelta: (sv, delta) =>
                {
                    var survivor = _survivors.Find(sv);
                    if (survivor != null) _survivors.Needs.Modify(survivor, NeedKind.Morale, delta);
                },
                applyHealthDelta: (sv, delta) =>
                {
                    var survivor = _survivors.Find(sv);
                    if (survivor != null) _survivors.Needs.Modify(survivor, NeedKind.Health, delta);
                },
                applyFatigueDelta: (sv, delta) =>
                {
                    var survivor = _survivors.Find(sv);
                    if (survivor != null) _survivors.Needs.Modify(survivor, NeedKind.Fatigue, delta);
                },
                applyShelterMoraleDelta: delta =>
                {
                    for (int i = 0; i < _survivors.RosterState.Count; i++)
                    {
                        var s = _survivors.RosterState[i];
                        if (s != null && s.IsAliveState)
                            _survivors.Needs.Modify(s, NeedKind.Morale, delta);
                    }
                },
                applyWorkEfficiencyMultiplier: (sv, mult) =>
                {
                    if (_crafting == null) return;
                    _crafting.Engine.SetCrafterCraftTimeMultiplier(id =>
                        id == sv ? MathfCompat.Max(0.1f, 1f / MathfCompat.Max(0.1f, mult)) : 1f);
                },
                applyCraftingPenaltyFactor: (sv, factor) =>
                {
                    if (_crafting == null) return;
                    _crafting.Engine.SetCrafterCraftTimeMultiplier(id =>
                        id == sv ? 1f + MathfCompat.Max(0f, factor) : 1f);
                },
                applyCombatPenaltyFactor: (sv, factor) =>
                {
                    if (_expeditions == null) return;
                    _expeditions.Engine.SetStaminaDrainMultiplier(id =>
                        id == sv ? 1f + MathfCompat.Max(0f, factor) : 1f);
                },
                applyStaminaDrainMultiplier: (sv, factor) =>
                {
                    if (_expeditions == null) return;
                    _expeditions.Engine.SetStaminaDrainMultiplier(id =>
                        id == sv ? 1f + MathfCompat.Max(0f, factor) : 1f);
                },
                fireNarrativeEvent: (narrativeId, sv) =>
                {
                    int day = _holdfastRuntime?.Day ?? _simDay;
                    string sourceId = $"{narrativeId}_{sv}_{day}";

                    // events.json is the prose authority for narrative event ids.
                    // Authored ids dispatch through the same catalog seam the
                    // wildlife bycatch beat uses (journal + codex unlock + HUD).
                    SetupEventsHost();
                    if (_eventsHost != null
                        && _eventsHost.TryGetEvent(narrativeId, out var authored)
                        && authored != null
                        && !string.IsNullOrWhiteSpace(authored.BodyText))
                    {
                        SetupEventAdapter();
                        _hostEventAdapter?.DispatchCatalogEvent(authored.Id, authored.BodyText, day, sourceId);
                        return;
                    }

                    // Unauthored id (narrative_final_wish_completed has no events.json
                    // row): keep the beat visible instead of dropping it silently.
                    GD.PushWarning($"[Phase0] narrative event '{narrativeId}' is not authored in events.json; writing placeholder journal entry.");
                    _journal.TryAddRawEntry(
                        sourceId,
                        $"{sv}: {narrativeId.Replace('_', ' ')}.",
                        author: null!,
                        day: day);
                },
                grantChronicIllness: (sv, afflictionId) =>
                {
                    var rad = _survivors.RadStateFor(sv);
                    if (rad != null && !rad.HasChronicIllness)
                    {
                        rad.HasChronicIllness = true;
                        SaveSurvivors();
                    }
                },
                resetRadiationDose: sv =>
                {
                    var rad = _survivors.RadStateFor(sv);
                    if (rad != null) _survivors.Radiation.SetDose(rad, 0f);
                },
                applyWorkRefusalHours: null);
            _phase0.ValidateConsumers();

            // Environment signals from the real world/shelter hosts.
            _phase0.CurrentDay = _simDay;
            _phase0.GetFilterHealth = () =>
            {
                var filter = _expansions?.Waystation?.State != null
                    ? _expansions.Waystation.State.filterHealth : 100f;
                return filter;
            };
            // Host flags: updated each tick from the real world/shelter state.
            _phase0.IsInFalloutStorm = _world != null && _world.Weather.Current == Ashfall.Core.WeatherKind.FalloutStorm;
            _phase0.IsNightTime = _world != null && _world.Weather.Current == Ashfall.Core.WeatherKind.BlackRain;

            var ids = new System.Collections.Generic.List<string>();
            for (int i = 0; i < _survivors.RosterState.Count; i++)
            {
                var s = _survivors.RosterState[i];
                if (s != null && s.IsAliveState) ids.Add(s.Id);
            }
            _phase0.RegisterSurvivors(ids);

            if (_shelterAssignment != null)
            {
                _phase0.BindShelterAssignment(_shelterAssignment.System);
            }

            var save = Phase0SaveStore.TryLoad();
            if (save != null)
            {
                _phase0.RestoreSave(save);
                _phase0Dirty = false; // restore just raised state-change events
                GD.Print("[Ashfall Godot] Phase-0 effects restored.");
            }

            // Bind the authored final-wish catalog so terminal prognoses draw from a
            // per-archetype pool and the panel can surface authored text. Safe to run
            // after restore: it only affects future DeclareTerminalPrognosis calls.
            _phase0.LoadFinalWishCatalog(_dataDir);
        }

        private void SavePhase0()
        {
            if (_phase0 == null) return;
            if (CaptureSection("phase0", Phase0SaveStore.TryCapturePersisted(_phase0.CaptureSave())))
            {
                _phase0Dirty = false;
                GD.Print("[Ashfall Godot] Phase-0 effects save written.");
            }
        }

        private void FlushPhase0IfDirty()
        {
            if (_phase0Dirty) SavePhase0();
        }

        /// <summary>
        /// Bridge a completed production craft into trade specialty progression.
        /// A player-assigned crafter is authoritative; an unassigned shelter craft
        /// falls back to a living survivor whose trade actually covers the item.
        /// A survivor whose profession resolves to no specialty has no tree to advance.
        /// </summary>
        private void OnCraftCompletedForSpecialty(Recipe recipe, string crafterId)
        {
            if (_phase0 == null || recipe?.result == null) return;

            string itemId = recipe.result.id;
            string survivorId = string.IsNullOrWhiteSpace(crafterId)
                ? AutoAssignSpecialtyCrafter(itemId)
                : crafterId;
            if (string.IsNullOrEmpty(survivorId)) return;

            string professionId = ResolveSurvivorProfessionId(survivorId);
            if (string.IsNullOrEmpty(professionId)) return;

            _phase0.CraftItem(survivorId, professionId, itemId);
        }

        /// <summary>
        /// Resolve a survivor's trade specialty id. An authored pre_war_profession_id
        /// wins; otherwise the roster profession label is matched against the
        /// authored profession_aliases in trade_specialties.json.
        /// </summary>
        private string ResolveSurvivorProfessionId(string survivorId, string? definitionId = null)
        {
            if (string.IsNullOrEmpty(survivorId)) return string.Empty;
            SetupEnrichment();
            string explicitId = _enrichment?.GetSurvivorFields(survivorId)?.pre_war_profession_id ?? string.Empty;
            string label = _survivors?.Roster?.FindDefinition(definitionId ?? survivorId)?.profession ?? string.Empty;
            return TradeSpecialtySystem.ResolveProfessionId(explicitId, label);
        }

        /// <summary>
        /// Attribute an unassigned craft to a living survivor whose trade covers the
        /// item. Deterministic: candidates are ordinal-sorted before the forked
        /// campaign RNG stream picks one, so a replay credits the same survivor and
        /// no wall-clock or hash-iteration order is involved. Empty when nobody's
        /// trade matches, which leaves the craft advancing no specialty.
        /// </summary>
        private string AutoAssignSpecialtyCrafter(string itemId)
        {
            var roster = _survivors?.Roster;
            if (roster == null || string.IsNullOrEmpty(itemId)) return string.Empty;

            var candidates = new List<string>();
            for (int i = 0; i < roster.Roster.Count; i++)
            {
                var entry = roster.Roster[i];
                if (entry == null || !entry.isAlive || string.IsNullOrEmpty(entry.survivorId)) continue;
                string professionId = ResolveSurvivorProfessionId(entry.survivorId, entry.definitionId);
                if (string.IsNullOrEmpty(professionId)) continue;
                if (!TradeSpecialtySystem.ProfessionMatchesItem(professionId, itemId)) continue;
                candidates.Add(entry.survivorId);
            }

            if (candidates.Count == 0) return string.Empty;
            if (candidates.Count == 1) return candidates[0];

            candidates.Sort(StringComparer.Ordinal);
            var rng = _campaignDay?.Rng?.Fork("trade_specialty_attribution");
            return rng != null ? candidates[rng.Next(0, candidates.Count)] : candidates[0];
        }

        private void OnPhase0ScavengeClicked()
        {
            SetupPhase0();
            _statusLabel.Text = _phase0.ScavengeItem("survivor_gunner_mikhail", "item_dog_tags");
        }

        private void OnPhase0NoiseClicked()
        {
            SetupPhase0();
            _statusLabel.Text = _phase0.RaiseNoise("siren");
        }

        private void OnPhase0CraftClicked()
        {
            SetupPhase0();
            _statusLabel.Text = _phase0.CraftItem("elena_vasquez", "machinist", "wrench_standard");
        }

        private void OnPhase0TickClicked()
        {
            SetupPhase0();
            _statusLabel.Text = _phase0.TickHour(6f);
        }

        private void SetupDoseLedger()
        {
            if (_doseLedger != null) return;
            SetupCampaignDay();
            _doseLedger = DoseLedgerHostSession.Create(_dataDir, campaignRng: _campaignDay.Rng);
            _doseLedger.StateChanged += () => _doseLedgerDirty = true;

            // CORE-MECH W2: bind the live campaign day and the authored Year-of-Ash
            // fallout windows so radiation bookings are conditioned by the season.
            // The provider is read-only and pure; the dose ledger keeps owning every
            // reading rule (AA.2 receiver contract).
            _doseLedger.DayProvider = () => _simDay;
            try
            {
                var catalogPath = CatalogPath.ResolveCatalog("year_of_ash_events.json");
                var catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
                if (catalogIo.FileExists(catalogPath))
                {
                    var yoaEvents = YearOfAshCatalogLoader.LoadEvents(
                        CatalogPath.ResolveDataDir(), catalogIo, new SystemTextJsonSerializer());
                    _doseLedger.FalloutWindowProviderRef = new FalloutWindowProvider(yoaEvents);
                }
            }
            catch (Exception ex)
            {
                // Fail-closed: no calendar ⇒ neutral multiplier (1.0), never a crash
                // and never a silently reduced exposure.
                GD.Print("[Ashfall Godot] Dose fallout windows unavailable: " + ex.Message);
            }

            var save = DoseLedgerSaveStore.TryLoad();
            if (save != null)
            {
                _doseLedger.RestoreSave(save);
                _doseLedgerDirty = false; // restore just raised state-change events
                GD.Print("[Ashfall Godot] Dose Ledger state restored.");
            }

            if (_doseSurface == null && _rightColumn != null)
            {
                _doseSurface = new DoseRegisterSurface();
                _rightColumn.AddChild(_doseSurface);
            }
            if (_doseSurface != null)
            {
                _doseSurface.BindSession(_doseLedger);
                _doseSurface.RefreshView();
            }
        }

        private void OnDoseRegisterClicked()
        {
            SetupDoseLedger();
            _statusLabel.Text = "The Dose Register is open. Four tabs, four people who keep books.";
        }

        private void OnDoseSealClicked()
        {
            SetupDoseLedger();
            _doseLedger.SealDemoSurvivors();
            _statusLabel.Text = "Dosimeters sealed: Gunner Mikhail (tag_1), Elena Vasquez (tag_2).";
            _codexViewer.Text = _doseLedger.DoseStatusLine();
            FlushDoseLedgerIfDirty();
        }

        private void OnDoseScribeClicked()
        {
            SetupDoseLedger();
            string result = _doseLedger.ScribeReading(180f, highEnergy: false);
            _statusLabel.Text = result;
            _codexViewer.Text = _doseLedger.DoseStatusLine();
            FlushDoseLedgerIfDirty();
        }

        private void OnDoseDiagnoseClicked()
        {
            SetupDoseLedger();
            string result = _doseLedger.DiagnoseDemo(DoseLedgerSystem.BandRed);
            _statusLabel.Text = result;
            _codexViewer.Text = _doseLedger.DoseStatusLine();
            FlushDoseLedgerIfDirty();
        }

        private void OnDoseCohortClicked()
        {
            SetupDoseLedger();
            string result = _doseLedger.BookDemoChild();
            _statusLabel.Text = result;
            _codexViewer.Text = _doseLedger.DoseStatusLine();
            FlushDoseLedgerIfDirty();
        }

        private void OnDoseVolunteerClicked()
        {
            SetupDoseLedger();
            string result = _doseLedger.SignDemoVolunteer();
            _statusLabel.Text = result;
            _codexViewer.Text = _doseLedger.DoseStatusLine();
            FlushDoseLedgerIfDirty();
        }

        private void SaveDoseLedger()
        {
            if (_doseLedger == null) return;
            int day = _core != null ? _core.Clock.Day : _simDay;
            if (CaptureSection("dose_ledger", DoseLedgerSaveStore.TryCapturePersisted(_doseLedger.CaptureSave(day))))
            {
                _doseLedgerDirty = false;
                GD.Print($"[Ashfall Godot] Dose Ledger save written (day {day}).");
            }
        }

        private void FlushDoseLedgerIfDirty()
        {
            if (_doseLedgerDirty) SaveDoseLedger();
        }

        private void ClosePhase0Panel()
        {
            _phase0Panel.Visible = false;
        }

    }
}
