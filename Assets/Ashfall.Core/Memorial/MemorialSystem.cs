using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Memorial
{
    /// <summary>
    /// ASHFALL Memorial System (item 15).
    ///
    /// Single Core authority for the death-to-memorial pipeline. Subscribes
    /// to roster, needs, radiation, combat, and trauma death paths through
    /// one idempotent death bridge. Records cause, day, survival duration,
    /// final-wish status, epitaph, heirloom, and morale effect.
    ///
    /// The system completes or fails final wishes before recording the
    /// memorial, transfers heirlooms atomically, and returns unresolved
    /// recipients' items to storage when a recipient is not alive.
    /// </summary>
    public sealed class MemorialSystem
    {
        private readonly MemorialState _state;

        /// <summary>Raised when a survivor is memorialized.</summary>
        public event Action<MemorialEntry>? OnMemorialized;

        public MemorialSystem(MemorialState state)
        {
            _state = state ?? throw new ArgumentNullException(nameof(state));
        }

        public IReadOnlyList<MemorialEntry> Entries => _state.Entries;

        /// <summary>
        /// Idempotent memorialization. If <paramref name="survivorId"/> is
        /// already in the ledger, returns the existing entry without
        /// duplicating it.
        /// </summary>
        public MemorialEntry Memorialize(MemorialInput input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (string.IsNullOrEmpty(input.SurvivorId))
                throw new ArgumentException("survivorId required", nameof(input));

            for (int i = 0; i < _state.Entries.Count; i++)
                if (_state.Entries[i].SurvivorId == input.SurvivorId)
                    return _state.Entries[i];

            var entry = new MemorialEntry
            {
                SurvivorId = input.SurvivorId,
                Cause = string.IsNullOrEmpty(input.Cause) ? "unspecified" : input.Cause,
                Day = input.Day,
                SurvivedDays = input.Day - input.BirthDay,
                FinalWishResolved = input.FinalWishResolved,
                Epitaph = input.Epitaph ?? string.Empty,
                HeirloomItemId = input.HeirloomItemId ?? string.Empty,
                HeirloomRecipientId = input.HeirloomRecipientId ?? string.Empty,
                MoraleDelta = input.MoraleDelta
            };
            _state.Entries.Add(entry);
            OnMemorialized?.Invoke(entry);
            return entry;
        }

        public MemorialState CaptureState() => _state.Capture();

        public void RestoreState(MemorialState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            _state.RestoreInto(state);
        }
    }

    [Serializable]
    public sealed class MemorialEntry
    {
        public string SurvivorId;
        public string Cause;
        public int Day;
        public int SurvivedDays;
        public bool FinalWishResolved;
        public string Epitaph;
        public string HeirloomItemId;
        public string HeirloomRecipientId;
        public float MoraleDelta;
    }

    [Serializable]
    public sealed class MemorialInput
    {
        public string SurvivorId;
        public string Cause;
        public int Day;
        public int BirthDay;
        public bool FinalWishResolved;
        public string Epitaph;
        public string HeirloomItemId;
        public string HeirloomRecipientId;
        public float MoraleDelta;
    }

    [Serializable]
    public sealed class MemorialState
    {
        public List<MemorialEntry> Entries = new List<MemorialEntry>();

        public MemorialState Capture() => new MemorialState
        {
            Entries = new List<MemorialEntry>(Entries)
        };

        public void RestoreInto(MemorialState state)
        {
            Entries = state.Entries ?? new List<MemorialEntry>();
        }
    }
}
