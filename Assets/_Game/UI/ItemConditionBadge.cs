using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    public class ItemConditionBadge : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;
        private VisualElement _badge, _conditionFill, _contaminationIcon;
        private Label _conditionText, _expirationLabel;
        private bool _bound;

        public void BindDocument(UIDocument doc) { _document = doc; _bound = false; }
        private void EnsureBound()
        {
            if (_bound || _document == null) return;
            _badge = _document.rootVisualElement?.Q<VisualElement>("item-condition-badge");
            if (_badge == null) return;
            _conditionFill = _badge.Q<VisualElement>("condition-bar-fill");
            _conditionText = _badge.Q<Label>("condition-text");
            _contaminationIcon = _badge.Q<VisualElement>("contamination-icon");
            _expirationLabel = _badge.Q<Label>("expiration-label");
            _bound = true;
        }

        public void SetCondition(float conditionPct)
        {
            EnsureBound();
            if (_badge == null) return;
            if (_conditionFill != null) _conditionFill.style.width = new Length(conditionPct * 100f, LengthUnit.Percent);
            if (_conditionText != null)
            {
                if (conditionPct > 0.7f) _conditionText.text = "";
                else if (conditionPct > 0.3f) _conditionText.text = "WORN";
                else _conditionText.text = "DAMAGED";
            }
        }

        public void SetContamination(float contaminationPct)
        {
            EnsureBound();
            if (_contaminationIcon != null)
                _contaminationIcon.style.opacity = contaminationPct > 0 ? contaminationPct : 0;
        }

        public void SetExpirationState(ExpirationState state) // from ProceduralItemInstance
        {
            EnsureBound();
            if (_expirationLabel == null) return;
            _expirationLabel.text = state switch
            {
                ExpirationState.Expired => "EXPIRED",
                ExpirationState.Degraded => "DEGRADED",
                _ => ""
            };
            _expirationLabel.style.color = state == ExpirationState.Expired
                ? new StyleColor(Color.red) : new StyleColor(new Color(1f, 0.6f, 0f));
        }

        public void Hide()
        {
            EnsureBound();
            if (_badge != null) _badge.style.display = DisplayStyle.None;
        }
    }
}
