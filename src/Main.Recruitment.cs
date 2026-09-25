// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 204 — Survivor recruitment & defection host composition.
// The Core RecruitmentSystem remains the sole recruitment campaign authority.
// This partial composes its catalog, save section, canonical campaign day,
// and presentation surfaces.
// ============================================================================

using System;
using Godot;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private RecruitmentHostSession? _recruitment;
        private bool _recruitmentDirty;
        private UI.RecruitmentPanel? _recruitmentPanel;

        public RecruitmentHostSession? Recruitment => _recruitment;

        public RecruitmentHostSession EnsureRecruitmentSession()
        {
            SetupRecruitment();
            return _recruitment!;
        }

        public void SetupRecruitment()
        {
            if (_recruitment != null) return;

            var system = new RecruitmentSystem();
            var saved = RecruitmentSaveStore.TryLoad();
            if (saved != null)
            {
                try
                {
                    system.RestoreState(saved);
                }
                catch (Exception ex)
                {
                    GD.PrintErr("[Recruitment] Failed to restore saved state: " + ex.Message);
                    system = new RecruitmentSystem();
                }
            }

            var session = new RecruitmentHostSession(
                system,
                onCandidateAdmittedToRoster: candidate =>
                {
                    _recruitmentDirty = true;
                });

            session.LoadCatalog(CatalogPath.ResolveDataDir());
            _recruitment = session;
            GD.Print("[Recruitment] Host session ready; catalog=" + (session.CatalogReady ? "ready" : "unavailable") + ".");
        }

        public void SaveRecruitment()
        {
            if (_recruitment == null) return;
            RecruitmentSaveStore.TrySave(_recruitment.CaptureState());
            _recruitmentDirty = false;
        }

        public void FlushRecruitment()
        {
            if (_recruitmentDirty)
            {
                SaveRecruitment();
            }
        }

        public void ResetRecruitment()
        {
            if (_recruitmentPanel != null && _recruitmentPanel.IsInsideTree())
            {
                RemoveChild(_recruitmentPanel);
            }
            _recruitmentPanel = null;
            _recruitment?.Clear();
            _recruitment = null;
            _recruitmentDirty = false;
        }

        public void OpenRecruitmentPanel()
        {
            EnsureRecruitmentSession();
            if (_recruitmentPanel == null)
            {
                _recruitmentPanel = new UI.RecruitmentPanel();
                _recruitmentPanel.OnClose += () => _recruitmentPanel.Visible = false;
                _recruitmentPanel.OnActionRequested += (action, param) =>
                {
                    switch (action)
                    {
                        case "START_CAMPAIGN":
                            StartRecruitmentCampaign(param, "shelter_leader", "neutral");
                            _recruitmentPanel.RefreshView();
                            break;
                        case "MAKE_OFFER":
                            MakeRecruitmentDefectionOffer(param, "rations_safety", 50f);
                            _recruitmentPanel.RefreshView();
                            break;
                        case "ADMIT_CANDIDATE":
                            AdmitRecruitmentCandidate(param, out _);
                            _recruitmentPanel.RefreshView();
                            break;
                    }
                };
                AddChild(_recruitmentPanel);
            }
            _recruitmentPanel.Bind(_recruitment!);
            ShowPanelLifecycle(_recruitmentPanel);
            _recruitmentPanel.Open();
        }

        public (bool Success, string Message, RecruitmentCampaignRecord? Campaign) StartRecruitmentCampaign(
            string campaignTypeId, string recruiterId, string targetFaction, int recruiterSkill = 50)
        {
            EnsureRecruitmentSession();
            int day = _campaignDay?.Calendar.CurrentDay ?? _simDay;
            var result = _recruitment!.StartCampaign(campaignTypeId, recruiterId, targetFaction, day, recruiterSkill);
            if (result.Success)
            {
                _recruitmentDirty = true;
            }
            return result;
        }

        public (bool Success, string Message, DefectionOfferRecord? Offer) MakeRecruitmentDefectionOffer(
            string candidateId, string offerType, float offerValue, int diplomacySkill = 50)
        {
            EnsureRecruitmentSession();
            int day = _campaignDay?.Calendar.CurrentDay ?? _simDay;
            var result = _recruitment!.MakeDefectionOffer(candidateId, offerType, offerValue, day, diplomacySkill);
            if (result.Success)
            {
                _recruitmentDirty = true;
            }
            return result;
        }

        public RecruitmentCandidateRecord? DiscoverRecruitmentCandidate(
            string candidateTemplateId, string locationId, string currentFaction = "")
        {
            EnsureRecruitmentSession();
            int day = _campaignDay?.Calendar.CurrentDay ?? _simDay;
            var candidate = _recruitment!.DiscoverCandidate(candidateTemplateId, locationId, day, currentFaction);
            _recruitmentDirty = true;
            return candidate;
        }

        public bool AdmitRecruitmentCandidate(string candidateId, out RecruitmentCandidateRecord? candidate)
        {
            EnsureRecruitmentSession();
            bool result = _recruitment!.AdmitCandidate(candidateId, out candidate);
            if (result)
            {
                _recruitmentDirty = true;
            }
            return result;
        }

        public RecruitmentCensus GetRecruitmentCensus()
        {
            EnsureRecruitmentSession();
            return _recruitment!.GetCensus();
        }
    }
}
