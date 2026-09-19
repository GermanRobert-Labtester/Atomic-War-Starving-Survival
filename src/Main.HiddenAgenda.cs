// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core.Survivors;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private HiddenAgendaHostSession _hiddenAgenda = null!;
        private HiddenAgendaPanel _hiddenAgendaPanel = null!;
        private bool _hiddenAgendaDirty;

        public HiddenAgendaHostSession HiddenAgenda => EnsureHiddenAgenda();

        public HiddenAgendaHostSession EnsureHiddenAgenda()
        {
            if (_hiddenAgenda != null) return _hiddenAgenda;

            SetupSurvivors();

            var state = HiddenAgendaSaveStore.TryLoad() ?? new HiddenAgendaState();
            var system = new HiddenAgendaSystem(state);

            // Seed initial narrative intrigue if brand new game and survivors exist
            if (state.Agendas.Count == 0 && _survivors?.RosterState != null && _survivors.RosterState.Count > 0)
            {
                var first = _survivors.RosterState[0];
                if (first != null && !string.IsNullOrEmpty(first.Id))
                {
                    system.AssignAgenda(
                        survivorId: first.Id,
                        type: AgendaType.SecretProtection,
                        targetFaction: "faction_free_settlers",
                        startDay: 1,
                        notes: "Conceals family pre-war records from authorities"
                    );
                }
            }

            _hiddenAgenda = new HiddenAgendaHostSession(system);
            _hiddenAgenda.StateChanged += OnHiddenAgendaStateChanged;
            return _hiddenAgenda;
        }

        private void OnHiddenAgendaStateChanged()
        {
            _hiddenAgendaDirty = true;
        }

        public void TickHiddenAgenda(int day)
        {
            EnsureHiddenAgenda();
            _hiddenAgenda.TickDay(day);
            _hiddenAgendaDirty = true;
        }

        private void SetupHiddenAgenda()
        {
            EnsureHiddenAgenda();
        }

        private void SaveHiddenAgenda()
        {
            var session = EnsureHiddenAgenda();
            if (session != null)
            {
                CaptureSection("hidden_agenda", HiddenAgendaSaveStore.TryCapturePersisted(session.System.CaptureState()));
                _hiddenAgendaDirty = false;
            }
        }

        private void SetupHiddenAgendaPanel()
        {
            if (_hiddenAgendaPanel != null && _hiddenAgendaPanel.IsInsideTree())
                return;

            EnsureHiddenAgenda();
            _hiddenAgendaPanel = new HiddenAgendaPanel();
            _hiddenAgendaPanel.Bind(_hiddenAgenda);
            _hiddenAgendaPanel.OnClose += () => _hiddenAgendaPanel.Visible = false;
            _hiddenAgendaPanel.Visible = false;
            AddChild(_hiddenAgendaPanel);
        }

        public void ShowHiddenAgendaPanel()
        {
            SetupHiddenAgendaPanel();
            _hiddenAgendaPanel.Visible = true;
            _hiddenAgendaPanel.RefreshView();
        }
    }
}
