// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : TimeCapsuleHostSession
// Core System  : Ashfall.Core.Communication.TimeCapsuleSystem
// Host Caller  : Main.TimeCapsule
// Purpose      : Plan 212 — Time capsules, legacy messages, delayed discovery, and cross-generational communication
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Communication;

namespace AtomicWar.GodotApp
{
    public sealed class TimeCapsuleHostSession
    {
        private readonly TimeCapsuleSystem _system;

        /// <summary>Plan 212: live campaign-day source (W2 provider pattern).
        /// Null-safe: unbound probes pass the day explicitly.</summary>
        public Func<int>? DayProvider { get; set; }

        private int EffectiveDay(int explicitDay) => DayProvider?.Invoke() ?? explicitDay;

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

        /// <summary>
        /// Plan 212: the truthful unseal command. Validates the capsule's own
        /// condition at the live campaign day and returns an explicit blocked
        /// reason instead of silently opening — the panel route uses this;
        /// never a literal day-1 bypass.
        /// </summary>
        public (bool opened, string reason) TryOpenCapsule(string capsuleId, string openedBy, int currentDay)
        {
            bool ok = _system.TryOpenCapsule(capsuleId, openedBy, currentDay, out string blocked, out _);
            if (ok) StateChanged?.Invoke();
            return (ok, ok ? "opened" : blocked);
        }

        /// <summary>
        /// Plan 212 privacy gate: a pending legacy message may not disclose its
        /// content; the recipient sees sender/recipient/condition only. A read
        /// model — no mutation — so a refresh can never fabricate delivery.
        /// </summary>
        public IReadOnlyList<LegacyMessage> VisibleMessages() =>
            _system.Messages.Select(m =>
                m.IsDelivered ? m : new LegacyMessage
                {
                    MessageId = m.MessageId,
                    AuthorId = m.AuthorId,
                    RecipientId = m.RecipientId,
                    Content = "[sealed until delivery]",
                    CreatedDay = m.CreatedDay,
                    Condition = m.Condition,
                    DeliveryDay = m.DeliveryDay,
                    IsDelivered = false,
                    IsRead = false,
                }).ToList();

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
