using UnityEngine.UIElements;

namespace AtomicWar._Game.UI.MainMenu
{
    /// <summary>
    /// The two ambient animations USS cannot express on its own, since UI
    /// Toolkit has no @keyframes: the background's slow Ken-Burns drift, and
    /// the menu rows' staggered entrance.
    ///
    /// Both are driven by <c>schedule</c> toggling a class, and both wait for
    /// the element's first <see cref="GeometryChangedEvent"/> before starting
    /// — a USS `transition` does not animate away from a style that has not
    /// resolved yet, so starting a class toggle on the very first frame would
    /// just snap to the end state instead of easing into it.
    /// </summary>
    public sealed partial class MainMenuController
    {
        /// <summary>How often the Ken-Burns drift target flips, in ms. Matches the 60s USS transition-duration.</summary>
        private const long KenBurnsIntervalMs = 60_000;

        /// <summary>Delay before the first menu row starts entering, in ms.</summary>
        private const long EntryBaseDelayMs = 120;

        /// <summary>Extra delay per row after the first, in ms.</summary>
        private const long EntryStepDelayMs = 70;

        private VisualElement _sceneImage;
        private bool _animationsArmed;

        private void ArmAnimations()
        {
            if (_animationsArmed) return;
            _animationsArmed = true;

            _sceneImage = _root.Q<VisualElement>("scene-image");

            ScheduleEntryStagger();

            if (_sceneImage != null)
            {
                // First GeometryChangedEvent = the element has a real layout
                // rect and its initial style has resolved, so unregister
                // immediately: the drift toggle only needs one starting point.
                _sceneImage.RegisterCallback<GeometryChangedEvent>(OnSceneImageFirstLayout);
            }
        }

        private void OnSceneImageFirstLayout(GeometryChangedEvent evt)
        {
            _sceneImage.UnregisterCallback<GeometryChangedEvent>(OnSceneImageFirstLayout);
            StartKenBurns();
        }

        private void StartKenBurns()
        {
            // Flip the class on a fixed interval; the USS transition (60s,
            // linear) does the actual easing between the two end states, so
            // this only ever needs to be a toggle, not a per-frame tween.
            _sceneImage.schedule.Execute(() =>
            {
                _sceneImage.ToggleInClassList("ken-burns-drift");
            }).Every(KenBurnsIntervalMs);
        }

        /// <summary>
        /// Fade + rise each row in top-to-bottom order. Runs once per Build();
        /// rows rebuilt later (there is currently no reason to rebuild them)
        /// would simply replay the same stagger.
        /// </summary>
        private void ScheduleEntryStagger()
        {
            for (int i = 0; i < _rows.Count; i++)
            {
                Button row = _rows[i];
                long delay = EntryBaseDelayMs + i * EntryStepDelayMs;
                row.schedule.Execute(() => row.AddToClassList("is-entered")).StartingIn(delay);
            }
        }
    }
}
