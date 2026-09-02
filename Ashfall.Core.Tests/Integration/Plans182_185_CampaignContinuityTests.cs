using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Factions;
using Ashfall.Core.Medical;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests.Integration
{
    public class Plans182_185_CampaignContinuityTests
    {
        private static string ReadDataFile(string filename)
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string candidate = Path.Combine(dir.FullName, "Assets", "StreamingAssets", "Data", filename);
                if (File.Exists(candidate)) return File.ReadAllText(candidate);
                dir = dir.Parent;
            }
            throw new FileNotFoundException($"Could not locate data file {filename}");
        }

        [Fact]
        public void Plans182_185_FlagshipCampaignContinuityJourney()
        {
            var json = new SystemTextJsonSerializer();
            var rng = new SeededRng(182_185);

            // 1. Initialize all four systems
            var aviation = new AviationSystem();
            aviation.LoadCatalog(ReadDataFile("aircraft_parts.json"), json);

            var labor = new ForcedLaborSystem();
            labor.LoadCatalog(ReadDataFile("labor_camps.json"), json);

            var narcotics = new NarcoticsSystem();
            narcotics.LoadCatalog(ReadDataFile("narcotics.json"), json);

            var politics = new PoliticsSystem();
            politics.LoadCatalog(ReadDataFile("political_policies.json"), json);
            politics.SetInitialLeader("commander_vance");

            // ── Phase A: Aviation Aerial Reconnaissance ──────────────────────
            var plane = aviation.RegisterAircraft("plane_alpha", "aircraft_observation_balloon");
            var flight = aviation.LaunchFlight("recon_01", "plane_alpha", new List<string> { "scout_elena" }, "holdfast", "ash_ridge", 30f, 40f, 0f);

            Assert.Equal(FlightPhase.AirborneOutbound, flight.phase);

            // Advance flight to completion
            for (int tick = 0; tick < 5; tick++)
            {
                if (flight.phase == FlightPhase.Landed) break;
                aviation.AdvanceFlightTick("recon_01", 0.5f, 10f, 0.9f, 5f, 0f, rng);
            }

            Assert.Equal(FlightPhase.Landed, flight.phase);
            Assert.True(flight.mapCellsRevealed > 0, "Aerial mission must uncover terrain cells");

            // ── Phase B: Coercive Captive Labor ──────────────────────────────
            labor.SetGuardCount(2);
            bool c1Assigned = labor.AssignLaborer("raider_capt_01", "camp_scrap_demolition", true, out string r1);
            bool c2Assigned = labor.AssignLaborer("raider_capt_02", "camp_sump_drainage", true, out string r2);
            Assert.True(c1Assigned, r1);
            Assert.True(c2Assigned, r2);

            float totalScrapOutput = 0f;
            labor.OnLaborOutputGenerated += (_, amount) => totalScrapOutput += amount;

            // Run 3 days of labor shifts
            for (int d = 0; d < 3; d++)
            {
                labor.AdvanceDailyShift(rng);
            }

            Assert.True(totalScrapOutput > 0f);
            Assert.True(labor.CrueltyIndex > 0f, "Captive labor must accumulate settlement CrueltyIndex");

            // ── Phase C: Chemical Medicine Synthesis & Administration ─────────
            var inventory = new Dictionary<string, int>
            {
                ["item_medical_precursor_base"] = 5,
                ["item_sterile_solvent_pack"] = 5,
                ["item_clean_water"] = 5
            };

            bool brewed = narcotics.BrewChem(
                "chem_dulcimer_tincture",
                id => inventory.GetValueOrDefault(id, 0),
                (id, count) => inventory[id] -= count,
                (id, count) => inventory[id] = inventory.GetValueOrDefault(id, 0) + count,
                out string brewErr);

            Assert.True(brewed, brewErr);
            Assert.True(inventory["item_chem_dulcimer_tincture"] > 0);

            // Administer to fatigued pilot
            bool administered = narcotics.AdministerChem("scout_elena", "chem_dulcimer_tincture", rng, out string adminMsg);
            Assert.True(administered, adminMsg);

            var elenaProfile = narcotics.GetOrCreateProfile("scout_elena");
            Assert.True(elenaProfile.bloodToxicity > 0f);
            Assert.Single(elenaProfile.activeEffects);

            // Put an addicted colonist in rehab
            var addict = narcotics.GetOrCreateProfile("addict_marcus");
            addict.dependencies.Add(new DependencyRecord { chemId = "chem_hyper_stim", dependencyLevel = 50f });
            narcotics.AssignToRehabBed("addict_marcus");
            Assert.True(addict.inRehabBed);

            // Advance 1 day of medical metabolism
            narcotics.AdvanceMedicalTick(24f, rng);
            Assert.True(elenaProfile.bloodToxicity < 20f);

            // ── Phase D: Settlement Politics & Democratic Election ───────────
            // Cruelty from labor impacts approval
            // Cruelty from labor impacts approval
            politics.AdvanceDailyPolitics(0.3f, 0.5f, labor.CrueltyIndex, 0, rng);
            var approval = politics.CalculateApprovalBreakdown(0.3f, 0.5f, labor.CrueltyIndex);
            Assert.True(approval.crueltyPenalty < 0f, "High cruelty must inflict an approval penalty");

            // Enact rationing policy
            bool enacted = politics.EnactPolicy("policy_emergency_rationing", out string polErr);
            Assert.True(enacted, polErr);
            Assert.Contains("policy_emergency_rationing", politics.ActivePolicies);

            // Hold scheduled democratic election
            var candidates = new List<string> { "commander_vance", "reformer_sarah" };
            var voters = new List<string> { "scout_elena", "addict_marcus", "colonist_john", "colonist_mary" };
            var traitBook = new Dictionary<string, List<string>>
            {
                ["commander_vance"] = new List<string> { "authoritarian" },
                ["reformer_sarah"] = new List<string> { "humanitarian" },
                ["scout_elena"] = new List<string> { "humanitarian" },
                ["addict_marcus"] = new List<string> { "humanitarian" },
                ["colonist_john"] = new List<string> { "humanitarian" },
                ["colonist_mary"] = new List<string> { "authoritarian" }
            };

            var electionResult = politics.HoldElection(
                currentDay: 30,
                candidates,
                voters,
                id => traitBook.GetValueOrDefault(id, new List<string>()),
                foodSat: 0.3f,
                secSat: 0.5f,
                rng);

            Assert.Equal(4, electionResult.totalTurnout);
            Assert.Equal("reformer_sarah", electionResult.electedLeaderId);
            Assert.Equal("reformer_sarah", politics.CurrentLeaderId);

            // ── Phase E: State Capture & Deterministic Restoration ────────────
            var avState = aviation.CaptureState();
            var lbState = labor.CaptureState();
            var ncState = narcotics.CaptureState();
            var polState = politics.CaptureState();

            // Recreate systems
            var restoredAviation = new AviationSystem();
            restoredAviation.RestoreState(avState);
            Assert.Equal(1, restoredAviation.TotalLaunched);
            Assert.Equal(1, restoredAviation.TotalLanded);

            var restoredLabor = new ForcedLaborSystem();
            restoredLabor.RestoreState(lbState);
            Assert.Equal(2, restoredLabor.Laborers.Count);
            Assert.Equal(lbState.crueltyIndex, restoredLabor.CrueltyIndex);

            var restoredNarcotics = new NarcoticsSystem();
            restoredNarcotics.RestoreState(ncState);
            Assert.Equal(2, restoredNarcotics.Profiles.Count);

            var restoredPolitics = new PoliticsSystem();
            restoredPolitics.RestoreState(polState);
            Assert.Equal("reformer_sarah", restoredPolitics.CurrentLeaderId);
            Assert.Contains("policy_emergency_rationing", restoredPolitics.ActivePolicies);
        }
    }
}
