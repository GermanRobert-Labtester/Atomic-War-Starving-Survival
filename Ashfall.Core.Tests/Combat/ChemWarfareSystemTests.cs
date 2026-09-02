// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Combat;
using Xunit;

namespace Ashfall.Core.Tests.Combat
{
    public class ChemWarfareSystemTests
    {
        private static string GetCatalogJson()
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Assets/StreamingAssets/Data/chemical_weapons.json");
            if (!File.Exists(path))
            {
                path = Path.Combine(AppContext.BaseDirectory, "../../../Assets/StreamingAssets/Data/chemical_weapons.json");
            }
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }

            return @"{
  ""schema_version"": 1,
  ""agents"": [
    {
      ""id"": ""chem_agent_irritant_prewar"",
      ""display_name"": ""Pre-War Tear Particulate"",
      ""hazard_class"": ""irritant"",
      ""base_density_permille"": 500,
      ""persistence_ticks"": 8,
      ""filter_wear_permille"": 40,
      ""exposure_severity"": 1,
      ""visual_profile_id"": ""toxic_fog_irritant"",
      ""description"": ""Test agent""
    },
    {
      ""id"": ""chem_agent_choke_vapor"",
      ""display_name"": ""Halogen Choke Vapor"",
      ""hazard_class"": ""choke_vapor"",
      ""base_density_permille"": 700,
      ""persistence_ticks"": 10,
      ""filter_wear_permille"": 75,
      ""exposure_severity"": 2,
      ""visual_profile_id"": ""toxic_fog_choke"",
      ""description"": ""Test agent 2""
    }
  ]
}";
        }

        [Fact]
        public void DeployHazard_CreatesActiveHazardWithCatalogDefaults()
        {
            var sys = new ChemWarfareSystem(new SeededRng(100));
            sys.LoadCatalog(GetCatalogJson());

            var hazard = sys.DeployHazard("chem_agent_irritant_prewar", 1, "test_turret");
            Assert.NotNull(hazard);
            Assert.Equal("chem_agent_irritant_prewar", hazard.AgentId);
            Assert.Equal(1, hazard.CombatLane);
            Assert.Equal(500, hazard.DensityPermille);
            Assert.Equal(8, hazard.RemainingTicks);
            Assert.Single(sys.State.ActiveHazards);
        }

        [Fact]
        public void DeployHazard_RespectsCustomDensityClamping()
        {
            var sys = new ChemWarfareSystem(new SeededRng(101));
            sys.LoadCatalog(GetCatalogJson());

            var hazard = sys.DeployHazard("chem_agent_choke_vapor", 0, "test_grenade", 1200);
            Assert.NotNull(hazard);
            Assert.Equal(1000, hazard.DensityPermille); // clamped to 1000
            Assert.Equal(0, hazard.CombatLane);
        }

        [Fact]
        public void TickCombat_DecaysDensityAndRemovesExpired()
        {
            var sys = new ChemWarfareSystem(new SeededRng(102));
            sys.LoadCatalog(GetCatalogJson());

            var hazard = sys.DeployHazard("chem_agent_irritant_prewar", 1, "source");
            Assert.NotNull(hazard);

            // Tick in clear weather, no wind
            sys.TickCombat(WeatherKind.Clear, 0, 0);
            Assert.Equal(7, hazard.RemainingTicks);
            Assert.Equal(450, hazard.DensityPermille); // 500 - 50 = 450

            // Advance through remaining ticks until cleared
            for (int i = 0; i < 7; i++)
            {
                sys.TickCombat(WeatherKind.Clear, 0, 0);
            }

            Assert.Empty(sys.State.ActiveHazards);
        }

        [Fact]
        public void TickCombat_WeatherAcceleratesPrecipitationDecay()
        {
            var sysClear = new ChemWarfareSystem(new SeededRng(103));
            sysClear.LoadCatalog(GetCatalogJson());
            var hClear = sysClear.DeployHazard("chem_agent_irritant_prewar", 1, "src");

            var sysRain = new ChemWarfareSystem(new SeededRng(103));
            sysRain.LoadCatalog(GetCatalogJson());
            var hRain = sysRain.DeployHazard("chem_agent_irritant_prewar", 1, "src");

            sysClear.TickCombat(WeatherKind.Clear, 0, 0);
            sysRain.TickCombat(WeatherKind.Rain, 0, 0);

            Assert.True(hRain.DensityPermille < hClear.DensityPermille);
        }

        [Fact]
        public void TickCombat_WindDriftsHazardAcrossLanes()
        {
            var sys = new ChemWarfareSystem(new SeededRng(104));
            sys.LoadCatalog(GetCatalogJson());

            var hazard = sys.DeployHazard("chem_agent_irritant_prewar", 1, "source");
            Assert.NotNull(hazard);

            // Wind pushing right (windDirection = 1, strength = 2)
            sys.TickCombat(WeatherKind.Clear, 1, 2);
            Assert.Equal(2, hazard.CombatLane);
        }

        [Fact]
        public void EvaluateActorExposure_ProtectedActor_FilterAbsorbsAndDegrades()
        {
            var sys = new ChemWarfareSystem(new SeededRng(105));
            sys.LoadCatalog(GetCatalogJson());

            sys.DeployHazard("chem_agent_irritant_prewar", 1, "src");

            // Good mask condition (90%)
            int severity = sys.EvaluateActorExposure("survivor_1", 1, 0.90f, out float filterWear);
            Assert.Equal(0, severity); // fully absorbed
            Assert.True(filterWear > 0f);
        }

        [Fact]
        public void EvaluateActorExposure_DamagedMask_PartialBreakthrough()
        {
            var sys = new ChemWarfareSystem(new SeededRng(106));
            sys.LoadCatalog(GetCatalogJson());

            sys.DeployHazard("chem_agent_choke_vapor", 0, "src"); // severity 2

            // Moderate mask condition (40%)
            int severity = sys.EvaluateActorExposure("survivor_2", 0, 0.40f, out float filterWear);
            Assert.Equal(1, severity); // reduced from 2 to 1
            Assert.True(filterWear > 0f);
        }

        [Fact]
        public void EvaluateActorExposure_NoMask_FullSeverityAndEventFired()
        {
            var sys = new ChemWarfareSystem(new SeededRng(107));
            sys.LoadCatalog(GetCatalogJson());

            sys.DeployHazard("chem_agent_choke_vapor", 2, "src");

            bool eventFired = false;
            sys.OnToxicExposureResolved += (actor, sev, lane) =>
            {
                if (actor == "survivor_3" && sev == 2 && lane == 2)
                    eventFired = true;
            };

            int severity = sys.EvaluateActorExposure("survivor_3", 2, 0f, out float filterWear);
            Assert.Equal(2, severity);
            Assert.True(eventFired);
        }

        [Fact]
        public void ClearHazard_RemovesHazardCorrectly()
        {
            var sys = new ChemWarfareSystem(new SeededRng(108));
            sys.LoadCatalog(GetCatalogJson());

            var hazard = sys.DeployHazard("chem_agent_irritant_prewar", 1, "src");
            Assert.NotNull(hazard);

            bool cleared = sys.ClearHazard(hazard.HazardId);
            Assert.True(cleared);
            Assert.Empty(sys.State.ActiveHazards);
        }

        [Fact]
        public void TriggerShelterResidueHandoff_FiresEventAndIncrementsCounter()
        {
            var sys = new ChemWarfareSystem(new SeededRng(109));

            string? reportedSector = null;
            int reportedSeverity = 0;
            sys.OnShelterResidueCreated += (sec, sev) =>
            {
                reportedSector = sec;
                reportedSeverity = sev;
            };

            sys.TriggerShelterResidueHandoff("sector_airlock_alpha", 3);
            Assert.Equal("sector_airlock_alpha", reportedSector);
            Assert.Equal(3, reportedSeverity);
            Assert.Equal(1, sys.State.TotalResidueIncidentsLogged);
        }

        [Fact]
        public void SaveRestore_PreservesActiveHazardsAndCounters()
        {
            var sys1 = new ChemWarfareSystem(new SeededRng(110));
            sys1.LoadCatalog(GetCatalogJson());

            sys1.DeployHazard("chem_agent_irritant_prewar", 0, "src_a", 800);
            sys1.DeployHazard("chem_agent_choke_vapor", 2, "src_b", 600);
            sys1.TriggerShelterResidueHandoff("sector_b", 2);

            var saved = sys1.CaptureState();

            var sys2 = new ChemWarfareSystem(new SeededRng(111));
            sys2.RestoreState(saved);

            Assert.Equal(2, sys2.State.ActiveHazards.Count);
            Assert.Equal(2, sys2.State.TotalHazardsDeployed);
            Assert.Equal(1, sys2.State.TotalResidueIncidentsLogged);
            Assert.Equal(800, sys2.State.ActiveHazards[0].DensityPermille);
            Assert.Equal(600, sys2.State.ActiveHazards[1].DensityPermille);
        }

        [Fact]
        public void DeterministicReplay_SameSeedProducesIdenticalOutcomes()
        {
            var sysA = new ChemWarfareSystem(new SeededRng(999));
            var sysB = new ChemWarfareSystem(new SeededRng(999));
            sysA.LoadCatalog(GetCatalogJson());
            sysB.LoadCatalog(GetCatalogJson());

            var hA = sysA.DeployHazard("chem_agent_choke_vapor", 1, "test");
            var hB = sysB.DeployHazard("chem_agent_choke_vapor", 1, "test");

            for (int i = 0; i < 4; i++)
            {
                sysA.TickCombat(WeatherKind.Overcast, 1, 1);
                sysB.TickCombat(WeatherKind.Overcast, 1, 1);
            }

            Assert.Equal(hA.RemainingTicks, hB.RemainingTicks);
            Assert.Equal(hA.DensityPermille, hB.DensityPermille);
            Assert.Equal(hA.CombatLane, hB.CombatLane);
        }
    }
}
