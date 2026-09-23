// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 51 — Holdfast presentation slate host-integration gate.
//
// Pins the production wiring contract:
//   * the slate is a derived read model (no save section, no parallel state),
//   * each projection reads its named canonical owner,
//   * a missing owner leaves its projection empty instead of fabricating data,
//   * the refresh points are the canonical day advance, the accessibility
//     preference seam and campaign setup,
//   * the player surface (interior view tooltip) only shows non-nominal hazards,
//   * the probe is registered.
// ============================================================================
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Ashfall.Core.Tests.Presentation
{
    public sealed class Plan51HoldfastPresentationHostIntegrationTests
    {
        private static string RepoRoot()
        {
            string dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 8 && dir != null; i++)
            {
                if (File.Exists(Path.Combine(dir, "Ashfall.csproj")))
                    return dir;
                dir = Directory.GetParent(dir)?.FullName;
            }
            throw new DirectoryNotFoundException("repo root not found from test context");
        }

        private static string ReadRepoFile(params string[] parts)
            => File.ReadAllText(Path.Combine(new[] { RepoRoot() }.Concat(parts).ToArray()));

        [Fact]
        public void SlateIsADerivedReadModel_NoSaveSectionNoParallelState()
        {
            string session = ReadRepoFile("src", "Host", "HoldfastPresentationHostSession.cs");
            Assert.DoesNotContain("SaveStore", session);
            Assert.DoesNotContain("SaveSectionRegistry", session);

            string main = ReadRepoFile("src", "Main.HoldfastPresentation.cs");
            Assert.Contains("Derived read model: nothing is persisted", main);
            // Each projection source is the existing owner, not a copy.
            Assert.Contains("Assignments = _shelterAssignment", main);
            Assert.Contains("Power = _powerGrid?.System", main);
            Assert.Contains("Map = _world?.WastelandMap", main);
            Assert.Contains("Memorial = _memorial", main);
        }

        [Fact]
        public void Projections_ReadTheirNamedCanonicalOwners()
        {
            string session = ReadRepoFile("src", "Host", "HoldfastPresentationHostSession.cs");
            Assert.Contains("sources.Power?.IsRoomServed(", session);
            Assert.Contains("GetAssignmentsForRoom", session);
            Assert.Contains("currentTempC", session);
            Assert.Contains("node.isFlooded", session);
            Assert.Contains("DutyRosterIds.StatusLevy", session);
            Assert.Contains("MemorialSystem? Memorial", session);
            Assert.Contains("MapFogState.Visited", session);
        }

        [Fact]
        public void MissingOwners_LeaveProjectionsEmptyNotInvented()
        {
            string session = ReadRepoFile("src", "Host", "HoldfastPresentationHostSession.cs");
            // Room temperature defaults are explicit, not fabricated state.
            Assert.Contains("return NominalTemperature;", session);
            Assert.Contains("?? true", session);
            // No room flood is ever invented: only the sump owner's nodes flood.
            Assert.Contains("node.isFlooded", session);
            Assert.DoesNotContain("IsFlooding = true", session);
        }

        [Fact]
        public void RefreshPoints_AreTheCanonicalOwnerSeams()
        {
            string holdfast = ReadRepoFile("src", "Main.Holdfast.cs");
            Assert.Contains("RefreshHoldfastPresentation();", holdfast);

            string onboarding = ReadRepoFile("src", "Main.Onboarding.cs");
            Assert.Contains("RefreshHoldfastPresentation();", onboarding);

            string expanded = ReadRepoFile("src", "Main.ExpandedShelterSystems.cs");
            Assert.Contains("SetupPresentation();", expanded);
            Assert.Contains("ResetPresentation();", expanded);
        }

        [Fact]
        public void PlayerSurface_OnlyShowsNonNominalHazards()
        {
            string view = ReadRepoFile("src", "World", "HoldfastInteriorView.cs");
            Assert.Contains("AppendRoomPresentation", view);
            // Nominal rooms keep their original tooltip text.
            Assert.Contains("if (room.PrimaryHazard != Ashfall.Core.Presentation.RoomHazardVisualBand.Nominal)", view);

            string panel = ReadRepoFile("src", "UI", "ShelterPanel.cs");
            Assert.Contains("public void SetPresentationSlate(", panel);
        }

        [Fact]
        public void HostCliProbe_IsRegistered()
        {
            string cli = ReadRepoFile("src", "Host", "HostCli.cs");
            Assert.Contains("HoldfastPresentationSelfTest", cli);
            Assert.Contains("--holdfast-presentation-selftest", cli);
            string probe = ReadRepoFile("src", "Host", "HostCli.HoldfastPresentation.cs");
            Assert.Contains("public static int RunHoldfastPresentationSelfTest(", probe);
        }

        [Fact]
        public void SlateCoreContract_HazardAndCrisisBandsDeriveFromRooms()
        {
            var slate = new Ashfall.Core.Presentation.HoldfastPresentationSlate();
            slate.AddRoom(new Ashfall.Core.Presentation.RoomPresentationSnapshot("room_a", "A", true, false, 18f));
            Assert.Equal(Ashfall.Core.Presentation.ShelterVisualCrisisBand.Calm, slate.ReevaluateCrisisBand());

            slate.AddRoom(new Ashfall.Core.Presentation.RoomPresentationSnapshot("room_b", "B", false, false, 18f));
            Assert.Equal(Ashfall.Core.Presentation.ShelterVisualCrisisBand.Brownout, slate.ReevaluateCrisisBand());

            slate.AddRoom(new Ashfall.Core.Presentation.RoomPresentationSnapshot("room_c", "C", true, true, 18f));
            Assert.Equal(Ashfall.Core.Presentation.ShelterVisualCrisisBand.Flooding, slate.ReevaluateCrisisBand());

            var flooded = new Ashfall.Core.Presentation.HoldfastPresentationSlate();
            flooded.AddRoom(new Ashfall.Core.Presentation.RoomPresentationSnapshot("room_a", "A", true, true, 18f));
            flooded.AddRoom(new Ashfall.Core.Presentation.RoomPresentationSnapshot("room_b", "B", true, true, 18f));
            Assert.Equal(Ashfall.Core.Presentation.ShelterVisualCrisisBand.Crisis, flooded.ReevaluateCrisisBand());
        }
    }
}
