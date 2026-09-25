// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : InternalCommunicationHostSession
// Core System  : Ashfall.Core.Communication.InternalCommunicationSystem
// Host Caller  : Main.InternalCommunication
// Purpose      : Plan 211 — thin identity, catalog, command, and read-model
//                adapter for the existing Core communication authority.
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core.Communication;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Result returned by every player/host communication command. The Core
    /// system remains the state authority; this type only makes refusals
    /// visible to the presentation layer without inventing a second result
    /// state machine.
    /// </summary>
    public readonly struct InternalCommunicationCommandResult
    {
        public bool Accepted { get; }
        public string ReasonKey { get; }
        public string MessageId { get; }
        public string Detail { get; }

        private InternalCommunicationCommandResult(bool accepted, string reasonKey, string messageId, string detail)
        {
            Accepted = accepted;
            ReasonKey = reasonKey ?? string.Empty;
            MessageId = messageId ?? string.Empty;
            Detail = detail ?? string.Empty;
        }

        public static InternalCommunicationCommandResult Success(string messageId, string detail = "") =>
            new(true, "accepted", messageId, detail);

        public static InternalCommunicationCommandResult Refused(string reasonKey, string detail = "") =>
            new(false, reasonKey, string.Empty, detail);
    }

    /// <summary>
    /// Godot-side adapter for the engine-free Plan 211 authority. It validates
    /// authored catalog rows and player identities before calling Core, then
    /// exposes a deliberately narrow public/mail read model to the UI.
    /// </summary>
    public sealed class InternalCommunicationHostSession : HostSessionBase
    {
        public const string WaterAdvisoryTemplateId = "comm_tpl_water_rationing_notice";
        public const int CurrentSchemaVersion = 1;

        private static readonly HashSet<string> s_allowedCategories = new(StringComparer.Ordinal)
        {
            "announcement", "notice", "request", "personal_mail", "warning", "memorial", "celebration"
        };

        private static readonly HashSet<string> s_allowedPriorities = new(StringComparer.Ordinal)
        {
            "low", "normal", "high", "urgent"
        };

        private readonly InternalCommunicationSystem _system;
        private bool _restoring;
        private Func<string, bool>? _survivorExists;
        private Func<string, bool>? _canAuthor;

        public InternalCommunicationSystem System => _system;
        public string? CatalogLoadError { get; private set; }
        public bool CatalogReady => _system.Templates.Count > 0 && CatalogLoadError == null;
        public IReadOnlyList<CommunicationMessage> PublicNotices => GetPublicNotices();
        public IReadOnlyList<BulletinBoard> Boards => _system.Boards;
        public IReadOnlyList<IntercomAnnouncement> Broadcasts => _system.Broadcasts;

        public InternalCommunicationHostSession(
            InternalCommunicationSystem system,
            Func<string, bool>? survivorExists = null,
            Func<string, bool>? canAuthor = null)
        {
            _system = system ?? throw new ArgumentNullException(nameof(system));
            _survivorExists = survivorExists;
            _canAuthor = canAuthor;
            _system.OnStateChanged += OnCoreStateChanged;
        }

        public void SetIdentityProviders(
            Func<string, bool>? survivorExists,
            Func<string, bool>? canAuthor)
        {
            _survivorExists = survivorExists;
            _canAuthor = canAuthor;
        }

        /// <summary>
        /// Strictly validates and loads the authored template catalog. Missing
        /// or malformed content fails closed and leaves the current catalog
        /// untouched; callers receive a stable reason for diagnostics/UI.
        /// </summary>
        public bool TryLoadCatalog(string json, out string reasonKey)
        {
            reasonKey = "catalog_invalid";
            if (string.IsNullOrWhiteSpace(json))
            {
                CatalogLoadError = "communication_templates.json is empty";
                return false;
            }

            CommunicationCatalogData? catalog;
            try
            {
                catalog = JsonSerializer.Deserialize<CommunicationCatalogData>(
                    json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex) when (ex is JsonException || ex is ArgumentException)
            {
                CatalogLoadError = "communication_templates.json is malformed";
                return false;
            }

            if (catalog == null)
            {
                CatalogLoadError = "communication_templates.json has no document";
                return false;
            }

            if (catalog.SchemaVersion != 1)
            {
                reasonKey = "catalog_schema_unsupported";
                CatalogLoadError = $"communication_templates.json schema_version={catalog.SchemaVersion} is unsupported";
                return false;
            }

            if (catalog.Templates == null || catalog.Templates.Count == 0)
            {
                reasonKey = "catalog_empty";
                CatalogLoadError = "communication_templates.json contains no templates";
                return false;
            }

            var ids = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var template in catalog.Templates)
            {
                if (template == null || string.IsNullOrWhiteSpace(template.Id))
                {
                    reasonKey = "template_id_missing";
                    CatalogLoadError = "communication_templates.json contains a template without an id";
                    return false;
                }

                string templateId = template.Id.Trim();
                if (!ids.Add(templateId))
                {
                    reasonKey = "template_id_duplicate";
                    CatalogLoadError = $"communication_templates.json contains duplicate id '{template.Id}'";
                    return false;
                }
                template.Id = templateId;

                if (string.IsNullOrWhiteSpace(template.Title) ||
                    string.IsNullOrWhiteSpace(template.Description))
                {
                    reasonKey = "template_text_missing";
                    CatalogLoadError = $"communication template '{template.Id}' is missing title/description";
                    return false;
                }

                string category = (template.Category ?? string.Empty).Trim().ToLowerInvariant();
                if (!s_allowedCategories.Contains(category))
                {
                    reasonKey = "template_category_invalid";
                    CatalogLoadError = $"communication template '{template.Id}' has invalid category '{template.Category}'";
                    return false;
                }

                string priority = (template.DefaultPriority ?? string.Empty).Trim().ToLowerInvariant();
                if (!s_allowedPriorities.Contains(priority))
                {
                    reasonKey = "template_priority_invalid";
                    CatalogLoadError = $"communication template '{template.Id}' has invalid priority '{template.DefaultPriority}'";
                    return false;
                }

                // 0/-1 intentionally mean non-expiring; authored finite notices
                // are bounded to one year to prevent accidental unbounded rows.
                if (template.DurationDays < -1 || template.DurationDays > 366)
                {
                    reasonKey = "template_duration_invalid";
                    CatalogLoadError = $"communication template '{template.Id}' has invalid duration_days={template.DurationDays}";
                    return false;
                }
            }

            _system.LoadCatalog(catalog);
            CatalogLoadError = null;
            reasonKey = "accepted";
            return true;
        }

        public bool TryLoadCatalogFile(string dataDirectory, out string reasonKey)
        {
            if (string.IsNullOrWhiteSpace(dataDirectory))
            {
                reasonKey = "catalog_missing";
                CatalogLoadError = "communication catalog directory is empty";
                return false;
            }

            string path = Path.Combine(dataDirectory, "communication_templates.json");
            if (!File.Exists(path))
            {
                reasonKey = "catalog_missing";
                CatalogLoadError = "communication_templates.json was not found";
                return false;
            }

            try
            {
                return TryLoadCatalog(File.ReadAllText(path), out reasonKey);
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                reasonKey = "catalog_unreadable";
                CatalogLoadError = "communication_templates.json could not be read";
                return false;
            }
        }

        public InternalCommunicationCommandResult PostWaterAdvisory(
            string authorId,
            int currentDay,
            string? details = null) =>
            PostPublicNotice(WaterAdvisoryTemplateId, authorId, currentDay, details);

        public InternalCommunicationCommandResult PostPublicNotice(
            string templateId,
            string authorId,
            int currentDay,
            string? details = null)
        {
            if (!CatalogReady)
                return InternalCommunicationCommandResult.Refused("catalog_unavailable", CatalogLoadError ?? "Communication catalog is not loaded.");
            if (currentDay < 1)
                return InternalCommunicationCommandResult.Refused("day_invalid", "Communication day must be positive.");
            if (!IsKnownSurvivor(authorId))
                return InternalCommunicationCommandResult.Refused("author_unknown", "The notice author is not in the canonical survivor roster.");
            if (!CanAuthor(authorId))
                return InternalCommunicationCommandResult.Refused("author_not_authorized", "Only the current shelter leader may author a public shelter notice.");
            if (string.IsNullOrWhiteSpace(templateId) || _system.GetTemplate(templateId) is not CommunicationTemplateDefinition template)
                return InternalCommunicationCommandResult.Refused("template_unknown", $"Unknown communication template '{templateId}'.");
            if (template.ParseCategory() == CommunicationCategory.PersonalMail)
                return InternalCommunicationCommandResult.Refused("template_not_public", "Private-mail templates cannot be posted to the public shelter board.");

            CommunicationMessage? message = _system.PostFromTemplate(
                templateId,
                authorId,
                recipientId: null,
                currentDay: currentDay,
                customDetails: details);

            return message == null
                ? InternalCommunicationCommandResult.Refused("post_failed", "Core rejected the communication post.")
                : InternalCommunicationCommandResult.Success(message.MessageId, message.Subject);
        }

        public InternalCommunicationCommandResult PostPersonalMail(
            string authorId,
            string recipientId,
            string subject,
            string content,
            int currentDay,
            int durationDays = 7)
        {
            if (!IsKnownSurvivor(authorId))
                return InternalCommunicationCommandResult.Refused("author_unknown", "The mail sender is not in the canonical survivor roster.");
            if (!IsKnownSurvivor(recipientId))
                return InternalCommunicationCommandResult.Refused("recipient_unknown", "The mail recipient is not in the canonical survivor roster.");
            if (string.Equals(authorId, recipientId, StringComparison.OrdinalIgnoreCase))
                return InternalCommunicationCommandResult.Refused("recipient_invalid", "A survivor cannot mail themselves.");
            if (string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(content))
                return InternalCommunicationCommandResult.Refused("mail_text_missing", "Mail requires a subject and content.");
            if (currentDay < 1 || durationDays < -1 || durationDays > 366)
                return InternalCommunicationCommandResult.Refused("mail_bounds_invalid", "Mail day or duration is outside the supported range.");

            CommunicationMessage message = _system.PostMessage(
                authorId,
                CommunicationCategory.PersonalMail,
                subject,
                content,
                recipientId,
                MessagePriority.Normal,
                currentDay,
                durationDays,
                CommunicationChannel.PersonalMail);
            return InternalCommunicationCommandResult.Success(message.MessageId, message.Subject);
        }

        public InternalCommunicationCommandResult BroadcastIntercom(
            string authorId,
            string message,
            MessagePriority priority,
            int currentDay)
        {
            if (currentDay < 1)
                return InternalCommunicationCommandResult.Refused("day_invalid", "Communication day must be positive.");
            if (!IsKnownSurvivor(authorId))
                return InternalCommunicationCommandResult.Refused("author_unknown", "The intercom author is not in the canonical survivor roster.");
            if (!CanAuthor(authorId))
                return InternalCommunicationCommandResult.Refused("author_not_authorized", "Only the current shelter leader may use the intercom.");
            if (string.IsNullOrWhiteSpace(message))
                return InternalCommunicationCommandResult.Refused("broadcast_empty", "An intercom broadcast requires text.");

            IntercomAnnouncement announcement = _system.BroadcastIntercom(authorId, message, priority, currentDay);
            return InternalCommunicationCommandResult.Success(announcement.AnnouncementId, announcement.Message);
        }

        public InternalCommunicationCommandResult CreateBulletinBoard(
            string authorId,
            string name,
            string roomId,
            int capacity,
            bool leadershipOnly)
        {
            if (!IsKnownSurvivor(authorId))
                return InternalCommunicationCommandResult.Refused("author_unknown", "The board author is not in the canonical survivor roster.");
            if (leadershipOnly && !CanAuthor(authorId))
                return InternalCommunicationCommandResult.Refused("author_not_authorized", "Only the current shelter leader may create a leadership-only board.");
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(roomId))
                return InternalCommunicationCommandResult.Refused("board_identity_missing", "A board requires a name and room id.");
            if (capacity < 1 || capacity > 500)
                return InternalCommunicationCommandResult.Refused("board_capacity_invalid", "Board capacity must be between 1 and 500.");

            BulletinBoard board = _system.AddBulletinBoard(name, roomId, capacity, leadershipOnly);
            return InternalCommunicationCommandResult.Success(board.BoardId, board.Name);
        }

        public IReadOnlyList<CommunicationMessage> GetPublicNotices()
        {
            return _system.GetPublicNotices()
                .OrderByDescending(message => (int)message.Priority)
                .ThenBy(message => message.PostedDay)
                .ThenBy(message => message.MessageId, StringComparer.Ordinal)
                .ToList();
        }

        public IReadOnlyList<CommunicationMessage> GetInbox(string survivorId)
        {
            return IsKnownSurvivor(survivorId)
                ? _system.GetMessagesForRecipient(survivorId)
                    .OrderBy(message => message.PostedDay)
                    .ThenBy(message => message.MessageId, StringComparer.Ordinal)
                    .ToList()
                : Array.Empty<CommunicationMessage>();
        }

        public InternalCommunicationCommandResult MarkRead(string messageId, string readerId)
        {
            if (!IsKnownSurvivor(readerId))
                return InternalCommunicationCommandResult.Refused("reader_unknown", "The reader is not in the canonical survivor roster.");

            CommunicationMessage? message = _system.Messages.FirstOrDefault(candidate =>
                string.Equals(candidate.MessageId, messageId, StringComparison.Ordinal));
            if (message == null)
                return InternalCommunicationCommandResult.Refused("message_unknown", $"Unknown communication message '{messageId}'.");
            if (!string.IsNullOrEmpty(message.RecipientId) &&
                !string.Equals(message.RecipientId, readerId, StringComparison.OrdinalIgnoreCase))
                return InternalCommunicationCommandResult.Refused("reader_not_recipient", "Private mail can only be read by its recipient.");
            if (message.IsRead)
                return InternalCommunicationCommandResult.Success(message.MessageId, "already_read");

            return _system.MarkAsRead(messageId, readerId)
                ? InternalCommunicationCommandResult.Success(message.MessageId, "read")
                : InternalCommunicationCommandResult.Refused("read_failed", "Core rejected the read marker.");
        }

        public InternalCommunicationCommandResult Acknowledge(string messageId, string acknowledgerId)
        {
            if (!IsKnownSurvivor(acknowledgerId))
                return InternalCommunicationCommandResult.Refused("acknowledger_unknown", "The acknowledger is not in the canonical survivor roster.");

            CommunicationMessage? message = _system.Messages.FirstOrDefault(candidate =>
                string.Equals(candidate.MessageId, messageId, StringComparison.Ordinal));
            if (message == null)
                return InternalCommunicationCommandResult.Refused("message_unknown", $"Unknown communication message '{messageId}'.");
            if (!string.IsNullOrEmpty(message.RecipientId) &&
                !string.Equals(message.RecipientId, acknowledgerId, StringComparison.OrdinalIgnoreCase))
                return InternalCommunicationCommandResult.Refused("acknowledger_not_recipient", "Private mail can only be acknowledged by its recipient.");
            if (message.IsAcknowledged)
                return InternalCommunicationCommandResult.Success(message.MessageId, "already_acknowledged");

            return _system.AcknowledgeMessage(messageId, acknowledgerId)
                ? InternalCommunicationCommandResult.Success(message.MessageId, "acknowledged")
                : InternalCommunicationCommandResult.Refused("acknowledge_failed", "Core rejected the acknowledgement.");
        }

        public InternalCommunicationCommandResult AcknowledgeIntercom(string announcementId, string survivorId)
        {
            if (!IsKnownSurvivor(survivorId))
                return InternalCommunicationCommandResult.Refused("acknowledger_unknown", "The acknowledger is not in the canonical survivor roster.");
            if (_system.Broadcasts.FirstOrDefault(announcement =>
                    string.Equals(announcement.AnnouncementId, announcementId, StringComparison.Ordinal)) == null)
                return InternalCommunicationCommandResult.Refused("announcement_unknown", $"Unknown intercom announcement '{announcementId}'.");

            return _system.AcknowledgeIntercom(announcementId, survivorId)
                ? InternalCommunicationCommandResult.Success(announcementId, "acknowledged")
                : InternalCommunicationCommandResult.Refused("already_acknowledged", "This survivor already acknowledged the announcement.");
        }

        public void TickDay(int currentDay)
        {
            if (currentDay < 1) return;
            _system.TickDay(currentDay);
        }

        public InternalCommunicationState CaptureState() => _system.CaptureState();

        public void RestoreState(InternalCommunicationState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            if (state.SchemaVersion > CurrentSchemaVersion)
                throw new InvalidOperationException($"Unsupported internal communication state schema {state.SchemaVersion}.");

            _restoring = true;
            try
            {
                _system.RestoreState(state);
            }
            finally
            {
                _restoring = false;
            }

            // Restore is a presentation boundary, not a new gameplay mutation.
            // Notify an already-bound panel, then clear the temporary dirty bit
            // so historical state is not saved again as a fresh command.
            RaiseStateChanged();
            ClearDirty();
        }

        public bool CanAuthor(string authorId)
        {
            if (string.IsNullOrWhiteSpace(authorId)) return false;
            // There is no ambient "leadership" bypass. Main supplies the
            // canonical LeadershipSystem decision through _canAuthor.
            return _canAuthor?.Invoke(authorId) == true;
        }

        public bool IsKnownSurvivor(string survivorId) =>
            !string.IsNullOrWhiteSpace(survivorId) && _survivorExists?.Invoke(survivorId) == true;

        protected override void UnsubscribeSystemEvents()
        {
            _system.OnStateChanged -= OnCoreStateChanged;
            _survivorExists = null;
            _canAuthor = null;
        }

        private void OnCoreStateChanged()
        {
            if (!_restoring)
                RaiseStateChanged();
        }
    }
}
