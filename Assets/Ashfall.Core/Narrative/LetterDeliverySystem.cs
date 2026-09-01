using System;
using System.Collections.Generic;

namespace Ashfall.Core.Narrative
{
    public enum LetterDeliveryState
    {
        Found = 0,
        Addressed = 1,
        Delivered = 2,
        Withheld = 3,
        Unanswered = 4
    }

    [Serializable]
    public sealed class LetterDeliveryRecord
    {
        public string letterId = string.Empty;
        public LetterDeliveryState state = LetterDeliveryState.Found;
        public int foundDay = -1;
        public int resolvedDay = -1;
        public string recipientSurvivorId = string.Empty;
        public string resolutionNotes = string.Empty;
        public float moraleDeltaApplied = 0f;
    }

    [Serializable]
    public sealed class LetterDeliverySystemState
    {
        public string systemId = LetterDeliverySystem.SystemId;
        public List<LetterDeliveryRecord> records = new List<LetterDeliveryRecord>();
    }

    /// <summary>
    /// Engine-agnostic state machine coordinating the delivery, withholding, or abandonment
    /// of survivor letters and surface dead-letters.
    /// Tracks survivor relationship resolution, morale consequences, and delivery history.
    /// </summary>
    public sealed class LetterDeliverySystem
    {
        public const string SystemId = "letter_delivery_system";

        private LetterDeliverySystemState _state = new LetterDeliverySystemState();
        private readonly ILog _log;

        public LetterDeliverySystemState State => _state;
        public IReadOnlyList<LetterDeliveryRecord> Records => _state.records;

        public event Action<LetterDeliveryRecord, float>? OnLetterDelivered;
        public event Action<LetterDeliveryRecord>? OnLetterWithheld;
        public event Action<LetterDeliveryRecord>? OnLetterUnanswered;
        public event Action<LetterDeliveryRecord>? OnLetterDiscovered;

        public LetterDeliverySystem(ILog? log = null)
        {
            _log = log ?? NullLog.Instance;
        }

        public LetterDeliveryRecord? GetRecord(string letterId)
        {
            if (string.IsNullOrEmpty(letterId)) return null;
            return _state.records.Find(r => string.Equals(r.letterId, letterId, StringComparison.OrdinalIgnoreCase));
        }

        public LetterDeliveryRecord DiscoverLetter(string letterId, int day, string recipientSurvivorId = "")
        {
            var record = GetRecord(letterId);
            if (record != null) return record;

            record = new LetterDeliveryRecord
            {
                letterId = letterId,
                state = string.IsNullOrEmpty(recipientSurvivorId) ? LetterDeliveryState.Found : LetterDeliveryState.Addressed,
                foundDay = day,
                recipientSurvivorId = recipientSurvivorId
            };
            _state.records.Add(record);
            _log.Info($"[LetterDelivery] Discovered letter '{letterId}' on day {day}");
            OnLetterDiscovered?.Invoke(record);
            return record;
        }

        public bool AddressLetter(string letterId, string recipientSurvivorId, int day)
        {
            if (string.IsNullOrEmpty(letterId) || string.IsNullOrEmpty(recipientSurvivorId)) return false;
            var record = GetRecord(letterId) ?? DiscoverLetter(letterId, day, recipientSurvivorId);
            if (record.state == LetterDeliveryState.Delivered || record.state == LetterDeliveryState.Withheld)
                return false;

            record.recipientSurvivorId = recipientSurvivorId;
            record.state = LetterDeliveryState.Addressed;
            _log.Info($"[LetterDelivery] Letter '{letterId}' addressed to survivor '{recipientSurvivorId}'");
            return true;
        }

        public bool DeliverLetter(string letterId, int day, string notes = "", float customMoraleDelta = 6.0f)
        {
            var record = GetRecord(letterId);
            if (record == null)
            {
                record = DiscoverLetter(letterId, day);
            }
            if (record.state == LetterDeliveryState.Delivered) return false;

            record.state = LetterDeliveryState.Delivered;
            record.resolvedDay = day;
            record.resolutionNotes = notes;
            record.moraleDeltaApplied = customMoraleDelta;

            _log.Info($"[LetterDelivery] Delivered letter '{letterId}' to '{record.recipientSurvivorId}' (+{customMoraleDelta} morale)");
            OnLetterDelivered?.Invoke(record, customMoraleDelta);
            return true;
        }

        public bool WithholdLetter(string letterId, int day, string notes = "")
        {
            var record = GetRecord(letterId);
            if (record == null)
            {
                record = DiscoverLetter(letterId, day);
            }
            if (record.state == LetterDeliveryState.Delivered) return false;

            record.state = LetterDeliveryState.Withheld;
            record.resolvedDay = day;
            record.resolutionNotes = notes;

            _log.Info($"[LetterDelivery] Withheld letter '{letterId}' on day {day}");
            OnLetterWithheld?.Invoke(record);
            return true;
        }

        public bool MarkUnanswered(string letterId, int day, string notes = "")
        {
            var record = GetRecord(letterId);
            if (record == null)
            {
                record = DiscoverLetter(letterId, day);
            }
            if (record.state == LetterDeliveryState.Delivered) return false;

            record.state = LetterDeliveryState.Unanswered;
            record.resolvedDay = day;
            record.resolutionNotes = notes;

            _log.Info($"[LetterDelivery] Letter '{letterId}' marked unanswered on day {day}");
            OnLetterUnanswered?.Invoke(record);
            return true;
        }

        public LetterDeliverySystemState CaptureState()
        {
            var copy = new LetterDeliverySystemState
            {
                systemId = _state.systemId
            };
            foreach (var r in _state.records)
            {
                copy.records.Add(new LetterDeliveryRecord
                {
                    letterId = r.letterId,
                    state = r.state,
                    foundDay = r.foundDay,
                    resolvedDay = r.resolvedDay,
                    recipientSurvivorId = r.recipientSurvivorId,
                    resolutionNotes = r.resolutionNotes,
                    moraleDeltaApplied = r.moraleDeltaApplied
                });
            }
            return copy;
        }

        public void RestoreState(LetterDeliverySystemState? state)
        {
            if (state == null) return;
            _state = new LetterDeliverySystemState
            {
                systemId = state.systemId
            };
            if (state.records != null)
            {
                foreach (var r in state.records)
                {
                    _state.records.Add(new LetterDeliveryRecord
                    {
                        letterId = r.letterId,
                        state = r.state,
                        foundDay = r.foundDay,
                        resolvedDay = r.resolvedDay,
                        recipientSurvivorId = r.recipientSurvivorId,
                        resolutionNotes = r.resolutionNotes,
                        moraleDeltaApplied = r.moraleDeltaApplied
                    });
                }
            }
        }
    }
}
