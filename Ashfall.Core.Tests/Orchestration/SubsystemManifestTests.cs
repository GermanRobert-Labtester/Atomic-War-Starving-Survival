// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Orchestration;
using Ashfall.Core.Save;
using Ashfall.Core.UI;
using Xunit;

namespace Ashfall.Core.Tests.Orchestration
{
    public sealed class SubsystemManifestTests
    {
        public SubsystemManifestTests()
        {
            // Ensure PanelRegistry is bootstrapped for cross-reference testing
            PanelRegistryBootstrap.RegisterAll();
        }

        [Fact]
        public void All_HasUniqueIds_AndValidFields()
        {
            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var descriptor in SubsystemManifest.All)
            {
                Assert.False(string.IsNullOrWhiteSpace(descriptor.Id), "Subsystem ID cannot be empty");
                Assert.True(seen.Add(descriptor.Id), $"Duplicate subsystem ID '{descriptor.Id}' in SubsystemManifest");
                Assert.False(string.IsNullOrWhiteSpace(descriptor.DisplayName), $"DisplayName cannot be empty for {descriptor.Id}");
                Assert.False(string.IsNullOrWhiteSpace(descriptor.Description), $"Description cannot be empty for {descriptor.Id}");
            }
        }

        [Fact]
        public void All_SaveSectionKeys_ExistInSaveSectionRegistry()
        {
            foreach (var descriptor in SubsystemManifest.All)
            {
                if (descriptor.SaveSectionKey != null)
                {
                    bool exists = SaveSectionRegistry.TryGetSection(descriptor.SaveSectionKey, out var metadata);
                    Assert.True(exists, $"Subsystem '{descriptor.Id}' declares SaveSectionKey '{descriptor.SaveSectionKey}' which does not exist in SaveSectionRegistry.");
                    Assert.NotNull(metadata);
                }
            }
        }

        [Fact]
        public void All_PrimaryPanelRoutes_ExistInPanelRegistry()
        {
            foreach (var descriptor in SubsystemManifest.All)
            {
                if (descriptor.PrimaryPanelRoute != null)
                {
                    bool exists = PanelRegistry.IsRegistered(descriptor.PrimaryPanelRoute);
                    Assert.True(exists, $"Subsystem '{descriptor.Id}' declares PrimaryPanelRoute '{descriptor.PrimaryPanelRoute}' which is not registered in PanelRegistry.");
                }
            }
        }

        [Fact]
        public void ForPhase_ReturnsExpectedSubsystems()
        {
            var bootstrap = SubsystemManifest.ForPhase(LifecyclePhase.Bootstrap);
            var core = SubsystemManifest.ForPhase(LifecyclePhase.CoreSimulation);
            var expansion = SubsystemManifest.ForPhase(LifecyclePhase.Expansion);

            Assert.NotEmpty(bootstrap);
            Assert.NotEmpty(core);
            Assert.NotEmpty(expansion);

            Assert.Contains(bootstrap, s => s.Id == "journal");
            Assert.Contains(core, s => s.Id == "needs");
            Assert.Contains(core, s => s.Id == "weather");
            Assert.Contains(expansion, s => s.Id == "greenhouse");
        }

        [Fact]
        public void Get_ThrowsOnUnknownId()
        {
            Assert.Throws<KeyNotFoundException>(() => SubsystemManifest.Get("nonexistent_subsystem_xyz"));
        }

        [Fact]
        public void TryGet_ReturnsFalseForUnknownOrEmpty()
        {
            Assert.False(SubsystemManifest.TryGet("invalid_id", out var d1));
            Assert.Null(d1);

            Assert.False(SubsystemManifest.TryGet("", out var d2));
            Assert.Null(d2);

            Assert.False(SubsystemManifest.TryGet(null!, out var d3));
            Assert.Null(d3);
        }

        [Fact]
        public void ExecuteSetup_InvokesConfiguredDelegates_AndReturnsCount()
        {
            bool invoked = false;
            SubsystemManifest.RegisterSetupAction("journal", () => invoked = true);
            int count = SubsystemManifest.ExecuteSetup(LifecyclePhase.Bootstrap);
            Assert.True(invoked, "Expected journal setup delegate to be invoked");
            Assert.True(count >= 1, "Expected at least 1 delegate executed");
        }
    }
}
