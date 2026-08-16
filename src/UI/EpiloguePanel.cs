using System;
using Godot;
using Ashfall.Core.Endgame;
using Ashfall.Core.UI;
using CoreTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Endgame Epilogue Panel.
    /// Evaluates whole-saga world state across 32 matrix permutations, generating the
    /// authoritative literary-grade chronicle of the wasteland.
    ///
    /// Presentation only — evaluates EpilogueMatrixRuntime against simulation state.
    /// </summary>
    public partial class EpiloguePanel : Control
    {
        public event Action? OnClose;

        private readonly EpilogueMatrixRuntime _runtime = new EpilogueMatrixRuntime();
        private EpilogueEvaluationContext _context = new EpilogueEvaluationContext();
        private VBoxContainer _outcomesContainer = null!;
        private Label _narrativeLabel = null!;
        private Label _statusLabel = null!;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            BuildLayout();
            Visible = false;
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;
            if (@event is InputEventKey key && key.Pressed && !key.Echo && key.Keycode == Key.Escape)
            {
                Close();
                GetViewport().SetInputAsHandled();
            }
        }

        public void Bind(
            int daysSurvived,
            int livingCount,
            int deathsCount,
            bool grandTreaty,
            bool tempestDecom,
            bool ledgersBurned,
            bool childrenAlive,
            bool velExposed)
        {
            _context = new EpilogueEvaluationContext
            {
                totalDaysSurvived = daysSurvived,
                livingDwellerCount = livingCount,
                totalDeathsRecorded = deathsCount,
                grandTreatySigned = grandTreaty,
                tempestDecommissioned = tempestDecom,
                debtLedgersBurned = ledgersBurned,
                childrenSurvived = childrenAlive,
                velSecretExposed = velExposed
            };
            RefreshView();
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
        }

        public void Close()
        {
            Visible = false;
            OnClose?.Invoke();
        }

        private void BuildLayout()
        {
            var backdrop = new ColorRect
            {
                Color = new Color(0.03f, 0.04f, 0.05f, 0.95f)
            };
            backdrop.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(backdrop);

            var margin = new MarginContainer();
            margin.SetAnchorsPreset(LayoutPreset.FullRect);
            margin.AddThemeConstantOverride("margin_left", (int)CoreTheme.SpacingLg);
            margin.AddThemeConstantOverride("margin_right", (int)CoreTheme.SpacingLg);
            margin.AddThemeConstantOverride("margin_top", (int)CoreTheme.SpacingLg);
            margin.AddThemeConstantOverride("margin_bottom", (int)CoreTheme.SpacingLg);
            AddChild(margin);

            var mainVBox = new VBoxContainer();
            mainVBox.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingMd);
            margin.AddChild(mainVBox);

            // ── Header Card ──
            var headerCard = AshfallUiHelpers.MakeCardFrame(
                "THE CHRONICLE OF TESSARAT // ENDGAME EPILOGUE MATRIX",
                "Thirty-two permutation whole-saga evaluation of regional fate, demographic outcomes, moral standing, and wasteland survival legacy."
            );
            mainVBox.AddChild(headerCard);

            // ── Scrollable Body ──
            var scroll = new ScrollContainer
            {
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                SizeFlagsVertical = SizeFlags.ExpandFill,
                HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled
            };
            mainVBox.AddChild(scroll);

            var contentBox = new VBoxContainer();
            contentBox.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingMd);
            contentBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            scroll.AddChild(contentBox);

            _outcomesContainer = new VBoxContainer();
            _outcomesContainer.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingSm);
            contentBox.AddChild(_outcomesContainer);

            contentBox.AddChild(AshfallUiHelpers.MakeSectionHeader("LITERARY CHRONICLE & REGIONAL OUTCOME"));

            var narrCard = AshfallUiHelpers.MakePanel();
            var narrMargin = AshfallUiHelpers.MakeMargins((int)CoreTheme.SpacingSm);
            narrCard.AddChild(narrMargin);

            var nBox = new VBoxContainer();
            nBox.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingSm);
            narrMargin.AddChild(nBox);

            _narrativeLabel = AshfallUiHelpers.MakeBody("Evaluating wasteland chronicle...");
            nBox.AddChild(_narrativeLabel);

            contentBox.AddChild(narrCard);

            // ── Bottom Action Bar ──
            var bottomBar = new HBoxContainer();
            bottomBar.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingMd);
            mainVBox.AddChild(bottomBar);

            var btnClose = AshfallUiHelpers.MakeButton("RETURN TO EXPANSION HUB [ESC]", Close);
            btnClose.CustomMinimumSize = new Vector2(240, 44);
            bottomBar.AddChild(btnClose);

            _statusLabel = AshfallUiHelpers.MakeMono("Chronicle matrix evaluated against active world ledger flags.");
            _statusLabel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            bottomBar.AddChild(_statusLabel);
        }

        public void RefreshView()
        {
            if (_outcomesContainer == null || _narrativeLabel == null) return;

            ClearContainer(_outcomesContainer);

            var fate = _runtime.EvaluateRegionalFate(_context);
            var demo = _runtime.EvaluateDemographics(_context);
            var moral = _runtime.EvaluateMoralStanding(_context);

            var outCard = AshfallUiHelpers.MakePanel();
            var outMargin = AshfallUiHelpers.MakeMargins((int)CoreTheme.SpacingSm);
            outCard.AddChild(outMargin);

            var oBox = new VBoxContainer();
            oBox.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingXs);
            outMargin.AddChild(oBox);

            oBox.AddChild(AshfallUiHelpers.MakeSectionHeader("EVALUATED HISTORICAL OUTCOMES"));
            oBox.AddChild(AshfallUiHelpers.MakeDataRow("Regional Fate", FormatEnum(fate), AshfallUiHelpers.ToColor(CoreTheme.Hot)));
            oBox.AddChild(AshfallUiHelpers.MakeDataRow("Demographic Legacy", FormatEnum(demo), AshfallUiHelpers.ToColor(CoreTheme.Warm)));
            oBox.AddChild(AshfallUiHelpers.MakeDataRow("Moral Standing", FormatEnum(moral), AshfallUiHelpers.ToColor(CoreTheme.Pale)));
            oBox.AddChild(AshfallUiHelpers.MakeDataRow("Days Survived", $"{_context.totalDaysSurvived} Days", AshfallUiHelpers.ToColor(CoreTheme.Pale)));
            oBox.AddChild(AshfallUiHelpers.MakeDataRow("Living Dwellers", $"{_context.livingDwellerCount} Active", AshfallUiHelpers.ToColor(CoreTheme.Pale)));
            oBox.AddChild(AshfallUiHelpers.MakeDataRow("Inscribed Deaths", $"{_context.totalDeathsRecorded} Losses", AshfallUiHelpers.ToColor(CoreTheme.Critical)));

            _outcomesContainer.AddChild(outCard);

            _narrativeLabel.Text = _runtime.GenerateEpilogueNarrative(_context);
        }

        private static string FormatEnum<T>(T val) where T : struct
        {
            string s = val.ToString() ?? string.Empty;
            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                if (i > 0 && char.IsUpper(s[i])) sb.Append(' ');
                sb.Append(s[i]);
            }
            return sb.ToString();
        }

        private static void ClearContainer(VBoxContainer container)
        {
            if (container == null) return;
            while (container.GetChildCount() > 0)
            {
                var child = container.GetChild(0);
                container.RemoveChild(child);
                child.QueueFree();
            }
        }
    }
}
