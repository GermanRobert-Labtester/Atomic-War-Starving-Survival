// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Survivors
{
    public enum BelongingCategory
    {
        Keepsake = 0,
        Clothing = 1,
        Tool = 2,
        Weapon = 3,
        Memento = 4,
        Document = 5,
        Jewelry = 6
    }

    public enum BelongingTransferType
    {
        Gift = 0,
        Inheritance = 1,
        Trade = 2,
        Theft = 3,
        Confiscation = 4,
        Assignment = 5
    }

    [Serializable]
    public sealed class PersonalBelonging
    {
        public string BelongingId { get; set; } = string.Empty;
        public string OwnerSurvivorId { get; set; } = string.Empty;
        public string ItemId { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public BelongingCategory Category { get; set; } = BelongingCategory.Keepsake;
        public float SentimentalValue { get; set; } = 50f;
        public float Condition { get; set; } = 100f;
        public int AcquiredDay { get; set; } = 1;
        public string AcquiredFrom { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsFavorite { get; set; } = false;
        public bool IsInherited { get; set; } = false;
    }

    [Serializable]
    public sealed class BelongingTransfer
    {
        public string TransferId { get; set; } = string.Empty;
        public string BelongingId { get; set; } = string.Empty;
        public string FromSurvivorId { get; set; } = string.Empty;
        public string ToSurvivorId { get; set; } = string.Empty;
        public BelongingTransferType TransferType { get; set; } = BelongingTransferType.Gift;
        public int TransferDay { get; set; } = 1;
        public string Reason { get; set; } = string.Empty;
        public float SentimentalEffect { get; set; } = 0f;
    }

    [Serializable]
    public sealed class BelongingEvent
    {
        public string EventId { get; set; } = string.Empty;
        public string EventType { get; set; } = string.Empty;
        public string SurvivorId { get; set; } = string.Empty;
        public string BelongingId { get; set; } = string.Empty;
        public int Day { get; set; } = 1;
        public string Description { get; set; } = string.Empty;
        public float MoraleEffect { get; set; } = 0f;
    }

    [Serializable]
    public sealed class PersonalBelongingsState
    {
        public int SchemaVersion { get; set; } = 1;
        public int NextSequence { get; set; } = 1;
        public int MaxBelongingsPerSurvivor { get; set; } = 10;
        public List<PersonalBelonging> Belongings { get; set; } = new List<PersonalBelonging>();
        public List<BelongingTransfer> Transfers { get; set; } = new List<BelongingTransfer>();
        public List<BelongingEvent> Events { get; set; } = new List<BelongingEvent>();
    }

    /// <summary>
    /// Plan 210 — Survivor Personal Belongings & Effects System.
    /// Tracks survivor personal possessions (keepsakes, clothing, tools, mementos, documents),
    /// sentimental value, favorite designations, gift-giving, inheritance, and morale impact.
    /// </summary>
    public sealed class PersonalBelongingsSystem
    {
        private readonly PersonalBelongingsState _state;

        public event Action<PersonalBelonging>? OnBelongingAcquired;
        public event Action<BelongingTransfer>? OnBelongingGifted;
        public event Action<PersonalBelonging, string>? OnBelongingInherited;
        public event Action<BelongingEvent>? OnBelongingLost;
        public event Action<PersonalBelonging, bool>? OnFavoriteToggled;

        public int TotalBelongingsCount => _state.Belongings.Count;

        /// <summary>Canonical claim metadata; physical item stacks remain owned by Inventory.</summary>
        public IReadOnlyList<PersonalBelonging> Belongings => _state.Belongings;

        /// <summary>
        /// Returns whether an inventory item already has a personal claim. A
        /// claim is intentionally one-per-item-id because the shared inventory
        /// has no per-instance ownership identity; this prevents two survivors
        /// from claiming the same physical definition as their keepsake.
        /// </summary>
        public bool HasClaimForItem(string itemId)
        {
            if (string.IsNullOrWhiteSpace(itemId)) return false;
            return _state.Belongings.Any(b =>
                b != null && string.Equals(b.ItemId, itemId, StringComparison.OrdinalIgnoreCase));
        }

        public PersonalBelongingsSystem(PersonalBelongingsState? state = null)
        {
            _state = state ?? new PersonalBelongingsState();
        }

        public PersonalBelonging? RegisterBelonging(
            string ownerSurvivorId,
            string itemId,
            string itemName,
            BelongingCategory category,
            float sentimentalValue = 50f,
            float condition = 100f,
            int acquiredDay = 1,
            string acquiredFrom = "",
            string description = "")
        {
            if (string.IsNullOrWhiteSpace(ownerSurvivorId)) throw new ArgumentNullException(nameof(ownerSurvivorId));

            int currentCount = _state.Belongings.Count(b => string.Equals(b.OwnerSurvivorId, ownerSurvivorId, StringComparison.OrdinalIgnoreCase));
            if (currentCount >= _state.MaxBelongingsPerSurvivor)
            {
                return null; // capacity reached
            }

            var item = new PersonalBelonging
            {
                BelongingId = $"blg_{_state.NextSequence++}",
                OwnerSurvivorId = ownerSurvivorId.Trim(),
                ItemId = itemId ?? string.Empty,
                ItemName = string.IsNullOrWhiteSpace(itemName) ? "Keepsake" : itemName.Trim(),
                Category = category,
                SentimentalValue = Math.Clamp(sentimentalValue, 0f, 100f),
                Condition = Math.Clamp(condition, 0f, 100f),
                AcquiredDay = Math.Max(1, acquiredDay),
                AcquiredFrom = acquiredFrom ?? string.Empty,
                Description = description ?? string.Empty,
                IsFavorite = false,
                IsInherited = false
            };

            _state.Belongings.Add(item);

            var ev = new BelongingEvent
            {
                EventId = $"bev_{_state.NextSequence++}",
                EventType = "item_acquired",
                SurvivorId = ownerSurvivorId,
                BelongingId = item.BelongingId,
                Day = acquiredDay,
                Description = $"{ownerSurvivorId} acquired personal {category}: {item.ItemName}.",
                MoraleEffect = item.SentimentalValue * 0.05f
            };
            _state.Events.Add(ev);

            OnBelongingAcquired?.Invoke(item);
            return item;
        }

        public IReadOnlyList<PersonalBelonging> GetBelongingsForSurvivor(string survivorId)
        {
            if (string.IsNullOrWhiteSpace(survivorId)) return Array.Empty<PersonalBelonging>();
            return _state.Belongings.Where(b => string.Equals(b.OwnerSurvivorId, survivorId, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public PersonalBelonging? GetBelonging(string belongingId)
        {
            if (string.IsNullOrWhiteSpace(belongingId)) return null;
            return _state.Belongings.FirstOrDefault(b => string.Equals(b.BelongingId, belongingId, StringComparison.OrdinalIgnoreCase));
        }

        public bool SetFavorite(string survivorId, string belongingId, bool isFavorite)
        {
            var item = GetBelonging(belongingId);
            if (item == null || !string.Equals(item.OwnerSurvivorId, survivorId, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (isFavorite)
            {
                // Unset any previous favorite for this survivor
                var prevFavorites = _state.Belongings.Where(b => string.Equals(b.OwnerSurvivorId, survivorId, StringComparison.OrdinalIgnoreCase) && b.IsFavorite);
                foreach (var prev in prevFavorites)
                {
                    prev.IsFavorite = false;
                }
            }

            item.IsFavorite = isFavorite;
            OnFavoriteToggled?.Invoke(item, isFavorite);
            return true;
        }

        public BelongingTransfer? GiftBelonging(string fromSurvivorId, string toSurvivorId, string belongingId, int currentDay, string reason = "Gift")
        {
            var item = GetBelonging(belongingId);
            if (item == null || !string.Equals(item.OwnerSurvivorId, fromSurvivorId, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            int recipientCount = _state.Belongings.Count(b => string.Equals(b.OwnerSurvivorId, toSurvivorId, StringComparison.OrdinalIgnoreCase));
            if (recipientCount >= _state.MaxBelongingsPerSurvivor)
            {
                return null;
            }

            item.OwnerSurvivorId = toSurvivorId;
            item.IsFavorite = false;

            float moraleBonus = MathF.Round(item.SentimentalValue * 0.1f, 1);
            var transfer = new BelongingTransfer
            {
                TransferId = $"tr_{_state.NextSequence++}",
                BelongingId = belongingId,
                FromSurvivorId = fromSurvivorId,
                ToSurvivorId = toSurvivorId,
                TransferType = BelongingTransferType.Gift,
                TransferDay = currentDay,
                Reason = reason,
                SentimentalEffect = moraleBonus
            };
            _state.Transfers.Add(transfer);

            var ev = new BelongingEvent
            {
                EventId = $"bev_{_state.NextSequence++}",
                EventType = "item_gifted",
                SurvivorId = toSurvivorId,
                BelongingId = item.BelongingId,
                Day = currentDay,
                Description = $"{fromSurvivorId} gifted {item.ItemName} to {toSurvivorId}.",
                MoraleEffect = moraleBonus
            };
            _state.Events.Add(ev);

            OnBelongingGifted?.Invoke(transfer);
            return transfer;
        }

        public List<PersonalBelonging> DistributeInheritanceOnDeath(string deceasedSurvivorId, string primaryHeirId, int currentDay)
        {
            var deceasedBelongings = _state.Belongings
                .Where(b => string.Equals(b.OwnerSurvivorId, deceasedSurvivorId, StringComparison.OrdinalIgnoreCase))
                .ToList();

            var inherited = new List<PersonalBelonging>();

            foreach (var item in deceasedBelongings)
            {
                item.OwnerSurvivorId = primaryHeirId;
                item.IsInherited = true;
                item.IsFavorite = false;
                item.SentimentalValue = Math.Clamp(item.SentimentalValue + 10f, 0f, 100f);

                var transfer = new BelongingTransfer
                {
                    TransferId = $"tr_{_state.NextSequence++}",
                    BelongingId = item.BelongingId,
                    FromSurvivorId = deceasedSurvivorId,
                    ToSurvivorId = primaryHeirId,
                    TransferType = BelongingTransferType.Inheritance,
                    TransferDay = currentDay,
                    Reason = $"Inherited from deceased dweller {deceasedSurvivorId}",
                    SentimentalEffect = 5.0f
                };
                _state.Transfers.Add(transfer);

                OnBelongingInherited?.Invoke(item, deceasedSurvivorId);
                inherited.Add(item);
            }

            return inherited;
        }

        public bool ReportTheftOrLoss(string survivorId, string belongingId, int currentDay, bool isStolen)
        {
            var item = GetBelonging(belongingId);
            if (item == null || !string.Equals(item.OwnerSurvivorId, survivorId, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            _state.Belongings.Remove(item);

            float penalty = -(item.SentimentalValue * (item.IsFavorite ? 0.25f : 0.12f));

            var ev = new BelongingEvent
            {
                EventId = $"bev_{_state.NextSequence++}",
                EventType = isStolen ? "item_stolen" : "item_lost",
                SurvivorId = survivorId,
                BelongingId = belongingId,
                Day = currentDay,
                Description = isStolen ? $"{item.ItemName} was stolen from {survivorId}!" : $"{survivorId} lost {item.ItemName}.",
                MoraleEffect = penalty
            };
            _state.Events.Add(ev);

            OnBelongingLost?.Invoke(ev);
            return true;
        }

        public void TickDay(int currentDay)
        {
            for (int i = 0; i < _state.Belongings.Count; i++)
            {
                var item = _state.Belongings[i];
                // Gradual condition degradation
                item.Condition = Math.Clamp(item.Condition - 0.15f, 0f, 100f);

                // Sentimental attachment grows if condition is still preserved
                if (item.Condition > 25f && item.SentimentalValue < 100f)
                {
                    item.SentimentalValue = Math.Clamp(item.SentimentalValue + 0.1f, 0f, 100f);
                }
            }
        }

        public float CalculateMoraleBuffer(string survivorId)
        {
            var items = GetBelongingsForSurvivor(survivorId);
            if (items.Count == 0) return 0f;

            float total = 0f;
            foreach (var item in items)
            {
                float weight = item.IsFavorite ? 2.0f : 1.0f;
                float conditionRatio = item.Condition / 100f;
                total += (item.SentimentalValue * 0.05f * weight * conditionRatio);
            }

            return MathF.Round(Math.Clamp(total, 0f, 25f), 1);
        }

        public PersonalBelongingsState CaptureState()
        {
            var captured = new PersonalBelongingsState
            {
                SchemaVersion = _state.SchemaVersion,
                NextSequence = _state.NextSequence,
                MaxBelongingsPerSurvivor = _state.MaxBelongingsPerSurvivor,
                Belongings = new List<PersonalBelonging>(_state.Belongings.Count),
                Transfers = new List<BelongingTransfer>(_state.Transfers.Count),
                Events = new List<BelongingEvent>(_state.Events.Count)
            };

            foreach (var b in _state.Belongings)
            {
                captured.Belongings.Add(new PersonalBelonging
                {
                    BelongingId = b.BelongingId,
                    OwnerSurvivorId = b.OwnerSurvivorId,
                    ItemId = b.ItemId,
                    ItemName = b.ItemName,
                    Category = b.Category,
                    SentimentalValue = b.SentimentalValue,
                    Condition = b.Condition,
                    AcquiredDay = b.AcquiredDay,
                    AcquiredFrom = b.AcquiredFrom,
                    Description = b.Description,
                    IsFavorite = b.IsFavorite,
                    IsInherited = b.IsInherited
                });
            }

            foreach (var t in _state.Transfers)
            {
                captured.Transfers.Add(new BelongingTransfer
                {
                    TransferId = t.TransferId,
                    BelongingId = t.BelongingId,
                    FromSurvivorId = t.FromSurvivorId,
                    ToSurvivorId = t.ToSurvivorId,
                    TransferType = t.TransferType,
                    TransferDay = t.TransferDay,
                    Reason = t.Reason,
                    SentimentalEffect = t.SentimentalEffect
                });
            }

            foreach (var e in _state.Events)
            {
                captured.Events.Add(new BelongingEvent
                {
                    EventId = e.EventId,
                    EventType = e.EventType,
                    SurvivorId = e.SurvivorId,
                    BelongingId = e.BelongingId,
                    Day = e.Day,
                    Description = e.Description,
                    MoraleEffect = e.MoraleEffect
                });
            }

            return captured;
        }

        public void RestoreState(PersonalBelongingsState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            _state.SchemaVersion = state.SchemaVersion;
            _state.NextSequence = state.NextSequence;
            _state.MaxBelongingsPerSurvivor = state.MaxBelongingsPerSurvivor;
            _state.Belongings.Clear();
            _state.Transfers.Clear();
            _state.Events.Clear();

            if (state.Belongings != null)
            {
                foreach (var b in state.Belongings)
                {
                    _state.Belongings.Add(new PersonalBelonging
                    {
                        BelongingId = b.BelongingId,
                        OwnerSurvivorId = b.OwnerSurvivorId,
                        ItemId = b.ItemId,
                        ItemName = b.ItemName,
                        Category = b.Category,
                        SentimentalValue = b.SentimentalValue,
                        Condition = b.Condition,
                        AcquiredDay = b.AcquiredDay,
                        AcquiredFrom = b.AcquiredFrom,
                        Description = b.Description,
                        IsFavorite = b.IsFavorite,
                        IsInherited = b.IsInherited
                    });
                }
            }

            if (state.Transfers != null)
            {
                foreach (var t in state.Transfers)
                {
                    _state.Transfers.Add(new BelongingTransfer
                    {
                        TransferId = t.TransferId,
                        BelongingId = t.BelongingId,
                        FromSurvivorId = t.FromSurvivorId,
                        ToSurvivorId = t.ToSurvivorId,
                        TransferType = t.TransferType,
                        TransferDay = t.TransferDay,
                        Reason = t.Reason,
                        SentimentalEffect = t.SentimentalEffect
                    });
                }
            }

            if (state.Events != null)
            {
                foreach (var e in state.Events)
                {
                    _state.Events.Add(new BelongingEvent
                    {
                        EventId = e.EventId,
                        EventType = e.EventType,
                        SurvivorId = e.SurvivorId,
                        BelongingId = e.BelongingId,
                        Day = e.Day,
                        Description = e.Description,
                        MoraleEffect = e.MoraleEffect
                    });
                }
            }
        }
    }
}
