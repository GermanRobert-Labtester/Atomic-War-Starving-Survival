using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// UI Element #10 — Power Flow Schematic.
    /// Top-centre modal: 8-node power grid, supply vs demand bar, BLACKOUT alert.
    /// Raises OnBlackoutStateChanged on state change.
    /// </summary>
    public class PowerFlowSchematic : MonoBehaviour
    {
        public event Action<bool> OnBlackoutStateChanged;

        [SerializeField] private UIDocument _document;

        private VisualElement _root;
        private Label _budgetLabel;
        private Label _blackoutLabel;
        private VisualElement _supplyFill;
        private VisualElement _demandFill;
        private readonly List<(VisualElement cell, Label nameLabel, Label loadLabel)> _nodes
            = new List<(VisualElement, Label, Label)>(8);

        private bool _blackout;
        private float _totalSupplyKW;
        private float _totalDemandKW;

        [Serializable]
        public struct NodeData
        {
            public string name;
            public float loadKW;
            public bool active;
        }

        [Serializable]
        public struct SaveState
        {
            public float totalSupplyKW;
            public float totalDemandKW;
            public NodeData[] nodes;
        }

        private NodeData[] _nodeData = new NodeData[8];
        public SaveState CaptureState() => new SaveState
        {
            totalSupplyKW = _totalSupplyKW, totalDemandKW = _totalDemandKW, nodes = (NodeData[])_nodeData.Clone()
        };
        public void RestoreState(SaveState s)
        {
            SetPowerData(s.totalSupplyKW, s.totalDemandKW, s.nodes);
        }

        private void OnEnable()
        {
            if (_document == null) _document = GetComponent<UIDocument>();
            if (_document == null) return;
            _root = _document.rootVisualElement.Q("power-flow-root");
            if (_root == null) return;
            _budgetLabel   = _root.Q<Label>("power-budget-label");
            _blackoutLabel = _root.Q<Label>("power-blackout-label");
            _supplyFill    = _root.Q("power-supply-fill");
            _demandFill    = _root.Q("power-demand-fill");

            _nodes.Clear();
            for (int i = 0; i < 8; i++)
            {
                var cell = _root.Q($"power-node-{i:D2}");
                var nameLbl = cell?.Q<Label>($"power-node-{i:D2}-name");
                var loadLbl = cell?.Q<Label>($"power-node-{i:D2}-load");
                _nodes.Add((cell, nameLbl, loadLbl));
            }
            Hide();
        }

        public void SetPowerData(float supplyKW, float demandKW, NodeData[] nodes)
        {
            _totalSupplyKW = supplyKW;
            _totalDemandKW = demandKW;
            _blackout      = demandKW > supplyKW;
            if (nodes != null)
                for (int i = 0; i < Mathf.Min(nodes.Length, 8); i++) _nodeData[i] = nodes[i];
            Refresh(nodes);
            OnBlackoutStateChanged?.Invoke(_blackout);
        }

        public void Show() => _root?.RemoveFromClassList("hidden");
        public void Hide() => _root?.AddToClassList("hidden");

        private void Refresh(NodeData[] nodes)
        {
            if (_root == null) return;
            float maxKW = Mathf.Max(_totalSupplyKW, _totalDemandKW, 1f);

            if (_budgetLabel != null)
                _budgetLabel.text = $"SUPPLY: {_totalSupplyKW:F1} kW   DEMAND: {_totalDemandKW:F1} kW";
            if (_supplyFill != null)
                _supplyFill.style.width = Length.Percent(Mathf.Clamp01(_totalSupplyKW / maxKW) * 100f);
            if (_demandFill != null)
            {
                _demandFill.style.width = Length.Percent(Mathf.Clamp01(_totalDemandKW / maxKW) * 100f);
                _demandFill.EnableInClassList("power-demand-fill--overload", _blackout);
            }
            if (_blackoutLabel != null)
            {
                _blackoutLabel.style.display = _blackout ? DisplayStyle.Flex : DisplayStyle.None;
                _blackoutLabel.text = "⚡ BLACKOUT — DEMAND EXCEEDS SUPPLY";
            }

            for (int i = 0; i < 8 && i < _nodes.Count; i++)
            {
                var (cell, nameLbl, loadLbl) = _nodes[i];
                if (cell == null) continue;
                bool hasData = nodes != null && i < nodes.Length;
                cell.style.display = hasData ? DisplayStyle.Flex : DisplayStyle.None;
                if (!hasData) continue;
                if (nameLbl != null) nameLbl.text = nodes[i].name?.ToUpper() ?? $"NODE {i}";
                if (loadLbl != null) loadLbl.text = $"{nodes[i].loadKW:F1} kW";
                cell.EnableInClassList("power-node--offline",   !nodes[i].active);
                cell.EnableInClassList("power-node--overloaded", nodes[i].active && _blackout);
            }
            _root.EnableInClassList("diegetic-panel--critical", _blackout);
        }
    }
}
