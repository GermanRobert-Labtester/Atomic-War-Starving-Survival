// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// ASHFALL — Polished in-game HUD overlay.
    /// Shows: day, health bar, radiation bar, needs glance (hunger/thirst/
    /// fatigue/morale), value, faction, weather, and menu.
    /// Thin presentation only — reads state from HoldfastRuntimeSession.
    /// </summary>
    public partial class GameHudOverlay : HBoxContainer
    {
        public event Action? OnMenuRequested;

        private Label _lblDay = null!;
        private Label _lblStage = null!;
        private Label _lblHealthText = null!;
        private ProgressBar _barHealth = null!;
        private ProgressBar _barRad = null!;
        private Label _lblRadText = null!;
        private Label _lblHunger = null!;
        private Label _lblThirst = null!;
        private Label _lblFatigue = null!;
        private Label _lblMorale = null!;
        private Label _lblWarmth = null!;
        private Label _lblValue = null!;
        private Label _lblFaction = null!;
        private Label _lblWeather = null!;
        private Button _btnMenu = null!;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.TopWide);
            AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingMd);

            // Day
            _lblDay = AshfallUiHelpers.MakeTitle(TrFormat("ui.hud.day", 1), Ashfall.Core.UI.Theme.FontSizeH3);
            _lblDay.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
            AddChild(_lblDay);

            AddChild(new VSeparator());

            // First-hour stage progress chip (P006) — e.g. `1/7 · WATER`.
            // Hidden until the host projects an active onboarding journey.
            _lblStage = AshfallUiHelpers.MakeMono(string.Empty);
            _lblStage.AddThemeColorOverride("font_color",
                AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted));
            _lblStage.Visible = false;
            AddChild(_lblStage);

            AddChild(new VSeparator());

            // Health bar with animation
            var healthGroup = new HBoxContainer();
            healthGroup.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            var healthLabel = AshfallUiHelpers.MakeSmall(Tr("ui.hud.label.hp", "HP"));
            healthLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
            healthGroup.AddChild(healthLabel);

            _barHealth = MakeMeter(Ashfall.Core.UI.Theme.Warm);
            healthGroup.AddChild(_barHealth);

            _lblHealthText = AshfallUiHelpers.MakeSmall(TrFormat("ui.hud.health", 100, 100));
            _lblHealthText.CustomMinimumSize = new Vector2(60, 0);
            _lblHealthText.HorizontalAlignment = HorizontalAlignment.Right;
            healthGroup.AddChild(_lblHealthText);

            AddChild(healthGroup);

            AddChild(new VSeparator());

            // Radiation bar with animation
            var radGroup = new HBoxContainer();
            radGroup.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            var radLabel = AshfallUiHelpers.MakeSmall(Tr("ui.hud.label.rad", "RAD"));
            radLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
            radGroup.AddChild(radLabel);

            _barRad = MakeMeter(Ashfall.Core.UI.Theme.Lethe);
            radGroup.AddChild(_barRad);

            _lblRadText = AshfallUiHelpers.MakeSmall(TrFormat("ui.hud.radiation", 0f));
            _lblRadText.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe));
            radGroup.AddChild(_lblRadText);

            AddChild(radGroup);

            AddChild(new VSeparator());

            // P009 — needs glance row. Four compact chips the host refreshes
            // from the shared HoldfastRuntimeSession projection; the HUD owns
            // no need values or thresholds of its own.
            var needsGroup = new HBoxContainer();
            needsGroup.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _lblHunger = MakeNeedChip("ui.hud.needs.hunger", "HUN");
            _lblThirst = MakeNeedChip("ui.hud.needs.thirst", "THI");
            _lblFatigue = MakeNeedChip("ui.hud.needs.fatigue", "FAT");
            _lblMorale = MakeNeedChip("ui.hud.needs.morale", "MOR");
            _lblWarmth = MakeNeedChip("ui.hud.needs.warmth", "WARM");
            needsGroup.AddChild(_lblHunger);
            needsGroup.AddChild(_lblThirst);
            needsGroup.AddChild(_lblFatigue);
            needsGroup.AddChild(_lblMorale);
            needsGroup.AddChild(_lblWarmth);
            AddChild(needsGroup);

            AddChild(new VSeparator());

            // Value counter
            _lblValue = AshfallUiHelpers.MakeSmall(TrFormat("ui.hud.value", 100));
            _lblValue.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Hot));
            AddChild(_lblValue);

            AddChild(new VSeparator());

            // Faction + weather
            _lblFaction = AshfallUiHelpers.MakeSmall("—");
            _lblFaction.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted));
            AddChild(_lblFaction);

            _lblWeather = AshfallUiHelpers.MakeSmall("");
            _lblWeather.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted));
            AddChild(_lblWeather);

            // Spacer
            AddChild(new Control { SizeFlagsHorizontal = SizeFlags.ExpandFill });

            // Menu button
            _btnMenu = AshfallUiHelpers.MakeButton("MENU [Esc]", () => OnMenuRequested?.Invoke());
            _btnMenu.CustomMinimumSize = new Vector2(100, 28);
            AddChild(_btnMenu);

            // Disable per-frame process polling as meters update reactively via UpdateState/UpdateHealth/UpdateRadiation.
            // (No _Process override: HUD is event-driven; per-frame polling would
            // waste frame budget for state that only changes on game events.)
            SetProcess(false);
        }

        /// <summary>
        /// Update HUD from HoldfastRuntimeSession state.
        /// </summary>
        public void UpdateState(int day, long value, string factionId = "", string weather = "")
        {
            _lblDay.Text = TrFormat("ui.hud.day", day);
            _lblValue.Text = TrFormat("ui.hud.value", value);
            _lblFaction.Text = string.IsNullOrEmpty(factionId) ? "—" : factionId.Replace("_", " ").ToUpperInvariant();
            _lblWeather.Text = string.IsNullOrEmpty(weather) ? "" : $"· {weather}";
        }

        /// <summary>
        /// P006 — projects first-hour stage progress (`1/7 · WATER`) onto the
        /// HUD. The host owns the (localized) string; an empty value hides the
        /// chip, so the HUD carries no onboarding knowledge of its own.
        /// </summary>
        public void UpdateOnboardingProgress(string text)
        {
            if (_lblStage == null) return;
            bool show = !string.IsNullOrEmpty(text);
            _lblStage.Text = show ? text : string.Empty;
            _lblStage.Visible = show;
        }

        public void UpdateHealth(int hp, int maxHp = 100)
        {
            _lblHealthText.Text = TrFormat("ui.hud.health", hp, maxHp);
            _barHealth.MaxValue = Math.Max(1, maxHp);
            _barHealth.Value = Math.Clamp(hp, 0, (int)_barHealth.MaxValue);
            if (hp <= 25)
                _lblHealthText.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical));
            else if (hp <= 50)
                _lblHealthText.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Entropy));
            else
                _lblHealthText.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
        }

        /// <summary>
        /// P009 — projects the player survivor's four core needs onto the HUD
        /// glance row. Critical and warn coloring is read from the owning
        /// <see cref="Ashfall.Core.Survivors.NeedsProfile"/> so the HUD can
        /// never teach a threshold the simulation does not enforce. Tags are
        /// localized; the value is dynamic.
        /// </summary>
        public void UpdateNeeds(int hunger, int thirst, int fatigue, int morale, int warmth,
            Ashfall.Core.Survivors.NeedsProfile? profile = null)
        {
            if (_lblHunger == null || _lblWarmth == null) return;
            int hungerCritical = (int)(profile?.hungerCritical ?? 90f);
            int thirstCritical = (int)(profile?.thirstCritical ?? 90f);
            int hungerWarn = (int)(profile?.hungerWarn ?? 70f);
            int thirstWarn = (int)(profile?.thirstWarn ?? 70f);
            int fatigueWarn = (int)(profile?.fatigueWarn ?? 70f);
            int fatigueCritical = (int)(profile?.fatigueCritical ?? 90f);
            int moraleWarn = (int)(profile?.moraleWarn ?? 30f);
            int moraleCritical = (int)(profile?.moraleCritical ?? 15f);
            int warmthWarn = (int)(profile?.warmthWarn ?? 40f);
            int warmthCritical = (int)(profile?.warmthCritical ?? 20f);
            ApplyNeedChip(_lblHunger, Tr("ui.hud.needs.hunger", "HUN"), hunger, highIsBad: true, warnAt: hungerWarn, criticalAt: hungerCritical);
            ApplyNeedChip(_lblThirst, Tr("ui.hud.needs.thirst", "THI"), thirst, highIsBad: true, warnAt: thirstWarn, criticalAt: thirstCritical);
            ApplyNeedChip(_lblFatigue, Tr("ui.hud.needs.fatigue", "FAT"), fatigue, highIsBad: true, warnAt: fatigueWarn, criticalAt: fatigueCritical);
            ApplyNeedChip(_lblMorale, Tr("ui.hud.needs.morale", "MOR"), morale, highIsBad: false, warnAt: moraleWarn, criticalAt: moraleCritical);
            // Warmth is low-is-bad, like morale.
            ApplyNeedChip(_lblWarmth, Tr("ui.hud.needs.warmth", "WARM"), warmth, highIsBad: false, warnAt: warmthWarn, criticalAt: warmthCritical);
        }

        private static string Tr(string key, string fallback) => AshfallUiText.Tr(key, fallback);

        private static string TrFormat(string key, params object[] args) => AshfallUiText.TrFormat(key, args);

        private static void ApplyNeedChip(Label label, string tag, int value, bool highIsBad, int warnAt, int criticalAt)
        {
            label.Text = $"{tag} {value}";
            // Task 8 — explain the colour from the owning bands.
            label.TooltipText = highIsBad
                ? TrFormat("ui.hud.needs.tooltip.high", warnAt, criticalAt)
                : TrFormat("ui.hud.needs.tooltip.low", warnAt, criticalAt);
            // Loop-1 hardening — the band rule now resolves through
            // AshfallUiBands, the same helper StatusPanel and
            // SurvivorDetailPanel use, so the three cannot drift apart.
            var color = AshfallUiBands.ForNeed(value, highIsBad, warnAt, criticalAt);
            label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(color));
        }

        private static Label MakeNeedChip(string key, string fallback)
        {
            var label = AshfallUiHelpers.MakeSmall($"{Tr(key, fallback)} --");
            label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
            return label;
        }

        public void UpdateRadiation(float msv)
        {
            _lblRadText.Text = TrFormat("ui.hud.radiation", msv);
            _barRad.MaxValue = 100f;
            _barRad.Value = Mathf.Clamp(msv, 0f, 100f);
            // Loop-2 — this label previously re-derived its own thresholds
            // (>= 100 red, >= 50 amber) while the survivor detail row and the
            // host's radiation_high toast both read RadiationSystem
            // (critical at AcuteThreshold 80, warn at WarnThreshold 50). At
            // 85 mSv the HUD said "amber" while the panel said "critical". All
            // three now resolve through the one band authority.
            //
            // Task 4 — the meter fill was latched once at construction, so the
            // bar stayed calm cyan while its own label went red. Bar and label
            // now read the same band variable.
            var band = AshfallUiBands.ForDose(msv);
            _lblRadText.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(band));
            SetMeterFill(_barRad, band);
        }

        /// <summary>Task 4 — re-tint a meter's fill so bar and label agree.</summary>
        private static void SetMeterFill(ProgressBar meter, (float r, float g, float b, float a) fillColor)
            => meter.AddThemeStyleboxOverride("fill", AshfallUiHelpers.MakeFlatBg(
                new Color(fillColor.r, fillColor.g, fillColor.b, 0.92f), null, 0, Ashfall.Core.UI.Theme.RadiusSm));

        private static ProgressBar MakeMeter((float r, float g, float b, float a) fillColor)
        {
            var meter = new ProgressBar
            {
                MinValue = 0,
                MaxValue = 100,
                Value = 0,
                ShowPercentage = false,
                CustomMinimumSize = new Vector2(96, 12)
            };
            meter.AddThemeStyleboxOverride("background", AshfallUiHelpers.MakeFlatBg(
                new Color(Ashfall.Core.UI.Theme.Ink.r, Ashfall.Core.UI.Theme.Ink.g, Ashfall.Core.UI.Theme.Ink.b, 0.9f),
                AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.LineSoft), 1, Ashfall.Core.UI.Theme.RadiusSm));
            meter.AddThemeStyleboxOverride("fill", AshfallUiHelpers.MakeFlatBg(
                new Color(fillColor.r, fillColor.g, fillColor.b, 0.92f), null, 0, Ashfall.Core.UI.Theme.RadiusSm));
            return meter;
        }
    }
}
