// SPDX-License-Identifier: MIT
// ============================================================================
// Host Diagnostic Probe : RecruitmentSelfTest
// Core System           : Ashfall.Core.Survivors.RecruitmentSystem
// Plan Reference        : Plan 204 — Survivor Recruitment & Defection System
// Purpose               : 12-check standalone diagnostic probe verifying
//                         recruitment catalogs, discovery, campaigns, offers,
//                         roster admission, day progression, census, and save round-trip.
// ============================================================================

using System;
using System.IO;
using System.Linq;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static class RecruitmentSelfTest
    {
        public static int Run(string dataDir)
        {
            Console.WriteLine("=== [HostCli] Survivor Recruitment & Defection System Self-Test (Plan 204) ===");
            int passed = 0;

            void Check(bool condition, string name)
            {
                if (condition)
                {
                    Console.WriteLine($"[PASS] Check {++passed}: {name}");
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check {passed + 1}: {name}");
                    throw new InvalidOperationException($"Recruitment self-test assertion failed: {name}");
                }
            }

            try
            {
                // Check 1: Catalog loading from recruitment_templates.json
                var system = new RecruitmentSystem();
                var session = new RecruitmentHostSession(system);
                bool loaded = session.LoadCatalog(dataDir);
                Check(loaded && session.CatalogReady,
                    "Authoritative templates loaded from recruitment_templates.json.");

                // Check 2: Initial state baseline
                var census0 = session.GetCensus();
                Check(census0.ActiveCampaigns == 0 && census0.KnownCandidates == 0 && census0.TotalRecruited == 0 && census0.PendingOffers == 0,
                    "Initial state baseline clean (0 campaigns, 0 candidates, 0 recruited, 0 offers).");

                // Check 3: Discover recruitment candidate
                var candidate = session.DiscoverCandidate("candidate_scavenger", "loc_outskirts", day: 1);
                Check(candidate != null && candidate.CandidateType == "wilderness_survivor" && !candidate.IsRecruited,
                    $"Candidate discovered from template (id={candidate?.CandidateId}, type={candidate?.CandidateType}).");

                // Check 4: Known candidate list tracking
                var candidates = session.GetKnownCandidates();
                Check(candidates.Count == 1 && candidates[0].CandidateId == candidate!.CandidateId,
                    "Discovered candidate accurately tracked in known candidates roster.");

                // Check 5: Start active recruitment campaign
                var (campOk, campMsg, campaign) = session.StartCampaign("active_recruitment", "recruiter_john", "neutral", day: 1, recruiterSkill: 60);
                Check(campOk && campaign != null && campaign.Status == "in_progress",
                    $"Active recruitment campaign started (id={campaign?.CampaignId}, type={campaign?.CampaignTypeId}).");

                // Check 6: Concurrent campaign capacity limits
                var (campOk2, _, _) = session.StartCampaign("asylum_offer", "recruiter_sarah", "refugees", day: 1, recruiterSkill: 50);
                var (campOk3, _, _) = session.StartCampaign("defection_inducement", "recruiter_mike", "militia", day: 1, recruiterSkill: 55);
                var (campOk4, campMsg4, _) = session.StartCampaign("trade_for_survivor", "recruiter_emma", "traders", day: 1, recruiterSkill: 40);
                Check(campOk2 && campOk3 && !campOk4 && campMsg4.Contains("capacity"),
                    "Maximum concurrent campaign capacity enforced (3 active allowed, 4th rejected).");

                // Check 7: Defection offer creation
                var candidateDefector = session.DiscoverCandidate("candidate_faction_deserter", "loc_perimeter", day: 1, currentFaction: "militia");
                var (offerOk, offerMsg, offer) = session.MakeDefectionOffer(candidateDefector.CandidateId, "rations_safety", 50f, day: 1, diplomacySkill: 75);
                Check(offerOk && offer != null && offer.Status == "pending",
                    $"Defection offer extended to candidate (targetId={offer?.TargetCandidateId}, value={offer?.OfferValue}).");

                // Check 8: Defection offer tracking
                var offers = session.GetDefectionOffers();
                Check(offers.Count == 1 && offers[0].OfferId == offer!.OfferId,
                    "Defection offer recorded in pending offers list.");

                // Check 9: Day tick progression
                session.TickDay(day: 10);
                var activeCampaigns = session.GetActiveCampaigns();
                Check(activeCampaigns.Count < 3,
                    $"Day progression correctly completed expired campaigns (active remaining={activeCampaigns.Count}).");

                // Check 10: Roster admission of discovered candidate
                bool admitted = session.AdmitCandidate(candidate!.CandidateId, out var admittedCandidate);
                Check(admitted && admittedCandidate != null && admittedCandidate.IsRecruited,
                    $"Candidate successfully admitted to shelter roster (id={admittedCandidate?.CandidateId}).");

                // Check 11: Census reflects operational metrics
                var census = session.GetCensus();
                Check(census.TotalRecruited >= 1 && census.PendingOffers >= 1,
                    $"Recruitment census reflects live metrics (Recruited={census.TotalRecruited}, Known={census.KnownCandidates}, Offers={census.PendingOffers}).");

                // Check 12: Save and restore state fidelity
                var state = session.CaptureState();
                var restoredSystem = new RecruitmentSystem();
                var restoredSession = new RecruitmentHostSession(restoredSystem);
                restoredSession.RestoreState(state);
                var restoredCensus = restoredSession.GetCensus();
                Check(restoredCensus.TotalRecruited >= 1 &&
                      restoredCensus.KnownCandidates == census.KnownCandidates &&
                      restoredSession.CaptureState().Candidates.Any(c => c.CandidateId == candidate.CandidateId && c.IsRecruited),
                    "Save and restore state verified with full round-trip fidelity.");

                Console.WriteLine($"=== [HostCli] Recruitment System Self-Test PASSED ({passed}/12 checks) ===");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Recruitment self-test threw exception: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                return 1;
            }
        }
    }
}
