// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core.UI;
using Ashfall.Core.YearOfAsh;
using AtomicWar.GodotApp.YearOfAsh;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Basal radon migration surface (UI/UX audit 2026-09-25 — converted from a
    /// static prototype shell into a truthful read-only projection).
    ///
    /// Authority: <see cref="YearOfAshHostSession.Radon"/> — the live
    /// <c>YearOfAshRadonSystem</c> (indoor Bq/m³, scrubber filter health,
    /// foundation fissures, logged alpha dose, scrubber alarm). No local state,
    /// no second radon ledger: values are refreshed from the session on every
    /// bind and open.
    /// </summary>
    public partial class BasalRadonMigrationPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private Label _status = null!;
        private VBoxContainer _telemetry = null!;
        private VBoxContainer _vectors = null!;
        private YearOfAshHostSession? _session;

        public bool IsBound { get; private set; }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            BuildInterface();
        }

        /// <summary>Binds the live Year of Ash session that owns radon state.</summary>
        public void Bind(YearOfAshHostSession? session)
        {
            if (_session != null)
            {
                _session.Radon.OnRadonLevelChanged -= OnRadonLevelChanged;
                _session.Radon.OnRadonAlarmTriggered -= OnRadonAlarmTriggered;
            }

            _session = session;
            IsBound = session != null;

            if (_session != null)
            {
                _session.Radon.OnRadonLevelChanged += OnRadonLevelChanged;
                _session.Radon.OnRadonAlarmTriggered += OnRadonAlarmTriggered;
            }
            RefreshView();
        }

        public void Unbind()
        {
            if (_session != null)
            {
                _session.Radon.OnRadonLevelChanged -= OnRadonLevelChanged;
                _session.Radon.OnRadonAlarmTriggered -= OnRadonAlarmTriggered;
            }
            _session = null;
            IsBound = false;
        }

        private void OnRadonLevelChanged(float _) => RefreshView();
        private void OnRadonAlarmTriggered(string _) => RefreshView();

        public void Open()
        {
            Visible = true;
            RefreshView();
        }

        private void BuildInterface()
        {
            var chrome = ThreePanePanelScaffold.BuildChrome(
                this,
                "FOUNDATION GEOLOGY // BASAL RADON MIGRATION & SCRUBBER STATUS",
                "[WAITING FOR SESSION]",
                AshfallUiHelpers.ToColor(DesignTheme.Dim),
                "[X] CLOSE CONSOLE",
                "Radon ingress telemetry reads from the live Year of Ash radon authority.",
                () => OnClose?.Invoke());
            _status = chrome.Status;
            var body = chrome.Body;

            var left = ThreePanePanelScaffold.CreatePanelFrame("RADON TELEMETRY");
            body.AddChild(left);
            _telemetry = ThreePanePanelScaffold.CreateColumn(left, DesignTheme.SpacingSm);

            var right = ThreePanePanelScaffold.CreatePanelFrame("INGRESS VECTORS & ALARM");
            body.AddChild(right);
            _vectors = ThreePanePanelScaffold.CreateColumn(right, DesignTheme.SpacingSm);

            RefreshView();
        }

        public void RefreshView()
        {
            ClearChildren(_telemetry);
            ClearChildren(_vectors);
            if (_session == null)
            {
                _status.Text = "[NOT CONNECTED — YEAR OF ASH SESSION NOT READY]";
                _status.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Dim));
                _vectors.AddChild(AshfallUiHelpers.MakeBody(
                    "Radon authority unavailable — start or load a campaign to read live ingress."));
                return;
            }

            var radon = _session.Radon;
            float bq = radon.IndoorRadonBqm3;
            bool dangerous = radon.IsDangerous;
            bool elevated = !dangerous && bq >= YearOfAshRadonSystem.SafeRadonThreshold;
            var band = dangerous
                ? AshfallUiHelpers.ToColor(DesignTheme.Critical)
                : elevated
                    ? AshfallUiHelpers.ToColor(DesignTheme.Warning)
                    : AshfallUiHelpers.ToColor(DesignTheme.Success);

            _status.Text = $"{(dangerous ? "DANGEROUS" : elevated ? "ELEVATED" : "STABLE")} — INDOOR {bq:0} Bq/m³ (SAFE < {YearOfAshRadonSystem.SafeRadonThreshold:0})";
            _status.AddThemeColorOverride("font_color", band);

            _telemetry.AddChild(ThreePanePanelScaffold.CreateTelemetryRow(
                "INDOOR RADON", $"{bq:0.#} Bq/m³", band));
            _telemetry.AddChild(ThreePanePanelScaffold.CreateTelemetryRow(
                "SCRUBBER FILTER HEALTH", $"{radon.ScrubberHealthPercent:0.#}%",
                radon.ScrubberHealthPercent < 40f
                    ? AshfallUiHelpers.ToColor(DesignTheme.Critical)
                    : AshfallUiHelpers.ToColor(DesignTheme.Pale)));
            _telemetry.AddChild(ThreePanePanelScaffold.CreateTelemetryRow(
                "LOGGED ALPHA DOSE", $"{radon.State.totalAlphaDoseLogged:0.###} mSv",
                AshfallUiHelpers.ToColor(DesignTheme.Muted)));

            _vectors.AddChild(ThreePanePanelScaffold.CreateTelemetryRow(
                "ACTIVE FOUNDATION FISSURES", $"{radon.ActiveFissures}",
                radon.ActiveFissures > 0
                    ? AshfallUiHelpers.ToColor(DesignTheme.Warning)
                    : AshfallUiHelpers.ToColor(DesignTheme.Pale)));
            _vectors.AddChild(ThreePanePanelScaffold.CreateTelemetryRow(
                "SCRUBBER ALARM",
                radon.State.isScrubberAlarmActive ? "ACTIVE" : "CLEAR",
                radon.State.isScrubberAlarmActive
                    ? AshfallUiHelpers.ToColor(DesignTheme.Critical)
                    : AshfallUiHelpers.ToColor(DesignTheme.Success)));
            _vectors.AddChild(ThreePanePanelScaffold.CreateTelemetryRow(
                "DANGER THRESHOLD", $"{YearOfAshRadonSystem.DangerousRadonThreshold:0} Bq/m³",
                AshfallUiHelpers.ToColor(DesignTheme.Dim)));
        }

        private static void ClearChildren(Node parent)
        {
            for (int i = parent.GetChildCount() - 1; i >= 0; i--)
            {
                var child = parent.GetChild(i);
                parent.RemoveChild(child);
                child.Free();
            }
        }
    }
}
