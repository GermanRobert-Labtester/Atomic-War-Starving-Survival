// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 195 — Survivor Specialization Roles: host wiring and eligibility tests.
// Core contract: discipline-based eligibility (read from the SkillProgression
// owner's normalized 0–100 levels), earned practice XP, exactly-once fact
// handling, save/restore round-trip, and the production wiring gate.
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class Plan195SurvivorRoleWiringTests
    {
        private static string RepoRoot()
        {
            string[] candidates = { Directory.GetCurrentDirectory(), AppContext.BaseDirectory };
            foreach (string start in candidates)
            {
                var directory = new DirectoryInfo(Path.GetFullPath(start));
                while (directory != null)
                {
                    if (File.Exists(Path.Combine(directory.FullName, "src", "Main.SurvivorRoles.cs")))
                        return directory.FullName;
                    directory = directory.Parent;
                }
            }
            throw new DirectoryNotFoundException("Could not locate the ASHFALL repository root.");
        }

        private static string Source(string relativePath) =>
            File.ReadAllText(Path.Combine(RepoRoot(), relativePath));

        private static string DataDirectory =>
            Path.Combine(RepoRoot(), "Assets", "StreamingAssets", "Data");

        private static readonly string[] ValidDisciplines =
            { "medical", "crafting", "science", "combat", "scavenging", "survival" };

        // ── Core contract: discipline-based eligibility ──

        [Fact]
        public void CanAssignRoleByDiscipline_RefusesBelowGate_AndAcceptsAtGate()
        {
            var system = new SurvivorRoleSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "survivor_roles.json")));

            var low = new Dictionary<string, float> { { "medical", 20f } };
            bool canLow = system.CanAssignRoleByDiscipline("surv_low", "role_medic", low, out string reasonLow);
            Assert.False(canLow);
            Assert.Equal("insufficient_discipline_medical", reasonLow);

            var ok = new Dictionary<string, float> { { "medical", 40f } };
            bool canOk = system.CanAssignRoleByDiscipline("surv_doc", "role_medic", ok, out _);
            Assert.True(canOk);

            // Missing level entry reads as zero — refused.
            bool canMissing = system.CanAssignRoleByDiscipline(
                "surv_other", "role_medic", new Dictionary<string, float>(), out string reasonMissing);
            Assert.False(canMissing);
            Assert.Equal("insufficient_discipline_medical", reasonMissing);
        }

        [Fact]
        public void CanAssignRoleByDiscipline_EnforcesCap_AndUnknownRole()
        {
            var system = new SurvivorRoleSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "survivor_roles.json")));
            var ok = new Dictionary<string, float> { { "medical", 50f } };

            Assert.NotNull(system.AssignRole("surv_a", "role_medic", force: true));
            Assert.NotNull(system.AssignRole("surv_b", "role_medic", force: true));
            bool third = system.CanAssignRoleByDiscipline("surv_c", "role_medic", ok, out string reasonCap);
            Assert.False(third);
            Assert.Equal("role_cap_reached", reasonCap);

            bool unknown = system.CanAssignRoleByDiscipline("surv_a", "role_nonexistent", ok, out string reasonUnknown);
            Assert.False(unknown);
            Assert.Equal("role_not_found", reasonUnknown);
        }

        [Fact]
        public void AuthoredCatalog_EveryRoleDeclaresAKnownDisciplineGate()
        {
            var system = new SurvivorRoleSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "survivor_roles.json")));

            var defs = system.GetAllRoleDefs();
            Assert.Equal(8, defs.Count);
            foreach (var def in defs)
            {
                Assert.Contains(def.required_discipline, ValidDisciplines);
                Assert.InRange(def.required_discipline_level, 1f, 100f);
            }
        }

        // ── Earned practice: exactly-once per fact, invalid facts rejected ──

        [Fact]
        public void PracticeFact_AdvancesXpExactlyOnce_PerFact()
        {
            var system = new SurvivorRoleSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "survivor_roles.json")));
            system.AssignRole("surv_doc", "role_medic", force: true);

            // Simulate the shared skill owner firing OnXpGained once per fact.
            int matchingFacts = 0;
            var handler = new Action<SkillActor, string, float>((actor, discipline, _) =>
            {
                var assignment = system.GetRoleAssignment(actor.Id);
                var def = system.GetRoleDef(assignment?.RoleId ?? string.Empty);
                if (assignment == null || def == null) return;
                if (!string.Equals(def.required_discipline, discipline, StringComparison.OrdinalIgnoreCase)) return;
                int before = assignment.ExperiencePoints;
                system.AddRoleXp(actor.Id, 10); // return value signals level-up only; XP accrues regardless
                if (assignment.ExperiencePoints > before) matchingFacts++;
            });

            var actor = new SimpleSkillActor("surv_doc");
            handler(actor, "medical", 5f);   // matching fact → +10
            handler(actor, "medical", 5f);   // matching fact → +10
            handler(actor, "crafting", 5f);  // non-matching discipline → no award

            var assignment = system.GetRoleAssignment("surv_doc");
            Assert.NotNull(assignment);
            Assert.Equal(2, matchingFacts);
            Assert.Equal(20, assignment.ExperiencePoints);
            Assert.Equal(1, assignment.Level);
        }

        [Fact]
        public void SaveRestore_PreservesRoleIdentityLevelAndXp_ThroughSessionState()
        {
            var system = new SurvivorRoleSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "survivor_roles.json")));
            system.AssignRole("surv_leader", "role_leader", force: true, day: 5);
            system.AddRoleXp("surv_leader", 260);

            var state = system.CaptureState();
            var restored = new SurvivorRoleSystem();
            restored.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "survivor_roles.json")));
            restored.RestoreState(state);

            var a = restored.GetRoleAssignment("surv_leader");
            Assert.NotNull(a);
            Assert.Equal("role_leader", a.RoleId);
            Assert.Equal(3, a.Level);
            Assert.Equal(260, a.ExperiencePoints);
        }

        // ── Production wiring gate (source-text evidence) ──

        [Fact]
        public void HostWiring_BindsProducerSaveLifecycleDispatchRegistryAndPanel()
        {
            string mainRoles = Source("src/Main.SurvivorRoles.cs");
            string orchestrator = Source("src/Main.SaveOrchestrator.cs");
            string lifecycle = Source("src/Main.Lifecycle.cs");
            string application = Source("src/Main.Application.cs");
            string registry = Source("Assets/Ashfall.Core/HostCliRegistry.cs");
            string sections = Source("Assets/Ashfall.Core/Save/SaveSectionRegistry.cs");
            string hostSession = Source("src/Host/SurvivorRoleHostSession.cs");
            string cliProbe = Source("src/Host/HostCli.SurvivorRoles.cs");
            string panel = Source("src/UI/SurvivorDetailPanel.cs");
            string surfaces = Source("src/Main.PlayerSurfaces.cs");

            // Named producer: the shared SkillProgression authority's verified work facts.
            Assert.Contains("EnsureSharedSkillProgression()", mainRoles);
            Assert.Contains("OnXpGained", mainRoles);
            Assert.Contains("required_discipline", mainRoles);

            // Save custody through the existing orchestrator seam; no parallel store.
            Assert.Contains("SetupSurvivorRoles();", orchestrator);
            Assert.Contains("SaveSurvivorRoles();", orchestrator);
            Assert.Contains("ResetSurvivorRoles();", lifecycle);
            Assert.Contains("SurvivorRoleSaveStore.TrySave", hostSession);
            Assert.Contains("SaveStoreHub.Checksummed<SurvivorRoleState>", hostSession);

            // CLI probe registration and dispatch.
            Assert.Contains("SurvivorRolesSelfTest", registry);
            Assert.Contains("case HostCliAction.SurvivorRolesSelfTest:", application);
            Assert.Contains("HostCliSurvivorRoles.RunSelfTest", application);

            // Save section registration.
            Assert.Contains("new(\"survivor_roles\", \"SaveSurvivorRoles\", \"SetupSurvivorRoles\"", sections);

            // Read-only UI projection through the existing survivor detail route.
            Assert.Contains("RoleProvider", panel);
            Assert.Contains("_survivorDetailPanel.RoleProvider = id => GetSurvivorRoleReadout(id);", surfaces);
        }

        [Fact]
        public void AutoActions_AreNotAdvertisedAsOperational()
        {
            // TriggerAutoAction is bookkeeping only; neither the host session nor
            // the CLI probe may expose it as an operational command.
            string hostSession = Source("src/Host/SurvivorRoleHostSession.cs");
            string cliProbe = Source("src/Host/HostCli.SurvivorRoles.cs");
            Assert.DoesNotContain("TriggerAutoAction", hostSession);
            Assert.DoesNotContain("TriggerAutoAction", cliProbe);
        }
    }
}
