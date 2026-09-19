// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : HiddenAgendaHostSession
// Core System  : Ashfall.Core.Survivors.HiddenAgendaSystem
// Host Caller  : Main.HiddenAgenda
// Purpose      : Plan 132 — Coordinates survivor hidden agendas, investigations,
//                clue discovery, and confrontation resolutions.
// ============================================================================
using System;
using System.Collections.Generic;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public sealed class HiddenAgendaHostSession
    {
        private readonly HiddenAgendaSystem _system;

        public HiddenAgendaSystem System => _system;

        public event Action? StateChanged;

        public int ActiveAgendaCount => _system.ActiveAgendaCount;
        public int TotalCluesCount => _system.TotalCluesCount;

        public HiddenAgendaHostSession(HiddenAgendaSystem system)
        {
            _system = system ?? throw new ArgumentNullException(nameof(system));

            _system.OnAgendaAssigned += _ => StateChanged?.Invoke();
            _system.OnClueDiscovered += _ => StateChanged?.Invoke();
            _system.OnAgendaExposed += _ => StateChanged?.Invoke();
            _system.OnAgendaResolved += (_, _) => StateChanged?.Invoke();
        }

        public SurvivorHiddenAgenda AssignAgenda(
            string survivorId,
            AgendaType type,
            string targetFaction = "",
            string targetSurvivor = "",
            int startDay = 1,
            string notes = "")
        {
            var agenda = _system.AssignAgenda(survivorId, type, targetFaction, targetSurvivor, startDay, notes);
            StateChanged?.Invoke();
            return agenda;
        }

        public AgendaClue? Investigate(string survivorId, float investigatorSkill = 20f, int currentDay = 1)
        {
            var clue = _system.InvestigateAgenda(survivorId, investigatorSkill, currentDay);
            if (clue != null)
            {
                StateChanged?.Invoke();
            }
            return clue;
        }

        public bool Confront(string agendaId, AgendaResolution resolution, int currentDay = 1)
        {
            bool success = _system.ConfrontSurvivor(agendaId, resolution, currentDay);
            if (success)
            {
                StateChanged?.Invoke();
            }
            return success;
        }

        public void TickDay(int currentDay)
        {
            _system.TickDay(currentDay);
            StateChanged?.Invoke();
        }

        public SurvivorHiddenAgenda? GetAgendaForSurvivor(string survivorId)
        {
            return _system.GetAgendaForSurvivor(survivorId);
        }

        public IReadOnlyList<SurvivorHiddenAgenda> GetActiveAgendas()
        {
            return _system.GetActiveAgendas();
        }

        public IReadOnlyList<SurvivorHiddenAgenda> GetAllAgendas()
        {
            return _system.GetAllAgendas();
        }

        public IReadOnlyList<AgendaClue> GetCluesForAgenda(string agendaId)
        {
            return _system.GetCluesForAgenda(agendaId);
        }

        public IReadOnlyList<AgendaClue> GetAllClues()
        {
            return _system.GetAllClues();
        }
    }
}
