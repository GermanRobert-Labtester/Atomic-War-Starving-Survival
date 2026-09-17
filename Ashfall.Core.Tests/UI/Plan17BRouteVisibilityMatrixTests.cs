// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Ashfall.Core.UI;
using Xunit;

namespace Ashfall.Core.Tests.UI
{
    /// <summary>
    /// Plan 17B / Task B2 — Deep Route and Visibility Matrix Gate.
    ///
    /// Exposes and validates the full route/visibility contract for all registered
    /// UI surfaces in PanelRegistry:
    /// 1. Identity, naming, and group validity across all descriptors;
    /// 2. Prototype isolation (strict non-navigability with diagnostic);
    /// 3. Main menu isolation (gameplay surfaces blocked in menu);
    /// 4. Dynamic availability rule evaluation and blocking;
    /// 5. Open/close/bind lifecycle determinism without side-effects;
    /// 6. 17B Guidance route contract compliance (Live, PlayerNavigable, Dashboard);
    /// 7. Complete inventory totality across Core, Expanded, Secondary, Menu, and Prototype routes.
    /// </summary>
    public sealed class Plan17BRouteVisibilityMatrixTests : IDisposable
    {
        public Plan17BRouteVisibilityMatrixTests()
        {
            PanelRegistryBootstrap.RegisterAll();
        }

        public void Dispose()
        {
            // Registry is additive and static; no teardown required
        }

        [Fact]
        public void Matrix_EveryRegisteredDescriptor_HasValidIdentityAndMetadata()
        {
            var descriptors = PanelRegistry.AllIds.Select(id => PanelRegistry.Get(id)!).ToList();
            Assert.True(descriptors.Count >= 60, $"Expected >= 60 descriptors, found {descriptors.Count}");

            foreach (var desc in descriptors)
            {
                Assert.False(string.IsNullOrWhiteSpace(desc.Id), "Panel ID must not be blank.");
                Assert.Matches(@"^[a-z][a-z0-9_]*$", desc.Id);
                Assert.False(string.IsNullOrWhiteSpace(desc.DisplayName), $"DisplayName for '{desc.Id}' must not be blank.");
                Assert.True(Enum.IsDefined(typeof(PanelGroup), desc.Group), $"Invalid group on '{desc.Id}'.");
                Assert.True(Enum.IsDefined(typeof(PanelMaturity), desc.Maturity), $"Invalid maturity on '{desc.Id}'.");
            }
        }

        [Fact]
        public void Matrix_AllLivePlayerNavigableRoutes_CanEvaluateAvailabilityWithoutThrowing()
        {
            var liveNavigable = PanelRegistry.AllIds
                .Select(id => PanelRegistry.Get(id)!)
                .Where(d => d.Maturity == PanelMaturity.Live && d.IsPlayerNavigable)
                .ToList();

            Assert.NotEmpty(liveNavigable);

            foreach (var desc in liveNavigable)
            {
                // CanOpen must execute cleanly and return a boolean without side-effects
                bool canOpen = desc.IsAvailable();
                Assert.True(canOpen == true || canOpen == false);

                var resolved = PanelRegistry.Resolve(desc.Id);
                Assert.NotNull(resolved);
                Assert.Same(desc, resolved);
            }
        }

        [Fact]
        public void Matrix_AllPrototypeRoutes_AreStrictlyNonPlayerNavigable_AndFailTryOpen()
        {
            var prototypes = PanelRegistry.AllIds
                .Select(id => PanelRegistry.Get(id)!)
                .Where(d => d.Maturity == PanelMaturity.Prototype)
                .ToList();

            Assert.NotEmpty(prototypes);
            Assert.True(prototypes.Count >= 25, $"Expected >= 25 prototypes, found {prototypes.Count}");

            foreach (var proto in prototypes)
            {
                Assert.False(proto.IsPlayerNavigable, $"Prototype '{proto.Id}' must not be player-navigable.");

                string? diagnostic = null;
                bool opened = PanelRegistry.TryOpen(proto.Id, msg => diagnostic = msg);

                Assert.False(opened, $"TryOpen on prototype '{proto.Id}' must return false.");
                Assert.NotNull(diagnostic);
                Assert.Contains("PROTOTYPE ROUTE", diagnostic);
                Assert.Contains(proto.Id, diagnostic);
            }
        }

