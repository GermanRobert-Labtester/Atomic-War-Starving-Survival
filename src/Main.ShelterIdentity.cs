// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Plan 166 — Shelter Identity, Naming, Origin & Reputation Projection
// host wiring. The Core ShelterIdentitySystem is the authority for name,
// origin, motto, emblem, infamy, and the shelter's own community-action
// profile. Faction standing remains owned by FactionWarSystem; this host never
// writes a competing faction reputation.
// ============================================================================

using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private ShelterIdentityHostSession? _shelterIdentity;
        private bool _shelterIdentityDirty;
        private bool _shelterIdentityDayBridgeWired;

        public ShelterIdentityHostSession? ShelterIdentity => _shelterIdentity;

        public void SetupShelterIdentity()
        {
            if (_shelterIdentity != null) return;

            _shelterIdentity = ShelterIdentityHostSession.Create(_dataDir);

            var saved = ShelterIdentitySaveStore.TryLoad();
            if (saved != null)
                _shelterIdentity.RestoreState(saved);

            _shelterIdentity.StateChanged += () => _shelterIdentityDirty = true;
            WireShelterIdentityDayBridge();
        }

        private void WireShelterIdentityDayBridge()
        {
            if (_shelterIdentityDayBridgeWired || _campaignDay == null) return;
            _shelterIdentityDayBridgeWired = true;
            _campaignDay.OnDayAdvanced += OnCampaignDayAdvancedShelterIdentity;
        }

        /// <summary>
        /// Plan 166 — reflects canonical day facts into the shelter's own
        /// community-action profile. This never decides anything: it only records
        /// that the shelter did the thing the owning system already did.
        /// The trade/raid/isolation axes have no canonical emitted day kind yet,
        /// so they stay unbound rather than being fabricated here.
        /// </summary>
        private void OnCampaignDayAdvancedShelterIdentity(DayAdvancedEventArgs args)
        {
            if (_shelterIdentity == null || args?.OwnerReports == null) return;

            for (int r = 0; r < args.OwnerReports.Length; r++)
            {
                var report = args.OwnerReports[r];
                if (report?.Events == null) continue;

                for (int e = 0; e < report.Events.Length; e++)
                {
                    if (string.Equals(report.Events[e]?.Kind, "medical_admitted", StringComparison.Ordinal))
                        _shelterIdentity.RecordCommunityAction("medical", 1);
                }
            }
        }

        public void SaveShelterIdentity()
        {
            if (_shelterIdentity == null) return;
            var state = _shelterIdentity.CaptureState();
            ShelterIdentitySaveStore.TrySave(state);
            if (CaptureSection("shelter_identity", ShelterIdentitySaveStore.TryCapturePersisted(state)))
                _shelterIdentityDirty = false;
        }

        public void TickShelterIdentity(int day)
        {
            if (_shelterIdentity == null) SetupShelterIdentity();
            if (_shelterIdentity == null) return;

            // A fresh campaign has no origin until one is selected; pick the
            // deterministic default and record the founder.
            if (string.IsNullOrEmpty(_shelterIdentity.OriginId))
                SelectDefaultShelterOrigin(day);
        }

        /// <summary>
        /// Chooses the sheltered origin deterministically when the player has not
        /// selected one: the government bunker when authored, else the first
        /// origin in ordinal order. Records the ordinal-first roster survivor as
        /// founder.
        /// </summary>
        public void SelectDefaultShelterOrigin(int day)
        {
            if (_shelterIdentity == null) return;

            var origins = new List<ShelterOriginDef>(_shelterIdentity.System.Origins.Values);
            if (origins.Count == 0) return;

            origins.Sort((a, b) => string.CompareOrdinal(a.origin_id, b.origin_id));
            const string preferred = "origin_government_bunker";
            string chosen = origins.Exists(o => string.Equals(o.origin_id, preferred, StringComparison.OrdinalIgnoreCase))
                ? preferred
                : origins[0].origin_id;

            string founder = _survivors?.RosterState != null && _survivors.RosterState.Count > 0
                ? _survivors.RosterState[0].Id
                : string.Empty;

            _shelterIdentity.SelectOrigin(chosen, Math.Max(1, day), founder);
        }

        public bool SelectShelterOrigin(string originId)
        {
            if (string.IsNullOrWhiteSpace(originId)) return false;
            if (_shelterIdentity == null) SetupShelterIdentity();
            if (_shelterIdentity == null) return false;

            var result = _shelterIdentity.SelectOrigin(originId.Trim(), _simDay,
                _survivors?.RosterState != null && _survivors.RosterState.Count > 0 ? _survivors.RosterState[0].Id : string.Empty);
            return result.Status == ActionResult.StatusKind.Success;
        }

        public bool SetShelterName(string name)
        {
            if (_shelterIdentity == null) SetupShelterIdentity();
            if (_shelterIdentity == null) return false;
            return _shelterIdentity.SetShelterName(name).Status == ActionResult.StatusKind.Success;
        }

        public bool SetShelterMotto(string motto)
        {
            if (_shelterIdentity == null) SetupShelterIdentity();
            if (_shelterIdentity == null) return false;
            return _shelterIdentity.SetMotto(motto).Status == ActionResult.StatusKind.Success;
        }

        public bool SetShelterEmblem(string symbol, string color)
        {
            if (_shelterIdentity == null) SetupShelterIdentity();
            if (_shelterIdentity == null) return false;
            return _shelterIdentity.SetEmblem(symbol, color).Status == ActionResult.StatusKind.Success;
        }

        /// <summary>Plan 166 — records a shelter community action for the known-for projection.</summary>
        public void RecordShelterCommunityAction(string actionType, int magnitude = 1)
        {
            if (_shelterIdentity == null) SetupShelterIdentity();
            _shelterIdentity?.RecordCommunityAction(actionType, magnitude);
        }

        public IReadOnlyList<string> GetShelterKnownForTags() =>
            _shelterIdentity?.GetKnownForTags() ?? (IReadOnlyList<string>)Array.Empty<string>();

        public ShelterIdentityCensus GetShelterIdentityCensus() =>
            _shelterIdentity?.Census ?? default;

        public void FlushShelterIdentityIfDirty()
        {
            if (_shelterIdentityDirty)
                SaveShelterIdentity();
        }

        public void ResetShelterIdentity()
        {
            if (_shelterIdentityDayBridgeWired && _campaignDay != null)
                _campaignDay.OnDayAdvanced -= OnCampaignDayAdvancedShelterIdentity;
            _shelterIdentityDayBridgeWired = false;
            _shelterIdentity = null;
            _shelterIdentityDirty = false;
        }
    }
}
