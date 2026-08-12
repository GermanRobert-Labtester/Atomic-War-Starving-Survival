using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    /// <summary>Phase 11 — hypervigilance eye badge on survivor portrait.</summary>
    public class HypervigilanceIndicator : MonoBehaviour
    {
        public const float HiddenThreshold = 0.3f;
        public const float AlertThreshold = 0.6f;
        public const float FalseAlarmFlashSeconds = 0.5f;

        public event Action<string, float> OnLevelChanged;

        [SerializeField] private UIDocument _document;

        private VisualElement _root;
        private Label _eye;
        private VisualElement _flash;
        private readonly Dictionary<string, float> _levels = new();
        private string _focusedSurvivorId;
        private Coroutine _flashRoutine;

        [Serializable]
        public struct SaveState
        {
            public string focusedSurvivorId;
            public float focusedLevel;
        }

        public SaveState CaptureState()
        {
            float level = 0f;
            if (!string.IsNullOrEmpty(_focusedSurvivorId))
                _levels.TryGetValue(_focusedSurvivorId, out level);
            return new SaveState { focusedSurvivorId = _focusedSurvivorId, focusedLevel = level };
        }

        public void RestoreState(SaveState s)
        {
            if (!string.IsNullOrEmpty(s.focusedSurvivorId))
                UpdateLevel(s.focusedSurvivorId, s.focusedLevel);
        }

        private void OnEnable()
        {
            if (_document == null) _document = GetComponent<UIDocument>();
            if (_document == null) return;
            _root = _document.rootVisualElement.Q("hypervigilance-indicator-root");
            _eye = _root?.Q<Label>("hypervigilance-eye");
            _flash = _document.rootVisualElement.Q("hypervigilance-flash-root");
            Refresh();
        }

        public void SetFocusedSurvivor(string survivorId)
        {
            _focusedSurvivorId = survivorId;
            Refresh();
        }

        public void UpdateLevel(string survivorId, float level)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            _levels[survivorId] = Mathf.Clamp01(level);
            if (survivorId == _focusedSurvivorId || string.IsNullOrEmpty(_focusedSurvivorId))
            {
                _focusedSurvivorId = survivorId;
                Refresh();
            }
            OnLevelChanged?.Invoke(survivorId, level);
        }

        public void TriggerFalseAlarm(string survivorId)
        {
            SetFocusedSurvivor(survivorId);
            if (_flashRoutine != null) StopCoroutine(_flashRoutine);
            _flashRoutine = StartCoroutine(FlashRoutine());
        }

        private IEnumerator FlashRoutine()
        {
            if (_flash == null) yield break;
            _flash.RemoveFromClassList("hidden");
            _flash.AddToClassList("hypervigilance-flash--active");
            if (_eye != null) _eye.AddToClassList("hypervigilance-eye--shake");
            yield return new WaitForSeconds(FalseAlarmFlashSeconds);
            _flash.RemoveFromClassList("hypervigilance-flash--active");
            _flash.AddToClassList("hidden");
            if (_eye != null) _eye.RemoveFromClassList("hypervigilance-eye--shake");
            _flashRoutine = null;
        }

        public void Show() => _root?.RemoveFromClassList("hidden");
        public void Hide() => _root?.AddToClassList("hidden");

        private void Refresh()
        {
            if (_root == null || _eye == null) return;
            float level = 0f;
            if (!string.IsNullOrEmpty(_focusedSurvivorId))
                _levels.TryGetValue(_focusedSurvivorId, out level);

            if (level < HiddenThreshold)
            {
                Hide();
                return;
            }

            Show();
            _eye.text = "◎";
            _eye.EnableInClassList("hypervigilance-eye--alert", level >= AlertThreshold);
        }
    }
}
