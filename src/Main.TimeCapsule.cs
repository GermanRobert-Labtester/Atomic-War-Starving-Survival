// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core.Communication;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private TimeCapsuleHostSession _timeCapsule = null!;
        private TimeCapsulePanel _timeCapsulePanel = null!;
        private bool _timeCapsuleDirty;

        public TimeCapsuleHostSession TimeCapsules => EnsureTimeCapsule();

        public TimeCapsuleHostSession EnsureTimeCapsule()
        {
            if (_timeCapsule != null) return _timeCapsule;

            var state = TimeCapsuleSaveStore.TryLoad() ?? new TimeCapsuleState();
            var system = new TimeCapsuleSystem(state);

            _timeCapsule = new TimeCapsuleHostSession(system);
            _timeCapsule.StateChanged += OnTimeCapsuleStateChanged;
            return _timeCapsule;
        }

        private void OnTimeCapsuleStateChanged()
        {
            _timeCapsuleDirty = true;
        }

        public void TickTimeCapsule(int day)
        {
            EnsureTimeCapsule();
            _timeCapsule.TickDay(day);
            _timeCapsuleDirty = true;
        }

        private void SetupTimeCapsules()
        {
            EnsureTimeCapsule();
        }

        private void SaveTimeCapsules()
        {
            var session = EnsureTimeCapsule();
            if (session != null)
            {
                CaptureSection("time_capsules", TimeCapsuleSaveStore.TryCapturePersisted(session.System.CaptureState()));
                _timeCapsuleDirty = false;
            }
        }

        private void FlushTimeCapsulesIfDirty()
        {
            if (_timeCapsuleDirty) SaveTimeCapsules();
        }

        private void SetupTimeCapsulePanel()
        {
            if (_timeCapsulePanel != null && _timeCapsulePanel.IsInsideTree())
                return;

            EnsureTimeCapsule();
            _timeCapsulePanel = new TimeCapsulePanel();
            _timeCapsulePanel.Bind(_timeCapsule);
            _timeCapsulePanel.OnClose += () => _timeCapsulePanel.Visible = false;
            _timeCapsulePanel.Visible = false;
            AddChild(_timeCapsulePanel);
        }

        public void ShowTimeCapsulePanel()
        {
            SetupTimeCapsulePanel();
            _timeCapsulePanel.Visible = true;
            _timeCapsulePanel.RefreshView();
        }
    }
}
