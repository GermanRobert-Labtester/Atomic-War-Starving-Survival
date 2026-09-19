// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Survivors
{
    public enum DeathCause
    {
        Starvation = 0,
        Dehydration = 1,
        Radiation = 2,
        Combat = 3,
        Disease = 4,
        OldAge = 5,
        Accident = 6,
        Murder = 7,
        Suicide = 8,
        Execution = 9,
        Unknown = 10
    }

    public enum InheritanceCategory
    {
        All = 0,
        Weapons = 1,
        Clothing = 2,
        Medical = 3,
        Food = 4,
        Tools = 5,
        Valuables = 6
    }

    public enum DisputeResolution
    {
        Pending = 0,
        Mediated = 1,
        Upheld = 2,
        Overturned = 3,
        Dropped = 4
    }

    [Serializable]
    public sealed class DeathRecord
    {
        public string RecordId { get; set; } = string.Empty;
        public string SurvivorId { get; set; } = string.Empty;
        public string SurvivorName { get; set; } = string.Empty;
        public int DeathDay { get; set; } = 1;
        public DeathCause Cause { get; set; } = DeathCause.Unknown;
        public string LocationAtDeath { get; set; } = string.Empty;
        public string LastWords { get; set; } = string.Empty;
        public List<string> Witnesses { get; set; } = new List<string>();
        public string Circumstances { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class BeneficiaryEntry
    {
        public string BeneficiaryId { get; set; } = string.Empty;
        public InheritanceCategory Category { get; set; } = InheritanceCategory.All;
        public float Percentage { get; set; } = 100f;
    }

    [Serializable]
    public sealed class SpecialBequest
    {
        public string ItemId { get; set; } = string.Empty;
        public string RecipientId { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class LastWill
    {
        public string WillId { get; set; } = string.Empty;
        public string SurvivorId { get; set; } = string.Empty;
        public int CreatedDay { get; set; } = 1;
        public List<BeneficiaryEntry> Beneficiaries { get; set; } = new List<BeneficiaryEntry>();
        public List<SpecialBequest> SpecialBequests { get; set; } = new List<SpecialBequest>();
        public string ResiduaryBeneficiary { get; set; } = string.Empty;
        public List<string> Witnesses { get; set; } = new List<string>();
        public bool IsValid { get; set; } = true;
    }

    [Serializable]
    public sealed class InheritedItem
    {
        public string ItemId { get; set; } = string.Empty;
        public string OriginalOwnerId { get; set; } = string.Empty;
        public string RecipientId { get; set; } = string.Empty;
        public float SentimentalValue { get; set; } = 50f;
    }

    [Serializable]
    public sealed class InheritanceDispute
    {
        public string DisputeId { get; set; } = string.Empty;
        public string WillId { get; set; } = string.Empty;
        public string DisputantId { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public DisputeResolution Resolution { get; set; } = DisputeResolution.Pending;
    }

    [Serializable]
    public sealed class SurvivorDeathLegacyState
    {
        public int SchemaVersion { get; set; } = 1;
        public int NextSequence { get; set; } = 1;
        public List<DeathRecord> DeathRecords { get; set; } = new List<DeathRecord>();
        public List<LastWill> Wills { get; set; } = new List<LastWill>();
        public List<InheritedItem> InheritedItems { get; set; } = new List<InheritedItem>();
        public List<InheritanceDispute> Disputes { get; set; } = new List<InheritanceDispute>();
    }

    /// <summary>
    /// Plan 206 — Survivor Death, Legacy & Inheritance System.
    /// Tracks survivor cause of death, last wills/testaments, inheritance distribution
    /// to designated beneficiaries or commons, inheritance disputes, and memorial legacy records.
    /// </summary>
    public sealed class SurvivorDeathLegacySystem
    {
        private readonly SurvivorDeathLegacyState _state;

        public event Action<DeathRecord>? OnDeathRecorded;
        public event Action<LastWill>? OnWillCreated;
        public event Action<DeathRecord, IReadOnlyList<InheritedItem>>? OnInheritanceDistributed;
        public event Action<InheritanceDispute>? OnDisputeRaised;
        public event Action<InheritanceDispute>? OnDisputeResolved;

        public int DeathCount => _state.DeathRecords.Count;
        public int WillCount => _state.Wills.Count;
        public int ActiveDisputeCount => _state.Disputes.Count(d => d.Resolution == DisputeResolution.Pending);

        public SurvivorDeathLegacySystem(SurvivorDeathLegacyState? state = null)
        {
            _state = state ?? new SurvivorDeathLegacyState();
        }

        public LastWill CreateWill(
            string survivorId,
            IEnumerable<BeneficiaryEntry>? beneficiaries = null,
            IEnumerable<SpecialBequest>? specialBequests = null,
            string residuaryBeneficiary = "",
            IEnumerable<string>? witnesses = null,
            int currentDay = 1)
        {
            if (string.IsNullOrWhiteSpace(survivorId)) throw new ArgumentNullException(nameof(survivorId));

            // Invalidate older wills of this survivor
            foreach (var w in _state.Wills.Where(w => string.Equals(w.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase)))
            {
                w.IsValid = false;
            }

            var will = new LastWill
            {
                WillId = $"will_{_state.NextSequence++}",
                SurvivorId = survivorId.Trim(),
                CreatedDay = Math.Max(1, currentDay),
                Beneficiaries = beneficiaries?.ToList() ?? new List<BeneficiaryEntry>(),
                SpecialBequests = specialBequests?.ToList() ?? new List<SpecialBequest>(),
                ResiduaryBeneficiary = residuaryBeneficiary ?? string.Empty,
                Witnesses = witnesses?.ToList() ?? new List<string>(),
                IsValid = true
            };

            _state.Wills.Add(will);
            OnWillCreated?.Invoke(will);
            return will;
        }

        public DeathRecord RecordDeath(
            string survivorId,
            string survivorName,
            DeathCause cause,
            int deathDay,
            string location = "",
            string lastWords = "",
            IEnumerable<string>? witnesses = null,
            string circumstances = "")
        {
            if (string.IsNullOrWhiteSpace(survivorId)) throw new ArgumentNullException(nameof(survivorId));

            var record = new DeathRecord
            {
                RecordId = $"dth_{_state.NextSequence++}",
                SurvivorId = survivorId.Trim(),
                SurvivorName = string.IsNullOrWhiteSpace(survivorName) ? survivorId.Trim() : survivorName.Trim(),
                Cause = cause,
                DeathDay = Math.Max(1, deathDay),
                LocationAtDeath = location ?? string.Empty,
                LastWords = lastWords ?? string.Empty,
                Witnesses = witnesses?.ToList() ?? new List<string>(),
                Circumstances = circumstances ?? string.Empty
            };

            _state.DeathRecords.Add(record);
            OnDeathRecorded?.Invoke(record);
            return record;
        }

        public IReadOnlyList<InheritedItem> DistributeInheritance(string deathRecordId, IEnumerable<string> itemIds, string fallbackBeneficiary = "commons")
        {
            var record = _state.DeathRecords.FirstOrDefault(r => string.Equals(r.RecordId, deathRecordId, StringComparison.OrdinalIgnoreCase));
            if (record == null) return Array.Empty<InheritedItem>();

            var will = _state.Wills.FirstOrDefault(w => w.IsValid && string.Equals(w.SurvivorId, record.SurvivorId, StringComparison.OrdinalIgnoreCase));
            var distributed = new List<InheritedItem>();

            foreach (var itemId in itemIds)
            {
                string recipient = fallbackBeneficiary;

                if (will != null)
                {
                    // Check special bequest
                    var bequest = will.SpecialBequests.FirstOrDefault(b => string.Equals(b.ItemId, itemId, StringComparison.OrdinalIgnoreCase));
                    if (bequest != null && !string.IsNullOrWhiteSpace(bequest.RecipientId))
                    {
                        recipient = bequest.RecipientId;
                    }
                    else if (will.Beneficiaries.Count > 0)
                    {
                        // Default to first primary beneficiary
                        recipient = will.Beneficiaries[0].BeneficiaryId;
                    }
                    else if (!string.IsNullOrWhiteSpace(will.ResiduaryBeneficiary))
                    {
                        recipient = will.ResiduaryBeneficiary;
                    }
                }

                var item = new InheritedItem
                {
                    ItemId = itemId,
                    OriginalOwnerId = record.SurvivorId,
                    RecipientId = recipient,
                    SentimentalValue = 60f
                };

                _state.InheritedItems.Add(item);
                distributed.Add(item);
            }

            OnInheritanceDistributed?.Invoke(record, distributed);
            return distributed;
        }

        public InheritanceDispute RaiseDispute(string willId, string disputantId, string reason)
        {
            if (string.IsNullOrWhiteSpace(willId)) throw new ArgumentNullException(nameof(willId));
            if (string.IsNullOrWhiteSpace(disputantId)) throw new ArgumentNullException(nameof(disputantId));

            var dispute = new InheritanceDispute
            {
                DisputeId = $"disp_{_state.NextSequence++}",
                WillId = willId.Trim(),
                DisputantId = disputantId.Trim(),
                Reason = reason ?? string.Empty,
                Resolution = DisputeResolution.Pending
            };

            _state.Disputes.Add(dispute);
            OnDisputeRaised?.Invoke(dispute);
            return dispute;
        }

        public bool ResolveDispute(string disputeId, DisputeResolution resolution)
        {
            var dispute = _state.Disputes.FirstOrDefault(d => string.Equals(d.DisputeId, disputeId, StringComparison.OrdinalIgnoreCase));
            if (dispute == null || dispute.Resolution != DisputeResolution.Pending) return false;

            dispute.Resolution = resolution;
            OnDisputeResolved?.Invoke(dispute);
            return true;
        }

        public LastWill? GetActiveWill(string survivorId)
        {
            return _state.Wills.FirstOrDefault(w => w.IsValid && string.Equals(w.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase));
        }

        public DeathRecord? GetDeathRecord(string survivorId)
        {
            return _state.DeathRecords.FirstOrDefault(r => string.Equals(r.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase));
        }

        public SurvivorDeathLegacyState CaptureState()
        {
            var state = new SurvivorDeathLegacyState
            {
                SchemaVersion = _state.SchemaVersion,
                NextSequence = _state.NextSequence,
                DeathRecords = new List<DeathRecord>(_state.DeathRecords.Count),
                Wills = new List<LastWill>(_state.Wills.Count),
                InheritedItems = new List<InheritedItem>(_state.InheritedItems.Count),
                Disputes = new List<InheritanceDispute>(_state.Disputes.Count)
            };

            foreach (var d in _state.DeathRecords)
            {
                state.DeathRecords.Add(new DeathRecord
                {
                    RecordId = d.RecordId,
                    SurvivorId = d.SurvivorId,
                    SurvivorName = d.SurvivorName,
                    DeathDay = d.DeathDay,
                    Cause = d.Cause,
                    LocationAtDeath = d.LocationAtDeath,
                    LastWords = d.LastWords,
                    Witnesses = new List<string>(d.Witnesses),
                    Circumstances = d.Circumstances
                });
            }

            foreach (var w in _state.Wills)
            {
                state.Wills.Add(new LastWill
                {
                    WillId = w.WillId,
                    SurvivorId = w.SurvivorId,
                    CreatedDay = w.CreatedDay,
                    Beneficiaries = w.Beneficiaries.Select(b => new BeneficiaryEntry
                    {
                        BeneficiaryId = b.BeneficiaryId,
                        Category = b.Category,
                        Percentage = b.Percentage
                    }).ToList(),
                    SpecialBequests = w.SpecialBequests.Select(s => new SpecialBequest
                    {
                        ItemId = s.ItemId,
                        RecipientId = s.RecipientId
                    }).ToList(),
                    ResiduaryBeneficiary = w.ResiduaryBeneficiary,
                    Witnesses = new List<string>(w.Witnesses),
                    IsValid = w.IsValid
                });
            }

            foreach (var i in _state.InheritedItems)
            {
                state.InheritedItems.Add(new InheritedItem
                {
                    ItemId = i.ItemId,
                    OriginalOwnerId = i.OriginalOwnerId,
                    RecipientId = i.RecipientId,
                    SentimentalValue = i.SentimentalValue
                });
            }

            foreach (var disp in _state.Disputes)
            {
                state.Disputes.Add(new InheritanceDispute
                {
                    DisputeId = disp.DisputeId,
                    WillId = disp.WillId,
                    DisputantId = disp.DisputantId,
                    Reason = disp.Reason,
                    Resolution = disp.Resolution
                });
            }

            return state;
        }

        public void RestoreState(SurvivorDeathLegacyState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            _state.SchemaVersion = state.SchemaVersion;
            _state.NextSequence = state.NextSequence;
            _state.DeathRecords.Clear();
            _state.Wills.Clear();
            _state.InheritedItems.Clear();
            _state.Disputes.Clear();

            if (state.DeathRecords != null)
            {
                foreach (var d in state.DeathRecords)
                {
                    _state.DeathRecords.Add(new DeathRecord
                    {
                        RecordId = d.RecordId,
                        SurvivorId = d.SurvivorId,
                        SurvivorName = d.SurvivorName,
                        DeathDay = d.DeathDay,
                        Cause = d.Cause,
                        LocationAtDeath = d.LocationAtDeath,
                        LastWords = d.LastWords,
                        Witnesses = new List<string>(d.Witnesses ?? Enumerable.Empty<string>()),
                        Circumstances = d.Circumstances
                    });
                }
            }

            if (state.Wills != null)
            {
                foreach (var w in state.Wills)
                {
                    _state.Wills.Add(new LastWill
                    {
                        WillId = w.WillId,
                        SurvivorId = w.SurvivorId,
                        CreatedDay = w.CreatedDay,
                        Beneficiaries = (w.Beneficiaries ?? Enumerable.Empty<BeneficiaryEntry>()).Select(b => new BeneficiaryEntry
                        {
                            BeneficiaryId = b.BeneficiaryId,
                            Category = b.Category,
                            Percentage = b.Percentage
                        }).ToList(),
                        SpecialBequests = (w.SpecialBequests ?? Enumerable.Empty<SpecialBequest>()).Select(s => new SpecialBequest
                        {
                            ItemId = s.ItemId,
                            RecipientId = s.RecipientId
                        }).ToList(),
                        ResiduaryBeneficiary = w.ResiduaryBeneficiary,
                        Witnesses = new List<string>(w.Witnesses ?? Enumerable.Empty<string>()),
                        IsValid = w.IsValid
                    });
                }
            }

            if (state.InheritedItems != null)
            {
                foreach (var i in state.InheritedItems)
                {
                    _state.InheritedItems.Add(new InheritedItem
                    {
                        ItemId = i.ItemId,
                        OriginalOwnerId = i.OriginalOwnerId,
                        RecipientId = i.RecipientId,
                        SentimentalValue = i.SentimentalValue
                    });
                }
            }

            if (state.Disputes != null)
            {
                foreach (var disp in state.Disputes)
                {
                    _state.Disputes.Add(new InheritanceDispute
                    {
                        DisputeId = disp.DisputeId,
                        WillId = disp.WillId,
                        DisputantId = disp.DisputantId,
                        Reason = disp.Reason,
                        Resolution = disp.Resolution
                    });
                }
            }
        }
    }
}
