using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.UI
{
    /// <summary>Phase 11 — radiation sickness phase dot indicator with medical tooltip.</summary>
    public class RadiationPhaseIndicator : MonoBehaviour
    {
        public event Action<string, RadiationSicknessPhase> OnPhaseChanged;

        [SerializeField] private UIDocument _document;

        private VisualElement _root;
        private VisualElement _dot;
        private Label _phaseLabel;
        private readonly Dictionary<string, RadiationSicknessPhase> _phases = new();

        private string _focusedSurvivorId;
        private string _tooltipText = "";

        [Serializable]
        public struct SaveState
        {
            public string focusedSurvivorId;
            public RadiationSicknessPhase focusedPhase;
        }

        public static Color GetColorForPhase(RadiationSicknessPhase phase) => phase switch
        {
            RadiationSicknessPhase.Healthy => new Color(0.298f, 0.686f, 0.314f),
            RadiationSicknessPhase.Prodromal => new Color(1f, 0.757f, 0.027f),
            RadiationSicknessPhase.Latent => new Color(0.298f, 0.686f, 0.314f),
            RadiationSicknessPhase.ManifestIllness => new Color(0.957f, 0.263f, 0.212f),
            RadiationSicknessPhase.ChronicFibrosis => new Color(0.620f, 0.620f, 0.620f),
            RadiationSicknessPhase.RecoveryOrDeath => new Color(0.129f, 0.588f, 0.953f),
            _ => Color.gray
        };

        public SaveState CaptureState()
        {
            var phase = RadiationSicknessPhase.Healthy;
            if (!string.IsNullOrEmpty(_focusedSurvivorId))
                _phases.TryGetValue(_focusedSurvivorId, out phase);
            return new SaveState { focusedSurvivorId = _focusedSurvivorId, focusedPhase = phase };
        }

        public void RestoreState(SaveState s)
        {
            if (!string.IsNullOrEmpty(s.focusedSurvivorId))
                SetPhase(s.focusedSurvivorId, s.focusedPhase);
        }

        /// <summary>Bind to the shared DiegeticHud UIDocument (elements live in DiegeticHud.uxml).</summary>
        public void BindDocument(UIDocument document)
        {
            _document = document;
            BindElements();
            Refresh();
        }

        private void OnEnable()
        {
            if (_document == null) _document = GetComponent<UIDocument>();
            BindElements();
            Refresh();
        }

        private void BindElements()
        {
            if (_document == null || _document.rootVisualElement == null) return;
            _root = _document.rootVisualElement.Q("radiation-phase-root");
            if (_root == null) return;
            _dot = _root.Q("radiation-phase-dot");
            _phaseLabel = _root.Q<Label>("radiation-phase-label");
            _root.RegisterCallback<PointerEnterEvent>(_ => { if (_root != null) _root.tooltip = _tooltipText; });
        }

        public void SetFocusedSurvivor(string survivorId)
        {
            _focusedSurvivorId = survivorId;
            Refresh();
        }

        public void SetPhase(string survivorId, RadiationSicknessPhase phase)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            _phases[survivorId] = phase;
            if (survivorId == _focusedSurvivorId || string.IsNullOrEmpty(_focusedSurvivorId))
            {
                _focusedSurvivorId = survivorId;
                Refresh();
            }
            OnPhaseChanged?.Invoke(survivorId, phase);
        }

        public void SetTooltipText(string text) => _tooltipText = text ?? "";

        public void Show() => _root?.RemoveFromClassList("hidden");
        public void Hide() => _root?.AddToClassList("hidden");

        private void Refresh()
        {
            if (_root == null || _dot == null) return;
            if (string.IsNullOrEmpty(_focusedSurvivorId) ||
                !_phases.TryGetValue(_focusedSurvivorId, out var phase))
            {
                phase = RadiationSicknessPhase.Healthy;
            }

            _dot.style.backgroundColor = GetColorForPhase(phase);
            _dot.EnableInClassList("radiation-phase-dot--prodromal", phase == RadiationSicknessPhase.Prodromal);
            _dot.EnableInClassList("radiation-phase-dot--latent", phase == RadiationSicknessPhase.Latent);
            _dot.EnableInClassList("radiation-phase-dot--manifest", phase == RadiationSicknessPhase.ManifestIllness);
            _dot.EnableInClassList("radiation-phase-dot--fibrosis", phase == RadiationSicknessPhase.ChronicFibrosis);
            _dot.EnableInClassList("radiation-phase-dot--recovery", phase == RadiationSicknessPhase.RecoveryOrDeath);
            _dot.EnableInClassList("radiation-phase-dot--pulse",
                phase == RadiationSicknessPhase.Prodromal || phase == RadiationSicknessPhase.ManifestIllness);

            if (_phaseLabel != null)
                _phaseLabel.text = phase == RadiationSicknessPhase.Healthy ? "" : phase.ToString().ToUpper();

            if (phase == RadiationSicknessPhase.Healthy)
                Hide();
            else
                Show();
        }
    }
}
