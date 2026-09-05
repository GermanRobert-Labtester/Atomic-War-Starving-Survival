using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.IO;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.Expeditions
{
    /// <summary>
    /// GAP-48B — force passage through weather gates. A gate whose data
    /// carries `force_stamina_cost` can be forced: the sortie starts
    /// stamina-short by that cost (ExpeditionSystem.Start startingStamina,
    /// clamped to [0, MaxStamina]). Gates without a cost cannot be forced.
    /// </summary>
    public sealed class ForcePassageTests : IDisposable
    {
        private const string DataDir = "./assets-gap48b";
        private readonly IFileIO _fileIO = new FileSystemIO();

        public ForcePassageTests()
        {
            Directory.CreateDirectory(DataDir);
        }

        public void Dispose()
        {
            if (Directory.Exists(DataDir)) Directory.Delete(DataDir, recursive: true);
        }

        private static ExpeditionDefinition Dest(string id, int danger = 5) => new ExpeditionDefinition
        {
            id = id,
            displayName = id,
            distanceTicks = 2,
            dangerLevel = danger,
            encounterChancePerTick = 0.1f,
            baseStaminaDrainPerHour = 2f
        };

        private static ScavengingTableCatalog EmptyTables() => new ScavengingTableCatalog(new List<ScavengingTableDef>());

        [Fact]
        public void Start_StartingStamina_IsAppliedAndClamped()
        {
            var sys = new ExpeditionSystem();
            var dest = Dest("loc_x");
            var rng = new SeededRng(1);

            Assert.True(sys.Start(dest, "sv", 1, startingStamina: 70f));
            Assert.Equal(70f, sys.Active["sv"].stamina);

            // clamp low
            var sys2 = new ExpeditionSystem();
            Assert.True(sys2.Start(dest, "sv2", 1, startingStamina: -5f));
            Assert.Equal(0f, sys2.Active["sv2"].stamina);

            // clamp high
            var sys3 = new ExpeditionSystem();
            Assert.True(sys3.Start(dest, "sv3", 1, startingStamina: 999f));
            Assert.Equal(ExpeditionSystem.MaxStamina, sys3.Active["sv3"].stamina);

            // default unchanged
            var sys4 = new ExpeditionSystem();
            Assert.True(sys4.Start(dest, "sv4", 1));
            Assert.Equal(ExpeditionSystem.MaxStamina, sys4.Active["sv4"].stamina);
        }

        [Fact]
        public void ForceConsequence_Lookup_RequiresBlockAndPositiveCost()
        {
            var gates = new WeatherRouteGateCatalog(new List<WeatherGateDef>
            {
                new WeatherGateDef
                {
                    id = "g_force", gate_type = "destination", target = "loc_forceable",
                    blocked_weather = new List<string> { "Blizzard" },
                    force_stamina_cost = 30f,
                    override_item = "gas_mask",
                    consequence_on_force = "You arrive chilled to the bone."
                },
                new WeatherGateDef
                {
                    id = "g_noforce", gate_type = "destination", target = "loc_absolute",
                    blocked_weather = new List<string> { "Blizzard" },
                    force_stamina_cost = 0f
                }
            });

            // forceable gate: blocked under blizzard, carries the cost
            var block = gates.EvaluateBlock("loc_forceable", "Blizzard", null);
            Assert.NotNull(block);
            Assert.Equal(30f, block!.ForceStaminaCost);

            // passable under clear → no block at all
            Assert.Null(gates.EvaluateBlock("loc_forceable", "Clear", null));

            // zero-cost gate: blocked but cannot be forced
            var noForce = gates.EvaluateBlock("loc_absolute", "Blizzard", null);
            Assert.NotNull(noForce);
            Assert.Equal(0f, noForce!.ForceStaminaCost);

            // override still lifts the block entirely (no cost applied)
            Assert.Null(gates.EvaluateBlock("loc_forceable", "Blizzard", item => item == "gas_mask"));
        }

        [Fact]
        public void AuthoredDestinationGates_CarryForceCosts()
        {
            var gates = WeatherRouteGateCatalog.LoadFromDirectory(
                Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data"),
                new FileSystemIO());
            if (!Directory.Exists(Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data")))
            {
                gates = WeatherRouteGateCatalog.LoadFromDirectory(
                    Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data"),
                    new FileSystemIO());
            }

            foreach (var target in new[]
                     {
                         "location_silent_observatory",
                         "location_flooded_subway_depot",
                         "loc_the_shallows_market"
                     })
            {
                Assert.True(gates.TryGetGatesForTarget(target, out var list));
                var gate = list.Single();
                Assert.True(gate.force_stamina_cost > 0f, $"{gate.id} must be forceable");
                Assert.False(string.IsNullOrEmpty(gate.consequence_on_force));
            }

            // radiological gates carry a rad dose on force; the cold gate does not
            var blizzard = gates.EvaluateBlock("location_silent_observatory", "Blizzard", null);
            Assert.NotNull(blizzard);
            Assert.Equal(0f, blizzard!.ForceRadDose);

            var blackRain = gates.EvaluateBlock("location_flooded_subway_depot", "BlackRain", null);
            Assert.NotNull(blackRain);
            Assert.True(blackRain!.ForceRadDose > 0f, "black-rain gate must dose on force");

            var fallout = gates.EvaluateBlock("loc_the_shallows_market", "FalloutStorm", null);
            Assert.NotNull(fallout);
            Assert.True(fallout!.ForceRadDose > 0f, "fallout gate must dose on force");
        }
    }
}
