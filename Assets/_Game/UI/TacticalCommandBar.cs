using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    public class TacticalCommandBar : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;
        private VisualElement _root;
        private Button[] _buttons = new Button[5];
        private VisualElement[] _cooldowns = new VisualElement[5];
        private string[] _commandIds = { "hold_line", "retreat", "suppressive", "deploy_trap", "decon_flush" };
        private bool _bound;

        public void BindDocument(UIDocument doc) { _document = doc; _bound = false; }
        private void EnsureBound()
        {
            if (_bound || _document == null) return;
            _root = _document.rootVisualElement?.Q<VisualElement>("tactical-command-bar");
            if (_root == null) return;
            for (int i = 0; i < 5; i++)
            {
                _buttons[i] = _root.Q<Button>($"cmd-{_commandIds[i].Replace("_","-")}");
                _cooldowns[i] = _buttons[i]?.Q<VisualElement>("cooldown-overlay");
            }
            _bound = true;
        }

        public void ShowCommands(bool[] available, float[] cooldowns, System.Action<int> callback)
        {
            EnsureBound();
            // #region agent log
            AtomicWar._Game.Utilities.AgentDebugLog.Write("H3", "TacticalCommandBar.ShowCommands", "show",
                "{\"rootNull\":" + (_root == null ? "true" : "false")
                + ",\"btn0Null\":" + (_buttons[0] == null ? "true" : "false") + "}");
            // #endregion
            if (_root == null) return;
            _root.style.display = DisplayStyle.Flex;
            for (int i = 0; i < 5 && i < _buttons.Length; i++)
            {
                if (_buttons[i] == null) continue;
                _buttons[i].SetEnabled(available != null && i < available.Length && available[i]);
                if (_cooldowns[i] != null)
                    _cooldowns[i].style.opacity = (cooldowns != null && i < cooldowns.Length && cooldowns[i] > 0) ? 0.6f : 0f;
                int idx = i;
                _buttons[i].clicked -= OnClick;
                _buttons[i].clicked += () => callback?.Invoke(idx);
            }
        }

        private void OnClick() { } // placeholder
        public void HideCommands() { if (_root != null) _root.style.display = DisplayStyle.None; }
    }
}
