// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests.ScratchProbe
{
    public sealed class PreFlightProbeTests
    {
        private static string RepoRoot()
        {
            var d = new DirectoryInfo(AppContext.BaseDirectory);
            while (d != null) { if (Directory.Exists(Path.Combine(d.FullName, "Assets", "Ashfall.Core"))) return d.FullName; d = d.Parent; }
            throw new DirectoryNotFoundException();
        }
        private static string Data => Path.Combine(RepoRoot(), "Assets", "StreamingAssets", "Data");

        [Fact]
        public void ProbePatrolValidatorAgainstAuthoredData()
        {
            var catalog = TravelEncounterCatalog.LoadFromDirectory(Data, new Ashfall.Core.FileSystemIO());
            Console.WriteLine($"PROBE encounters={catalog.Count}");
            var errs = PatrolEncounterValidator.Validate(catalog.Encounters);
            Console.WriteLine($"PROBE no-context errors={errs.Count}");
            foreach (var e in errs.Take(12)) Console.WriteLine("PROBE   | " + e);
        }

        [Fact]
        public void ProbeSurfaceManifest()
        {
            var m = Ashfall.Core.UI.PlayerSurfaceManifest.Generate();
            Console.WriteLine($"PROBE surfaces total={m.TotalSurfaces} routed={m.RoutedSurfaces} bound={m.BoundSurfaces} closeable={m.CloseableSurfaces} snap={m.SnapshotCoveredSurfaces} interactive={m.InteractiveActionSurfaces} readonly={m.ReadOnlySurfaces}");
        }
    }
}
