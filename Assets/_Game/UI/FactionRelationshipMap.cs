using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    public class FactionRelationshipMap : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;
        private VisualElement _root, _mapCanvas;
        private VisualElement _nodeGarrison, _nodeMilitia, _nodeCult, _nodeWarlords;
        private bool _bound;

        public void BindDocument(UIDocument doc) { _document = doc; _bound = false; }
        private void EnsureBound()
        {
            if (_bound || _document == null) return;
            _root = _document.rootVisualElement?.Q<VisualElement>("faction-relationship-map");
            if (_root == null) return;
            _mapCanvas = _root.Q<VisualElement>("map-canvas");
            _nodeGarrison = _root.Q<VisualElement>("node-garrison");
            _nodeMilitia = _root.Q<VisualElement>("node-militia");
            _nodeCult = _root.Q<VisualElement>("node-cult");
            _nodeWarlords = _root.Q<VisualElement>("node-warlords");
            _bound = true;
        }

        public void Show() { EnsureBound(); if (_root != null) _root.style.display = DisplayStyle.Flex; }
        public void Hide() { if (_root != null) _root.style.display = DisplayStyle.None; }

        private VisualElement GetNode(string factionId) => factionId switch
        {
            "iron_garrison" or "garrison" => _nodeGarrison,
            "ash_militia" or "militia" => _nodeMilitia,
            "cult_of_ash_sign" or "cult" => _nodeCult,
            "warlords_sector_4" or "warlords" => _nodeWarlords,
            _ => null
        };

        public void SetFactionNodeData(string factionId, string displayName,
            float standing, string relationshipToPlayer)
        {
            EnsureBound();
            var node = GetNode(factionId);
            if (node == null) return;
            var standingIndicator = node.Q<VisualElement>(className: "standing-indicator");
            if (standingIndicator != null)
            {
                standingIndicator.style.backgroundColor = standing switch
                {
                    > 50 => new StyleColor(new Color(0.3f, 0.69f, 0.31f)),
                    >= 0 => new StyleColor(new Color(1f, 0.76f, 0.03f)),
                    _ => new StyleColor(Color.red)
                };
            }
        }

        public void SetRelationship(string factionA, string factionB, string status)
        {
            // Visual relationship lines are CSS-based; simplified implementation
        }

        public void PulseNode(string factionId)
        {
            var node = GetNode(factionId);
            if (node != null)
            {
                node.AddToClassList("pulse");
                node.schedule.Execute(() => node.RemoveFromClassList("pulse")).StartingIn(1000);
            }
        }
    }
}
