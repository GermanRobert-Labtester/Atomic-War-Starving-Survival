// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Ashfall.Core.Disease;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    /// <summary>Thin Godot adapter for shelter fluid topology and delivery.</summary>
    public sealed class FluidLogisticsHostSession : HostSessionBase
    {
        public FluidLogisticsSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;
        public FluidDeliveryApplicator.ApplicationReport? LastDelivery { get; private set; }

        public static FluidLogisticsHostSession Create(string dataDir, FluidLogisticsSystem? system = null)
        {
            var session = new FluidLogisticsHostSession(system ?? new FluidLogisticsSystem(new GodotLog()));
            if (!string.IsNullOrWhiteSpace(dataDir))
            {
                var catalog = FluidInfrastructureCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
                session.System.LoadCatalog(catalog);
                session.LastEvent = $"Fluid infrastructure catalog loaded: {catalog.Pipes.Count} pipes, {catalog.Pumps.Count} pumps";
            }
            return session;
        }

        public FluidLogisticsHostSession(FluidLogisticsSystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
            System.OnPipeBurst += edge =>
            {
                LastEvent = "Pipe burst: " + edge;
                RaiseStateChanged();
            };
            System.OnDistributionSolved += report =>
            {
                LastEvent = $"Fluid delivery @ day {report.day}: {report.deliveredVolume:F1} units";
                RaiseStateChanged();
            };
            System.OnStateChanged += () => RaiseStateChanged();
        }

        /// <summary>
        /// Plan 168 daily cadence: ensure topology → WT transfer → Tick/Solve → apply.
        /// Inventory bottle watering remains a separate packaging path.
        /// </summary>
        public FluidDistributionReport AdvanceDay(
            int day,
            float temperatureC,
            float powerAvailability01,
            WaterTreatmentSystem? treatment,
            GreenhouseSystem? greenhouse,
            DiseaseSystem? disease,
            Func<string?>? pickLivingSurvivorId)
        {
            System.EnsureDefaultShelterTopology();
            if (treatment != null)
            {
                var transfer = FluidDeliveryApplicator.TransferBoundedCleanWater(treatment, System);
                if (transfer.IsSuccess)
                    LastEvent = "Fluid transfer: " + transfer.MessageKey;
            }

            var report = System.Tick(day, temperatureC, powerAvailability01);
            LastDelivery = FluidDeliveryApplicator.Apply(
                System,
                greenhouse,
                disease,
                pickLivingSurvivorId,
                day);
            LastEvent = $"Fluid tick @ day {day}: {report.deliveredVolume:F1} units delivered"
                + (LastDelivery != null
                    ? $" (gh={LastDelivery.greenhouseLiters:F1}, drink={LastDelivery.drinkingLiters:F1})"
                    : string.Empty);
            RaiseStateChanged();
            return report;
        }

        public FluidDistributionReport AdvanceDay(int day, float temperatureC, float powerAvailability01 = 1f)
            => AdvanceDay(day, temperatureC, powerAvailability01, null, null, null, null);

        public FluidLogisticsState CaptureState() => System.CaptureState();
        public void RestoreState(FluidLogisticsState state) => System.RestoreState(state);
    }
}
