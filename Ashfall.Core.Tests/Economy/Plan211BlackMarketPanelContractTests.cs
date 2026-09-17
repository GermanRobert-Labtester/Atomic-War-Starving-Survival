// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Xunit;

namespace Ashfall.Core.Tests.Economy
{
    public sealed class Plan211BlackMarketPanelContractTests
    {
        private static string RepoFile(string relative)
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string candidate = Path.Combine(dir.FullName, relative);
                if (File.Exists(candidate)) return candidate;
                dir = dir.Parent;
            }
            throw new FileNotFoundException(relative);
        }

        [Fact]
        public void Panel_RoutesAllActionsThroughHost_AndDoesNotMutateOwners()
        {
            string source = File.ReadAllText(RepoFile("src/UI/BlackMarketPanel.cs"));
            Assert.Contains("_host?.Buy(", source, StringComparison.Ordinal);
            Assert.Contains("_host?.Sell(", source, StringComparison.Ordinal);
            Assert.Contains("_host?.TakeLoan(", source, StringComparison.Ordinal);
            Assert.Contains("_host?.Repay(", source, StringComparison.Ordinal);
            Assert.DoesNotContain(".Inventory.", source, StringComparison.Ordinal);
            Assert.DoesNotContain("TryDebitValue", source, StringComparison.Ordinal);
            Assert.DoesNotContain("TryCreditValue", source, StringComparison.Ordinal);
        }

        [Fact]
        public void PanelRefresh_CannotAdvanceOrRerollMarket()
        {
            string source = File.ReadAllText(RepoFile("src/UI/BlackMarketPanel.cs"));
            Assert.DoesNotContain("EnsureStockSnapshot", source, StringComparison.Ordinal);
            Assert.DoesNotContain("TickDay", source, StringComparison.Ordinal);
            Assert.DoesNotContain("TickDaily", source, StringComparison.Ordinal);
            Assert.DoesNotContain("ISeededRng", source, StringComparison.Ordinal);
        }

        [Fact]
        public void PanelBindLifecycle_UnsubscribesStateAndActionEvents()
        {
            string source = File.ReadAllText(RepoFile("src/UI/BlackMarketPanel.cs"));
            Assert.Contains("_host.StateChanged += RefreshView", source, StringComparison.Ordinal);
            Assert.Contains("_host.StateChanged -= RefreshView", source, StringComparison.Ordinal);
            Assert.Contains("_host.ActionCompleted += OnActionCompleted", source, StringComparison.Ordinal);
            Assert.Contains("_host.ActionCompleted -= OnActionCompleted", source, StringComparison.Ordinal);
            Assert.Contains("public override void _ExitTree() => Unbind();", source, StringComparison.Ordinal);
        }
    }
}
