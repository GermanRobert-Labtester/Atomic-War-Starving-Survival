// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Communication
{
    public enum OpenConditionType
    {
        DateBased = 0,
        SurvivorBased = 1,
        EventBased = 2,
        Manual = 3
    }

    public enum CapsuleContentType
    {
        Letter = 0,
        Item = 1,
        Photo = 2,
        Recording = 3,
        Drawing = 4,
        Artifact = 5
    }

    public enum DeliveryCondition
    {
        Immediate = 0,
        OnDate = 1,
        OnDeath = 2,
        OnEvent = 3
    }

    [Serializable]
    public sealed class CapsuleContent
    {
        public string ContentId { get; set; } = string.Empty;
        public CapsuleContentType ContentType { get; set; } = CapsuleContentType.Letter;
        public string ItemId { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string AuthorId { get; set; } = string.Empty;
        public float SentimentalValue { get; set; } = 50f;
    }

    [Serializable]
    public sealed class TimeCapsule
    {
        public string CapsuleId { get; set; } = string.Empty;
        public string CapsuleName { get; set; } = string.Empty;
        public string CreatorId { get; set; } = string.Empty;
        public int CreatedDay { get; set; } = 1;
        public OpenConditionType ConditionType { get; set; } = OpenConditionType.Manual;
        public int OpenDay { get; set; } = -1;
        public string TargetSurvivorId { get; set; } = string.Empty;
        public string TargetEventId { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public List<CapsuleContent> Contents { get; set; } = new List<CapsuleContent>();
        public bool IsOpen { get; set; } = false;
        public int OpenedDay { get; set; } = -1;
        public string OpenedBy { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class LegacyMessage
    {
        public string MessageId { get; set; } = string.Empty;
        public string AuthorId { get; set; } = string.Empty;
        public string RecipientId { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int CreatedDay { get; set; } = 1;
        public DeliveryCondition Condition { get; set; } = DeliveryCondition.Immediate;
        public int DeliveryDay { get; set; } = -1;
        public bool IsDelivered { get; set; } = false;
        public bool IsRead { get; set; } = false;
    }

    [Serializable]
    public sealed class TimeCapsuleState
    {
        public int SchemaVersion { get; set; } = 1;
        public int NextSequence { get; set; } = 1;
        public List<TimeCapsule> Capsules { get; set; } = new List<TimeCapsule>();
        public List<LegacyMessage> Messages { get; set; } = new List<LegacyMessage>();
    }

    /// <summary>
    /// Plan 212 — Time Capsule & Legacy Messages System.
    /// Manages time capsules buried or hidden in shelter rooms, scheduled opening conditions,
    /// contents (letters, items, drawings), and legacy messages delivered across time or milestones.
    /// </summary>
    public sealed class TimeCapsuleSystem
    {
        private readonly TimeCapsuleState _state;

        public event Action<TimeCapsule>? OnCapsuleCreated;
        public event Action<TimeCapsule, string>? OnCapsuleOpened;
        public event Action<LegacyMessage>? OnMessageCreated;
        public event Action<LegacyMessage>? OnMessageDelivered;

        public int TotalCapsuleCount => _state.Capsules.Count;
        public int UnopenedCapsuleCount => _state.Capsules.Count(c => !c.IsOpen);
        public int PendingMessageCount => _state.Messages.Count(m => !m.IsDelivered);

        public TimeCapsuleSystem(TimeCapsuleState? state = null)
        {
            _state = state ?? new TimeCapsuleState();
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
            if (string.IsNullOrWhiteSpace(capsuleName)) throw new ArgumentNullException(nameof(capsuleName));
            if (string.IsNullOrWhiteSpace(creatorId)) throw new ArgumentNullException(nameof(creatorId));

            var capsule = new TimeCapsule
            {
                CapsuleId = $"capsule_{_state.NextSequence++}",
                CapsuleName = capsuleName.Trim(),
                CreatorId = creatorId.Trim(),
                CreatedDay = Math.Max(1, createdDay),
                ConditionType = conditionType,
                OpenDay = targetOpenDay,
                TargetSurvivorId = targetSurvivorId ?? string.Empty,
                Location = location ?? string.Empty,
                Message = message ?? string.Empty,
                Contents = contents?.ToList() ?? new List<CapsuleContent>(),
                IsOpen = false,
                OpenedDay = -1,
                OpenedBy = string.Empty
            };

            _state.Capsules.Add(capsule);
            OnCapsuleCreated?.Invoke(capsule);
            return capsule;
        }

        public bool OpenCapsule(string capsuleId, string openedBy, int currentDay)
        {
            var capsule = _state.Capsules.FirstOrDefault(c => string.Equals(c.CapsuleId, capsuleId, StringComparison.OrdinalIgnoreCase));
            if (capsule == null || capsule.IsOpen) return false;

            capsule.IsOpen = true;
            capsule.OpenedDay = Math.Max(1, currentDay);
            capsule.OpenedBy = string.IsNullOrWhiteSpace(openedBy) ? "unknown" : openedBy.Trim();

            OnCapsuleOpened?.Invoke(capsule, capsule.OpenedBy);
            return true;
        }

        public LegacyMessage WriteMessage(
            string authorId,
            string recipientId,
            string content,
            DeliveryCondition condition = DeliveryCondition.Immediate,
            int deliveryDay = -1,
            int currentDay = 1)
        {
            if (string.IsNullOrWhiteSpace(authorId)) throw new ArgumentNullException(nameof(authorId));
            if (string.IsNullOrWhiteSpace(content)) throw new ArgumentNullException(nameof(content));

            var msg = new LegacyMessage
            {
                MessageId = $"legacy_msg_{_state.NextSequence++}",
                AuthorId = authorId.Trim(),
                RecipientId = recipientId ?? string.Empty,
                Content = content.Trim(),
                CreatedDay = Math.Max(1, currentDay),
                Condition = condition,
                DeliveryDay = deliveryDay,
                IsDelivered = condition == DeliveryCondition.Immediate,
                IsRead = false
            };

            _state.Messages.Add(msg);
            OnMessageCreated?.Invoke(msg);

            if (msg.IsDelivered)
            {
                OnMessageDelivered?.Invoke(msg);
            }

            return msg;
        }

        public void TickDay(int currentDay)
        {
            // Auto-open date-based capsules
            foreach (var capsule in _state.Capsules)
            {
                if (!capsule.IsOpen && capsule.ConditionType == OpenConditionType.DateBased && capsule.OpenDay > 0 && currentDay >= capsule.OpenDay)
                {
                    OpenCapsule(capsule.CapsuleId, "Shelter Community", currentDay);
                }
            }

            // Deliver date-based messages
            foreach (var msg in _state.Messages)
            {
                if (!msg.IsDelivered && msg.Condition == DeliveryCondition.OnDate && msg.DeliveryDay > 0 && currentDay >= msg.DeliveryDay)
                {
                    msg.IsDelivered = true;
                    OnMessageDelivered?.Invoke(msg);
                }
            }
        }

        public void DeliverDeathMessages(string deceasedSurvivorId)
        {
            foreach (var msg in _state.Messages)
            {
                if (!msg.IsDelivered && msg.Condition == DeliveryCondition.OnDeath && string.Equals(msg.AuthorId, deceasedSurvivorId, StringComparison.OrdinalIgnoreCase))
                {
                    msg.IsDelivered = true;
                    OnMessageDelivered?.Invoke(msg);
                }
            }
        }

        public TimeCapsule? GetCapsule(string capsuleId)
        {
            return _state.Capsules.FirstOrDefault(c => string.Equals(c.CapsuleId, capsuleId, StringComparison.OrdinalIgnoreCase));
        }

        public TimeCapsuleState CaptureState()
        {
            var state = new TimeCapsuleState
            {
                SchemaVersion = _state.SchemaVersion,
                NextSequence = _state.NextSequence,
                Capsules = new List<TimeCapsule>(_state.Capsules.Count),
                Messages = new List<LegacyMessage>(_state.Messages.Count)
            };

            foreach (var c in _state.Capsules)
            {
                state.Capsules.Add(new TimeCapsule
                {
                    CapsuleId = c.CapsuleId,
                    CapsuleName = c.CapsuleName,
                    CreatorId = c.CreatorId,
                    CreatedDay = c.CreatedDay,
                    ConditionType = c.ConditionType,
                    OpenDay = c.OpenDay,
                    TargetSurvivorId = c.TargetSurvivorId,
                    TargetEventId = c.TargetEventId,
                    Location = c.Location,
                    Message = c.Message,
                    IsOpen = c.IsOpen,
                    OpenedDay = c.OpenedDay,
                    OpenedBy = c.OpenedBy,
                    Contents = c.Contents.Select(cnt => new CapsuleContent
                    {
                        ContentId = cnt.ContentId,
                        ContentType = cnt.ContentType,
                        ItemId = cnt.ItemId,
                        Text = cnt.Text,
                        AuthorId = cnt.AuthorId,
                        SentimentalValue = cnt.SentimentalValue
                    }).ToList()
                });
            }

            foreach (var m in _state.Messages)
            {
                state.Messages.Add(new LegacyMessage
                {
                    MessageId = m.MessageId,
                    AuthorId = m.AuthorId,
                    RecipientId = m.RecipientId,
                    Content = m.Content,
                    CreatedDay = m.CreatedDay,
                    Condition = m.Condition,
                    DeliveryDay = m.DeliveryDay,
                    IsDelivered = m.IsDelivered,
                    IsRead = m.IsRead
                });
            }

            return state;
        }

        public void RestoreState(TimeCapsuleState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            _state.SchemaVersion = state.SchemaVersion;
            _state.NextSequence = state.NextSequence;
            _state.Capsules.Clear();
            _state.Messages.Clear();

            if (state.Capsules != null)
            {
                foreach (var c in state.Capsules)
                {
                    _state.Capsules.Add(new TimeCapsule
                    {
                        CapsuleId = c.CapsuleId,
                        CapsuleName = c.CapsuleName,
                        CreatorId = c.CreatorId,
                        CreatedDay = c.CreatedDay,
                        ConditionType = c.ConditionType,
                        OpenDay = c.OpenDay,
                        TargetSurvivorId = c.TargetSurvivorId,
                        TargetEventId = c.TargetEventId,
                        Location = c.Location,
                        Message = c.Message,
                        IsOpen = c.IsOpen,
                        OpenedDay = c.OpenedDay,
                        OpenedBy = c.OpenedBy,
                        Contents = (c.Contents ?? Enumerable.Empty<CapsuleContent>()).Select(cnt => new CapsuleContent
                        {
                            ContentId = cnt.ContentId,
                            ContentType = cnt.ContentType,
                            ItemId = cnt.ItemId,
                            Text = cnt.Text,
                            AuthorId = cnt.AuthorId,
                            SentimentalValue = cnt.SentimentalValue
                        }).ToList()
                    });
                }
            }

            if (state.Messages != null)
            {
                foreach (var m in state.Messages)
                {
                    _state.Messages.Add(new LegacyMessage
                    {
                        MessageId = m.MessageId,
                        AuthorId = m.AuthorId,
                        RecipientId = m.RecipientId,
                        Content = m.Content,
                        CreatedDay = m.CreatedDay,
                        Condition = m.Condition,
                        DeliveryDay = m.DeliveryDay,
                        IsDelivered = m.IsDelivered,
                        IsRead = m.IsRead
                    });
                }
            }
        }
    }
}
