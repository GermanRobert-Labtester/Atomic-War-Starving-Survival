using System;
using System.Linq;
#pragma warning disable CS8618
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Research panel.
    /// Shows discovered knowledge, active research, and unlocked tech — bound
    /// to the live ResearchHostSession. Unbound renders an honest empty state.
    /// </summary>
    public partial class ResearchPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblKnowledgeTitle;
        private VBoxContainer _knowledgeList;
        private Label _lblResearchTitle;
        private VBoxContainer _researchList;
        private Label _lblTechTitle;
        private VBoxContainer _techList;

        private ResearchSystem? _research;

        public bool IsBound => _research != null;
        public int RenderedRowCount { get; private set; }

        public void Bind(ResearchSystem? research)
        {
            _research = research;
            RefreshView();
        }

        public void RefreshView()
        {
            if (_knowledgeList == null || _researchList == null || _techList == null) return;

            AshfallUiHelpers.EmptyChildren(_knowledgeList);
            AshfallUiHelpers.EmptyChildren(_researchList);
            AshfallUiHelpers.EmptyChildren(_techList);

            RenderedRowCount = 0;

            if (_research == null)
            {
                _knowledgeList.AddChild(MakeDimLine("No research session bound."));
                return;
            }

            // ── Unlocked knowledge (catalog entries that are unlocked) ──
            foreach (var kv in _research.Catalog.OrderBy(k => k.Key))
            {
                if (!_research.IsManualUnlocked(kv.Key)) continue;
                AddRow(_knowledgeList, $"{kv.Value.displayName} — {kv.Value.category}",
                    Ashfall.Core.UI.Theme.Lethe);
                RenderedRowCount++;
            }
            if (RenderedRowCount == 0)
                _knowledgeList.AddChild(MakeDimLine("No knowledge unlocked yet."));

            // ── Active research ──
            var active = _research.GetActiveResearch();
            if (active != null)
            {
                AddRow(_researchList, $"{active.displayName} — Day {_research.State.activeResearchDays} in progress",
                    Ashfall.Core.UI.Theme.Warm);
            }
            else
            {
                _researchList.AddChild(MakeDimLine("No active research."));
            }

            // ── Completed tech ──
            int completed = 0;
            foreach (var id in _research.State.completedIds)
            {
                var def = _research.Catalog.TryGetValue(id, out var d) ? d : null;
                AddRow(_techList, def != null ? $"{def.displayName} — Complete" : $"{id} — Complete",
                    Ashfall.Core.UI.Theme.Pale);
                completed++;
            }
            if (completed == 0)
                _techList.AddChild(MakeDimLine("No completed research."));
        }

        private void AddRow(VBoxContainer parent, string text, (float r, float g, float b, float a) col)
        {
            var label = new Label { Text = text };
            label.CustomMinimumSize = new Vector2(400, 0);
            label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(col));
            parent.AddChild(label);
        }

        private Label MakeDimLine(string text)
        {
            var l = new Label { Text = text };
            l.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            l.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            return l;
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.05f, 0.05f, 0.05f, 0.92f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var container = new CenterContainer();
            container.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(container);

            var vbox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingLg);
            vbox.CustomMinimumSize = new Vector2(550, 0);
            container.AddChild(vbox);

            var title = AshfallUiHelpers.MakeTitle("RESEARCH & TECHNOLOGY", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Knowledge section
            _lblKnowledgeTitle = AshfallUiHelpers.MakeSectionHeader("DISCOVERED KNOWLEDGE");
            vbox.AddChild(_lblKnowledgeTitle);

            _knowledgeList = new VBoxContainer();
            _knowledgeList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _knowledgeList.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_knowledgeList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Research progress section
            _lblResearchTitle = AshfallUiHelpers.MakeSectionHeader("ACTIVE RESEARCH");
            vbox.AddChild(_lblResearchTitle);

            _researchList = new VBoxContainer();
            _researchList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _researchList.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_researchList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Unlocked tech section
            _lblTechTitle = AshfallUiHelpers.MakeSectionHeader("UNLOCKED TECHNOLOGY");
            vbox.AddChild(_lblTechTitle);

            _techList = new VBoxContainer();
            _techList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _techList.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_techList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc]", () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(200, 40);
            vbox.AddChild(btnClose);

            var hint = AshfallUiHelpers.MakeSmall("[Esc] to close");
            hint.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeLabel);
            hint.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            vbox.AddChild(hint);
        }

        public void Open()
        {
            Visible = true;
            QueueRedraw();
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;

            if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
            {
                OnClose?.Invoke();
                GetViewport().SetInputAsHandled();
            }
        }
    }
}