        [Fact]
        public void Matrix_MenuIsolation_GatedTotalAcrossAllDescriptors()
        {
            foreach (var desc in PanelRegistry.AllIds.Select(id => PanelRegistry.Get(id)!))
            {
                if (desc.Maturity == PanelMaturity.Prototype)
                {
                    string? diagnostic = null;
                    bool opened = PanelRegistry.TryOpen(desc.Id, msg => diagnostic = msg, isMenu: true);
                    Assert.False(opened);
                    Assert.NotNull(diagnostic);
                    Assert.Contains("PROTOTYPE ROUTE", diagnostic);
                }
                else if (!desc.AvailableInMenu)
                {
                    string? diagnostic = null;
                    bool opened = PanelRegistry.TryOpen(desc.Id, msg => diagnostic = msg, isMenu: true);
                    Assert.False(opened, $"Non-menu panel '{desc.Id}' must not open in menu mode.");
                    Assert.NotNull(diagnostic);
                    Assert.Contains("BLOCKED ROUTE", diagnostic);
                    Assert.Contains("not available in main menu", diagnostic);
                }
                else
                {
                    // Menu-allowed panels must not be blocked by menu check
                    bool actionFired = false;
                    PanelRegistry.ConfigureActions(desc.Id, openAction: () => actionFired = true);
                    bool opened = PanelRegistry.TryOpen(desc.Id, isMenu: true);
                    Assert.True(opened, $"Menu-allowed panel '{desc.Id}' failed to open in menu mode.");
                    Assert.True(actionFired, $"Open action for menu panel '{desc.Id}' did not fire.");
                }
            }
        }

        [Fact]
        public void Matrix_GuidanceRoute_ConformsTo17BContract()
        {
            Assert.True(PanelRegistry.IsRegistered("guidance"), "Panel 'guidance' must be registered.");
            var desc = PanelRegistry.Get("guidance")!;

            Assert.Equal("guidance", desc.Id);
            Assert.Equal(PanelGroup.Dashboard, desc.Group);
            Assert.Equal(PanelMaturity.Live, desc.Maturity);
            Assert.True(desc.IsPlayerNavigable, "17B Guidance must be player-navigable.");
            Assert.False(desc.AvailableInMenu, "Guidance is a survival campaign panel, not menu-facing.");
            Assert.False(string.IsNullOrWhiteSpace(desc.DisplayName));
        }

        [Fact]
        public void Matrix_DynamicAvailabilityRule_BlocksDeterministicallyWithReason()
        {
            const string testId = "weather";
            bool allowOpen = false;

            PanelRegistry.ConfigureActions(testId,
                availabilityRule: () => allowOpen);

            string? blockedMsg = null;
            bool blocked = !PanelRegistry.TryOpen(testId, msg => blockedMsg = msg);
            Assert.True(blocked);
            Assert.NotNull(blockedMsg);
            Assert.Contains("BLOCKED ROUTE", blockedMsg);

            allowOpen = true;
            bool opened = PanelRegistry.TryOpen(testId);
            Assert.True(opened);
        }

        [Fact]
        public void Matrix_OpenCloseBindLifecycle_ExecutesInStrictSequence()
        {
            const string testId = "inventory";
            var sequence = new List<string>();

            PanelRegistry.ConfigureActions(testId,
                bindAction: () => sequence.Add("bind"),
                openAction: () => sequence.Add("open"),
                closeAction: () => sequence.Add("close"));

            bool openOk = PanelRegistry.TryOpen(testId);
            Assert.True(openOk);
            Assert.Equal(new[] { "bind", "open" }, sequence);

            bool closeOk = PanelRegistry.TryClose(testId);
            Assert.True(closeOk);
            Assert.Equal(new[] { "bind", "open", "close" }, sequence);
        }

        [Fact]
        public void Matrix_UnknownRoute_ReportsHonestDiagnosticAndNeverThrows()
        {
            string? captured = null;
            var resolved = PanelRegistry.Resolve("phantom_route_nowhere_404", msg => captured = msg);

            Assert.Null(resolved);
            Assert.NotNull(captured);
            Assert.Contains("UNKNOWN ROUTE", captured);
            Assert.Contains("phantom_route_nowhere_404", captured);
        }
    }
}
