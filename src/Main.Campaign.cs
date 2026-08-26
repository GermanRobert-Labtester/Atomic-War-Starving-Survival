using Godot;
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Survivors;
using AtomicWar.GodotApp.UI;

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

        private void SetupCampaignDay()
        {
            if (_campaignDay != null) return;
            _campaignDay = new CampaignDayCoordinator();
            _dailyBriefing = new DailyBriefingState();
            LoadDailyBriefing();
            var loadedCampaignDay = CampaignDaySaveStore.TryLoad();
            if (loadedCampaignDay != null)
            {
                _campaignDay.RestoreState(loadedCampaignDay);
            }
        }

        private void SetupDailyBriefingModal()
        {
            if (_dailyBriefingModal != null) return;
            _dailyBriefingModal = new DailyBriefingModal();
            _dailyBriefingModal.OnAcknowledged += OnBriefingAcknowledged;
            AddChild(_dailyBriefingModal);
            _dailyBriefingModal.Hide();
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
                if (DailyBriefingSaveStore.TrySave(save)) _dailyBriefingDirty = false;
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
                if (CampaignDaySaveStore.TrySave(save)) _campaignDayDirty = false;
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
        }

        private void SetupMemorial()
        {
            if (_memorial != null) return;
            _memorial = new Ashfall.Core.Memorial.MemorialSystem(
                new Ashfall.Core.Memorial.MemorialState());
            _memorial.OnMemorialized += _ => _memorialDirty = true;
            LoadMemorial();
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
                if (MemorialSaveStore.TrySave(save))
                    _memorialDirty = false;
            }
            catch (Exception e)
            {
                GD.PushWarning("[Ashfall Godot] Memorial save failed: " + e.Message);
            }
        }

        /// <summary>
        /// Builds the typed <see cref="DailyBriefingInputs"/> snapshot and shows
        /// the briefing modal. Blocks further simulation until acknowledged.
        /// </summary>
        private void ShowBriefingForDay(int day)
        {
            SetupDailyBriefingModal();
            SetupSurvivors();
            SetupInventory();
            SetupPowerGrid();

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
                        survivorChanges.Add(new DailyBriefingEntry("Survivor", s.Id, $"{name} is starving ({s.Hunger:F0}% hunger).", order: i, numeric: s.Hunger));
                    else if (s.Hunger >= 40f)
                        survivorChanges.Add(new DailyBriefingEntry("Survivor", s.Id, $"{name} is hungry ({s.Hunger:F0}% hunger).", order: i, numeric: s.Hunger));

                    if (s.Thirst >= 80f)
                        survivorChanges.Add(new DailyBriefingEntry("Survivor", s.Id, $"{name} is severely dehydrated ({s.Thirst:F0}% thirst).", order: i, numeric: s.Thirst));
                    else if (s.Thirst >= 40f)
                        survivorChanges.Add(new DailyBriefingEntry("Survivor", s.Id, $"{name} is thirsty ({s.Thirst:F0}% thirst).", order: i, numeric: s.Thirst));

                    if (dose >= 50f)
                        warnings.Add(new DailyBriefingEntry("Warning", s.Id, $"{name} has dangerous radiation exposure ({dose:F0} mSv).", order: i, numeric: dose));
                    else if (dose >= 15f)
                        survivorChanges.Add(new DailyBriefingEntry("Survivor", s.Id, $"{name} has accumulated radiation ({dose:F0} mSv).", order: i, numeric: dose));

                    if (s.Health < 60f)
                        warnings.Add(new DailyBriefingEntry("Warning", s.Id, $"{name} is in critical condition (HP {s.Health:F0}%).", order: i, numeric: s.Health));
                    else if (s.Health < 90f)
                        survivorChanges.Add(new DailyBriefingEntry("Survivor", s.Id, $"{name} is injured (HP {s.Health:F0}%).", order: i, numeric: s.Health));
                    else
                        survivorChanges.Add(new DailyBriefingEntry("Survivor", s.Id, $"{name} is stable (HP {s.Health:F0}%).", order: i, numeric: s.Health));
                }
            }

            if (_inventory?.Inventory != null)
            {
                int food = _inventory.Inventory.CountById("item_canned_food");
                int water = _inventory.Inventory.CountById("item_purified_water");
                int fuel = _inventory.Inventory.CountById("item_fuel_canister");

                resourceConsumption.Add(new DailyBriefingEntry("Resource", "item_canned_food", $"Canned Food in stock: {food}", order: 0, numeric: food));
                resourceConsumption.Add(new DailyBriefingEntry("Resource", "item_purified_water", $"Purified Water in stock: {water}", order: 1, numeric: water));
                resourceConsumption.Add(new DailyBriefingEntry("Resource", "item_fuel_canister", $"Fuel Canisters in stock: {fuel}", order: 2, numeric: fuel));

                if (food <= 2) warnings.Add(new DailyBriefingEntry("Warning", "food", "Food reserves are critically low!", order: 1));
                if (water <= 2) warnings.Add(new DailyBriefingEntry("Warning", "water", "Water reserves are critically low!", order: 2));
            }

            if (_powerGrid?.System != null)
            {
                resourceConsumption.Add(new DailyBriefingEntry("Resource", "power_grid", $"Generator fuel: {_powerGrid.System.FuelUnits:F0} units (Reserve: {_powerGrid.System.BatteryReserveWh:F0} Wh)", order: 3));
                if (_powerGrid.System.FuelUnits <= 5f)
                    warnings.Add(new DailyBriefingEntry("Warning", "power", "Generator fuel almost exhausted!", order: 3));
            }

            if (_world?.Weather != null)
            {
                weatherForecast.Add(new DailyBriefingEntry("Weather", "surface_conditions",
                    $"Surface condition: {_world.Weather.Current} ({_world.Weather.State.currentKind})", order: 0));
            }

            radioIntercepts.Add(new DailyBriefingEntry("Radio", "emergency_band", "Emergency radio scanner active. Monitoring regional frequencies.", order: 0));

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
                        expeditionMilestones.Add(new DailyBriefingEntry("Expeditions", expId, $"Expedition {expId} ({ex.survivorId}): Phase {(ExpeditionPhase)ex.phase} at {ex.locationId}.", order: i));
                    }
                }
                else
                {
                    expeditionMilestones.Add(new DailyBriefingEntry("Expeditions", "none", "No active wasteland exploration parties.", order: 0));
                }
            }

            var inputs = new DailyBriefingInputs
            {
                Day = day,
                GeneratedUtc = DateTime.UtcNow.ToString("o"),
                BuildSeed = day,
                SurvivorChanges = survivorChanges,
                ResourceConsumption = resourceConsumption,
                WeatherForecast = weatherForecast,
                RadioIntercepts = radioIntercepts,
                ExpeditionMilestones = expeditionMilestones,
                Deaths = deaths,
                Warnings = warnings
            };

            var report = DailyBriefingReportBuilder.Build(inputs);
            if (report.IsEmpty) return;
            _dailyBriefing.Enqueue(report);
            _dailyBriefingDirty = true;
            SaveDailyBriefing();
            _briefingPending = true;
            _dailyBriefingModal.Show(report);
        }
    }
}
