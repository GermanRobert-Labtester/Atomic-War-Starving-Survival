using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    [Serializable]
    public class BloodTypeData
    {
        public string BloodType = "O-"; // A+, A-, B+, B-, O+, O-, AB+, AB-
        public bool IsUniversalDonor = true;
        public int CompatibilityScore = 4; // 1..4 dots
    }

    public class BloodTypeIndicator : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;
        [SerializeField] private BloodTypeData _data = new BloodTypeData();

        private VisualElement _root;
        private Label _typeCodeLabel;
        private Label _donorTagLabel;
        private VisualElement[] _dots = new VisualElement[4];

        public event Action<BloodTypeData> OnStateChanged;

        public BloodTypeData CurrentData => _data;

        private void OnEnable()
        {
            if (_document == null)
                _document = GetComponent<UIDocument>();

            if (_document != null && _document.rootVisualElement != null)
            {
                _root = _document.rootVisualElement.Q("blood-type-root") 
                      ?? _document.rootVisualElement.Q("blood_type_root");
                Bind();
                RefreshUI();
            }
        }

        private void Bind()
        {
            if (_root == null) return;
            _typeCodeLabel = _root.Q<Label>("blood_type_code");
            _donorTagLabel = _root.Q<Label>("universal_donor_label");

            for (int i = 0; i < 4; i++)
            {
                _dots[i] = _root.Q<VisualElement>($"dot_comp_{i}");
            }
        }

        public void SetData(BloodTypeData data)
        {
            if (data == null) return;
            _data = data;
            RefreshUI();
            OnStateChanged?.Invoke(_data);
        }

        public void SetBloodType(string type)
        {
            _data.BloodType = type.ToUpper();
            _data.IsUniversalDonor = (_data.BloodType == "O-");
            _data.CompatibilityScore = GetDefaultScoreForType(_data.BloodType);

            RefreshUI();
            OnStateChanged?.Invoke(_data);
        }

        private int GetDefaultScoreForType(string type)
        {
            switch (type)
            {
                case "O-": return 4;
                case "O+": return 3;
                case "A-": return 3;
                case "B-": return 3;
                case "A+": return 2;
                case "B+": return 2;
                case "AB-": return 2;
                case "AB+": return 1;
                default: return 1;
            }
        }

        private void RefreshUI()
        {
            if (_root == null) return;

            if (_typeCodeLabel != null)
                _typeCodeLabel.text = _data.BloodType;

            if (_donorTagLabel != null)
            {
                if (_data.IsUniversalDonor || _data.BloodType == "O-")
                    _donorTagLabel.RemoveFromClassList("hidden");
                else
                    _donorTagLabel.AddToClassList("hidden");
            }

            for (int i = 0; i < 4; i++)
            {
                if (_dots[i] != null)
                {
                    if (i < _data.CompatibilityScore)
                        _dots[i].AddToClassList("active");
                    else
                        _dots[i].RemoveFromClassList("active");
                }
            }
        }

        public void Show() => _root?.RemoveFromClassList("hidden");
        public void Hide() => _root?.AddToClassList("hidden");
    }
}
