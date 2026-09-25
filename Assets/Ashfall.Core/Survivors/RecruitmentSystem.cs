// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 204 — Survivor Recruitment & Defection System
// Pure domain authority for active recruitment campaigns, wilderness discovery,
// faction defection inducements, asylum processing, and shelter population growth.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Ashfall.Core.Survivors
{
    // ── Catalog DTOs ────────────────────────────────────────────────────────

    [Serializable]
    public sealed class RecruitmentCampaignDef
    {
        public string campaign_type_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string category { get; set; } = "exploration";
        public int base_duration_days { get; set; } = 5;
        public int base_success_chance { get; set; } = 50;
        public int cost_food { get; set; } = 15;
        public int cost_water { get; set; } = 15;
        public int cost_currency { get; set; } = 0;
        public string description { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class RecruitmentCandidateDef
    {
        public string candidate_template_id { get; set; } = string.Empty;
        public string candidate_type { get; set; } = "wilderness_survivor";
        public string display_name { get; set; } = string.Empty;
        public int base_willingness { get; set; } = 50;
        public string primary_skill { get; set; } = "scavenging";
        public string personality_trait { get; set; } = "resilient";
        public string description { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class RecruitmentTemplatesCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<RecruitmentCampaignDef> campaign_templates { get; set; } = new List<RecruitmentCampaignDef>();
        public List<RecruitmentCandidateDef> candidate_templates { get; set; } = new List<RecruitmentCandidateDef>();
    }

    // ── State DTOs ──────────────────────────────────────────────────────────

    [Serializable]
    public sealed class RecruitmentCampaignRecord
    {
        public string CampaignId { get; set; } = string.Empty;
        public string CampaignTypeId { get; set; } = string.Empty;
        public string TargetFaction { get; set; } = string.Empty;
        public string AssignedRecruiterId { get; set; } = string.Empty;
        public int DurationDays { get; set; } = 5;
        public int SuccessChance { get; set; } = 50;
        public string Status { get; set; } = "in_progress"; // in_progress, succeeded, failed, cancelled
        public int StartedDay { get; set; } = 1;
        public int CompletedDay { get; set; } = -1;
    }

    [Serializable]
    public sealed class RecruitmentCandidateRecord
    {
        public string CandidateId { get; set; } = string.Empty;
        public string CandidateTemplateId { get; set; } = string.Empty;
        public string CandidateType { get; set; } = "wilderness_survivor";
        public string CurrentFaction { get; set; } = string.Empty;
        public string LocationId { get; set; } = string.Empty;
        public int Willingness { get; set; } = 50;
        public int DiscoveredDay { get; set; } = 1;
        public bool IsRecruited { get; set; } = false;
    }

    [Serializable]
    public sealed class DefectionOfferRecord
    {
        public string OfferId { get; set; } = string.Empty;
        public string TargetCandidateId { get; set; } = string.Empty;
        public string OfferType { get; set; } = "asylum";
        public float OfferValue { get; set; } = 50f;
        public int SuccessChance { get; set; } = 50;
        public int DiscoveredRisk { get; set; } = 15;
        public string Status { get; set; } = "pending"; // pending, accepted, rejected, discovered
        public int Day { get; set; } = 1;
    }

    [Serializable]
    public sealed class RecruitmentEventRecord
    {
        public string EventId { get; set; } = string.Empty;
        public string EventType { get; set; } = "recruitment_success";
        public int Day { get; set; } = 1;
        public string Description { get; set; } = string.Empty;
        public string Outcome { get; set; } = "success";
    }

    [Serializable]
    public struct RecruitmentCensus
    {
        public int ActiveCampaigns { get; }
        public int KnownCandidates { get; }
        public int TotalRecruited { get; }
        public int PendingOffers { get; }
        public int TotalEvents { get; }

        public RecruitmentCensus(int activeCampaigns, int knownCandidates, int totalRecruited, int pendingOffers, int totalEvents)
        {
            ActiveCampaigns = activeCampaigns;
            KnownCandidates = knownCandidates;
            TotalRecruited = totalRecruited;
            PendingOffers = pendingOffers;
            TotalEvents = totalEvents;
        }
    }

    [Serializable]
    public sealed class RecruitmentState
    {
        public int SchemaVersion { get; set; } = 1;
        public int NextSequence { get; set; } = 1;
        public int MaxActiveCampaigns { get; set; } = 3;
        public List<RecruitmentCampaignRecord> Campaigns { get; set; } = new List<RecruitmentCampaignRecord>();
        public List<RecruitmentCandidateRecord> Candidates { get; set; } = new List<RecruitmentCandidateRecord>();
        public List<DefectionOfferRecord> Offers { get; set; } = new List<DefectionOfferRecord>();
        public List<RecruitmentEventRecord> Events { get; set; } = new List<RecruitmentEventRecord>();
    }

    // ── Domain System ───────────────────────────────────────────────────────

    public sealed class RecruitmentSystem
    {
        private RecruitmentState _state;
        private readonly Dictionary<string, RecruitmentCampaignDef> _campaignDefs =
            new Dictionary<string, RecruitmentCampaignDef>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, RecruitmentCandidateDef> _candidateDefs =
            new Dictionary<string, RecruitmentCandidateDef>(StringComparer.OrdinalIgnoreCase);

        public event Action<RecruitmentCampaignRecord>? OnCampaignStarted;
        public event Action<RecruitmentCampaignRecord>? OnCampaignCompleted;
        public event Action<RecruitmentCandidateRecord>? OnCandidateDiscovered;
        public event Action<DefectionOfferRecord>? OnDefectionResolved;
        public event Action<DefectionOfferRecord>? OnRecruiterDiscoveredAlert;

        public int ActiveCampaignCount => _state.Campaigns.Count(c => c.Status == "in_progress");
        public int KnownCandidateCount => _state.Candidates.Count(c => !c.IsRecruited);
        public int TotalRecruitedCount => _state.Candidates.Count(c => c.IsRecruited);

        public RecruitmentSystem()
        {
            _state = new RecruitmentState();
        }

        public RecruitmentCensus GetCensus()
        {
            int pendingOffers = _state.Offers.Count(o => o.Status == "pending");
            return new RecruitmentCensus(
                ActiveCampaignCount,
                KnownCandidateCount,
                TotalRecruitedCount,
                pendingOffers,
                _state.Events.Count
            );
        }

        public bool TryAdmitCandidate(string candidateId, out RecruitmentCandidateRecord? candidate)
        {
            candidate = _state.Candidates.FirstOrDefault(c => string.Equals(c.CandidateId, candidateId, StringComparison.OrdinalIgnoreCase));
            if (candidate == null || candidate.IsRecruited)
            {
                return false;
            }

            candidate.IsRecruited = true;
            _state.Events.Add(new RecruitmentEventRecord
            {
                EventId = $"recevt_{_state.NextSequence++}",
                EventType = "candidate_admitted",
                Day = candidate.DiscoveredDay,
                Description = $"Candidate {candidate.CandidateId} formally admitted to shelter roster.",
                Outcome = "admitted"
            });
            return true;
        }

        public void Clear()
        {
            _state = new RecruitmentState();
        }

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentException("Catalog JSON cannot be null or empty", nameof(json));

            var catalog = JsonSerializer.Deserialize<RecruitmentTemplatesCatalog>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (catalog == null) return;

            _campaignDefs.Clear();
            if (catalog.campaign_templates != null)
            {
                foreach (var camp in catalog.campaign_templates)
                {
                    if (!string.IsNullOrEmpty(camp.campaign_type_id))
                    {
                        _campaignDefs[camp.campaign_type_id] = camp;
                    }
                }
            }

            _candidateDefs.Clear();
            if (catalog.candidate_templates != null)
            {
                foreach (var cand in catalog.candidate_templates)
                {
                    if (!string.IsNullOrEmpty(cand.candidate_template_id))
                    {
                        _candidateDefs[cand.candidate_template_id] = cand;
                    }
                }
            }
        }

        public IReadOnlyList<RecruitmentCampaignDef> GetAllCampaignDefs() => _campaignDefs.Values.ToList();
        public IReadOnlyList<RecruitmentCandidateDef> GetAllCandidateDefs() => _candidateDefs.Values.ToList();

        public RecruitmentCampaignDef? GetCampaignDef(string id)
        {
            _campaignDefs.TryGetValue(id, out var def);
            return def;
        }

        public RecruitmentCandidateDef? GetCandidateDef(string id)
        {
            _candidateDefs.TryGetValue(id, out var def);
            return def;
        }

        public RecruitmentCandidateRecord DiscoverCandidate(
            string candidateTemplateId, string locationId, int day, string currentFaction = "")
        {
            _candidateDefs.TryGetValue(candidateTemplateId, out var def);
            int baseWillingness = def?.base_willingness ?? 50;
            string candType = def?.candidate_type ?? "wilderness_survivor";

            var candidate = new RecruitmentCandidateRecord
            {
                CandidateId = $"cand_{_state.NextSequence++}",
                CandidateTemplateId = candidateTemplateId,
                CandidateType = candType,
                CurrentFaction = currentFaction,
                LocationId = locationId,
                Willingness = baseWillingness,
                DiscoveredDay = day,
                IsRecruited = false
            };

            _state.Candidates.Add(candidate);
            OnCandidateDiscovered?.Invoke(candidate);
            return candidate;
        }

        public (bool Success, string Message, RecruitmentCampaignRecord? Campaign) StartCampaign(
            string campaignTypeId, string recruiterId, string targetFaction, int day, int recruiterSkill = 50)
        {
            if (!_campaignDefs.TryGetValue(campaignTypeId, out var def))
                return (false, $"Unknown campaign template '{campaignTypeId}'", null);

            if (ActiveCampaignCount >= _state.MaxActiveCampaigns)
                return (false, $"Maximum active campaigns ({_state.MaxActiveCampaigns}) reached", null);

            int calculatedSuccessChance = Math.Clamp(def.base_success_chance + (recruiterSkill / 4), 10, 95);

            var campaign = new RecruitmentCampaignRecord
            {
                CampaignId = $"camp_{_state.NextSequence++}",
                CampaignTypeId = campaignTypeId,
                TargetFaction = targetFaction,
                AssignedRecruiterId = recruiterId,
                DurationDays = def.base_duration_days,
                SuccessChance = calculatedSuccessChance,
                Status = "in_progress",
                StartedDay = day,
                CompletedDay = -1
            };

            _state.Campaigns.Add(campaign);
            OnCampaignStarted?.Invoke(campaign);
            return (true, "Recruitment campaign dispatched.", campaign);
        }

        public (bool Success, string Message, DefectionOfferRecord? Offer) MakeDefectionOffer(
            string candidateId, string offerType, float offerValue, int day, int diplomacySkill = 50)
        {
            var candidate = _state.Candidates.FirstOrDefault(c => string.Equals(c.CandidateId, candidateId, StringComparison.OrdinalIgnoreCase));
            if (candidate == null)
                return (false, $"Candidate '{candidateId}' not found", null);

            if (candidate.IsRecruited)
                return (false, "Candidate is already recruited", null);

            int successChance = Math.Clamp((int)(candidate.Willingness * 0.5f + offerValue * 0.3f + diplomacySkill * 0.2f), 10, 95);
            int discoveryRisk = Math.Clamp((int)(40 - diplomacySkill * 0.3f), 5, 60);

            var offer = new DefectionOfferRecord
            {
                OfferId = $"offer_{_state.NextSequence++}",
                TargetCandidateId = candidateId,
                OfferType = offerType,
                OfferValue = offerValue,
                SuccessChance = successChance,
                DiscoveredRisk = discoveryRisk,
                Status = "pending",
                Day = day
            };

            _state.Offers.Add(offer);
            return (true, "Defection offer delivered.", offer);
        }

        public void TickDay(int day)
        {
            // Process campaigns
            foreach (var campaign in _state.Campaigns)
            {
                if (campaign.Status == "in_progress" && day >= campaign.StartedDay + campaign.DurationDays)
                {
                    bool succeeded = campaign.SuccessChance >= 40;
                    campaign.Status = succeeded ? "succeeded" : "failed";
                    campaign.CompletedDay = day;

                    if (succeeded)
                    {
                        // Generate recruited candidate
                        var candidate = DiscoverCandidate("candidate_scavenger", "outpost_zone", day, campaign.TargetFaction);
                        candidate.IsRecruited = true;

                        _state.Events.Add(new RecruitmentEventRecord
                        {
                            EventId = $"recevt_{_state.NextSequence++}",
                            EventType = "recruitment_success",
                            Day = day,
                            Description = $"Campaign {campaign.CampaignId} recruited new survivor.",
                            Outcome = "success"
                        });
                    }
                    else
                    {
                        _state.Events.Add(new RecruitmentEventRecord
                        {
                            EventId = $"recevt_{_state.NextSequence++}",
                            EventType = "recruitment_failure",
                            Day = day,
                            Description = $"Campaign {campaign.CampaignId} concluded without new recruits.",
                            Outcome = "failure"
                        });
                    }

                    OnCampaignCompleted?.Invoke(campaign);
                }
            }

            // Process defection offers
            foreach (var offer in _state.Offers)
            {
                if (offer.Status == "pending" && day > offer.Day)
                {
                    bool isDiscovered = offer.DiscoveredRisk >= 35; // Risk check
                    if (isDiscovered)
                    {
                        offer.Status = "discovered";
                        OnRecruiterDiscoveredAlert?.Invoke(offer);
                        _state.Events.Add(new RecruitmentEventRecord
                        {
                            EventId = $"recevt_{_state.NextSequence++}",
                            EventType = "defection_discovered",
                            Day = day,
                            Description = $"Defection offer {offer.OfferId} discovered by faction security.",
                            Outcome = "discovered"
                        });
                    }
                    else
                    {
                        bool accepted = offer.SuccessChance >= 35;
                        offer.Status = accepted ? "accepted" : "rejected";

                        var cand = _state.Candidates.FirstOrDefault(c => string.Equals(c.CandidateId, offer.TargetCandidateId, StringComparison.OrdinalIgnoreCase));
                        if (cand != null && accepted)
                        {
                            cand.IsRecruited = true;
                        }

                        OnDefectionResolved?.Invoke(offer);
                    }
                }
            }
        }

        public IReadOnlyList<RecruitmentCampaignRecord> GetActiveCampaigns() =>
            _state.Campaigns.Where(c => c.Status == "in_progress").ToList();

        public IReadOnlyList<RecruitmentCandidateRecord> GetKnownCandidates() =>
            _state.Candidates.Where(c => !c.IsRecruited).ToList();

        public IReadOnlyList<DefectionOfferRecord> GetDefectionOffers() => _state.Offers.ToList();

        public RecruitmentState CaptureState() => _state;

        public void RestoreState(RecruitmentState? state)
        {
            _state = state ?? new RecruitmentState();
        }
    }
}
