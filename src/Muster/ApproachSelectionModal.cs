using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using Godot;
using AtomicWar.GodotApp.UI;
using Ashfall.Core.UI;
using Ashfall.Core.Muster;

namespace AtomicWar.GodotApp.Muster
{
    /// <summary>
    /// Generic Approach-fork modal (Expansion 06 pattern, Section XIII): driven
    /// entirely by a data-defined list of ApproachOption, reused across every
    /// Muster questline. Thin presentation only; selection is forwarded to the
    /// host, which validates against MusterSystem.
    /// </summary>
    public partial class ApproachSelectionModal : PanelContainer
    {
        public event Action<QuestApproach> OnApproachChosen;
        public event Action OnModalClosed;

        private string _questlineId = string.Empty;
        private VBoxContainer _choicesContainer;
        private Label _lblTitle;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.Center);
            CustomMinimumSize = new Vector2(520, 0);

            var rootVbox = new VBoxContainer();
            rootVbox.AddThemeConstantOverride("separation", 8);
            AddChild(rootVbox);

            _lblTitle = new Label
            {
                Text = "CHOOSE AN APPROACH",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            _lblTitle.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            rootVbox.AddChild(_lblTitle);

            _choicesContainer = new VBoxContainer();
            rootVbox.AddChild(_choicesContainer);

            var closeButton = new Button { Text = "Close" };
            closeButton.Pressed += Close;
            rootVbox.AddChild(closeButton);
        }

        public void ShowQuestline(string questlineId, IReadOnlyList<ApproachOption> approaches)
        {
            _questlineId = questlineId;
            if (_lblTitle != null)
                _lblTitle.Text = questlineId.ToUpperInvariant() + " — CHOOSE AN APPROACH";

            if (_choicesContainer == null) return;
            foreach (Node child in _choicesContainer.GetChildren())
                child.QueueFree();

            for (int i = 0; i < approaches.Count; i++)
            {
                var option = approaches[i];
                var button = new Button
                {
                    Text = $"{option.approach} — {option.label}\n{option.description}",
                    AutowrapMode = TextServer.AutowrapMode.WordSmart,
                    CustomMinimumSize = new Vector2(0, 56)
                };
                button.Pressed += () => Choose(option.approach);
                _choicesContainer.AddChild(button);
            }
        }

        private void Choose(QuestApproach approach)
        {
            OnApproachChosen?.Invoke(approach);
            Close();
        }

        private void Close() => OnModalClosed?.Invoke();
    }
}
