using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Ashfall.Core;
using Ashfall.Core.Random;
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Journeys
{
    public sealed class FireIncidentJourneyTests
    {
        private static List<FireZoneState> CreateDemoZones()
        {
            return new List<FireZoneState>
            {
                new FireZoneState
                {
                    zoneId = "zone_kitchen",
                    displayName = "Kitchen & Mess",
                    fireLevel = 0.4f,
                    smokeLevel = 0.2f,
                    coLevel = 0.05f,
                    heatLevel = 0.2f,
                    damperOpen = true,
                    adjacentZoneIds = new List<string> { "zone_corridor" }
                },
                new FireZoneState
                {
                    zoneId = "zone_corridor",
                    displayName = "Central Corridor",
                    fireLevel = 0f,
                    smokeLevel = 0f,
                    coLevel = 0f,
                    heatLevel = 0f,
                    damperOpen = true,
                    adjacentZoneIds = new List<string> { "zone_kitchen", "zone_reactor" }
                },
                new FireZoneState
                {
                    zoneId = "zone_reactor",
                    displayName = "Reactor Bay",
                    fireLevel = 0f,
                    smokeLevel = 0f,
                    coLevel = 0f,
                    heatLevel = 0f,
                    damperOpen = false,
                    adjacentZoneIds = new List<string> { "zone_corridor" }
                }
            };
        }

        [Fact]
        public void Journey_J39_FireIncident_CanonicalAuthority_DynamicResolution_AndActionMutations()
        {
            // 1. Composition root owns single ShelterFireHazardSystem
            var fireSystem = new ShelterFireHazardSystem();
            Assert.Empty(fireSystem.Incidents);

            // 2. An incident ignites in the shelter (e.g. from an arc fault or hazard event)
            string canonicalIncidentId = "vent_arc_kitchen_day5";
            bool ignited = fireSystem.Ignite(canonicalIncidentId, "zone_kitchen", 5, CreateDemoZones());
            Assert.True(ignited);
            Assert.Single(fireSystem.Incidents);

            // 3. Dynamic resolution finds the canonical incident without hardcoded 'inc_default'
            var incident = fireSystem.GetIncident(canonicalIncidentId);
            Assert.NotNull(incident);
            Assert.Equal("zone_kitchen", incident.sourceZoneId);
            Assert.False(incident.alarmRaised);
            Assert.Empty(incident.brigadeWorkers);
            Assert.Equal(0, incident.extinguisherChargesUsed);

            // 4. Raise alarm mutates canonical incident
            bool alarmRaised = fireSystem.RaiseAlarm(canonicalIncidentId);
            Assert.True(alarmRaised);
            Assert.True(incident.alarmRaised);

            // 5. Assign brigade mutates canonical incident
            var workers = new List<string> { "surv_01", "surv_02" };
            bool brigadeAssigned = fireSystem.AssignBrigade(canonicalIncidentId, workers);
            Assert.True(brigadeAssigned);
            Assert.Equal(2, incident.brigadeWorkers.Count);

            // 6. Deploy extinguisher mutates hottest zone on canonical incident
            float initialFire = incident.zones.First(z => z.zoneId == "zone_kitchen").fireLevel;
            bool extDeployed = fireSystem.DeployExtinguisher(canonicalIncidentId, "zone_kitchen");
            Assert.True(extDeployed);
            Assert.Equal(1, incident.extinguisherChargesUsed);
            float afterFire = incident.zones.First(z => z.zoneId == "zone_kitchen").fireLevel;
            Assert.True(afterFire < initialFire, $"Extinguisher did not reduce fire level: {initialFire} -> {afterFire}");

            // 7. Advancing simulation tick operates on canonical incident
            var rng = new SeededRng(1986);
            fireSystem.Tick(canonicalIncidentId, rng);
            Assert.Equal(1, incident.ticksElapsed);

            // 8. CaptureState / RestoreState round-trips canonical incident
            var captured = fireSystem.CaptureState();
            Assert.True(captured.ContainsKey(canonicalIncidentId));
            Assert.True(captured[canonicalIncidentId].alarmRaised);
            Assert.Equal(1, captured[canonicalIncidentId].extinguisherChargesUsed);

            var restoredSystem = new ShelterFireHazardSystem();
            restoredSystem.RestoreState(captured);
            var restoredIncident = restoredSystem.GetIncident(canonicalIncidentId);
            Assert.NotNull(restoredIncident);
            Assert.True(restoredIncident.alarmRaised);
            Assert.Equal(2, restoredIncident.brigadeWorkers.Count);
            Assert.Equal(1, restoredIncident.extinguisherChargesUsed);
            Assert.Equal(1, restoredIncident.ticksElapsed);
        }

        [Fact]
        public void ProductionRoutes_DoNotInstantiateThrowawayFireSystem_OrUseFixtureId()
        {
            string current = Directory.GetCurrentDirectory();
            string? srcRoot = null;
            while (!string.IsNullOrEmpty(current))
            {
                string candidate = Path.Combine(current, "src");
                if (Directory.Exists(candidate))
                {
                    srcRoot = candidate;
                    break;
                }
                string parent = Path.GetDirectoryName(current)!;
                if (parent == current) break;
                current = parent;
            }

            Assert.NotNull(srcRoot);

            string playerSurfacesPath = Path.Combine(srcRoot, "Main.PlayerSurfaces.cs");
            Assert.True(File.Exists(playerSurfacesPath), $"Could not find Main.PlayerSurfaces.cs at {playerSurfacesPath}");

            string content = File.ReadAllText(playerSurfacesPath);

            // Prohibit new ShelterFireHazardSystem in route
            Assert.DoesNotContain("new Ashfall.Core.Shelter.ShelterFireHazardSystem", content);
            Assert.DoesNotContain("new ShelterFireHazardSystem", content);

            // Prohibit hardcoded fixture id inc_default in production route
            Assert.DoesNotContain("\"inc_default\"", content);
        }

        [Fact]
        public void FireIncidentPanel_DoesNotHaveProhibitedLambdaUnsubscriptions()
        {
            string current = Directory.GetCurrentDirectory();
            string? srcRoot = null;
            while (!string.IsNullOrEmpty(current))
            {
                string candidate = Path.Combine(current, "src");
                if (Directory.Exists(candidate))
                {
                    srcRoot = candidate;
                    break;
                }
                string parent = Path.GetDirectoryName(current)!;
                if (parent == current) break;
                current = parent;
            }

            Assert.NotNull(srcRoot);

            string panelPath = Path.Combine(srcRoot, "UI", "FireIncidentPanel.cs");
            Assert.True(File.Exists(panelPath), $"Could not find FireIncidentPanel.cs at {panelPath}");

            string content = File.ReadAllText(panelPath);

            var lambdaUnsubscribeRegex = new Regex(@"-=\s*(?:\w+|\([^)]*\))\s*=>", RegexOptions.Compiled);
            var lines = content.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (line.StartsWith("//") || line.StartsWith("/*")) continue;
                Assert.False(lambdaUnsubscribeRegex.IsMatch(line),
                    $"FireIncidentPanel.cs line {i + 1} contains prohibited lambda unsubscription: {line}");
            }
        }

        [Fact]
        public void FireIncident_SaveEnvelope_RoundTripsCanonicalLedger()
        {
            // Audit #30 — host-facing SaveStore shape (checksum envelope) over Core state.
            var fireSystem = new ShelterFireHazardSystem();
            Assert.True(fireSystem.Ignite("vent_arc_kitchen_day5", "zone_kitchen", 5, CreateDemoZones()));
            Assert.True(fireSystem.RaiseAlarm("vent_arc_kitchen_day5"));
            Assert.True(fireSystem.AssignBrigade("vent_arc_kitchen_day5", new List<string> { "surv_01" }));

            var payload = new Dictionary<string, FireIncidentState>(fireSystem.CaptureState());
            string envelope = SaveEnvelopeHelper.CaptureEnvelope(payload);
            var (ok, restored, error) = SaveEnvelopeHelper.RestoreEnvelope<Dictionary<string, FireIncidentState>>(
                envelope, allowBareFallback: false);
            Assert.True(ok, error);
            Assert.NotNull(restored);

            var reloaded = new ShelterFireHazardSystem();
            reloaded.RestoreState(restored!);
            var incident = reloaded.GetIncident("vent_arc_kitchen_day5");
            Assert.NotNull(incident);
            Assert.True(incident.alarmRaised);
            Assert.Single(incident.brigadeWorkers);
        }

        [Fact]
        public void HostWiring_SaveAllAndProcessFlush_EnrollShelterFire()
        {
            string? srcRoot = FindSrcRoot();
            Assert.NotNull(srcRoot);

            string orch = File.ReadAllText(Path.Combine(srcRoot!, "Main.SaveOrchestrator.cs"));
            string app = File.ReadAllText(Path.Combine(srcRoot!, "Main.Application.cs"));
            Assert.Contains("SaveShelterFire()", orch);
            Assert.Contains("FlushShelterFireIfDirty()", app);
        }

        private static string? FindSrcRoot()
        {
            string current = Directory.GetCurrentDirectory();
            while (!string.IsNullOrEmpty(current))
            {
                string candidate = Path.Combine(current, "src");
                if (Directory.Exists(candidate)) return candidate;
                string parent = Path.GetDirectoryName(current)!;
                if (parent == current) break;
                current = parent;
            }
            return null;
        }
    }
}
