// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core.Survivors;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private SurvivorDeathLegacyHostSession _deathLegacy = null!;
        private SurvivorDeathLegacyPanel _deathLegacyPanel = null!;
        private bool _deathLegacyDirty;

        public SurvivorDeathLegacyHostSession DeathLegacy => EnsureSurvivorDeathLegacy();

        public SurvivorDeathLegacyHostSession EnsureSurvivorDeathLegacy()
        {
            if (_deathLegacy != null) return _deathLegacy;

            var state = SurvivorDeathLegacySaveStore.TryLoad() ?? new SurvivorDeathLegacyState();
            var system = new SurvivorDeathLegacySystem(state);

            _deathLegacy = new SurvivorDeathLegacyHostSession(system);
            _deathLegacy.StateChanged += OnDeathLegacyStateChanged;
            return _deathLegacy;
        }

        private void OnDeathLegacyStateChanged()
        {
            _deathLegacyDirty = true;
        }

        public void TickSurvivorDeathLegacy(int day)
        {
            EnsureSurvivorDeathLegacy();
            // Survivor death records are event-driven, but daily tick flushes if dirty
        }

        private void SetupDeathLegacy()
        {
            EnsureSurvivorDeathLegacy();
        }

        private void SaveDeathLegacy()
        {
            var session = EnsureSurvivorDeathLegacy();
            if (session != null)
            {
                CaptureSection("death_legacy", SurvivorDeathLegacySaveStore.TryCapturePersisted(session.System.CaptureState()));
                _deathLegacyDirty = false;
            }
        }

        private void FlushDeathLegacyIfDirty()
        {
            if (_deathLegacyDirty) SaveDeathLegacy();
        }

        private void SetupDeathLegacyPanel()
        {
            if (_deathLegacyPanel != null && _deathLegacyPanel.IsInsideTree())
                return;

            EnsureSurvivorDeathLegacy();
            _deathLegacyPanel = new SurvivorDeathLegacyPanel();
            _deathLegacyPanel.Bind(_deathLegacy);
            _deathLegacyPanel.OnClose += () => _deathLegacyPanel.Visible = false;
            _deathLegacyPanel.Visible = false;
            AddChild(_deathLegacyPanel);
        }

        public void ShowSurvivorDeathLegacyPanel()
        {
            SetupDeathLegacyPanel();
            _deathLegacyPanel.Visible = true;
            _deathLegacyPanel.RefreshView();
        }
    }
}
