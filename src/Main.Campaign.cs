// SPDX-License-Identifier: MIT
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Memorial;
using Ashfall.Core.Survivors;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;
using AtomicWar.GodotApp.YearOfAsh;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private CampaignDayCoordinator _campaignDay = null!;
        private DailyBriefingState _dailyBriefing = null!;
        private DailyBriefingModal _dailyBriefingModal = null!;
        private bool _briefingPending;
        private bool _dailyBriefingDirty;
        private bool _campaignDayDirty;
        private Ashfall.Core.Memorial.MemorialSystem _memorial = null!;
        private bool _memorialDirty;
        /// <summary>Plan 24C (A3) — the mourning vigil's attributed morale
        /// recovery per living survivor (restrained: recovers less than half
        /// of the shelter-wide grief hit).</summary>
        private const float MourningMoraleRecovery = 3f;

        private void SetupCampaignDay()
        {
            if (_campaignDay != null) return;
            _campaignDay = new CampaignDayCoordinator();
            RegisterProductionCampaignOwners();
            _dailyBriefing = new DailyBriefingState();
            LoadDailyBriefing();
            var loadedCampaignDay = CampaignDaySaveStore.TryLoad();
            if (loadedCampaignDay != null)
            {
                ReconcileLegacySectionDays(loadedCampaignDay);
                _campaignDay.RestoreState(loadedCampaignDay);
            }
        }

        /// <summary>
        /// Task #112 substeps 7/8: legacy sections carry their own day values;
        /// before the coordinator adopts the campaign_day section, reconcile
        /// every persisted day source, adopt the authoritative winner (and
        /// upgrade an older campaign_day envelope when a later section saw
        /// more days), and report every mismatch via [CALENDAR_MISMATCH]
        /// instead of silently trusting whichever section loads last.
        /// </summary>
        private void ReconcileLegacySectionDays(CampaignDaySave loadedCampaignDay)
        {
            try
            {
                var sectionDays = new Dictionary<string, int>();
                if (loadedCampaignDay.lastAdvancedDay > 0)
                    sectionDays["campaign_day"] = loadedCampaignDay.lastAdvancedDay;

                var holdfastSave = HoldfastSaveStore.TryLoad();
                if (holdfastSave != null && holdfastSave.simDay > 0)
                    sectionDays["holdfast"] = holdfastSave.simDay;

                var rosterSave = DutyRosterSaveStore.TryLoad();
                if (rosterSave != null && rosterSave.simDay > 0)
                    sectionDays["duty_roster"] = rosterSave.simDay;

                var economySave = EconomySaveStore.TryLoad();
                if (economySave != null && economySave.day > 0)
                    sectionDays["economy"] = economySave.day;

                var yoaSave = YearOfAshSaveStore.TryLoad();
                if (yoaSave != null && yoaSave.timeline != null && yoaSave.timeline.currentDay > 0)
                    sectionDays["year_of_ash"] = yoaSave.timeline.currentDay;

                if (sectionDays.Count == 0) return;

                var result = CampaignCalendarReconciler.Reconcile(sectionDays, new GodotLog());

                // Adopt the reconciled day when it is AHEAD of the stored
                // campaign_day value (never rewind a newer envelope).
                if (result.AuthoritativeDay > loadedCampaignDay.lastAdvancedDay)
                {
                    GD.Print($"[Ashfall Godot] Calendar reconciled to day {result.AuthoritativeDay} " +
                             $"(source: {result.PrimarySource}; stored campaign_day was {loadedCampaignDay.lastAdvancedDay}).");
                    loadedCampaignDay.lastAdvancedDay = result.AuthoritativeDay;
                }
            }
            catch (Exception ex)
            {
                GD.PushWarning("[Ashfall Godot] Calendar reconciliation skipped: " + ex.Message);
            }
        }

        private void SetupDailyBriefingModal()
        {
            if (_dailyBriefingModal != null) return;
            _dailyBriefingModal = PanelSceneLoader.Load<DailyBriefingModal>("res://assets/ui/modals/DailyBriefingModal.tscn");
            _dailyBriefingModal.OnAcknowledged += OnBriefingAcknowledged;
            _dailyBriefingModal.OnDeepLinkRequested += HandleBriefingDeepLink;
            AddChild(_dailyBriefingModal);
            _dailyBriefingModal.Hide();
        }

        private void HandleBriefingDeepLink(string route)
        {
            if (string.IsNullOrEmpty(route)) return;
            if (_dailyBriefingModal != null && _dailyBriefingModal.Visible)
            {
                _dailyBriefingModal.Hide();
            }

            if (route.StartsWith("panel:", StringComparison.OrdinalIgnoreCase))
            {
                string panelSpec = route.Substring("panel:".Length);
                string panelId = panelSpec;
                int qIdx = panelSpec.IndexOf('?');
                if (qIdx >= 0)
                {
                    panelId = panelSpec.Substring(0, qIdx);
                }

                // Plan 31B.8 — validate the route target is still live before
                // navigating; a shelved/unregistered target stays informational.
                var descriptor = PanelRegistry.Get(panelId);
                if (descriptor == null || !descriptor.IsPlayerNavigable)
                {
                    GD.PushWarning($"[Briefing] deep-link target '{panelId}' is not a live player panel; staying informational.");
                    return;
                }

                OpenPlayerPanel(panelId);
            }
        }

        private void LoadDailyBriefing()
        {
            try
            {
                var loaded = DailyBriefingSaveStore.TryLoad();
                if (loaded != null) _dailyBriefing.RestoreState(loaded);
            }
            catch (Exception e)
            {
                GD.PushWarning("[Ashfall Godot] DailyBriefing load failed: " + e.Message);
                _dailyBriefing = new DailyBriefingState();
            }
        }

        private void SaveDailyBriefing()
        {
            if (_dailyBriefing == null) return;
            try
            {
                var save = _dailyBriefing.CaptureState();
                if (CaptureSection("daily_briefing", DailyBriefingSaveStore.TryCapturePersisted(save))) _dailyBriefingDirty = false;
            }
            catch (Exception e)
            {
                GD.PushWarning("[Ashfall Godot] DailyBriefing save failed: " + e.Message);
            }
        }

        private void SaveCampaignDay()
        {
            if (_campaignDay == null) return;
            try
            {
                var save = _campaignDay.CaptureState();
                if (CaptureSection("campaign_day", CampaignDaySaveStore.TryCapturePersisted(save))) _campaignDayDirty = false;
            }
            catch (Exception e)
            {
                GD.PushWarning("[Ashfall Godot] CampaignDay save failed: " + e.Message);
            }
        }

        private void FlushCampaignDayIfDirty()
        {
            if (_campaignDayDirty) SaveCampaignDay();
        }

        private void OnBriefingAcknowledged(int day)
        {
            _briefingPending = false;
            if (_dailyBriefing == null) return;
            _dailyBriefing.Consume(day);
            _dailyBriefingDirty = true;
            if (_dailyBriefingDirty) SaveDailyBriefing();
            UpdateHud();

            // A narrative arc selected during the day tick is presented after
            // the briefing closes, keeping the choice modal on the normal
            // player path while preserving selection/execution separation.
            if (_narrative != null && _narrative.PendingArcEvent != null)
                OpenNarrativeArcModal();
            else
            {
                SetupEchoes();
                if (_echoes?.PendingEcho != null)
                    OpenEchoModal();
            }
        }

        private void SetupMemorial()
        {
            if (_memorial != null) return;
            _memorial = new Ashfall.Core.Memorial.MemorialSystem(
                new Ashfall.Core.Memorial.MemorialState());
            _memorial.OnMemorialized += _ => _memorialDirty = true;
            _memorial.OnMemorialized += OnMemorializedForShelterDecor;
            // Plan 178/190: the same committed death record enters the culture
            // vault chronicle (one creation command; vault owns append/dedup).
            _memorial.OnMemorialized += OnMemorializedForArchiveChronicle;
            // Plan 24C (A3): the mourning vigil eases the shelter's grief —
            // one attributed morale recovery per living survivor, exactly once
            // per deceased (the entry's persisted MournedDay is the gate),
            // plus one restrained journal line.
            _memorial.OnMourned += entry =>
            {
                _memorialDirty = true;
                if (_survivors?.Needs != null)
                {
                    var living = _survivors.Needs.Registered;
                    for (int i = 0; i < living.Count; i++)
                    {
                        var s = living[i];
                        if (s == null || !s.IsAliveState) continue;
                        _survivors.Needs.ApplyAttributedDelta(
                            s.Id, NeedKind.Morale, MourningMoraleRecovery,
                            "memorial.mourning");
                    }
                }
                _journal?.TryAddRawEntry(
                    "memorial_mourned_" + entry?.SurvivorId,
                    "The shelter held a vigil for "
                    + FormatSurvivorName(entry?.SurvivorId ?? string.Empty)
                    + ". For a moment, the weight eased.",
                    null!, _simDay);
            };
            LoadMemorial();
        }

        /// <summary>
        /// The memorial system remains the death-record authority. This host
        /// event only projects a newly committed record to the dedicated decor
        /// wall, where the projection is independently persisted with the room
        /// placements. If decor has not been initialized yet, its setup pass
        /// reconciles the entry later.
        /// </summary>
        private void OnMemorializedForShelterDecor(MemorialEntry entry)
        {
            if (_shelterDecor == null || entry == null) return;
            if (!_shelterDecor.TryMountMemorialPlaque(entry, out var reason))
                GD.PushWarning("[Ashfall Godot] Memorial plaque mount skipped: " + reason);
        }

        private void LoadMemorial()
        {
            try
            {
                var loaded = MemorialSaveStore.TryLoad();
                if (loaded != null) _memorial.RestoreState(loaded.State);
            }
            catch (Exception e)
            {
                GD.PushWarning("[Ashfall Godot] Memorial load failed: " + e.Message);
            }
        }

        private void SaveMemorial()
        {
            if (_memorial == null) return;
            try
            {
                var save = new Ashfall.Core.Memorial.MemorialSave
                {
                    simDay = _simDay,
                    State = _memorial.CaptureState()
                };
                if (CaptureSection("memorial", MemorialSaveStore.TryCapturePersisted(save)))
                    _memorialDirty = false;
            }
            catch (Exception e)
            {
                GD.PushWarning("[Ashfall Godot] Memorial save failed: " + e.Message);
            }
        }

        /// <summary>
        /// Builds the authoritative daily briefing and shows the modal.
        /// Derives entries directly from DayAdvancedEventArgs if present, or
        /// from canonical host state on fallback.
        /// </summary>
        private void ShowBriefingForDay(int day, DayAdvancedEventArgs? args = null)
        {
            // Plan 31C — opt-in replayable day record (dev-only, off by default).
            AppendDayRecordIfEnabled(day, args);
            SetupDailyBriefingModal();
            SetupSurvivors();
            SetupInventory();
            SetupPowerGrid();

            DailyBriefingReport report;

            if (args != null && args.AllEvents().Any())
            {
                report = DailyBriefingReportBuilder.BuildFromDayEvents(day, day, args.AllEvents());

                // C2 / Plan 17A-S §6.8 — owner failures must be visible in the
                // briefing, never presented as a quiet "nothing happened" day.
                if (args.HasFailures)
                {
                    var failed = args.FailedReports;
                    var names = new List<string>();
                    foreach (var f in failed)
                    {
                        if (f != null && !string.IsNullOrEmpty(f.OwnerId)) names.Add(f.OwnerId);
                    }
                    string ownerList = string.Join(", ", names);
                    report.Sections.Add(new DailyBriefingSection(
                        "Warnings",
                        new[]
                        {
                            new DailyBriefingEntry(
                                "Warnings",
                                "day_advance_incomplete",
                                $"Day {day} advance incomplete — {names.Count} system update(s) failed ({ownerList}). Some reported effects may be missing.",
                                order: -1)
                        }));
                }
            }
            else
            {
                var survivorChanges = new List<DailyBriefingEntry>();
                var resourceConsumption = new List<DailyBriefingEntry>();
                var weatherForecast = new List<DailyBriefingEntry>();
                var radioIntercepts = new List<DailyBriefingEntry>();
                var expeditionMilestones = new List<DailyBriefingEntry>();
                var deaths = new List<DailyBriefingEntry>();
                var warnings = new List<DailyBriefingEntry>();

                if (_survivors?.RosterState != null)
                {
                    for (int i = 0; i < _survivors.RosterState.Count; i++)
                    {
                        var s = _survivors.RosterState[i];
                        if (s == null) continue;

                        string name = s.Id;
                        if (!s.IsAlive)
                        {
                            deaths.Add(new DailyBriefingEntry("Deaths", s.Id, $"{name} has perished in the holdfast.", order: i));
                            continue;
                        }

                        var rad = _survivors.RadStateFor(s.Id);
                        float dose = rad?.RadiationDose ?? 0f;

                        if (s.Hunger >= 80f)
                            warnings.Add(new DailyBriefingEntry("Warnings", s.Id, $"{name} is starving ({s.Hunger:F0}% hunger).", order: i, numeric: s.Hunger));
                        else if (s.Hunger >= 40f)
                            survivorChanges.Add(new DailyBriefingEntry("Survivor Changes", s.Id, $"{name} is hungry ({s.Hunger:F0}% hunger).", order: i, numeric: s.Hunger));

                        if (s.Thirst >= 80f)
                            warnings.Add(new DailyBriefingEntry("Warnings", s.Id, $"{name} is severely dehydrated ({s.Thirst:F0}% thirst).", order: i, numeric: s.Thirst));
                        else if (s.Thirst >= 40f)
                            survivorChanges.Add(new DailyBriefingEntry("Survivor Changes", s.Id, $"{name} is thirsty ({s.Thirst:F0}% thirst).", order: i, numeric: s.Thirst));

                        if (dose >= 50f)
                            warnings.Add(new DailyBriefingEntry("Warnings", s.Id, $"{name} has dangerous radiation exposure ({dose:F0} mSv).", order: i, numeric: dose));
                        else if (dose >= 15f)
                            survivorChanges.Add(new DailyBriefingEntry("Survivor Changes", s.Id, $"{name} has accumulated radiation ({dose:F0} mSv).", order: i, numeric: dose));

                        if (s.Health < 60f)
                            warnings.Add(new DailyBriefingEntry("Warnings", s.Id, $"{name} is in critical condition (HP {s.Health:F0}%).", order: i, numeric: s.Health));
                        else if (s.Health < 90f)
                            survivorChanges.Add(new DailyBriefingEntry("Survivor Changes", s.Id, $"{name} is injured (HP {s.Health:F0}%).", order: i, numeric: s.Health));
                    }
                }

                if (_inventory?.Inventory != null)
                {
                    int food = _inventory.Inventory.CountById("canned_food");
                    int water = _inventory.Inventory.CountById("clean_water");
                    int fuel = _inventory.Inventory.CountById("fuel_canister");

                    resourceConsumption.Add(new DailyBriefingEntry("Resource Consumption", "canned_food", $"Canned Food in stock: {food}", order: 0, numeric: food));
                    resourceConsumption.Add(new DailyBriefingEntry("Resource Consumption", "clean_water", $"Clean Water in stock: {water}", order: 1, numeric: water));
                    resourceConsumption.Add(new DailyBriefingEntry("Resource Consumption", "fuel_canister", $"Fuel Canisters in stock: {fuel}", order: 2, numeric: fuel));

                    if (food <= 2) warnings.Add(new DailyBriefingEntry("Warnings", "canned_food", "Food reserves are critically low!", order: 1));
                    if (water <= 2) warnings.Add(new DailyBriefingEntry("Warnings", "clean_water", "Water reserves are critically low!", order: 2));
                }

                if (_powerGrid?.System != null)
                {
                    resourceConsumption.Add(new DailyBriefingEntry("Resource Consumption", "power_grid", $"Generator fuel: {_powerGrid.System.FuelUnits:F0} units (Reserve: {_powerGrid.System.BatteryReserveWh:F0} Wh)", order: 3));
                    if (_powerGrid.System.FuelUnits <= 5f)
                        warnings.Add(new DailyBriefingEntry("Warnings", "power_grid", "Generator fuel almost exhausted!", order: 3));
                }

                // B5–B8 expansion (§27 machine feedback): the shelter's
                // condition-bearing machines in one briefing section — degraded
                // subsystems are visible without opening four panels.
                foreach (var machine in Ashfall.Core.ShelterMachineryReport.Build(
                    _powerGrid?.System, _deepWell?.System, _waterCondenser?.System,
                    _waterTreatment?.System, _sumpFlooding?.System))
                {
                    resourceConsumption.Add(new DailyBriefingEntry(
                        "Machinery Condition", machine.MachineId,
                        $"{machine.Label}: {machine.Detail} ({Ashfall.Core.ShelterMachineryReport.LabelFor(machine.StatusBand)})",
                        order: 4));
                    if (machine.StatusBand == Ashfall.Core.ShelterMachineryReport.Band.Critical
                        || machine.StatusBand == Ashfall.Core.ShelterMachineryReport.Band.Offline)
                        warnings.Add(new DailyBriefingEntry("Warnings", machine.MachineId,
                            $"{machine.Label} is {Ashfall.Core.ShelterMachineryReport.LabelFor(machine.StatusBand)}: {machine.Detail}", order: 4));
                }

                if (_world?.Weather != null)
                {
                    weatherForecast.Add(new DailyBriefingEntry("Weather Forecast", "surface_weather",
                        $"Surface condition: {_world.Weather.Current} ({_world.Weather.State.currentKind})", order: 0));
                }

                if (_expeditions?.Engine != null)
                {
                    var active = _expeditions.Engine.CaptureState();
                    if (active != null && active.Count > 0)
                    {
                        for (int i = 0; i < active.Count; i++)
                        {
                            var ex = active[i];
                            if (ex == null) continue;
                            string expId = !string.IsNullOrEmpty(ex.expeditionId) ? ex.expeditionId : (!string.IsNullOrEmpty(ex.displayName) ? ex.displayName : $"expedition_{i + 1}");
                            expeditionMilestones.Add(new DailyBriefingEntry("Expedition Milestones", expId, $"Expedition {expId} ({ex.survivorId}): Phase {(ExpeditionPhase)ex.phase} at {ex.locationId}.", order: i));
                        }
                    }
                }

                var inputs = new DailyBriefingInputs
                {
                    Day = day,
                    GeneratedUtc = string.Empty,
                    BuildSeed = day,
                    SurvivorChanges = survivorChanges,
                    ResourceConsumption = resourceConsumption,
                    WeatherForecast = weatherForecast,
                    RadioIntercepts = radioIntercepts,
                    ExpeditionMilestones = expeditionMilestones,
                    Deaths = deaths,
                    Warnings = warnings
                };

                report = DailyBriefingReportBuilder.Build(inputs);
            }

            // C1.4 deferred consumer — surface the authoritative crisis
            // predictions (food/water/power/radiation/disease/weather) in the
            // briefing. Empty predictions leave the report untouched.
            DailyBriefingReportBuilder.AppendCrisisWarnings(report, BuildBriefingCrisisPredictions(day));

            if (report.IsEmpty) return;
            _dailyBriefing.Enqueue(report);
            _dailyBriefingDirty = true;
            SaveDailyBriefing();
            _briefingPending = true;
            _dailyBriefingModal.Show(report);
        }
    }
}
