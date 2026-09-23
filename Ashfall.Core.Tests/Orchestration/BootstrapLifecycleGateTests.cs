// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core.Orchestration;
using Xunit;

namespace Ashfall.Core.Tests.Orchestration
{
    public sealed class BootstrapLifecycleGateTests
    {
        [Fact]
        public void AdvanceStage_SequentialProgression_Succeeds()
        {
            var gate = new BootstrapLifecycleGate();
            Assert.Equal(BootstrapStage.None, gate.CurrentStage);

            Assert.True(gate.AdvanceStage(BootstrapStage.Configuration));
            Assert.True(gate.AdvanceStage(BootstrapStage.Foundation));
            Assert.True(gate.AdvanceStage(BootstrapStage.DomainServices));
            Assert.True(gate.AdvanceStage(BootstrapStage.CampaignOwners));
            Assert.True(gate.AdvanceStage(BootstrapStage.UiSurfaces));
            Assert.True(gate.AdvanceStage(BootstrapStage.Ready));

            Assert.Equal(BootstrapStage.Ready, gate.CurrentStage);
        }

        [Fact]
        public void AdvanceStage_SkippingOrReversing_Fails()
        {
            var gate = new BootstrapLifecycleGate();

            // Cannot jump from None to DomainServices directly
            Assert.False(gate.AdvanceStage(BootstrapStage.DomainServices));
            Assert.Equal(BootstrapStage.None, gate.CurrentStage);

            // Advance correctly to Configuration
            Assert.True(gate.AdvanceStage(BootstrapStage.Configuration));

            // Cannot regress to None
            Assert.False(gate.AdvanceStage(BootstrapStage.None));
            // Cannot stay at same stage
            Assert.False(gate.AdvanceStage(BootstrapStage.Configuration));
        }

        [Fact]
        public void ValidateLifecycleParity_CleanReadyBootstrap_PassesWithoutViolations()
        {
            var gate = new BootstrapLifecycleGate();
            gate.RegisterSubsystem("config_loader", BootstrapStage.Configuration, isRequired: true);
            gate.RegisterSubsystem("save_store_hub", BootstrapStage.Foundation, isRequired: true);
            gate.RegisterSubsystem("needs_system", BootstrapStage.DomainServices, isRequired: true);
            gate.RegisterSubsystem("campaign_coordinator", BootstrapStage.CampaignOwners, isRequired: true);
            gate.RegisterSubsystem("panel_registry", BootstrapStage.UiSurfaces, isRequired: true);

            for (int stage = (int)BootstrapStage.Configuration; stage <= (int)BootstrapStage.Ready; stage++)
            {
                gate.AdvanceStage((BootstrapStage)stage);
            }

            Assert.True(gate.ValidateLifecycleParity(BootstrapPathMode.FreshGame, out var violationsFresh));
            Assert.Empty(violationsFresh);

            Assert.True(gate.ValidateLifecycleParity(BootstrapPathMode.SaveRestore, out var violationsRestore));
            Assert.Empty(violationsRestore);

            Assert.True(gate.ValidateLifecycleParity(BootstrapPathMode.SessionReset, out var violationsReset));
            Assert.Empty(violationsReset);
        }

        [Fact]
        public void ValidateLifecycleParity_WithDeferredSeams_ReportsViolation()
        {
            var gate = new BootstrapLifecycleGate();
            gate.RegisterSubsystem("unsealed_experimental_subsystem", BootstrapStage.DomainServices, isRequired: false, hasDeferredSeams: true);

            for (int stage = (int)BootstrapStage.Configuration; stage <= (int)BootstrapStage.Ready; stage++)
            {
                gate.AdvanceStage((BootstrapStage)stage);
            }

            Assert.False(gate.ValidateLifecycleParity(BootstrapPathMode.FreshGame, out var violations));
            Assert.Single(violations);
            Assert.Contains("deferred seams", violations[0]);
        }

        [Fact]
        public void ValidateLifecycleParity_NotReachingReady_ReportsViolation()
        {
            var gate = new BootstrapLifecycleGate();
            gate.RegisterSubsystem("config_loader", BootstrapStage.Configuration, isRequired: true);
            gate.AdvanceStage(BootstrapStage.Configuration);

            Assert.False(gate.ValidateLifecycleParity(BootstrapPathMode.SaveRestore, out var violations));
            Assert.Contains("Lifecycle not ready", violations[0]);
        }
    }
}
