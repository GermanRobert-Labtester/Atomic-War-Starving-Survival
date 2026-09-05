// SPDX-License-Identifier: MIT
// ASHFALL Campaign Endgame Chronicle & Epilogue UI Panel (Plan 84 / Task B25).

using System;
using Godot;
using Ashfall.Core.Endgame;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Campaign closure panel presenting the definitive ending chronicle,
    /// memorial roll, faction reactions, and one-way campaign sealing.
    /// </summary>
    public partial class ChroniclePanel : Control
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail _statusRail = null!;
        private EndgameHostSession? _host;
        private EndgameSystem? _system;

        private Label _lblEndingTitle = null!;
        private Label _lblEndingCategory = null!;
        private Label _lblMainProse = null!;
        private VBoxContainer _memorialContainer = null!;
        private VBoxContainer _factionsContainer = null!;
        private Label _lblMetrics = null!;
        private Button _btnSeal = null!;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            BuildUi();
        }

        public void Bind(EndgameHostSession? host)
        {
            if (_host != null)
            {
                _host.StateChanged -= Refresh;
            }
            _host = host;
            _system = host?.System;
            if (_host != null)
            {
                _host.StateChanged += Refresh;
            }
            Refresh();
        }

        public void Bind(EndgameSystem? system)
        {
            _system = system;
            Refresh();
        }

        private void BuildUi()
        {
            _shell = new AshfallDashboardShell("CAMPAIGN CHRONICLE // EPILOGUE ARCHIVE", minWidth: 1100, minHeight: 700);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("phase", "Phase", "ACTIVE", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("sealed", "Archive Sealed", "NO", AshfallMetricCard.Criticality.Normal, minWidth: 130);

            var scroll = new ScrollContainer
            {
                CustomMinimumSize = new Vector2(1060, 560),
                SizeFlagsVertical = SizeFlags.ExpandFill,
                SizeFlagsHorizontal = SizeFlags.ExpandFill
            };

            var contentBox = new VBoxContainer();
            contentBox.AddThemeConstantOverride("separation", 16);
            contentBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;

            // Header card
            var headerCard = AshfallUiHelpers.MakeCardFrame("FINAL DESTINY");
            var headerInner = new VBoxContainer();
            _lblEndingTitle = AshfallUiHelpers.MakeTitle("Awaiting Campaign Closure");
            _lblEndingCategory = AshfallUiHelpers.MakeBody("Status: Active Campaign — Final outcome unresolved.");
            headerInner.AddChild(_lblEndingTitle);
            headerInner.AddChild(_lblEndingCategory);
            headerCard.AddChild(headerInner);
            contentBox.AddChild(headerCard);

            // Epilogue prose card
            var proseCard = AshfallUiHelpers.MakeCardFrame("EPILOGUE CHRONICLE");
            _lblMainProse = AshfallUiHelpers.MakeBody(
                "The blast doors remain sealed while the struggle for day-to-day survival continues across the shelter sectors.");
            proseCard.AddChild(_lblMainProse);
            contentBox.AddChild(proseCard);

            // Memorial tributes card
            var memorialCard = AshfallUiHelpers.MakeCardFrame("HONOR ROLL & MEMORIALS");
            _memorialContainer = new VBoxContainer();
            _memorialContainer.AddThemeConstantOverride("separation", 6);
            memorialCard.AddChild(_memorialContainer);
            contentBox.AddChild(memorialCard);

            // Faction legacy card
            var factionCard = AshfallUiHelpers.MakeCardFrame("REGIONAL FACTION LEGACY");
            _factionsContainer = new VBoxContainer();
            _factionsContainer.AddThemeConstantOverride("separation", 6);
            factionCard.AddChild(_factionsContainer);
            contentBox.AddChild(factionCard);

            // Campaign metrics card
            var metricsCard = AshfallUiHelpers.MakeCardFrame("FINAL RECORD METRICS");
            _lblMetrics = AshfallUiHelpers.MakeBody("Days Survived: 0 | Living Dwellers: 0 | Casualties: 0 | Expeditions: 0");
            metricsCard.AddChild(_lblMetrics);
            contentBox.AddChild(metricsCard);

            // Action row
            var actionRow = new HBoxContainer();
            actionRow.AddThemeConstantOverride("separation", 12);
            _btnSeal = AshfallUiHelpers.MakeButton("SEAL CAMPAIGN ARCHIVE", () => OnSealClicked());
            actionRow.AddChild(_btnSeal);
            contentBox.AddChild(actionRow);

            scroll.AddChild(contentBox);
            _shell.SetContent(scroll);
        }

        public void Refresh()
        {
            if (!IsInsideTree() || _lblEndingTitle == null) return;

            var epilogue = _system?.State?.epilogueReport;
            bool isSealed = _system?.IsSealed ?? false;
            var phase = _system?.Phase ?? EndgamePhase.Active;

            _statusRail.Set("phase", phase.ToString().ToUpperInvariant(),
                phase == EndgamePhase.Sealed ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Warn);
            _statusRail.Set("sealed", isSealed ? "YES" : "NO",
                isSealed ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Caution);

            if (epilogue != null)
            {
                _lblEndingTitle.Text = epilogue.endingTitle;
                _lblEndingCategory.Text = $"Tone: {epilogue.tone.ToUpperInvariant()} | Sealed Day: {epilogue.sealedDay}";
                _lblMainProse.Text = epilogue.mainEpilogueProse;

                // Populate memorials
                foreach (Node child in _memorialContainer.GetChildren())
                    child.QueueFree();
                foreach (string tribute in epilogue.memorialTributes)
                {
                    _memorialContainer.AddChild(AshfallUiHelpers.MakeBody($"• {tribute}"));
                }

                // Populate faction reactions
                foreach (Node child in _factionsContainer.GetChildren())
                    child.QueueFree();
                foreach (string reaction in epilogue.factionReactions)
                {
                    _factionsContainer.AddChild(AshfallUiHelpers.MakeBody($"• {reaction}"));
                }

                _lblMetrics.Text = $"Days Survived: {epilogue.daysSurvived}  |  Living: {epilogue.livingSurvivors}  |  Deceased: {epilogue.deceasedSurvivors}  |  Morale: {epilogue.finalMoraleAverage:F1}%  |  Expeditions: {epilogue.expeditionsCompleted}";
            }

            if (isSealed)
            {
                _btnSeal.Text = "CAMPAIGN SEALED — RECORD IMMUTABLE";
                _btnSeal.Disabled = true;
            }
            else if (phase == EndgamePhase.Epilogue)
            {
                _btnSeal.Text = "SEAL CAMPAIGN & FREEZE ARCHIVE";
                _btnSeal.Disabled = false;
            }
            else
            {
                _btnSeal.Text = "CAMPAIGN ACTIVE (NOT READY TO SEAL)";
                _btnSeal.Disabled = true;
            }
        }

        private void OnSealClicked()
        {
            if (_host != null && !_host.IsSealed)
            {
                int day = _system?.State?.epilogueReport?.daysSurvived ?? 1;
                _host.SealCampaign(day);
                Refresh();
            }
        }
    }
}
