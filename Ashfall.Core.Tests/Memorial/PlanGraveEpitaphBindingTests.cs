// SPDX-License-Identifier: MIT
// Focused Core contract tests — Package H / Grave Epitaph Binding.
//
// MemorialSystem.SelectEpitaph's fallback rule was already written but read two
// properties nothing ever assigned (EpitaphCatalog, EpitaphRng), so every grave
// got an empty inscription and the authored wasteland_grave_epitaphs.json data
// had no consumer. These tests prove the authored table drives real memorials.
//
// Run: scripts/run_test.sh PlanGraveEpitaphBinding

using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Memorial;
using Xunit;
using Xunit.Abstractions;

namespace Ashfall.Core.Tests.Memorial
{
    public class PlanGraveEpitaphBindingTests
    {
        private readonly ITestOutputHelper _out;
        public PlanGraveEpitaphBindingTests(ITestOutputHelper output) => _out = output;

        private static string FindRepoRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null && !File.Exists(Path.Combine(dir.FullName, "AGENTS.md")))
                dir = dir.Parent;
            Assert.NotNull(dir);
            return dir!.FullName;
        }

        private static string DataDir() =>
            Path.Combine(FindRepoRoot(), "Assets", "StreamingAssets", "Data");

        private static GraveEpitaphCatalog Authored() =>
            GraveEpitaphCatalog.LoadFromDataDir(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());

        private static MemorialInput Input(string cause, int day = 100, string survivor = "dweller_test") =>
            new MemorialInput { SurvivorId = survivor, Cause = cause, Day = day, BirthDay = 0 };

        [Fact]
        public void UnboundMemorial_ProducesEmptyEpitaph_TheBugThisCloses()
        {
            var memorial = new MemorialSystem(new MemorialState());
            Assert.Null(memorial.EpitaphCatalog);
            Assert.Null(memorial.EpitaphRng);

            var entry = memorial.Memorialize(Input("radiosis"));
            Assert.Equal(string.Empty, entry.Epitaph);
        }

        [Fact]
        public void BoundMemorial_InscribesAnAuthoredEpitaph()
        {
            var memorial = new MemorialSystem(new MemorialState());
            memorial.EpitaphCatalog = Authored();
            memorial.EpitaphRng = new SeededRng(2026);

            var entry = memorial.Memorialize(Input("radiosis"));

            Assert.False(string.IsNullOrWhiteSpace(entry.Epitaph));
            // Never invents: the result must be one of the authored inscriptions.
            bool authored = false;
            foreach (var candidate in memorial.EpitaphCatalog.AllEntries)
                if (candidate.epitaph == entry.Epitaph) authored = true;
            Assert.True(authored, "selected epitaph must come from the authored table");
            _out.WriteLine($"epitaph: {entry.Epitaph}");
        }

        [Fact]
        public void Selection_IsDeterministicForAFixedSeed()
        {
            var cause = Authored().AllEntries[0].cause;

            var a = new MemorialSystem(new MemorialState());
            a.EpitaphCatalog = Authored();
            a.EpitaphRng = new SeededRng(7);
            var b = new MemorialSystem(new MemorialState());
            b.EpitaphCatalog = Authored();
            b.EpitaphRng = new SeededRng(7);

            Assert.Equal(
                a.Memorialize(Input(cause, day: 50, survivor: "same_dweller")).Epitaph,
                b.Memorialize(Input(cause, day: 50, survivor: "same_dweller")).Epitaph);
        }

        [Fact]
        public void UnknownCause_StillResolvesToAuthoredText()
        {
            var memorial = new MemorialSystem(new MemorialState());
            memorial.EpitaphCatalog = Authored();
            memorial.EpitaphRng = new SeededRng(11);

            var entry = memorial.Memorialize(Input("cause_that_was_never_authored"));
            Assert.False(string.IsNullOrWhiteSpace(entry.Epitaph));
        }

        [Fact]
        public void ExplicitEpitaph_WinsOverCatalogSelection()
        {
            var memorial = new MemorialSystem(new MemorialState());
            memorial.EpitaphCatalog = Authored();
            memorial.EpitaphRng = new SeededRng(3);

            var input = Input("radiosis");
            input.Epitaph = "hand-authored line";
            var entry = memorial.Memorialize(input);

            Assert.Equal("hand-authored line", entry.Epitaph);
        }

        [Fact]
        public void CatalogRemainsTheSoleEpitaphAuthority()
        {
            var catalog = Authored();
            var memorial = new MemorialSystem(new MemorialState());
            memorial.EpitaphCatalog = catalog;
            memorial.EpitaphRng = new SeededRng(5);

            memorial.Memorialize(Input("radiosis"));
            memorial.Memorialize(Input("thirst"));

            // The catalog is never grown or rewritten by gameplay.
            Assert.True(ReferenceEquals(catalog, memorial.EpitaphCatalog));
            Assert.Equal(catalog.TotalCount, Authored().TotalCount);
        }

    }
}
