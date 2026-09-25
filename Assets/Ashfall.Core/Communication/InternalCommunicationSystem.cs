// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Communication
{
    public enum CommunicationCategory
    {
        Announcement = 0,
        Notice = 1,
        Request = 2,
        PersonalMail = 3,
        Warning = 4,
        Memorial = 5,
        Celebration = 6
    }

    public enum CommunicationChannel
    {
        BulletinBoard = 0,
        Intercom = 1,
        PersonalMail = 2
    }

    public enum MessagePriority
    {
        Low = 0,
        Normal = 1,
        High = 2,
        Urgent = 3
    }

    [Serializable]
    public sealed class CommunicationTemplateDefinition
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("category")]
        public string Category { get; set; } = "notice";

        [JsonPropertyName("default_priority")]
        public string DefaultPriority { get; set; } = "normal";

        [JsonPropertyName("duration_days")]
        public int DurationDays { get; set; } = 7;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        public CommunicationCategory ParseCategory() => Category?.ToLowerInvariant() switch
        {
            "announcement" => CommunicationCategory.Announcement,
            "request" => CommunicationCategory.Request,
            "personal_mail" => CommunicationCategory.PersonalMail,
            "warning" => CommunicationCategory.Warning,
            "memorial" => CommunicationCategory.Memorial,
            "celebration" => CommunicationCategory.Celebration,
            _ => CommunicationCategory.Notice
        };

        public MessagePriority ParsePriority() => DefaultPriority?.ToLowerInvariant() switch
        {
            "low" => MessagePriority.Low,
            "high" => MessagePriority.High,
            "urgent" => MessagePriority.Urgent,
            _ => MessagePriority.Normal
        };
    }

    [Serializable]
    public sealed class CommunicationCatalogData
    {
        [JsonPropertyName("schema_version")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("templates")]
        public List<CommunicationTemplateDefinition> Templates { get; set; } = new List<CommunicationTemplateDefinition>();
    }

    [Serializable]
    public sealed class CommunicationMessage
    {
        public string MessageId { get; set; } = string.Empty;
        public CommunicationCategory Category { get; set; } = CommunicationCategory.Notice;
        public CommunicationChannel Channel { get; set; } = CommunicationChannel.BulletinBoard;
        public string AuthorId { get; set; } = string.Empty;
        public string RecipientId { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public MessagePriority Priority { get; set; } = MessagePriority.Normal;
        public int PostedDay { get; set; } = 1;
        public int ExpiresDay { get; set; } = 8;
        public bool IsRead { get; set; } = false;
        public bool IsAcknowledged { get; set; } = false;
    }

    [Serializable]
    public sealed class IntercomAnnouncement
    {
        public string AnnouncementId { get; set; } = string.Empty;
        public string AuthorId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public MessagePriority Priority { get; set; } = MessagePriority.Normal;
        public int BroadcastDay { get; set; } = 1;
        public List<string> AcknowledgedSurvivors { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class BulletinBoard
    {
        public string BoardId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string RoomId { get; set; } = string.Empty;
        public int Capacity { get; set; } = 20;
        public bool IsLeadershipOnly { get; set; } = false;
    }

    [Serializable]
    public sealed class InternalCommunicationState
    {
        public int SchemaVersion { get; set; } = 1;
        public int NextSequence { get; set; } = 1;
        public List<CommunicationMessage> Messages { get; set; } = new List<CommunicationMessage>();
        public List<BulletinBoard> Boards { get; set; } = new List<BulletinBoard>();
        public List<IntercomAnnouncement> IntercomBroadcasts { get; set; } = new List<IntercomAnnouncement>();
    }

    /// <summary>
    /// Plan 211 — Internal Shelter Communication Network System.
    /// Manages shelter bulletin boards, intercom broadcasts, internal mail, and notice distribution.
    /// Engine-agnostic domain authority with deterministic state tracking.
    /// </summary>
    public sealed class InternalCommunicationSystem
    {
        public const int CurrentSchemaVersion = 1;

        private readonly InternalCommunicationState _state;
        private readonly Dictionary<string, CommunicationTemplateDefinition> _templates =
            new Dictionary<string, CommunicationTemplateDefinition>(StringComparer.OrdinalIgnoreCase);

        public event Action<CommunicationMessage>? OnMessagePosted;
        public event Action<CommunicationMessage, string>? OnMessageRead;
        public event Action<CommunicationMessage, string>? OnMessageAcknowledged;
        public event Action<IntercomAnnouncement>? OnIntercomBroadcast;
        public event Action? OnStateChanged;

        public IReadOnlyList<CommunicationMessage> Messages => _state.Messages;
        public IReadOnlyList<BulletinBoard> Boards => _state.Boards;
        public IReadOnlyList<IntercomAnnouncement> Broadcasts => _state.IntercomBroadcasts;
        public IReadOnlyCollection<CommunicationTemplateDefinition> Templates => _templates.Values;

        public InternalCommunicationSystem(InternalCommunicationState? state = null)
        {
            _state = state ?? new InternalCommunicationState();
            EnsureDefaultBoard();
        }

        private void EnsureDefaultBoard()
        {
            if (_state.Boards.Count != 0) return;
            _state.Boards.Add(new BulletinBoard
            {
                BoardId = "board_main_commons",
                Name = "Common Room Notice Board",
                RoomId = "room_mess_hall",
                Capacity = 25,
                IsLeadershipOnly = false
            });
        }

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var data = JsonSerializer.Deserialize<CommunicationCatalogData>(json, options);
            if (data != null)
            {
                LoadCatalog(data);
            }
        }

        public void LoadCatalog(CommunicationCatalogData catalog)
        {
            if (catalog?.Templates == null) return;

            // The authored file is the catalog authority. A successful reload
            // replaces the prior definition set instead of leaving removed
            // templates reachable through a stale in-memory merge.
            _templates.Clear();
            foreach (var t in catalog.Templates)
            {
                if (t == null || string.IsNullOrWhiteSpace(t.Id)) continue;
                t.Id = t.Id.Trim();
                _templates[t.Id] = t;
            }
        }

        public CommunicationTemplateDefinition? GetTemplate(string templateId)
        {
            if (string.IsNullOrWhiteSpace(templateId)) return null;
            return _templates.TryGetValue(templateId, out var t) ? t : null;
        }

        public CommunicationMessage PostMessage(
            string authorId,
            CommunicationCategory category,
            string subject,
            string content,
            string? recipientId = null,
            MessagePriority priority = MessagePriority.Normal,
            int currentDay = 1,
            int durationDays = 7,
            CommunicationChannel channel = CommunicationChannel.BulletinBoard)
        {
            var msg = new CommunicationMessage
            {
                MessageId = $"msg_{_state.NextSequence++}",
                Category = category,
                Channel = channel,
                AuthorId = authorId ?? string.Empty,
                RecipientId = recipientId ?? string.Empty,
                Subject = subject ?? "Notice",
                Content = content ?? string.Empty,
                Priority = priority,
                PostedDay = currentDay,
                ExpiresDay = durationDays > 0 ? currentDay + durationDays : -1,
                IsRead = false,
                IsAcknowledged = false
            };

            _state.Messages.Add(msg);
            OnMessagePosted?.Invoke(msg);
            OnStateChanged?.Invoke();
            return msg;
        }

        public CommunicationMessage? PostFromTemplate(
            string templateId,
            string authorId,
            string? recipientId = null,
            int currentDay = 1,
            string? customDetails = null)
        {
            if (string.IsNullOrWhiteSpace(templateId)) return null;
            if (!_templates.TryGetValue(templateId, out var tpl)) return null;

            string content = string.IsNullOrWhiteSpace(customDetails)
                ? tpl.Description
                : $"{tpl.Description} Details: {customDetails}";

            return PostMessage(
                authorId,
                tpl.ParseCategory(),
                tpl.Title,
                content,
                recipientId,
                tpl.ParsePriority(),
                currentDay,
                tpl.DurationDays,
                string.IsNullOrEmpty(recipientId) ? CommunicationChannel.BulletinBoard : CommunicationChannel.PersonalMail);
        }

        public IntercomAnnouncement BroadcastIntercom(
            string authorId,
            string message,
            MessagePriority priority = MessagePriority.Normal,
            int currentDay = 1)
        {
            var broadcast = new IntercomAnnouncement
            {
                AnnouncementId = $"ann_{_state.NextSequence++}",
                AuthorId = authorId ?? "leadership",
                Message = message ?? string.Empty,
                Priority = priority,
                BroadcastDay = currentDay,
                AcknowledgedSurvivors = new List<string>()
            };

            _state.IntercomBroadcasts.Add(broadcast);

            // Also mirror as high-priority shelter announcement message
            PostMessage(
                authorId,
                CommunicationCategory.Announcement,
                "Intercom Broadcast",
                message,
                recipientId: null,
                priority: priority,
                currentDay: currentDay,
                durationDays: 3,
                channel: CommunicationChannel.Intercom);

            OnIntercomBroadcast?.Invoke(broadcast);
            OnStateChanged?.Invoke();
            return broadcast;
        }

        public bool AcknowledgeIntercom(string announcementId, string survivorId)
        {
            if (string.IsNullOrWhiteSpace(announcementId) || string.IsNullOrWhiteSpace(survivorId))
                return false;

            var ann = _state.IntercomBroadcasts.FirstOrDefault(a => a.AnnouncementId == announcementId);
            if (ann == null || ann.AcknowledgedSurvivors.Contains(survivorId))
                return false;

            ann.AcknowledgedSurvivors.Add(survivorId);
            OnStateChanged?.Invoke();
            return true;
        }

        public bool MarkAsRead(string messageId, string readerId)
        {
            if (string.IsNullOrWhiteSpace(messageId)) return false;
            var msg = _state.Messages.FirstOrDefault(m => m.MessageId == messageId);
            if (msg == null) return false;

            msg.IsRead = true;
            OnMessageRead?.Invoke(msg, readerId);
            OnStateChanged?.Invoke();
            return true;
        }

        public bool AcknowledgeMessage(string messageId, string acknowledgerId)
        {
            if (string.IsNullOrWhiteSpace(messageId)) return false;
            var msg = _state.Messages.FirstOrDefault(m => m.MessageId == messageId);
            if (msg == null) return false;

            msg.IsAcknowledged = true;
            msg.IsRead = true;
            OnMessageAcknowledged?.Invoke(msg, acknowledgerId);
            OnStateChanged?.Invoke();
            return true;
        }

        public IReadOnlyList<CommunicationMessage> GetMessagesForRecipient(string survivorId)
        {
            if (string.IsNullOrWhiteSpace(survivorId)) return Array.Empty<CommunicationMessage>();
            return _state.Messages
                .Where(m => string.Equals(m.RecipientId, survivorId, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public IReadOnlyList<CommunicationMessage> GetPublicNotices()
        {
            return _state.Messages
                .Where(m => string.IsNullOrEmpty(m.RecipientId))
                .ToList();
        }

        public BulletinBoard AddBulletinBoard(string name, string roomId, int capacity = 20, bool isLeadershipOnly = false)
        {
            var board = new BulletinBoard
            {
                BoardId = $"board_{_state.NextSequence++}",
                Name = name ?? "Notice Board",
                RoomId = roomId ?? "room_hallway",
                Capacity = capacity,
                IsLeadershipOnly = isLeadershipOnly
            };
            _state.Boards.Add(board);
            OnStateChanged?.Invoke();
            return board;
        }

        public void TickDay(int currentDay)
        {
            bool changed = false;
            for (int i = _state.Messages.Count - 1; i >= 0; i--)
            {
                var msg = _state.Messages[i];
                if (msg.ExpiresDay > 0 && currentDay >= msg.ExpiresDay)
                {
                    _state.Messages.RemoveAt(i);
                    changed = true;
                }
            }

            if (changed)
            {
                OnStateChanged?.Invoke();
            }
        }

        public InternalCommunicationState CaptureState()
        {
            var capture = new InternalCommunicationState
            {
                SchemaVersion = _state.SchemaVersion,
                NextSequence = _state.NextSequence,
                Messages = new List<CommunicationMessage>(_state.Messages.Count),
                Boards = new List<BulletinBoard>(_state.Boards.Count),
                IntercomBroadcasts = new List<IntercomAnnouncement>(_state.IntercomBroadcasts.Count)
            };

            foreach (var m in _state.Messages)
            {
                capture.Messages.Add(new CommunicationMessage
                {
                    MessageId = m.MessageId,
                    Category = m.Category,
                    Channel = m.Channel,
                    AuthorId = m.AuthorId,
                    RecipientId = m.RecipientId,
                    Subject = m.Subject,
                    Content = m.Content,
                    Priority = m.Priority,
                    PostedDay = m.PostedDay,
                    ExpiresDay = m.ExpiresDay,
                    IsRead = m.IsRead,
                    IsAcknowledged = m.IsAcknowledged
                });
            }

            foreach (var b in _state.Boards)
            {
                capture.Boards.Add(new BulletinBoard
                {
                    BoardId = b.BoardId,
                    Name = b.Name,
                    RoomId = b.RoomId,
                    Capacity = b.Capacity,
                    IsLeadershipOnly = b.IsLeadershipOnly
                });
            }

            foreach (var a in _state.IntercomBroadcasts)
            {
                capture.IntercomBroadcasts.Add(new IntercomAnnouncement
                {
                    AnnouncementId = a.AnnouncementId,
                    AuthorId = a.AuthorId,
                    Message = a.Message,
                    Priority = a.Priority,
                    BroadcastDay = a.BroadcastDay,
                    AcknowledgedSurvivors = new List<string>(a.AcknowledgedSurvivors ?? new List<string>())
                });
            }

            return capture;
        }

        public void RestoreState(InternalCommunicationState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            if (state.SchemaVersion > CurrentSchemaVersion)
                throw new InvalidOperationException($"Unsupported internal communication state schema {state.SchemaVersion}.");

            // A missing schema field is a legacy v1 payload, not a reason to
            // reject an otherwise readable campaign. Future versions fail
            // explicitly above rather than being silently downgraded.
            _state.SchemaVersion = state.SchemaVersion <= 0 ? CurrentSchemaVersion : state.SchemaVersion;
            _state.Messages.Clear();
            _state.Boards.Clear();
            _state.IntercomBroadcasts.Clear();

            if (state.Messages != null)
            {
                foreach (var m in state.Messages)
                {
                    _state.Messages.Add(new CommunicationMessage
                    {
                        MessageId = m.MessageId,
                        Category = m.Category,
                        Channel = m.Channel,
                        AuthorId = m.AuthorId,
                        RecipientId = m.RecipientId,
                        Subject = m.Subject,
                        Content = m.Content,
                        Priority = m.Priority,
                        PostedDay = m.PostedDay,
                        ExpiresDay = m.ExpiresDay,
                        IsRead = m.IsRead,
                        IsAcknowledged = m.IsAcknowledged
                    });
                }
            }

            if (state.Boards != null)
            {
                foreach (var b in state.Boards)
                {
                    _state.Boards.Add(new BulletinBoard
                    {
                        BoardId = b.BoardId,
                        Name = b.Name,
                        RoomId = b.RoomId,
                        Capacity = b.Capacity,
                        IsLeadershipOnly = b.IsLeadershipOnly
                    });
                }
            }

            if (state.IntercomBroadcasts != null)
            {
                foreach (var a in state.IntercomBroadcasts)
                {
                    _state.IntercomBroadcasts.Add(new IntercomAnnouncement
                    {
                        AnnouncementId = a.AnnouncementId,
                        AuthorId = a.AuthorId,
                        Message = a.Message,
                        Priority = a.Priority,
                        BroadcastDay = a.BroadcastDay,
                        AcknowledgedSurvivors = new List<string>(a.AcknowledgedSurvivors ?? new List<string>())
                    });
                }
            }

            int nextSequence = Math.Max(1, state.NextSequence);
            foreach (var message in _state.Messages)
                nextSequence = Math.Max(nextSequence, NextSequenceAfter(message.MessageId));
            foreach (var board in _state.Boards)
                nextSequence = Math.Max(nextSequence, NextSequenceAfter(board.BoardId));
            foreach (var broadcast in _state.IntercomBroadcasts)
                nextSequence = Math.Max(nextSequence, NextSequenceAfter(broadcast.AnnouncementId));
            _state.NextSequence = nextSequence;

            EnsureDefaultBoard();
            OnStateChanged?.Invoke();
        }

        private static int NextSequenceAfter(string? id)
        {
            if (string.IsNullOrWhiteSpace(id)) return 1;
            int separator = id.LastIndexOf('_');
            if (separator < 0 || separator >= id.Length - 1) return 1;
            return int.TryParse(id.AsSpan(separator + 1), out int value) && value >= 0
                ? value + 1
                : 1;
        }
    }
}
