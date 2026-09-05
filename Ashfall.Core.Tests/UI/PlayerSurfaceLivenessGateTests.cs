// SPDX-License-Identifier: MIT
// ASHFALL CI Gate: Player Surface Liveness & Anti-Fabrication Gate (REM-003 / REM-006 / R08 / R10).
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Ashfall.Core.UI;
using Xunit;

namespace Ashfall.Core.Tests.UI
{
    public sealed class PlayerSurfaceLivenessGateTests
    {
        private static readonly string[] ShelvedPrototypeIds = new[]
        {
            "biogas_digester",
            "cartography_gis",
            "printing_press",
            "silicon_slicing",
            "geothermal_turbine",
            "war_dog_kennel",
            "isotope_separator",
            "plasma_smelting",
            "borehole_seismograph",
            "logistics_airlock",
            "cryo_permafrost_core",
            "basal_radon_migration",
            "trauma_bonding_cohort",
            "clandestine_insurgency",
            "surface_shrapnel_aegis",
            "long_walk_expedition",
            "sonic_rupture_drill",
            "vault_door_breaching",
            "iron_cenotaph_memorial",
            "aquifer_treaty_concession",
            "mechanical_prosthetics_lathe",
            "ultrasonic_decontam_airlock",
            "tropospheric_radio_relay",
            "induction_cupola_furnace",
            "heavy_marine_diesel_gen",
            "magnetic_drum_archive"
        };

        public PlayerSurfaceLivenessGateTests()
        {
            PanelRegistryBootstrap.RegisterAll();
        }

        [Fact]
        public void ShelvedPrototypes_AreRegisteredButNotPlayerNavigable()
        {
            foreach (var id in ShelvedPrototypeIds)
            {
                var descriptor = PanelRegistry.Get(id);
                Assert.NotNull(descriptor);
                Assert.Equal(PanelMaturity.Prototype, descriptor!.Maturity);
                Assert.False(descriptor.IsPlayerNavigable,
                    $"Shelved prototype '{id}' must not be player-navigable.");
                Assert.Null(descriptor.BindAction);
                Assert.Null(descriptor.OpenAction);
                Assert.Null(descriptor.CloseAction);
            }
        }

        [Fact]
        public void LiveSlurrySurface_RemainsLive()
        {
            var descriptor = PanelRegistry.Get("slurry_dewatering_sump");
            Assert.NotNull(descriptor);
            Assert.Equal(PanelMaturity.Live, descriptor!.Maturity);
        }

        private static readonly System.Collections.Generic.HashSet<string> StandaloneAutonomousPanels = new(StringComparer.OrdinalIgnoreCase)
        {
            "help", "save", "settings", "combat", "combat_detail", "combat_history"
        };

        [Fact]
        public void AllRegisteredPanels_HaveAuthoritativeBindingOrSetupDependencies()
        {
            foreach (var id in PanelRegistry.AllIds)
            {
                var desc = PanelRegistry.Get(id);
                Assert.NotNull(desc);

                bool hasSetupDeps = desc.SetupDependencies != null && desc.SetupDependencies.Length > 0;
                bool isAutonomous = desc.Group == PanelGroup.Expanded || desc.Group == PanelGroup.MainMenu || StandaloneAutonomousPanels.Contains(id);

                Assert.True(hasSetupDeps || isAutonomous,
                    $"Panel '{id}' in group '{desc.Group}' has no declared setup dependencies and is not an autonomous expanded/menu panel.");
            }
        }

        [Fact]
        public void PlayerSurfaces_DoesNotInstantiateFreshGameplaySystemsInRoutes()
        {
            string srcRoot = FindSrcRoot();
            string playerSurfacesPath = Path.Combine(srcRoot, "Main.PlayerSurfaces.cs");
            Assert.True(File.Exists(playerSurfacesPath), $"Could not find Main.PlayerSurfaces.cs at {playerSurfacesPath}");

            string content = File.ReadAllText(playerSurfacesPath);

            // Banned gameplay system constructions inside player routing
            string[] bannedSystems = new[]
            {
                "FactionStanceEngine",
                "ShelterFireHazardSystem",
                "SkillProgressionSystem",
                "NeedsSystem",
                "RadiationSystem",
                "WeatherSystem",
                "WeatherHostSession"
            };

            foreach (var sys in bannedSystems)
            {
                var match = Regex.Match(content, $@"new\s+(?:Ashfall\.Core\.(?:\w+\.)*)?{sys}\s*\(");
                Assert.False(match.Success,
                    $"Main.PlayerSurfaces.cs contains prohibited fresh gameplay system instantiation 'new {sys}()'. Routes must bind to campaign-owned state.");
            }
        }

        private static string FindSrcRoot()
        {
            string current = Directory.GetCurrentDirectory();
            while (!string.IsNullOrEmpty(current))
            {
                string candidate = Path.Combine(current, "src");
                if (Directory.Exists(candidate))
                    return candidate;
                string parent = Path.GetDirectoryName(current)!;
                if (parent == current) break;
                current = parent;
            }
            throw new DirectoryNotFoundException("Could not locate src/ directory from " + Directory.GetCurrentDirectory());
        }
    }
}
