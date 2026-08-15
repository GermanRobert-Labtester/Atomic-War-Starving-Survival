using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    public class VehicleStatusPanel : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;
        private VisualElement _root, _modSlots;
        private Label _vehicleName, _fuelValue, _cargoValue, _breakdownRisk;
        private ProgressBar _conditionBar, _fuelBar, _cargoBar;
        private bool _bound;

        public void BindDocument(UIDocument doc) { _document = doc; _bound = false; }
        private void EnsureBound()
        {
            if (_bound || _document == null) return;
            _root = _document.rootVisualElement?.Q<VisualElement>("vehicle-status-panel");
            if (_root == null) return;
            _vehicleName = _root.Q<Label>("vehicle-name");
            _conditionBar = _root.Q<ProgressBar>("condition-bar");
            _fuelBar = _root.Q<ProgressBar>("fuel-bar");
            _fuelValue = _root.Q<Label>("fuel-value");
            _cargoBar = _root.Q<ProgressBar>("cargo-bar");
            _cargoValue = _root.Q<Label>("cargo-value");
            _modSlots = _root.Q<VisualElement>("mod-slots");
            _breakdownRisk = _root.Q<Label>("breakdown-risk");
            _bound = true;
        }

        public void ShowVehicle(string name, float condition, float fuel, float maxFuel,
            float cargo, float maxCargo)
        {
            EnsureBound();
            if (_root == null) return;
            _root.style.display = DisplayStyle.Flex;
            if (_vehicleName != null) _vehicleName.text = name;
            UpdateCondition(condition);
            UpdateFuel(fuel, maxFuel);
            UpdateCargo(cargo, maxCargo);
        }

        public void UpdateCondition(float v) { if (_conditionBar != null) _conditionBar.value = v * 100f; }
        public void UpdateFuel(float fuel, float max)
        {
            if (_fuelBar != null) { _fuelBar.value = fuel; _fuelBar.highValue = max; }
            if (_fuelValue != null) _fuelValue.text = $"{fuel:F0} / {max:F0} L";
        }
        public void UpdateCargo(float cargo, float max)
        {
            if (_cargoBar != null) { _cargoBar.value = cargo; _cargoBar.highValue = max; }
            if (_cargoValue != null) _cargoValue.text = $"{cargo:F0} / {max:F0} kg";
        }
        public void SetModifications(string[] modIds)
        {
            if (_modSlots == null) return;
            _modSlots.Clear();
            foreach (var id in modIds)
                _modSlots.Add(new Label(id.Replace("_", " ")) { style = { fontSize = 10, color = new StyleColor(new Color(0.3f, 0.69f, 0.31f)), marginRight = 6 } });
        }
        public void SetBreakdownRisk(float risk)
        {
            if (_breakdownRisk != null)
            {
                _breakdownRisk.text = risk > 0.5f ? $"⚠ BREAKDOWN RISK: {risk:P0}" : "";
                _breakdownRisk.style.color = risk > 0.7f ? new StyleColor(Color.red) : new StyleColor(new Color(1f, 0.76f, 0.03f));
            }
        }
        public void Hide() { if (_root != null) _root.style.display = DisplayStyle.None; }
    }
}
