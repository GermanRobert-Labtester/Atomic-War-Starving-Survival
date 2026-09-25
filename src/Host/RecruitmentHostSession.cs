// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : RecruitmentHostSession
// Core System  : Ashfall.Core.Survivors.RecruitmentSystem
// Host Caller  : Main.Recruitment
// Purpose      : Plan 204 — thin host lifecycle, catalog loading, candidate
//                discovery, defection offer delivery, and roster admission adapter.
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public sealed class RecruitmentHostSession : HostSessionBase
    {
        private readonly RecruitmentSystem _system;
        private readonly Action<RecruitmentCandidateRecord>? _onCandidateAdmittedToRoster;

        public RecruitmentSystem System => _system;
        public string? CatalogLoadError { get; private set; }
        public bool CatalogReady { get; private set; }

        public RecruitmentHostSession(
            RecruitmentSystem system,
            Action<RecruitmentCandidateRecord>? onCandidateAdmittedToRoster = null)
        {
            _system = system ?? throw new ArgumentNullException(nameof(system));
            _onCandidateAdmittedToRoster = onCandidateAdmittedToRoster;
        }

        public bool LoadCatalog(string dataDir)
        {
            if (string.IsNullOrWhiteSpace(dataDir))
            {
                CatalogLoadError = "Data directory path is empty.";
                CatalogReady = false;
                return false;
            }

            string catalogPath = Path.Combine(dataDir, "recruitment_templates.json");
            if (!File.Exists(catalogPath))
            {
                CatalogLoadError = $"Catalog file not found: {catalogPath}";
                CatalogReady = false;
                return false;
            }

            try
            {
                string json = File.ReadAllText(catalogPath);
                _system.LoadCatalog(json);
                CatalogLoadError = null;
                CatalogReady = true;
                return true;
            }
            catch (Exception ex)
            {
                CatalogLoadError = ex.Message;
                CatalogReady = false;
                return false;
            }
        }

        public (bool Success, string Message, RecruitmentCampaignRecord? Campaign) StartCampaign(
            string campaignTypeId, string recruiterId, string targetFaction, int day, int recruiterSkill = 50)
        {
            return _system.StartCampaign(campaignTypeId, recruiterId, targetFaction, day, recruiterSkill);
        }

        public (bool Success, string Message, DefectionOfferRecord? Offer) MakeDefectionOffer(
            string candidateId, string offerType, float offerValue, int day, int diplomacySkill = 50)
        {
            return _system.MakeDefectionOffer(candidateId, offerType, offerValue, day, diplomacySkill);
        }

        public RecruitmentCandidateRecord DiscoverCandidate(
            string candidateTemplateId, string locationId, int day, string currentFaction = "")
        {
            return _system.DiscoverCandidate(candidateTemplateId, locationId, day, currentFaction);
        }

        public bool AdmitCandidate(string candidateId, out RecruitmentCandidateRecord? candidate)
        {
            bool success = _system.TryAdmitCandidate(candidateId, out candidate);
            if (success && candidate != null)
            {
                _onCandidateAdmittedToRoster?.Invoke(candidate);
            }
            return success;
        }

        public void TickDay(int day)
        {
            _system.TickDay(day);
        }

        public RecruitmentCensus GetCensus() => _system.GetCensus();

        public IReadOnlyList<RecruitmentCampaignRecord> GetActiveCampaigns() => _system.GetActiveCampaigns();
        public IReadOnlyList<RecruitmentCandidateRecord> GetKnownCandidates() => _system.GetKnownCandidates();
        public IReadOnlyList<DefectionOfferRecord> GetDefectionOffers() => _system.GetDefectionOffers();

        public RecruitmentState CaptureState() => _system.CaptureState();
        public void RestoreState(RecruitmentState? state) => _system.RestoreState(state);
        public void Clear() => _system.Clear();
    }
}
