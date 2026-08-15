using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    /// <summary>Phase 11 — full-screen phantom memory vignette with narrative text.</summary>
    public class PhantomMemoryVignette : MonoBehaviour
    {
        public const float FadeInSeconds = 0.3f;
        public const float HoldSeconds = 2.5f;
        public const float FadeOutSeconds = 0.5f;

        public event Action<bool> OnVignetteTriggered;

        [SerializeField] private UIDocument _document;

        private VisualElement _root;
        private Label _textLabel;
        private Coroutine _animRoutine;

        [Serializable]
        public struct SaveState
        {
            public bool isVisible;
            public bool isMotivation;
            public string lastText;
        }

        private SaveState _lastState;

        public SaveState CaptureState() => _lastState;
        public void RestoreState(SaveState s) => _lastState = s;

        /// <summary>Bind to the shared DiegeticHud UIDocument.</summary>
        public void BindDocument(UIDocument document)
        {
            _document = document;
            BindElements();
            Hide();
        }

        private void OnEnable()
        {
            if (_document == null) _document = GetComponent<UIDocument>();
            BindElements();
            Hide();
        }

        private void BindElements()
        {
            if (_document == null || _document.rootVisualElement == null) return;
            _root = _document.rootVisualElement.Q("phantom-memory-vignette-root");
            _textLabel = _root?.Q<Label>("phantom-memory-text");
        }

        public void Trigger(string displayName, string narrativeText, bool isMotivation)
        {
            string prefix = isMotivation ? "Resolve: " : "Memory: ";
            string body = string.IsNullOrEmpty(narrativeText)
                ? $"{displayName} is shaken by a buried memory."
                : narrativeText;
            _lastState = new SaveState { isVisible = true, isMotivation = isMotivation, lastText = body };
            OnVignetteTriggered?.Invoke(isMotivation);

            if (_root == null) return;
            if (_animRoutine != null) StopCoroutine(_animRoutine);
            if (_textLabel != null) _textLabel.text = prefix + body;

            _root.EnableInClassList("phantom-memory-vignette--motivation", isMotivation);
            _root.EnableInClassList("phantom-memory-vignette--breakdown", !isMotivation);
            _animRoutine = StartCoroutine(AnimateVignette());
        }

        private IEnumerator AnimateVignette()
        {
            _root.RemoveFromClassList("hidden");
            _root.style.opacity = 0f;
            float t = 0f;
            while (t < FadeInSeconds)
            {
                t += Time.deltaTime;
                _root.style.opacity = Mathf.Clamp01(t / FadeInSeconds);
                yield return null;
            }
            _root.style.opacity = 1f;
            yield return new WaitForSeconds(HoldSeconds);
            t = 0f;
            while (t < FadeOutSeconds)
            {
                t += Time.deltaTime;
                _root.style.opacity = 1f - Mathf.Clamp01(t / FadeOutSeconds);
                yield return null;
            }
            Hide();
            _animRoutine = null;
        }

        public void Hide()
        {
            if (_root == null) return;
            _root.AddToClassList("hidden");
            _root.style.opacity = 0f;
            _lastState.isVisible = false;
        }
    }
}
