// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using Godot;
using Ashfall.Core.Shelter;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Cryogenic permafrost core surface (UI/UX audit 2026-09-25 — converted
    /// from a static prototype shell into a truthful read-only projection).
    ///
    /// Authority: <see cref="CryoVaultSystem"/> (Plan B69) — canister
    /// viability/phase, coolant reserve, insulation tier, breach state, and the
    /// released-sample log. No local state: values are refreshed from the
    /// system on every bind and open.
    /// </summary>
    public partial class CryogenicPermafrostCorePanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private Label _status = null!;
        private VBoxContainer _vault = null!;
        private VBoxContainer _canisters = null!;
        private CryoVaultSystem? _cryo;

        public bool IsBound { get; private set; }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            BuildInterface();
        }

        /// <summary>Binds the live cryo vault system.</summary>
        public void Bind(CryoVaultSystem? cryo)
        {
            _cryo = cryo;
            IsBound = cryo != null;
            RefreshView();
        }

        public void Unbind()
        {
            _cryo = null;
            IsBound = false;
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
        }

        private void BuildInterface()
        {
            var chrome = ThreePanePanelScaffold.BuildChrome(
                this,
                "CRYOGENIC PERMAFROST CORE // SAMPLE PRESERVATION VAULT",
                "[WAITING FOR SESSION]",
                AshfallUiHelpers.ToColor(DesignTheme.Dim),
                "[X] CLOSE CONSOLE",
                "Vault telemetry reads from the live CryoVaultSystem; values refresh on open.",
                () => OnClose?.Invoke());
            _status = chrome.Status;
            var body = chrome.Body;

            var left = ThreePanePanelScaffold.CreatePanelFrame("VAULT CONDITION");
            body.AddChild(left);
            _vault = ThreePanePanelScaffold.CreateColumn(left, DesignTheme.SpacingSm);

            var right = ThreePanePanelScaffold.CreatePanelFrame("STABLED CANISTERS");
            body.AddChild(right);
            _canisters = ThreePanePanelScaffold.CreateColumn(right, DesignTheme.SpacingSm);

            RefreshView();
        }

        public void RefreshView()
        {
            ClearChildren(_vault);
            ClearChildren(_canisters);
            if (_cryo == null)
            {
                _status.Text = "[NOT CONNECTED — CRYO VAULT NOT INITIALIZED]";
                _status.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Dim));
                _canisters.AddChild(AshfallUiHelpers.MakeBody(
                    "Cryo authority unavailable — start or load a campaign to read live vault state."));
                return;
            }

            var state = _cryo.State;
            int canisterCount = state.canisters?.Count ?? 0;
            float coolantPct = state.coolant_reserve / CryoVaultSystem.MaxCoolant * 100f;

            _status.Text = state.breach_active
                ? $"BREACH ACTIVE — {state.breach_reason}"
                : $"VAULT SEALED — {canisterCount} CANISTER(S) STABLED";
            _status.AddThemeColorOverride("font_color",
                state.breach_active
                    ? AshfallUiHelpers.ToColor(DesignTheme.Critical)
                    : AshfallUiHelpers.ToColor(DesignTheme.Success));

            _vault.AddChild(ThreePanePanelScaffold.CreateTelemetryRow(
                "COOLANT RESERVE", $"{state.coolant_reserve:0.#} / {CryoVaultSystem.MaxCoolant:0} ({coolantPct:0}%)",
                coolantPct < 25f
                    ? AshfallUiHelpers.ToColor(DesignTheme.Critical)
                    : coolantPct < 50f
                        ? AshfallUiHelpers.ToColor(DesignTheme.Warning)
                        : AshfallUiHelpers.ToColor(DesignTheme.Pale)));
            _vault.AddChild(ThreePanePanelScaffold.CreateTelemetryRow(
                "INSULATION TIER", $"{state.insulation_level} / {CryoVaultSystem.MaxInsulationLevel}",
                AshfallUiHelpers.ToColor(DesignTheme.Muted)));
            _vault.AddChild(ThreePanePanelScaffold.CreateTelemetryRow(
                "BREACH STATUS",
                state.breach_active ? $"ACTIVE SINCE DAY {state.breach_started_day}" : "NONE",
                state.breach_active
                    ? AshfallUiHelpers.ToColor(DesignTheme.Critical)
                    : AshfallUiHelpers.ToColor(DesignTheme.Success)));
            _vault.AddChild(ThreePanePanelScaffold.CreateTelemetryRow(
                "SAMPLES RELEASED", $"{state.released_log?.Count ?? 0}",
                AshfallUiHelpers.ToColor(DesignTheme.Muted)));

            if (canisterCount == 0)
            {
                _canisters.AddChild(AshfallUiHelpers.MakeBody("No samples stabled in this vault."));
                return;
            }

            foreach (var canister in state.canisters!.Take(8))
            {
                float viabilityPct = canister.viability_permille / 10f;
                string phase = ((CryoCanisterPhase)canister.phase).ToString().ToUpperInvariant();
                string protectedTag = canister.triage_protected ? " · TRIAGE-PROTECTED" : "";
                _canisters.AddChild(ThreePanePanelScaffold.CreateTelemetryRow(
                    $"{canister.cultivar_id} [{phase}]",
                    $"{viabilityPct:0.#}% VIABLE{protectedTag}",
                    viabilityPct < 40f
                        ? AshfallUiHelpers.ToColor(DesignTheme.Critical)
                        : viabilityPct < 70f
                            ? AshfallUiHelpers.ToColor(DesignTheme.Warning)
                            : AshfallUiHelpers.ToColor(DesignTheme.Pale)));
            }

            if (canisterCount > 8)
            {
                _canisters.AddChild(ThreePanePanelScaffold.CreateTelemetryRow(
                    "…", $"+{canisterCount - 8} MORE CANISTER(S)",
                    AshfallUiHelpers.ToColor(DesignTheme.Dim)));
            }
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
