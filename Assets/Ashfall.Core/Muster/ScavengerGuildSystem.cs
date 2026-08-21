using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Muster
{
    /// <summary>Serialized state of the Scavenger Guild (Section V.5) — Brannick Sten's
    /// two-color claim ledger at loc_scavenger_guildhall.</summary>
    public class ScavengerGuildState
    {
        public string systemId = ScavengerGuildSystem.SystemId;
        public bool isActive;
        public List<string> claimedSiteIds = new List<string>();
        public HashSet<string> blacklistedShelterIds = new HashSet<string>(StringComparer.Ordinal);
        public float trust;

        public const int ApprenticeYieldCap = 5; // sites the player may strip clean per season
    }

    /// <summary>
    /// Engine-agnostic state machine for faction_scavenger_guild (Section V.5).
    /// ClaimSite marks a location's claim; RecordOverStrip blacklists a shelter
    /// permanently — no removal method, ever, per access_rule (both a mechanic
    /// and a narrative position). Freelance carries that one-way risk.
    /// </summary>
    public class ScavengerGuildSystem
    {
        public const string SystemId = "scavenger_guild_system";

        private readonly ScavengerGuildState _state;

        public event Action<ScavengerGuildState> OnStateChanged;
        public event Action<string> OnClaimed;
        public event Action<string, string> OnBlacklisted;

        public ScavengerGuildSystem(ScavengerGuildState state = null!)
        {
            _state = state ?? new ScavengerGuildState();
            if (_state.systemId != SystemId) _state.systemId = SystemId;
            if (_state.claimedSiteIds == null) _state.claimedSiteIds = new List<string>();
            if (_state.blacklistedShelterIds == null)
                _state.blacklistedShelterIds = new HashSet<string>(StringComparer.Ordinal);
        }

        public ScavengerGuildState State => _state;
        public float Trust => _state.trust;
        public bool IsBlacklisted(string shelterId) =>
            !string.IsNullOrEmpty(shelterId) && _state.blacklistedShelterIds.Contains(shelterId);
        public bool IsClaimed(string locationId) => _state.claimedSiteIds.Contains(locationId);

        /// <summary>Formally claim a site (Apprentice/approved routing).</summary>
        public bool ClaimSite(string locationId)
        {
            if (string.IsNullOrEmpty(locationId)) return false;
            if (_state.claimedSiteIds.Contains(locationId)) return false;
            _state.claimedSiteIds.Add(locationId);
            _state.trust = System.Math.Max(0f, _state.trust + 1f);
            OnClaimed?.Invoke(locationId);
            RaiseChanged();
            return true;
        }

        /// <summary>An over-stripped claimed site permanently blacklists the shelter.
        /// The ledger never crosses a name out.</summary>
        public bool RecordOverStrip(string shelterId, string locationId)
        {
            if (string.IsNullOrEmpty(shelterId)) return false;
            if (!IsClaimed(locationId)) return false; // only claimed sites can be over-stripped
            bool added = _state.blacklistedShelterIds.Add(shelterId);
            if (added)
            {
                _state.trust = System.Math.Max(0f, _state.trust - 4f);
                OnBlacklisted?.Invoke(shelterId, locationId);
                RaiseChanged();
            }
            return added;
        }

        public bool ApprenticeOverstripCheck(string shelterId, string locationId)
        {
            // Apprentices respect the yield cap; the check still applies if they break their word.
            return RecordOverStrip(shelterId, locationId);
        }

        public void Activate() { _state.isActive = true; RaiseChanged(); }

        // ── Save / Load ────────────────────────────────────────────────

        public ScavengerGuildState CaptureState()
        {
            var copy = new ScavengerGuildState
            {
                systemId = _state.systemId,
                isActive = _state.isActive,
                trust = _state.trust
            };
            var sites = new List<string>(_state.claimedSiteIds);
            sites.Sort(StringComparer.Ordinal);
            copy.claimedSiteIds = sites;
            var black = new List<string>(_state.blacklistedShelterIds);
            black.Sort(StringComparer.Ordinal);
            copy.blacklistedShelterIds = new HashSet<string>(black, StringComparer.Ordinal);
            return copy;
        }

        public void RestoreState(ScavengerGuildState saved)
        {
            if (saved == null) return;
            _state.systemId = SystemId;
            _state.isActive = saved.isActive;
            _state.trust = Math.Max(0f, saved.trust);
            _state.claimedSiteIds.Clear();
            if (saved.claimedSiteIds != null) _state.claimedSiteIds.AddRange(saved.claimedSiteIds);
            _state.blacklistedShelterIds.Clear();
            if (saved.blacklistedShelterIds != null)
                foreach (var s in saved.blacklistedShelterIds)
                    if (!string.IsNullOrEmpty(s)) _state.blacklistedShelterIds.Add(s);
            RaiseChanged();
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }
}
