// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Shelter;

namespace Ashfall.Core
{
    /// <summary>
    /// B5–B8 expansion (§27 machine-identity/condition feedback): a read-only
    /// condition projection across the shelter's known condition-bearing
    /// machines (§17.4 — a player should understand a degraded subsystem
    /// without opening four panels). The report owns nothing: every condition
    /// value is read live from its owning system, and every entry is a
    /// projection of already-persisted state (no RNG, no mutation).
    /// </summary>
    public static class ShelterMachineryReport
    {
        public enum Band { Healthy, Worn, Critical, Offline }

        public sealed class MachineryStatus
        {
            public string MachineId = string.Empty;   // stable machine identity
            public string Label = string.Empty;
            public string Detail = string.Empty;
            public float Condition01;                // 0..1; 0 = spent/dead
            public Band StatusBand;
        }

        public static Band BandFor(float condition01) => condition01 switch
        {
            <= 0f => Band.Offline,
            < 0.3f => Band.Critical,
            < 0.6f => Band.Worn,
            _ => Band.Healthy
        };

        /// <summary>Condition labels for the briefing/UI projection.</summary>
        public static string LabelFor(Band band) => band switch
        {
            Band.Healthy => "healthy",
            Band.Worn => "worn",
            Band.Critical => "critical",
            _ => "offline"
        };

        /// <summary>
        /// The shelter's condition-bearing machines, in stable identity order.
        /// Absent systems contribute nothing — the report never fabricates a
        /// machine the shelter does not own.
        /// </summary>
        public static IReadOnlyList<MachineryStatus> Build(
            PowerGridSystem? grid,
            DeepWellSystem? well,
            AtmosphericCondenserSystem? condenser,
            WaterTreatmentSystem? water,
            SumpFloodingSystem? sump)
        {
            var report = new List<MachineryStatus>();

            if (grid != null)
            {
                float cond = grid.GeneratorCondition / 100f;
                report.Add(new MachineryStatus
                {
                    MachineId = "power.generator",
                    Label = "Base Generator",
                    Detail = $"{grid.GeneratorCondition:0}% condition" +
                             (grid.GeneratorCondition < PowerGridSystem.GeneratorDegradationThreshold
                                 ? " — output derated" : ""),
                    Condition01 = cond,
                    StatusBand = BandFor(cond)
                });
            }

            if (well != null && well.IsBuilt)
            {
                float cond = well.Condition / 100f;
                report.Add(new MachineryStatus
                {
                    MachineId = "deepwell.pump",
                    Label = "Deep-Well Pump",
                    Detail = $"{well.Condition:0}% condition · {well.TotalYieldLiters} L lifetime",
                    Condition01 = cond,
                    StatusBand = BandFor(cond)
                });
            }

            if (condenser != null && condenser.IsBuilt)
            {
                float cond = condenser.MembraneIntegrity / 100f;
                report.Add(new MachineryStatus
                {
                    MachineId = "water_condenser.membrane",
                    Label = "Condenser Membrane",
                    Detail = condenser.MembraneIntegrity <= 0f
                        ? "spent — replace with a desalination membrane"
                        : $"{condenser.MembraneIntegrity:0}% integrity",
                    Condition01 = cond,
                    StatusBand = BandFor(cond)
                });
            }

            if (water != null)
            {
                float integrity = water.State.filterIntegrity / 100f;
                report.Add(new MachineryStatus
                {
                    MachineId = "water.filter",
                    Label = "Treatment Filter",
                    Detail = $"{water.State.filterIntegrity:0}% integrity" +
                             (water.State.filterIntegrity <= 0f ? " — replace (1x water_filter)" : ""),
                    Condition01 = integrity,
                    StatusBand = BandFor(integrity)
                });
            }

            if (sump != null)
            {
                for (int i = 0; i < sump.State.nodes.Count; i++)
                {
                    var node = sump.State.nodes[i];
                    if (node == null || !node.hasSumpPump) continue;
                    float cond = node.pumpCondition / 100f;
                    report.Add(new MachineryStatus
                    {
                        MachineId = $"sump.pump.{node.nodeId}",
                        Label = $"Sump Pump ({node.displayName})",
                        Detail = $"{node.pumpCondition:0}% condition" +
                                 (node.pumpCondition <= 0 ? " — failed; repair at the node" : ""),
                        Condition01 = cond,
                        StatusBand = BandFor(cond)
                    });
                }
            }

            return report;
        }
    }
}
