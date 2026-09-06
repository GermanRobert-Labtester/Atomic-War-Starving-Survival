// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Narrative
{
    public static class LetterDeliveryStates
    {
        public const string NotFound = "not_found";
        public const string Found = "found";
        public const string Addressed = "addressed";
        public const string Delivered = "delivered";
        public const string Withheld = "withheld";
        public const string Unanswered = "unanswered";
    }

    [Serializable]
    public sealed class SurvivorLetterRecordState
    {
        public string letter_id = string.Empty;
        public string delivery_state = LetterDeliveryStates.NotFound;
        public string? matched_survivor_id;
        public int found_day = -1;
        public int resolved_day = -1;
        public float morale_delta_applied;
    }

    [Serializable]
    public sealed class SurvivorLetterDeliverySaveState
    {
        public int schema_version = 1;
        public List<SurvivorLetterRecordState> letters = new List<SurvivorLetterRecordState>();
    }

    /// <summary>
    /// Lightweight survivor info passed for address matching.
    /// </summary>
    public sealed class DwellerAddressCandidate
    {
        public string SurvivorId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsAlive { get; set; } = true;
    }

    /// <summary>
    /// State machine tracking the 25 Unopened Letters to Lost Kin & Surface Postboxes.
    /// Manages discovery, recipient resolution, delivery, and withholding consequences.
    /// Engine-agnostic, deterministic, and save-safe.
    /// </summary>
    public sealed class SurvivorLetterDeliverySystem
    {
        public const string SystemId = "survivor_letter_delivery";
        public const float DefaultDeliveryMoraleBonus = 8f;
        public const float DefaultWithholdMoralePenalty = -3f;

        private readonly Dictionary<string, SurvivorLetterRecordState> _records =
            new Dictionary<string, SurvivorLetterRecordState>(StringComparer.OrdinalIgnoreCase);

        private readonly SurvivorLetterCatalog? _catalog;
        private readonly ILog _log;

        public event Action<string, int>? OnLetterFound;
        public event Action<string, string>? OnLetterAddressed;
        public event Action<string, string, float>? OnLetterDelivered;
        public event Action<string, string?>? OnLetterWithheld;
        public event Action? OnStateChanged;

        public SurvivorLetterDeliverySystem(SurvivorLetterCatalog? catalog = null, ILog? log = null)
        {
            _catalog = catalog;
            _log = log ?? NullLog.Instance;
        }

        public SurvivorLetterRecordState GetOrCreateRecord(string letterId)
        {
            if (!_records.TryGetValue(letterId, out var rec))
            {
                rec = new SurvivorLetterRecordState
                {
                    letter_id = letterId,
                    delivery_state = LetterDeliveryStates.NotFound
                };
                _records[letterId] = rec;
            }
            return rec;
        }

        public string GetState(string letterId)
        {
            return _records.TryGetValue(letterId, out var rec) ? rec.delivery_state : LetterDeliveryStates.NotFound;
        }

        public IReadOnlyList<SurvivorLetterRecordState> GetAllRecords()
        {
            return _records.Values.OrderBy(r => r.letter_id, StringComparer.Ordinal).ToList();
        }

        public IReadOnlyList<SurvivorLetterRecordState> GetRecordsByState(string deliveryState)
        {
            return _records.Values
                .Where(r => string.Equals(r.delivery_state, deliveryState, StringComparison.OrdinalIgnoreCase))
                .OrderBy(r => r.letter_id, StringComparer.Ordinal)
                .ToList();
        }

        public bool MarkFound(string letterId, int day)
        {
            if (string.IsNullOrEmpty(letterId)) return false;
            var rec = GetOrCreateRecord(letterId);
            if (rec.delivery_state != LetterDeliveryStates.NotFound) return false;

            rec.delivery_state = LetterDeliveryStates.Found;
            rec.found_day = day;
            _log.Info($"[Letters] Letter '{letterId}' found on day {day}");
            OnLetterFound?.Invoke(letterId, day);
            OnStateChanged?.Invoke();
            return true;
        }

        public bool TryAddressToSurvivor(
            string letterId,
            IEnumerable<DwellerAddressCandidate> livingDwellers)
        {
            if (string.IsNullOrEmpty(letterId) || livingDwellers == null) return false;
            var rec = GetOrCreateRecord(letterId);
            if (rec.delivery_state != LetterDeliveryStates.Found) return false;

            var entry = _catalog?.GetById(letterId);
            string recipientQuery = entry?.intended_recipient ?? string.Empty;
            string authorQuery = entry?.author_dweller ?? string.Empty;

            DwellerAddressCandidate? bestMatch = null;

            foreach (var dweller in livingDwellers)
            {
                if (!dweller.IsAlive) continue;

                // Match by recipient name or author dweller name in shelter
                if (!string.IsNullOrEmpty(recipientQuery) &&
                    (recipientQuery.IndexOf(dweller.Name, StringComparison.OrdinalIgnoreCase) >= 0 ||
                     dweller.Name.IndexOf(recipientQuery, StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    bestMatch = dweller;
                    break;
                }

                if (!string.IsNullOrEmpty(authorQuery) &&
                    (authorQuery.IndexOf(dweller.Name, StringComparison.OrdinalIgnoreCase) >= 0 ||
                     dweller.Name.IndexOf(authorQuery, StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    bestMatch = dweller;
                    break;
                }
            }

            if (bestMatch != null)
            {
                rec.matched_survivor_id = bestMatch.SurvivorId;
                rec.delivery_state = LetterDeliveryStates.Addressed;
                _log.Info($"[Letters] Letter '{letterId}' addressed to {bestMatch.Name} ({bestMatch.SurvivorId})");
                OnLetterAddressed?.Invoke(letterId, bestMatch.SurvivorId);
                OnStateChanged?.Invoke();
                return true;
            }

            return false;
        }

        public bool AssignRecipientExplicit(string letterId, string survivorId)
        {
            if (string.IsNullOrEmpty(letterId) || string.IsNullOrEmpty(survivorId)) return false;
            var rec = GetOrCreateRecord(letterId);
            if (rec.delivery_state != LetterDeliveryStates.Found && rec.delivery_state != LetterDeliveryStates.Addressed)
                return false;

            rec.matched_survivor_id = survivorId;
            rec.delivery_state = LetterDeliveryStates.Addressed;
            OnLetterAddressed?.Invoke(letterId, survivorId);
            OnStateChanged?.Invoke();
            return true;
        }

        public bool Deliver(string letterId, int day, Action<string, float>? applyMorale = null, float moraleBonus = DefaultDeliveryMoraleBonus)
        {
            if (string.IsNullOrEmpty(letterId)) return false;
            var rec = GetOrCreateRecord(letterId);
            if (rec.delivery_state != LetterDeliveryStates.Addressed && rec.delivery_state != LetterDeliveryStates.Found)
                return false;

            rec.delivery_state = LetterDeliveryStates.Delivered;
            rec.resolved_day = day;
            rec.morale_delta_applied = moraleBonus;

            string targetId = rec.matched_survivor_id ?? string.Empty;
            if (!string.IsNullOrEmpty(targetId) && applyMorale != null)
            {
                applyMorale.Invoke(targetId, moraleBonus);
            }

            _log.Info($"[Letters] Letter '{letterId}' delivered on day {day}. Morale bonus: +{moraleBonus}");
            OnLetterDelivered?.Invoke(letterId, targetId, moraleBonus);
            OnStateChanged?.Invoke();
            return true;
        }

        public bool Withhold(string letterId, int day, Action<string, float>? applyMorale = null, float moralePenalty = DefaultWithholdMoralePenalty)
        {
            if (string.IsNullOrEmpty(letterId)) return false;
            var rec = GetOrCreateRecord(letterId);
            if (rec.delivery_state != LetterDeliveryStates.Addressed && rec.delivery_state != LetterDeliveryStates.Found)
                return false;

            rec.delivery_state = LetterDeliveryStates.Withheld;
            rec.resolved_day = day;
            rec.morale_delta_applied = moralePenalty;

            string targetId = rec.matched_survivor_id ?? string.Empty;
            if (!string.IsNullOrEmpty(targetId) && applyMorale != null)
            {
                applyMorale.Invoke(targetId, moralePenalty);
            }

            _log.Info($"[Letters] Letter '{letterId}' withheld on day {day}. Morale penalty: {moralePenalty}");
            OnLetterWithheld?.Invoke(letterId, rec.matched_survivor_id);
            OnStateChanged?.Invoke();
            return true;
        }

        public bool MarkUnanswered(string letterId, int day)
        {
            if (string.IsNullOrEmpty(letterId)) return false;
            var rec = GetOrCreateRecord(letterId);
            if (rec.delivery_state != LetterDeliveryStates.Found && rec.delivery_state != LetterDeliveryStates.NotFound)
                return false;

            rec.delivery_state = LetterDeliveryStates.Unanswered;
            rec.resolved_day = day;
            _log.Info($"[Letters] Letter '{letterId}' marked unanswered on day {day}");
            OnStateChanged?.Invoke();
            return true;
        }

        public SurvivorLetterDeliverySaveState CaptureState()
        {
            var save = new SurvivorLetterDeliverySaveState();
            foreach (var kvp in _records.OrderBy(k => k.Key, StringComparer.Ordinal))
            {
                var r = kvp.Value;
                save.letters.Add(new SurvivorLetterRecordState
                {
                    letter_id = r.letter_id,
                    delivery_state = r.delivery_state,
                    matched_survivor_id = r.matched_survivor_id,
                    found_day = r.found_day,
                    resolved_day = r.resolved_day,
                    morale_delta_applied = r.morale_delta_applied
                });
            }
            return save;
        }

        public void RestoreState(SurvivorLetterDeliverySaveState? save)
        {
            if (save?.letters == null) return;
            _records.Clear();
            foreach (var r in save.letters)
            {
                if (r == null || string.IsNullOrEmpty(r.letter_id)) continue;
                _records[r.letter_id] = new SurvivorLetterRecordState
                {
                    letter_id = r.letter_id,
                    delivery_state = r.delivery_state,
                    matched_survivor_id = r.matched_survivor_id,
                    found_day = r.found_day,
                    resolved_day = r.resolved_day,
                    morale_delta_applied = r.morale_delta_applied
                };
            }
            OnStateChanged?.Invoke();
        }
    }
}
