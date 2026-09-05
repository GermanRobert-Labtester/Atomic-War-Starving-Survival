using System;
using System.Linq;
using Godot;
using Ashfall.Core.UI;
using Ashfall.Core.World;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Status panel.
    /// Shows overall game status, day counter, current objectives, and quick stats
    /// using tactile 9-slice card framing and status badge icons.
    /// Bound to live host sessions (Survivors, Weather, Power Grid, Inventory);
    /// unbound systems render "NOT MONITORED" instead of fabricated data.
    /// </summary>
    public partial class StatusPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _dayInfo = null!;
        private VBoxContainer _objectivesList = null!;
        private VBoxContainer _statsData = null!;
        private VBoxContainer _statusData = null!;

        private SurvivorsHostSession? _survivors;
        private WeatherSystem? _weather;
        private PowerGridHostSession? _power;
        private InventoryHostSession? _inventory;
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
            int simDay = 1)
        {
            _survivors = survivors;
            _weather = weather;
            _power = power;
            _inventory = inventory;
            _simDay = simDay;
            RefreshView();
        }

        public void RefreshView()
        {
            if (_dayInfo == null || _objectivesList == null || _statsData == null || _statusData == null) return;

            AshfallUiHelpers.EmptyChildren(_dayInfo);
            AshfallUiHelpers.EmptyChildren(_objectivesList);
            AshfallUiHelpers.EmptyChildren(_statsData);
            AshfallUiHelpers.EmptyChildren(_statusData);

            RenderDayInfo();
            RenderObjectives();
            RenderStats();
            RenderSystemStatus();
        }

        private void RenderDayInfo()
        {
            int rows = 0;

            _dayInfo.AddChild(AshfallUiHelpers.MakeDataRow("Current Day", $"Day {_simDay}",
                AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm)));
            rows++;

            if (_survivors?.RosterState != null && _survivors.RosterState.Count > 0)
            {
                int total = _survivors.RosterState.Count;
                int alive = _survivors.RosterState.Count(s => s != null && s.IsAlive);
                _dayInfo.AddChild(AshfallUiHelpers.MakeDataRow("Survivors",
                    $"{alive} / {total} Alive",
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
                _dayInfo.AddChild(AshfallUiHelpers.MakeDataRow("External Conditions",
                    $"{kind}{(hazard ? " — HAZARD WATCH" : " — Nominal")}",
                    AshfallUiHelpers.ToColor(hazard
                        ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Lethe)));
                rows++;
            }
            else
            {
                _dayInfo.AddChild(AshfallUiHelpers.MakeDataRow("External Conditions", "NOT MONITORED",
                    AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim)));
                rows++;
            }

            if (_power?.LastSnapshot != null)
            {
                var snap = _power.LastSnapshot;
                _dayInfo.AddChild(AshfallUiHelpers.MakeDataRow("Power Reserve",
                    $"Battery {snap.BatteryReserveWh:0}/{snap.BatteryCapacityWh:0} Wh · Fuel {snap.FuelUnits:0}",
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
                int critical = _survivors.RosterState
                    .Count(s => s != null && s.IsAlive && s.Health < 30f);
                if (critical > 0)
                    objectives.Add(("PRIMARY",
                        $"{critical} survivor(s) in critical health — triage immediately"));

                int dosed = _survivors.RosterState
                    .Count(s => s != null && s.IsAlive
                        && (_survivors.RadStateFor(s.Id)?.RadiationDose ?? 0f) >= 50f);
                if (dosed > 0)
                    objectives.Add(("PRIMARY",
                        $"{dosed} survivor(s) above 50 mSv dose — iodine/chelation required"));
            }

            if (_power?.LastSnapshot is { IsBrownout: true })
                objectives.Add(("PRIMARY", "Power grid in brownout — shed load or add fuel"));

            if (_weather != null)
            {
                var kind = _weather.Current;
                if (kind == Ashfall.Core.WeatherKind.FalloutStorm
                    || kind == Ashfall.Core.WeatherKind.BlackRain)
                    objectives.Add(("DAILY", "Hazard weather active — keep survivors indoors"));
            }

            if (_inventory?.Inventory != null)
            {
                int water = CountItem("clean_water", "item_clean_water") + CountItem("water_bottle");
                if (water <= 3)
                    objectives.Add(("SECONDARY", "Water stores critical — purify or trade for clean water"));
            }

            objectives.Add(("STANDING", "Keep the roster fed, hydrated, and below dose ceiling"));

            foreach (var obj in objectives)
            {
                var row = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
                var tag = AshfallUiHelpers.MakeSmall($"[{obj.type}]");
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
                var none = AshfallUiHelpers.MakeSmall("No survivor roster bound.");
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

            AddStatRow("Survivor Cohort", $"{alive} / {roster.Count} Alive", "badge_exhaustion");
            AddStatRow("Average Health", $"{avgHealth:0} / 100", "item_first_aid_kit");
            AddStatRow("Bunker Morale", $"{avgMorale:0} / 100", "badge_guilt_insomnia");
            AddStatRow("Dosimetry Dose", $"{avgDose:0.0} mSv (avg)", "item_dosimeter_pen");

            if (_inventory?.Inventory != null)
            {
                int water = CountItem("clean_water", "item_clean_water") + CountItem("water_bottle");
                int food = CountItem("canned_food") + CountItem("canned_meat")
                         + CountItem("canned_soup") + CountItem("canned_beans");
                AddStatRow("Water Stores", $"{water} unit(s)", "item_desal_membrane");
                AddStatRow("Food Stores", $"{food} unit(s)", "item_brine_salt");
            }
        }

        private void RenderSystemStatus()
        {
            if (_power?.LastSnapshot != null)
            {
                var snap = _power.LastSnapshot;
                _statusData.AddChild(AshfallUiHelpers.MakeDataRow("Power Grid",
                    snap.IsBrownout
                        ? $"BROWNOUT · Gen {snap.GenerationWatts:0} W vs Draw {snap.TotalDrawWatts:0} W"
                        : $"Online · Gen {snap.GenerationWatts:0} W · Draw {snap.TotalDrawWatts:0} W",
                    AshfallUiHelpers.ToColor(snap.IsBrownout
                        ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Lethe)));
            }
            else
            {
                _statusData.AddChild(AshfallUiHelpers.MakeDataRow("Power Grid", "NOT MONITORED",
                    AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim)));
            }

            if (_survivors?.Shelter != null)
            {
                float weakest = _survivors.Shelter.GetWeakestCeilingAttenuation();
                _statusData.AddChild(AshfallUiHelpers.MakeDataRow("Radiation Shielding",
                    $"Weakest ceiling {weakest * 100f:0}% attenuation",
                    AshfallUiHelpers.ToColor(weakest >= 0.5f
                        ? Ashfall.Core.UI.Theme.Lethe : Ashfall.Core.UI.Theme.Warm)));
            }

            if (_weather != null)
            {
                float outdoor = _weather.OutdoorRadModifier;
                _statusData.AddChild(AshfallUiHelpers.MakeDataRow("Outdoor Radiation",
                    outdoor > 0f ? $"+{outdoor:0} mSv/h modifier active" : "No weather modifier",
                    AshfallUiHelpers.ToColor(outdoor > 0f
                        ? Ashfall.Core.UI.Theme.Critical : Ashfall.Core.UI.Theme.Lethe)));
            }
        }

        private void AddStatRow(string label, string value, string badge)
        {
            var row = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
            var icon = AshfallUiHelpers.MakeBadgeIcon(badge, 22);
            row.AddChild(icon);

            var lbl = AshfallUiHelpers.MakeSmall(label);
            lbl.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            row.AddChild(lbl);

            var val = AshfallUiHelpers.MakeMono(value);
            val.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
            row.AddChild(val);

            _statsData.AddChild(row);
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

            var bg = new ColorRect { Color = new Color(0.04f, 0.05f, 0.06f, 0.88f) };
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
            var title = AshfallUiHelpers.MakeTitle("HOLDFAST STATUS & OPERATIONS", Ashfall.Core.UI.Theme.FontSizeH2);
            title.HorizontalAlignment = HorizontalAlignment.Left;
            title.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            header.AddChild(title);

            var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc]", () => OnClose?.Invoke());
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

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("DAY & ENVIRONMENT"));
            _dayInfo = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
            contentBox.AddChild(_dayInfo);

            contentBox.AddChild(AshfallUiHelpers.MakeSeparator());

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("STANDING DIRECTIVES"));
            _objectivesList = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
            contentBox.AddChild(_objectivesList);

            contentBox.AddChild(AshfallUiHelpers.MakeSeparator());

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("COHORT & SUPPLY TELEMETRY"));
            _statsData = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
            contentBox.AddChild(_statsData);

            contentBox.AddChild(AshfallUiHelpers.MakeSeparator());

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("SHELTER SUBSYSTEM HEALTH"));
            _statusData = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
            contentBox.AddChild(_statusData);

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
