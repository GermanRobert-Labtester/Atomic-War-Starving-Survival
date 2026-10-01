// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core.UI;
using Ashfall.Core.World;
using Ashfall.Core.Radiation;
using Ashfall.Core.Survivors;
using Ashfall.Core.Expeditions;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Status panel.
    /// Shows overall game status, day counter, current objectives, and quick stats
    /// using tactile 9-slice card framing and status badge icons.
    /// Bound to live host sessions (Survivors, Weather, Power Grid, Inventory);
    /// unbound systems render Tr("ui.status.not_monitored", "NOT MONITORED") instead of fabricated data.
    /// </summary>
    public partial class StatusPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _dayInfo = null!;
        private VBoxContainer _objectivesList = null!;
        private VBoxContainer _statsData = null!;
        private VBoxContainer _statusData = null!;
        private VBoxContainer _thresholdData = null!;
        private VBoxContainer _thermalData = null!;
        private VBoxContainer _forecastData = null!;

        private SurvivorsHostSession? _survivors;
        private WeatherSystem? _weather;
        private PowerGridHostSession? _power;
        private InventoryHostSession? _inventory;
        private ClothingWarmthHostSession? _clothingWarmth;
        private ShelterThermalHostSession? _shelterThermal;
        private IReadOnlyList<Ashfall.Core.Campaign.SupplyForecastDay>? _supplyForecast;
        private HealthHistoryHostSession? _healthHistory;
        private int _simDay = 1;

        /// <summary>True when at least one live session is bound.</summary>
        public bool IsBound => _survivors != null || _weather != null || _power != null;

        /// <summary>Live day-info rows rendered by the last refresh.</summary>
        public int RenderedDayInfoCount { get; private set; }

        public void Bind(
            SurvivorsHostSession? survivors = null,
            WeatherSystem? weather = null,
            PowerGridHostSession? power = null,
            InventoryHostSession? inventory = null,
            int simDay = 1,
            ClothingWarmthHostSession? clothingWarmth = null,
            ShelterThermalHostSession? shelterThermal = null,
            IReadOnlyList<Ashfall.Core.Campaign.SupplyForecastDay>? supplyForecast = null,
            HealthHistoryHostSession? healthHistory = null)
        {
            // Live refresh (UI/UX audit silent-failure sweep): rebound sessions
            // are detached first, then the open status view tracks state changes
            // instead of freezing at open time.
            if (_survivors != null) _survivors.StateChanged -= RefreshView;
            if (_weather != null) _weather.OnStateChanged -= OnWeatherStateChanged;
            if (_power != null) _power.OnStateChanged -= RefreshView;
            if (_inventory != null) _inventory.StateChanged -= RefreshView;
            if (_clothingWarmth != null) _clothingWarmth.StateChanged -= RefreshView;
            if (_healthHistory != null) _healthHistory.StateChanged -= RefreshView;

            _survivors = survivors;
            _weather = weather;
            _power = power;
            _inventory = inventory;
            _simDay = simDay;
            _clothingWarmth = clothingWarmth;
            _shelterThermal = shelterThermal;
            _supplyForecast = supplyForecast;
            _healthHistory = healthHistory;

            if (_survivors != null) _survivors.StateChanged += RefreshView;
            if (_weather != null) _weather.OnStateChanged += OnWeatherStateChanged;
            if (_power != null) _power.OnStateChanged += RefreshView;
            if (_inventory != null) _inventory.StateChanged += RefreshView;
            if (_clothingWarmth != null) _clothingWarmth.StateChanged += RefreshView;
            if (_healthHistory != null) _healthHistory.StateChanged += RefreshView;
            RefreshView();
        }

        private void OnWeatherStateChanged(Ashfall.Core.World.WorldWeatherState _) => RefreshView();

        public void RefreshView()
        {
            if (_dayInfo == null || _objectivesList == null || _statsData == null || _statusData == null || _thresholdData == null || _thermalData == null || _forecastData == null) return;

            AshfallUiHelpers.EmptyChildren(_dayInfo);
            AshfallUiHelpers.EmptyChildren(_objectivesList);
            AshfallUiHelpers.EmptyChildren(_statsData);
            AshfallUiHelpers.EmptyChildren(_statusData);
            AshfallUiHelpers.EmptyChildren(_thresholdData);
            AshfallUiHelpers.EmptyChildren(_thermalData);
            AshfallUiHelpers.EmptyChildren(_forecastData);

            RenderDayInfo();
            RenderObjectives();
            RenderStats();
            RenderExpeditionInjuries();
            RenderSystemStatus();
            RenderThresholds();
            RenderThermal();
            RenderForecast();
        }

        private void RenderDayInfo()
        {
            int rows = 0;

            _dayInfo.AddChild(AshfallUiHelpers.MakeDataRow(Tr("ui.status.day.current_day", "Current Day"), TrFormat("ui.status.day.value", _simDay),
                AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));
            rows++;

            if (_survivors?.RosterState != null && _survivors.RosterState.Count > 0)
            {
                int total = _survivors.RosterState.Count;
                int alive = _survivors.RosterState.Count(s => s != null && s.IsAlive);
                _dayInfo.AddChild(AshfallUiHelpers.MakeDataRow(Tr("ui.status.day.survivors", "Survivors"),
                    TrFormat("ui.status.day.alive", alive, total),
                    AshfallUiHelpers.ToColor(alive < total
                        ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Lethe)));
                rows++;
            }

            if (_weather != null)
            {
                var kind = _weather.Current;
                bool hazard = kind == Ashfall.Core.WeatherKind.FalloutStorm
                           || kind == Ashfall.Core.WeatherKind.BlackRain
                           || kind == Ashfall.Core.WeatherKind.Blizzard;
                string condition = hazard
                    ? Tr("ui.status.day.hazard", "HAZARD WATCH")
                    : Tr("ui.status.day.nominal", "Nominal");
                _dayInfo.AddChild(AshfallUiHelpers.MakeDataRow(Tr("ui.status.day.external_conditions", "External Conditions"),
                    TrFormat("ui.status.day.weather_value", kind, condition),
                    AshfallUiHelpers.ToColor(hazard
                        ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Lethe)));
                rows++;
            }
            else
            {
                _dayInfo.AddChild(AshfallUiHelpers.MakeDataRow(Tr("ui.status.day.external_conditions", "External Conditions"), Tr("ui.status.not_monitored", "NOT MONITORED"),
                    AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim)));
                rows++;
            }

            if (_power?.LastSnapshot != null)
            {
                var snap = _power.LastSnapshot;
                _dayInfo.AddChild(AshfallUiHelpers.MakeDataRow(Tr("ui.status.day.power_reserve", "Power Reserve"),
                    TrFormat("ui.status.day.battery", snap.BatteryReserveWh, snap.BatteryCapacityWh, snap.FuelUnits),
                    AshfallUiHelpers.ToColor(snap.IsBrownout
                        ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Lethe)));
                rows++;
            }

            RenderedDayInfoCount = rows;
        }

        private void RenderObjectives()
        {
            // Objectives are derived from live state — the panel never fabricates
            // quests. Critical conditions surface first; standing orders last.
            var objectives = new System.Collections.Generic.List<(string type, string text)>();

            if (_survivors?.RosterState != null)
            {
                // Task 3 (eighth wave) — the objective text says "critical health",
                // so it counts the critical band, not the wider warn band. The
                // shared predicate keeps this surface aligned with the roster's
                // CRITICAL label and the HUD chip.
                var healthProfile = _survivors.Needs.Profile;
                int critical = _survivors.RosterState
                    .Count(s => s != null && s.IsAlive && healthProfile.IsHealthCritical(s.Health));
                if (critical > 0)
                    objectives.Add(("PRIMARY",
                        TrFormat("ui.status.objective.critical_health", critical)));

                int dosed = _survivors.RosterState
                    .Count(s => s != null && s.IsAlive
                        && (_survivors.RadStateFor(s.Id)?.RadiationDose ?? 0f) >= RadiationSystem.WarnThreshold);
                if (dosed > 0)
                    objectives.Add(("PRIMARY",
                        TrFormat("ui.status.objective.dosed", dosed, $"{RadiationSystem.WarnThreshold:0}")));

                // P013 — the seeded day-1 acute-radiation start sits below the 50
                // mSv glance band, so flag the acute *status* explicitly (read
                // from the radiation owner) instead of letting it stay invisible.
                int acuteRad = _survivors.RosterState
                    .Count(s => s != null && s.IsAlive
                        && (_survivors.RadStateFor(s.Id)?.HasAcuteRadiationSickness ?? false));
                if (acuteRad > 0)
                    objectives.Add(("PRIMARY",
                        TrFormat("ui.status.objective.acute_rad", acuteRad)));
            }

            if (_power?.LastSnapshot is { IsBrownout: true })
                objectives.Add(("PRIMARY", Tr("ui.status.objective.brownout", "Power grid in brownout — shed load or add fuel")));

            if (_weather != null)
            {
                var kind = _weather.Current;
                if (kind == Ashfall.Core.WeatherKind.FalloutStorm
                    || kind == Ashfall.Core.WeatherKind.BlackRain)
                    objectives.Add(("DAILY", Tr("ui.status.objective.hazard_weather", "Hazard weather active — keep survivors indoors")));
            }

            if (_inventory?.Inventory != null)
            {
                int water = CountItem("clean_water", "item_clean_water") + CountItem("water_bottle");
                if (water <= 3)
                    objectives.Add(("SECONDARY", Tr("ui.status.objective.water_critical", "Water stores critical — purify or trade for clean water")));
            }

            objectives.Add(("STANDING", Tr("ui.status.objective.standing", "Keep the roster fed, hydrated, and below dose ceiling")));

            foreach (var obj in objectives)
            {
                var row = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
                var tag = AshfallUiHelpers.MakeSmall($"[{ObjectiveTag(obj.type)}]");
                tag.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                tag.CustomMinimumSize = new Vector2(90, 0);
                row.AddChild(tag);

                var desc = AshfallUiHelpers.MakeSmall(obj.text, true);
                desc.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
                row.AddChild(desc);
                _objectivesList.AddChild(row);
            }
        }

        private void RenderStats()
        {
            if (_survivors?.RosterState == null || _survivors.RosterState.Count == 0)
            {
                var none = AshfallUiHelpers.MakeSmall(Tr("ui.status.no_roster", "No survivor roster bound."));
                none.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
                _statsData.AddChild(none);
                return;
            }

            var roster = _survivors.RosterState.Where(s => s != null).ToList();
            int alive = roster.Count(s => s.IsAlive);
            float avgHealth = roster.Count > 0 ? roster.Average(s => s.Health) : 0f;
            float avgMorale = roster.Count > 0 ? roster.Average(s => s.Morale) : 0f;
            float avgDose = roster.Count > 0
                ? roster.Average(s => _survivors.RadStateFor(s.Id)?.RadiationDose ?? 0f)
                : 0f;

            // Task 1 — cohort labels resolve through the shared text helper so
            // the German catalog is not bypassed by hardcoded English rows.
            //
            // Task 9 — the dose and morale cards carry the same three-band
            // colour the HUD chips and the host toast use. The thresholds come
            // from the owning authorities (RadiationSystem / NeedsProfile), so a
            // card can never teach a band the simulation does not enforce.
            var profile = _survivors?.Needs?.Profile;
            // Task 7 — RenderDayInfo already raises the "survivors lost" fact in
            // Critical; the cohort card showed the same fact in neutral text.
            // One fact, one signal.
            AddStatRow(Tr("ui.status.cohort.survivors", "Survivor Cohort"),
                TrFormat("ui.status.cohort.alive_of_total", alive, roster.Count),
                "badge_exhaustion",
                alive < roster.Count ? AshfallUiBands.Critical : null);
            AddStatRow(Tr("ui.status.cohort.avg_health", "Average Health"), $"{avgHealth:0} / 100", "item_first_aid_kit",
                profile != null
                    ? AshfallUiBands.ForNeed(avgHealth, highIsBad: false, warnAt: profile.healthWarn, criticalAt: profile.healthCritical)
                    : null);
            AddStatRow(Tr("ui.status.cohort.morale", "Bunker Morale"), $"{avgMorale:0} / 100", "badge_guilt_insomnia",
                profile != null
                    ? AshfallUiBands.ForLow(avgMorale, warnAt: profile.moraleWarn, criticalAt: profile.moraleCritical)
                    : null);
            AddStatRow(Tr("ui.status.cohort.dose", "Dosimetry Dose"), $"{avgDose:0.0} mSv (avg)", "item_dosimeter_pen",
                AshfallUiBands.ForDose(avgDose));

            // P010 — day-over-day drift. When the daily owner has captured a
            // baseline, show the average movement; otherwise say so truthfully
            // instead of silently omitting the rows.
            if (_survivors != null)
            {
                if (!_survivors.HasNeedsBaseline)
                {
                    AddStatRow(Tr("ui.status.drift.header", "Needs Drift"),
                        Tr("ui.status.drift.pending", "trend available after the first day advance"),
                        "badge_exhaustion");
                }
                else
                {
                    float hungerDrift = 0f, thirstDrift = 0f, fatigueDrift = 0f, moraleDrift = 0f, warmthDrift = 0f;
                    int driftSamples = 0;
                    for (int i = 0; i < roster.Count; i++)
                    {
                        var s = roster[i];
                        if (s == null || !s.IsAlive) continue;
                        if (!_survivors.TryGetNeedDayDelta(s.Id, NeedKind.Hunger, out float dh)) continue;
                        _survivors.TryGetNeedDayDelta(s.Id, NeedKind.Thirst, out float dt);
                        _survivors.TryGetNeedDayDelta(s.Id, NeedKind.Fatigue, out float df);
                        _survivors.TryGetNeedDayDelta(s.Id, NeedKind.Morale, out float dm);
                        _survivors.TryGetNeedDayDelta(s.Id, NeedKind.Warmth, out float dw);
                        hungerDrift += dh;
                        thirstDrift += dt;
                        fatigueDrift += df;
                        moraleDrift += dm;
                        warmthDrift += dw;
                        driftSamples++;
                    }
                    if (driftSamples > 0)
                    {
                        // Annotate the span so a skipped/offline gap is not read
                        // as a one-day rate (task 9).
                        int span = Math.Max(1, _survivors.NeedDaySpan(_simDay));
                        string suffix = span <= 1
                            ? Tr("ui.status.drift.per_day", "/ day")
                            : TrFormat("ui.status.drift.per_span", span);
                        AddDriftRow(Tr("ui.status.drift.hunger", "Hunger Drift"), hungerDrift / driftSamples, suffix, positiveIsGood: false, "badge_exhaustion");
                        AddDriftRow(Tr("ui.status.drift.thirst", "Thirst Drift"), thirstDrift / driftSamples, suffix, positiveIsGood: false, "badge_exhaustion");
                        AddDriftRow(Tr("ui.status.drift.fatigue", "Fatigue Drift"), fatigueDrift / driftSamples, suffix, positiveIsGood: false, "badge_exhaustion");
                        // Task 8 — morale is tracked by the day-delta owner; the
                        // panel previously rendered every need but morale. Rising
                        // morale is good, so positiveIsGood: true.
                        AddDriftRow(Tr("ui.status.drift.morale", "Morale Drift"), moraleDrift / driftSamples, suffix, positiveIsGood: true, "badge_guilt_insomnia");
                        AddDriftRow(Tr("ui.status.drift.warmth", "Warmth Drift"), warmthDrift / driftSamples, suffix, positiveIsGood: true, "badge_exhaustion");
                    }
                }
            }

            if (_inventory?.Inventory != null)
            {
                int water = CountItem("clean_water", "item_clean_water") + CountItem("water_bottle");
                int food = CountItem("canned_food") + CountItem("canned_meat")
                         + CountItem("canned_soup") + CountItem("canned_beans");
                // Task 6 — the stores rows were the only unbanded measurements on
                // the card. Thresholds are named consts here because store
                // scarcity is a host presentation concern, not a simulation
                // constant owned by NeedsProfile/RadiationSystem; the critical
                // figure (water <= 3) is the same one RenderObjectives already
                // raises as a water-critical directive.
                //
                // Loop-1 of the verification pass — this originally called a
                // local BandStores helper that returned Lethe for nominal, so
                // these two rows were the only banded rows on the card not
                // sharing the shared authority's nominal token. Stores are
                // low-is-bad like morale, so the existing helper is the correct
                // one; no second band rule is warranted.
                AddStatRow(Tr("ui.status.stores.water", "Water Stores"),
                    TrFormat("ui.status.stores.water_value", water),
                    "item_desal_membrane",
                    AshfallUiBands.ForLow(water, warnAt: WaterWarnStores, criticalAt: WaterCriticalStores));
                AddStatRow(Tr("ui.status.stores.food", "Food Stores"),
                    TrFormat("ui.status.stores.food_value", food),
                    "item_brine_salt",
                    AshfallUiBands.ForLow(food, warnAt: FoodWarnStores, criticalAt: FoodCriticalStores));
            }
        }

        private const int WaterCriticalStores = 3;
        private const int WaterWarnStores = 8;
        private const int FoodCriticalStores = 2;
        private const int FoodWarnStores = 6;

        private void RenderSystemStatus()
        {
            if (_power?.LastSnapshot != null)
            {
                var snap = _power.LastSnapshot;
                _statusData.AddChild(AshfallUiHelpers.MakeDataRow(Tr("ui.status.system.power_grid", "Power Grid"),
                    snap.IsBrownout
                        ? TrFormat("ui.status.system.power_brownout", $"{snap.GenerationWatts:0}", $"{snap.TotalDrawWatts:0}")
                        : TrFormat("ui.status.system.power_online", $"{snap.GenerationWatts:0}", $"{snap.TotalDrawWatts:0}"),
                    AshfallUiHelpers.ToColor(snap.IsBrownout
                        ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Lethe)));
            }
            else
            {
                _statusData.AddChild(AshfallUiHelpers.MakeDataRow(Tr("ui.status.system.power_grid", "Power Grid"), Tr("ui.status.not_monitored", "NOT MONITORED"),
                    AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim)));
            }

            if (_survivors?.Shelter != null)
            {
                float weakest = _survivors.Shelter.GetWeakestCeilingAttenuation();
                _statusData.AddChild(AshfallUiHelpers.MakeDataRow(Tr("ui.status.system.radiation_shielding", "Radiation Shielding"),
                    TrFormat("ui.status.system.shielding_value", $"{weakest * 100f:0}"),
                    AshfallUiHelpers.ToColor(weakest >= 0.5f
                        ? Ashfall.Core.UI.Theme.Lethe : Ashfall.Core.UI.Theme.Warm)));
            }

            if (_weather != null)
            {
                float outdoor = _weather.OutdoorRadModifier;
                _statusData.AddChild(AshfallUiHelpers.MakeDataRow(Tr("ui.status.system.outdoor_radiation", "Outdoor Radiation"),
                    outdoor > 0f ? TrFormat("ui.status.system.outdoor_value", $"{outdoor:0}") : Tr("ui.status.system.outdoor_none", "No weather modifier"),
                    AshfallUiHelpers.ToColor(outdoor > 0f
                        ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Lethe)));
            }
        }

        /// <summary>
        /// P015 — warmth/temperature readout. Roster warmth is the survivors
        /// owner's value; clothing warmth is the cold-loss reduction the
        /// ClothingWarmth owner already feeds NeedsSystem; shelter thermal is the
        /// ShelterThermal owner's coldest room + boiler state. An unbound owner
        /// reads NOT MONITORED rather than a fabricated comfort figure.
        /// </summary>
        private void RenderThermal()
        {
            if (_thermalData == null) return;

            var alive = _survivors?.RosterState?.Where(s => s != null && s.IsAlive).ToList();
            if (alive != null && alive.Count > 0)
            {
                float avgWarmth = alive.Average(s => s.Warmth);
                float minWarmth = alive.Min(s => s.Warmth);
                var profile = _survivors?.Needs?.Profile;
                // Task 3 — three-band warmth readout routed through the shared
                // band authority. This row previously re-derived the Critical /
                // Warm / Lethe decision inline, which is the same drift class
                // the helper exists to remove, and it took its critical test
                // from a different expression (IsWarmthCritical) than its warn
                // test. One rule, one owner.
                _thermalData.AddChild(AshfallUiHelpers.MakeDataRow(Tr("ui.status.thermal.roster_warmth", "Roster Warmth"),
                    TrFormat("ui.status.thermal.roster_value", $"{avgWarmth:0}", $"{minWarmth:0}"),
                    AshfallUiHelpers.ToColor(profile != null
                        ? AshfallUiBands.ForLow(minWarmth, warnAt: profile.warmthWarn, criticalAt: profile.warmthCritical)
                        : Ashfall.Core.UI.Theme.Lethe)));
            }
            else
            {
                _thermalData.AddChild(AshfallUiHelpers.MakeDataRow(Tr("ui.status.thermal.roster_warmth", "Roster Warmth"), Tr("ui.status.not_monitored", "NOT MONITORED"),
                    AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim)));
            }

            if (_clothingWarmth != null && alive != null && alive.Count > 0)
            {
                float avgReduction = alive.Average(s => _clothingWarmth.CalculateColdLossReduction(s.Id));
                _thermalData.AddChild(AshfallUiHelpers.MakeDataRow(Tr("ui.status.thermal.clothing_warmth", "Clothing Warmth"),
                    TrFormat("ui.status.thermal.clothing_value", $"{avgReduction * 100f:0}"),
                    AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe)));
            }
            else
            {
                _thermalData.AddChild(AshfallUiHelpers.MakeDataRow(Tr("ui.status.thermal.clothing_warmth", "Clothing Warmth"), Tr("ui.status.not_monitored", "NOT MONITORED"),
                    AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim)));
            }

            var thermal = _shelterThermal?.System;
            var rooms = thermal?.State?.rooms;
            if (thermal != null && rooms != null && rooms.Count > 0)
            {
                float coldest = rooms.Min(r => r.currentTempC);
                _thermalData.AddChild(AshfallUiHelpers.MakeDataRow(Tr("ui.status.thermal.shelter_thermal", "Shelter Thermal"),
                    TrFormat("ui.status.thermal.shelter_value", $"{coldest:0.0}", thermal.BoilerActive ? Tr("ui.status.thermal.boiler_on", "on") : Tr("ui.status.thermal.boiler_off", "off"), $"{thermal.BoilerFuelLevel:0}"),
                    AshfallUiHelpers.ToColor(coldest < 5f
                        ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Lethe)));
                if (!string.Equals(thermal.WaterwayFreezeState, "Open", StringComparison.Ordinal))
                    _thermalData.AddChild(AshfallUiHelpers.MakeDataRow(Tr("ui.status.thermal.waterway", "Waterway"),
                        thermal.WaterwayFreezeState,
                        AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));
            }
            else
            {
                _thermalData.AddChild(AshfallUiHelpers.MakeDataRow(Tr("ui.status.thermal.shelter_thermal", "Shelter Thermal"), Tr("ui.status.not_monitored", "NOT MONITORED"),
                    AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim)));
            }
        }

        /// <summary>
        /// P016 — read-only "next 3 days: needs vs supply" projection. Rows are
        /// computed by the host from the ration + inventory owners; the panel
        /// only renders them, so it never re-derives a consumption rate.
        /// </summary>
        private void RenderForecast()
        {
            if (_forecastData == null) return;
            var rows = _supplyForecast;
            if (rows == null || rows.Count == 0)
            {
                _forecastData.AddChild(AshfallUiHelpers.MakeDataRow(Tr("ui.status.forecast.supply_projection", "Supply projection"), Tr("ui.status.not_monitored", "NOT MONITORED"),
                    AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim)));
                return;
            }

            int firstShortfall = Ashfall.Core.Campaign.SupplyForecast.FirstShortfallDay(rows);
            for (int i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                bool isShort = row.FoodShort || row.WaterShort;
                // Task 10 — the " — SHORT" suffix was glued on from a hardcoded
                // separator outside the catalog, so a German player saw an
                // English punctuation fragment spliced onto a translated value.
                // Each composed form is now a single key.
                string value = isShort
                    ? TrFormat("ui.status.forecast.day_value_short", row.FoodAfter, row.WaterAfter)
                    : TrFormat("ui.status.forecast.day_value", row.FoodAfter, row.WaterAfter);
                _forecastData.AddChild(AshfallUiHelpers.MakeDataRow(TrFormat("ui.status.forecast.day_label", i + 1), value,
                    AshfallUiHelpers.ToColor(isShort
                        ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Lethe)));
            }
            if (firstShortfall > 0)
            {
                _forecastData.AddChild(AshfallUiHelpers.MakeDataRow(Tr("ui.status.forecast.shortfall", "Shortfall"),
                    TrFormat("ui.status.forecast.shortfall_value", firstShortfall),
                    AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical)));
            }
        }

        /// <summary>
        /// P011 — explicit lethal thresholds, read from the owning authorities
        /// (NeedsProfile critical values and RadiationSystem constants) so the
        /// panel can never teach numbers the simulation does not enforce.
        /// </summary>
        private void RenderThresholds()
        {
            if (_thresholdData == null) return;
            var profile = _survivors?.Needs?.Profile;
            if (profile == null)
            {
                _thresholdData.AddChild(AshfallUiHelpers.MakeDataRow(
                    Tr("ui.status.threshold.needs", "Needs thresholds"), Tr("ui.status.not_monitored", "NOT MONITORED"),
                    AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim)));
                return;
            }

            AddThresholdRow(Tr("ui.status.threshold.starvation", "Starvation"),
                TrFormat("ui.status.threshold.value.starvation", profile.hungerCritical, profile.healthLossFromHunger),
                critical: true);
            AddThresholdRow(Tr("ui.status.threshold.dehydration", "Dehydration"),
                TrFormat("ui.status.threshold.value.dehydration", profile.thirstCritical, profile.healthLossFromThirst),
                critical: true);
            AddThresholdRow(Tr("ui.status.threshold.hypothermia", "Hypothermia"),
                TrFormat("ui.status.threshold.value.hypothermia", profile.warmthCritical, profile.healthLossFromCold),
                critical: true);
            AddThresholdRow(Tr("ui.status.threshold.acute_radiation", "Acute radiation"),
                TrFormat("ui.status.threshold.value.acute_radiation", RadiationSystem.AcuteThreshold, RadiationSystem.HealthLossPerHourAtAcute),
                critical: true);
            AddThresholdRow(Tr("ui.status.threshold.chronic_exposure", "Chronic exposure"),
                TrFormat("ui.status.threshold.value.chronic_exposure", RadiationSystem.ChronicLifetimeThreshold),
                critical: false);
        }

        // Task 4 — resolve through the shared AshfallUiText helper so every UI
        // panel localizes through one implementation.
        private static string Tr(string key, string fallback) => AshfallUiText.Tr(key, fallback);

        /// <summary>Localizes the objective tag code for display (loop-3 hardening).</summary>
        private static string ObjectiveTag(string type) => type switch
        {
            "PRIMARY" => Tr("ui.status.objective.tag.primary", "PRIMARY"),
            "DAILY" => Tr("ui.status.objective.tag.daily", "DAILY"),
            "SECONDARY" => Tr("ui.status.objective.tag.secondary", "SECONDARY"),
            _ => Tr("ui.status.objective.tag.standing", "STANDING"),
        };

        private static string TrFormat(string key, params object[] args) => AshfallUiText.TrFormat(key, args);

        private void AddThresholdRow(string label, string value, bool critical)
        {
            _thresholdData.AddChild(AshfallUiHelpers.MakeDataRow(label, value,
                AshfallUiHelpers.ToColor(critical
                    ? Ashfall.Core.UI.Theme.Critical
                    : Ashfall.Core.UI.Theme.Warm)));
        }

        /// <summary>Formats a signed per-day drift value, e.g. "+19" / "-4".</summary>
        private static string Signed(float value) => Ashfall.Core.Survivors.NeedsDayDeltaFormat.Signed(value);

        /// <summary>
        /// P108 follow-up — surface the most recent expedition injury per living
        /// survivor from the canonical health-history owner. Read-only projection;
        /// no new state, and nothing is shown when no expedition injury exists.
        /// </summary>
        private void RenderExpeditionInjuries()
        {
            var history = _healthHistory?.System;
            var roster = _survivors?.RosterState;
            if (history == null || roster == null) return;

            var members = new List<(string Id, bool Alive)>();
            foreach (var s in roster)
                if (s != null) members.Add((s.Id, s.IsAlive));

            var rows = ExpeditionInjuryDigest.Build(
                members,
                id => history.GetLatestEventOfTypePrefix(id, ExpeditionInjuryDigest.EventTypePrefix));
            foreach (var row in rows)
            {
                // Task 8 — the fallback label for a nameless survivor was a
                // hardcoded English "[UNNAMED]" rendered inside a row whose
                // other half is translated.
                string name = string.IsNullOrEmpty(row.SurvivorId)
                    ? Tr("ui.status.expedition_injury.unnamed", "[UNNAMED]")
                    : row.SurvivorId.Replace("survivor_", "").Replace("_", " ").ToUpperInvariant();
                AddStatRow(Tr("ui.status.expedition_injury", "Expedition Injury"),
                    TrFormat("ui.status.expedition_injury.value", name, row.Day, row.Description), "badge_exhaustion");
            }
        }

        private void AddStatRow(string label, string value, string badge, (float r, float g, float b, float a)? color = null)
        {
            var row = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
            var icon = AshfallUiHelpers.MakeBadgeIcon(badge, 22);
            row.AddChild(icon);

            var lbl = AshfallUiHelpers.MakeSmall(label);
            lbl.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            row.AddChild(lbl);

            var val = AshfallUiHelpers.MakeMono(value);
            val.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(color ?? Ashfall.Core.UI.Theme.Pale));
            row.AddChild(val);

            _statsData.AddChild(row);
        }

        /// <summary>
        /// T9 — drift rows colour by direction. Rising hunger/thirst/fatigue is
        /// bad (Warm); rising warmth is good (Lethe), unlike the other needs.
        /// </summary>
        private void AddDriftRow(string label, float delta, string suffix, bool positiveIsGood, string badge)
        {
            bool good = positiveIsGood ? delta >= 0f : delta <= 0f;
            var color = good ? Ashfall.Core.UI.Theme.Lethe : Ashfall.Core.UI.Theme.Warm;
            AddStatRow(label, $"{Signed(delta)} {suffix}", badge, color);
        }

        private int CountItem(string primaryId, string fallbackId = null!)
        {
            if (_inventory?.Inventory == null) return 0;
            int count = _inventory.Inventory.CountById(primaryId);
            if (count == 0 && fallbackId != null)
                count = _inventory.Inventory.CountById(fallbackId);
            return count;
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = AshfallUiHelpers.PanelScrim() };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var center = new CenterContainer();
            center.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(center);

            var panel = AshfallUiHelpers.MakePanel(680, 560);
            center.AddChild(panel);

            var margins = AshfallUiHelpers.MakeMargins(Ashfall.Core.UI.Theme.SpacingMd);
            panel.AddChild(margins);

            var vbox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingMd);
            margins.AddChild(vbox);

            var header = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
            var title = AshfallUiHelpers.MakeTitle(Tr("ui.status.title", "HOLDFAST STATUS & OPERATIONS"), Ashfall.Core.UI.Theme.FontSizeH2);
            title.HorizontalAlignment = HorizontalAlignment.Left;
            title.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            header.AddChild(title);

            var btnClose = AshfallUiHelpers.MakeButton(Tr("ui.status.close", "CLOSE [Esc]"), () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(110, 32);
            header.AddChild(btnClose);
            vbox.AddChild(header);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            var scroll = new ScrollContainer
            {
                CustomMinimumSize = new Vector2(640, 440),
                SizeFlagsVertical = SizeFlags.ExpandFill
            };
            vbox.AddChild(scroll);

            var contentBox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingMd);
            contentBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            scroll.AddChild(contentBox);

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader(Tr("ui.status.section.day_environment", "DAY & ENVIRONMENT")));
            _dayInfo = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
            contentBox.AddChild(_dayInfo);

            contentBox.AddChild(AshfallUiHelpers.MakeSeparator());

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader(Tr("ui.status.section.directives", "STANDING DIRECTIVES")));
            _objectivesList = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
            contentBox.AddChild(_objectivesList);

            contentBox.AddChild(AshfallUiHelpers.MakeSeparator());

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader(Tr("ui.status.section.cohort", "COHORT & SUPPLY TELEMETRY")));
            _statsData = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
            contentBox.AddChild(_statsData);

            contentBox.AddChild(AshfallUiHelpers.MakeSeparator());

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader(Tr("ui.status.section.shelter", "SHELTER SUBSYSTEM HEALTH")));
            _statusData = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
            contentBox.AddChild(_statusData);

            contentBox.AddChild(AshfallUiHelpers.MakeSeparator());

            // P011 — the lethal thresholds the simulation actually enforces,
            // read from the owning constants (never re-typed here).
            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader(
                Tr("ui.status.threshold.header", "LETHAL THRESHOLDS")));
            _thresholdData = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
            contentBox.AddChild(_thresholdData);

            contentBox.AddChild(AshfallUiHelpers.MakeSeparator());

            // P015 — warmth/temperature glance, read from the clothing-warmth and
            // shelter-thermal owners (never a fabricated comfort number).
            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader(Tr("ui.status.section.thermal", "WARMTH & THERMAL")));
            _thermalData = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
            contentBox.AddChild(_thermalData);

            contentBox.AddChild(AshfallUiHelpers.MakeSeparator());

            // P016 — read-only "next 3 days: needs vs supply" projection computed
            // by the host from the ration + inventory owners.
            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader(Tr("ui.status.section.forecast", "3-DAY SUPPLY FORECAST")));
            _forecastData = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
            contentBox.AddChild(_forecastData);

            RefreshView();
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
            QueueRedraw();
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;
            if (AshfallInputActions.IsCloseOrCancel(@event))
            {
                OnClose?.Invoke();
                GetViewport().SetInputAsHandled();
            }
        }
    }
}
