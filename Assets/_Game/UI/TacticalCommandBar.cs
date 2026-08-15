using System;
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
        private Action<int> _onCommand;
        private readonly Action[] _clickHandlers = new Action[5];
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
                if (_buttons[i] == null) continue;
                int idx = i;
                if (_clickHandlers[i] != null)
                    _buttons[i].clicked -= _clickHandlers[i];
                _clickHandlers[i] = () => _onCommand?.Invoke(idx);
                _buttons[i].clicked += _clickHandlers[i];
            }
            _bound = true;
        }

        public void ShowCommands(bool[] available, float[] cooldowns, System.Action<int> callback)
        {
            EnsureBound();
            _onCommand = callback;
            if (_root == null) return;
            _root.style.display = DisplayStyle.Flex;
            for (int i = 0; i < 5 && i < _buttons.Length; i++)
            {
                if (_buttons[i] == null) continue;
                _buttons[i].SetEnabled(available != null && i < available.Length && available[i]);
                if (_cooldowns[i] != null)
                    _cooldowns[i].style.opacity = (cooldowns != null && i < cooldowns.Length && cooldowns[i] > 0) ? 0.6f : 0f;
            }
        }

        public void HideCommands() { if (_root != null) _root.style.display = DisplayStyle.None; }

        /// <summary>Issue a bound command by index (0 hold_line … 4 decon_flush).</summary>
        public void IssueCommand(int index)
        {
            EnsureBound();
            _onCommand?.Invoke(index);
        }
    }
}
