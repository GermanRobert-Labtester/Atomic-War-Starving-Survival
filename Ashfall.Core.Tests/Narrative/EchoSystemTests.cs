// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests.Narrative
{
    public sealed class EchoSystemTests
    {
        [Fact]
        public void DayAndFlagBoundariesControlAvailability()
        {
            var catalog = LoadCatalog();
            var system = new EchoSystem(catalog);
            system.HasWorldFlag = _ => false;
            var gated = system.Find("echo_answering_machine");
            Assert.NotNull(gated);

            Assert.False(system.IsAvailable(gated!, 30));
            Assert.False(system.IsAvailable(gated!, 31));
            system.HasWorldFlag = _ => true;
            Assert.True(system.IsAvailable(gated!, 31));

            var selected = system.SelectForDay(31, new SeededRng(18));
            Assert.NotNull(selected);
            Assert.StartsWith("echo_", selected!.Id, StringComparison.Ordinal);
        }

        [Fact]
        public void SameSeedProducesTheSameSelection()
        {
            var catalog = LoadCatalog();
            var first = new EchoSystem(catalog) { HasWorldFlag = _ => true };
            var second = new EchoSystem(catalog) { HasWorldFlag = _ => true };

            var firstSelected = first.SelectForDay(31, new SeededRng(77));
            var secondSelected = second.SelectForDay(31, new SeededRng(77));

            Assert.NotNull(firstSelected);
            Assert.Equal(firstSelected!.Id, secondSelected?.Id);
        }

        [Fact]
        public void ResolutionIsExactlyOnceAndRestoreDoesNotReplay()
        {
            var system = new EchoSystem(LoadCatalog()) { HasWorldFlag = _ => true };
            var surfaced = system.SelectForDay(31, new SeededRng(9));
            Assert.NotNull(surfaced);

            var choice = surfaced!.Choices[0];
            var resolved = system.Resolve(surfaced.Id, choice.ChoiceId, 31);
            Assert.Equal(EchoResolutionStatus.Committed, resolved.Status);
            Assert.True(system.IsResolved(surfaced.Id));
            Assert.False(system.HasPendingEcho);

            var again = system.Resolve(surfaced.Id, choice.ChoiceId, 31);
            Assert.Equal(EchoResolutionStatus.AlreadyResolved, again.Status);

            var restored = new EchoSystem(LoadCatalog()) { HasWorldFlag = _ => true };
            restored.RestoreState(system.CaptureState());
            Assert.True(restored.IsResolved(surfaced.Id));
            Assert.False(restored.HasPendingEcho);
            var replay = restored.Resolve(surfaced.Id, choice.ChoiceId, 31);
            Assert.Equal(EchoResolutionStatus.AlreadyResolved, replay.Status);
        }

        [Fact]
        public void DelayedConsequenceSurvivesRestoreAndTicksExactlyOnce()
        {
            var system = new EchoSystem(LoadCatalog()) { HasWorldFlag = _ => true };
            system.RestoreState(new EchoState
            {
                PendingEchoId = "echo_childs_coat",
                PendingDay = 31
            });

            var resolved = system.Resolve("echo_childs_coat", "bury_the_coat", 31);
            Assert.Equal(EchoResolutionStatus.Committed, resolved.Status);
            Assert.NotNull(resolved.Choice?.DelayedConsequence);
            Assert.Single(system.State.PendingConsequences);
            Assert.Equal(32, system.State.PendingConsequences[0].DueDay);

            var restored = new EchoSystem(LoadCatalog());
            restored.RestoreState(system.CaptureState());
            Assert.Empty(restored.TickDay(31));

            var due = restored.TickDay(32);
            var consequence = Assert.Single(due);
            Assert.Equal("echo_childs_coat", consequence.EchoId);
            Assert.Equal("bury_the_coat", consequence.ChoiceId);
            Assert.Equal("warmth", consequence.Consequence.Effects[0].TargetNeed);
            Assert.Empty(restored.TickDay(32));
            Assert.Empty(restored.State.PendingConsequences);
        }

        private static List<EchoDefinition> LoadCatalog()
        {
            string path = AppContext.BaseDirectory;
            while (!string.IsNullOrEmpty(path))
            {
                string candidate = System.IO.Path.Combine(path, "Assets", "StreamingAssets", "Data");
                if (System.IO.Directory.Exists(candidate))
                    return EchoCatalogLoader.Load(candidate, new FileSystemIO(), new SystemTextJsonSerializer());
                path = System.IO.Directory.GetParent(path)?.FullName ?? string.Empty;
            }
            throw new InvalidOperationException("could not locate Assets/StreamingAssets/Data");
        }
    }
}
