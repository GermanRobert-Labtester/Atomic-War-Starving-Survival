// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Narrative;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Narrative
{
    public class JusticeSystemTests
    {
        private WastelandLawDef CreateTheftLaw() => new WastelandLawDef
        {
            law_id = "law_theft",
            display_name = "Theft Restitution",
            crime_type = "Theft",
            min_evidence_confidence = 0.40f,
            allowed_punishments = new List<string> { "Warning", "Restitution" },
            legitimacy_impact = 5f
        };

        private WastelandLawDef CreateMurderLaw() => new WastelandLawDef
        {
            law_id = "law_murder",
            display_name = "Blood Decree",
            crime_type = "Murder",
            min_evidence_confidence = 0.60f,
            allowed_punishments = new List<string> { "Banishment", "Execution" },
            legitimacy_impact = 15f
        };

        [Fact]
        public void CrimeReporting_And_EvidenceAggregation()
        {
            var sys = new JusticeSystem(new SeededRng(42));
            sys.RegisterLaw(CreateTheftLaw());

            var inc = sys.ReportCrime("inc_001", CrimeType.Theft, "survivor_thief", "survivor_victim", 1);
            Assert.Equal("law_theft", inc.assignedLawId);
            Assert.Equal(IncidentStatus.Unresolved, inc.status);

            sys.AddEvidence("inc_001", "clue_01", "Stolen rations found under bunk", 0.50f, 1);
            float score = sys.CalculateEvidenceStrength("inc_001");
            Assert.Equal(0.50f, score, 2);
        }

        [Fact]
        public void Trial_Blocks_If_Evidence_Below_Law_Threshold()
        {
            var sys = new JusticeSystem(new SeededRng(42));
            sys.RegisterLaw(CreateMurderLaw());

            sys.ReportCrime("inc_002", CrimeType.Murder, "survivor_suspect", "survivor_victim", 1);
            sys.AddEvidence("inc_002", "clue_weak", "Circumstantial witness statement", 0.25f, 1);

            var decision = new TrialDecision
            {
                incidentId = "inc_002",
                verdict = TrialVerdict.Guilty,
                punishment = PunishmentLevel.Execution
            };

            var res = sys.HoldTrial(decision, 2);
            Assert.False(res.Success);
            Assert.Equal("insufficient_evidence_for_conviction", res.FailureCode);
        }

        [Fact]
        public void Trial_Applies_Restitution_And_AwardsScrap()
        {
            var inv = new Inventory.Inventory();
            var sys = new JusticeSystem(new SeededRng(42), inv);
            sys.RegisterLaw(CreateTheftLaw());

            sys.ReportCrime("inc_003", CrimeType.Theft, "survivor_thief", null, 1);
            sys.AddEvidence("inc_003", "clue_firm", "Caught in the act", 0.80f, 1);

            var decision = new TrialDecision
            {
                incidentId = "inc_003",
                verdict = TrialVerdict.Guilty,
                punishment = PunishmentLevel.Restitution
            };

            var res = sys.HoldTrial(decision, 1);
            Assert.True(res.Success);
            Assert.Equal(TrialVerdict.Guilty, res.Verdict);
            Assert.True(inv.CountById("scrap_metal") > 0);
        }

        [Fact]
        public void Trial_Applies_Banishment()
        {
            var sys = new JusticeSystem(new SeededRng(42));
            sys.RegisterLaw(CreateMurderLaw());

            sys.ReportCrime("inc_004", CrimeType.Murder, "survivor_killer", null, 1);
            sys.AddEvidence("inc_004", "clue_strong", "Bloody knife in locker", 0.90f, 1);

            var decision = new TrialDecision
            {
                incidentId = "inc_004",
                verdict = TrialVerdict.Guilty,
                punishment = PunishmentLevel.Banishment
            };

            var res = sys.HoldTrial(decision, 2);
            Assert.True(res.Success);
            Assert.Equal(1, sys.State.totalBanishments);
            Assert.Single(sys.State.banishments);
            Assert.Equal("survivor_killer", sys.State.banishments[0].survivorId);
        }

        [Fact]
        public void Trial_Applies_Execution_And_Inflicts_Death()
        {
            var needs = new NeedsSystem();
            var dweller = new SurvivorNeedsState { Id = "survivor_condemned", Health = 100f, Morale = 50f };
            var bystander = new SurvivorNeedsState { Id = "survivor_bystander", Health = 100f, Morale = 50f };
            needs.Register(dweller);
            needs.Register(bystander);

            var sys = new JusticeSystem(new SeededRng(42), null, needs);
            sys.RegisterLaw(CreateMurderLaw());

            sys.ReportCrime("inc_005", CrimeType.Murder, "survivor_condemned", null, 1);
            sys.AddEvidence("inc_005", "clue_forensic", "Forensic match", 0.95f, 1);

            var decision = new TrialDecision
            {
                incidentId = "inc_005",
                verdict = TrialVerdict.Guilty,
                punishment = PunishmentLevel.Execution
            };

            var res = sys.HoldTrial(decision, 2);
            Assert.True(res.Success);
            Assert.Equal(1, sys.State.totalExecutions);
            Assert.True(dweller.Health <= 0f);
            Assert.True(bystander.Morale < 50f); // morale shockwave
        }

        [Fact]
        public void VigilanteMob_Triggers_On_Neglected_Severe_Crimes()
        {
            var sys = new JusticeSystem(new SeededRng(42));
            sys.ReportCrime("inc_assault", CrimeType.Assault, "survivor_violent", null, 1);

            // Day 1 to 8 without trial
            for (int day = 1; day <= 8; day++)
            {
                sys.TickDay(day);
            }

            var inc = sys.State.incidents.Find(i => i.incidentId == "inc_assault");
            Assert.NotNull(inc);
            Assert.Equal(IncidentStatus.VigilanteResolved, inc!.status);
        }

        [Fact]
        public void State_RoundTrip_Preserves_Incidents_And_Banishments()
        {
            var sys = new JusticeSystem(new SeededRng(42));
            sys.ReportCrime("inc_saved", CrimeType.Sabotage, "survivor_saboteur", null, 1);
            sys.State.banishments.Add(new BanishmentRecord { survivorId = "survivor_exiled", banishedDay = 3 });

            var state = sys.State;
            var json = System.Text.Json.JsonSerializer.Serialize(state);

            var deserialized = System.Text.Json.JsonSerializer.Deserialize<JusticeState>(json);
            var sys2 = new JusticeSystem(new SeededRng(42));
            sys2.RestoreState(deserialized!);

            Assert.Single(sys2.State.incidents);
            Assert.Equal("inc_saved", sys2.State.incidents[0].incidentId);
            Assert.Single(sys2.State.banishments);
            Assert.Equal("survivor_exiled", sys2.State.banishments[0].survivorId);
        }
    }
}
