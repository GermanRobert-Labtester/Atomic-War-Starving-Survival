// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Godot;
using Ashfall.Core.World;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Plan 176 presentation — Anomaly Watch // Moving Hazards. Shows ONLY what
    /// the shelter can actually know: hazard zones within detection range are
    /// reported at the confidence the carried devices support; unknown zones
    /// stay unknown (no radar-through-fog). Pure presentation.
    /// </summary>
    public partial class AnomalyWatchPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _watchText = null!;
        private Func<AnomalyHazardSystem?>? _source;
        private Func<float>? _detectorCapability;

        public bool IsBound => _source != null;

        public void Bind(Func<AnomalyHazardSystem?> source, Func<float>? detectorCapability = null)
        {
            _source = source;
            _detectorCapability = detectorCapability;
            RefreshView();
        }

        public void Unbind() { _source = null; _detectorCapability = null; }

        public void RefreshView()
        {
            if (_source == null || _statusRail == null) return;
            var system = _source();
            if (system == null) return;

            float capability = _detectorCapability?.Invoke() ?? 0f;
            var active = system.State.hazards.Where(h => h != null && h.active)
                .OrderBy(h => h.hazard_id, StringComparer.Ordinal).ToList();
            float shelterRate = system.GetRadiationRate(0f, 0f);
            int unresolvedSites = system.State.loot_sites.Count(s => s != null && !s.resolved);

            _statusRail.Set("active", $"{active.Count}",
                active.Count > 0 ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("shelter_rad", shelterRateText(shelterRate),
                shelterRate > 5f ? AshfallMetricCard.Criticality.Critical
                : shelterRate > 0f ? AshfallMetricCard.Criticality.Warn
                : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("detector", capability >= AnomalyHazardSystem.GeigerCapabilityRadsPerHour
                ? "geiger" : capability > 0 ? "dosimeter" : "NONE",
                capability > 0 ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Warn);
            _statusRail.Set("sites", $"{unresolvedSites} gated", AshfallMetricCard.Criticality.Normal);

            var lines = new StringBuilder();
            if (active.Count == 0)
                lines.AppendLine("No known anomaly zones on the survey ledger. New signatures surface through survey teams and approach warnings.");
            foreach (var h in active)
            {
                var def = system.Definition(h.anomaly_id);
                HazardDetection detection = system.ClassifyDetection(h.position_x, h.position_y, capability);
                float confidence = system.GetDetectionConfidence(h.position_x, h.position_y, capability);

                lines.AppendLine($"{h.hazard_id} — {(detection == HazardDetection.Classified ? def?.display_name ?? h.anomaly_id : "unidentified signature")}");
                lines.AppendLine($"  position ({h.position_x:0.0}, {h.position_y:0.0}) · age {h.age_days}/{h.duration_days} days · intensity {h.intensity:P0}");
                if (detection == HazardDetection.Classified)
                {
                    lines.AppendLine($"  classified · confidence {confidence:P0} · radiation {h.radiation_rate:0} rads/hr at core · falloff radius {h.radius_km:0} km");
                    lines.AppendLine($"  effects: {string.Join(", ", def?.environmental_effect_tags ?? new List<string>())}");
                    if (def?.movement_profile == "storm_front")
                        lines.AppendLine($"  mobile — bearing {h.bearing_deg:0.0}°");
                    else if (def?.movement_profile == "wind_drift")
                        lines.AppendLine("  drifting with the surface wind.");
                    else
                        lines.AppendLine("  static terrain feature.");
                }
                else
                {
                    lines.AppendLine($"  presence felt · confidence {confidence:P0} — a working detector resolves the classification.");
                }
                lines.AppendLine();
            }

            foreach (var site in system.State.loot_sites.Where(s => s != null).Take(6))
            {
                lines.AppendLine($"Gated site {site.site_id}: {(site.resolved ? $"resolved day {site.resolved_day}" : "unresolved — detection and access required")}");
            }
            _watchText.Text = lines.ToString().TrimEnd();
        }

        private static string shelterRateText(float rate) => rate <= 0f ? "clear" : $"{rate:0.0} rads/hr";

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            _shell = new AshfallDashboardShell("Anomaly Watch // Moving Hazards", minWidth: 900, minHeight: 560);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("active", "Known Zones", "0", AshfallMetricCard.Criticality.Normal, minWidth: 150);
            _statusRail.AddCard("shelter_rad", "Shelter Rad", "clear", AshfallMetricCard.Criticality.Normal, minWidth: 170);
            _statusRail.AddCard("detector", "Detector", "none", AshfallMetricCard.Criticality.Normal, minWidth: 150);
            _statusRail.AddCard("sites", "Gated Sites", "0", AshfallMetricCard.Criticality.Normal, minWidth: 140);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 12);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("SURVEY PICTURE"));
            _watchText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart };
            _contentStack.AddChild(_watchText);

            var note = AshfallUiHelpers.MakeBody(
                "Storm fronts and anomaly fields move with the wind and wander with the weather. A Geiger counter classifies signatures a dosimeter can only hint at. Approaching fronts are warned about once; expeditions route around what the watch knows about — and eat the dose where it doesn't.");
            note.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            note.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            _contentStack.AddChild(note);

            _shell.SetContent(_contentStack);
            _shell.AttachHeaderCloseButton("CLOSE", () => { Visible = false; OnClose?.Invoke(); });
            RefreshView();
        }

        public override void _ExitTree() => Unbind();
    }
}
