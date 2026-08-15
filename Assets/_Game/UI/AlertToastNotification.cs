using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    public enum ToastSeverity
    {
        Info,
        Warning,
        Critical
    }

    [Serializable]
    public class AlertToastItem
    {
        public string Id = Guid.NewGuid().ToString();
        public ToastSeverity Severity = ToastSeverity.Info;
        public string Message = "SYSTEM ALERT";
        public string SubText = "Details specified here";
        public float RemainingDuration = 4.0f;
    }

    public class AlertToastNotification : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;
        [SerializeField] private List<AlertToastItem> _activeToasts = new List<AlertToastItem>();
        [SerializeField] private int _maxToasts = 3;

        private VisualElement _root;
        private VisualElement _stackContainer;

        public event Action<AlertToastItem> OnToastPosted;
        public event Action OnStateChanged;

        public IReadOnlyList<AlertToastItem> ActiveToasts => _activeToasts;

        private void OnEnable()
        {
            if (_document == null)
                _document = GetComponent<UIDocument>();

            if (_document != null && _document.rootVisualElement != null)
            {
                _root = _document.rootVisualElement.Q("alert-toast-root") 
                      ?? _document.rootVisualElement.Q("alert_toast_root");
                Bind();
                RefreshUI();
            }
        }

        private void Bind()
        {
            if (_root == null) return;
            _stackContainer = _root.Q<VisualElement>("toast_stack_container");
        }

        private void Update()
        {
            if (_activeToasts.Count == 0) return;

            bool changed = false;
            for (int i = _activeToasts.Count - 1; i >= 0; i--)
            {
                _activeToasts[i].RemainingDuration -= Time.deltaTime;
                if (_activeToasts[i].RemainingDuration <= 0)
                {
                    _activeToasts.RemoveAt(i);
                    changed = true;
                }
            }

            if (changed)
            {
                RefreshUI();
                OnStateChanged?.Invoke();
            }
        }

        public void PostToast(string message, string subText, ToastSeverity severity, float duration = 4.0f)
        {
            AlertToastItem toast = new AlertToastItem
            {
                Message = message,
                SubText = subText,
                Severity = severity,
                RemainingDuration = duration
            };

            if (_activeToasts.Count >= _maxToasts)
            {
                _activeToasts.RemoveAt(0); // Evict oldest
            }

            _activeToasts.Add(toast);
            Show();
            RefreshUI();

            OnToastPosted?.Invoke(toast);
            OnStateChanged?.Invoke();
        }

        private void RefreshUI()
        {
            if (_stackContainer == null) return;

            _stackContainer.Clear();

            foreach (var toast in _activeToasts)
            {
                VisualElement item = new VisualElement();
                item.AddToClassList("toast-item");
                switch (toast.Severity)
                {
                    case ToastSeverity.Info: item.AddToClassList("severity-info"); break;
                    case ToastSeverity.Warning: item.AddToClassList("severity-warning"); break;
                    case ToastSeverity.Critical: item.AddToClassList("severity-critical"); break;
                }

                VisualElement dot = new VisualElement();
                dot.AddToClassList("toast-dot");
                switch (toast.Severity)
                {
                    case ToastSeverity.Info: dot.AddToClassList("dot-info"); break;
                    case ToastSeverity.Warning: dot.AddToClassList("dot-warning"); break;
                    case ToastSeverity.Critical: dot.AddToClassList("dot-critical"); break;
                }

                VisualElement content = new VisualElement();
                content.AddToClassList("toast-content");

                Label msgLabel = new Label(toast.Message);
                msgLabel.AddToClassList("toast-msg");

                Label subLabel = new Label(toast.SubText);
                subLabel.AddToClassList("toast-sub");

                content.Add(msgLabel);
                content.Add(subLabel);

                item.Add(dot);
                item.Add(content);

                _stackContainer.Add(item);
            }
        }

        public void Show() => _root?.RemoveFromClassList("hidden");
        public void Hide() => _root?.AddToClassList("hidden");
    }
}
