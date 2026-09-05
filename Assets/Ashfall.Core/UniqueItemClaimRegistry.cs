using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    /// <summary>
    /// Persisted DTO for <see cref="UniqueItemClaimRegistry"/>. Ordinal-sorted
    /// so serialization is deterministic and checksum-stable.
    /// </summary>
    [Serializable]
    public sealed class UniqueClaimSave
    {
        public int schema_version = 1;
        public string[] claimed_unique_ids = Array.Empty<string>();
    }

    /// <summary>
    /// Campaign-level registry of globally unique item ids that have already
    /// entered the campaign economy. Once claimed, a unique id can never be
    /// GENERATED again — by scavenging, merchant restock, procedural loot, or
    /// scripted grants — regardless of whether the claimed copy is still owned,
    /// was sold, or was lost. Selling does NOT unclaim.
    ///
    /// Deliberately separate from <see cref="CollectibleDiscoveryState"/>:
    /// uniqueness gates GENERATION; discovery gates the one-time EFFECT.
    ///
    /// The set of unique ids is supplied at construction (today: the
    /// <c>unique</c> flags on collectibles.json; tomorrow: any provider). The
    /// claim set itself only ever contains known unique ids, so claiming is a
    /// no-op for ordinary items and generation filtering is O(1) per entry.
    /// Instances are campaign state — never a global/static set.
    /// </summary>
    public sealed class UniqueItemClaimRegistry
    {
        private readonly HashSet<string> _uniqueIds;
        private readonly HashSet<string> _claimedIds =
            new HashSet<string>(StringComparer.Ordinal);

        public UniqueItemClaimRegistry(IEnumerable<string>? uniqueItemIds)
        {
            _uniqueIds = new HashSet<string>(StringComparer.Ordinal);
            if (uniqueItemIds != null)
            {
                foreach (string id in uniqueItemIds)
                {
                    if (!string.IsNullOrEmpty(id))
                        _uniqueIds.Add(id);
                }
            }
        }

        public int UniqueCount => _uniqueIds.Count;
        public int ClaimedCount => _claimedIds.Count;

        public bool IsUniqueItem(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return false;
            return _uniqueIds.Contains(itemId);
        }

        public bool IsClaimed(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return false;
            return _claimedIds.Contains(itemId);
        }

        /// <summary>
        /// Generation-eligibility query shared by every loot channel:
        /// ordinary items are always available; unique items are available
        /// until first claim. O(1).
        /// </summary>
        public bool IsAvailable(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return true;
            if (!_uniqueIds.Contains(itemId)) return true;
            return !_claimedIds.Contains(itemId);
        }

        /// <summary>
        /// Claim a unique item at the moment generation/award commits it into
        /// the campaign economy. Idempotent: returns true when the item ends
        /// the call claimed — first claim or already claimed — and false for
        /// unknown/non-unique/empty ids so callers can treat it as an
        /// unconditional no-op for ordinary loot.
        /// </summary>
        public bool TryClaim(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return false;
            if (!_uniqueIds.Contains(itemId)) return false;
            return _claimedIds.Contains(itemId) || _claimedIds.Add(itemId);
        }

        /// <summary>Ordinal-sorted snapshot; checksum-stable.</summary>
        public UniqueClaimSave CaptureState()
        {
            var ids = new string[_claimedIds.Count];
            _claimedIds.CopyTo(ids, 0);
            Array.Sort(ids, StringComparer.Ordinal);
            return new UniqueClaimSave { claimed_unique_ids = ids };
        }

        /// <summary>
        /// Restore clears current claims, then loads the persisted ids.
        /// Duplicates are harmless; ids that are no longer unique under the
        /// current catalog are dropped so a stale save cannot suppress an
        /// ordinary item forever.
        /// </summary>
        public void RestoreState(UniqueClaimSave? save)
        {
            _claimedIds.Clear();
            if (save == null || save.claimed_unique_ids == null) return;
            for (int i = 0; i < save.claimed_unique_ids.Length; i++)
            {
                string id = save.claimed_unique_ids[i];
                if (!string.IsNullOrEmpty(id) && _uniqueIds.Contains(id))
                    _claimedIds.Add(id);
            }
        }
    }
}
