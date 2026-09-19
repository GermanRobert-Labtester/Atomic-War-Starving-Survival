// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : PropagandaHostSession
// Core System  : Ashfall.Core.Propaganda.PropagandaSystem
// Host Caller  : Main.Propaganda
// Purpose      : Plan 168 — Coordinates propaganda messages, multi-day campaigns,
//                detection risks, and morale warfare effects.
// ============================================================================
using System;
using System.Collections.Generic;
using Ashfall.Core.Propaganda;

namespace AtomicWar.GodotApp
{
    public sealed class PropagandaHostSession
    {
        private readonly PropagandaSystem _system;

        public PropagandaSystem System => _system;

        public event Action? StateChanged;

        public float ShelterCredibility => _system.ShelterCredibility;
        public int MessageCount => _system.MessageCount;
        public int ActiveCampaignCount => _system.ActiveCampaignCount;
        public IReadOnlyList<PropagandaMessage> Messages => _system.Messages;
        public IReadOnlyList<PropagandaCampaign> Campaigns => _system.Campaigns;

        public PropagandaHostSession(PropagandaSystem system)
        {
            _system = system ?? throw new ArgumentNullException(nameof(system));

            _system.OnMessageCreated += _ => StateChanged?.Invoke();
            _system.OnCampaignStarted += _ => StateChanged?.Invoke();
            _system.OnCampaignProgressed += (_, _) => StateChanged?.Invoke();
            _system.OnCampaignCompleted += _ => StateChanged?.Invoke();
            _system.OnCampaignCompromised += _ => StateChanged?.Invoke();
            _system.OnStateChanged += () => StateChanged?.Invoke();
        }

        public PropagandaMessage CreateMessage(
            string authorId,
            PropagandaMedium medium,
            MessageTruthfulness truthfulness,
            PropagandaTheme theme,
            string targetFactionId,
            string content,
            float authorSkill = 50f,
            string targetAudience = "Civilians",
            int currentDay = 1)
        {
            var msg = _system.CreateMessage(authorId, medium, truthfulness, theme, targetFactionId, content, authorSkill, targetAudience, currentDay);
            StateChanged?.Invoke();
            return msg;
        }

        public PropagandaCampaign LaunchCampaign(
            string campaignName,
            string targetFactionId,
            PropagandaObjective objective,
            IEnumerable<string> messageIds,
            int durationDays = 5,
            int currentDay = 1)
        {
            var cmp = _system.LaunchCampaign(campaignName, targetFactionId, objective, messageIds, durationDays, currentDay);
            StateChanged?.Invoke();
            return cmp;
        }

        public void TickDay(int currentDay, float randomRoll = 0.5f)
        {
            _system.TickDay(currentDay, randomRoll);
            StateChanged?.Invoke();
        }

        public float GetFactionMoraleImpact(string factionId)
        {
            return _system.GetFactionMoraleImpact(factionId);
        }

        public PropagandaState CaptureState()
        {
            return _system.CaptureState();
        }

        public void RestoreState(PropagandaState state)
        {
            _system.RestoreState(state);
            StateChanged?.Invoke();
        }
    }
}
