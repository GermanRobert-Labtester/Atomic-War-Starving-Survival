using System;
using Godot;
using AtomicWar.GodotApp.UI;
using Ashfall.Core.UI;
using Ashfall.Core.UtilityAI;

namespace AtomicWar.GodotApp.UtilityAI
{
    /// <summary>
    /// Thin Godot panel: renders the action catalog and the demo survivor's
    /// current selection with scores. Presentation only; zero rules.
    /// </summary>
    public partial class UtilityAiPanel : PanelContainer
    {
        private UtilityAiHostSession _session;
        private VBoxContainer _actionList;
        private Label _lblSelection;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.TopRight);
            CustomMinimumSize = new Vector2(400, 260);

            // Apply standard panel 9-slice via shared helper (frame_9slice first)
            AddThemeStyleboxOverride("panel", AtomicWar.GodotApp.UI.AshfallUiHelpers.MakePanelFrameStyleBox());

            var rootVbox = new VBoxContainer();
            rootVbox.AddThemeConstantOverride("separation", 6);
            AddChild(rootVbox);

            var title = new Label
            {
                Text = "UTILITY AI — COMPANION DECISIONS",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            title.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            rootVbox.AddChild(title);

            _lblSelection = new Label { Text = "No selection yet." };
            rootVbox.AddChild(_lblSelection);

            var scroll = new ScrollContainer
            {
                HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled,
                CustomMinimumSize = new Vector2(0, 180)
            };
            rootVbox.AddChild(scroll);

            _actionList = new VBoxContainer();
            scroll.AddChild(_actionList);
        }

        public void BindSession(UtilityAiHostSession session)
        {
            _session = session;
            if (_session != null)
                _session.StateChanged += RefreshView;
        }

        public void UnbindSession()
        {
            if (_session == null) return;
            _session.StateChanged -= RefreshView;
            _session = null;
        }

        public override void _ExitTree()
        {
            UnbindSession();
            base._ExitTree();
        }

        public void RefreshView()
        {
            if (_session == null) return;

            foreach (Node child in _actionList.GetChildren())
                child.QueueFree();

            _lblSelection.Text = string.IsNullOrEmpty(_session.LastEvent)
                ? "No selection yet."
                : _session.LastEvent;

            var ctx = new AIActionContext
            {
                SurvivorId = "demo",
                IsAlive = true,
                Fatigue = 30f,
                CraftingSkill = 0.5f
            };
            var scorer = new UtilityActionScorer();
            var scored = new UtilityAiSystem().ScoreAll(ctx, _session.Actions, scorer);
            for (int i = 0; i < scored.Count; i++)
            {
                var row = new Label
                {
                    Text = $"{scored[i].Key.displayName} — {scored[i].Value:0.00} " +
                           (scored[i].Key.HasTag(UtilityTags.TagLoudLabor) ? " [loud]" : "")
                };
                row.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeSmall);
                _actionList.AddChild(row);
            }
        }
    }
}
