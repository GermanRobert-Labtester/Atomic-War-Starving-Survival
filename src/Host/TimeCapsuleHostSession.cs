// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : TimeCapsuleHostSession
// Core System  : Ashfall.Core.Communication.TimeCapsuleSystem
// Host Caller  : Main.TimeCapsule
// Purpose      : Plan 212 — Time capsules, legacy messages, delayed discovery, and cross-generational communication
// ============================================================================
using System;
using System.Collections.Generic;
using Ashfall.Core.Communication;

namespace AtomicWar.GodotApp
{
    public sealed class TimeCapsuleHostSession
    {
        private readonly TimeCapsuleSystem _system;

        public TimeCapsuleSystem System => _system;

        public event Action? StateChanged;

        public int TotalCapsuleCount => _system.TotalCapsuleCount;
        public int UnopenedCapsuleCount => _system.UnopenedCapsuleCount;
        public int PendingMessageCount => _system.PendingMessageCount;
        public IReadOnlyList<TimeCapsule> Capsules => _system.Capsules;
        public IReadOnlyList<LegacyMessage> Messages => _system.Messages;

        public TimeCapsuleHostSession(TimeCapsuleSystem system)
        {
            _system = system ?? throw new ArgumentNullException(nameof(system));

            _system.OnCapsuleCreated += _ => StateChanged?.Invoke();
            _system.OnCapsuleOpened += (_, _) => StateChanged?.Invoke();
            _system.OnMessageCreated += _ => StateChanged?.Invoke();
            _system.OnMessageDelivered += _ => StateChanged?.Invoke();
        }

        public TimeCapsule CreateCapsule(
            string capsuleName,
            string creatorId,
            OpenConditionType conditionType,
            int createdDay = 1,
            int targetOpenDay = -1,
            string targetSurvivorId = "",
            string location = "",
            string message = "",
            IEnumerable<CapsuleContent>? contents = null)
        {
            var cap = _system.CreateCapsule(capsuleName, creatorId, conditionType, createdDay, targetOpenDay, targetSurvivorId, location, message, contents);
            StateChanged?.Invoke();
            return cap;
        }

        public bool OpenCapsule(string capsuleId, string openedBy, int currentDay)
        {
            bool result = _system.OpenCapsule(capsuleId, openedBy, currentDay);
            if (result)
                StateChanged?.Invoke();
            return result;
        }

        public LegacyMessage WriteMessage(
            string authorId,
            string recipientId,
            string content,
            DeliveryCondition condition = DeliveryCondition.Immediate,
            int deliveryDay = -1,
            int currentDay = 1)
        {
            var msg = _system.WriteMessage(authorId, recipientId, content, condition, deliveryDay, currentDay);
            StateChanged?.Invoke();
            return msg;
        }

        public void TickDay(int currentDay)
        {
            _system.TickDay(currentDay);
            StateChanged?.Invoke();
        }

        public void DeliverDeathMessages(string deceasedSurvivorId)
        {
            _system.DeliverDeathMessages(deceasedSurvivorId);
            StateChanged?.Invoke();
        }

        public TimeCapsuleState CaptureState()
        {
            return _system.CaptureState();
        }

        public void RestoreState(TimeCapsuleState state)
        {
            _system.RestoreState(state);
            StateChanged?.Invoke();
        }
    }
}
